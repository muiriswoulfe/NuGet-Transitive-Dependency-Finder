// <copyright file="DependencyGraph.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ProjectAnalysis;

using System;
using System.Diagnostics;
using System.IO;
using NuGet.ProjectModel;
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
    /// A temporary file for storing the dependency graph information.
    /// </summary>
    /// <remarks>This file will be deleted when <see cref="Dispose()"/> is invoked.</remarks>
    private readonly string filePath;

    /// <summary>
    /// A value tracking whether <see cref="Dispose()"/> has been invoked.
    /// </summary>
    private bool disposedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="DependencyGraph"/> class.
    /// </summary>
    /// <param name="dotNetRunner">The object managing the running of .NET commands on project and solution
    /// files.</param>
    public DependencyGraph(IDotNetRunner dotNetRunner)
    {
        this.dotNetRunner = dotNetRunner;
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
    public async Task<DependencyGraphSpec> CreateAsync(string projectOrSolutionPath)
    {
        var msBuildAvailable = this.IsMSBuildAvailableAsync();

        var projectOrSolutionDirectory = Path.GetDirectoryName(projectOrSolutionPath)!;
        var buildCommand = await msBuildAvailable ? "msbuild" : "dotnet build";
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
    private async Task<bool> IsMSBuildAvailableAsync()
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = Environment.OSVersion.Platform == PlatformID.Win32NT ? "where" : "which",
            Arguments = "msbuild",
            RedirectStandardOutput = true,
            UseShellExecute = false,
        };

        var process = new Process
        {
            StartInfo = processStartInfo,
        };
        _ = process.Start();
        var output = process.StandardOutput.ReadToEndAsync();
        await process.WaitForExitAsync();

        return !string.IsNullOrWhiteSpace(await output);
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
}
