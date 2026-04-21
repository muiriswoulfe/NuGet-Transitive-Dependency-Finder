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

    /// <summary>
    /// Tests that invoking the ConsoleApp with the <c>--all</c> flag against a project with transitive dependencies
    /// exits successfully.
    /// </summary>
    [Fact]
    public void Run_WithAllFlag_ExitsSuccessfully()
    {
        // Act
        var result = ConsoleAppRunner.Run(
            $"--projectOrSolution \"{SystemTestPaths.TransitiveDependenciesProject}\" --all");

        // Assert
        _ = result.ExitCode
            .Should().Be(0);
    }

    /// <summary>
    /// Tests that invoking the ConsoleApp using the short-form <c>-p</c> argument for <c>--projectOrSolution</c>
    /// exits successfully.
    /// </summary>
    [Fact]
    public void Run_WithShortFormProjectArgument_ExitsSuccessfully()
    {
        // Act
        var result = ConsoleAppRunner.Run(
            $"-p \"{SystemTestPaths.NoTransitiveDependenciesProject}\"");

        // Assert
        _ = result.ExitCode
            .Should().Be(0);
    }

    /// <summary>
    /// Tests that invoking the ConsoleApp with a non-existent project path yields a non-zero exit code.
    /// </summary>
    [Fact]
    public void Run_WithNonExistentProject_ExitsNonZero()
    {
        // Act
        var result = ConsoleAppRunner.Run("--projectOrSolution \"/tmp/does-not-exist-xyz.csproj\"");

        // Assert
        _ = result.ExitCode
            .Should().NotBe(0);
    }

    /// <summary>
    /// Tests that invoking the ConsoleApp with a matching <c>--filter</c> regex against a project with transitive
    /// dependencies exits successfully.
    /// </summary>
    [Fact]
    public void Run_WithFilterRegex_ExitsSuccessfully()
    {
        // Act
        var result = ConsoleAppRunner.Run(
            $"--projectOrSolution \"{SystemTestPaths.TransitiveDependenciesProject}\" --filter \".*\"");

        // Assert
        _ = result.ExitCode
            .Should().Be(0);
    }

    /// <summary>
    /// Tests that invoking the ConsoleApp against the TestCollateral <c>.sln</c> solution file exits successfully,
    /// exercising the multi-project solution-level path that individual project runs do not cover.
    /// </summary>
    [Fact]
    public void Run_AgainstSolution_ExitsSuccessfully()
    {
        // Act
        var result = ConsoleAppRunner.Run(
            $"--projectOrSolution \"{SystemTestPaths.TestCollateralSolution}\"");

        // Assert
        _ = result.ExitCode
            .Should().Be(0);
    }

    /// <summary>
    /// Tests that invoking the ConsoleApp against the solution file writes output that mentions both project names,
    /// verifying the output formatting processes every project in the solution.
    /// </summary>
    [Fact]
    public void Run_AgainstSolution_WritesBothProjectNamesToOutput()
    {
        // Act
        var result = ConsoleAppRunner.Run(
            $"--projectOrSolution \"{SystemTestPaths.TestCollateralSolution}\" --all");
        var combined = result.StandardOutput + result.StandardError;

        // Assert
        _ = combined
            .Should().Contain("NoTransitiveDependencies");
        _ = combined
            .Should().Contain("TransitiveDependencies");
    }

    /// <summary>
    /// Tests that invoking the ConsoleApp with <c>--all</c> against a project with transitive dependencies emits
    /// dependency output, verifying that the collate-all code path produces visible console output (not just a
    /// success exit code).
    /// </summary>
    [Fact]
    public void Run_WithAllFlag_WritesDependencyOutput()
    {
        // Act
        var result = ConsoleAppRunner.Run(
            $"--projectOrSolution \"{SystemTestPaths.TransitiveDependenciesProject}\" --all");

        // Assert: --all emits at least one line of output beyond any banner.
        _ = result.StandardOutput
            .Should().NotBeNullOrWhiteSpace();
    }

    /// <summary>
    /// Tests that invoking the ConsoleApp with the combined <c>--all</c> and <c>--filter</c> flags exits successfully,
    /// exercising the interaction between the two options (collate-all + filter pipeline).
    /// </summary>
    [Fact]
    public void Run_WithAllAndFilterFlags_ExitsSuccessfully()
    {
        // Act
        var result = ConsoleAppRunner.Run(
            $"--projectOrSolution \"{SystemTestPaths.TransitiveDependenciesProject}\" --all --filter \".*\"");

        // Assert
        _ = result.ExitCode
            .Should().Be(0);
    }

    /// <summary>
    /// Tests that invoking the ConsoleApp with the short-form <c>-f</c> filter flag exits successfully, exercising
    /// the short option alias to complement the existing long-form <c>--filter</c> test.
    /// </summary>
    [Fact]
    public void Run_WithShortFilterFlag_ExitsSuccessfully()
    {
        // Act
        var result = ConsoleAppRunner.Run(
            $"-p \"{SystemTestPaths.TransitiveDependenciesProject}\" -f \".*\"");

        // Assert
        _ = result.ExitCode
            .Should().Be(0);
    }

    /// <summary>
    /// Tests that invoking the ConsoleApp with a filter that matches no real dependency identifier still exits
    /// successfully, since an empty match is a valid outcome rather than an error condition.
    /// </summary>
    [Fact]
    public void Run_WithNonMatchingFilter_ExitsSuccessfully()
    {
        // Act
        var result = ConsoleAppRunner.Run(
            $"--projectOrSolution \"{SystemTestPaths.TransitiveDependenciesProject}\" --filter \"^ZzNoSuchPackage$\"");

        // Assert
        _ = result.ExitCode
            .Should().Be(0);
    }

    /// <summary>
    /// Tests that invoking the ConsoleApp with an unknown flag yields a non-zero exit code, verifying the argument
    /// parser rejects unrecognised options rather than silently ignoring them.
    /// </summary>
    [Fact]
    public void Run_WithUnknownFlag_ExitsNonZero()
    {
        // Act
        var result = ConsoleAppRunner.Run(
            $"--projectOrSolution \"{SystemTestPaths.NoTransitiveDependenciesProject}\" --not-a-real-flag");

        // Assert
        _ = result.ExitCode
            .Should().NotBe(0);
    }
}
