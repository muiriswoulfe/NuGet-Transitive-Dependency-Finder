// <copyright file="FrameworkUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Output;

using System;
using System.Collections.Generic;
using FluentAssertions;
using NuGet.Frameworks;
using NuGetTransitiveDependencyFinder.Output;
using NuGetTransitiveDependencyFinder.UnitTests.Output.Serialization;
using NuGetTransitiveDependencyFinder.UnitTests.Output.UnitTests.Utilities;
using NuGetTransitiveDependencyFinder.UnitTests.Utilities.Globalization;
using Xunit;

/// <summary>
/// Unit tests for the <see cref="Framework"/> class.
/// </summary>
public class FrameworkUnitTests
{
    /// <summary>
    /// The default test identifier framework.
    /// </summary>
    private const string DefaultIdentifierFramework = "Identifier";

    /// <summary>
    /// The default test identifier version.
    /// </summary>
    private static readonly Version DefaultIdentifierVersion = new(1, 0);

    /// <summary>
    /// The default test identifier.
    /// </summary>
    private static readonly NuGetFramework DefaultIdentifier =
        new(DefaultIdentifierFramework, DefaultIdentifierVersion);

    /// <summary>
    /// The default collection of children.
    /// </summary>
    private static readonly IReadOnlyCollection<Dependency> DefaultChildren = [];

    /// <summary>
    /// The default test value.
    /// </summary>
    private static readonly SerializedFramework DefaultValue = new(new(DefaultIdentifier, DefaultChildren));

    /// <summary>
    /// A clone of <see cref="DefaultValue"/>, where the object content is identical, but the object reference is not.
    /// </summary>
    private static readonly SerializedFramework ClonedDefaultValue = new(new(DefaultIdentifier, DefaultChildren));

    /// <summary>
    /// The lesser test value, which occurs prior to <see cref="DefaultValue"/> according to an ordered sort.
    /// </summary>
    private static readonly SerializedFramework LesserValue =
        new(new(new("ABC", DefaultIdentifierVersion), DefaultChildren));

    /// <summary>
    /// The data for testing the operators.
    /// </summary>
    private static readonly IReadOnlyCollection<ComparisonTestData<SerializedFramework>> OperatorTestData =
        ComparisonDataGenerator.GenerateOperatorTestData(
            DefaultValue,
            ClonedDefaultValue,
            LesserValue,
            [
                new(
                    DefaultValue,
                    new(new(new(DefaultIdentifierFramework, DefaultIdentifierVersion), DefaultChildren)),
                    Comparisons.Equal),
                new(
                    DefaultValue,
                    new(new(new("Identifier", DefaultIdentifierVersion), DefaultChildren)),
                    Comparisons.Equal),
                new(
                    DefaultValue,
                    new(new(new(DefaultIdentifierFramework, new(1, 0)), DefaultChildren)),
                    Comparisons.Equal),
                new(
                    DefaultValue,
                    new(new(new("IDENTIFIER", DefaultIdentifierVersion), DefaultChildren)),
                    Comparisons.Equal),
                new(
                    DefaultValue,
                    new(new(DefaultIdentifier, [new(DefaultIdentifierFramework, new("1.0.0"))])),
                    Comparisons.Equal),
                new(
                    new(new(new NuGetFramework("ABC", DefaultIdentifierVersion), DefaultChildren)),
                    DefaultValue,
                    Comparisons.LessThan),
                new(
                    new(new(new NuGetFramework(DefaultIdentifierFramework, new(0, 9)), DefaultChildren)),
                    DefaultValue,
                    Comparisons.LessThan),
                new(
                    DefaultValue,
                    new(new(new("ABC", DefaultIdentifierVersion), DefaultChildren)),
                    Comparisons.GreaterThan),
                new(
                    DefaultValue,
                    new(new(new(DefaultIdentifierFramework, new(0, 9)), DefaultChildren)),
                    Comparisons.GreaterThan),
            ]);

