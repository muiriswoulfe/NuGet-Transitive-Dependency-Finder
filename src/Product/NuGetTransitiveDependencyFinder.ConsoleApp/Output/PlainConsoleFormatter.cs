// <copyright file="PlainConsoleFormatter.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ConsoleApp.Output
{
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.Extensions.Logging.Console;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// A class for formatting logging messages for plain console display.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Performance",
        "CA1812",
        Justification = "This class is constructed via the LoggerFactory.")]
    internal class PlainConsoleFormatter : ConsoleFormatter
    {
        /// <summary>
        /// The console formatting code indicating that the foreground should be bold.
        /// </summary>
        private const string Bold = "\x1B[1m";

        /// <summary>
        /// The console formatting code indicating that the foreground should be dark yellow.
        /// </summary>
        private const string DarkYellow = "\x1B[33m";

        /// <summary>
        /// The console formatting code indicating that the foreground should be green.
        /// </summary>
        private const string Green = "\x1B[32m";

        /// <summary>
        /// The console formatting code indicating that the foreground should be magenta.
        /// </summary>
        private const string Magenta = "\x1B[35m";

        /// <summary>
        /// The console formatting code indicating that the foreground should be red.
        /// </summary>
        private const string Red = "\x1B[31m";

        /// <summary>
        /// The console formatting code indicating that the foreground should be yellow.
        /// </summary>
        private const string Yellow = "\x1B[33m";

        /// <summary>
        /// Initializes a new instance of the <see cref="PlainConsoleFormatter"/> class.
        /// </summary>
        /// <param name="_">The unused set of formatting options.</param>
        public PlainConsoleFormatter(IOptionsMonitor<ConsoleFormatterOptions> _)
            : base(nameof(PlainConsoleFormatter))
        {
        }

        /// <inheritdoc/>
        public override void Write<TState>(
            in LogEntry<TState> logEntry,
            IExternalScopeProvider scopeProvider,
            TextWriter textWriter)
        {
            const string resetColorAndFormatting = "\x1B[39m\x1B[22m";

            textWriter.WriteLine(
                "{0}{1}{2}",
                GetColorAndFormatting(logEntry.LogLevel),
                logEntry.Formatter(logEntry.State, logEntry.Exception),
                resetColorAndFormatting);
        }

        /// <summary>
        /// Gets the ASCII color and formatting codes for the corresponding log level.
        /// </summary>
        /// <param name="logLevel">The log level for which to get the color and formatting codes.</param>
        /// <returns>A string comprising the ASCII color and formatting codes.</returns>
        /// <exception cref="InvalidEnumArgumentException"><paramref name="logLevel"/> is set to an unacceptable
        /// value.</exception>
        private static string GetColorAndFormatting(LogLevel logLevel) =>
            logLevel switch
            {
                LogLevel.Trace =>
                    Green,
                LogLevel.Debug =>
                    string.Empty,
                LogLevel.Information =>
                    Bold,
                LogLevel.Warning =>
                    Bold + DarkYellow,
                LogLevel.Error =>
                    Bold + Red,
                LogLevel.Critical =>
                    Bold + Magenta,
                LogLevel.None =>
                    Yellow,
                _ =>
                    throw new InvalidEnumArgumentException(nameof(logLevel), (int)logLevel, typeof(LogLevel)),
            };
    }
}
