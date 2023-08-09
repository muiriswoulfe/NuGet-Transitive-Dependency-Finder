// <copyright file="SystemTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.SystemTests;

using System.Diagnostics;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

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
    /// Tests the console app using a solution.
    /// </summary>
    /// <param name="configuration">The app configuration, which should be either Debug or Release.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [Theory]
    [InlineData("Debug")]
    [InlineData("Release")]
    public Task TestSolution(string configuration) =>
        this.Test("../../../../../../../TestCollateral.sln", configuration);

    /// <summary>
    /// Tests the console app using a project with no transitive dependencies.
    /// </summary>
    /// <param name="configuration">The app configuration, which should be either Debug or Release.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [Theory]
    [InlineData("Debug")]
    [InlineData("Release")]
    public Task TestProjectWithNoTransitiveDependencies(string configuration) =>
        this.Test("../../../../../../../NoTransitiveDependencies/NoTransitiveDependencies.csproj", configuration);

    /// <summary>
    /// Tests the console app using a project with no transitive dependencies.
    /// </summary>
    /// <param name="configuration">The app configuration, which should be either Debug or Release.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [Theory]
    [InlineData("Debug")]
    [InlineData("Release")]
    public Task TestProjectWithTransitiveDependencies(string configuration) =>
        this.Test("../../../../../../../TransitiveDependencies/TransitiveDependencies.csproj", configuration);

    /// <summary>
    /// Tests the console app using the specified project or solution.
    /// </summary>
    /// <param name="path">The path to the project or solution.</param>
    /// <param name="configuration">The app configuration, which should be either Debug or Release.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    private async Task Test(string path, string configuration)
    {
        // Arrange
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"dotnet-transitive-dependency-finder.dll --projectOrSolution {path}",
            WorkingDirectory = $"../../../../Product/NuGetTransitiveDependencyFinder.ConsoleApp/bin/{configuration}/{DotNetVersion}/",
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
