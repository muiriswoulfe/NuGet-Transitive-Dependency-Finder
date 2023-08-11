// <copyright file="SystemTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.SystemTests;

using System.Diagnostics;
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
    [Fact]
    public void TestSolution() =>
        this.Test("../../../../../TestCollateral/TestCollateral.sln");

    /// <summary>
    /// Tests the console app using a project with no transitive dependencies.
    /// </summary>
    [Fact]
    public void TestProjectWithNoTransitiveDependencies() =>
        this.Test("../../../../../TestCollateral/NoTransitiveDependencies/NoTransitiveDependencies.csproj");

    /// <summary>
    /// Tests the console app using a project with no transitive dependencies.
    /// </summary>
    [Fact]
    public void TestProjectWithTransitiveDependencies() =>
        this.Test("../../../../../TestCollateral/TransitiveDependencies/TransitiveDependencies.csproj");

    /// <summary>
    /// Tests the console app using the specified project or solution.
    /// </summary>
    /// <param name="path">The path to the project or solution.</param>
    private void Test(string path)
    {
        // Arrange
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = Invariant($"dotnet-transitive-dependency-finder.dll --projectOrSolution {path}"),
            WorkingDirectory = "../../../../Product/NuGetTransitiveDependencyFinder.ConsoleApp/bin/" +
                Invariant($"{Configuration}/{DotNetVersion}/"),
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
        var error = process.StandardError.ReadToEnd();
        var output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        // Assert
        _ = result
            .Should().BeTrue();
        _ = error
            .Should().BeEmpty();
        _ = output
            .Should().BeEmpty();
    }
}
