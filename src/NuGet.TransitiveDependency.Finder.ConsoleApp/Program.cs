// <copyright file="Program.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGet.TransitiveDependency.Finder.ConsoleApp
{
    using System;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Console;
    using NuGet.TransitiveDependency.Finder.ConsoleApp.Output;
    using NuGet.TransitiveDependency.Finder.ConsoleApp.Resources;
    using NuGet.TransitiveDependency.Finder.Library;

    /// <summary>
    /// The main class of the application, defining the entry point and all interaction.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The entry point of the application.
        /// </summary>
        /// <param name="parameters">A collection of command-line parameters.</param>
        public static void Main(string[] parameters)
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole(options => options.FormatterName = nameof(Formatter))
                    .AddConsoleFormatter<Formatter, ConsoleFormatterOptions>()
                    .SetMinimumLevel(LogLevel.Trace);
            });

            var logger = loggerFactory.CreateLogger(nameof(Program));
            if (parameters.Length != 1)
            {
                logger.LogError(Strings.Error.MissingParameter);
                return;
            }

            logger.LogInformation(Strings.Information.CommencingAnalysis);
            var finder = new TransitiveDependencyFinder(loggerFactory);
            var projects = finder.Run(parameters[0]);
            new Writer(loggerFactory).Write(projects);
        }
    }
}
