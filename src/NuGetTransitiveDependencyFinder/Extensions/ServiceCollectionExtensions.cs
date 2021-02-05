// <copyright file="ServiceCollectionExtensions.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using NuGetTransitiveDependencyFinder.ProjectAnalysis;

    /// <summary>
    /// A class containing extension methods for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds all dependencies related to the NuGet Transitive Dependency Finder to the provided
        /// <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="value">The <see cref="IServiceCollection"/> to which to add the dependencies.</param>
        /// <returns>The <see cref="IServiceCollection"/> passed to the method, with the dependencies added.</returns>
        public static IServiceCollection AddNuGetTransitiveDependencyFinder(this IServiceCollection value) =>
            value.AddSingleton<Assets>()
                .AddSingleton<DependencyGraph>()
                .AddSingleton<DotNetRunner>()
                .AddSingleton<TransitiveDependencyFinder>();
    }
}
