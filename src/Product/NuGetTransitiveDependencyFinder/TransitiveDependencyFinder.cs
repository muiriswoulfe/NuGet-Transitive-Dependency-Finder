// <copyright file="TransitiveDependencyFinder.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
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
    internal sealed class TransitiveDependencyFinder : ITransitiveDependencyFinder
    {
        /// <summary>
        /// The service provider, which specifies the project dependencies.
        /// </summary>
        private readonly ServiceProvider serviceProvider;

        /// <summary>
        /// A value tracking whether <see cref="Dispose()"/> has been invoked.
        /// </summary>
        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransitiveDependencyFinder"/> class.
        /// </summary>
        /// <param name="loggingBuilderAction">The logging builder action, which will be used for initializing the .NET
        /// logging infrastructure.</param>
        public TransitiveDependencyFinder(Action<ILoggingBuilder> loggingBuilderAction) =>
            this.serviceProvider = CreateServiceProvider(loggingBuilderAction);

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
        public Task<Projects> RunAsync(string? projectOrSolutionPath, bool collateAllDependencies)
        {
            if (projectOrSolutionPath == null)
            {
                throw new ArgumentNullException(nameof(projectOrSolutionPath));
            }

            return this.serviceProvider
                .GetService<IDependencyFinder>()!
                .RunAsync(projectOrSolutionPath, collateAllDependencies);
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
                .AddScoped<Process, Process>()
                .AddTransient<IDependencyGraph, DependencyGraph>()
                .BuildServiceProvider();

        /// <summary>
        /// Disposes of the resources maintained by the current object.
        /// </summary>
        /// <param name="disposing">A value indicating whether disposal should be performed for managed as well as
        /// unmanaged resources.</param>
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
}
