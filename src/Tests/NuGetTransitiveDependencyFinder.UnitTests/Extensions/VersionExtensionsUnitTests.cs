// <copyright file="VersionExtensionsUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Extensions
{
    using System;
    using FluentAssertions;
    using NuGetTransitiveDependencyFinder.Extensions;
    using NuGetTransitiveDependencyFinder.TestUtilities.Globalization;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="VersionExtensions"/> class.
    /// </summary>
    public class VersionExtensionsUnitTests
    {
        /// <summary>
        /// Gets the data for testing <see cref="VersionExtensions.ToShortenedString(Version)"/>.
        /// </summary>
        public static TheoryData<Version, string> ToShortenedStringTestData =>
            new TheoryData<Version, string>
            {
                { new Version(0, 0, 0, 0), "0.0" },
                { new Version(1, 0, 0, 0), "1.0" },
                { new Version(1, 1, 0, 0), "1.1" },
                { new Version(1, 1, 1, 0), "1.1.1" },
                { new Version(1, 1, 0, 1), "1.1.0.1" },
                { new Version(1, 1, 1, 1), "1.1.1.1" },
            };

        /// <summary>
        /// Tests that when <see cref="VersionExtensions.ToShortenedString(Version)"/> is called with different values,
        /// it returns the correct value each time.
        /// </summary>
        /// <param name="value">The value to be converted to a string.</param>
        /// <param name="expected">The expected result.</param>
        [AllCulturesTheory]
        [MemberData(nameof(ToShortenedStringTestData))]
        public void ToShortenedString_WithDifferentValues_ReturnsCorrectValues(Version value, string expected)
        {
            // Act
            var result = value.ToShortenedString();

            // Assert
            _ = result.Should().Be(expected);
        }
    }
}
