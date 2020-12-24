// <copyright file="FrameworkUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Output
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using FluentAssertions;
    using NuGet.Frameworks;
    using NuGetTransitiveDependencyFinder.Output;
    using NuGetTransitiveDependencyFinder.UnitTests.Output.TestUtilities;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="Framework"/> class.
    /// </summary>
    public class FrameworkUnitTests
    {
        /// <summary>
        /// The default test identifier framework.
        /// </summary>
        private const string DefaultFramework = "Identifier";

        /// <summary>
        /// The default test identifier version.
        /// </summary>
        private static readonly Version DefaultVersion = new(1, 0);

        /// <summary>
        /// The default test identifier.
        /// </summary>
        private static readonly NuGetFramework DefaultIdentifier = new(DefaultFramework, DefaultVersion);

        /// <summary>
        /// The default collection of children.
        /// </summary>
        private static readonly IReadOnlyCollection<Dependency> DefaultChildren = Array.Empty<Dependency>();

        /// <summary>
        /// The default test value.
        /// </summary>
        private static readonly Framework DefaultValue = new(DefaultIdentifier, DefaultChildren);

        /// <summary>
        /// A clone of <see cref="DefaultValue"/>, where the object contents are identical but the object reference is
        /// not.
        /// </summary>
        private static readonly Framework ClonedDefaultValue = new(DefaultIdentifier, DefaultChildren);

        /// <summary>
        /// The lesser test value, which occurs prior to <see cref="DefaultValue"/> according to an ordered sort.
        /// </summary>
        private static readonly Framework LesserValue = new(new("ABC", DefaultVersion), DefaultChildren);

        /// <summary>
        /// The data for testing the operators.
        /// </summary>
        private static readonly IEnumerable<ComparisonTestData<Framework>> OperatorTestData =
            ComparisonDataGenerator.GenerateOperatorTestData(
                DefaultValue,
                ClonedDefaultValue,
                LesserValue,
                new ComparisonTestData<Framework>[]
                {
                    new(
                        DefaultValue,
                        new(new(DefaultFramework, DefaultVersion), DefaultChildren),
                        Comparisons.Equal),
                    new(
                        DefaultValue,
                        new(new("Identifier", DefaultVersion), DefaultChildren),
                        Comparisons.Equal),
                    new(
                        DefaultValue,
                        new(new(DefaultFramework, new(1, 0)), DefaultChildren),
                        Comparisons.Equal),
                    new(
                        DefaultValue,
                        new(new("IDENTIFIER", DefaultVersion), DefaultChildren),
                        Comparisons.Equal),
                    new(
                        DefaultValue,
                        new(DefaultIdentifier, new Dependency[] { new(DefaultFramework, new("1.0.0")) }),
                        Comparisons.Equal),
                    new(
                        new(new NuGetFramework("ABC", DefaultVersion), DefaultChildren),
                        DefaultValue,
                        Comparisons.LessThan),
                    new(
                        new(new NuGetFramework(DefaultFramework, new(0, 9)), DefaultChildren),
                        DefaultValue,
                        Comparisons.LessThan),
                    new(
                        DefaultValue,
                        new(new("ABC", DefaultVersion), DefaultChildren),
                        Comparisons.GreaterThan),
                    new(
                        DefaultValue,
                        new(new(DefaultFramework, new(0, 9)), DefaultChildren),
                        Comparisons.GreaterThan),
                });

        /// <summary>
        /// Gets the data for testing <see cref="Framework.operator =="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static TheoryData<Framework?, Framework?, bool> OperatorEqualTestData =>
            ComparisonDataGenerator.GenerateOperatorEqualTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Framework.operator !="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static TheoryData<Framework?, Framework?, bool> OperatorNotEqualTestData =>
            ComparisonDataGenerator.GenerateOperatorNotEqualTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Framework.operator &lt;"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static TheoryData<Framework?, Framework?, bool> OperatorLessThanTestData =>
            ComparisonDataGenerator.GenerateOperatorLessThanTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Framework.operator &lt;="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static TheoryData<Framework?, Framework?, bool> OperatorLessThanOrEqualTestData =>
            ComparisonDataGenerator.GenerateOperatorLessThanOrEqualTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Framework.operator &gt;"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static TheoryData<Framework?, Framework?, bool> OperatorGreaterThanTestData =>
            ComparisonDataGenerator.GenerateOperatorGreaterThanTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Framework.operator &gt;="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static TheoryData<Framework?, Framework?, bool> OperatorGreaterThanOrEqualTestData =>
            ComparisonDataGenerator.GenerateOperatorGreaterThanOrEqualTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="IComparable{Framework}.CompareTo"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static TheoryData<Framework, Framework?, int> CompareToTestData =>
            ComparisonDataGenerator.GenerateCompareToTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="IEquatable{Framework}.Equals"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static TheoryData<Framework, Framework?, bool> EqualsTestData =>
            ComparisonDataGenerator.GenerateEqualsTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Framework.GetHashCode()"/>.
        /// </summary>
        public static TheoryData<Framework, Framework> GetHashCodeTestData =>
            GenerateGetHashCodeTestData();

        /// <summary>
        /// Gets the data for testing <see cref="object.ToString()"/>.
        /// </summary>
        public static TheoryData<Framework, string> ToStringTestData =>
            new TheoryData<Framework, string>
            {
                { DefaultValue, "Identifier v1.0" },
                { LesserValue, "ABC v1.0" },
                { new(new(DefaultFramework, new(1, 0, 1)), DefaultChildren), "Identifier v1.0.1" },
                { new(new(DefaultFramework, new(1, 0, 1, 1)), DefaultChildren), "Identifier v1.0.1.1" },
                { new(new(DefaultFramework, new(1, 0, 0, 1)), DefaultChildren), "Identifier v1.0.0.1" },
            };

        /// <summary>
        /// Tests that when <see cref="Framework.operator =="/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(OperatorEqualTestData))]
        public void OperatorEqual_WithAllCases_ReturnsValue(Framework left, Framework right, bool expected)
        {
            // Act
            var result = left == right;

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Framework.operator !="/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(OperatorNotEqualTestData))]
        public void OperatorNotEqual_WithAllCases_ReturnsValue(Framework left, Framework right, bool expected)
        {
            // Act
            var result = left != right;

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Framework.operator &lt;"/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(OperatorLessThanTestData))]
        public void OperatorLessThan_WithAllCases_ReturnsValue(Framework left, Framework right, bool expected)
        {
            // Act
            var result = left < right;

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Framework.operator &lt;="/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(OperatorLessThanOrEqualTestData))]
        public void OperatorLessThanOrEqual_WithAllCases_ReturnsValue(Framework left, Framework right, bool expected)
        {
            // Act
            var result = left <= right;

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Framework.operator &gt;"/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(OperatorGreaterThanTestData))]
        public void OperatorGreaterThan_WithAllCases_ReturnsValue(Framework left, Framework right, bool expected)
        {
            // Act
            var result = left > right;

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Framework.operator &gt;="/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(OperatorGreaterThanOrEqualTestData))]
        public void OperatorGreaterThanOrEqual_WithAllCases_ReturnsValue(Framework left, Framework right, bool expected)
        {
            // Act
            var result = left >= right;

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="IComparable{Framework}.CompareTo"/> is called with different values, it returns
        /// the expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(CompareToTestData))]
        public void CompareTo_WithAllCases_ReturnsValue(Framework left, Framework right, int expected)
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
        public void CompareToObject_WithAllCases_ReturnsValue(Framework left, object right, int expected)
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
                .WithMessage("Object must be of type Framework. (Parameter 'obj')")
                .And.ParamName.Should().Be("obj");
        }

        /// <summary>
        /// Tests that when <see cref="IEquatable{Framework}.Equals"/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(EqualsTestData))]
        public void Equals_WithAllCases_ReturnsValue(Framework left, Framework right, bool expected)
        {
            // Act
            var result = left.Equals(right);

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="IEquatable{Framework}.Equals"/> is called with different values against an
        /// <c>object</c>, it returns the expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(EqualsTestData))]
        public void EqualsObject_WithAllCases_ReturnsValue(Framework left, object right, bool expected)
        {
            // Act
            var result = left.Equals(right);

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="IEquatable{Framework}.Equals"/> is called with different object types, it returns
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
        /// Tests that when <see cref="Framework.GetHashCode()"/> is called with identical objects, it returns the same
        /// value each time.
        /// </summary>
        /// <param name="value1">The first value for which to compute a hash code.</param>
        /// <param name="value2">The second value for which to compute a hash code.</param>
        [Theory]
        [MemberData(nameof(GetHashCodeTestData))]
        public void GetHashCode_WithIdenticalObjects_ReturnsSameValue(Framework value1, Framework value2)
        {
            // Act
            var result1 = value1.GetHashCode();
            var result2 = value2.GetHashCode();

            // Assert
            _ = result1.Should().Be(result2);
        }

        /// <summary>
        /// Tests that when <see cref="Framework.GetHashCode()"/> is called with different objects, it returns different
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
        public void ToString_WithDifferentObjects_ReturnsString(Framework value, string expected)
        {
            // Arrange
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            // Act
            var result = value.ToString();

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Generates the data for testing <see cref="object.GetHashCode()"/>, combining data from the base class with
        /// <see cref="Framework"/>-specific data.
        /// </summary>
        /// <returns>The generated data.</returns>
        private static TheoryData<Framework, Framework> GenerateGetHashCodeTestData()
        {
            var result =
                ComparisonDataGenerator.GenerateGetHashCodeTestData(DefaultValue, ClonedDefaultValue, LesserValue);

            result.Add(DefaultValue, new(new(DefaultFramework, DefaultVersion), DefaultChildren));
            result.Add(DefaultValue, new(new("Identifier", DefaultVersion), DefaultChildren));
            result.Add(DefaultValue, new(new(DefaultFramework, new(1, 0)), DefaultChildren));
            result.Add(DefaultValue, new(new("IDENTIFIER", DefaultVersion), DefaultChildren));
            result.Add(DefaultValue, new(DefaultIdentifier, new Dependency[] { new(DefaultFramework, new("1.0.0")) }));

            return result;
        }
    }
}
