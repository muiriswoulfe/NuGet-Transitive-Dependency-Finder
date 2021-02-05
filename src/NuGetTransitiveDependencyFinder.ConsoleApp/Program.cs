// <copyright file="Program.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ConsoleApp
{
    using CommandLine;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Console;
    using NuGetTransitiveDependencyFinder.ConsoleApp.Input;
    using NuGetTransitiveDependencyFinder.ConsoleApp.Output;
    using NuGetTransitiveDependencyFinder.Extensions;

    /// <summary>
    /// The main class of the application, defining the entry point and the basic operation of the application.
    /// </summary>
    internal static class Program
    {
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
                        var serviceProvider = CreateServiceProvider(commandLineOptions);
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
        /// <returns>The service provider.</returns>
        private static ServiceProvider CreateServiceProvider(CommandLineOptions commandLineOptions) =>
            new ServiceCollection()
                .AddLogging(logging =>
                    logging.AddConsole(options => options.FormatterName = nameof(PlainConsoleFormatter))
                        .AddConsoleFormatter<PlainConsoleFormatter, ConsoleFormatterOptions>()
                        .SetMinimumLevel(LogLevel.Trace))
                .AddSingleton(_ => commandLineOptions)
                .AddSingleton<DependencyWriter>()
                .AddSingleton<ProgramRunner>()
                .AddNuGetTransitiveDependencyFinder()
                .BuildServiceProvider();
    }
}
