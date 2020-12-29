// <copyright file="ProjectsUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Output
{
    using System.Collections.Generic;
    using FluentAssertions;
    using NuGetTransitiveDependencyFinder.Output;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="Projects"/> class.
    /// </summary>
    public class ProjectsUnitTests
    {
        /// <summary>
        /// The default test identifier.
        /// </summary>
        private const string DefaultIdentifier = "Identifier";

        /// <summary>
        /// The default test value.
        /// </summary>
        private static readonly Projects DefaultValue = new(0);

        /// <summary>
        /// The default test value for a <see cref="Framework"/> object.
        /// </summary>
        private static readonly Framework DefaultFramework =
            new(new("Framework", new("1.0.0")), new Dependency[] { new("Dependency", new("1.0.0")) });

        /// <summary>
        /// The default test value for a <see cref="Project"/> object.
        /// </summary>
        private static readonly Project DefaultProject = CreateDefaultProject();

        /// <summary>
        /// The default test value for a <see cref="Project"/> object without children.
        /// </summary>
        private static readonly Project DefaultProjectWithoutChildren = new(DefaultIdentifier, 0);

        /// <summary>
        /// The data for testing <see cref="Base{Project}.SortedChildren"/>.
        /// </summary>
        private static readonly IReadOnlyList<Project> SortedChildrenTestData = CreateSortedChildrenTestData();

        /// <summary>
        /// Creates the default test value for a <see cref="Project"/> object.
        /// </summary>
        /// <returns>The default test <see cref="Project"/> object.</returns>
        private static Project CreateDefaultProject()
        {
            var result = new Project(DefaultIdentifier, 1);
            result.Add(DefaultFramework);

            return result;
        }

        /// <summary>
        /// Creates the data for testing <see cref="Base{Project}.SortedChildren"/>.
        /// </summary>
        /// <returns>The data for testing <see cref="Base{Project}.SortedChildren"/>.</returns>
        private static IReadOnlyList<Project> CreateSortedChildrenTestData()
        {
            var result = new Project[]
            {
                new("A", 1),
                new("A", 1),
                new("B", 1),
                new("C", 1),
                new("Y", 1),
                new("Z", 1),
            };

            foreach (var project in result)
            {
                project.Add(DefaultFramework);
            }

            return result;
        }

        /// <summary>
        /// Tests than when <see cref="Projects.IsAddValid(Project?)"/> is called with a <see cref="Project"/> with
        /// children, it returns <c>true</c>.
        /// </summary>
        [Fact]
        public void IsAddValid_WithProjectWithChildren_ReturnsTrue()
        {
            // Act
            var result = DefaultValue.IsAddValid(DefaultProject);

            // Assert
            _ = result.Should().BeTrue();
        }

        /// <summary>
        /// Tests than when <see cref="Projects.IsAddValid(Project?)"/> is called with a <see cref="Project"/> without
        /// children, it returns <c>false</c>.
        /// </summary>
        [Fact]
        public void IsAddValid_WithProjectWithoutChildren_ReturnsFalse()
        {
            // Act
            var result = DefaultValue.IsAddValid(new(DefaultIdentifier, 0));

            // Assert
            _ = result.Should().BeFalse();
        }

        /// <summary>
        /// Tests that when <see cref="Base{Project}.HasChildren"/> is called for a <see cref="Projects"/> object
        /// without children, it returns <c>false</c>.
        /// </summary>
        [Fact]
        public void HasChildren_WithoutChildren_ReturnsFalse()
        {
            // Act
            var result = DefaultValue.HasChildren;

            // Assert
            _ = result.Should().BeFalse();
        }

        /// <summary>
        /// Tests that when <see cref="Base{Project}.HasChildren"/> is called for a <see cref="Projects"/> object with
        /// children containing no children, it returns <c>false</c>.
        /// </summary>
        [Fact]
        public void HasChildren_WithChildrenContainingNoChildren_ReturnsFalse()
        {
            // Arrange
            var projects = new Projects(1);
            projects.Add(DefaultProjectWithoutChildren);

            // Act
            var result = projects.HasChildren;

            // Assert
            _ = result.Should().BeFalse();
        }

        /// <summary>
        /// Tests that when <see cref="Base{Project}.HasChildren"/> is called for a <see cref="Projects"/> object with
        /// children containing children, it returns <c>true</c>.
        /// </summary>
        [Fact]
        public void HasChildren_WithChildrenContainingChildren_ReturnsTrue()
        {
            // Arrange
            var projects = new Projects(1);
            projects.Add(DefaultProject);

            // Act
            var result = projects.HasChildren;

            // Assert
            _ = result.Should().BeTrue();
        }

        /// <summary>
        /// Tests that when <see cref="Base{Project}.HasChildren"/> is called for a <see cref="Projects"/> object with
        /// some children containing children, it returns <c>true</c>.
        /// </summary>
        [Fact]
        public void HasChildren_WithSomeChildrenContainingChildren_ReturnsTrue()
        {
            // Arrange
            var projects = new Projects(2);
            projects.Add(DefaultProjectWithoutChildren);
            projects.Add(DefaultProject);

            // Act
            var result = projects.HasChildren;

            // Assert
            _ = result.Should().BeTrue();
        }

        /// <summary>
        /// Tests that when <see cref="Base{Project}.SortedChildren"/> is called for a <see cref="Projects"/> object
        /// without children, it returns the empty collection.
        /// </summary>
        [Fact]
        public void SortedChildren_WithoutChildren_ReturnsEmptyCollection()
        {
            // Act
            var result = DefaultValue.SortedChildren;

            // Assert
            _ = result.Should().BeEmpty();
        }

        /// <summary>
        /// Tests that when <see cref="Base{Project}.SortedChildren"/> is called for a <see cref="Projects"/> object
        /// with children containing no children, it returns the empty collection.
        /// </summary>
        [Fact]
        public void SortedChildren_WithChildrenContainingNoChildren_ReturnsEmptyCollection()
        {
            // Arrange
            var projects = new Projects(6);
            projects.Add(new(SortedChildrenTestData[5].Identifier, 0));
            projects.Add(new(SortedChildrenTestData[4].Identifier, 0));
            projects.Add(new(SortedChildrenTestData[1].Identifier, 0));
            projects.Add(new(SortedChildrenTestData[3].Identifier, 0));
            projects.Add(new(SortedChildrenTestData[2].Identifier, 0));
            projects.Add(new(SortedChildrenTestData[0].Identifier, 0));

            // Act
            var result = projects.SortedChildren;

            // Assert
            _ = result.Should().BeEmpty();
        }

        /// <summary>
        /// Tests that when <see cref="Base{Project}.SortedChildren"/> is called for a <see cref="Projects"/> object
        /// with children containing children, it returns the sorted collection of children.
        /// </summary>
        [Fact]
        public void SortedChildren_WithChildrenContainingChildren_ReturnsSortedChildren()
        {
            // Arrange
            var projects = new Projects(6);
            projects.Add(SortedChildrenTestData[5]);
            projects.Add(SortedChildrenTestData[4]);
            projects.Add(SortedChildrenTestData[1]);
            projects.Add(SortedChildrenTestData[3]);
            projects.Add(SortedChildrenTestData[2]);
            projects.Add(SortedChildrenTestData[0]);

            // Act
            var result = projects.SortedChildren;

            // Assert
            _ = result.Should().Equal(SortedChildrenTestData);
        }

        /// <summary>
        /// Tests that when <see cref="Base{Project}.SortedChildren"/> is called for a <see cref="Projects"/> object
        /// with some children containing children, it returns the sorted collection of children of those children.
        /// </summary>
        [Fact]
        public void SortedChildren_WithSomeChildrenContainingChildren_ReturnsSortedCollection()
        {
            // Arrange
            var projects = new Projects(6);
            projects.Add(SortedChildrenTestData[5]);
            projects.Add(new(SortedChildrenTestData[4].Identifier, 0));
            projects.Add(new(SortedChildrenTestData[1].Identifier, 0));
            projects.Add(SortedChildrenTestData[3]);
            projects.Add(new(SortedChildrenTestData[2].Identifier, 0));
            projects.Add(SortedChildrenTestData[0]);

            // Act
            var result = projects.SortedChildren;

            // Assert
            _ = result.Should().Equal(
                SortedChildrenTestData[0],
                SortedChildrenTestData[3],
                SortedChildrenTestData[5]);
        }
    }
}
