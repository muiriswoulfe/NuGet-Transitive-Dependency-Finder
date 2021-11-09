// <copyright file="ITransitiveDependencyFinder.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder;

using System;
using System.Text.RegularExpressions;
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
    /// <param name="filter">An optional regular expression pattern, to match certain dependencies. It will filter
    /// non-transitive dependencies as well, if <paramref name="collateAllDependencies"/> is
    /// <see langword="true"/>.</param>
    /// <returns>The transitive NuGet dependency information, which can be processed for display.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="projectOrSolutionPath"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="filter"/> is not a valid regular expression.</exception>
    public Projects Run(string? projectOrSolutionPath, bool collateAllDependencies, Regex? filter);
}
