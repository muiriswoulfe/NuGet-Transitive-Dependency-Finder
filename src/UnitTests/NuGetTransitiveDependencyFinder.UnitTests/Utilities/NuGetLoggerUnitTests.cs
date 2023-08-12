// <copyright file="NuGetLoggerUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Utilities;

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Common;
using NuGetTransitiveDependencyFinder.UnitTests.Utilities.Globalization;
using NuGetTransitiveDependencyFinder.UnitTests.Utilities.Logging;
using NuGetTransitiveDependencyFinder.Utilities;
using Xunit;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using NuGetLogLevel = NuGet.Common.LogLevel;

/// <summary>
/// Unit tests for the <see cref="NuGetLogger"/> class.
/// </summary>
public class NuGetLoggerUnitTests
{
    /// <summary>
    /// The mock <see cref="ILogger"/> object.
    /// </summary>
    private readonly MockLogger<NuGetLogger> loggerMock;

    /// <summary>
    /// The <see cref="NuGetLogger"/> under test.
    /// </summary>
    private readonly NuGetLogger nuGetLogger;

    /// <summary>
    /// The message for use within the tests.
    /// </summary>
    private const string TestMessage = "Message";

    /// <summary>
    /// The full regular expression to which message results created from <see cref="ILogMessage"/> objects are expected
    /// to adhere.
    /// </summary>
    private const string ExpectedFullRegularExpression =
        @"\[\d{4}\-\d{2}\-\d{2} \d{2}:\d{2}:\d{2}Z\] Severe – NU1000: " + TestMessage + @" \(\(null\)\)";

    /// <summary>
    /// The regular expression to which message results are expected to adhere.
    /// </summary>
    private const string ExpectedRegularExpression =
        @"\[\d{4}\-\d{2}\-\d{2} \d{2}:\d{2}:\d{2}Z\] Severe – " + TestMessage + @" \(\(null\)\)";

    /// <summary>
    /// The test data for validating <see cref="LogLevel"/> test scenarios.
    /// </summary>
    public static readonly TheoryData<NuGetLogLevel, LogLevel> LogLevelTestData =
        new()
        {
            { NuGetLogLevel.Debug, LogLevel.Debug },
            { NuGetLogLevel.Verbose, LogLevel.Debug },
            { NuGetLogLevel.Information, LogLevel.Information },
            { NuGetLogLevel.Minimal, LogLevel.Information },
            { NuGetLogLevel.Warning, LogLevel.Warning },
            { NuGetLogLevel.Error, LogLevel.Error },
            { (NuGetLogLevel)100, LogLevel.None },
        };

    /// <summary>
    /// Initializes a new instance of the <see cref="NuGetLoggerUnitTests"/> class.
    /// </summary>
    public NuGetLoggerUnitTests()
    {
        this.loggerMock = new MockLogger<NuGetLogger>();
        this.nuGetLogger = new NuGetLogger(this.loggerMock);
    }

    /// <summary>
    /// Tests that when <see cref="NuGetLogger.Log(ILogMessage)"/> is called with each <see cref="NuGetLogLevel"/>, the
    /// appropriate conversion is performed.
    /// </summary>
    /// <param name="inputLogLevel">The <see cref="NuGetLogLevel"/> to input.</param>
    /// <param name="expectedLogLevel">The expected <see cref="LogLevel"/> resulting from converting
    /// <paramref name="inputLogLevel"/>.</param>
    [AllCulturesTheory]
    [MemberData(nameof(LogLevelTestData))]
    public void LogWithLogMessage_WithEachLevel_ConvertsLevel(
        NuGetLogLevel inputLogLevel,
        LogLevel expectedLogLevel)
    {
        // Act
        this.nuGetLogger.Log(CreateLogMessage(inputLogLevel));

        // Assert
        _ = this.loggerMock.Entries
            .Should().ContainSingle();
        _ = this.loggerMock.Entries[0].LogLevel
            .Should().Be(expectedLogLevel);
        _ = this.loggerMock.Entries[0].Message
            .Should().MatchRegex(ExpectedFullRegularExpression);
    }

