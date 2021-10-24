// <copyright file="IDependencyGraph.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ProjectAnalysis
{
    using System;
    using NuGet.ProjectModel;

    /// <summary>
    /// An interface representing a dependency graph of .NET projects and their NuGet dependencies.
    /// </summary>
    internal interface IDependencyGraph : IDisposable
    {
        /// <summary>
        /// Creates a <see cref="DependencyGraphSpec"/> object representing the project dependency graph.
        /// </summary>
        /// <param name="projectOrSolutionPath">The path of the .NET project or solution file, including the file
        /// name.</param>
        /// <returns>The <see cref="DependencyGraphSpec"/> object.</returns>
        public DependencyGraphSpec Create(string projectOrSolutionPath);
    }
}
