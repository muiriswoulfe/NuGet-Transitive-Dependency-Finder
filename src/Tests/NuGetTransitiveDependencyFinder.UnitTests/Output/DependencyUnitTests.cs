// <copyright file="DependencyUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Output;

using System;
using System.Collections.Generic;
using FluentAssertions;
using NuGet.Versioning;
using NuGetTransitiveDependencyFinder.Output;
using NuGetTransitiveDependencyFinder.TestUtilities.Globalization;
using NuGetTransitiveDependencyFinder.UnitTests.Output.TestUtilities;
using Xunit;

/// <summary>
/// Unit tests for the <see cref="Dependency"/> class.
/// </summary>
public class DependencyUnitTests
{
    /// <summary>
    /// The default test identifier.
    /// </summary>
    private const string DefaultIdentifier = "Identifier";

    /// <summary>
    /// The default test version.
    /// </summary>
    private static readonly NuGetVersion DefaultVersion = new("1.0.0-alpha");

    /// <summary>
    /// The default test value.
    /// </summary>
    private static readonly Dependency DefaultValue = new(DefaultIdentifier, DefaultVersion);

    /// <summary>
    /// A clone of <see cref="DefaultValue"/>, where the object content is identical, but the object reference is not.
    /// </summary>
    private static readonly Dependency ClonedDefaultValue = new(DefaultIdentifier, DefaultVersion);

    /// <summary>
    /// The lesser test value, which occurs prior to <see cref="DefaultValue"/> according to an ordered sort.
    /// </summary>
    private static readonly Dependency LesserValue = new(DefaultIdentifier, new("0.9.9-alpha"));

    /// <summary>
    /// The data for testing the operators.
    /// </summary>
    private static readonly IReadOnlyCollection<ComparisonTestData<Dependency>> OperatorTestData =
        ComparisonDataGenerator.GenerateOperatorTestData(
            DefaultValue,
            ClonedDefaultValue,
            LesserValue,
            new ComparisonTestData<Dependency>[]
            {
                new(DefaultValue, new("Identifier", DefaultVersion), Comparisons.Equal),
                new(DefaultValue, new(DefaultIdentifier, new("1.0.0-alpha")), Comparisons.Equal),
                new(DefaultValue, new("IDENTIFIER", DefaultVersion), Comparisons.Equal),
                new(DefaultValue, new(DefaultIdentifier, new("1.0.0-ALPHA")), Comparisons.Equal),
                new(new("ABC", DefaultVersion), DefaultValue, Comparisons.LessThan),
                new(new(DefaultIdentifier, new("0.9.9-alpha")), DefaultValue, Comparisons.LessThan),
                new(DefaultValue, new("ABC", DefaultVersion), Comparisons.GreaterThan),
                new(DefaultValue, new(DefaultIdentifier, new("0.9.9-alpha")), Comparisons.GreaterThan),
            });

    /// <summary>
    /// Gets the data for testing <see cref="Dependency.operator ==(Dependency?, Dependency?)"/>
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<Dependency?, Dependency?, bool> OperatorEqualTestData =>
        ComparisonDataGenerator.GenerateOperatorEqualTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="Dependency.operator !=(Dependency?, Dependency?)"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<Dependency?, Dependency?, bool> OperatorNotEqualTestData =>
        ComparisonDataGenerator.GenerateOperatorNotEqualTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="Dependency.operator &lt;(Dependency?, Dependency?)"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<Dependency?, Dependency?, bool> OperatorLessThanTestData =>
        ComparisonDataGenerator.GenerateOperatorLessThanTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="Dependency.operator &lt;=(Dependency?, Dependency?)"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<Dependency?, Dependency?, bool> OperatorLessThanOrEqualTestData =>
        ComparisonDataGenerator.GenerateOperatorLessThanOrEqualTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="Dependency.operator &gt;(Dependency?, Dependency?)"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<Dependency?, Dependency?, bool> OperatorGreaterThanTestData =>
        ComparisonDataGenerator.GenerateOperatorGreaterThanTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="Dependency.operator &gt;=(Dependency?, Dependency?)"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<Dependency?, Dependency?, bool> OperatorGreaterThanOrEqualTestData =>
        ComparisonDataGenerator.GenerateOperatorGreaterThanOrEqualTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="IComparable{Dependency}.CompareTo(Dependency)"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<Dependency, Dependency?, int> CompareToTestData =>
        ComparisonDataGenerator.GenerateCompareToTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="IEquatable{Dependency}.Equals(Dependency)"/>.
    /// </summary>
    /// <returns>The generated data.</returns>
    public static TheoryData<Dependency, Dependency?, bool> EqualsTestData =>
        ComparisonDataGenerator.GenerateEqualsTestData(OperatorTestData);

