// <copyright file="ProjectUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Output
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using FluentAssertions;
    using NuGetTransitiveDependencyFinder.Output;
    using NuGetTransitiveDependencyFinder.UnitTests.TestUtilities.ComparisonTests;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="Project"/> class.
    /// </summary>
    public class ProjectUnitTests
    {
        /// <summary>
        /// The default identifier.
        /// </summary>
        private const string DefaultIdentifier = "Identifier";

        /// <summary>
        /// The default test value.
        /// </summary>
        private static readonly Project DefaultValue = new(DefaultIdentifier, 0);

        /// <summary>
        /// A clone of <see cref="DefaultValue"/>, where the object contents are identical but the object reference is
        /// not.
        /// </summary>
        private static readonly Project ClonedDefaultValue = new(DefaultIdentifier, 0);

        /// <summary>
        /// The lesser test value, which occurs prior to <see cref="DefaultValue"/> according to an ordered sort.
        /// </summary>
        private static readonly Project LesserValue = new("ABC", 0);

        /// <summary>
        /// The data for testing the operators.
        /// </summary>
        private static readonly IList<ComparisonTestData<Project>> OperatorTestData = GenerateOperatorTestData();

        /// <summary>
        /// Gets the data for testing <see cref="Project.operator =="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object?[]> OperatorEqualTestData =>
            ComparisonDataGenerator.GenerateOperatorEqualTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Project.operator !="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object?[]> OperatorNotEqualTestData =>
            ComparisonDataGenerator.GenerateOperatorNotEqualTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Project.operator &lt;"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object?[]> OperatorLessThanTestData =>
            ComparisonDataGenerator.GenerateOperatorLessThanTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Project.operator &lt;="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object?[]> OperatorLessThanOrEqualTestData =>
            ComparisonDataGenerator.GenerateOperatorLessThanOrEqualTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Project.operator &gt;"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object?[]> OperatorGreaterThanTestData =>
            ComparisonDataGenerator.GenerateOperatorGreaterThanTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Project.operator &gt;="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object?[]> OperatorGreaterThanOrEqualTestData =>
            ComparisonDataGenerator.GenerateOperatorGreaterThanOrEqualTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="IComparable{Project}.CompareTo"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object?[]> CompareToTestData =>
            ComparisonDataGenerator.GenerateCompareToTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="IEquatable{Project}.Equals"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object?[]> EqualsTestData =>
            ComparisonDataGenerator.GenerateEqualsTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Project.GetHashCode()"/>.
        /// </summary>
        public static IEnumerable<object[]> GetHashCodeTestData =>
            GenerateGetHashCodeTestData();

        /// <summary>
        /// Gets the data for testing <see cref="object.ToString()"/>.
        /// </summary>
        public static IEnumerable<object[]> ToStringTestData =>
            new object[][]
            {
                new object[] { DefaultValue, "Identifier" },
                new object[] { LesserValue, "ABC" },
            };

        /// <summary>
        /// Tests that when <see cref="Project.operator =="/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(OperatorEqualTestData))]
        public void OperatorEqual_WithAllCases_ReturnsValue(Project left, Project right, bool expected)
        {
            // Act
            var result = left == right;

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Project.operator !="/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(OperatorNotEqualTestData))]
        public void OperatorNotEqual_WithAllCases_ReturnsValue(Project left, Project right, bool expected)
        {
            // Act
            var result = left != right;

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Project.operator &lt;"/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(OperatorLessThanTestData))]
        public void OperatorLessThan_WithAllCases_ReturnsValue(Project left, Project right, bool expected)
        {
            // Act
            var result = left < right;

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Project.operator &lt;="/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(OperatorLessThanOrEqualTestData))]
        public void OperatorLessThanOrEqual_WithAllCases_ReturnsValue(Project left, Project right, bool expected)
        {
            // Act
            var result = left <= right;

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Project.operator &gt;"/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(OperatorGreaterThanTestData))]
        public void OperatorGreaterThan_WithAllCases_ReturnsValue(Project left, Project right, bool expected)
        {
            // Act
            var result = left > right;

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Project.operator &gt;="/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(OperatorGreaterThanOrEqualTestData))]
        public void OperatorGreaterThanOrEqual_WithAllCases_ReturnsValue(Project left, Project right, bool expected)
        {
            // Act
            var result = left >= right;

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="IComparable{Project}.CompareTo"/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(CompareToTestData))]
        public void CompareTo_WithAllCases_ReturnsValue(Project left, Project right, int expected)
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
        public void CompareToObject_WithAllCases_ReturnsValue(Project left, object right, int expected)
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
                .WithMessage("Object must be of type Project. (Parameter 'obj')")
                .And.ParamName.Should().Be("obj");
        }

        /// <summary>
        /// Tests that when <see cref="IEquatable{Project}.Equals"/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(EqualsTestData))]
        public void Equals_WithAllCases_ReturnsValue(Project left, Project right, bool expected)
        {
            // Act
            var result = left.Equals(right);

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="IEquatable{Project}.Equals"/> is called with different values against an
        /// <c>object</c>, it returns the expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(EqualsTestData))]
        public void EqualsObject_WithAllCases_ReturnsValue(Project left, object right, bool expected)
        {
            // Act
            var result = left.Equals(right);

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="IEquatable{Project}.Equals"/> is called with different object types, it returns
        /// <c>false</c>.
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
        /// Tests that when <see cref="Project.GetHashCode()"/> is called with identical objects, it returns the same
        /// value each time.
        /// </summary>
        /// <param name="value1">The first value for which to compute a hash code.</param>
        /// <param name="value2">The second value for which to compute a hash code.</param>
        [Theory]
        [MemberData(nameof(GetHashCodeTestData))]
        public void GetHashCode_WithIdenticalObjects_ReturnsSameValue(Project value1, Project value2)
        {
            // Act
            var result1 = value1.GetHashCode();
            var result2 = value2.GetHashCode();

            // Assert
            _ = result1.Should().Be(result2);
        }

        /// <summary>
        /// Tests that when <see cref="Project.GetHashCode()"/> is called with different objects, it returns different
        /// values for each object.
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
        public void ToString_WithDifferentObjects_ReturnsString(Project value, string expected)
        {
            // Arrange
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            // Act
            var result = value.ToString();

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Generates the data for testing the operators, combining data from the base class with
        /// <see cref="Project"/>-specific data.
        /// </summary>
        /// <returns>The generated data.</returns>
        private static IList<ComparisonTestData<Project>> GenerateOperatorTestData()
        {
            var result =
                ComparisonDataGenerator.GenerateOperatorTestData(DefaultValue, ClonedDefaultValue, LesserValue, 5);

            result.Add(
                new ComparisonTestData<Project>(
                    DefaultValue,
                    new("Identifier", 0),
                    Comparisons.Equal));
            result.Add(
                new ComparisonTestData<Project>(
                    DefaultValue,
                    new("IDENTIFIER", 0),
                    Comparisons.Equal));
            result.Add(
                new ComparisonTestData<Project>(
                    DefaultValue,
                    new(DefaultIdentifier, 1),
                    Comparisons.Equal));
            result.Add(
                new ComparisonTestData<Project>(
                    new("ABC", 0),
                    DefaultValue,
                    Comparisons.LessThan));
            result.Add(
                new ComparisonTestData<Project>(
                    DefaultValue,
                    new("ABC", 0),
                    Comparisons.GreaterThan));
            return result;
        }

        /// <summary>
        /// Generates the data for testing <see cref="object.GetHashCode()"/>, combining data from the base class with
        /// <see cref="Project"/>-specific data.
        /// </summary>
        /// <returns>The generated data.</returns>
        private static IEnumerable<object[]> GenerateGetHashCodeTestData()
        {
            var result =
                ComparisonDataGenerator.GenerateGetHashCodeTestData(DefaultValue, ClonedDefaultValue, LesserValue, 3);

            result.Add(
                new object[]
                {
                    DefaultValue,
                    new Project("Identifier", 0),
                });
            result.Add(
                new object[]
                {
                    DefaultValue,
                    new Project("IDENTIFIER", 0),
                });
            result.Add(
                new object[]
                {
                    DefaultValue,
                    new Project(DefaultIdentifier, 1),
                });

            return result;
        }
    }
}