    /// <summary>
    /// The data for testing <see cref="Base{Dependency}.SortedChildren"/>.
    /// </summary>
    private static readonly IReadOnlyList<Dependency> SortedChildrenTestData =
        [
            new("A", new("0.9.9")),
            new("A", new("1.0.0")),
            new("B", new("1.0.0")),
            new("C", new("1.0.0")),
            new("Y", new("1.0.0")),
            new("Z", new("1.0.0")),
        ];

    /// <summary>
    /// Gets the data for testing <see cref="Framework.operator ==(Framework?, Framework?)"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<SerializedFramework?, SerializedFramework?, bool> OperatorEqualTestData =>
        ComparisonDataGenerator.GenerateOperatorEqualTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="Framework.operator !=(Framework?, Framework?)"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<SerializedFramework?, SerializedFramework?, bool> OperatorNotEqualTestData =>
        ComparisonDataGenerator.GenerateOperatorNotEqualTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="Framework.operator &lt;(Framework?, Framework?)"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<SerializedFramework?, SerializedFramework?, bool> OperatorLessThanTestData =>
        ComparisonDataGenerator.GenerateOperatorLessThanTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="Framework.operator &lt;=(Framework?, Framework?)"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<SerializedFramework?, SerializedFramework?, bool> OperatorLessThanOrEqualTestData =>
        ComparisonDataGenerator.GenerateOperatorLessThanOrEqualTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="Framework.operator &gt;(Framework?, Framework?)"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<SerializedFramework?, SerializedFramework?, bool> OperatorGreaterThanTestData =>
        ComparisonDataGenerator.GenerateOperatorGreaterThanTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="Framework.operator &gt;=(Framework?, Framework?)"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<SerializedFramework?, SerializedFramework?, bool> OperatorGreaterThanOrEqualTestData =>
        ComparisonDataGenerator.GenerateOperatorGreaterThanOrEqualTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="IComparable{Framework}.CompareTo(Framework)"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<SerializedFramework, SerializedFramework?, int> CompareToTestData =>
        ComparisonDataGenerator.GenerateCompareToTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="IEquatable{Framework}.Equals(Framework)"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<SerializedFramework, SerializedFramework?, bool> EqualsTestData =>
        ComparisonDataGenerator.GenerateEqualsTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="Framework.GetHashCode()"/>.
    /// </summary>
    public static TheoryData<SerializedFramework, SerializedFramework> GetHashCodeTestData =>
        ComparisonDataGenerator.GenerateGetHashCodeTestData(
            DefaultValue,
            ClonedDefaultValue,
            LesserValue,
            new TheoryData<SerializedFramework, SerializedFramework>
            {
                {
                    DefaultValue,
                    new(new(new(DefaultIdentifierFramework, DefaultIdentifierVersion), DefaultChildren))
                },
                {
                    DefaultValue,
                    new(new(new("Identifier", DefaultIdentifierVersion), DefaultChildren))
                },
                {
                    DefaultValue,
                    new(new(new(DefaultIdentifierFramework, new(1, 0)), DefaultChildren))
                },
                {
                    DefaultValue,
                    new(new(new("IDENTIFIER", DefaultIdentifierVersion), DefaultChildren))
                },
                {
                    DefaultValue,
                    new(new(DefaultIdentifier, [new(DefaultIdentifierFramework, new("1.0.0"))]))
                },
            });

    /// <summary>
    /// Gets the data for testing <see cref="object.ToString()"/>.
    /// </summary>
    public static TheoryData<SerializedFramework, string> ToStringTestData =>
        new()
        {
            { DefaultValue, "Identifier v1.0" },
            { LesserValue, "ABC v1.0" },
            { new(new(new(DefaultIdentifierFramework, new(1, 0, 1)), DefaultChildren)), "Identifier v1.0.1" },
            { new(new(new(DefaultIdentifierFramework, new(1, 0, 1, 1)), DefaultChildren)), "Identifier v1.0.1.1" },
            { new(new(new(DefaultIdentifierFramework, new(1, 0, 0, 1)), DefaultChildren)), "Identifier v1.0.0.1" },
        };

