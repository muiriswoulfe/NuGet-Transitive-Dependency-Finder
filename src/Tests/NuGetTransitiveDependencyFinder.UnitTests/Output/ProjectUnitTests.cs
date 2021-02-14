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
    using NuGetTransitiveDependencyFinder.TestUtilities.Globalization;
    using NuGetTransitiveDependencyFinder.UnitTests.Output.TestUtilities;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="Project"/> class.
    /// </summary>
    public class ProjectUnitTests
    {
        /// <summary>
        /// The default test identifier.
        /// </summary>
        private const string DefaultIdentifier = "Identifier";

        /// <summary>
        /// The default test value.
        /// </summary>
        private static readonly Project DefaultValue = new(DefaultIdentifier, 0);

        /// <summary>
        /// A clone of <see cref="DefaultValue"/>, where the object content is identical, but the object reference is
        /// not.
        /// </summary>
        private static readonly Project ClonedDefaultValue = new(DefaultIdentifier, 0);

        /// <summary>
        /// The lesser test value, which occurs prior to <see cref="DefaultValue"/> according to an ordered sort.
        /// </summary>
        private static readonly Project LesserValue = new("ABC", 0);

        /// <summary>
        /// The default test version.
        /// </summary>
        private static readonly Version DefaultVersion = new(1, 0);

        /// <summary>
        /// The test value for the collection of <see cref="Dependency"/> objects where no dependencies exist.
        /// </summary>
        private static readonly IReadOnlyCollection<Dependency> NoDependencies = Array.Empty<Dependency>();

        /// <summary>
        /// The default test value for the collection of <see cref="Dependency"/> objects.
        /// </summary>
        private static readonly IReadOnlyCollection<Dependency> DefaultDependencies =
            new Dependency[]
            {
                new("Dependency", new("1.0.0")),
            };

        /// <summary>
        /// The data for testing the operators.
        /// </summary>
        private static readonly IReadOnlyCollection<ComparisonTestData<Project>> OperatorTestData =
            ComparisonDataGenerator.GenerateOperatorTestData(
                DefaultValue,
                ClonedDefaultValue,
                LesserValue,
                new ComparisonTestData<Project>[]
                {
                    new(DefaultValue, new(DefaultIdentifier, 0), Comparisons.Equal),
                    new(DefaultValue, new("IDENTIFIER", 0), Comparisons.Equal),
                    new(DefaultValue, new(DefaultIdentifier, 1), Comparisons.Equal),
                    new(new("ABC", 0), DefaultValue, Comparisons.LessThan),
                    new(DefaultValue, new("ABC", 0), Comparisons.GreaterThan),
                });

        /// <summary>
        /// The data for testing <see cref="Base{Framework}.SortedChildren"/>.
        /// </summary>
        private static readonly IReadOnlyList<Framework> SortedChildrenTestData =
            new Framework[]
            {
                new(new("A", new(0, 9)), DefaultDependencies),
                new(new("A", DefaultVersion), DefaultDependencies),
                new(new("B", DefaultVersion), DefaultDependencies),
                new(new("C", DefaultVersion), DefaultDependencies),
                new(new("Y", DefaultVersion), DefaultDependencies),
                new(new("Z", DefaultVersion), DefaultDependencies),
            };

        /// <summary>
        /// Gets the data for testing <see cref="Project.operator =="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static TheoryData<Project?, Project?, bool> OperatorEqualTestData =>
            ComparisonDataGenerator.GenerateOperatorEqualTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Project.operator !="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static TheoryData<Project?, Project?, bool> OperatorNotEqualTestData =>
            ComparisonDataGenerator.GenerateOperatorNotEqualTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Project.operator &lt;"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static TheoryData<Project?, Project?, bool> OperatorLessThanTestData =>
            ComparisonDataGenerator.GenerateOperatorLessThanTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Project.operator &lt;="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static TheoryData<Project?, Project?, bool> OperatorLessThanOrEqualTestData =>
            ComparisonDataGenerator.GenerateOperatorLessThanOrEqualTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Project.operator &gt;"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static TheoryData<Project?, Project?, bool> OperatorGreaterThanTestData =>
            ComparisonDataGenerator.GenerateOperatorGreaterThanTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Project.operator &gt;="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static TheoryData<Project?, Project?, bool> OperatorGreaterThanOrEqualTestData =>
            ComparisonDataGenerator.GenerateOperatorGreaterThanOrEqualTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="IComparable{Project}.CompareTo"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static TheoryData<Project, Project?, int> CompareToTestData =>
            ComparisonDataGenerator.GenerateCompareToTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="IEquatable{Project}.Equals"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static TheoryData<Project, Project?, bool> EqualsTestData =>
            ComparisonDataGenerator.GenerateEqualsTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Project.GetHashCode()"/>.
        /// </summary>
        public static TheoryData<Project, Project> GetHashCodeTestData =>
            ComparisonDataGenerator.GenerateGetHashCodeTestData(
                DefaultValue,
                ClonedDefaultValue,
                LesserValue,
                new TheoryData<Project, Project>
                {
                    { DefaultValue, new(DefaultIdentifier, 0) },
                    { DefaultValue, new("IDENTIFIER", 0) },
                    { DefaultValue, new(DefaultIdentifier, 1) },
                });

        /// <summary>
        /// Gets the data for testing <see cref="object.ToString()"/>.
        /// </summary>
        public static TheoryData<Project, string> ToStringTestData =>
            new TheoryData<Project, string>
            {
                { DefaultValue, DefaultIdentifier },
                { LesserValue, "ABC" },
            };

        /// <summary>
        /// Tests that when <see cref="Project.operator =="/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected result.</param>
        [CultureTheory]
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
        [CultureTheory]
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
        [CultureTheory]
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
        [CultureTheory]
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
        [CultureTheory]
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
        [CultureTheory]
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
        [CultureTheory]
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
        [CultureTheory]
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
        [CultureFact]
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
        [CultureTheory]
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
        [CultureTheory]
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
        [CultureFact]
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
        [CultureTheory]
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
        [CultureFact]
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
        [CultureTheory]
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
        /// Tests than when <see cref="Project.IsAddValid(Framework?)"/> is called with a <see cref="Framework"/> not
        /// comprising children, it returns <c>false</c>.
        /// </summary>
        [CultureFact]
        public void IsAddValid_WithFrameworkNotComprisingChildren_ReturnsFalse()
        {
            // Act
            var result = DefaultValue.IsAddValid(new(new(DefaultIdentifier, DefaultVersion), NoDependencies));

            // Assert
            _ = result.Should().BeFalse();
        }

        /// <summary>
        /// Tests than when <see cref="Project.IsAddValid(Framework?)"/> is called with a <see cref="Framework"/>
        /// comprising children, it returns <c>true</c>.
        /// </summary>
        [CultureFact]
        public void IsAddValid_WithFrameworkComprisingChildren_ReturnsTrue()
        {
            // Act
            var result = DefaultValue.IsAddValid(new(new(DefaultIdentifier, DefaultVersion), DefaultDependencies));

            // Assert
            _ = result.Should().BeTrue();
        }

        /// <summary>
        /// Tests that when <see cref="Base{Framework}.HasChildren"/> is called for a <see cref="Project"/> object not
        /// comprising children, it returns <c>false</c>.
        /// </summary>
        [CultureFact]
        public void HasChildren_NotComprisingChildren_ReturnsFalse()
        {
            // Act
            var result = DefaultValue.HasChildren;

            // Assert
            _ = result.Should().BeFalse();
        }

        /// <summary>
        /// Tests that when <see cref="Base{Framework}.HasChildren"/> is called for a <see cref="Project"/> object
        /// comprising children without children, it returns <c>false</c>.
        /// </summary>
        [CultureFact]
        public void HasChildren_ComprisingChildrenWithoutChildren_ReturnsFalse()
        {
            // Arrange
            var project = new Project(DefaultIdentifier, 1);
            project.Add(new(new(DefaultIdentifier, DefaultVersion), NoDependencies));

            // Act
            var result = project.HasChildren;

            // Assert
            _ = result.Should().BeFalse();
        }

        /// <summary>
        /// Tests that when <see cref="Base{Framework}.HasChildren"/> is called for a <see cref="Project"/> object
        /// comprising children with children, it returns <c>true</c>.
        /// </summary>
        [CultureFact]
        public void HasChildren_ComprisingChildrenWithChildren_ReturnsTrue()
        {
            // Arrange
            var project = new Project(DefaultIdentifier, 1);
            project.Add(new(new(DefaultIdentifier, DefaultVersion), DefaultDependencies));

            // Act
            var result = project.HasChildren;

            // Assert
            _ = result.Should().BeTrue();
        }

        /// <summary>
        /// Tests that when <see cref="Base{Framework}.HasChildren"/> is called for a <see cref="Project"/> object
        /// comprising children for which a subset have children, it returns <c>true</c>.
        /// </summary>
        [CultureFact]
        public void HasChildren_ComprisingChildrenWithSubsetHavingChildren_ReturnsTrue()
        {
            // Arrange
            var project = new Project(DefaultIdentifier, 2);
            project.Add(new(new(DefaultIdentifier, DefaultVersion), NoDependencies));
            project.Add(new(new(DefaultIdentifier, DefaultVersion), DefaultDependencies));

            // Act
            var result = project.HasChildren;

            // Assert
            _ = result.Should().BeTrue();
        }

        /// <summary>
        /// Tests that when <see cref="Base{Framework}.SortedChildren"/> is called for a <see cref="Project"/> object
        /// not comprising children, it returns the empty collection.
        /// </summary>
        [CultureFact]
        public void SortedChildren_NotComprisingChildren_ReturnsEmptyCollection()
        {
            // Act
            var result = DefaultValue.SortedChildren;

            // Assert
            _ = result.Should().BeEmpty();
        }

        /// <summary>
        /// Tests that when <see cref="Base{Framework}.SortedChildren"/> is called for a <see cref="Project"/> object
        /// comprising children without children, it returns the empty collection.
        /// </summary>
        [CultureFact]
        public void SortedChildren_ComprisingChildrenWithoutChildren_ReturnsEmptyCollection()
        {
            // Arrange
            var project = new Project(DefaultIdentifier, 6);
            project.Add(new(SortedChildrenTestData[5].Identifier, NoDependencies));
            project.Add(new(SortedChildrenTestData[4].Identifier, NoDependencies));
            project.Add(new(SortedChildrenTestData[1].Identifier, NoDependencies));
            project.Add(new(SortedChildrenTestData[3].Identifier, NoDependencies));
            project.Add(new(SortedChildrenTestData[2].Identifier, NoDependencies));
            project.Add(new(SortedChildrenTestData[0].Identifier, NoDependencies));

            // Act
            var result = project.SortedChildren;

            // Assert
            _ = result.Should().BeEmpty();
        }

        /// <summary>
        /// Tests that when <see cref="Base{Framework}.SortedChildren"/> is called for a <see cref="Project"/> object
        /// comprising children with children, it returns the sorted collection of children.
        /// </summary>
        [CultureFact]
        public void SortedChildren_ComprisingChildrenWithChildren_ReturnsSortedChildren()
        {
            // Arrange
            var project = new Project(DefaultIdentifier, 6);
            project.Add(SortedChildrenTestData[5]);
            project.Add(SortedChildrenTestData[4]);
            project.Add(SortedChildrenTestData[1]);
            project.Add(SortedChildrenTestData[3]);
            project.Add(SortedChildrenTestData[2]);
            project.Add(SortedChildrenTestData[0]);

            // Act
            var result = project.SortedChildren;

            // Assert
            _ = result.Should().Equal(SortedChildrenTestData);
        }

        /// <summary>
        /// Tests that when <see cref="Base{Framework}.SortedChildren"/> is called for a <see cref="Project"/> object
        /// comprising children for which a subset have children, it returns the sorted collection of children.
        /// </summary>
        [CultureFact]
        public void SortedChildren_ComprisingChildrenWithSubsetHavingChildren_ReturnsSortedCollection()
        {
            // Arrange
            var project = new Project(DefaultIdentifier, 6);
            project.Add(SortedChildrenTestData[5]);
            project.Add(new(SortedChildrenTestData[4].Identifier, NoDependencies));
            project.Add(new(SortedChildrenTestData[1].Identifier, NoDependencies));
            project.Add(SortedChildrenTestData[3]);
            project.Add(new(SortedChildrenTestData[2].Identifier, NoDependencies));
            project.Add(SortedChildrenTestData[0]);

            // Act
            var result = project.SortedChildren;

            // Assert
            _ = result.Should().Equal(
                SortedChildrenTestData[0],
                SortedChildrenTestData[3],
                SortedChildrenTestData[5]);
        }
    }
}
