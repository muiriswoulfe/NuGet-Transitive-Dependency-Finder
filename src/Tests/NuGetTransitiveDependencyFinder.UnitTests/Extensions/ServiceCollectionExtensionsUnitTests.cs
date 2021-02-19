// <copyright file="ServiceCollectionExtensionsUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Extensions
{
    using System;
    using FluentAssertions;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using NuGetTransitiveDependencyFinder.Extensions;
    using NuGetTransitiveDependencyFinder.TestUtilities.Globalization;

    /// <summary>
    /// Unit tests for the <see cref="ServiceCollectionExtensions"/> class.
    /// </summary>
    public class ServiceCollectionExtensionsUnitTests
    {
        /// <summary>
        /// The logging builder action for use within the tests.
        /// </summary>
        private static readonly Action<ILoggingBuilder> LoggingBuilderAction =
            configure => configure.SetMinimumLevel(LogLevel.Trace);

        /// <summary>
        /// Tests that when
        /// <see cref="ServiceCollectionExtensions.AddNuGetTransitiveDependencyFinder(IServiceCollection,
        /// Action{ILoggingBuilder})"/> is called, it adds the expected dependencies.
        /// </summary>
        [AllCulturesFact]
        public void AddNuGetTransitiveDependencyFinder_Called_AddsDependencies()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            using var serviceProvider = serviceCollection.AddNuGetTransitiveDependencyFinder(LoggingBuilderAction)
                .BuildServiceProvider();

            // Assert
            _ = serviceProvider.GetService<ITransitiveDependencyFinder>()
                .Should().BeOfType<TransitiveDependencyFinder>();
            _ = serviceProvider.GetService<Action<ILoggingBuilder>>()
                .Should().Be(LoggingBuilderAction);
        }
    }
}
