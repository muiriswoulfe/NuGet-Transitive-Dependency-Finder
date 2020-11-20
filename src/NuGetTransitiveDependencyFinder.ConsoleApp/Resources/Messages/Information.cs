// <copyright file="Information.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ConsoleApp.Resources.Messages
{
    using static System.FormattableString;

    /// <summary>
    /// A strongly typed resource class for accessing localized information strings.
    /// </summary>
    internal static class Information
    {
        /// <summary>
        /// Gets a localized string corresponding to the message displayed when analysis is about to commence.
        /// </summary>
        public static string CommencingAnalysis =>
            GetString(nameof(CommencingAnalysis));

        /// <summary>
        /// Gets a localized string corresponding to the message displayed when no NuGet dependencies are found.
        /// </summary>
        public static string NoDependencies =>
            GetString(nameof(NoDependencies));

        /// <summary>
        /// Gets a localized string corresponding to the message displayed for each transitive NuGet dependency.
        /// </summary>
        public static string TransitiveDependency =>
            GetString(nameof(TransitiveDependency));

        /// <summary>
        /// Gets the localized string.
        /// </summary>
        /// <param name="name">The name of the localized string to be accessed.</param>
        /// <returns>The contents of the localized string.</returns>
        private static string GetString(string name) =>
            Strings.GetString(Invariant($"{nameof(Information)}_{name}"));
    }
}
