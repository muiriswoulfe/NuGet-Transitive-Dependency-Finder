// <copyright file="ICommandLineOptions.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ConsoleApp.Input;

using System.Text.RegularExpressions;

/// <summary>
/// An interface specifying the application's command-line parameters.
/// </summary>
internal interface ICommandLineOptions
{
    /// <summary>
    /// Gets a value indicating whether all NuGet dependencies, including non-transitive dependencies, should be listed.
    /// </summary>
    public bool All { get; init; }

    /// <summary>
    /// Gets the file name of the .NET project or solution to analyze.
    /// </summary>
    public string? ProjectOrSolution { get; init; }

    /// <summary>
    /// Gets the optional regular expression, to match certain dependencies.
    /// </summary>
    public Regex? Filter { get; init; }
}
