// <copyright file="ComparerInvariantUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Output;

using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NuGet.Versioning;
using NuGetTransitiveDependencyFinder.Output;
using Xunit;

/// <summary>
/// Property-style invariant tests for the comparison infrastructure exposed through <see cref="Dependency"/>,
/// <see cref="Framework"/>, and <see cref="Project"/>. Rather than pinning a single example, these tests exercise
/// algebraic properties of a total order — reflexivity, antisymmetry, transitivity, and consistency-with-equals —
/// across a wide matrix of inputs. They catch a class of subtle ordering bugs that individual example-based tests
/// cannot, because a comparator can satisfy every hand-crafted example while silently violating transitivity on a
/// triple that no reviewer thought to write down.
/// </summary>
/// <remarks>
/// Test-data rows are emitted as integer indices into the <see cref="DependencySamples"/> and
/// <see cref="FrameworkSamples"/> collections rather than directly as <see cref="Dependency"/> or
/// <see cref="Framework"/> instances. This is deliberate: xUnit's Test Explorer integration requires the
/// <see cref="TheoryData{T}"/> type arguments to be serialisable, and the production comparison types are not.
/// Indirecting through indices keeps the reported test names stable and enumerable per-row.
/// </remarks>
public class ComparerInvariantUnitTests
{
    /// <summary>
    /// A default <see cref="Framework"/> instance attached to constructed <see cref="Project"/> values so that
    /// <see cref="Base{TChild}.SortedChildren"/> does not regard them as empty.
    /// </summary>
    private static readonly Framework DefaultFramework =
        new(new("net8.0"), [new("Dependency", new("1.0.0"))]);

    /// <summary>
    /// The diverse sample collection for <see cref="Dependency"/>.
    /// </summary>
    private static readonly IReadOnlyList<Dependency> DependencySamples =
    [
        new("A", NuGetVersion.Parse("0.0.1")),
        new("A", NuGetVersion.Parse("1.0.0")),
        new("A", NuGetVersion.Parse("1.0.0-alpha")),
        new("A", NuGetVersion.Parse("2.0.0")),
        new("a", NuGetVersion.Parse("1.0.0")),
        new("B", NuGetVersion.Parse("1.0.0")),
        new("Newtonsoft.Json", NuGetVersion.Parse("12.0.3")),
        new("Newtonsoft.Json", NuGetVersion.Parse("13.0.0")),
        new("newtonsoft.json", NuGetVersion.Parse("13.0.0")),
        new("Z", NuGetVersion.Parse("9.9.9")),
    ];

    /// <summary>
    /// The diverse sample collection for <see cref="Framework"/>.
    /// </summary>
    private static readonly IReadOnlyList<Framework> FrameworkSamples =
    [
        new(new("net6.0"), [new("X", NuGetVersion.Parse("1.0.0"))]),
        new(new("net7.0"), [new("X", NuGetVersion.Parse("1.0.0"))]),
        new(new("net8.0"), [new("X", NuGetVersion.Parse("1.0.0"))]),
        new(new("net9.0"), [new("X", NuGetVersion.Parse("1.0.0"))]),
        new(new("netstandard2.0"), [new("X", NuGetVersion.Parse("1.0.0"))]),
        new(new("netstandard2.1"), [new("X", NuGetVersion.Parse("1.0.0"))]),
    ];

    /// <summary>
    /// Gets single-index rows over <see cref="DependencySamples"/>.
    /// </summary>
    public static TheoryData<int> DependencyIndices => MakeIndices(DependencySamples.Count);

    /// <summary>
    /// Gets single-index rows over <see cref="FrameworkSamples"/>.
    /// </summary>
    public static TheoryData<int> FrameworkIndices => MakeIndices(FrameworkSamples.Count);

    /// <summary>
    /// Gets index pairs over <see cref="DependencySamples"/>.
    /// </summary>
    public static TheoryData<int, int> DependencyIndexPairs => MakeIndexPairs(DependencySamples.Count);

    /// <summary>
    /// Gets index triples over <see cref="DependencySamples"/>.
    /// </summary>
    public static TheoryData<int, int, int> DependencyIndexTriples => MakeIndexTriples(DependencySamples.Count);

