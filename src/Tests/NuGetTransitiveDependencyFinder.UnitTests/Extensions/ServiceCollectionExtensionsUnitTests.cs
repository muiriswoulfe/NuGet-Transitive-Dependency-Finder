// <copyright file="ServiceCollectionExtensionsUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Extensions
{
    using System;
    using System.Diagnostics;
    using FluentAssertions;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using NuGetTransitiveDependencyFinder.Extensions;
    using NuGetTransitiveDependencyFinder.ProjectAnalysis;
    using NuGetTransitiveDependencyFinder.TestUtilities.Globalization;

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
            _ = result.GetService<IProcessWrapper>()
                .Should().BeOfType<ProcessWrapper>();
            _ = result.GetService<Process>()
                .Should().BeOfType<Process>();
            _ = result.GetService<Action<ILoggingBuilder>>()
                .Should().Be(LoggingBuilderAction);
        }
    }
}
