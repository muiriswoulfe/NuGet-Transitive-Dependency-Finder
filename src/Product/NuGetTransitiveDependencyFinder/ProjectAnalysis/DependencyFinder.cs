// <copyright file="DependencyFinder.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ProjectAnalysis;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using NuGet.ProjectModel;
using NuGetTransitiveDependencyFinder.Output;

/// <summary>
/// An internal class that manages the overall process of finding transitive NuGet dependencies, abstracting away
/// internal APIs from applications consuming the library.
/// </summary>
internal class DependencyFinder : IDependencyFinder
{
    /// <summary>
    /// The object representing the contents of a "project.assets.json" file.
    /// </summary>
    private readonly IAssets assets;

    /// <summary>
    /// The object representing a dependency graph of .NET projects and their NuGet dependencies.
    /// </summary>
    private readonly IDependencyGraph dependencyGraph;

    /// <summary>
    /// The collection of dependencies recorded and stored temporarily for the purposes of finding transitive NuGet
    /// dependencies.
    /// </summary>
    /// <remarks>The keys contain the dependency identifiers, while the values contain the complete dependency
    /// information.</remarks>
    private readonly IDictionary<string, Dependency> dependencies =
        new Dictionary<string, Dependency>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Initializes a new instance of the <see cref="DependencyFinder"/> class.
    /// </summary>
    /// <param name="assets">The object representing the contents of a "project.assets.json" file.</param>
    /// <param name="dependencyGraph">The object representing a dependency graph of .NET projects and their NuGet
    /// dependencies.</param>
    public DependencyFinder(IAssets assets, IDependencyGraph dependencyGraph)
    {
        this.assets = assets;
        this.dependencyGraph = dependencyGraph;
    }

    /// <inheritdoc/>
    public Projects Run(string projectOrSolutionPath, bool collateAllDependencies, Regex? filter)
    {
        var projects = this.CreateProjects(projectOrSolutionPath);
        var result = new Projects(projects.Count);
        foreach (var project in projects)
        {
            var assetsFile = this.CreateAssetsFile(project);
            if (assetsFile is null)
            {
                continue;
            }

            var resultProject = new Project(project.Name, project.TargetFrameworks.Count);
            foreach (var framework in project.TargetFrameworks)
            {
                this.dependencies.Clear();

                var libraries = assetsFile
                    .Targets
                    .First(target => target.TargetFramework == framework.FrameworkName)
                    .Libraries
                    .ToImmutableDictionary(library => library.Name, StringComparer.OrdinalIgnoreCase);

                var projectDependencyGroup = assetsFile
                    .ProjectFileDependencyGroups
                    .FirstOrDefault(target =>
                        target.FrameworkName == framework.TargetAlias ||
                        target.FrameworkName == framework.FrameworkName.DotNetFrameworkName);

                if (projectDependencyGroup is null)
                {
                    continue;
                }

                var projectDependencies = projectDependencyGroup
                    .Dependencies
                    .Select(dependency =>
                        libraries.TryGetValue(dependency.Split(" ")[0], out var library) ? library : null)
                    .Where(library => library is not null);

                this.PopulateDependencies(libraries);

                var resultFramework = this.FindTransitiveDependencies(
                    framework, projectDependencies!, collateAllDependencies, filter);

                resultProject.Add(resultFramework);
            }

            result.Add(resultProject);
        }

        return result;
    }

    /// <summary>
    /// Creates the collection of .NET projects to analyze.
    /// </summary>
    /// <param name="projectOrSolutionPath">The path of the .NET project or solution file, including the file
    /// name.</param>
    /// <returns>The collection of .NET projects.</returns>
    private IReadOnlyCollection<PackageSpec> CreateProjects(string projectOrSolutionPath) =>
        this.CreateProjectDependencyGraph(projectOrSolutionPath)
            .Projects
            .Where(project => project.RestoreMetadata.ProjectStyle == ProjectStyle.PackageReference)
            .ToArray();

