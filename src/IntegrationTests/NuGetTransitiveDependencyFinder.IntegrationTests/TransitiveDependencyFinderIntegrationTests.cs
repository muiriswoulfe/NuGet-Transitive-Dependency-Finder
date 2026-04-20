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
    /// Tests that running the finder twice against the same project yields results with equivalent dependency
    /// identifiers, demonstrating that the finder is safely re-runnable.
    /// </summary>
    [Fact]
    public void Run_CalledTwice_ReturnsSameDependencies()
    {
        // Arrange
        using var finder = CreateFinder();

        // Act
        var first = EnumerateDependencies(
            finder.Run(TestCollateralPaths.TransitiveDependenciesProject, true, null))
            .Select(dependency => dependency.Identifier).Order().ToList();
        var second = EnumerateDependencies(
            finder.Run(TestCollateralPaths.TransitiveDependenciesProject, true, null))
            .Select(dependency => dependency.Identifier).Order().ToList();

        // Assert
        _ = second
            .Should().Equal(first);
    }

    /// <summary>
    /// Tests that two independent <see cref="ITransitiveDependencyFinder"/> instances from the DI container produce
    /// identical results when run against the same project.
    /// </summary>
    [Fact]
    public void Run_WithTwoInstances_ProducesEquivalentResults()
    {
        // Arrange
        using var first = CreateFinder();
        using var second = CreateFinder();

        // Act
        var firstResult = EnumerateDependencies(
            first.Run(TestCollateralPaths.NoTransitiveDependenciesProject, true, null))
            .Select(dependency => dependency.Identifier).Order().ToList();
        var secondResult = EnumerateDependencies(
            second.Run(TestCollateralPaths.NoTransitiveDependenciesProject, true, null))
            .Select(dependency => dependency.Identifier).Order().ToList();

        // Assert
        _ = secondResult
            .Should().Equal(firstResult);
    }

    /// <summary>
    /// Tests that <see cref="ITransitiveDependencyFinder"/> is correctly resolved from the DI container as a
    /// disposable instance.
    /// </summary>
    [Fact]
    public void ITransitiveDependencyFinder_ResolvedFromContainer_IsDisposable()
    {
        // Arrange
        var finder = CreateFinder();

        // Act
        Action action = finder.Dispose;

        // Assert
        _ = action
            .Should().NotThrow();
    }

    /// <summary>
    /// Tests that <see cref="ITransitiveDependencyFinder.Run(string?, bool, Regex?)"/> called after
    /// <see cref="IDisposable.Dispose"/> throws an <see cref="ObjectDisposedException"/> because the underlying
    /// <c>IServiceProvider</c> has been torn down.
    /// </summary>
    [Fact]
    public void Run_AfterDispose_ThrowsObjectDisposedException()
    {
        // Arrange
        var finder = CreateFinder();
        finder.Dispose();

        // Act
        Action action = () => finder.Run(TestCollateralPaths.NoTransitiveDependenciesProject, false, null);

        // Assert
        _ = action
            .Should().Throw<ObjectDisposedException>();
    }

    /// <summary>
    /// Tests that <see cref="IDisposable.Dispose"/> can be called twice on the same finder without throwing,
    /// verifying idempotent disposal.
    /// </summary>
    [Fact]
    public void Dispose_CalledTwice_DoesNotThrow()
    {
        // Arrange
        var finder = CreateFinder();

        // Act
        finder.Dispose();
        Action action = finder.Dispose;

        // Assert
        _ = action
            .Should().NotThrow();
    }

    /// <summary>
    /// Tests that running the finder against the TestCollateral solution file (<c>.sln</c>) — which contains multiple
    /// projects — returns a result that includes a project entry for each project in the solution, exercising the
    /// multi-project code path that per-project runs cannot.
    /// </summary>
    [Fact]
    public void Run_AgainstSolution_ReturnsMultipleProjects()
    {
        // Arrange
        using var finder = CreateFinder();

        // Act
        var result = finder.Run(TestCollateralPaths.TestCollateralSolution, true, null);

        // Assert
        _ = result
            .Should().NotBeNull();
        _ = result.SortedChildren
            .Should().HaveCountGreaterThanOrEqualTo(2, "the TestCollateral solution references both the " +
                "NoTransitiveDependencies and TransitiveDependencies projects");
    }

    /// <summary>
    /// Tests that running the finder with a regex filter that matches a real dependency actually surfaces that
    /// dependency when <c>collateAllDependencies</c> is <see langword="true"/>, verifying that the filter is applied
    /// in the positive case (all existing filter tests exercise only the empty-result case).
    /// </summary>
    [Fact]
    public void Run_WithFilterMatchingRealDependency_ReturnsOnlyMatchingDependencies()
    {
        // Arrange
        using var finder = CreateFinder();
        var allDependencyIdentifiers = EnumerateDependencies(
            finder.Run(TestCollateralPaths.TransitiveDependenciesProject, true, null))
            .Select(dependency => dependency.Identifier)
            .Distinct()
            .ToList();
        allDependencyIdentifiers
            .Should().NotBeEmpty("this test requires at least one real dependency to match against");
        var target = allDependencyIdentifiers[0];
        var filter = new Regex($"^{Regex.Escape(target)}$");

        // Act
        var filtered = EnumerateDependencies(
            finder.Run(TestCollateralPaths.TransitiveDependenciesProject, true, filter))
            .ToList();

        // Assert
        _ = filtered
            .Should().NotBeEmpty();
        _ = filtered
            .Should().OnlyContain(dependency => dependency.Identifier == target);
    }

    /// <summary>
    /// Tests that two <see cref="ITransitiveDependencyFinder"/> instances run concurrently on different threads
    /// against the same project both complete successfully and produce identical results, verifying that the library
    /// is safe to use in multi-finder concurrent scenarios (each finder has its own service provider).
    /// </summary>
    [Fact]
    public async System.Threading.Tasks.Task Run_ConcurrentlyOnSeparateInstances_ProducesEquivalentResultsAsync()
    {
        // Arrange
        var tasks = Enumerable.Range(0, 4).Select(_ => System.Threading.Tasks.Task.Run(() =>
        {
            using var finder = CreateFinder();
            return EnumerateDependencies(
                finder.Run(TestCollateralPaths.TransitiveDependenciesProject, true, null))
                .Select(dependency => dependency.Identifier)
                .Order()
                .ToList();
        })).ToArray();

        // Act
        var results = await System.Threading.Tasks.Task.WhenAll(tasks);

        // Assert
        var baseline = results[0];
        foreach (var result in results)
        {
            _ = result
                .Should().Equal(baseline);
        }
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
