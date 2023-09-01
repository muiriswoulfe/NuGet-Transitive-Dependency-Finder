// <copyright file="IntegrationTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.IntegrationTests;

using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;

/// <summary>
/// Integration tests for the NuGet Transitive Dependency Finder.
/// </summary>
public class IntegrationTests
{
    /// <summary>
    /// Tests the app using a solution.
    /// </summary>
    [Fact]
    public void TestSolution() =>
        this.Test("../../../../TestCollateral/TestCollateral.sln");

    /// <summary>
    /// Tests the app using a project with no transitive dependencies.
    /// </summary>
    [Fact]
    public void TestProjectWithNoTransitiveDependencies() =>
        this.Test("../../../../TestCollateral/NoTransitiveDependencies/NoTransitiveDependencies.csproj");

    /// <summary>
    /// Tests the app using a project with no transitive dependencies.
    /// </summary>
    [Fact]
    public void TestProjectWithTransitiveDependencies() =>
        this.Test("../../../../TestCollateral/TransitiveDependencies/TransitiveDependencies.csproj");

    /// <summary>
    /// Tests the app using the specified project or solution.
    /// </summary>
    /// <param name="path">The path to the project or solution.</param>
    private void Test(string path)
    {
        // Arrange
        var fullPath = Path.GetFullPath(path);
        static void LoggingBuilderAction(ILoggingBuilder configure) =>
            configure
                .AddConsole()
                .AddDebug()
                .SetMinimumLevel(LogLevel.Trace);
        var transitiveDependencyFinder = new TransitiveDependencyFinder(LoggingBuilderAction);

        // Act
        var projects = transitiveDependencyFinder.Run(fullPath, true, null);

        // Assert
        _ = projects.HasChildren
            .Should().BeFalse();
    }
}
