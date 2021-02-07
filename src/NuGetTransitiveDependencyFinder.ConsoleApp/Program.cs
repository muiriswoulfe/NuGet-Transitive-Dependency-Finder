// <copyright file="Program.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ConsoleApp
{
    using System;
    using CommandLine;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Console;
    using NuGetTransitiveDependencyFinder.ConsoleApp.Input;
    using NuGetTransitiveDependencyFinder.ConsoleApp.Output;

    /// <summary>
    /// The main class of the application, defining the entry point and the basic operation of the application.
    /// </summary>
    internal static class Program
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
        /// The entry point of the application.
        /// </summary>
        /// <param name="parameters">A collection of command-line parameters.</param>
        /// <returns>A status code where 0 represents success and 1 represents failure.</returns>
        public static int Main(string[] parameters)
        {
            const int success = 0;
            const int error = 1;

            return Parser.Default.ParseArguments<CommandLineOptions>(parameters)
                .MapResult(
                    commandLineOptions =>
                    {
                        using var serviceProvider = CreateServiceProvider(commandLineOptions);
                        serviceProvider.GetService<ProgramRunner>()!.Run();

                        return success;
                    },
                    _ =>
                        error);
        }

        /// <summary>
        /// Creates the service provider, which initializes dependency injection for the application.
        /// </summary>
        /// <param name="commandLineOptions">The command-line options.</param>
        /// <returns>The service provider, which specifies the project dependencies.</returns>
        private static ServiceProvider CreateServiceProvider(CommandLineOptions commandLineOptions) =>
            new ServiceCollection()
                .AddLogging(LoggingBuilderAction)
                .AddScoped(_ => commandLineOptions)
                .AddScoped<DependencyWriter>()
                .AddScoped<ProgramRunner>()
                .AddScoped<TransitiveDependencyFinder>()
                .AddSingleton(LoggingBuilderAction)
                .BuildServiceProvider();
    }
}
