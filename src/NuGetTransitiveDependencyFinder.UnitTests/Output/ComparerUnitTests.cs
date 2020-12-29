// <copyright file="ComparerUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Output
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using NuGetTransitiveDependencyFinder.Output;
    using NuGetTransitiveDependencyFinder.UnitTests.Output.TestUtilities;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="Comparer"/> class.
    /// </summary>
    public class ComparerUnitTests
    {
        /// <summary>
        /// The comparison logic specific to <see cref="Version"/> which takes two objects of type <see cref="Version"/>
        /// and returns an <see cref="int"/>.
        /// </summary>
        private static readonly Func<Version, Version, int> ComparisonFunction = (Version current, Version other) =>
            current.CompareTo(other);

        /// <summary>
        /// The default test value.
        /// </summary>
        private static readonly Version DefaultValue = new(1, 0, 0, 0);

        /// <summary>
        /// A clone of <see cref="DefaultValue"/>, where the object contents are identical but the object reference is
        /// not.
        /// </summary>
        private static readonly Version ClonedDefaultValue = (DefaultValue.Clone() as Version)!;

        /// <summary>
        /// The lesser test value, which occurs prior to <see cref="DefaultValue"/> according to an ordered sort.
        /// </summary>
        private static readonly Version LesserValue = new(0, 9, 9, 9);

        /// <summary>
        /// The data for testing the operators.
        /// </summary>
        private static readonly IReadOnlyCollection<ComparisonTestData<Version>> OperatorTestData =
            ComparisonDataGenerator.GenerateOperatorTestData(
                DefaultValue,
                ClonedDefaultValue,
                LesserValue,
                Array.Empty<ComparisonTestData<Version>>());

        /// <summary>
        /// Gets the data for testing <see cref="Comparer.IsEqual"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static TheoryData<Version?, Version?, bool> IsEqualTestData =>
            ComparisonDataGenerator.GenerateOperatorEqualTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Comparer.IsNotEqual"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static TheoryData<Version?, Version?, bool> IsNotEqualTestData =>
            ComparisonDataGenerator.GenerateOperatorNotEqualTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Comparer.IsLess"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static TheoryData<Version?, Version?, bool> IsLessTestData =>
            ComparisonDataGenerator.GenerateOperatorLessThanTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Comparer.IsLessOrEqual"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static TheoryData<Version?, Version?, bool> IsLessOrEqualTestData =>
            ComparisonDataGenerator.GenerateOperatorLessThanOrEqualTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Comparer.IsGreater"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static TheoryData<Version?, Version?, bool> IsGreaterTestData =>
            ComparisonDataGenerator.GenerateOperatorGreaterThanTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Comparer.IsGreaterOrEqual"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static TheoryData<Version?, Version?, bool> IsGreaterOrEqualTestData =>
            ComparisonDataGenerator.GenerateOperatorGreaterThanOrEqualTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing
        /// <see cref="Comparer.CompareTo{TValue}(TValue, TValue, Func{TValue, TValue, int})"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static TheoryData<Version, Version?, int> CompareToTestData =>
            ComparisonDataGenerator.GenerateCompareToTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="IEquatable{Version}.Equals"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static TheoryData<Version, Version?, bool> EqualsTestData =>
            ComparisonDataGenerator.GenerateEqualsTestData(OperatorTestData);

        /// <summary>
        /// Tests that when <see cref="Comparer.IsEqual"/> is called with different values, it returns the expected
        /// value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(IsEqualTestData))]
        public void IsEqual_WithAllCases_ReturnsValue(Version left, Version right, bool expected)
        {
            // Act
            var result = Comparer.IsEqual(left, right, ComparisonFunction);

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Comparer.IsNotEqual"/> is called with different values, it returns the expected
        /// value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(IsNotEqualTestData))]
        public void IsNotEqual_WithAllCases_ReturnsValue(Version left, Version right, bool expected)
        {
            // Act
            var result = Comparer.IsNotEqual(left, right, ComparisonFunction);

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Comparer.IsLess"/> is called with different values, it returns the expected value
        /// in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(IsLessTestData))]
        public void IsLess_WithAllCases_ReturnsValue(Version left, Version right, bool expected)
        {
            // Act
            var result = Comparer.IsLess(left, right, ComparisonFunction);

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Comparer.IsLessOrEqual"/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(IsLessOrEqualTestData))]
        public void IsLessOrEqual_WithAllCases_ReturnsValue(Version left, Version right, bool expected)
        {
            // Act
            var result = Comparer.IsLessOrEqual(left, right, ComparisonFunction);

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Comparer.IsGreater"/> is called with different values, it returns the expected
        /// value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(IsGreaterTestData))]
        public void IsGreater_WithAllCases_ReturnsValue(Version left, Version right, bool expected)
        {
            // Act
            var result = Comparer.IsGreater(left, right, ComparisonFunction);

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Comparer.IsGreaterOrEqual"/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(IsGreaterOrEqualTestData))]
        public void IsGreaterOrEqual_WithAllCases_ReturnsValue(Version left, Version right, bool expected)
        {
            // Act
            var result = Comparer.IsGreaterOrEqual(left, right, ComparisonFunction);

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Comparer.CompareTo{TValue}(TValue, TValue, Func{TValue, TValue, int})"/> is
        /// called with different values, it returns the expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(CompareToTestData))]
        public void CompareTo_WithAllCases_ReturnsValue(Version left, Version right, int expected)
        {
            // Act
            var result = Comparer.CompareTo(left, right, ComparisonFunction);

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Comparer.CompareTo{TValue}(TValue, object, Func{TValue, TValue, int}, string)"/>
        /// is called with different values against an <c>object</c>, it returns the expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(CompareToTestData))]
        public void CompareToObject_WithAllCases_ReturnsValue(Version left, object right, int expected)
        {
            // Act
            var result = Comparer.CompareTo(left, right, ComparisonFunction, nameof(Version));

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Comparer.CompareTo{TValue}(TValue, object, Func{TValue, TValue, int}, string)"/>
        /// is called with different object types, it throws an <see cref="ArgumentException"/>.
        /// </summary>
        [Fact]
        public void CompareToObject_WithDifferentObjectTypes_ThrowsArgumentException()
        {
            // Act
            Action action = () => Comparer.CompareTo(DefaultValue, "value", ComparisonFunction, nameof(Version));

            // Assert
            _ = action.Should().Throw<ArgumentException>()
                .WithMessage("Object must be of type Version. (Parameter 'obj')")
                .And.ParamName.Should().Be("obj");
        }

        /// <summary>]
        /// Tests that when <see cref="Comparer.Equals{TValue}(TValue, TValue, Func{TValue, TValue, int})"/> is called
        /// with different values, it returns the expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(EqualsTestData))]
        public void Equals_WithAllCases_ReturnsValue(Version left, Version right, bool expected)
        {
            // Act
            var result = Comparer.Equals(left, right, ComparisonFunction);

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Comparer.Equals{TValue}(TValue, object, Func{TValue, TValue, int})"/> is called
        /// with different values against an <c>object</c>, it returns the expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [MemberData(nameof(EqualsTestData))]
        public void EqualsObject_WithAllCases_ReturnsValue(Version left, object right, bool expected)
        {
            // Act
            var result = Comparer.Equals(left, right, ComparisonFunction);

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Comparer.Equals{TValue}(TValue, object, Func{TValue, TValue, int})"/> is called
        /// with different object types, it returns <c>false</c>.
        /// </summary>
        [Fact]
        public void EqualsObject_WithDifferentObjectTypes_ReturnsFalse()
        {
            // Act
            var result = Comparer.Equals(DefaultValue, "value", ComparisonFunction);

            // Assert
            _ = result.Should().Be(false);
        }
    }
}
