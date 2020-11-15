// <copyright file="Strings.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ConsoleApp.Resources
{
    using System.Globalization;
    using System.Resources;
    using static System.FormattableString;

    /// <summary>
    /// A strongly typed resource class for accessing localized strings.
    /// </summary>
    internal static class Strings
    {
        /// <summary>
        /// The resource manager, through which the localized strings are accessed.
        /// </summary>
        private static readonly ResourceManager ResourceManager =
            new ResourceManager(typeof(Strings).FullName!, typeof(Strings).Assembly);

        /// <summary>
        /// Gets the localized string.
        /// </summary>
        /// <param name="name">The name of the localized string to be accessed.</param>
        /// <returns>The contents of the localized string.</returns>
        private static string GetString(string name) =>
            ResourceManager.GetString(name, CultureInfo.CurrentCulture) !;

        /// <summary>
        /// A strongly typed resource class for accessing localized error strings.
        /// </summary>
        public static class Error
        {
            /// <summary>
            /// Gets a localized string corresponding to the error message displayed when no command-line parameter is
            /// provided.
            /// </summary>
            public static string MissingParameter =>
                GetErrorString(nameof(MissingParameter));

            /// <summary>
            /// Gets the localized error string.
            /// </summary>
            /// <param name="name">The name of the localized error string to be accessed.</param>
            /// <returns>The contents of the localized error string.</returns>
            private static string GetErrorString(string name) =>
                GetString(Invariant($"{nameof(Error)}_{name}"));
        }

        /// <summary>
        /// A strongly typed resource class for accessing localized information strings.
        /// </summary>
        public static class Information
        {
            /// <summary>
            /// Gets a localized string corresponding to the message displayed when analysis is about to commence.
            /// </summary>
            public static string CommencingAnalysis =>
                GetInformationString(nameof(CommencingAnalysis));

            /// <summary>
            /// Gets a localized string corresponding to the message displayed when no transitive NuGet dependencies are
            /// found.
            /// </summary>
            public static string NoTransitiveNuGetDependencies =>
                GetInformationString(nameof(NoTransitiveNuGetDependencies));

            /// <summary>
            /// Gets the localized information string.
            /// </summary>
            /// <param name="name">The name of the localized information string to be accessed.</param>
            /// <returns>The contents of the localized information string.</returns>
            private static string GetInformationString(string name) =>
                GetString(Invariant($"{nameof(Information)}_{name}"));
        }
    }
}