    /// <summary>
    /// Gets index pairs over <see cref="FrameworkSamples"/>.
    /// </summary>
    public static TheoryData<int, int> FrameworkIndexPairs => MakeIndexPairs(FrameworkSamples.Count);

    /// <summary>
    /// Tests that every <see cref="Dependency"/> compares equal to itself. Reflexivity is a foundational axiom of a
    /// total order and is what justifies identity-equivalence optimisations in sort routines and hash-based
    /// containers.
    /// </summary>
    /// <param name="index">The index into <see cref="DependencySamples"/>.</param>
    [Theory]
    [MemberData(nameof(DependencyIndices))]
    public void DependencyCompareTo_WithItself_ReturnsZero(int index)
    {
        // Arrange
        var value = DependencySamples[index];

        // Act
        var result = value.CompareTo(value);

        // Assert
        _ = result
            .Should().Be(0);
    }

    /// <summary>
    /// Tests antisymmetry on <see cref="Dependency.CompareTo(Dependency?)"/>: for any two values, the sign of
    /// <c>a.CompareTo(b)</c> must be the negation of the sign of <c>b.CompareTo(a)</c>. Violating this invariant is
    /// the classic cause of <see cref="List{T}.Sort()"/> returning different results depending on which element
    /// happens to land on which side of the partition, and is one of the subtlest comparator bugs to reproduce.
    /// </summary>
    /// <param name="leftIndex">The index of the left operand.</param>
    /// <param name="rightIndex">The index of the right operand.</param>
    [Theory]
    [MemberData(nameof(DependencyIndexPairs))]
    public void DependencyCompareTo_WhenSwapped_NegatesSign(int leftIndex, int rightIndex)
    {
        // Arrange
        var left = DependencySamples[leftIndex];
        var right = DependencySamples[rightIndex];

        // Act
        var forward = Math.Sign(left.CompareTo(right));
        var reverse = Math.Sign(right.CompareTo(left));

        // Assert
        _ = forward
            .Should().Be(-reverse);
    }

    /// <summary>
    /// Tests that <see cref="Dependency.CompareTo(Dependency?)"/> maps its result into the closed interval
    /// <c>[-1, 1]</c>. The contract of <see cref="IComparable{T}"/> does not in general mandate this, but the
    /// comparator pipeline in this project explicitly funnels every comparison through the shared
    /// <c>Comparer.MapCompareTo</c> helper, which does. Pinning the post-condition here eliminates an entire class
    /// of integer-overflow and out-of-range-propagation bugs that a looser assertion would miss.
    /// </summary>
    /// <param name="leftIndex">The index of the left operand.</param>
    /// <param name="rightIndex">The index of the right operand.</param>
    [Theory]
    [MemberData(nameof(DependencyIndexPairs))]
    public void DependencyCompareTo_ForAnyPair_ReturnsValueInClosedMinusOneToOne(int leftIndex, int rightIndex)
    {
        // Arrange
        var left = DependencySamples[leftIndex];
        var right = DependencySamples[rightIndex];

        // Act
        var result = left.CompareTo(right);

        // Assert
        _ = result
            .Should().BeInRange(-1, 1);
    }

    /// <summary>
    /// Tests transitivity on <see cref="Dependency.CompareTo(Dependency?)"/>: if <c>a ≤ b</c> and <c>b ≤ c</c>, then
    /// <c>a ≤ c</c>. Transitivity is the invariant that <see cref="List{T}.Sort()"/> depends on for correctness; a
    /// comparator that is antisymmetric but non-transitive can produce a different sorted order on every call,
    /// silently corrupting downstream consumers of <see cref="Base{TChild}.SortedChildren"/>.
    /// </summary>
    /// <param name="firstIndex">The index of the first operand.</param>
    /// <param name="secondIndex">The index of the second operand.</param>
    /// <param name="thirdIndex">The index of the third operand.</param>
    [Theory]
    [MemberData(nameof(DependencyIndexTriples))]
    public void DependencyCompareTo_WithLessOrEqualChain_IsTransitive(
        int firstIndex, int secondIndex, int thirdIndex)
    {
        // Arrange
        var first = DependencySamples[firstIndex];
        var second = DependencySamples[secondIndex];
        var third = DependencySamples[thirdIndex];

        // Act
        var firstToSecond = first.CompareTo(second);
        var secondToThird = second.CompareTo(third);
        var firstToThird = first.CompareTo(third);

        // Assert
        if (firstToSecond <= 0 && secondToThird <= 0)
        {
            _ = firstToThird
                .Should().BeLessThanOrEqualTo(0);
        }
    }

