// <copyright file="TransitiveDependencyFinderUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests;

using System;
using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NuGetTransitiveDependencyFinder.TestUtilities.Globalization;

/// <summary>
/// Unit tests for the <see cref="TransitiveDependencyFinder"/> class.
/// </summary>
public class TransitiveDependencyFinderUnitTests
{
    /// <summary>
    /// The logging builder action for use within the unit tests.
    /// </summary>
    private static readonly Action<ILoggingBuilder> LoggingBuilderAction =
        configure => configure.SetMinimumLevel(LogLevel.Trace);

    /// <summary>
    /// Tests that when <see cref="TransitiveDependencyFinder.Run(string?, bool, Regex?)"/> is called with a
    /// <see langword="null"/> <c>projectOrSolutionPath</c> parameter, it throws an <see cref="ArgumentNullException"/>.
    /// </summary>
    [AllCulturesFact]
    public void Run_WithNullProjectOrSolutionPath_ThrowsArgumentNullException()
    {
        // Arrange
        using var transitiveDependencyFinder = new TransitiveDependencyFinder(LoggingBuilderAction);

        // Act
        Action action = () => transitiveDependencyFinder.Run(null, true, null);

        // Assert
        _ = action
            .Should().Throw<ArgumentNullException>()
            .WithParameterName("projectOrSolutionPath");
    }

    /// <summary>
    /// Tests that when <see cref="TransitiveDependencyFinder.Dispose()"/> is called, it does not throw an exception.
    /// </summary>
    [AllCulturesFact]
    public void Dispose_Called_DoesNotThrowException()
    {
        // Arrange
        using var transitiveDependencyFinder = new TransitiveDependencyFinder(LoggingBuilderAction);

        // Act
        Action action = transitiveDependencyFinder.Dispose;

        // Assert
        _ = action
            .Should().NotThrow();
    }

    /// <summary>
    /// Tests that when <see cref="TransitiveDependencyFinder.Dispose()"/> is called multiple times, it does not throw
    /// an exception.
    /// </summary>
    [AllCulturesFact]
    public void Dispose_CalledMultipleTimes_DoesNotThrowException()
    {
        // Arrange
        using var transitiveDependencyFinder = new TransitiveDependencyFinder(LoggingBuilderAction);

        // Act
        Action action1 = transitiveDependencyFinder.Dispose;
        Action action2 = transitiveDependencyFinder.Dispose;
        Action action3 = transitiveDependencyFinder.Dispose;

        // Assert
        _ = action1
            .Should().NotThrow();
        _ = action2
            .Should().NotThrow();
        _ = action3
            .Should().NotThrow();
    }

    /// <summary>
    /// Tests that the finalizer runs successfully when invoked.
    /// </summary>
    [AllCulturesFact]
    public void Finalizer_Invoked_RunsSuccessfully()
    {
        // Arrange
        TransitiveDependencyFinder? transitiveDependencyFinder;
        var transitiveDependencyFinderReference = CreateWithWeakReference(() =>
        {
            var temporary = new TransitiveDependencyFinder(LoggingBuilderAction);
            transitiveDependencyFinder = temporary;
            return temporary;
        });

        // Act
        transitiveDependencyFinder = null;
        GC.Collect();

        // Assert
        _ = transitiveDependencyFinderReference.IsAlive
            .Should().BeFalse();
    }

    /// <summary>
    /// Creates a <see cref="WeakReference"/> to an object.
    /// </summary>
    /// <typeparam name="TReference">The type of the object to be constructed.</typeparam>
    /// <param name="factory">The factory to construct the object.</param>
    /// <returns>The object wrapped instead a <see cref="WeakReference"/>.</returns>
    private static WeakReference CreateWithWeakReference<TReference>(Func<TReference> factory) =>
        new(factory());
}
