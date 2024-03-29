// <copyright file="TransitiveDependencyFinder.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder;

using System;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NuGetTransitiveDependencyFinder.Output;
using NuGetTransitiveDependencyFinder.ProjectAnalysis;
using NuGetTransitiveDependencyFinder.Utilities;
using NuGetTransitiveDependencyFinder.Wrappers;
using INuGetLogger = NuGet.Common.ILogger;

/// <summary>
/// A class that manages the overall process of finding transitive NuGet dependencies.
/// </summary>
/// <param name="loggingBuilderAction">The logging builder action, which will be used for initializing the .NET
/// logging infrastructure.</param>
internal sealed class TransitiveDependencyFinder(
    Action<ILoggingBuilder> loggingBuilderAction) : ITransitiveDependencyFinder
{
    /// <summary>
    /// The service provider, which specifies the project dependencies.
    /// </summary>
    private readonly ServiceProvider serviceProvider = CreateServiceProvider(loggingBuilderAction);

    /// <summary>
    /// A value tracking whether <see cref="Dispose()"/> has been invoked.
    /// </summary>
    private bool disposedValue;

    /// <summary>
    /// Finalizes an instance of the <see cref="TransitiveDependencyFinder"/> class.
    /// </summary>
    ~TransitiveDependencyFinder() =>
        this.Dispose(false);

    /// <inheritdoc/>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    public Projects Run(string? projectOrSolutionPath, bool collateAllDependencies, Regex? filter)
    {
        if (projectOrSolutionPath == null)
        {
            throw new ArgumentNullException(nameof(projectOrSolutionPath));
        }

        return this.serviceProvider
            .GetService<IDependencyFinder>()!
            .Run(projectOrSolutionPath, collateAllDependencies, filter);
    }

    /// <summary>
    /// Creates the service provider, which initializes dependency injection for the application.
    /// </summary>
    /// <param name="loggingBuilderAction">The logging builder action, which will be used for initializing the .NET
    /// logging infrastructure.</param>
    /// <returns>The service provider, which specifies the project dependencies.</returns>
    private static ServiceProvider CreateServiceProvider(Action<ILoggingBuilder> loggingBuilderAction) =>
        new ServiceCollection()
            .AddLogging(loggingBuilderAction)
            .AddScoped<IAssets, Assets>()
            .AddScoped<IDependencyFinder, DependencyFinder>()
            .AddScoped<IDotNetRunner, DotNetRunner>()
            .AddScoped<ILockFileUtilitiesWrapper, LockFileUtilitiesWrapper>()
            .AddScoped<INuGetLogger, NuGetLogger>()
            .AddScoped<IProcessWrapper, ProcessWrapper>()
            .AddTransient<IDependencyGraph, DependencyGraph>()
            .BuildServiceProvider();

    /// <summary>
    /// Disposes of the resources maintained by the current object.
    /// </summary>
    /// <param name="disposing">A value indicating whether disposal should be performed for managed as well as unmanaged
    /// resources.</param>
    private void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                this.serviceProvider.Dispose();
            }

            this.disposedValue = true;
        }
    }
}
