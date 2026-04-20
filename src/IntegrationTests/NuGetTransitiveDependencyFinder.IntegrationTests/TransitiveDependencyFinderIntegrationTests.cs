// <copyright file="TransitiveDependencyFinderIntegrationTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.IntegrationTests;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NuGetTransitiveDependencyFinder.Extensions;
using NuGetTransitiveDependencyFinder.Output;
using Xunit;

/// <summary>
/// Integration tests that exercise the public <see cref="ITransitiveDependencyFinder"/> façade end-to-end against the
/// TestCollateral projects. These tests run the real library against real project files; they do not mock the
/// dependency-analysis pipeline.
/// </summary>
[Collection("TestCollateral")]
public sealed partial class TransitiveDependencyFinderIntegrationTests
{
    /// <summary>
    /// The logging builder configuration used by all integration tests. Suppresses output via
    /// <see cref="NullLoggerProvider"/>.
    /// </summary>
    private static readonly Action<ILoggingBuilder> LoggingBuilderAction =
        configure => configure.AddProvider(NullLoggerProvider.Instance);

    /// <summary>
    /// Tests that running the finder against a project with no transitive dependencies returns an empty result set.
    /// </summary>
    [Fact]
    public void Run_WithNoTransitiveDependenciesProject_ReturnsEmptyResult()
    {
        // Arrange
        using var finder = CreateFinder();

        // Act
        var result = finder.Run(TestCollateralPaths.NoTransitiveDependenciesProject, false, null);

        // Assert
        _ = result
            .Should().NotBeNull();
        _ = EnumerateDependencies(result)
            .Should().BeEmpty();
    }

    /// <summary>
    /// Tests that running the finder against a project with known transitive dependencies returns a non-null result.
    /// The count may be zero if no transitive dependencies are surfaced in the restored graph.
    /// </summary>
    [Fact]
    public void Run_WithTransitiveDependenciesProject_ReturnsNonNullResult()
    {
        // Arrange
        using var finder = CreateFinder();

        // Act
        var result = finder.Run(TestCollateralPaths.TransitiveDependenciesProject, false, null);

        // Assert
        _ = result
            .Should().NotBeNull();
    }

    /// <summary>
    /// Tests that when a regex filter that does not match any dependency is supplied, all dependencies are filtered
    /// out.
    /// </summary>
    [Fact]
    public void Run_WithRegexFilterThatMatchesNothing_ReturnsEmptyDependencyLists()
    {
        // Arrange
        using var finder = CreateFinder();
        var filter = ImpossibleFilter();

        // Act
        var result = finder.Run(TestCollateralPaths.TransitiveDependenciesProject, false, filter);

        // Assert
        _ = EnumerateDependencies(result)
            .Should().BeEmpty();
    }

    /// <summary>
    /// Tests that <see cref="ITransitiveDependencyFinder.Run(string?, bool, Regex?)"/> with a
    /// <see langword="null"/> path throws <see cref="ArgumentNullException"/>.
    /// </summary>
    [Fact]
    public void Run_WithNullPath_ThrowsArgumentNullException()
    {
        // Arrange
        using var finder = CreateFinder();

        // Act
        Action action = () => finder.Run(null, false, null);

        // Assert
        _ = action
            .Should().Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Tests that <see cref="ITransitiveDependencyFinder.Run(string?, bool, Regex?)"/> with an invalid path
    /// propagates an exception rather than silently returning.
    /// </summary>
    [Fact]
    public void Run_WithInvalidPath_ThrowsException()
    {
        // Arrange
        using var finder = CreateFinder();
        var missing = Path.Combine(
            Path.GetTempPath(),
            $"nuget-tdf-integration-{Guid.NewGuid()}.csproj");

        // Act
        Action action = () => finder.Run(missing, false, null);

        // Assert
        _ = action
            .Should().Throw<Exception>();
    }

    /// <summary>
    /// Tests that when <c>collateAllDependencies</c> is <see langword="true"/>, the result contains at least as many
    /// dependencies as when it is <see langword="false"/>.
    /// </summary>
    [Fact]
    public void Run_WithCollateAllDependenciesTrue_ReturnsAtLeastAsManyAsTransitiveOnly()
    {
        // Arrange
        using var finder = CreateFinder();
        var transitive = EnumerateDependencies(
            finder.Run(TestCollateralPaths.TransitiveDependenciesProject, false, null)).ToList();
        var all = EnumerateDependencies(
            finder.Run(TestCollateralPaths.TransitiveDependenciesProject, true, null)).ToList();

        // Assert
        _ = all.Count
            .Should().BeGreaterThanOrEqualTo(transitive.Count);
    }

    /// <summary>
    /// Returns a regex that cannot match any realistic dependency identifier.
    /// </summary>
    /// <returns>The regex.</returns>
    [GeneratedRegex("^ThisPackageDoesNotExist_zzzzz$")]
    private static partial Regex ImpossibleFilter();

    /// <summary>
    /// Flattens the nested projects/frameworks/dependencies hierarchy into a flat dependency enumeration.
    /// </summary>
    /// <param name="projects">The projects root.</param>
    /// <returns>An enumerable of all dependencies across all projects and frameworks.</returns>
    private static IEnumerable<Dependency> EnumerateDependencies(Projects projects) =>
        projects.SortedChildren
            .SelectMany(project => project.SortedChildren)
            .SelectMany(framework => framework.SortedChildren);

    /// <summary>
    /// Constructs a fresh <see cref="ITransitiveDependencyFinder"/> instance from the DI container configured by
    /// <see cref="ServiceCollectionExtensions.AddNuGetTransitiveDependencyFinder(IServiceCollection?,
    /// Action{ILoggingBuilder}?)"/>.
    /// </summary>
    /// <returns>A disposable <see cref="ITransitiveDependencyFinder"/>.</returns>
    private static ITransitiveDependencyFinder CreateFinder() =>
        new ServiceCollection()
            .AddNuGetTransitiveDependencyFinder(LoggingBuilderAction)
            .BuildServiceProvider()
            .GetRequiredService<ITransitiveDependencyFinder>();
}
