// <copyright file="DotNetRunner.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ProjectAnalysis
{
    using System.Diagnostics;
    using Microsoft.Extensions.Logging;
    using NuGetTransitiveDependencyFinder.Wrappers;

    /// <summary>
    /// A class that manages the running of .NET commands on project and solution files.
    /// </summary>
    internal class DotNetRunner : IDotNetRunner
    {
        /// <summary>
        /// The logger for asynchronous messages that have been created by external processes.
        /// </summary>
        private readonly ILogger<DotNetRunner> logger;

        /// <summary>
        /// The wrapper around <see cref="Process"/>.
        /// </summary>
        private readonly IProcessWrapper processWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetRunner"/> class.
        /// </summary>
        /// <param name="logger">The logger for asynchronous messages that have been created by external
        /// processes.</param>
        /// <param name="processWrapper">The wrapper around <see cref="Process"/>.</param>
        public DotNetRunner(ILogger<DotNetRunner> logger, IProcessWrapper processWrapper)
        {
            this.logger = logger;
            this.processWrapper = processWrapper;
        }

        /// <inheritdoc/>
        public void Run(string parameters, string workingDirectory)
        {
            var startInfo = new ProcessStartInfo("dotnet", parameters)
            {
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WorkingDirectory = workingDirectory,
            };

            this.processWrapper.Start(startInfo, this.LogOutput!, this.LogError!);

            this.processWrapper.BeginErrorReadLine();
            this.processWrapper.BeginOutputReadLine();

            this.processWrapper.WaitForExit();
        }

        /// <summary>
        /// Logs messages sent to the Standard Error stream.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="eventParameters">The event parameters.</param>
        private void LogError(object sender, DataReceivedEventArgs eventParameters) =>
            this.logger.LogError(eventParameters.Data ?? string.Empty);

        /// <summary>
        /// Logs messages sent to the Standard Output stream.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="eventParameters">The event parameters.</param>
        private void LogOutput(object sender, DataReceivedEventArgs eventParameters) =>
            this.logger.LogTrace(eventParameters.Data ?? string.Empty);
    }
}
