// <copyright file="DotNetRunner.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGet.TransitiveDependency.Finder.Library.ProjectAnalysis
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
        private readonly ILogger logger;

        /// <summary>
        /// Information to be provided to the process initialization operation.
        /// </summary>
        private readonly ProcessStartInfo processStartInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetRunner"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory from which a logger will be created.</param>
        /// <param name="parameters">The parameters to pass to the "dotnet" command, excluding the file name of the
        /// executable.</param>
        /// <param name="workingDirectory">The path of the directory in which to store the files created after running
        /// the "dotnet" command.</param>
        public DotNetRunner(ILoggerFactory loggerFactory, string parameters, string workingDirectory)
        {
            this.logger = loggerFactory.CreateLogger(nameof(DotNetRunner));
            this.processStartInfo = new ProcessStartInfo("dotnet", parameters)
            {
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WorkingDirectory = workingDirectory,
            };
        }

        /// <summary>
        /// Runs the .NET process.
        /// </summary>
        public void Run()
        {
            using var process = new Process();

            process.ErrorDataReceived += this.LogError;
            process.OutputDataReceived += this.LogOutput;
            process.StartInfo = this.processStartInfo;

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
            this.logger.LogDebug(e.Data ?? string.Empty);
    }
}
