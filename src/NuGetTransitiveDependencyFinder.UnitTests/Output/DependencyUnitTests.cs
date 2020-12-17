// <copyright file="DependencyUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Output
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using NuGet.Versioning;
    using NuGetTransitiveDependencyFinder.Output;
    using NuGetTransitiveDependencyFinder.UnitTests.TestUtilities;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="Dependency"/> class.
    /// </summary>
    public class DependencyUnitTests
    {
        /// <summary>
        /// The default identifier.
        /// </summary>
        private const string DefaultIdentifier = "identifier";

        /// <summary>
        /// The default version.
        /// </summary>
        private static readonly NuGetVersion DefaultVersion = new NuGetVersion("1.0.0");

        /// <summary>
        /// The default dependency.
        /// </summary>
        private static readonly Dependency DefaultDependency = new Dependency(DefaultIdentifier, DefaultVersion);

        /// <summary>
        /// The data for testing the operators.
        /// </summary>
        private static readonly (Dependency left, Dependency right, Comparisons comparison)[] OperatorTestData = new[]
            {
                (
                    null,
                    null,
                    Comparisons.Equal
                ),
                (
                    DefaultDependency,
                    null,
                    Comparisons.NotEqual
                ),
                (
                    null,
                    DefaultDependency,
                    Comparisons.NotEqual
                ),
                (
                    DefaultDependency,
                    DefaultDependency,
                    Comparisons.Equal | Comparisons.LessThanOrEqual | Comparisons.GreaterThanOrEqual
                ),
                (
                    DefaultDependency,
                    new Dependency("Identifier", DefaultVersion),
                    Comparisons.Equal | Comparisons.LessThanOrEqual | Comparisons.GreaterThanOrEqual
                ),
                (
                    DefaultDependency,
                    new Dependency(DefaultIdentifier, new NuGetVersion("1.0.0")),
                    Comparisons.Equal | Comparisons.LessThanOrEqual | Comparisons.GreaterThanOrEqual
                ),
                (
                    DefaultDependency,
                    new Dependency("xyz", DefaultVersion),
                    Comparisons.NotEqual | Comparisons.LessThan | Comparisons.LessThanOrEqual
                ),
                (
                    DefaultDependency,
                    new Dependency("XYZ", DefaultVersion),
                    Comparisons.NotEqual | Comparisons.LessThan | Comparisons.LessThanOrEqual
                ),
                (
                    DefaultDependency,
                    new Dependency(DefaultIdentifier, new NuGetVersion("1.0.1")),
                    Comparisons.NotEqual | Comparisons.LessThan | Comparisons.LessThanOrEqual
                ),
                (
                    DefaultDependency,
                    new Dependency("abc", DefaultVersion),
                    Comparisons.NotEqual | Comparisons.GreaterThan | Comparisons.GreaterThanOrEqual
                ),
                (
                    DefaultDependency,
                    new Dependency("ABC", DefaultVersion),
                    Comparisons.NotEqual | Comparisons.GreaterThan | Comparisons.GreaterThanOrEqual
                ),
                (
                    DefaultDependency,
                    new Dependency(DefaultIdentifier, new NuGetVersion("0.9.9")),
                    Comparisons.NotEqual | Comparisons.GreaterThan | Comparisons.GreaterThanOrEqual
                ),
            };

        /// <summary>
        /// The data for testing the operators.
        /// </summary>
        private static readonly (DateTime? left, DateTime? right, Comparisons comparison)[] OperatorTestData2 = new[]
            {
                (
                    (DateTime?)null,
                    (DateTime?)null,
                    Comparisons.Equal
                ),
                (
                    DateTime.UtcNow,
                    (DateTime?)null,
                    Comparisons.NotEqual
                ),
                (
                    (DateTime?)null,
                    DateTime.UtcNow,
                    Comparisons.NotEqual
                ),
                (
                    DateTime.UtcNow,
                    DateTime.UtcNow,
                    Comparisons.Equal | Comparisons.LessThanOrEqual | Comparisons.GreaterThanOrEqual
                ),
                (
                    DateTime.UtcNow,
                    DateTime.MaxValue,
                    Comparisons.NotEqual | Comparisons.LessThan | Comparisons.LessThanOrEqual
                ),
                (
                    DateTime.UtcNow,
                    DateTime.MinValue,
                    Comparisons.NotEqual | Comparisons.GreaterThan | Comparisons.GreaterThanOrEqual
                ),
            };

        /// <summary>
        /// Generates the data for testing <see cref="Dependency.operator =="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> GenerateOperatorEqualTestData() =>
            FilterData(Comparisons.Equal);

        /// <summary>
        /// Generates the data for testing <see cref="Dependency.operator !="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> GenerateOperatorNotEqualTestData() =>
            FilterData(Comparisons.NotEqual);

        /// <summary>
        /// Generates the data for testing <see cref="Dependency.operator &lt;"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> GenerateOperatorLessThanTestData() =>
            FilterData(Comparisons.LessThan);

        /// <summary>
        /// Generates the data for testing <see cref="Dependency.operator &lt;="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> GenerateOperatorLessThanOrEqualTestData() =>
            FilterData(Comparisons.LessThanOrEqual);

        /// <summary>
        /// Generates the data for testing <see cref="Dependency.operator &gt;"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> GenerateOperatorGreaterThanTestData() =>
            FilterData(Comparisons.GreaterThan);

        /// <summary>
        /// Generates the data for testing <see cref="Dependency.operator &gt;="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> GenerateOperatorGreaterThanOrEqualTestData() =>
            FilterData(Comparisons.GreaterThanOrEqual);

        /// <summary>
        /// Generates the data for testing <see cref="Dependency.operator =="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> GenerateOperatorEqualTestData2() =>
            FilterData2(Comparisons.Equal);

        /// <summary>
        /// Generates the data for testing <see cref="Dependency.operator !="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> GenerateOperatorNotEqualTestData2() =>
            FilterData2(Comparisons.NotEqual);

        /// <summary>
        /// Generates the data for testing <see cref="Dependency.operator &lt;"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> GenerateOperatorLessThanTestData2() =>
            FilterData2(Comparisons.LessThan);

        /// <summary>
        /// Generates the data for testing <see cref="Dependency.operator &lt;="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> GenerateOperatorLessThanOrEqualTestData2() =>
            FilterData2(Comparisons.LessThanOrEqual);

        /// <summary>
        /// Generates the data for testing <see cref="Dependency.operator &gt;"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> GenerateOperatorGreaterThanTestData2() =>
            FilterData2(Comparisons.GreaterThan);

        /// <summary>
        /// Generates the data for testing <see cref="Dependency.operator &gt;="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> GenerateOperatorGreaterThanOrEqualTestData2() =>
            FilterData2(Comparisons.GreaterThanOrEqual);

        /// <summary>
        /// Tests that when <see cref="Dependency.Identifier"/> is called after construction, it returns the value
        /// specified.
        /// </summary>
        /// <param name="value">The value of <see cref="Dependency.Identifier"/>.</param>
        [Theory]
        [InlineData("Identifier 1")]
        [InlineData("Identifier 2")]
        public void Identifer_CalledAfterConstruction_ReturnsValue(string value)
        {
            // Arrange & Act
            var dependency = new Dependency(value, DefaultVersion);

            // Assert
            value.Should().Be(dependency.Identifier);
        }

        /// <summary>
        /// Tests that when <see cref="Dependency.Version"/> is called after construction, it returns the value
        /// specified.
        /// </summary>
        /// <param name="value">The value of <see cref="Dependency.Version"/>.</param>
        [Theory]
        [InlineData("1.0.0")]
        [InlineData("2.0.0")]
        public void Version_CalledAfterConstruction_ReturnsValue(string value)
        {
            // Arrange & Act
            var dependency = new Dependency(DefaultIdentifier, new NuGetVersion(value));

            // Assert
            value.Should().Be(dependency.Version.ToString());
        }

        /// <summary>
        /// Tests that when <see cref="Dependency.IsTransitive"/> is called after being set, it returns the value
        /// specified.
        /// </summary>
        /// <param name="value">The value of <see cref="Dependency.IsTransitive"/>.</param>
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsTransitive_CalledAfterSetting_ReturnsValue(bool value)
        {
            // Arrange & Act
            var dependency = new Dependency(DefaultIdentifier, DefaultVersion)
            {
                IsTransitive = value,
            };

            // Assert
            value.Should().Be(dependency.IsTransitive);
        }

        /// <summary>
        /// Tests that when <see cref="Dependency.operator =="/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected response.</param>
        [Theory]
        [MemberData(nameof(GenerateOperatorEqualTestData))]
        public void OperatorEqual_CalledWithDifferentValues_ReturnsExpectedValues(
            Dependency left,
            Dependency right,
            bool expected)
        {
            // Act
            var result = left == right;

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Dependency.operator !="/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected response.</param>
        [Theory]
        [MemberData(nameof(GenerateOperatorNotEqualTestData))]
        public void OperatorNotEqual_CalledWithDifferentValues_ReturnsExpectedValues(
            Dependency left,
            Dependency right,
            bool expected)
        {
            // Act
            var result = left != right;

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Dependency.operator &lt;"/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected response.</param>
        [Theory]
        [MemberData(nameof(GenerateOperatorLessThanTestData))]
        public void OperatorLessThan_CalledWithDifferentValues_ReturnsExpectedValues(
            Dependency left,
            Dependency right,
            bool expected)
        {
            // Act
            var result = left < right;

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Dependency.operator &lt;="/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected response.</param>
        [Theory]
        [MemberData(nameof(GenerateOperatorLessThanOrEqualTestData))]
        public void OperatorLessThanOrEqual_CalledWithDifferentValues_ReturnsExpectedValues(
            Dependency left,
            Dependency right,
            bool expected)
        {
            // Act
            var result = left <= right;

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Dependency.operator &gt;"/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected response.</param>
        [Theory]
        [MemberData(nameof(GenerateOperatorGreaterThanTestData))]
        public void OperatorGreaterThan_CalledWithDifferentValues_ReturnsExpectedValues(
            Dependency left,
            Dependency right,
            bool expected)
        {
            // Act
            var result = left > right;

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Dependency.operator &gt;="/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected response.</param>
        [Theory]
        [MemberData(nameof(GenerateOperatorGreaterThanOrEqualTestData))]
        public void OperatorGreaterThanOrEqual_CalledWithDifferentValues_ReturnsExpectedValues(
            Dependency left,
            Dependency right,
            bool expected)
        {
            // Act
            var result = left >= right;

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Dependency.operator =="/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected response.</param>
        [Theory]
        [MemberData(nameof(GenerateOperatorEqualTestData2))]
        public void OperatorEqual_CalledWithDifferentValues_ReturnsExpectedValues2(
            DateTime? left,
            DateTime? right,
            bool expected)
        {
            // Act
            var result = left == right;

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Dependency.operator !="/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected response.</param>
        [Theory]
        [MemberData(nameof(GenerateOperatorNotEqualTestData2))]
        public void OperatorNotEqual_CalledWithDifferentValues_ReturnsExpectedValues2(
            DateTime? left,
            DateTime? right,
            bool expected)
        {
            // Act
            var result = left != right;

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Dependency.operator &lt;"/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected response.</param>
        [Theory]
        [MemberData(nameof(GenerateOperatorLessThanTestData2))]
        public void OperatorLessThan_CalledWithDifferentValues_ReturnsExpectedValues2(
            DateTime? left,
            DateTime? right,
            bool expected)
        {
            // Act
            var result = left < right;

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Dependency.operator &lt;="/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected response.</param>
        [Theory]
        [MemberData(nameof(GenerateOperatorLessThanOrEqualTestData2))]
        public void OperatorLessThanOrEqual_CalledWithDifferentValues_ReturnsExpectedValues2(
            DateTime? left,
            DateTime? right,
            bool expected)
        {
            // Act
            var result = left <= right;

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Dependency.operator &gt;"/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected response.</param>
        [Theory]
        [MemberData(nameof(GenerateOperatorGreaterThanTestData2))]
        public void OperatorGreaterThan_CalledWithDifferentValues_ReturnsExpectedValues2(
            DateTime? left,
            DateTime? right,
            bool expected)
        {
            // Act
            var result = left > right;

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Dependency.operator &gt;="/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected response.</param>
        [Theory]
        [MemberData(nameof(GenerateOperatorGreaterThanOrEqualTestData2))]
        public void OperatorGreaterThanOrEqual_CalledWithDifferentValues_ReturnsExpectedValues2(
            DateTime? left,
            DateTime? right,
            bool expected)
        {
            // Act
            var result = left >= right;

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Filters <see cref="OperatorTestData"/> using <paramref name="match"/>.
        /// </summary>
        /// <param name="match">The match to apply to <see cref="OperatorTestData"/> when filtering the data.</param>
        /// <returns>The filtered data.</returns>
        private static IEnumerable<object[]> FilterData(Comparisons match)
        {
            foreach ((var left, var right, var comparison) in OperatorTestData)
            {
                yield return new object[] { left, right, (comparison & match) != 0 };
            }
        }

        /// <summary>
        /// Filters <see cref="OperatorTestData2"/> using <paramref name="match"/>.
        /// </summary>
        /// <param name="match">The match to apply to <see cref="OperatorTestData2"/> when filtering the data.</param>
        /// <returns>The filtered data.</returns>
        private static IEnumerable<object[]> FilterData2(Comparisons match)
        {
            foreach ((var left, var right, var comparison) in OperatorTestData2)
            {
                yield return new object[] { left, right, (comparison & match) != 0 };
            }
        }
    }
}
