// <copyright file="VersionExtensionsUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Extensions;

using System;
using FluentAssertions;
using NuGetTransitiveDependencyFinder.Extensions;
using NuGetTransitiveDependencyFinder.UnitTests.Output.Serialization;
using NuGetTransitiveDependencyFinder.UnitTests.Utilities.Globalization;
using Xunit;

/// <summary>
/// Unit tests for the <see cref="VersionExtensions"/> class.
/// </summary>
public class VersionExtensionsUnitTests
{
    /// <summary>
    /// Gets the data for testing <see cref="VersionExtensions.ToShortenedString(Version)"/>.
    /// </summary>
    public static TheoryData<SerializedVersion, string> ToShortenedStringTestData =>
        new()
        {
            { new(new Version(0, 0, 0, 0)), "0.0" },
            { new(new Version(1, 0, 0, 0)), "1.0" },
            { new(new Version(1, 1, 0, 0)), "1.1" },
            { new(new Version(1, 1, 1, 0)), "1.1.1" },
            { new(new Version(1, 1, 0, 1)), "1.1.0.1" },
            { new(new Version(1, 1, 1, 1)), "1.1.1.1" },
        };

    /// <summary>
    /// Tests that when <see cref="VersionExtensions.ToShortenedString(Version)"/> is called with different values, it
    /// returns the correct value each time.
    /// </summary>
    /// <param name="value">The value to be converted to a string.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(ToShortenedStringTestData))]
    public void ToShortenedString_WithDifferentValues_ReturnsCorrectValues(SerializedVersion value, string expected)
    {
        // Act
        var result = value.Version.ToShortenedString();

        // Assert
        _ = result
            .Should().Be(expected);
    }
}
