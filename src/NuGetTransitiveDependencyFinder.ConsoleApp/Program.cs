// <copyright file="Program.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ConsoleApp
{
    using CommandLine;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Console;
    using NuGetTransitiveDependencyFinder.ConsoleApp.Input;
    using NuGetTransitiveDependencyFinder.ConsoleApp.Output;
    using NuGetTransitiveDependencyFinder.ConsoleApp.Resources.Messages;

    /// <summary>
    /// The main class of the application, defining the entry point and all interaction.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The entry point of the application.
        /// </summary>
        /// <param name="parameters">A collection of command-line parameters.</param>
        /// <returns>A status code where 0 represents success and 1 indicates failure.</returns>
        public static int Main(string[] parameters)
        {
            const int success = 0;
            const int error = 1;

            return Parser.Default.ParseArguments<CommandLineOptions>(parameters)
                .MapResult(
                    options =>
                    {
                        using var loggerFactory = CreateLoggerFactory();
                        var logger = loggerFactory.CreateLogger(nameof(Program));
                        logger.LogInformation(Information.CommencingAnalysis);

                        var finder = new TransitiveDependencyFinder(loggerFactory);
                        var projects = finder.Run(options.Solution!, options.All);
                        var writer = new DependencyWriter(loggerFactory);
                        writer.Write(projects);

                        return success;
                    },
                    _ =>
                        error);
        }

        /// <summary>
        /// Creates the logger factory from which a logger will be created.
        /// </summary>
        /// <returns>The logger factory.</returns>
        private static ILoggerFactory CreateLoggerFactory() =>
            LoggerFactory.Create(builder =>
            {
                builder.AddConsole(options => options.FormatterName = nameof(PlainConsoleFormatter))
                    .AddConsoleFormatter<PlainConsoleFormatter, ConsoleFormatterOptions>()
                    .SetMinimumLevel(LogLevel.Trace);
            });
    }
}