    /// <summary>
    /// Gets the data for testing <see cref="Dependency.GetHashCode()"/>.
    /// </summary>
    public static TheoryData<Dependency, Dependency> GetHashCodeTestData =>
        ComparisonDataGenerator.GenerateGetHashCodeTestData(
            DefaultValue,
            ClonedDefaultValue,
            LesserValue,
            new TheoryData<Dependency, Dependency>
            {
                { DefaultValue, new("Identifier", DefaultVersion) },
                { DefaultValue, new(DefaultIdentifier, new("1.0.0-alpha")) },
                { DefaultValue, new("IDENTIFIER", DefaultVersion) },
                { DefaultValue, new(DefaultIdentifier, new("1.0.0-ALPHA")) },
            });

    /// <summary>
    /// Gets the data for testing <see cref="object.ToString()"/>.
    /// </summary>
    public static TheoryData<Dependency, string> ToStringTestData =>
        new()
        {
            { DefaultValue, "Identifier v1.0.0-alpha" },
            { LesserValue, "Identifier v0.9.9-alpha" },
        };

    /// <summary>
    /// Tests that when <see cref="Dependency.Identifier"/> is called after construction, it returns the value
    /// specified.
    /// </summary>
    /// <param name="value">The value of <see cref="Dependency.Identifier"/>.</param>
    [AllCulturesTheory]
    [InlineData("Identifier 1")]
    [InlineData("Identifier 2")]
    public void Identifier_AfterConstruction_ReturnsValue(string value)
    {
        // Arrange & Act
        var dependency = new Dependency(value, DefaultVersion);

        // Assert
        _ = value
            .Should().Be(dependency.Identifier);
    }

    /// <summary>
    /// Tests that when <see cref="Dependency.Version"/> is called after construction, it returns the value specified.
    /// </summary>
    /// <param name="value">The value of <see cref="Dependency.Version"/>.</param>
    [AllCulturesTheory]
    [InlineData("0.9.9")]
    [InlineData("1.0.0-alpha")]
    [InlineData("2.0.0-beta")]
    public void Version_AfterConstruction_ReturnsValue(string value)
    {
        // Arrange & Act
        var dependency = new Dependency(DefaultIdentifier, new(value));

        // Assert
        _ = value
            .Should().Be(dependency.Version.ToString());
    }

    /// <summary>
    /// Tests that when <see cref="Dependency.IsTransitive"/> is called after being set, it returns the value specified.
    /// </summary>
    /// <param name="value">The value of <see cref="Dependency.IsTransitive"/>.</param>
    [AllCulturesTheory]
    [InlineData(true)]
    [InlineData(false)]
    public void IsTransitive_AfterSetting_ReturnsValue(bool value)
    {
        // Arrange & Act
        var dependency = new Dependency(DefaultIdentifier, DefaultVersion)
        {
            IsTransitive = value,
        };

        // Assert
        _ = value
            .Should().Be(dependency.IsTransitive);
    }

