// <copyright file="DotNetRunner.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ProjectAnalysis;

using System.Diagnostics;
using Microsoft.Extensions.Logging;
using NuGetTransitiveDependencyFinder.Wrappers;

/// <summary>
/// A class that manages the running of .NET commands on project and solution files.
/// </summary>
/// <param name="logger">The logger for asynchronous messages that have been created by external processes.</param>
/// <param name="processWrapper">The wrapper around <see cref="Process"/>.</param>
internal class DotNetRunner(ILogger<DotNetRunner> logger, IProcessWrapper processWrapper) : IDotNetRunner
{
    /// <inheritdoc/>
    public void Run(string parameters, string workingDirectory)
    {
        var startInfo = new ProcessStartInfo("dotnet", parameters)
        {
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            WorkingDirectory = workingDirectory,
        };

        processWrapper.Start(startInfo, this.LogOutput!, this.LogError!);

        processWrapper.BeginErrorReadLine();
        processWrapper.BeginOutputReadLine();

        processWrapper.WaitForExit();
    }

    /// <summary>
    /// Logs messages sent to the Standard Error stream.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="eventParameters">The event parameters.</param>
    private void LogError(object sender, DataReceivedEventArgs eventParameters) =>
        logger.LogError(eventParameters.Data ?? string.Empty);

    /// <summary>
    /// Logs messages sent to the Standard Output stream.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="eventParameters">The event parameters.</param>
    private void LogOutput(object sender, DataReceivedEventArgs eventParameters) =>
        logger.LogTrace(eventParameters.Data ?? string.Empty);
}
