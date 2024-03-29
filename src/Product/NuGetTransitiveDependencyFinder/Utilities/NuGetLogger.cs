// <copyright file="NuGetLogger.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.Utilities;

using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NuGet.Common;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using NuGetLogLevel = NuGet.Common.LogLevel;

/// <summary>
/// A class that manages logging from within the NuGet infrastructure.
/// </summary>
/// <param name="logger">The underlying logging functionality.</param>
internal class NuGetLogger(ILogger<NuGetLogger> logger) : LoggerBase
{
    /// <summary>
    /// Logs a message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public override void Log(ILogMessage message)
    {
        var level = message.Level switch
        {
            NuGetLogLevel.Debug =>
                LogLevel.Debug,
            NuGetLogLevel.Verbose =>
                LogLevel.Debug,
            NuGetLogLevel.Information =>
                LogLevel.Information,
            NuGetLogLevel.Minimal =>
                LogLevel.Information,
            NuGetLogLevel.Warning =>
                LogLevel.Warning,
            NuGetLogLevel.Error =>
                LogLevel.Error,
            _ =>
                LogLevel.None,
        };

        logger.Log(
            level,
            "[{Time}] {WarningLevel} – {Message} ({ProjectPath})",
            message.Time.ToString(
                CultureInfo.InvariantCulture.DateTimeFormat.UniversalSortableDateTimePattern,
                CultureInfo.InvariantCulture),
            message.WarningLevel,
            message.FormatWithCode(),
            message.ProjectPath);
    }

    /// <summary>
    /// Asynchronously logs a message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public override Task LogAsync(ILogMessage message)
    {
        this.Log(message);
        return Task.CompletedTask;
    }
}