    /// <summary>
    /// Tests that when <see cref="Dependency.operator ==(Dependency?, Dependency?)"/> is called with different values,
    /// it returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(OperatorEqualTestData))]
    public void OperatorEqual_WithAllCases_ReturnsValue(Dependency left, Dependency right, bool expected)
    {
        // Act
        var result = left == right;

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="Dependency.operator !=(Dependency?, Dependency?)"/> is called with different values,
    /// it returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(OperatorNotEqualTestData))]
    public void OperatorNotEqual_WithAllCases_ReturnsValue(Dependency left, Dependency right, bool expected)
    {
        // Act
        var result = left != right;

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="Dependency.operator &lt;(Dependency?, Dependency?)"/> is called with different
    /// values, it returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(OperatorLessThanTestData))]
    public void OperatorLessThan_WithAllCases_ReturnsValue(Dependency left, Dependency right, bool expected)
    {
        // Act
        var result = left < right;

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="Dependency.operator &lt;=(Dependency?, Dependency?)"/> is called with different
    /// values, it returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(OperatorLessThanOrEqualTestData))]
    public void OperatorLessThanOrEqual_WithAllCases_ReturnsValue(Dependency left, Dependency right, bool expected)
    {
        // Act
        var result = left <= right;

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="Dependency.operator &gt;(Dependency?, Dependency?)"/> is called with different
    /// values, it returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(OperatorGreaterThanTestData))]
    public void OperatorGreaterThan_WithAllCases_ReturnsValue(Dependency left, Dependency right, bool expected)
    {
        // Act
        var result = left > right;

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="Dependency.operator &gt;=(Dependency?, Dependency?)"/> is called with different
    /// values, it returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(OperatorGreaterThanOrEqualTestData))]
    public void OperatorGreaterThanOrEqual_WithAllCases_ReturnsValue(
        Dependency left,
        Dependency right,
        bool expected)
    {
        // Act
        var result = left >= right;

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="IComparable{Dependency}.CompareTo(Dependency)"/> is called with different values, it
    /// returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(CompareToTestData))]
    public void CompareTo_WithAllCases_ReturnsValue(Dependency left, Dependency right, int expected)
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
    public void CompareToObject_WithAllCases_ReturnsValue(Dependency left, object right, int expected)
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
            .Should().Throw<ArgumentException>().WithMessage("Object must be of type Dependency. (Parameter 'obj')")
            .And.ParamName.Should().Be("obj");
    }

    /// <summary>
    /// Tests that when <see cref="IEquatable{Dependency}.Equals(Dependency)"/> is called with different values, it
    /// returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(EqualsTestData))]
    public void Equals_WithAllCases_ReturnsValue(Dependency left, Dependency right, bool expected)
    {
        // Act
        var result = left.Equals(right);

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="IEquatable{Dependency}.Equals(Dependency)"/> is called with different values against
    /// an <see langword="object"/>, it returns the expected value in each case.
    /// </summary>
    /// <param name="left">The left operand to compare.</param>
    /// <param name="right">The right operand to compare.</param>
    /// <param name="expected">The expected result.</param>
    [AllCulturesTheory]
    [MemberData(nameof(EqualsTestData))]
    public void EqualsObject_WithAllCases_ReturnsValue(Dependency left, object right, bool expected)
    {
        // Act
        var result = left.Equals(right);

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Tests that when <see cref="IEquatable{Dependency}.Equals(Dependency)"/> is called with different object types,
    /// it returns <see langword="false"/>.
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
    /// Tests that when <see cref="Dependency.GetHashCode()"/> is called with identical objects, it returns the same
    /// value each time.
    /// </summary>
    /// <param name="value1">The first value for which to compute a hash code.</param>
    /// <param name="value2">The second value for which to compute a hash code.</param>
    [AllCulturesTheory]
    [MemberData(nameof(GetHashCodeTestData))]
    public void GetHashCode_WithIdenticalObjects_ReturnsSameValue(Dependency value1, Dependency value2)
    {
        // Act
        var result1 = value1.GetHashCode();
        var result2 = value2.GetHashCode();

        // Assert
        _ = result1
            .Should().Be(result2);
    }

    /// <summary>
    /// Tests that when <see cref="Dependency.GetHashCode()"/> is called with different objects, it returns different
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
    public void ToString_WithDifferentObjects_ReturnsString(Dependency value, string expected)
    {
        // Act
        var result = value.ToString();

        // Assert
        _ = result
            .Should().Be(expected);
    }
}
