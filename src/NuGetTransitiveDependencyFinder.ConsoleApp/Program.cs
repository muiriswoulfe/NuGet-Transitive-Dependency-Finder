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
    using NuGetTransitiveDependencyFinder.ConsoleApp.Resources;

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
        public static int Main(string[] parameters) =>
            Parser.Default.ParseArguments<CommandLineOptions>(parameters)
                .MapResult(
                    options =>
                        {
                            using var loggerFactory = CreateLoggerFactory();
                            var logger = loggerFactory.CreateLogger(nameof(Program));
                            logger.LogInformation(Strings.Information.CommencingAnalysis);

                            var finder = new TransitiveDependencyFinder(loggerFactory, options.All);
                            var projects = finder.Run(options.Solution!);
                            var writer = new Writer(loggerFactory);
                            writer.Write(projects);

                            return 0;
                        },
                    _ =>
                        1);

        /// <summary>
        /// Creates the logger factory from which a logger will be created.
        /// </summary>
        /// <returns>The logger factory.</returns>
        private static ILoggerFactory CreateLoggerFactory() =>
            LoggerFactory.Create(builder =>
            {
                builder.AddConsole(options => options.FormatterName = nameof(Formatter))
                    .AddConsoleFormatter<Formatter, ConsoleFormatterOptions>()
                    .SetMinimumLevel(LogLevel.Trace);
            });
    }
}
