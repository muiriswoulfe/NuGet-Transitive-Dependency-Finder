// <copyright file="TestCollateralFixture.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.IntegrationTests;

using System;
using System.Diagnostics;
using System.IO;

/// <summary>
/// An xUnit fixture that ensures the TestCollateral projects have been restored before integration tests run. Restoring
/// the projects populates their <c>project.assets.json</c> lockfiles, which the library reads during its analysis.
/// </summary>
public sealed class TestCollateralFixture : IDisposable
{
    private bool disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestCollateralFixture"/> class and restores every TestCollateral
    /// project.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if <c>dotnet restore</c> cannot be started.</exception>
    public TestCollateralFixture()
    {
        Restore(TestCollateralPaths.NoTransitiveDependenciesProject);
        Restore(TestCollateralPaths.TransitiveDependenciesProject);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (this.disposed)
        {
            return;
        }

        this.disposed = true;
    }

    /// <summary>
    /// Runs <c>dotnet restore</c> on the specified project path, draining both standard streams to avoid deadlocks.
    /// </summary>
    /// <param name="projectPath">The absolute path to the project to restore.</param>
    /// <exception cref="InvalidOperationException">Thrown if <c>dotnet restore</c> cannot be started.</exception>
    private static void Restore(string projectPath)
    {
        var startInfo = new ProcessStartInfo("dotnet", $"restore \"{projectPath}\"")
        {
            WorkingDirectory = Path.GetDirectoryName(projectPath),
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };
        using var process = Process.Start(startInfo)
            ?? throw new InvalidOperationException("Unable to start dotnet restore.");
        _ = process.StandardOutput.ReadToEnd();
        _ = process.StandardError.ReadToEnd();
        process.WaitForExit();
    }
}

/// <summary>
/// xUnit collection definition that shares a single <see cref="TestCollateralFixture"/> across all integration tests,
/// avoiding redundant restores.
/// </summary>
[Xunit.CollectionDefinition("TestCollateral")]
public sealed class TestCollateralCollection : Xunit.ICollectionFixture<TestCollateralFixture>;
