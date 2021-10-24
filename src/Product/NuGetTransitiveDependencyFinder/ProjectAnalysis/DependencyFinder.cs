// <copyright file="DependencyFinder.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ProjectAnalysis
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
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
        public Projects Run(string projectOrSolutionPath, bool collateAllDependencies)
        {
            var projects = this.CreateProjects(projectOrSolutionPath);
            var result = new Projects(projects.Count);
            foreach (var project in projects)
            {
                var assetsFiles = this.CreateAssetsFiles(project);
                if (assetsFiles is null)
                {
                    continue;
                }

                var resultProject = new Project(project.Name, project.TargetFrameworks.Count);
                foreach (var framework in project.TargetFrameworks)
                {
                    this.dependencies.Clear();
                    var libraries = assetsFiles
                        .First(target => target.TargetFramework == framework.FrameworkName)
                        .Libraries
                        .ToImmutableDictionary(library => library.Name, StringComparer.OrdinalIgnoreCase);
                    this.PopulateDependencies(framework, libraries);
                    var resultFramework = this.FindTransitiveDependencies(framework, collateAllDependencies);

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
        /// Creates the collection of assets files to be analyzed.
        /// </summary>
        /// <param name="project">The .NET project to analyze.</param>
        /// <returns>The collection of assets files.</returns>
        private IReadOnlyCollection<LockFileTarget>? CreateAssetsFiles(PackageSpec project)
        {
            var createdAssets = this.assets
                .Create(project.FilePath, project.RestoreMetadata.OutputPath);

            return createdAssets?.Targets?.ToArray();
        }

        /// <summary>
        /// Populates the collection of dependencies for a .NET project and framework combination.
        /// </summary>
        /// <param name="framework">The .NET project and framework combination to analyze.</param>
        /// <param name="libraries">The collection of all libraries associated with the .NET project and framework
        /// combination.</param>
        private void PopulateDependencies(
            TargetFrameworkInformation framework,
            IReadOnlyDictionary<string, LockFileTargetLibrary> libraries)
        {
            foreach (var library in framework
                .Dependencies
                .Select(dependency => libraries.TryGetValue(dependency.Name, out var library) ? library : null)
                .Where(library => library is not null))
            {
                this.RecordDependency(true, library!, libraries);
            }
        }

        /// <summary>
        /// Records a dependency for a .NET project and framework combination.
        /// </summary>
        /// <param name="isTopLevel">A value indicating whether the current library is a top-level library, i.e. one
        /// that is referenced within a .NET project file.</param>
        /// <param name="library">The current library.</param>
        /// <param name="libraries">The collection of all libraries associated with the .NET project and framework
        /// combination.</param>
        private void RecordDependency(
            bool isTopLevel,
            LockFileTargetLibrary library,
            IReadOnlyDictionary<string, LockFileTargetLibrary> libraries)
        {
            if (this.dependencies.ContainsKey(library.Name))
            {
                return;
            }

            if (!isTopLevel)
            {
                this.dependencies.Add(library.Name, new(library.Name, library.Version));
            }

            foreach (var libraryDependencies in library.Dependencies)
            {
                this.RecordDependency(false, libraries[libraryDependencies.Id], libraries);
            }
        }

        /// <summary>
        /// Finds the transitive NuGet dependencies by traversing the collection of dependencies previously recorded.
        /// </summary>
        /// <param name="framework">The .NET project and framework combination to analyze.</param>
        /// <param name="collateAllDependencies">A value indicating whether all dependencies, or merely those that are
        /// transitive, should be collated.</param>
        /// <returns>The transitive NuGet dependency information, which can be processed for display.</returns>
        private Framework FindTransitiveDependencies(TargetFrameworkInformation framework, bool collateAllDependencies)
        {
            foreach (var dependency in framework
                .Dependencies
                .Select(dependency =>
                    !string.Equals(dependency.Name, "NETStandard.Library", StringComparison.OrdinalIgnoreCase) &&
                    this.dependencies.TryGetValue(dependency.Name, out var value) ? value : null)
                .Where(dependency => dependency is not null))
            {
                dependency!.IsTransitive = true;
            }

            var frameworkDependencies = collateAllDependencies
                ? this.dependencies.Values
                : this.dependencies.Values.Where(dependency => dependency.IsTransitive);

            return new(framework.FrameworkName, frameworkDependencies.ToList());
        }
    }
}
