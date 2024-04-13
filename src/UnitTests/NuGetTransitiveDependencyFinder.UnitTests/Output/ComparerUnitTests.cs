// <copyright file="ComparerUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Output;

using System;
using System.Collections.Generic;
using FluentAssertions;
using NuGetTransitiveDependencyFinder.Output;
using NuGetTransitiveDependencyFinder.UnitTests.Output.Serialization;
using NuGetTransitiveDependencyFinder.UnitTests.Output.UnitTests.Utilities;
using NuGetTransitiveDependencyFinder.UnitTests.Utilities.Globalization;
using Xunit;

/// <summary>
/// Unit tests for the <see cref="Comparer"/> class.
/// </summary>
public class ComparerUnitTests
{
    /// <summary>
    /// The comparison logic specific to <see cref="Version"/>, which takes two objects of type <see cref="Version"/>
    /// and returns an <see cref="int"/>.
    /// </summary>
    private static readonly Func<Version, Version, int> ComparisonFunction = (Version current, Version other) =>
        current.CompareTo(other);

    /// <summary>
    /// The default test value.
    /// </summary>
    private static readonly SerializedVersion DefaultValue = new(new(1, 0, 0, 0));

    /// <summary>
    /// A clone of <see cref="DefaultValue"/>, where the object content is identical, but the object reference is not.
    /// </summary>
    private static readonly SerializedVersion ClonedDefaultValue = new((DefaultValue.Version.Clone() as Version)!);

    /// <summary>
    /// The lesser test value, which occurs prior to <see cref="DefaultValue"/> according to an ordered sort.
    /// </summary>
    private static readonly SerializedVersion LesserValue = new(new(0, 9, 9, 9));

    /// <summary>
    /// The data for testing the operators.
    /// </summary>
    private static readonly IReadOnlyCollection<ComparisonTestData<SerializedVersion>> OperatorTestData =
        ComparisonDataGenerator.GenerateOperatorTestData(
            DefaultValue,
            ClonedDefaultValue,
            LesserValue,
            []);

    /// <summary>
    /// Gets the data for testing <see cref="Comparer.IsEqual{TValue}(TValue, TValue, Func{TValue, TValue, int})"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<SerializedVersion?, SerializedVersion?, bool> IsEqualTestData =>
        ComparisonDataGenerator.GenerateOperatorEqualTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="Comparer.IsNotEqual{TValue}(TValue, TValue, Func{TValue, TValue, int})"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<SerializedVersion?, SerializedVersion?, bool> IsNotEqualTestData =>
        ComparisonDataGenerator.GenerateOperatorNotEqualTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="Comparer.IsLess{TValue}(TValue, TValue, Func{TValue, TValue, int})"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<SerializedVersion?, SerializedVersion?, bool> IsLessTestData =>
        ComparisonDataGenerator.GenerateOperatorLessThanTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing
    /// <see cref="Comparer.IsLessOrEqual{TValue}(TValue, TValue, Func{TValue, TValue, int})"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<SerializedVersion?, SerializedVersion?, bool> IsLessOrEqualTestData =>
        ComparisonDataGenerator.GenerateOperatorLessThanOrEqualTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="Comparer.IsGreater{TValue}(TValue, TValue, Func{TValue, TValue, int})"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<SerializedVersion?, SerializedVersion?, bool> IsGreaterTestData =>
        ComparisonDataGenerator.GenerateOperatorGreaterThanTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing
    /// <see cref="Comparer.IsGreaterOrEqual{TValue}(TValue, TValue, Func{TValue, TValue, int})"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<SerializedVersion?, SerializedVersion?, bool> IsGreaterOrEqualTestData =>
        ComparisonDataGenerator.GenerateOperatorGreaterThanOrEqualTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="Comparer.CompareTo{TValue}(TValue, TValue, Func{TValue, TValue, int})"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<SerializedVersion, SerializedVersion?, int> CompareToTestData =>
        ComparisonDataGenerator.GenerateCompareToTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="IEquatable{Version}.Equals(Version)"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<SerializedVersion, SerializedVersion?, bool> EqualsTestData =>
        ComparisonDataGenerator.GenerateEqualsTestData(OperatorTestData);

