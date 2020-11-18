// <copyright file="Formatter.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ConsoleApp.Output
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.Extensions.Logging.Console;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// A class for formatting logging messages for simple console display.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Performance",
        "CA1812",
        Justification = "This class is constructed via the LoggerFactory.")]
    internal class Formatter : ConsoleFormatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Formatter"/> class.
        /// </summary>
        /// <param name="_">The unused set of formatting options.</param>
#pragma warning disable SA1313 // ParameterNamesMustBeginWithLowerCaseLetter
        public Formatter(IOptionsMonitor<ConsoleFormatterOptions> _)
#pragma warning restore SA1313 // ParameterNamesMustBeginWithLowerCaseLetter
            : base(nameof(Formatter))
        {
        }

        /// <inheritdoc/>
        public override void Write<TState>(
            in LogEntry<TState> logEntry,
            IExternalScopeProvider scopeProvider,
            TextWriter textWriter)
        {
            const string ResetColorAndFormatting = "\x1B[39m\x1B[22m";

            textWriter.WriteLine(
                "{0}{1}{2}",
                GetColorAndFormatting(logEntry.LogLevel),
                logEntry.Formatter(logEntry.State, logEntry.Exception),
                ResetColorAndFormatting);
        }

        /// <summary>
        /// Gets the ASCII color and formatting codes for the corresponding log level.
        /// </summary>
        /// <param name="logLevel">The log level for which to get the color and formatting codes.</param>
        /// <returns>A string comprising the ASCII color and formatting codes.</returns>
        private static string GetColorAndFormatting(LogLevel logLevel) =>
            logLevel switch
            {
                LogLevel.Trace =>
                    "\x1B[32m", // Green
                LogLevel.Debug =>
                    string.Empty,
                LogLevel.Information =>
                    "\x1B[1m", // Bold
                LogLevel.Warning =>
                    "\x1B[1m\x1B[33m", // Bold Dark Yellow
                LogLevel.Error =>
                    "\x1B[1m\x1B[31m", // Bold Red
                LogLevel.Critical =>
                    "\x1B[1m\x1B[35m", // Bold Magenta
                _ =>
                    throw new ArgumentOutOfRangeException(nameof(logLevel))
            };
    }
}