    /// <summary>
    /// Tests that when <see cref="NuGetLogger.Log(ILogMessage)"/> is called with each <see cref="NuGetLogLevel"/>
    /// present in the enumeration, the appropriate conversion is performed and <see cref="LogLevel.None"/> is never
    /// used.
    /// </summary>
    [AllCulturesFact]
    public void LogWithLogMessage_WithAllLevelsInEnumeration_ConvertsLevel()
    {
        // Arrange
        var values = Enum.GetValues(typeof(NuGetLogLevel)).Cast<NuGetLogLevel>().ToList();

        // Act
        foreach (var value in values)
        {
            this.nuGetLogger.Log(CreateLogMessage(value));
        }

        // Assert
        _ = this.loggerMock.Entries
            .Should().HaveCount(values.Count);
        foreach (var entry in this.loggerMock.Entries)
        {
            _ = entry.LogLevel
                .Should().NotBe(LogLevel.None);
            _ = entry.Message
                .Should().MatchRegex(ExpectedFullRegularExpression);
        }
    }

    /// <summary>
    /// Tests that when <see cref="LoggerBase.Log(NuGetLogLevel, string)"/> is called with each
    /// <see cref="NuGetLogLevel"/>, the appropriate conversion is performed.
    /// </summary>
    /// <param name="inputLogLevel">The <see cref="NuGetLogLevel"/> to input.</param>
    /// <param name="expectedLogLevel">The expected <see cref="LogLevel"/> resulting from converting
    /// <paramref name="inputLogLevel"/>.</param>
    [AllCulturesTheory]
    [MemberData(nameof(LogLevelTestData))]
    public void LogWithLevelAndString_WithEachLevel_ConvertsLevel(
        NuGetLogLevel inputLogLevel,
        LogLevel expectedLogLevel)
    {
        // Act
        this.nuGetLogger.Log(inputLogLevel, TestMessage);

        // Assert
        _ = this.loggerMock.Entries
            .Should().ContainSingle();
        _ = this.loggerMock.Entries[0].LogLevel
            .Should().Be(expectedLogLevel);
        _ = this.loggerMock.Entries[0].Message
            .Should().MatchRegex(ExpectedRegularExpression);
    }

    /// <summary>
    /// Tests that when <see cref="LoggerBase.Log(NuGetLogLevel, string)"/> is called with each
    /// <see cref="NuGetLogLevel"/> present in the enumeration, the appropriate conversion is performed and
    /// <see cref="LogLevel.None"/> is never used.
    /// </summary>
    [AllCulturesFact]
    public void LogWithLevelAndString_WithAllLevelsInEnumeration_ConvertsLevel()
    {
        // Arrange
        var values = Enum.GetValues(typeof(NuGetLogLevel)).Cast<NuGetLogLevel>().ToList();

        // Act
        foreach (var value in values)
        {
            this.nuGetLogger.Log(value, TestMessage);
        }

        // Assert
        _ = this.loggerMock.Entries
            .Should().HaveCount(values.Count);
        foreach (var entry in this.loggerMock.Entries)
        {
            _ = entry.LogLevel
                .Should().NotBe(LogLevel.None);
            _ = entry.Message
                .Should().MatchRegex(ExpectedRegularExpression);
        }
    }

    /// <summary>
    /// Tests that when <see cref="NuGetLogger.LogAsync(ILogMessage)"/> is called with each <see cref="NuGetLogLevel"/>,
    /// the appropriate conversion is performed.
    /// </summary>
    /// <param name="inputLogLevel">The <see cref="NuGetLogLevel"/> to input.</param>
    /// <param name="expectedLogLevel">The expected <see cref="LogLevel"/> resulting from converting
    /// <paramref name="inputLogLevel"/>.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [AllCulturesTheory]
    [MemberData(nameof(LogLevelTestData))]
    public async Task LogAsyncWithLogMessage_WithEachLevel_ConvertsLevel(
        NuGetLogLevel inputLogLevel,
        LogLevel expectedLogLevel)
    {
        // Act
        await this.nuGetLogger.LogAsync(CreateLogMessage(inputLogLevel));

        // Assert
        _ = this.loggerMock.Entries
            .Should().ContainSingle();
        _ = this.loggerMock.Entries[0].LogLevel
            .Should().Be(expectedLogLevel);
        _ = this.loggerMock.Entries[0].Message
            .Should().MatchRegex(ExpectedFullRegularExpression);
    }

