// <copyright file="ConsoleAppRunner.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.SystemTests;

using System;
using System.Diagnostics;
using System.IO;
using System.Text;

/// <summary>
/// Represents the outcome of running the ConsoleApp — exit code plus captured standard streams.
/// </summary>
/// <param name="ExitCode">The process exit code.</param>
/// <param name="StandardOutput">The combined standard output stream.</param>
/// <param name="StandardError">The combined standard error stream.</param>
internal sealed record ConsoleAppResult(int ExitCode, string StandardOutput, string StandardError);

/// <summary>
/// Runs the ConsoleApp tool out-of-process using <c>dotnet &lt;dll&gt;</c> and captures the exit code and standard
/// streams.
/// </summary>
internal static class ConsoleAppRunner
{
    /// <summary>
    /// The ConsoleApp assembly file name, matching the <c>AssemblyName</c> value in
    /// <c>NuGetTransitiveDependencyFinder.ConsoleApp.csproj</c>.
    /// </summary>
    private const string ConsoleAppAssembly = "dotnet-transitive-dependency-finder.dll";

    /// <summary>
    /// Runs the ConsoleApp with the specified arguments.
    /// </summary>
    /// <param name="arguments">The command-line arguments to pass to the ConsoleApp.</param>
    /// <returns>A <see cref="ConsoleAppResult"/> describing the outcome.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the process fails to start.</exception>
    public static ConsoleAppResult Run(string arguments)
    {
        var assemblyPath = Path.Combine(SystemTestPaths.ConsoleAppOutputDirectory, ConsoleAppAssembly);
        var startInfo = new ProcessStartInfo("dotnet", $"\"{assemblyPath}\" {arguments}")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };

        var stdOut = new StringBuilder();
        var stdErr = new StringBuilder();
        using var process = Process.Start(startInfo)
            ?? throw new InvalidOperationException("Unable to start ConsoleApp process.");
        while (!process.StandardOutput.EndOfStream)
        {
            _ = stdOut.AppendLine(process.StandardOutput.ReadLine());
        }

        while (!process.StandardError.EndOfStream)
        {
            _ = stdErr.AppendLine(process.StandardError.ReadLine());
        }

        process.WaitForExit();
        return new ConsoleAppResult(process.ExitCode, stdOut.ToString(), stdErr.ToString());
    }
}
