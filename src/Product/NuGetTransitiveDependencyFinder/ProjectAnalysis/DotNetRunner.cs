// <copyright file="DotNetRunner.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ProjectAnalysis
{
    using System.Diagnostics;
    using System.Threading.Tasks;
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
        public Task RunAsync(string parameters, string workingDirectory)
        {
            this.processWrapper.ErrorDataReceived += this.LogError!;
            this.processWrapper.OutputDataReceived += this.LogOutput!;
            this.processWrapper.StartInfo = new("dotnet", parameters)
            {
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WorkingDirectory = workingDirectory,
            };

            // Start() will return a Boolean value indicating whether a new process was started. A false return value
            // indicates that an existing process was reused and is not indicative of failure.
            _ = this.processWrapper.Start();

            this.processWrapper.BeginErrorReadLine();
            this.processWrapper.BeginOutputReadLine();

            return this.processWrapper.WaitForExitAsync();
        }

        /// <summary>
        /// Logs messages sent to the Standard Error stream.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event parameters.</param>
        private void LogError(object sender, DataReceivedEventArgs e) =>
            this.logger.LogError(e.Data ?? string.Empty);

        /// <summary>
        /// Logs messages sent to the Standard Output stream.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event parameters.</param>
        private void LogOutput(object sender, DataReceivedEventArgs e) =>
            this.logger.LogTrace(e.Data ?? string.Empty);
    }
}
