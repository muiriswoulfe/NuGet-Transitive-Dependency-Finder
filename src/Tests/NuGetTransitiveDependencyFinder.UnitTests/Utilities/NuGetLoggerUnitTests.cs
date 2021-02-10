// <copyright file="NuGetLoggerUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Utilities
{
    using FluentAssertions;
    using NuGet.Common;
    using NuGetTransitiveDependencyFinder.Utilities;
    using NuGetTransitiveDependencyFinder.UnitTests.TestUtilities;
    using Xunit;
    using ILogger = Microsoft.Extensions.Logging.ILogger;
    using LogLevel = Microsoft.Extensions.Logging.LogLevel;
    using NuGetLogLevel = NuGet.Common.LogLevel;
    using System;
    using System.Linq;

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
        /// Initializes a new instance of the <see cref="NuGetLoggerUnitTests"/> class.
        /// </summary>
        public NuGetLoggerUnitTests()
        {
            this.loggerMock = new MockLogger<NuGetLogger>();
            this.nuGetLogger = new NuGetLogger(this.loggerMock);
        }

        /// <summary>
        /// Tests that when <see cref="NuGetLogger.Log(ILogMessage)"/> is called with each <see cref="NuGetLogLevel"/>,
        /// the appropriate conversion is performed.
        /// </summary>
        /// <param name="inputLogLevel">The <see cref="NuGetLogLevel"/> to input.</param>
        /// <param name="expectedLogLevel">The expected <see cref="LogLevel"/> resulting from converting
        /// <paramref name="inputLogLevel"/>.</param>
        [Theory]
        [InlineData(NuGetLogLevel.Debug, LogLevel.Debug)]
        [InlineData(NuGetLogLevel.Verbose, LogLevel.Debug)]
        [InlineData(NuGetLogLevel.Information, LogLevel.Information)]
        [InlineData(NuGetLogLevel.Minimal, LogLevel.Information)]
        [InlineData(NuGetLogLevel.Warning, LogLevel.Warning)]
        [InlineData(NuGetLogLevel.Error, LogLevel.Error)]
        [InlineData((NuGetLogLevel)100, LogLevel.None)]
        public void Log_WithEachLevel_ConvertsLevel(NuGetLogLevel inputLogLevel, LogLevel expectedLogLevel)
        {
            this.nuGetLogger.Log(new LogMessage(inputLogLevel, "Message", NuGetLogCode.NU1000));

            _ = this.loggerMock.Entries.Should().ContainSingle();
            _ = this.loggerMock.Entries[0].LogLevel.Should().Be(expectedLogLevel);
            _ = this.loggerMock.Entries[0].Message.Should()
                .MatchRegex(@"\[\d{4}\-\d{2}\-\d{2} \d{2}:\d{2}:\d{2}Z\] Severe – NU1000: Message \(\(null\)\)");
        }

        /// <summary>
        /// Tests that when <see cref="NuGetLogger.Log(ILogMessage)"/> is called with each <see cref="NuGetLogLevel"/>
        /// present in the enumeration, the appropriate conversion is performed and <see cref="LogLevel.None"/> is never
        /// used.
        /// </summary>
        [Fact]
        public void Log_WithAllLevelsInEnumeration_ConvertsLevel()
        {
            var values = Enum.GetValues(typeof(NuGetLogLevel)).Cast<NuGetLogLevel>().ToList();

            foreach (var value in values)
            {
                this.nuGetLogger.Log(new LogMessage(value, "Message", NuGetLogCode.NU1000));
            }

            _ = this.loggerMock.Entries.Should().HaveCount(values.Count);
            foreach (var entry in this.loggerMock.Entries)
            {
                _ = entry.LogLevel.Should().NotBe(LogLevel.None);
                _ = entry.Message.Should()
                    .MatchRegex(@"\[\d{4}\-\d{2}\-\d{2} \d{2}:\d{2}:\d{2}Z\] Severe – NU1000: Message \(\(null\)\)");
            }
        }
    }
}
