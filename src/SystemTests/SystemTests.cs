// <copyright file="SystemTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.SystemTests;

using System.Diagnostics;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using static System.FormattableString;

/// <summary>
/// System tests for the NuGet Transitive Dependency Finder console app.
/// </summary>
public class SystemTests
{
    /// <summary>
    /// The current version of .NET.
    /// </summary>
    private const string DotNetVersion = "net7.0";

    /// <summary>
    /// The current configuration.
    /// </summary>
    private const string Configuration =
#if DEBUG
        "Debug";
#else
        "Release";
#endif

    /// <summary>
    /// Tests the console app using a solution.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [Fact]
    public Task TestSolution() =>
        this.Test("../../../../../TestCollateral/TestCollateral.sln");

    /// <summary>
    /// Tests the console app using a project with no transitive dependencies.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [Fact]
    public Task TestProjectWithNoTransitiveDependencies() =>
        this.Test("../../../../../TestCollateral/NoTransitiveDependencies/NoTransitiveDependencies.csproj");

    /// <summary>
    /// Tests the console app using a project with no transitive dependencies.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [Fact]
    public Task TestProjectWithTransitiveDependencies() =>
        this.Test("../../../../../TestCollateral/TransitiveDependencies/TransitiveDependencies.csproj");

    /// <summary>
    /// Tests the console app using the specified project or solution.
    /// </summary>
    /// <param name="path">The path to the project or solution.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    private async Task Test(string path)
    {
        // Arrange
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = Invariant($"dotnet-transitive-dependency-finder.dll --projectOrSolution {path}"),
            WorkingDirectory = Invariant($"../../../../Product/NuGetTransitiveDependencyFinder.ConsoleApp/bin/{Configuration}/{DotNetVersion}/"),
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
        };
        var process = new Process
        {
            StartInfo = processStartInfo,
        };

        // Act
        var result = process.Start();
        await process.WaitForExitAsync();
        var error = process.StandardError.ReadToEndAsync();
        var output = process.StandardOutput.ReadToEndAsync();

        // Assert
        _ = result
            .Should().BeTrue();
        _ = (await error)
            .Should().BeEmpty();
        _ = (await output)
            .Should().BeEmpty();
    }
}
