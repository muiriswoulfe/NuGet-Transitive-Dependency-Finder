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
using NuGetTransitiveDependencyFinder.UnitTests.Utilities.Globalization;
using NuGetTransitiveDependencyFinder.UnitTests.Output.UnitTests.Utilities;
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
    private static readonly Framework DefaultValue = new(DefaultIdentifier, DefaultChildren);

    /// <summary>
    /// A clone of <see cref="DefaultValue"/>, where the object content is identical, but the object reference is not.
    /// </summary>
    private static readonly Framework ClonedDefaultValue = new(DefaultIdentifier, DefaultChildren);

    /// <summary>
    /// The lesser test value, which occurs prior to <see cref="DefaultValue"/> according to an ordered sort.
    /// </summary>
    private static readonly Framework LesserValue = new(new("ABC", DefaultIdentifierVersion), DefaultChildren);

    /// <summary>
    /// The data for testing the operators.
    /// </summary>
    private static readonly IReadOnlyCollection<ComparisonTestData<Framework>> OperatorTestData =
        ComparisonDataGenerator.GenerateOperatorTestData(
            DefaultValue,
            ClonedDefaultValue,
            LesserValue,
            [
                new(
                    DefaultValue,
                    new(new(DefaultIdentifierFramework, DefaultIdentifierVersion), DefaultChildren),
                    Comparisons.Equal),
                new(
                    DefaultValue,
                    new(new("Identifier", DefaultIdentifierVersion), DefaultChildren),
                    Comparisons.Equal),
                new(
                    DefaultValue,
                    new(new(DefaultIdentifierFramework, new(1, 0)), DefaultChildren),
                    Comparisons.Equal),
                new(
                    DefaultValue,
                    new(new("IDENTIFIER", DefaultIdentifierVersion), DefaultChildren),
                    Comparisons.Equal),
                new(
                    DefaultValue,
                    new(DefaultIdentifier, [new(DefaultIdentifierFramework, new("1.0.0"))]),
                    Comparisons.Equal),
                new(
                    new(new NuGetFramework("ABC", DefaultIdentifierVersion), DefaultChildren),
                    DefaultValue,
                    Comparisons.LessThan),
                new(
                    new(new NuGetFramework(DefaultIdentifierFramework, new(0, 9)), DefaultChildren),
                    DefaultValue,
                    Comparisons.LessThan),
                new(
                    DefaultValue,
                    new(new("ABC", DefaultIdentifierVersion), DefaultChildren),
                    Comparisons.GreaterThan),
                new(
                    DefaultValue,
                    new(new(DefaultIdentifierFramework, new(0, 9)), DefaultChildren),
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
    public static TheoryData<Framework?, Framework?, bool> OperatorEqualTestData =>
        ComparisonDataGenerator.GenerateOperatorEqualTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="Framework.operator !=(Framework?, Framework?)"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<Framework?, Framework?, bool> OperatorNotEqualTestData =>
        ComparisonDataGenerator.GenerateOperatorNotEqualTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="Framework.operator &lt;(Framework?, Framework?)"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<Framework?, Framework?, bool> OperatorLessThanTestData =>
        ComparisonDataGenerator.GenerateOperatorLessThanTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="Framework.operator &lt;=(Framework?, Framework?)"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<Framework?, Framework?, bool> OperatorLessThanOrEqualTestData =>
        ComparisonDataGenerator.GenerateOperatorLessThanOrEqualTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="Framework.operator &gt;(Framework?, Framework?)"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<Framework?, Framework?, bool> OperatorGreaterThanTestData =>
        ComparisonDataGenerator.GenerateOperatorGreaterThanTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="Framework.operator &gt;=(Framework?, Framework?)"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<Framework?, Framework?, bool> OperatorGreaterThanOrEqualTestData =>
        ComparisonDataGenerator.GenerateOperatorGreaterThanOrEqualTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="IComparable{Framework}.CompareTo(Framework)"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<Framework, Framework?, int> CompareToTestData =>
        ComparisonDataGenerator.GenerateCompareToTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="IEquatable{Framework}.Equals(Framework)"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<Framework, Framework?, bool> EqualsTestData =>
        ComparisonDataGenerator.GenerateEqualsTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="Framework.GetHashCode()"/>.
    /// </summary>
    public static TheoryData<Framework, Framework> GetHashCodeTestData =>
        ComparisonDataGenerator.GenerateGetHashCodeTestData(
            DefaultValue,
            ClonedDefaultValue,
            LesserValue,
            new TheoryData<Framework, Framework>
            {
                {
                    DefaultValue,
                    new(new(DefaultIdentifierFramework, DefaultIdentifierVersion), DefaultChildren)
                },
                {
                    DefaultValue,
                    new(new("Identifier", DefaultIdentifierVersion), DefaultChildren)
                },
                {
                    DefaultValue,
                    new(new(DefaultIdentifierFramework, new(1, 0)), DefaultChildren)
                },
                {
                    DefaultValue,
                    new(new("IDENTIFIER", DefaultIdentifierVersion), DefaultChildren)
                },
                {
                    DefaultValue,
                    new(DefaultIdentifier, [new(DefaultIdentifierFramework, new("1.0.0"))])
                },
            });

    /// <summary>
    /// Gets the data for testing <see cref="object.ToString()"/>.
    /// </summary>
    public static TheoryData<Framework, string> ToStringTestData =>
        new()
        {
            { DefaultValue, "Identifier v1.0" },
            { LesserValue, "ABC v1.0" },
            { new(new(DefaultIdentifierFramework, new(1, 0, 1)), DefaultChildren), "Identifier v1.0.1" },
            { new(new(DefaultIdentifierFramework, new(1, 0, 1, 1)), DefaultChildren), "Identifier v1.0.1.1" },
            { new(new(DefaultIdentifierFramework, new(1, 0, 0, 1)), DefaultChildren), "Identifier v1.0.0.1" },
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
    public void OperatorEqual_WithAllCases_ReturnsValue(Framework? left, Framework? right, bool expected)
    {
        // Act
        var result = left == right;

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
    public void OperatorNotEqual_WithAllCases_ReturnsValue(Framework? left, Framework? right, bool expected)
    {
        // Act
        var result = left != right;

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
    public void OperatorLessThan_WithAllCases_ReturnsValue(Framework? left, Framework? right, bool expected)
    {
        // Act
        var result = left < right;

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
    public void OperatorLessThanOrEqual_WithAllCases_ReturnsValue(Framework? left, Framework? right, bool expected)
    {
        // Act
        var result = left <= right;

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
    public void OperatorGreaterThan_WithAllCases_ReturnsValue(Framework? left, Framework? right, bool expected)
    {
        // Act
        var result = left > right;

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
    public void OperatorGreaterThanOrEqual_WithAllCases_ReturnsValue(Framework? left, Framework? right, bool expected)
    {
        // Act
        var result = left >= right;

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
    public void CompareTo_WithAllCases_ReturnsValue(Framework left, Framework? right, int expected)
    {
        // Act
        var result = left.CompareTo(right);

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
    public void CompareToObject_WithAllCases_ReturnsValue(Framework left, object? right, int expected)
    {
        // Act
        var result = left.CompareTo(right);

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
        Action action = () => DefaultValue.CompareTo("value");

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
    public void Equals_WithAllCases_ReturnsValue(Framework left, Framework? right, bool expected)
    {
        // Act
        var result = left.Equals(right);

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
    public void EqualsObject_WithAllCases_ReturnsValue(Framework left, object? right, bool expected)
    {
        // Act
        var result = left.Equals(right);

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
        var result = DefaultValue.Equals("value");

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
    public void GetHashCode_WithIdenticalObjects_ReturnsSameValue(Framework value1, Framework value2)
    {
        // Act
        var result1 = value1.GetHashCode();
        var result2 = value2.GetHashCode();

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
        var result1 = DefaultValue.GetHashCode();
        var result2 = LesserValue.GetHashCode();

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
    public void ToString_WithDifferentObjects_ReturnsString(Framework value, string expected)
    {
        // Act
        var result = value.ToString();

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
        var result = DefaultValue.IsAddValid(new(DefaultIdentifierFramework, new("1.0.0")));

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
        var result = DefaultValue.HasChildren;

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
        var result = DefaultValue.SortedChildren;

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