    /// <summary>
    /// Tests that when <see cref="NuGetLogger.LogAsync(ILogMessage)"/> is called with each <see cref="NuGetLogLevel"/>
    /// present in the enumeration, the appropriate conversion is performed and <see cref="LogLevel.None"/> is never
    /// used.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [AllCulturesFact]
    public async Task LogAsyncWithLogMessage_WithAllLevelsInEnumeration_ConvertsLevel()
    {
        // Arrange
        var values = Enum.GetValues(typeof(NuGetLogLevel)).Cast<NuGetLogLevel>().ToList();

        // Act
        foreach (var value in values)
        {
            await this.nuGetLogger.LogAsync(CreateLogMessage(value));
        }

        // Assert
        _ = this.loggerMock.Entries
            .Should().HaveCount(values.Count);
        foreach (var entry in this.loggerMock.Entries)
        {
            _ = entry.LogLevel
                .Should().NotBe(LogLevel.None);
            _ = entry.Message
                .Should().MatchRegex(ExpectedFullRegularExpression);
        }
    }

    /// <summary>
    /// Tests that when <see cref="LoggerBase.LogAsync(NuGetLogLevel, string)"/> is called with each
    /// <see cref="NuGetLogLevel"/>, the appropriate conversion is performed.
    /// </summary>
    /// <param name="inputLogLevel">The <see cref="NuGetLogLevel"/> to input.</param>
    /// <param name="expectedLogLevel">The expected <see cref="LogLevel"/> resulting from converting
    /// <paramref name="inputLogLevel"/>.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [AllCulturesTheory]
    [MemberData(nameof(LogLevelTestData))]
    public async Task LogAsyncWithLevelAndString_WithEachLevel_ConvertsLevel(
        NuGetLogLevel inputLogLevel,
        LogLevel expectedLogLevel)
    {
        // Act
        await this.nuGetLogger.LogAsync(inputLogLevel, TestMessage);

        // Assert
        _ = this.loggerMock.Entries
            .Should().ContainSingle();
        _ = this.loggerMock.Entries[0].LogLevel
            .Should().Be(expectedLogLevel);
        _ = this.loggerMock.Entries[0].Message
            .Should().MatchRegex(ExpectedRegularExpression);
    }

    /// <summary>
    /// Tests that when <see cref="LoggerBase.LogAsync(NuGetLogLevel, string)"/> is called with each
    /// <see cref="NuGetLogLevel"/> present in the enumeration, the appropriate conversion is performed and
    /// <see cref="LogLevel.None"/> is never used.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [AllCulturesFact]
    public async Task LogAsyncWithLevelAndString_WithAllLevelsInEnumeration_ConvertsLevel()
    {
        // Arrange
        var values = Enum.GetValues(typeof(NuGetLogLevel)).Cast<NuGetLogLevel>().ToList();

        // Act
        foreach (var value in values)
        {
            await this.nuGetLogger.LogAsync(value, TestMessage);
        }

        // Assert
        _ = this.loggerMock.Entries
            .Should().HaveCount(values.Count);
        foreach (var entry in this.loggerMock.Entries)
        {
            _ = entry.LogLevel
                .Should().NotBe(LogLevel.None);
            _ = entry.Message
                .Should().MatchRegex(ExpectedRegularExpression);
        }
    }

    /// <summary>
    /// Tests that when <see cref="LoggerBase.LogDebug(string)"/> is called, the logging is performed correctly.
    /// </summary>
    [AllCulturesFact]
    public void LogDebug_WithEachLevel_LogsCorrectly()
    {
        // Act
        this.nuGetLogger.LogDebug(TestMessage);

        // Assert
        _ = this.loggerMock.Entries
            .Should().ContainSingle();
        _ = this.loggerMock.Entries[0].LogLevel
            .Should().Be(LogLevel.Debug);
        _ = this.loggerMock.Entries[0].Message
            .Should().MatchRegex(ExpectedRegularExpression);
    }

