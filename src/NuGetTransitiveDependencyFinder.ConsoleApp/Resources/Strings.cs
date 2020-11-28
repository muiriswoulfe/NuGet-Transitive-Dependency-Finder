// <copyright file="Strings.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ConsoleApp.Resources
{
    using System.Globalization;
    using System.Resources;

    /// <summary>
    /// A resource class for accessing localized strings.
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
        public static string GetString(string name) =>
            ResourceManager.GetString(name, CultureInfo.CurrentCulture) !;
    }
}