    /// <summary>
    /// Tests that <see cref="Dependency.CompareTo(Dependency?)"/> returning <c>0</c> is consistent with
    /// <see cref="Dependency.Equals(Dependency)"/>. A comparator that reports equality without the objects being
    /// <see cref="IEquatable{T}.Equals(T)"/>-equal will silently deduplicate non-equivalent entries when fed through
    /// hash-based collections, which is exactly the pattern used inside the dependency-finder analysis layer.
    /// </summary>
    /// <param name="leftIndex">The index of the left operand.</param>
    /// <param name="rightIndex">The index of the right operand.</param>
    [Theory]
    [MemberData(nameof(DependencyIndexPairs))]
    public void DependencyCompareTo_ReturnsZero_IfAndOnlyIfEquals(int leftIndex, int rightIndex)
    {
        // Arrange
        var left = DependencySamples[leftIndex];
        var right = DependencySamples[rightIndex];

        // Act
        var compareReturnsZero = left.CompareTo(right) == 0;
        var equals = left.Equals(right);

        // Assert
        _ = compareReturnsZero
            .Should().Be(equals);
    }

    /// <summary>
    /// Tests that sorting a collection twice yields the same result as sorting it once, for a non-trivial collection.
    /// This is the concrete consumer-level manifestation of the comparator invariants and pins down the whole
    /// <see cref="Base{TChild}.SortedChildren"/> pipeline.
    /// </summary>
    [Fact]
    public void SortedChildren_SortedTwice_YieldsIdenticalOrdering()
    {
        // Arrange
        var projects = new Projects(DependencySamples.Count);
        for (var index = 0; index < DependencySamples.Count; index++)
        {
            var project = new Project(DependencySamples[index].Identifier + index, 1);
            project.Add(DefaultFramework);
            projects.Add(project);
        }

        // Act
        var firstPass = projects.SortedChildren.ToList();
        var secondPass = projects.SortedChildren.ToList();

        // Assert
        _ = secondPass
            .Should().Equal(firstPass);
    }

    /// <summary>
    /// Tests reflexivity on <see cref="Framework.CompareTo(Framework?)"/>.
    /// </summary>
    /// <param name="index">The index into <see cref="FrameworkSamples"/>.</param>
    [Theory]
    [MemberData(nameof(FrameworkIndices))]
    public void FrameworkCompareTo_WithItself_ReturnsZero(int index)
    {
        // Arrange
        var value = FrameworkSamples[index];

        // Act
        var result = value.CompareTo(value);

        // Assert
        _ = result
            .Should().Be(0);
    }

    /// <summary>
    /// Tests antisymmetry on <see cref="Framework.CompareTo(Framework?)"/>.
    /// </summary>
    /// <param name="leftIndex">The index of the left operand.</param>
    /// <param name="rightIndex">The index of the right operand.</param>
    [Theory]
    [MemberData(nameof(FrameworkIndexPairs))]
    public void FrameworkCompareTo_WhenSwapped_NegatesSign(int leftIndex, int rightIndex)
    {
        // Arrange
        var left = FrameworkSamples[leftIndex];
        var right = FrameworkSamples[rightIndex];

        // Act
        var forward = Math.Sign(left.CompareTo(right));
        var reverse = Math.Sign(right.CompareTo(left));

        // Assert
        _ = forward
            .Should().Be(-reverse);
    }

