// <copyright file="PlainConsoleFormatterUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.ConsoleApp.Output
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using FluentAssertions;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.Extensions.Logging.Console;
    using Microsoft.Extensions.Options;
    using Moq;
    using NuGetTransitiveDependencyFinder.ConsoleApp.Output;
    using NuGetTransitiveDependencyFinder.TestUtilities.Globalization;
    using Xunit;
    using static System.FormattableString;

    /// <summary>
    /// Unit tests for the <see cref="PlainConsoleFormatter"/> class.
    /// </summary>
    public class PlainConsoleFormatterUnitTests
    {
        /// <summary>
        /// A mock value for the unused set of formatting options.
        /// </summary>
        private static readonly Mock<IOptionsMonitor<ConsoleFormatterOptions>> OptionsMonitorMock = new();

        /// <summary>
        /// A mock value for the unused external scope provider.
        /// </summary>
        private static readonly Mock<IExternalScopeProvider> ExternalScopeProviderMock = new();

        /// <summary>
        /// Tests that when
        /// <see cref="ConsoleFormatter.Write{TState}(in LogEntry{TState}, IExternalScopeProvider, TextWriter)"/> is
        /// called with a valid log level, the correct string is logged.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="expectedPrefix">The expected color and formatting prefix result.</param>
        [AllCulturesTheory]
        [InlineData(LogLevel.Trace, "\x1B[32m")]
        [InlineData(LogLevel.Debug, "")]
        [InlineData(LogLevel.Information, "\x1B[1m")]
        [InlineData(LogLevel.Warning, "\x1B[1m\x1B[33m")]
        [InlineData(LogLevel.Error, "\x1B[1m\x1B[31m")]
        [InlineData(LogLevel.Critical, "\x1B[1m\x1B[35m")]
        [InlineData(LogLevel.None, "\x1B[33m")]
        public void Write_WithValidLevel_LogsCorrectString(LogLevel logLevel, string expectedPrefix)
        {
            // Arrange
            var plainConsoleFormatter = new PlainConsoleFormatter(OptionsMonitorMock.Object);
            using var result = new StringWriter();

            // Act
            plainConsoleFormatter.Write(
                new LogEntry<string>(
                    logLevel,
                    "Category",
                    new(0),
                    "State",
                    new NotSupportedException(),
                    (string state, Exception? exception) => Invariant($"{state} {exception}")),
                ExternalScopeProviderMock.Object,
                result);

            // Assert
            _ = result.ToString()
                .Should().Be(
                    Invariant($"{expectedPrefix}State System.NotSupportedException: ") +
                    Invariant($"Specified method is not supported.\x1B[39m\x1B[22m{Environment.NewLine}"));
        }

        /// <summary>
        /// Tests that when
        /// <see cref="ConsoleFormatter.Write{TState}(in LogEntry{TState}, IExternalScopeProvider, TextWriter)"/> is
        /// called with each <see cref="LogLevel"/> present in the enumeration, the appropriate conversion is performed
        /// and <see cref="InvalidEnumArgumentException"/> is never thrown.
        /// </summary>
        [AllCulturesFact]
        public void Write_WithAllLevelsInEnumeration_DoesNotThrow()
        {
            // Arrange
            var plainConsoleFormatter = new PlainConsoleFormatter(OptionsMonitorMock.Object);
            var values = Enum.GetValues(typeof(LogLevel)).Cast<LogLevel>();
            using var result = new StringWriter();

            // Act
            var actions = values.Select(value =>
                new Action(() => plainConsoleFormatter.Write(
                    new LogEntry<string>(
                        value,
                        "Category",
                        new(0),
                        "State",
                        new NotSupportedException(),
                        (string state, Exception? exception) => Invariant($"{state} {exception}")),
                    ExternalScopeProviderMock.Object,
                    result)));

            // Assert
            foreach (var action in actions)
            {
                _ = action
                    .Should().NotThrow<InvalidEnumArgumentException>();
            }
        }

        /// <summary>
        /// Tests that when
        /// <see cref="ConsoleFormatter.Write{TState}(in LogEntry{TState}, IExternalScopeProvider, TextWriter)"/> is
        /// called with an invalid log level, a <see cref="InvalidEnumArgumentException"/> is thrown.
        /// </summary>
        [AllCulturesFact]
        public void Write_WithInvalidLevel_ThrowsInvalidEnumArgumentException()
        {
            // Arrange
            var plainConsoleFormatter = new PlainConsoleFormatter(OptionsMonitorMock.Object);
            using var result = new StringWriter();

            // Act
            Action action = () => plainConsoleFormatter.Write(
                new LogEntry<string>(
                    (LogLevel)100,
                    "Category",
                    new(0),
                    "State",
                    new NotSupportedException(),
                    (string state, Exception? exception) => Invariant($"{state} {exception}")),
                ExternalScopeProviderMock.Object,
                result);

            // Assert
            _ = action
                .Should().Throw<InvalidEnumArgumentException>().WithMessage(
                    "The value of argument 'logLevel' (100) is invalid for Enum type 'LogLevel'. " +
                    "(Parameter 'logLevel')")
                .And.ParamName.Should().Be("logLevel");
        }
    }
}
