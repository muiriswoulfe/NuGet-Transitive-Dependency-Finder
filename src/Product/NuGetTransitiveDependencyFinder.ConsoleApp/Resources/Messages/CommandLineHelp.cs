// <copyright file="CommandLineHelp.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ConsoleApp.Resources.Messages
{
    using static System.FormattableString;

    /// <summary>
    /// A strongly typed resource class for accessing localized command-line help strings.
    /// </summary>
    public static class CommandLineHelp
    {
        /// <summary>
        /// Gets a localized string corresponding to the description of the "--all" parameter, which is displayed as
        /// part of the help message.
        /// </summary>
        public static string All =>
            GetString(nameof(All));

        /// <summary>
        /// Gets a localized string corresponding to the description of the  "--projectOrSolution" parameter, which is
        /// displayed as part of the help message.
        /// </summary>
        public static string ProjectOrSolution =>
            GetString(nameof(ProjectOrSolution));

        /// <summary>
        /// Gets a localized string corresponding to the description of the  "--filter" parameter, which is
        /// displayed as part of the help message.
        /// </summary>
        public static string Filter =>
            GetString(nameof(Filter));

        /// <summary>
        /// Gets the localized string.
        /// </summary>
        /// <param name="name">The name of the localized string to be accessed.</param>
        /// <returns>The contents of the localized string.</returns>
        private static string GetString(string name) =>
            Strings.GetString(Invariant($"{nameof(CommandLineHelp)}_{name}"));
    }
}
