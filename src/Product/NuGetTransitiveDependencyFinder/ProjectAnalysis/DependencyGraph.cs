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
internal sealed class DependencyGraph : IDependencyGraph
{
    /// <summary>
    /// The object managing the running of .NET commands on project and solution files.
    /// </summary>
    private readonly IDotNetRunner dotNetRunner;

    /// <summary>
    /// The wrapper around <see cref="Process"/>.
    /// </summary>
    private readonly IProcessWrapper processWrapper;

    /// <summary>
    /// A temporary file for storing the dependency graph information.
    /// </summary>
    /// <remarks>This file will be deleted when <see cref="Dispose()"/> is invoked.</remarks>
    private readonly string filePath;

    /// <summary>
    /// The output from the command to check for the existence of MSBuild.
    /// </summary>
    private string msBuildCheckOutput = string.Empty;

    /// <summary>
    /// A value tracking whether <see cref="Dispose()"/> has been invoked.
    /// </summary>
    private bool disposedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="DependencyGraph"/> class.
    /// </summary>
    /// <param name="dotNetRunner">The object managing the running of .NET commands on project and solution
    /// files.</param>
    /// <param name="processWrapper">The wrapper around <see cref="Process"/>.</param>
    public DependencyGraph(IDotNetRunner dotNetRunner, IProcessWrapper processWrapper)
    {
        this.dotNetRunner = dotNetRunner;
        this.processWrapper = processWrapper;

        this.filePath = Path.Join(AppContext.BaseDirectory, Path.GetRandomFileName());
    }

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
        this.dotNetRunner.Run(arguments, projectOrSolutionDirectory);

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
            RedirectStandardOutput = true,
            UseShellExecute = false,
        };

        this.msBuildCheckOutput = string.Empty;

        this.processWrapper.Start(startInfo, this.LogOutput!, this.LogError!);

        this.processWrapper.BeginErrorReadLine();
        this.processWrapper.BeginOutputReadLine();

        this.processWrapper.WaitForExit();

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
