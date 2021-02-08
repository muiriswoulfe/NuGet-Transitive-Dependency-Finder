// <copyright file="NuGetLogger.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.Utilities
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using NuGet.Common;
    using INuGetLogger = NuGet.Common.ILogger;
    using LogLevel = Microsoft.Extensions.Logging.LogLevel;
    using NuGetLogLevel = NuGet.Common.LogLevel;

    /// <summary>
    /// A class that manages logging from within the NuGet infrastructure.
    /// </summary>
    internal class NuGetLogger : INuGetLogger
    {
        /// <summary>
        /// The underlying logging functionality.
        /// </summary>
        private readonly ILogger<NuGetLogger> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetLogger"/> class.
        /// </summary>
        /// <param name="logger">The underlying logging functionality.</param>
        public NuGetLogger(ILogger<NuGetLogger> logger) =>
            this.logger = logger;

        /// <inheritdoc/>
        public void LogDebug(string data) =>
            this.logger.LogDebug(data);

        /// <inheritdoc/>
        public void LogVerbose(string data) =>
            this.logger.LogTrace(data);

        /// <inheritdoc/>
        public void LogInformation(string data) =>
            this.logger.LogInformation(data);

        /// <inheritdoc/>
        public void LogMinimal(string data) =>
            this.logger.LogInformation(data);

        /// <inheritdoc/>
        public void LogWarning(string data) =>
            this.logger.LogWarning(data);

        /// <inheritdoc/>
        public void LogError(string data) =>
            this.logger.LogError(data);

        /// <inheritdoc/>
        public void LogInformationSummary(string data) =>
            this.logger.LogInformation(data);

        /// <inheritdoc/>
        public void Log(NuGetLogLevel level, string data)
        {
            var convertedLevel = level switch
            {
                NuGetLogLevel.Debug =>
                    LogLevel.Debug,
                NuGetLogLevel.Verbose =>
                    LogLevel.Trace,
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

            this.logger.Log(convertedLevel, data);
        }

        /// <inheritdoc/>
        public void Log(ILogMessage message) =>
            this.logger.LogInformation(message.Message);

        /// <inheritdoc/>
        public Task LogAsync(NuGetLogLevel level, string data)
        {
            this.Log(level, data);
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task LogAsync(ILogMessage message)
        {
            this.Log(message);
            return Task.CompletedTask;
        }
    }
}
