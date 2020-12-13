// <copyright file="DependencyUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Output
{
    using System;
    using System.Collections.Generic;
    using NuGet.Versioning;
    using NuGetTransitiveDependencyFinder.Output;
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
        private static readonly (
            Dependency left,
            Dependency right,
            bool isEqual,
            bool isLessThan,
            bool isGreaterThan)[] OperatorTestData = new[]
            {
                (null, null, true, false, false),
                (null, DefaultDependency, false, false, false),
                (DefaultDependency, null, false, false, true),
                (DefaultDependency, DefaultDependency, true, false, false),
                (DefaultDependency, new Dependency("Identifier", DefaultVersion), true, false, false),
                (DefaultDependency, new Dependency(DefaultIdentifier, new NuGetVersion("1.0.0")), true, false, false),
                (DefaultDependency, new Dependency("xyz", DefaultVersion), false, true, false),
                (DefaultDependency, new Dependency("XYZ", DefaultVersion), false, true, false),
                (DefaultDependency, new Dependency(DefaultIdentifier, new NuGetVersion("1.0.1")), false, true, false),
                (DefaultDependency, new Dependency("abc", DefaultVersion), false, false, true),
                (DefaultDependency, new Dependency("ABC", DefaultVersion), false, false, true),
                (DefaultDependency, new Dependency(DefaultIdentifier, new NuGetVersion("0.9.9")), false, false, true),
            };

        /// <summary>
        /// Generates the data for testing <see cref="Dependency.operator =="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> GenerateOperatorEqualTestData() =>
            FilterData((bool isEqual, bool _, bool _) => isEqual);

        /// <summary>
        /// Generates the data for testing <see cref="Dependency.operator !="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> GenerateOperatorNotEqualTestData() =>
            FilterData((bool isEqual, bool _, bool _) => !isEqual);

        /// <summary>
        /// Generates the data for testing <see cref="Dependency.operator &lt;"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> GenerateOperatorLessThanTestData() =>
            FilterData((bool _, bool isLessThan, bool _) => isLessThan);

        /// <summary>
        /// Generates the data for testing <see cref="Dependency.operator &lt;="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> GenerateOperatorLessThanOrEqualTestData() =>
            FilterData((bool isEqual, bool isLessThan, bool _) => isLessThan || isEqual);

        /// <summary>
        /// Generates the data for testing <see cref="Dependency.operator &gt;"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> GenerateOperatorGreaterThanTestData() =>
            FilterData((bool _, bool _, bool isGreaterThan) => isGreaterThan);

        /// <summary>
        /// Generates the data for testing <see cref="Dependency.operator &gt;="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> GenerateOperatorGreaterThanOrEqualTestData() =>
            FilterData((bool isEqual, bool _, bool isGreaterThan) => isGreaterThan || isEqual);

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
            Assert.Equal(value, dependency.Identifier);
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
            Assert.Equal(value, dependency.Version.ToString());
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
            Assert.Equal(value, dependency.IsTransitive);
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
            Assert.Equal(expected, result);
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
            Assert.Equal(expected, result);
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
            Assert.Equal(expected, result);
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
            Assert.Equal(expected, result);
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
            Assert.Equal(expected, result);
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
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Filters <see cref="OperatorTestData"/> using <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">The predicate with which to filter <see cref="OperatorTestData"/>.</param>
        /// <returns>The filtered data.</returns>
        private static IEnumerable<object[]> FilterData(Func<bool, bool, bool, bool> predicate)
        {
            foreach ((var left, var right, var isEqual, var isLessThan, var isGreaterThan) in OperatorTestData)
            {
                yield return new object[] { left, right, predicate(isEqual, isLessThan, isGreaterThan) };
            }
        }
    }
}
