// <copyright file="CommandLineOptions.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ConsoleApp.Input
{
    using System.Diagnostics.CodeAnalysis;
    using CommandLine;

    /// <summary>
    /// A class for formatting logging messages for simple console display.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Performance",
        "CA1812",
        Justification = "This class is constructed via the command-line parser.")]
    internal class CommandLineOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether stuff.
        /// </summary>
        [Option(HelpText = "Set output to verbose messages.")]
        public bool All { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether stuff part 2.
        /// </summary>
        [Value(0, MetaName="abc", Required = true, HelpText = "Set output to verbose messages.")]
        public string? Solution { get; set; }
    }
}
