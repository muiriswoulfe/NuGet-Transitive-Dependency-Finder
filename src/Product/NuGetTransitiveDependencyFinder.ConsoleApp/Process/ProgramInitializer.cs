// <copyright file="ProgramInitializer.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ConsoleApp.Process
{
    using System;
    using CommandLine;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Console;
    using NuGetTransitiveDependencyFinder.ConsoleApp.Input;
    using NuGetTransitiveDependencyFinder.ConsoleApp.Output;
    using NuGetTransitiveDependencyFinder.Extensions;

    /// <summary>
    /// The class internally defining the application logic flow and which initializes the main application logic.
    /// </summary>
    internal static class ProgramInitializer
    {
        /// <summary>
        /// The <see cref="Action{ILoggerBuilder}"/>, which defines the logging action for both the current application
        /// and the NuGet Transitive Dependency Finder library.
        /// </summary>
        private static readonly Action<ILoggingBuilder> LoggingBuilderAction =
            configure => configure
                .AddConsole(configure => configure.FormatterName = nameof(PlainConsoleFormatter))
                .AddDebug()
                .AddConsoleFormatter<PlainConsoleFormatter, ConsoleFormatterOptions>()
                .SetMinimumLevel(LogLevel.Trace);

        /// <summary>
        /// Runs the main application logic.
        /// </summary>
        /// <param name="parameters">A collection of command-line parameters.</param>
        /// <param name="programAction">The application logic to run against the <see cref="IServiceProvider"/>
        /// object.</param>
        /// <returns>A status code where 0 represents success and 1 represents failure.</returns>
        public static int Run(string[] parameters, Action<IServiceProvider> programAction)
        {
            const int success = 0;
            const int error = 1;

            return Parser.Default.ParseArguments<CommandLineOptions>(parameters)
                .MapResult(
                    commandLineOptions =>
                    {
                        using var serviceProvider = CreateServiceProvider(commandLineOptions);
                        programAction(serviceProvider);

                        return success;
                    },
                    _ =>
                        error);
        }

        /// <summary>
        /// Gets the <see cref="IProgramRunner"/> from the <see cref="IServiceProvider"/> dependency injection
        /// container.
        /// </summary>
        /// <param name="serviceProvider">The dependency injection container.</param>
        /// <returns>The <see cref="IProgramRunner"/> object.</returns>
        public static IProgramRunner GetProgramRunner(IServiceProvider serviceProvider) =>
            serviceProvider.GetService<IProgramRunner>()!;

        /// <summary>
        /// Creates the service provider, which initializes dependency injection for the application.
        /// </summary>
        /// <param name="commandLineOptions">The command-line options.</param>
        /// <returns>The service provider, which specifies the project dependencies.</returns>
        private static ServiceProvider CreateServiceProvider(ICommandLineOptions commandLineOptions) =>
            new ServiceCollection()
                .AddLogging(LoggingBuilderAction)
                .AddScoped(_ => commandLineOptions)
                .AddScoped<IDependencyWriter, DependencyWriter>()
                .AddScoped<IProgramRunner, ProgramRunner>()
                .AddNuGetTransitiveDependencyFinder(LoggingBuilderAction)
                .BuildServiceProvider();
    }
}
