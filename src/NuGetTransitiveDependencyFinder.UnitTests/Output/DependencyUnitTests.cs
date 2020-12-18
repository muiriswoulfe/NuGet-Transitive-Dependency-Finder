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
    using NuGetTransitiveDependencyFinder.UnitTests.TestUtilities.ComparisonTests;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="Dependency"/> class.
    /// </summary>
    public class DependencyUnitTests
    {
        /// <summary>
        /// The default identifier.
        /// </summary>
        private const string DefaultIdentifier = "Identifier";

        /// <summary>
        /// The default version.
        /// </summary>
        private static readonly NuGetVersion DefaultVersion = new NuGetVersion("1.0.0-alpha");

        /// <summary>
        /// The default test value.
        /// </summary>
        private static readonly Dependency DefaultValue = new Dependency(DefaultIdentifier, DefaultVersion);

        /// <summary>
        /// A clone of <see cref="DefaultValue"/>, where the object contents are identical but the object reference is
        /// not.
        /// </summary>
        private static readonly Dependency ClonedDefaultValue = new Dependency(DefaultIdentifier, DefaultVersion);

        /// <summary>
        /// The lesser test value, which occurs prior to <see cref="DefaultValue"/> according to an ordered sort.
        /// </summary>
        private static readonly Dependency LesserValue =
            new Dependency(DefaultIdentifier, new NuGetVersion("0.9.9-alpha"));

        /// <summary>
        /// The data for testing the operators.
        /// </summary>
        private static readonly IList<ComparisonTestData<Dependency>> OperatorTestData = GenerateOperatorTestData();

        /// <summary>
        /// Gets the data for testing <see cref="Dependency.operator =="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> OperatorEqualTestData =>
            ComparisonDataGenerator.GenerateOperatorEqualTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Dependency.operator !="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> OperatorNotEqualTestData =>
            ComparisonDataGenerator.GenerateOperatorNotEqualTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Dependency.operator &lt;"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> OperatorLessThanTestData =>
            ComparisonDataGenerator.GenerateOperatorLessThanTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Dependency.operator &lt;="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> OperatorLessThanOrEqualTestData =>
            ComparisonDataGenerator.GenerateOperatorLessThanOrEqualTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Dependency.operator &gt;"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> OperatorGreaterThanTestData =>
            ComparisonDataGenerator.GenerateOperatorGreaterThanTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Dependency.operator &gt;="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> OperatorGreaterThanOrEqualTestData =>
            ComparisonDataGenerator.GenerateOperatorGreaterThanOrEqualTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="IComparable{Dependency}.CompareTo"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> CompareToTestData =>
            ComparisonDataGenerator.GenerateCompareToTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="IEquatable{Dependency}.Equals"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> EqualsTestData =>
            ComparisonDataGenerator.GenerateEqualsTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Dependency.GetHashCode()"/>.
        /// </summary>
        public static IEnumerable<object[]> GetHashCodeTestData =>
            GenerateGetHashCodeTestData();

        /// <summary>
        /// Gets the data for testing <see cref="object.ToString()"/>.
        /// </summary>
        public static IEnumerable<object[]> ToStringTestData =>
            new object[][]
            {
                new object[] { DefaultValue, "Identifier v1.0.0-alpha" },
                new object[] { LesserValue, "Identifier v0.9.9-alpha" },
            };

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
            _ = value.Should().Be(dependency.Identifier);
        }

        /// <summary>
        /// Tests that when <see cref="Dependency.Version"/> is called after construction, it returns the value
        /// specified.
        /// </summary>
        /// <param name="value">The value of <see cref="Dependency.Version"/>.</param>
        [Theory]
        [InlineData("0.9.9")]
        [InlineData("1.0.0-alpha")]
        [InlineData("2.0.0-beta")]
        public void Version_CalledAfterConstruction_ReturnsValue(string value)
        {
            // Arrange & Act
            var dependency = new Dependency(DefaultIdentifier, new NuGetVersion(value));

            // Assert
            _ = value.Should().Be(dependency.Version.ToString());
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
            _ = value.Should().Be(dependency.IsTransitive);
        }

        /// <summary>
        /// Tests that when <see cref="Dependency.operator =="/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(OperatorEqualTestData))]
        public void OperatorEqual_WithAllCases_ReturnsValue(Dependency left, Dependency right, bool expected)
        {
            // Act
            var result = left == right;

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Dependency.operator !="/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(OperatorNotEqualTestData))]
        public void OperatorNotEqual_WithAllCases_ReturnsValue(Dependency left, Dependency right, bool expected)
        {
            // Act
            var result = left != right;

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Dependency.operator &lt;"/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(OperatorLessThanTestData))]
        public void OperatorLessThan_WithAllCases_ReturnsValue(Dependency left, Dependency right, bool expected)
        {
            // Act
            var result = left < right;

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Dependency.operator &lt;="/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(OperatorLessThanOrEqualTestData))]
        public void OperatorLessThanOrEqual_WithAllCases_ReturnsValue(Dependency left, Dependency right, bool expected)
        {
            // Act
            var result = left <= right;

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Dependency.operator &gt;"/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(OperatorGreaterThanTestData))]
        public void OperatorGreaterThan_WithAllCases_ReturnsValue(Dependency left, Dependency right, bool expected)
        {
            // Act
            var result = left > right;

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Dependency.operator &gt;="/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(OperatorGreaterThanOrEqualTestData))]
        public void OperatorGreaterThanOrEqual_WithAllCases_ReturnsValue(
            Dependency left,
            Dependency right,
            bool expected)
        {
            // Act
            var result = left >= right;

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="IComparable{Dependency}.CompareTo"/> is called with different values, it returns
        /// the expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(CompareToTestData))]
        public void CompareTo_WithAllCases_ReturnsValue(Dependency left, Dependency right, int expected)
        {
            // Act
            var result = left.CompareTo(right);

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="IComparable.CompareTo"/> is called with different values against an
        /// <c>object</c>, it returns the expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(CompareToTestData))]
        public void CompareToObject_WithAllCases_ReturnsValue(Dependency left, object right, int expected)
        {
            // Act
            var result = left.CompareTo(right);

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="IComparable.CompareTo"/> is called with different object types, it throws an
        /// <see cref="ArgumentException"/>.
        /// </summary>
        [Fact]
        public void CompareToObject_WithDifferentObjectTypes_ThrowsArgumentException()
        {
            // Act
            Action action = () => DefaultValue.CompareTo("value");

            // Assert
            _ = action.Should().Throw<ArgumentException>()
                .WithMessage("Object must be of type Dependency. (Parameter 'obj')")
                .And.ParamName.Should().Be("obj");
        }

        /// <summary>
        /// Tests that when <see cref="IEquatable{Dependency}.Equals"/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(EqualsTestData))]
        public void Equals_WithAllCases_ReturnsValue(Dependency left, Dependency right, bool expected)
        {
            // Act
            var result = left.Equals(right);

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="IEquatable{Dependency}.Equals"/> is called with different values against an
        /// <c>object</c>, it returns the expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(EqualsTestData))]
        public void EqualsObject_WithAllCases_ReturnsValue(Dependency left, object right, bool expected)
        {
            // Act
            var result = left.Equals(right);

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="IEquatable{Dependency}.Equals"/> is called with different object types, it
        /// returns <c>false</c>.
        /// </summary>
        [Fact]
        public void EqualsObject_WithDifferentObjectTypes_ReturnsFalse()
        {
            // Act
            var result = DefaultValue.Equals("value");

            // Assert
            _ = result.Should().Be(false);
        }

        /// <summary>
        /// Tests that when <see cref="Dependency.GetHashCode()"/> is called with identical objects, it returns the same
        /// value each time.
        /// </summary>
        /// <param name="value1">The first value for which to compute a hash code.</param>
        /// <param name="value2">The second value for which to compute a hash code.</param>
        [Theory]
        [MemberData(nameof(GetHashCodeTestData))]
        public void GetHashCode_WithIdenticalObjects_ReturnsSameValue(Dependency value1, Dependency value2)
        {
            // Act
            var result1 = value1.GetHashCode();
            var result2 = value2.GetHashCode();

            // Assert
            _ = result1.Should().Be(result2);
        }

        /// <summary>
        /// Tests that when <see cref="Dependency.GetHashCode()"/> is called with different objects, it returns
        /// different values for each object.
        /// </summary>
        [Fact]
        public void GetHashCode_WithDifferentObjects_ReturnsDifferentValues()
        {
            // Act
            var result1 = DefaultValue.GetHashCode();
            var result2 = LesserValue.GetHashCode();

            // Assert
            _ = result1.Should().NotBe(result2);
        }

        /// <summary>
        /// Tests that when <see cref="object.ToString()"/> is called with different objects, it returns different
        /// strings for each object.
        /// </summary>
        /// <param name="value">The value to be converted to a string.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(ToStringTestData))]
        public void ToString_WithDifferentObjects_ReturnsString(Dependency value, string expected)
        {
            // Act
            var result = value.ToString();

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Generates the data for testing the operators, combining data from the base class with
        /// <see cref="Dependency"/>-specific data.
        /// </summary>
        /// <returns>The generated data.</returns>
        private static IList<ComparisonTestData<Dependency>> GenerateOperatorTestData()
        {
            var result =
                ComparisonDataGenerator.GenerateOperatorTestData(DefaultValue, ClonedDefaultValue, LesserValue, 8);

            result.Add(
                new ComparisonTestData<Dependency>(
                    DefaultValue,
                    new Dependency("Identifier", DefaultVersion),
                    Comparisons.Equal));
            result.Add(
                new ComparisonTestData<Dependency>(
                    DefaultValue,
                    new Dependency(DefaultIdentifier, new NuGetVersion("1.0.0-alpha")),
                    Comparisons.Equal));
            result.Add(
                new ComparisonTestData<Dependency>(
                    DefaultValue,
                    new Dependency("IDENTIFIER", DefaultVersion),
                    Comparisons.Equal));
            result.Add(
                new ComparisonTestData<Dependency>(
                    DefaultValue,
                    new Dependency(DefaultIdentifier, new NuGetVersion("1.0.0-ALPHA")),
                    Comparisons.Equal));
            result.Add(
                new ComparisonTestData<Dependency>(
                    new Dependency("ABC", DefaultVersion),
                    DefaultValue,
                    Comparisons.LessThan));
            result.Add(
                new ComparisonTestData<Dependency>(
                    new Dependency(DefaultIdentifier, new NuGetVersion("0.9.9-alpha")),
                    DefaultValue,
                    Comparisons.LessThan));
            result.Add(
                new ComparisonTestData<Dependency>(
                    DefaultValue,
                    new Dependency("ABC", DefaultVersion),
                    Comparisons.GreaterThan));
            result.Add(
                new ComparisonTestData<Dependency>(
                    DefaultValue,
                    new Dependency(DefaultIdentifier, new NuGetVersion("0.9.9-alpha")),
                    Comparisons.GreaterThan));

            return result;
        }

        /// <summary>
        /// Generates the data for testing <see cref="object.GetHashCode()"/>, combining data from the base class with
        /// <see cref="Dependency"/>-specific data.
        /// </summary>
        /// <returns>The generated data.</returns>
        private static IEnumerable<object[]> GenerateGetHashCodeTestData()
        {
            var result =
                ComparisonDataGenerator.GenerateGetHashCodeTestData(DefaultValue, ClonedDefaultValue, LesserValue, 4);

            result.Add(
                new object[]
                {
                    DefaultValue,
                    new Dependency("Identifier", DefaultVersion),
                });
            result.Add(
                new object[]
                {
                    DefaultValue,
                    new Dependency(DefaultIdentifier, new NuGetVersion("1.0.0-alpha")),
                });
            result.Add(
                new object[]
                {
                    DefaultValue,
                    new Dependency("IDENTIFIER", DefaultVersion),
                });
            result.Add(
                new object[]
                {
                    DefaultValue,
                    new Dependency(DefaultIdentifier, new NuGetVersion("1.0.0-ALPHA")),
                });

            return result;
        }
    }
}
