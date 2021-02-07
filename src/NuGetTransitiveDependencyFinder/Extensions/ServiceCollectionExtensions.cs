// <copyright file="ServiceCollectionExtensions.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.Extensions
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// A class containing extension methods for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds all public dependencies related to the NuGet Transitive Dependency Finder to the provided
        /// <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="value">The <see cref="IServiceCollection"/> to which to add the dependencies.</param>
        /// <param name="loggingBuilderAction">The <see cref="Action{ILoggerBuilder}"/>, which defines the logging
        /// action for both the current application and the NuGet Transitive Dependency Finder library.</param>
        /// <returns>The <see cref="IServiceCollection"/> passed to the method, with the dependencies added.</returns>
        public static IServiceCollection AddNuGetTransitiveDependencyFinder(
            this IServiceCollection value,
            Action<ILoggingBuilder> loggingBuilderAction) =>
            value
                .AddTransient<ITransitiveDependencyFinder, TransitiveDependencyFinder>()
                .AddSingleton(loggingBuilderAction);
    }
}