    /// <summary>
    /// Tests that when <see cref="Comparer.IsEqual{TValue}(TValue, TValue, Func{TValue, TValue, int})"/> is called with
    /// different values, it returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(IsEqualTestData))]
    public void IsEqual_WithAllCases_ReturnsValue(SerializedVersion? left, SerializedVersion? right, bool expected)
    {
        // Act
        var result = Comparer.IsEqual(left?.Version, right?.Version, ComparisonFunction);

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="Comparer.IsNotEqual{TValue}(TValue, TValue, Func{TValue, TValue, int})"/> is called
    /// with different values, it returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(IsNotEqualTestData))]
    public void IsNotEqual_WithAllCases_ReturnsValue(SerializedVersion? left, SerializedVersion? right, bool expected)
    {
        // Act
        var result = Comparer.IsNotEqual(left?.Version, right?.Version, ComparisonFunction);

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="Comparer.IsLess{TValue}(TValue, TValue, Func{TValue, TValue, int})"/> is called with
    /// different values, it returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(IsLessTestData))]
    public void IsLess_WithAllCases_ReturnsValue(SerializedVersion? left, SerializedVersion? right, bool expected)
    {
        // Act
        var result = Comparer.IsLess(left?.Version, right?.Version, ComparisonFunction);

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="Comparer.IsLessOrEqual{TValue}(TValue, TValue, Func{TValue, TValue, int})"/> is
    /// called with different values, it returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(IsLessOrEqualTestData))]
    public void IsLessOrEqual_WithAllCases_ReturnsValue(SerializedVersion? left, SerializedVersion? right, bool expected)
    {
        // Act
        var result = Comparer.IsLessOrEqual(left?.Version, right?.Version, ComparisonFunction);

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="Comparer.IsGreater{TValue}(TValue, TValue, Func{TValue, TValue, int})"/> is called
    /// with different values, it returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(IsGreaterTestData))]
    public void IsGreater_WithAllCases_ReturnsValue(SerializedVersion? left, SerializedVersion? right, bool expected)
    {
        // Act
        var result = Comparer.IsGreater(left?.Version, right?.Version, ComparisonFunction);

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="Comparer.IsGreaterOrEqual{TValue}(TValue, TValue, Func{TValue, TValue, int})"/> is
    /// called with different values, it returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(IsGreaterOrEqualTestData))]
    public void IsGreaterOrEqual_WithAllCases_ReturnsValue(SerializedVersion? left, SerializedVersion? right, bool expected)
    {
        // Act
        var result = Comparer.IsGreaterOrEqual(left?.Version, right?.Version, ComparisonFunction);

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="Comparer.CompareTo{TValue}(TValue, TValue, Func{TValue, TValue, int})"/> is called
    /// with different values, it returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(CompareToTestData))]
    public void CompareTo_WithAllCases_ReturnsValue(SerializedVersion left, SerializedVersion? right, int expected)
    {
        // Act
        var result = Comparer.CompareTo(left.Version, right?.Version, ComparisonFunction);

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="Comparer.CompareTo{TValue}(TValue, object, Func{TValue, TValue, int}, string)"/> is
    /// called with different values against an <see langword="object"/>, it returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(CompareToTestData))]
    public void CompareToObject_WithAllCases_ReturnsValue(SerializedVersion left, SerializedVersion? right, int expected)
    {
        // Act
        var result = Comparer.CompareTo(left.Version, right?.Version, ComparisonFunction, nameof(Version));

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="Comparer.CompareTo{TValue}(TValue, object, Func{TValue, TValue, int}, string)"/> is
    /// called with different object types, it throws an <see cref="ArgumentException"/>.
    /// </summary>
    [AllCulturesFact]
    public void CompareToObject_WithDifferentObjectTypes_ThrowsArgumentException()
    {
        // Act
        Action action = () => Comparer.CompareTo(DefaultValue.Version, "value", ComparisonFunction, nameof(Version));

        // Assert
        _ = action
            .Should().Throw<ArgumentException>().WithMessage("Object must be of type Version. (Parameter 'obj')")
            .And.ParamName.Should().Be("obj");
    }

    /// <summary>
    /// Tests that when <see cref="Comparer.Equals{TValue}(TValue, TValue, Func{TValue, TValue, int})"/> is called with
    /// different values, it returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(EqualsTestData))]
    public void Equals_WithAllCases_ReturnsValue(SerializedVersion left, SerializedVersion? right, bool expected)
    {
        // Act
        var result = Comparer.Equals(left.Version, right?.Version, ComparisonFunction);

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="Comparer.Equals{TValue}(TValue, object, Func{TValue, TValue, int})"/> is called with
    /// different values against an <see langword="object"/>, it returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(EqualsTestData))]
    public void EqualsObject_WithAllCases_ReturnsValue(SerializedVersion left, SerializedVersion? right, bool expected)
    {
        // Act
        var result = Comparer.Equals(left.Version, right?.Version as object, ComparisonFunction);

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="Comparer.Equals{TValue}(TValue, object, Func{TValue, TValue, int})"/> is called with
    /// different object types, it returns <see langword="false"/>.
    /// </summary>
    [AllCulturesFact]
    public void EqualsObject_WithDifferentObjectTypes_ReturnsFalse()
    {
        // Act
        var result = Comparer.Equals(DefaultValue.Version, "value", ComparisonFunction);

        // Assert
        _ = result
            .Should().BeFalse();
    }
}
