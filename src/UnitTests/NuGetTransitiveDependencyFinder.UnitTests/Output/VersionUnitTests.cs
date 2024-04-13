// <copyright file="VersionUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Output;

using System;
using System.Collections.Generic;
using FluentAssertions;
using NuGetTransitiveDependencyFinder.UnitTests.Output.Serialization;
using NuGetTransitiveDependencyFinder.UnitTests.Output.UnitTests.Utilities;
using NuGetTransitiveDependencyFinder.UnitTests.Utilities.Globalization;
using Xunit;

/// <summary>
/// Unit tests for the <see cref="Version"/> class.
/// </summary>
/// <remarks>This class does not unit test functionality from the associated library. Instead, it is used to ensure that
/// the behavior of the library classes is identical to that of the classes provided as part of .NET.</remarks>
public class VersionUnitTests
{
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
    /// Gets the data for testing <see cref="Version.operator ==(Version?, Version?)"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<SerializedVersion?, SerializedVersion?, bool> OperatorEqualTestData =>
        ComparisonDataGenerator.GenerateOperatorEqualTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="Version.operator !=(Version?, Version?)"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<SerializedVersion?, SerializedVersion?, bool> OperatorNotEqualTestData =>
        ComparisonDataGenerator.GenerateOperatorNotEqualTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="Version.operator &lt;(Version?, Version?)"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<SerializedVersion?, SerializedVersion?, bool> OperatorLessThanTestData =>
        ComparisonDataGenerator.GenerateOperatorLessThanTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="Version.operator &lt;=(Version?, Version?)"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<SerializedVersion?, SerializedVersion?, bool> OperatorLessThanOrEqualTestData =>
        ComparisonDataGenerator.GenerateOperatorLessThanOrEqualTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="Version.operator &gt;(Version?, Version?)"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<SerializedVersion?, SerializedVersion?, bool> OperatorGreaterThanTestData =>
        ComparisonDataGenerator.GenerateOperatorGreaterThanTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="Version.operator &gt;=(Version?, Version?)"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<SerializedVersion?, SerializedVersion?, bool> OperatorGreaterThanOrEqualTestData =>
        ComparisonDataGenerator.GenerateOperatorGreaterThanOrEqualTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="IComparable{Version}.CompareTo(Version)"/>.
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
    /// Gets the data for testing <see cref="Version.GetHashCode()"/>.
    /// </summary>
    public static TheoryData<SerializedVersion, SerializedVersion> GetHashCodeTestData =>
        ComparisonDataGenerator.GenerateGetHashCodeTestData(DefaultValue, ClonedDefaultValue, LesserValue, []);

