// <copyright file="CommandLineOptions.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ConsoleApp.Input
{
    using CommandLine;
    using NuGetTransitiveDependencyFinder.ConsoleApp.Resources.Messages;

    /// <summary>
    /// A class specifying the application's command-line parameters.
    /// </summary>
    internal class CommandLineOptions : ICommandLineOptions
    {
        /// <inheritdoc/>
        [Option('a', "all", HelpText = nameof(All), ResourceType = typeof(CommandLineHelp))]
        public bool All { get; init; }

        /// <inheritdoc/>
        [Option(
            'p',
            "projectOrSolution",
            Required = true,
            HelpText = nameof(ProjectOrSolution),
            ResourceType = typeof(CommandLineHelp))]
        public string? ProjectOrSolution { get; init; }
    }
}
