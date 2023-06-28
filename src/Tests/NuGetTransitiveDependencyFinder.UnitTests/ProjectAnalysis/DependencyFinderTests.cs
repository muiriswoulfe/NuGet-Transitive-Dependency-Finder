// <copyright file="DependencyFinderTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.ProjectAnalysis;

using System.Text.RegularExpressions;
using FluentAssertions;
using Moq;
using NuGetTransitiveDependencyFinder.ProjectAnalysis;
using Xunit;

/// <summary>
/// Unit tests for the <see cref="DependencyFinder"/> class.
/// </summary>
public partial class DependencyFinderTests
{
    private readonly Mock<IAssets> assets;
    private readonly Mock<IDependencyGraph> dependencyGraph;
    private readonly DependencyFinder dependencyFinder;

    /// <summary>
    /// Initializes a new instance of the <see cref="DependencyFinderTests"/> class.
    /// </summary>
    public DependencyFinderTests()
    {
        this.assets = new Mock<IAssets>();
        this.dependencyGraph = new Mock<IDependencyGraph>();
        this.dependencyFinder = new DependencyFinder(this.assets.Object, this.dependencyGraph.Object);
    }

    [Fact]
    public void Run_WithInvalidPath_ReturnsNull()
    {
        // Arrange
        const string projectOrSolutionPath = "invalid/path/to/project";

        // Act
        var result = this.dependencyFinder.Run(projectOrSolutionPath, false, null);

        // Assert
        _ = result
            .Should().BeNull();
    }

    [Fact]
    public void Run_WithPath_ReturnsDependencies()
    {
        // Arrange
        const string projectOrSolutionPath = "valid/path/to/project";

        // Act
        var result = this.dependencyFinder.Run(projectOrSolutionPath, false, null);

        // Assert
        _ = result
            .Should().NotBeNull();
    }

    [Fact]
    public void Run_WithPathAndDependencyCollation_ReturnsDependencies()
    {
        // Arrange
        const string projectOrSolutionPath = "valid/path/to/project";

        // Act
        var result = this.dependencyFinder.Run(projectOrSolutionPath, true, null);

        // Assert
        _ = result
            .Should().NotBeNull();
    }

    [Fact]
    public void Run_WithPathAndFilter_ReturnsFilteredDependencies()
    {
        // Arrange
        const string projectOrSolutionPath = "valid/path/to/project";

        // Act
        var result = this.dependencyFinder.Run(projectOrSolutionPath, false, DependencyFilterRegex());

        // Assert
        _ = result
            .Should().NotBeNull();
    }

    [GeneratedRegex("Microsoft.*")]
    private static partial Regex DependencyFilterRegex();
}