    /// <summary>
    /// Tests that when <see cref="Framework.operator ==(Framework?, Framework?)"/> is called with different values, it
    /// returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(OperatorEqualTestData))]
    public void OperatorEqual_WithAllCases_ReturnsValue(
        SerializedFramework? left,
        SerializedFramework? right,
        bool expected)
    {
        // Act
        var result = left?.Framework == right?.Framework;

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="Framework.operator !=(Framework?, Framework?)"/> is called with different values, it
    /// returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(OperatorNotEqualTestData))]
    public void OperatorNotEqual_WithAllCases_ReturnsValue(
        SerializedFramework? left,
        SerializedFramework? right,
        bool expected)
    {
        // Act
        var result = left?.Framework != right?.Framework;

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="Framework.operator &lt;(Framework?, Framework?)"/> is called with different values,
    /// it returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(OperatorLessThanTestData))]
    public void OperatorLessThan_WithAllCases_ReturnsValue(
        SerializedFramework? left,
        SerializedFramework? right,
        bool expected)
    {
        // Act
        var result = left?.Framework < right?.Framework;

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="Framework.operator &lt;=(Framework?, Framework?)"/> is called with different values,
    /// it returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(OperatorLessThanOrEqualTestData))]
    public void OperatorLessThanOrEqual_WithAllCases_ReturnsValue(
        SerializedFramework? left,
        SerializedFramework? right,
        bool expected)
    {
        // Act
        var result = left?.Framework <= right?.Framework;

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="Framework.operator &gt;(Framework?, Framework?)"/> is called with different values,
    /// it returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(OperatorGreaterThanTestData))]
    public void OperatorGreaterThan_WithAllCases_ReturnsValue(
        SerializedFramework? left,
        SerializedFramework? right,
        bool expected)
    {
        // Act
        var result = left?.Framework > right?.Framework;

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="Framework.operator &gt;=(Framework?, Framework?)"/> is called with different values,
    /// it returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(OperatorGreaterThanOrEqualTestData))]
    public void OperatorGreaterThanOrEqual_WithAllCases_ReturnsValue(
        SerializedFramework? left,
        SerializedFramework? right,
        bool expected)
    {
        // Act
        var result = left?.Framework >= right?.Framework;

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="IComparable{Framework}.CompareTo(Framework)"/> is called with different values, it
    /// returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(CompareToTestData))]
    public void CompareTo_WithAllCases_ReturnsValue(SerializedFramework left, SerializedFramework? right, int expected)
    {
        // Act
        var result = left.Framework.CompareTo(right?.Framework);

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
        SerializedFramework left,
        SerializedFramework? right,
        int expected)
    {
        // Act
        var result = left.Framework.CompareTo(right?.Framework as object);

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
        Action action = () => DefaultValue.Framework.CompareTo("value");

        // Assert
        _ = action
            .Should().Throw<ArgumentException>().WithMessage("Object must be of type Framework. (Parameter 'obj')")
            .And.ParamName.Should().Be("obj");
    }

    /// <summary>
    /// Tests that when <see cref="IEquatable{Framework}.Equals(Framework)"/> is called with different values, it
    /// returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(EqualsTestData))]
    public void Equals_WithAllCases_ReturnsValue(SerializedFramework left, SerializedFramework? right, bool expected)
    {
        // Act
        var result = left.Framework.Equals(right?.Framework);

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="IEquatable{Framework}.Equals(Framework)"/> is called with different values against an
    /// <see langword="object"/>, it returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(EqualsTestData))]
    public void EqualsObject_WithAllCases_ReturnsValue(
        SerializedFramework left,
        SerializedFramework? right,
        bool expected)
    {
        // Act
        var result = left.Framework.Equals(right?.Framework as object);

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="IEquatable{Framework}.Equals(Framework)"/> is called with different object types, it
    /// returns <see langword="false"/>.
    /// </summary>
    [AllCulturesFact]
    public void EqualsObject_WithDifferentObjectTypes_ReturnsFalse()
    {
        // Act
        var result = DefaultValue.Framework.Equals("value");

        // Assert
        _ = result
            .Should().BeFalse();
    }

    /// <summary>
    /// Tests that when <see cref="Framework.GetHashCode()"/> is called with identical objects, it returns the same
    /// value each time.
    /// </summary>
    /// <param name="value1">The first value for which to compute a hash code.</param>
    /// <param name="value2">The second value for which to compute a hash code.</param>
    [AllCulturesTheory]
    [MemberData(nameof(GetHashCodeTestData))]
    public void GetHashCode_WithIdenticalObjects_ReturnsSameValue(
        SerializedFramework value1,
        SerializedFramework value2)
    {
        // Act
        var result1 = value1.Framework.GetHashCode();
        var result2 = value2.Framework.GetHashCode();

        // Assert
        _ = result1
            .Should().Be(result2);
    }

    /// <summary>
    /// Tests that when <see cref="Framework.GetHashCode()"/> is called with different objects, it returns different
    /// values for each object.
    /// </summary>
    [AllCulturesFact]
    public void GetHashCode_WithDifferentObjects_ReturnsDifferentValues()
    {
        // Act
        var result1 = DefaultValue.Framework.GetHashCode();
        var result2 = LesserValue.Framework.GetHashCode();

        // Assert
        _ = result1
            .Should().NotBe(result2);
    }

    /// <summary>
    /// Tests that when <see cref="object.ToString()"/> is called with different objects, it returns different strings
    /// for each object.
    /// </summary>
    /// <param name="value">The value to be converted to a string.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(ToStringTestData))]
    public void ToString_WithDifferentObjects_ReturnsString(SerializedFramework value, string expected)
    {
        // Act
        var result = value.Framework.ToString();

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests than when <see cref="Framework.IsAddValid(Dependency?)"/> is called with any value, it returns
    /// <see langword="true"/>.
    /// </summary>
    [AllCulturesFact]
    public void IsAddValid_WithAnyValue_ReturnsTrue()
    {
        // Act
        var result = DefaultValue.Framework.IsAddValid(new(DefaultIdentifierFramework, new("1.0.0")));

        // Assert
        _ = result
            .Should().BeTrue();
    }

    /// <summary>
    /// Tests that when <see cref="Base{Dependency}.HasChildren"/> is called for a <see cref="Framework"/> object not
    /// comprising children, it returns <see langword="false"/>.
    /// </summary>
    [AllCulturesFact]
    public void HasChildren_NotComprisingChildren_ReturnsFalse()
    {
        // Act
        var result = DefaultValue.Framework.HasChildren;

        // Assert
        _ = result
            .Should().BeFalse();
    }

    /// <summary>
    /// Tests that when <see cref="Base{Dependency}.HasChildren"/> is called for a <see cref="Framework"/> object
    /// comprising children added during construction, it returns <see langword="true"/>.
    /// </summary>
    [AllCulturesFact]
    public void HasChildren_ComprisingChildrenAddedDuringConstruction_ReturnsTrue()
    {
        // Arrange
        var framework = new Framework(
            DefaultIdentifier,
            [new(DefaultIdentifierFramework, new("1.0.0"))]);

        // Act
        var result = framework.HasChildren;

        // Assert
        _ = result
            .Should().BeTrue();
    }

    /// <summary>
    /// Tests that when <see cref="Base{Dependency}.HasChildren"/> is called for a <see cref="Framework"/> object
    /// comprising children added after construction, it returns <see langword="true"/>.
    /// </summary>
    [AllCulturesFact]
    public void HasChildren_ComprisingChildrenAddedAfterConstruction_ReturnsTrue()
    {
        // Arrange
        var framework = new Framework(DefaultIdentifier, DefaultChildren);
        framework.Add(new(DefaultIdentifierFramework, new("1.0.0")));

        // Act
        var result = framework.HasChildren;

        // Assert
        _ = result
            .Should().BeTrue();
    }

    /// <summary>
    /// Tests that when <see cref="Base{Dependency}.SortedChildren"/> is called for a <see cref="Framework"/> object not
    /// comprising children, it returns the empty collection.
    /// </summary>
    [AllCulturesFact]
    public void SortedChildren_NotComprisingChildren_ReturnsEmptyCollection()
    {
        // Act
        var result = DefaultValue.Framework.SortedChildren;

        // Assert
        _ = result
            .Should().BeEmpty();
    }

    /// <summary>
    /// Tests that when <see cref="Base{Dependency}.SortedChildren"/> is called for a <see cref="Framework"/> object
    /// comprising children added during construction, it returns the sorted collection of children.
    /// </summary>
    [AllCulturesFact]
    public void SortedChildren_ComprisingChildrenAddedDuringConstruction_ReturnsSortedChildren()
    {
        // Arrange
        var framework = new Framework(
            DefaultIdentifier,
            [
                SortedChildrenTestData[5],
                SortedChildrenTestData[4],
                SortedChildrenTestData[1],
                SortedChildrenTestData[3],
                SortedChildrenTestData[2],
                SortedChildrenTestData[0],
            ]);

        // Act
        var result = framework.SortedChildren;

        // Assert
        _ = result
            .Should().BeInAscendingOrder()
            .And.Equal(SortedChildrenTestData);
    }

    /// <summary>
    /// Tests that when <see cref="Base{Dependency}.SortedChildren"/> is called for a <see cref="Framework"/> object
    /// comprising children added after construction, it returns the sorted collection of children.
    /// </summary>
    [AllCulturesFact]
    public void SortedChildren_ComprisingChildrenAddedAfterConstruction_ReturnsSortedChildren()
    {
        // Arrange
        var framework = new Framework(DefaultIdentifier, DefaultChildren);
        framework.Add(SortedChildrenTestData[5]);
        framework.Add(SortedChildrenTestData[4]);
        framework.Add(SortedChildrenTestData[1]);
        framework.Add(SortedChildrenTestData[3]);
        framework.Add(SortedChildrenTestData[2]);
        framework.Add(SortedChildrenTestData[0]);

        // Act
        var result = framework.SortedChildren;

        // Assert
        _ = result
            .Should().BeInAscendingOrder()
            .And.Equal(SortedChildrenTestData);
    }

    /// <summary>
    /// Tests that when <see cref="Base{Dependency}.SortedChildren"/> is called for a <see cref="Framework"/> object
    /// comprising children added both during and after construction, it returns the sorted collection of children.
    /// </summary>
    [AllCulturesFact]
    public void SortedChildren_ComprisingChildrenAddedDuringAndAfterConstruction_ReturnsSortedChildren()
    {
        // Arrange
        var framework = new Framework(
            DefaultIdentifier,
            [
                SortedChildrenTestData[5],
                SortedChildrenTestData[4],
                SortedChildrenTestData[1],
            ]);
        framework.Add(SortedChildrenTestData[3]);
        framework.Add(SortedChildrenTestData[2]);
        framework.Add(SortedChildrenTestData[0]);

        // Act
        var result = framework.SortedChildren;

        // Assert
        _ = result
            .Should().BeInAscendingOrder()
            .And.Equal(SortedChildrenTestData);
    }

    /// <summary>
    /// Tests that when <see cref="Base{Dependency}.SortedChildren"/> is called for a <see cref="Framework"/> object
    /// comprising children added after an initial call to <see cref="Base{Dependency}.SortedChildren"/>, it returns the
    /// sorted collection of children.
    /// </summary>
    [AllCulturesFact]
    public void SortedChildren_ComprisingChildrenAddedAfterInitialSortedChildrenCall_ReturnsSortedChildren()
    {
        // Arrange
        var framework = new Framework(
            DefaultIdentifier,
            [
                SortedChildrenTestData[5],
                SortedChildrenTestData[4],
                SortedChildrenTestData[1],
            ]);
        _ = framework.SortedChildren;
        framework.Add(SortedChildrenTestData[3]);
        framework.Add(SortedChildrenTestData[2]);
        framework.Add(SortedChildrenTestData[0]);

        // Act
        var result = framework.SortedChildren;

        // Assert
        _ = result
            .Should().BeInAscendingOrder()
            .And.Equal(SortedChildrenTestData);
    }
}
