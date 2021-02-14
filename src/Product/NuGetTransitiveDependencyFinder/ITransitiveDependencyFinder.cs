// <copyright file="ITransitiveDependencyFinder.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder
{
    using System;
    using NuGetTransitiveDependencyFinder.Output;

    /// <summary>
    /// An interface that manages the overall process of finding transitive NuGet dependencies.
    /// </summary>
    public interface ITransitiveDependencyFinder : IDisposable
    {
        /// <summary>
        /// Runs the logic for finding transitive NuGet dependencies.
        /// </summary>
        /// <param name="projectOrSolutionPath">The path of the .NET project or solution file, including the file
        /// name.</param>
        /// <param name="collateAllDependencies">A value indicating whether all dependencies, or merely those that are
        /// transitive, should be collated.</param>
        /// <returns>The transitive NuGet dependency information, which can be processed for display.</returns>
        public Projects Run(string projectOrSolutionPath, bool collateAllDependencies);
    }
}