    /// <summary>
    /// Creates the project dependency graph, which is used for generating the collection of .NET projects to be
    /// analyzed.
    /// </summary>
    /// <param name="projectOrSolutionPath">The path of the .NET project or solution file, including the file
    /// name.</param>
    /// <returns>The project dependency graph.</returns>
    private DependencyGraphSpec CreateProjectDependencyGraph(string projectOrSolutionPath) =>
        this.dependencyGraph.Create(projectOrSolutionPath);

    /// <summary>
    /// Creates the assets file to be analyzed.
    /// </summary>
    /// <param name="project">The .NET project to analyze.</param>
    /// <returns>The assets file.</returns>
    private LockFile? CreateAssetsFile(PackageSpec project) =>
        this.assets.Create(project.FilePath, project.RestoreMetadata.OutputPath);

    /// <summary>
    /// Populates the collection of dependencies for a .NET project and framework combination.
    /// </summary>
    /// <param name="libraries">The collection of all libraries associated with the .NET project and framework
    /// combination.</param>
    private void PopulateDependencies(
        IReadOnlyDictionary<string, LockFileTargetLibrary> libraries)
    {
        foreach (var library in libraries.Values)
        {
            this.RecordDependency(library, null, libraries);
        }
    }

    /// <summary>
    /// Records a dependency for a .NET project and framework combination.
    /// </summary>
    /// <param name="library">The current library.</param>
    /// <param name="parent">The dependency that depends on <paramref name="library"/>, or <see langword="null"/> if
    /// <paramref name="library" /> is a direct project dependency.</param>
    /// <param name="libraries">The collection of all libraries associated with the .NET project and framework
    /// combination.</param>
    private void RecordDependency(
        LockFileTargetLibrary library,
        Dependency? parent,
        IReadOnlyDictionary<string, LockFileTargetLibrary> libraries)
    {
        if (!this.dependencies.ContainsKey(library.Name))
        {
            this.dependencies.Add(library.Name, new(library.Name, library.Version));
        }

        if (parent is not null)
        {
            _ = this.dependencies[library.Name].Via.Add(parent);
        }

        foreach (var libraryDependencies in library.Dependencies)
        {
            this.RecordDependency(libraries[libraryDependencies.Id], this.dependencies[library.Name], libraries);
        }
    }

    /// <summary>
    /// Finds the transitive NuGet dependencies by traversing the collection of dependencies previously recorded.
    /// </summary>
    /// <param name="framework">The .NET project and framework combination to analyze.</param>
    /// <param name="dependencies">The project dependencies.</param>
    /// <param name="collateAllDependencies">A value indicating whether all dependencies, or merely those that are
    /// transitive, should be collated.</param>
    /// <param name="filter">An optional regular expression, to match certain dependencies. It will filter
    /// non-transitive dependencies as well, if <paramref name="collateAllDependencies"/> is
    /// <see langword="true"/>.</param>
    /// <returns>The transitive NuGet dependency information, which can be processed for display.</returns>
    private Framework FindTransitiveDependencies(
        TargetFrameworkInformation framework,
        IEnumerable<LockFileTargetLibrary> dependencies,
        bool collateAllDependencies,
        Regex? filter)
    {
        foreach (var dependency in dependencies
            .Select(dependency =>
                !string.Equals(dependency.Name, "NETStandard.Library", StringComparison.OrdinalIgnoreCase) &&
                this.dependencies.TryGetValue(dependency.Name, out var value) ? value : null)
            .Where(dependency => dependency is not null && dependency!.Via.Count > 0))
        {
            dependency!.IsTransitive = true;
        }

        var frameworkDependencies = collateAllDependencies
            ? this.dependencies.Values
            : this.dependencies.Values.Where(dependency => dependency.IsTransitive);

        if (filter is not null)
        {
            frameworkDependencies = frameworkDependencies
                .Where(dependency => filter.Match(dependency.Identifier).Success);
        }

        return new(framework.FrameworkName, frameworkDependencies.ToList());
    }
}