    /// <summary>
    /// Tests that when <see cref="LoggerBase.LogVerbose(string)"/> is called, the logging is performed correctly.
    /// </summary>
    [AllCulturesFact]
    public void LogVerbose_WithEachLevel_LogsCorrectly()
    {
        // Act
        this.nuGetLogger.LogVerbose(TestMessage);

        // Assert
        _ = this.loggerMock.Entries
            .Should().ContainSingle();
        _ = this.loggerMock.Entries[0].LogLevel
            .Should().Be(LogLevel.Debug);
        _ = this.loggerMock.Entries[0].Message
            .Should().MatchRegex(ExpectedRegularExpression);
    }

    /// <summary>
    /// Tests that when <see cref="LoggerBase.LogInformation(string)"/> is called, the logging is performed correctly.
    /// </summary>
    [AllCulturesFact]
    public void LogInformation_WithEachLevel_LogsCorrectly()
    {
        // Act
        this.nuGetLogger.LogInformation(TestMessage);

        // Assert
        _ = this.loggerMock.Entries
            .Should().ContainSingle();
        _ = this.loggerMock.Entries[0].LogLevel
            .Should().Be(LogLevel.Information);
        _ = this.loggerMock.Entries[0].Message
            .Should().MatchRegex(ExpectedRegularExpression);
    }

    /// <summary>
    /// Tests that when <see cref="LoggerBase.LogInformationSummary(string)"/> is called, the logging is performed
    /// correctly.
    /// </summary>
    [AllCulturesFact]
    public void LogInformationSummary_WithEachLevel_LogsCorrectly()
    {
        // Act
        this.nuGetLogger.LogInformationSummary(TestMessage);

        // Assert
        _ = this.loggerMock.Entries
            .Should().ContainSingle();
        _ = this.loggerMock.Entries[0].LogLevel
            .Should().Be(LogLevel.Information);
        _ = this.loggerMock.Entries[0].Message
            .Should().MatchRegex(ExpectedRegularExpression);
    }

    /// <summary>
    /// Tests that when <see cref="LoggerBase.LogMinimal(string)"/> is called, the logging is performed correctly.
    /// </summary>
    [AllCulturesFact]
    public void LogMinimal_WithEachLevel_LogsCorrectly()
    {
        // Act
        this.nuGetLogger.LogMinimal(TestMessage);

        // Assert
        _ = this.loggerMock.Entries
            .Should().ContainSingle();
        _ = this.loggerMock.Entries[0].LogLevel
            .Should().Be(LogLevel.Information);
        _ = this.loggerMock.Entries[0].Message
            .Should().MatchRegex(ExpectedRegularExpression);
    }

    /// <summary>
    /// Tests that when <see cref="LoggerBase.LogWarning(string)"/> is called, the logging is performed correctly.
    /// </summary>
    [AllCulturesFact]
    public void LogWarning_WithEachLevel_LogsCorrectly()
    {
        // Act
        this.nuGetLogger.LogWarning(TestMessage);

        // Assert
        _ = this.loggerMock.Entries
            .Should().ContainSingle();
        _ = this.loggerMock.Entries[0].LogLevel
            .Should().Be(LogLevel.Warning);
        _ = this.loggerMock.Entries[0].Message
            .Should().MatchRegex(ExpectedRegularExpression);
    }

    /// <summary>
    /// Tests that when <see cref="LoggerBase.LogError(string)"/> is called, the logging is performed correctly.
    /// </summary>
    [AllCulturesFact]
    public void LogError_WithEachLevel_LogsCorrectly()
    {
        // Act
        this.nuGetLogger.LogError(TestMessage);

        // Assert
        _ = this.loggerMock.Entries
            .Should().ContainSingle();
        _ = this.loggerMock.Entries[0].LogLevel
            .Should().Be(LogLevel.Error);
        _ = this.loggerMock.Entries[0].Message
            .Should().MatchRegex(ExpectedRegularExpression);
    }

    /// <summary>
    /// Creates an <see cref="ILogMessage"/> for use within the tests.
    /// </summary>
    /// <param name="logLevel">The <see cref="NuGetLogLevel"/> of the message.</param>
    /// <returns>The created <see cref="ILogMessage"/>.</returns>
    private static ILogMessage CreateLogMessage(NuGetLogLevel logLevel) =>
        new LogMessage(logLevel, TestMessage, NuGetLogCode.NU1000);
}
