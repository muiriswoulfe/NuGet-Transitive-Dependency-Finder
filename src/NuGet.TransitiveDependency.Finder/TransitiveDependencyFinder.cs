// <copyright file="TransitiveDependencyFinder.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGet.TransitiveDependency.Finder
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using Microsoft.Extensions.Logging;
    using NuGet.ProjectModel;
    using NuGet.TransitiveDependency.Finder.Output;
    using NuGet.TransitiveDependency.Finder.ProjectAnalysis;
    using static System.FormattableString;

    /// <summary>
    /// A class that manages the overall process of finding transitive NuGet dependencies.
    /// </summary>
    public class TransitiveDependencyFinder
    {
        /// <summary>
        /// The logger factory from which a logger will be created.
        /// </summary>
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// The collection of dependencies recorded and stored temporarily for the purposes of finding transitive NuGet
        /// dependencies.
        /// </summary>
        private readonly ISet<string> dependencies = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Initializes a new instance of the <see cref="TransitiveDependencyFinder"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory from which a logger will be created.</param>
        public TransitiveDependencyFinder(ILoggerFactory loggerFactory) =>
            this.loggerFactory = loggerFactory;

        /// <summary>
        /// Runs the logic for finding transitive NuGet dependencies.
        /// </summary>
        /// <param name="solutionPath">The path of the .NET solution file, including the file name.</param>
        /// <returns>The transitive NuGet dependency information, which can be processed for display.</returns>
        public Projects Run(string solutionPath)
        {
            var projects = this.CreateProjects(solutionPath);
            var result = new Projects(projects.Count);
            foreach (var project in projects)
            {
                var assetsFiles = this.CreateAssetsFiles(project);
                if (assetsFiles == null)
                {
                    continue;
                }

                var resultProject = new Project(project.TargetFrameworks.Count, project.Name);
                foreach (var framework in project.TargetFrameworks)
                {
                    this.dependencies.Clear();
                    var libraries = assetsFiles
                        .First(target => target.TargetFramework == framework.FrameworkName)
                        .Libraries
                        .ToImmutableDictionary(library => library.Name, StringComparer.OrdinalIgnoreCase);
                    this.PopulateDependencies(framework, libraries);
                    var resultFramework = this.FindTransitiveDependencies(framework);

                    resultProject.Add(resultFramework);
                }

                result.Add(resultProject);
            }

            return result;
        }

        /// <summary>
        /// Creates the collection of .NET projects to analyze.
        /// </summary>
        /// <param name="solutionPath">The path of the .NET solution file, including the file name.</param>
        /// <returns>The collection of .NET projects.</returns>
        private IReadOnlyCollection<PackageSpec> CreateProjects(string solutionPath) =>
            this.CreateProjectDependencyGraph(solutionPath)
                .Projects
                .Where(project => project.RestoreMetadata.ProjectStyle == ProjectStyle.PackageReference)
                .ToArray();

        /// <summary>
        /// Creates the project dependency graph, which is used for generating the collection of .NET projects to be
        /// analyzed.
        /// </summary>
        /// <param name="solutionPath">The path of the .NET solution file, including the file name.</param>
        /// <returns>The project dependency graph.</returns>
        private DependencyGraphSpec CreateProjectDependencyGraph(string solutionPath)
        {
            using var dependencyGraph = new DependencyGraph(this.loggerFactory, solutionPath);
            return dependencyGraph.Create();
        }

        /// <summary>
        /// Creates the collection of assets files to be analyzed.
        /// </summary>
        /// <param name="project">The .NET project to analyze.</param>
        /// <returns>The collection of assets files.</returns>
        private IReadOnlyCollection<LockFileTarget>? CreateAssetsFiles(PackageSpec project)
        {
            var assets = new Assets(this.loggerFactory, project.FilePath, project.RestoreMetadata.OutputPath);
            return assets.Create()?.Targets?.ToArray();
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
                .Where(library => library != null))
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
            if (this.dependencies.Contains(library.Name))
            {
                return;
            }

            if (!isTopLevel)
            {
                var result = this.dependencies.Add(library.Name);
                if (!result)
                {
                    throw new InvalidOperationException(
                        Invariant($"Failed to add '{library.Name}' to the collection."));
                }
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
        /// <returns>The transitive NuGet dependency information, which can be processed for display.</returns>
        private Framework FindTransitiveDependencies(TargetFrameworkInformation framework)
        {
            var result = new Framework(framework.Dependencies.Count, framework.FrameworkName);
            foreach (var dependency in framework
                .Dependencies
                .Where(dependency =>
                    !string.Equals(dependency.Name, "NETStandard.Library", StringComparison.OrdinalIgnoreCase) &&
                    this.dependencies.Contains(dependency.Name)))
            {
                result.Add(dependency.Name);
            }

            return result;
        }
    }
}
