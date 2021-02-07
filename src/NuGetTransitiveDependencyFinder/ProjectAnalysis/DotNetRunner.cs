// <copyright file="DotNetRunner.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ProjectAnalysis
{
    using System.Diagnostics;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// A class that manages the running of .NET commands on project and solution files.
    /// </summary>
    internal class DotNetRunner
    {
        /// <summary>
        /// The logger for asynchronous messages that have been created by external processes.
        /// </summary>
        private readonly ILogger<DotNetRunner> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetRunner"/> class.
        /// </summary>
        /// <param name="logger">The logger for asynchronous messages that have been created by external
        /// processes.</param>
        public DotNetRunner(ILogger<DotNetRunner> logger) =>
            this.logger = logger;

        /// <summary>
        /// Runs the .NET process.
        /// </summary>
        /// <param name="parameters">The parameters to pass to the "dotnet" command, excluding the file name of the
        /// executable.</param>
        /// <param name="workingDirectory">The path of the directory in which to store the files created after running
        /// the "dotnet" command.</param>
        public void Run(string parameters, string workingDirectory)
        {
            using var process = new Process();

            process.ErrorDataReceived += this.LogError;
            process.OutputDataReceived += this.LogOutput;
            process.StartInfo = new("dotnet", parameters)
            {
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WorkingDirectory = workingDirectory,
            };

            // Start() will return a Boolean value indicating whether a new process was started. A false return value
            // indicates that an existing process was reused and is not indicative of failure.
            _ = process.Start();

            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

            process.WaitForExit();
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
