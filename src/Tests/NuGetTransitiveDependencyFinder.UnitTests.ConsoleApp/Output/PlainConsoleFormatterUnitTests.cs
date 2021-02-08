// <copyright file="PlainConsoleFormatterUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.ConsoleApp.Output
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using FluentAssertions;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.Extensions.Logging.Console;
    using Microsoft.Extensions.Options;
    using Moq;
    using NuGetTransitiveDependencyFinder.ConsoleApp.Output;
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
        [Theory]
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
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
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
                    (string state, Exception exception) => Invariant($"{state} {exception}")),
                ExternalScopeProviderMock.Object,
                result);

            // Assert
            var expected =
                Invariant($"{expectedPrefix}State System.NotSupportedException: Specified method is not supported.") +
                Invariant($"\x1B[39m\x1B[22m{Environment.NewLine}");
            _ = result.ToString().Should().Be(expected);
        }

        /// <summary>
        /// Tests that when
        /// <see cref="ConsoleFormatter.Write{TState}(in LogEntry{TState}, IExternalScopeProvider, TextWriter)"/> is
        /// called with an invalid log level, a <see cref="InvalidEnumArgumentException"/> is thrown.
        /// </summary>
        [Fact]
        public void Write_WithInvalidLevel_ThrowsInvalidEnumArgumentException()
        {
            // Arrange
            var plainConsoleFormatter = new PlainConsoleFormatter(OptionsMonitorMock.Object);
            using var result = new StringWriter();

            // Act
            Action action = () => plainConsoleFormatter.Write(
                new LogEntry<string>(
                    (LogLevel)7,
                    "Category",
                    new(0),
                    "State",
                    new NotSupportedException(),
                    (string state, Exception exception) => Invariant($"{state} {exception}")),
                ExternalScopeProviderMock.Object,
                result);

            // Assert
            _ = action.Should().Throw<InvalidEnumArgumentException>()
                .WithMessage(
                    "The value of argument 'logLevel' (7) is invalid for Enum type 'LogLevel'. (Parameter 'logLevel')")
                .And.ParamName.Should().Be("logLevel");
        }
    }
}