    /// <summary>
    /// Tests that when <see cref="Version.operator ==(Version?, Version?)"/> is called with different values, it
    /// returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(OperatorEqualTestData))]
    public void OperatorEqual_WithAllCases_ReturnsValue(
        SerializedVersion? left,
        SerializedVersion? right,
        bool expected)
    {
        // Act
        var result = left?.Version == right?.Version;

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="Version.operator !=(Version?, Version?)"/> is called with different values, it
    /// returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(OperatorNotEqualTestData))]
    public void OperatorNotEqual_WithAllCases_ReturnsValue(
        SerializedVersion? left,
        SerializedVersion? right,
        bool expected)
    {
        // Act
        var result = left?.Version != right?.Version;

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="Version.operator &lt;(Version?, Version?)"/> is called with different values, it
    /// returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(OperatorLessThanTestData))]
    public void OperatorLessThan_WithAllCases_ReturnsValue(
        SerializedVersion? left,
        SerializedVersion? right,
        bool expected)
    {
        // Act
        var result = left?.Version < right?.Version;

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="Version.operator &lt;=(Version?, Version?)"/> is called with different values, it
    /// returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(OperatorLessThanOrEqualTestData))]
    public void OperatorLessThanOrEqual_WithAllCases_ReturnsValue(
        SerializedVersion? left,
        SerializedVersion? right,
        bool expected)
    {
        // Act
        var result = left?.Version <= right?.Version;

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="Version.operator &gt;(Version?, Version?)"/> is called with different values, it
    /// returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(OperatorGreaterThanTestData))]
    public void OperatorGreaterThan_WithAllCases_ReturnsValue(
        SerializedVersion? left,
        SerializedVersion? right,
        bool expected)
    {
        // Act
        var result = left?.Version > right?.Version;

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="Version.operator &gt;=(Version?, Version?)"/> is called with different values, it
    /// returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(OperatorGreaterThanOrEqualTestData))]
    public void OperatorGreaterThanOrEqual_WithAllCases_ReturnsValue(
        SerializedVersion? left,
        SerializedVersion? right,
        bool expected)
    {
        // Act
        var result = left?.Version >= right?.Version;

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="IComparable{Version}.CompareTo(Version)"/> is called with different values, it
    /// returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(CompareToTestData))]
    public void CompareTo_WithAllCases_ReturnsValue(SerializedVersion left, SerializedVersion? right, int expected)
    {
        // Act
        var result = left.Version.CompareTo(right?.Version);

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="IComparable.CompareTo(object?)"/> is called with different values against an
    /// <see langword="object"/>, it returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(CompareToTestData))]
    public void CompareToObject_WithAllCases_ReturnsValue(
        SerializedVersion left,
        SerializedVersion? right,
        int expected)
    {
        // Act
        var result = left.Version.CompareTo(right?.Version as object);

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="IComparable.CompareTo(object?)"/> is called with different object types, it throws an
    /// <see cref="ArgumentException"/>.
    /// </summary>
    [AllCulturesFact]
    public void CompareToObject_WithDifferentObjectTypes_ThrowsArgumentException()
    {
        // Act
        Action action = () => DefaultValue.Version.CompareTo("value");

        // Assert
        _ = action
            .Should().Throw<ArgumentException>().WithMessage("Object must be of type Version. (Parameter 'version')");
    }

    /// <summary>
    /// Tests that when <see cref="IEquatable{Version}.Equals(Version)"/> is called with different values, it returns
    /// the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(EqualsTestData))]
    public void Equals_WithAllCases_ReturnsValue(SerializedVersion left, SerializedVersion? right, bool expected)
    {
        // Act
        var result = left.Version.Equals(right?.Version);

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="IEquatable{Version}.Equals(Version)"/> is called with different values against an
    /// <see langword="object"/>, it returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(EqualsTestData))]
    public void EqualsObject_WithAllCases_ReturnsValue(SerializedVersion left, SerializedVersion? right, bool expected)
    {
        // Act
        var result = left.Version.Equals(right?.Version as object);

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="IEquatable{Version}.Equals(Version)"/> is called with different object types, it
    /// returns <see langword="false"/>.
    /// </summary>
    [AllCulturesFact]
    public void EqualsObject_WithDifferentObjectTypes_ReturnsFalse()
    {
        // Act
        var result = DefaultValue.Version.Equals("value");

        // Assert
        _ = result
            .Should().BeFalse();
    }

    /// <summary>
    /// Tests that when <see cref="Version.GetHashCode()"/> is called with identical objects, it returns the same value
    /// each time.
    /// </summary>
    /// <param name="value1">The first value for which to compute a hash code.</param>
    /// <param name="value2">The second value for which to compute a hash code.</param>
    [AllCulturesTheory]
    [MemberData(nameof(GetHashCodeTestData))]
    public void GetHashCode_WithIdenticalObjects_ReturnsSameValue(SerializedVersion value1, SerializedVersion value2)
    {
        // Act
        var result1 = value1.Version.GetHashCode();
        var result2 = value2.Version.GetHashCode();

        // Assert
        _ = result1
            .Should().Be(result2);
    }

    /// <summary>
    /// Tests that when <see cref="Version.GetHashCode()"/> is called with different objects, it returns different
    /// values for each object.
    /// </summary>
    [AllCulturesFact]
    public void GetHashCode_WithDifferentObjects_ReturnsDifferentValues()
    {
        // Act
        var result1 = DefaultValue.Version.GetHashCode();
        var result2 = LesserValue.Version.GetHashCode();

        // Assert
        _ = result1
            .Should().NotBe(result2);
    }
}
