// <copyright file="ConsoleAppSystemTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.SystemTests;

using FluentAssertions;
using Xunit;

/// <summary>
/// System tests that invoke the compiled ConsoleApp executable out-of-process and assert on its exit code and console
/// output.
/// </summary>
public sealed class ConsoleAppSystemTests
{
    /// <summary>
    /// Tests that invoking the ConsoleApp with no arguments yields a non-zero exit code because the
    /// <c>--projectOrSolution</c> argument is required.
    /// </summary>
    [Fact]
    public void Run_WithNoArguments_ExitsNonZero()
    {
        // Act
        var result = ConsoleAppRunner.Run(string.Empty);

        // Assert
        _ = result.ExitCode
            .Should().NotBe(0);
    }

    /// <summary>
    /// Tests that invoking the ConsoleApp with <c>--help</c> exits with code <c>0</c> or <c>1</c> (CommandLineParser
    /// behaviour) and writes output describing the tool.
    /// </summary>
    [Fact]
    public void Run_WithHelpOption_WritesHelpOutput()
    {
        // Act
        var result = ConsoleAppRunner.Run("--help");

        // Assert
        _ = (result.StandardOutput + result.StandardError)
            .Should().Contain("projectOrSolution");
    }

    /// <summary>
    /// Tests that invoking the ConsoleApp against a project without transitive dependencies exits successfully.
    /// </summary>
    [Fact]
    public void Run_AgainstNoTransitiveDependenciesProject_ExitsSuccessfully()
    {
        // Act
        var result = ConsoleAppRunner.Run(
            $"--projectOrSolution \"{SystemTestPaths.NoTransitiveDependenciesProject}\"");

        // Assert
        _ = result.ExitCode
            .Should().Be(0);
    }

    /// <summary>
    /// Tests that invoking the ConsoleApp against a project with transitive dependencies exits successfully.
    /// </summary>
    [Fact]
    public void Run_AgainstTransitiveDependenciesProject_ExitsSuccessfully()
    {
        // Act
        var result = ConsoleAppRunner.Run(
            $"--projectOrSolution \"{SystemTestPaths.TransitiveDependenciesProject}\"");

        // Assert
        _ = result.ExitCode
            .Should().Be(0);
    }

    /// <summary>
    /// Tests that invoking the ConsoleApp with an invalid <c>--filter</c> regex yields a non-zero exit code.
    /// </summary>
    [Fact]
    public void Run_WithInvalidFilterRegex_ExitsNonZero()
    {
        // Act
        var result = ConsoleAppRunner.Run(
            $"--projectOrSolution \"{SystemTestPaths.NoTransitiveDependenciesProject}\" --filter \"[invalid\"");

        // Assert
        _ = result.ExitCode
            .Should().NotBe(0);
    }
}
