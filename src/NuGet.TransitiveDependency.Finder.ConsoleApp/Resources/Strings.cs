// <copyright file="Strings.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGet.TransitiveDependency.Finder.ConsoleApp.Resources
{
    using System.Globalization;
    using System.Resources;

    /// <summary>
    /// A strongly-typed resource class for accessing localized strings.
    /// </summary>
    internal static class Strings
    {
        /// <summary>
        /// The resource manager, through which the localized strings are accessed.
        /// </summary>
        private static readonly ResourceManager ResourceManager =
            new ResourceManager(typeof(Strings).FullName!, typeof(Strings).Assembly);

        /// <summary>
        /// Gets a localized string containing the error message displayed when no command-line parameter is provided.
        /// </summary>
        public static string ErrorMissingParameter =>
            GetString("ErrorMissingParameter");

        /// <summary>
        /// Gets a localized string containing the message displayed when analysis is about to commence.
        /// </summary>
        public static string InfoCommencingAnalysis =>
            GetString("InfoCommencingAnalysis");

        /// <summary>
        /// Gets a localized string containing the message displayed when no transitive NuGet dependencies are found.
        /// </summary>
        public static string InfoNoTransitiveNuGetDependencies =>
            GetString("InfoNoTransitiveNuGetDependencies");

        /// <summary>
        /// Get the localized string.
        /// </summary>
        /// <param name="name">The name of the localized string to be accessed.</param>
        /// <returns>The contents of the localized string.</returns>
        private static string GetString(string name) =>
            ResourceManager.GetString(name, CultureInfo.CurrentUICulture) !;
    }
}
