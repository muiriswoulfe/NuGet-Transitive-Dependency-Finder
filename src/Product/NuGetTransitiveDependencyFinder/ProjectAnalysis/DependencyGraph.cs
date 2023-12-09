// <copyright file="DependencyGraph.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ProjectAnalysis;

using System;
using System.Diagnostics;
using System.IO;
using NuGet.ProjectModel;
using NuGetTransitiveDependencyFinder.Wrappers;
using static System.FormattableString;

/// <summary>
/// A class representing a dependency graph of .NET projects and their NuGet dependencies.
/// </summary>
/// <param name="dotNetRunner">The object managing the running of .NET commands on project and solution
/// files.</param>
/// <param name="processWrapper">The wrapper around <see cref="Process"/>.</param>
internal sealed class DependencyGraph(IDotNetRunner dotNetRunner, IProcessWrapper processWrapper) : IDependencyGraph
{
    /// <summary>
    /// A temporary file for storing the dependency graph information.
    /// </summary>
    /// <remarks>This file will be deleted when <see cref="Dispose()"/> is invoked.</remarks>
    private readonly string filePath = Path.Join(AppContext.BaseDirectory, Path.GetRandomFileName());

    /// <summary>
    /// The output from the command to check for the existence of MSBuild.
    /// </summary>
    private string msBuildCheckOutput = string.Empty;

    /// <summary>
    /// A value tracking whether <see cref="Dispose()"/> has been invoked.
    /// </summary>
    private bool disposedValue;

    /// <summary>
    /// Finalizes an instance of the <see cref="DependencyGraph"/> class.
    /// </summary>
    ~DependencyGraph() =>
        this.Dispose(false);

    /// <inheritdoc/>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    public DependencyGraphSpec Create(string projectOrSolutionPath)
    {
        var msBuildAvailable = this.IsMSBuildAvailable();

        var projectOrSolutionDirectory = Path.GetDirectoryName(projectOrSolutionPath)!;
        var buildCommand = msBuildAvailable ? "msbuild" : "dotnet build";
        var arguments =
            Invariant($"{buildCommand} \"{projectOrSolutionPath}\" /maxCpuCount /target:GenerateRestoreGraphFile ") +
            Invariant($"/property:RestoreGraphOutputPath=\"{this.filePath}\"");
        dotNetRunner.Run(arguments, projectOrSolutionDirectory);

        return DependencyGraphSpec.Load(this.filePath);
    }

    /// <summary>
    /// Determines whether the msbuild command is available.
    /// </summary>
    /// <returns><c>true</c> is msbuild is available; <c>false</c> otherwise.</returns>
    private bool IsMSBuildAvailable()
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = Environment.OSVersion.Platform == PlatformID.Win32NT ? "where" : "which",
            Arguments = "msbuild",
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
        };

        this.msBuildCheckOutput = string.Empty;

        processWrapper.Start(startInfo, this.LogOutput!, this.LogError!);

        processWrapper.BeginErrorReadLine();
        processWrapper.BeginOutputReadLine();

        processWrapper.WaitForExit();

        return !string.IsNullOrWhiteSpace(this.msBuildCheckOutput);
    }

    /// <summary>
    /// Disposes of the resources maintained by the current object.
    /// </summary>
    /// <param name="disposing">A value indicating whether disposal should be performed for managed as well as unmanaged
    /// resources.</param>
    private void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                File.Delete(this.filePath);
            }

            this.disposedValue = true;
        }
    }

    /// <summary>
    /// Handles messages sent to the Standard Error stream.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="eventParameters">The event parameters.</param>
    /// <exception cref="InvalidOperationException">Thrown if an error occurs.</exception>
    private void LogError(object sender, DataReceivedEventArgs eventParameters)
    {
        if (!string.IsNullOrWhiteSpace(eventParameters.Data))
        {
            throw new InvalidOperationException(eventParameters.Data);
        }
    }

    /// <summary>
    /// Records messages sent to the Standard Output stream.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="eventParameters">The event parameters.</param>
    private void LogOutput(object sender, DataReceivedEventArgs eventParameters)
    {
        if (!string.IsNullOrWhiteSpace(eventParameters.Data))
        {
            this.msBuildCheckOutput += eventParameters.Data;
        }
    }
}