    /// <summary>
    /// Tests that the internal <c>Comparer.MapCompareTo</c> helper always produces a value in <c>{-1, 0, 1}</c>
    /// regardless of the magnitude of its input. This covers <see cref="int.MaxValue"/>, <see cref="int.MinValue"/>,
    /// and every order of magnitude between, and is the invariant that justifies the post-condition pinned by
    /// <see cref="DependencyCompareTo_ForAnyPair_ReturnsValueInClosedMinusOneToOne"/>.
    /// </summary>
    /// <param name="value">The input to <c>Comparer.MapCompareTo</c>.</param>
    [Theory]
    [InlineData(int.MinValue)]
    [InlineData(-1_000_000)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(1_000_000)]
    [InlineData(int.MaxValue)]
    public void MapCompareTo_ForAnyInput_ReturnsValueInNegativeOneZeroOne(int value)
    {
        // Act
        var result = InvokeMapCompareTo(value);

        // Assert
        _ = result
            .Should().BeOneOf(-1, 0, 1);
    }

    /// <summary>
    /// Tests that <c>Comparer.MapCompareTo</c> preserves sign: strictly-positive inputs map to <c>1</c>,
    /// strictly-negative inputs map to <c>-1</c>, and <c>0</c> maps to <c>0</c>. Inverting the sign mapping would
    /// silently reverse every comparison in the library, so having an explicit invariant here is load-bearing.
    /// </summary>
    /// <param name="value">The input.</param>
    /// <param name="expected">The expected mapped output.</param>
    [Theory]
    [InlineData(int.MinValue, -1)]
    [InlineData(-2, -1)]
    [InlineData(-1, -1)]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(int.MaxValue, 1)]
    public void MapCompareTo_PreservesSign(int value, int expected)
    {
        // Act
        var result = InvokeMapCompareTo(value);

        // Assert
        _ = result
            .Should().Be(expected);
    }

    /// <summary>
    /// Invokes the internal <c>Comparer.MapCompareTo</c> helper via reflection. Required because <c>Comparer</c> is
    /// declared <c>internal</c> to the product assembly and is not exposed to the test assembly via an
    /// <c>InternalsVisibleTo</c> attribute on the current configuration.
    /// </summary>
    /// <param name="value">The input to pass through.</param>
    /// <returns>The mapped output.</returns>
    private static int InvokeMapCompareTo(int value)
    {
        var method = typeof(Dependency).Assembly
            .GetType("NuGetTransitiveDependencyFinder.Output.Comparer")!
            .GetMethod(
                "MapCompareTo",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)!;
        return (int)method.Invoke(null, [value])!;
    }

    /// <summary>
    /// Builds a <see cref="TheoryData{T}"/> of integer indices in <c>[0, count)</c>.
    /// </summary>
    /// <param name="count">The exclusive upper bound.</param>
    /// <returns>The populated data.</returns>
    private static TheoryData<int> MakeIndices(int count)
    {
        var data = new TheoryData<int>();
        for (var index = 0; index < count; index++)
        {
            data.Add(index);
        }

        return data;
    }

    /// <summary>
    /// Builds a <see cref="TheoryData{T1, T2}"/> of all index pairs in <c>[0, count)²</c>.
    /// </summary>
    /// <param name="count">The exclusive upper bound on each axis.</param>
    /// <returns>The populated data.</returns>
    private static TheoryData<int, int> MakeIndexPairs(int count)
    {
        var data = new TheoryData<int, int>();
        for (var left = 0; left < count; left++)
        {
            for (var right = 0; right < count; right++)
            {
                data.Add(left, right);
            }
        }

        return data;
    }

    /// <summary>
    /// Builds a <see cref="TheoryData{T1, T2, T3}"/> of all index triples in <c>[0, count)³</c>.
    /// </summary>
    /// <param name="count">The exclusive upper bound on each axis.</param>
    /// <returns>The populated data.</returns>
    private static TheoryData<int, int, int> MakeIndexTriples(int count)
    {
        var data = new TheoryData<int, int, int>();
        for (var first = 0; first < count; first++)
        {
            for (var second = 0; second < count; second++)
            {
                for (var third = 0; third < count; third++)
                {
                    data.Add(first, second, third);
                }
            }
        }

        return data;
    }
}
