// <copyright file="VersionExtensions.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.Extensions;

using System;
using System.Globalization;
using System.Text;

/// <summary>
/// A class containing extension methods for <see cref="Version"/>.
/// </summary>
internal static class VersionExtensions
{
    /// <summary>
    /// Converts a <see cref="Version"/> to a shortened string, in which the build and revision elements will be
    /// excluded when they are set to 0.
    /// </summary>
    /// <param name="value">The <see cref="Version"/> to convert.</param>
    /// <returns>The shortened string.</returns>
    public static string ToShortenedString(this Version value)
    {
        const int initialCapacity = 7;

        var result = new StringBuilder(initialCapacity);
        _ = result.AppendFormat(CultureInfo.CurrentCulture, "{0}.{1}", value.Major, value.Minor);

        if (value.Build > 0 || value.Revision > 0)
        {
            _ = result.AppendFormat(CultureInfo.CurrentCulture, ".{0}", value.Build);
        }

        if (value.Revision > 0)
        {
            _ = result.AppendFormat(CultureInfo.CurrentCulture, ".{0}", value.Revision);
        }

        return result.ToString();
    }
}
