// <copyright file="Formatter.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGet.TransitiveDependency.Finder.ConsoleApp.Output
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
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
        /// The ASCII code for resetting the console color and formatting to their defaults.
        /// </summary>
        private const string GetColorAndFormattingReset = "\x1B[39m\x1B[22m";

        /// <summary>
        /// The set of formatting options.
        /// </summary>
        private readonly IOptionsMonitor<ConsoleFormatterOptions> options;

        /// <summary>
        /// Initializes a new instance of the <see cref="Formatter"/> class.
        /// </summary>
        /// <param name="options">The set of formatting options.</param>
        public Formatter(IOptionsMonitor<ConsoleFormatterOptions> options)
            : base(nameof(Formatter)) =>
            this.options = options;

        /// <inheritdoc/>
        public override void Write<TState>(
            in LogEntry<TState> logEntry,
            IExternalScopeProvider scopeProvider,
            TextWriter textWriter)
        {
            var currentOptions = this.options.CurrentValue;
            var timestamp = currentOptions.UseUtcTimestamp ? DateTime.UtcNow : DateTime.Now;
            textWriter.WriteLine(
                "{0} {1}{2}{3}",
                timestamp.ToString(currentOptions.TimestampFormat ?? "s", CultureInfo.CurrentCulture),
                GetColorAndFormatting(logEntry.LogLevel),
                logEntry.Formatter(logEntry.State, logEntry.Exception),
                GetColorAndFormattingReset);
        }

        /// <summary>
        /// Gets the ASCII color and formatting codes for the corresponding log level.
        /// </summary>
        /// <param name="logLevel">The log level for which to get the formatting.</param>
        /// <returns>A string comprising the ASCII color and formatting codes.</returns>
        private static string GetColorAndFormatting(LogLevel logLevel) =>
            logLevel switch
            {
                LogLevel.Trace => "\x1B[32m", // Green
                LogLevel.Debug => "\x1B[32m", // Green
                LogLevel.Information => "\x1B[1m", // Bold
                LogLevel.Warning => "\x1B[1m\x1B[33m", // Bold Dark Yellow
                LogLevel.Error => "\x1B[1m\x1B[31m", // Bold Red
                LogLevel.Critical => "\x1B[1m\x1B[31m", // Bold Red
                LogLevel.None => string.Empty,
                _ => throw new ArgumentOutOfRangeException(nameof(logLevel))
            };
    }
}
