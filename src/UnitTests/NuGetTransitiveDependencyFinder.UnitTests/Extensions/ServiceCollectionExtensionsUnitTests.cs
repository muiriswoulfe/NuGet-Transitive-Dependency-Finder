// <copyright file="ServiceCollectionExtensionsUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Extensions;

using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NuGetTransitiveDependencyFinder.Extensions;
using NuGetTransitiveDependencyFinder.UnitTests.Utilities.Globalization;

/// <summary>
/// Unit tests for the <see cref="ServiceCollectionExtensions"/> class.
/// </summary>
public class ServiceCollectionExtensionsUnitTests
{
    /// <summary>
    /// The logging builder action for use within the unit tests.
    /// </summary>
    private static readonly Action<ILoggingBuilder> LoggingBuilderAction =
        configure => configure.SetMinimumLevel(LogLevel.Trace);

    /// <summary>
    /// Tests that when
    /// <see cref="ServiceCollectionExtensions.AddNuGetTransitiveDependencyFinder(IServiceCollection,
    /// Action{ILoggingBuilder})"/> is called with a <see langword="null"/> <c>value</c> parameter, it throws an
    /// <see cref="ArgumentNullException"/>.
    /// </summary>
    [AllCulturesFact]
    public void AddNuGetTransitiveDependencyFinder_WithNullValue_ThrowsArgumentNullException()
    {
        // Arrange
        IServiceCollection? serviceCollection = null;

        // Act
        Action action = () => serviceCollection.AddNuGetTransitiveDependencyFinder(LoggingBuilderAction);

        // Assert
        _ = action
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("value");
    }

    /// <summary>
    /// Tests that when
    /// <see cref="ServiceCollectionExtensions.AddNuGetTransitiveDependencyFinder(IServiceCollection,
    /// Action{ILoggingBuilder})"/> is called with a <see langword="null"/> <c>loggingBuilderAction</c> parameter,
    /// it throws an <see cref="ArgumentNullException"/>.
    /// </summary>
    [AllCulturesFact]
    public void AddNuGetTransitiveDependencyFinder_WithNullLoggingBuilderAction_ThrowsArgumentNullException()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();

        // Act
        Action action = () => serviceCollection.AddNuGetTransitiveDependencyFinder(null);

        // Assert
        _ = action
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("loggingBuilderAction");
    }

    /// <summary>
    /// Tests that when
    /// <see cref="ServiceCollectionExtensions.AddNuGetTransitiveDependencyFinder(IServiceCollection,
    /// Action{ILoggingBuilder})"/> is called with valid parameters, it adds the expected dependencies.
    /// </summary>
    [AllCulturesFact]
    public void AddNuGetTransitiveDependencyFinder_WithValidParameters_AddsDependencies()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();

        // Act
        using var result = serviceCollection.AddNuGetTransitiveDependencyFinder(LoggingBuilderAction)
            .BuildServiceProvider();

        // Assert
        _ = result.GetService<ITransitiveDependencyFinder>()
            .Should().BeOfType<TransitiveDependencyFinder>();
        _ = result.GetService<Action<ILoggingBuilder>>()
            .Should().Be(LoggingBuilderAction);
    }

    /// <summary>
    /// Tests that <see cref="ServiceCollectionExtensions.AddNuGetTransitiveDependencyFinder(IServiceCollection,
    /// Action{ILoggingBuilder})"/> is fluent: it returns the same <see cref="IServiceCollection"/> instance it was
    /// called on.
    /// </summary>
    [AllCulturesFact]
    public void AddNuGetTransitiveDependencyFinder_ReturnsSameServiceCollection()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();

        // Act
        var result = serviceCollection.AddNuGetTransitiveDependencyFinder(LoggingBuilderAction);

        // Assert
        _ = result
            .Should().BeSameAs(serviceCollection);
    }

    /// <summary>
    /// Tests that <see cref="ITransitiveDependencyFinder"/> is registered with a transient lifetime, yielding a new
    /// instance on each resolve.
    /// </summary>
    [AllCulturesFact]
    public void AddNuGetTransitiveDependencyFinder_RegistersITransitiveDependencyFinderAsTransient()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        using var provider = serviceCollection
            .AddNuGetTransitiveDependencyFinder(LoggingBuilderAction)
            .BuildServiceProvider();

        // Act
        var first = provider.GetService<ITransitiveDependencyFinder>();
        var second = provider.GetService<ITransitiveDependencyFinder>();

        // Assert
        _ = first
            .Should().NotBeSameAs(second);
    }

    /// <summary>
    /// Tests that <see cref="Action{ILoggingBuilder}"/> is registered as a singleton, yielding the same instance on
    /// each resolve.
    /// </summary>
    [AllCulturesFact]
    public void AddNuGetTransitiveDependencyFinder_RegistersLoggingBuilderActionAsSingleton()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        using var provider = serviceCollection
            .AddNuGetTransitiveDependencyFinder(LoggingBuilderAction)
            .BuildServiceProvider();

        // Act
        var first = provider.GetService<Action<ILoggingBuilder>>();
        var second = provider.GetService<Action<ILoggingBuilder>>();

        // Assert
        _ = first
            .Should().BeSameAs(second);
    }
}
