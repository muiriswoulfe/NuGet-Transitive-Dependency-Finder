// <copyright file="CommandLineOptions.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ConsoleApp.Input
{
    using System.Diagnostics.CodeAnalysis;
    using CommandLine;
    using NuGetTransitiveDependencyFinder.ConsoleApp.Resources.Messages;

    /// <summary>
    /// A class specifying the application's command-line parameters.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Performance",
        "CA1812",
        Justification = "This class is constructed via the command-line parser.")]
    internal class CommandLineOptions
    {
        /// <summary>
        /// Gets a value indicating whether all NuGet dependencies, including non-transitive dependencies,
        /// should be listed.
        /// </summary>
        [Option('a', "all", HelpText = nameof(All), ResourceType = typeof(CommandLineHelp))]
        public bool All { get; init; }

        /// <summary>
        /// Gets the file name of the .NET project or solution to analyze.
        /// </summary>
        [Option(
            'p',
            "projectOrSolution",
            Required = true,
            HelpText = nameof(ProjectOrSolution),
            ResourceType = typeof(CommandLineHelp))]
        public string? ProjectOrSolution { get; init; }
    }
}
