// <copyright file="ProgramInitializerUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.ConsoleApp.Process;

using System;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;
using NuGetTransitiveDependencyFinder.ConsoleApp.Input;
using NuGetTransitiveDependencyFinder.ConsoleApp.Output;
using NuGetTransitiveDependencyFinder.ConsoleApp.Process;
using NuGetTransitiveDependencyFinder.TestUtilities.Globalization;
using Xunit;

/// <summary>
/// Unit tests for the <see cref="ProgramInitializer"/> class.
/// </summary>
public class ProgramInitializerUnitTests
{
    /// <summary>
    /// The success code returned in valid cases.
    /// </summary>
    private const int SuccessCode = 0;

    /// <summary>
    /// The error code returned in failure cases.
    /// </summary>
    private const int ErrorCode = 1;

    /// <summary>
    /// Tests that when <see cref="ProgramInitializer.Run(string[], Action{IServiceProvider})"/> is called, the
    /// <see cref="IServiceProvider"/> is initialized with the expected dependencies.
    /// </summary>
    [AllCulturesFact]
    public void Run_Called_InitializesDependencies()
    {
        // Act
        var result = ProgramInitializer.Run(
            new[]
            {
                "--projectOrSolution",
                "Project.csproj",
            },
            serviceProvider =>
            {
                _ = serviceProvider.GetService<ICommandLineOptions>()
                    .Should().BeOfType<CommandLineOptions>();
                _ = serviceProvider.GetService<IDependencyWriter>()
                    .Should().BeOfType<DependencyWriter>();
                _ = serviceProvider.GetService<IProgramRunner>()
                    .Should().BeOfType<ProgramRunner>();

                _ = serviceProvider.GetService<ITransitiveDependencyFinder>()
                    .Should().NotBeNull();

                var logger = serviceProvider.GetService<ILogger<ProgramInitializerUnitTests>>()!;
                _ = logger
                    .Should().BeOfType<Logger<ProgramInitializerUnitTests>>();
                foreach (var value in
                    Enum.GetValues(typeof(LogLevel)).Cast<LogLevel>().Where(logLevel => logLevel != LogLevel.None))
                {
                    _ = logger.IsEnabled(value)
                        .Should().BeTrue();
                }

                _ = serviceProvider.GetService<ConsoleFormatter>()
                    .Should().BeOfType<PlainConsoleFormatter>();
                _ = serviceProvider.GetService<ILoggerProvider>()
                    .Should().BeOfType<DebugLoggerProvider>();
            });

        // Assert
        _ = result
            .Should().Be(SuccessCode);
    }

    /// <summary>
    /// Tests that when <see cref="ProgramInitializer.Run(string[], Action{IServiceProvider})"/> is called with valid
    /// command-line parameters, it returns the success code.
    /// </summary>
    /// <param name="parameters">The command-line parameters to test.</param>
    [AllCulturesTheory]
    [InlineData("-p Project.csproj")]
    [InlineData("-p Project.csproj -a")]
    [InlineData("-p Project.csproj --all")]
    [InlineData("--projectOrSolution Project.csproj")]
    [InlineData("--projectOrSolution Project.csproj -a")]
    [InlineData("--projectOrSolution Project.csproj --all")]
    public void Run_WithValidCommandLineParameters_ReturnsSuccessCode(string parameters)
    {
        // Act
        var result = ProgramInitializer.Run(parameters.Split(' ', StringSplitOptions.RemoveEmptyEntries), _ => { });

        // Assert
        _ = result
            .Should().Be(SuccessCode);
    }

    /// <summary>
    /// Tests that when <see cref="ProgramInitializer.Run(string[], Action{IServiceProvider})"/> is called with failure
    /// case command-line parameters, it returns the error code.
    /// </summary>
    /// <param name="parameters">The command-line parameters to test.</param>
    [AllCulturesTheory]
    [InlineData("-a")]
    [InlineData("--all")]
    [InlineData("-p")]
    [InlineData("--project")]
    [InlineData("--help")]
    [InlineData("--version")]
    [InlineData("--invalid")]
    [InlineData("--projectOrSolution Project.csproj --projectOrSolution Project.csproj")]
    [InlineData("--projectOrSolution Project.csproj --all --all")]
    [InlineData("--projectOrSolution Project.csproj --help")]
    [InlineData("--projectOrSolution Project.csproj --version")]
    [InlineData("--projectOrSolution Project.csproj --invalid")]
    [InlineData("-p Project.csproj -p Project.csproj")]
    [InlineData("-p Project.csproj --all --all")]
    [InlineData("-p Project.csproj --help")]
    [InlineData("-p Project.csproj --version")]
    [InlineData("-p Project.csproj --invalid")]
    public void Run_WithFailureCaseCommandLineParameters_ReturnsErrorCode(string parameters)
    {
        // Act
        var result = ProgramInitializer.Run(parameters.Split(' ', StringSplitOptions.RemoveEmptyEntries), _ => { });

        // Assert
        _ = result
            .Should().Be(ErrorCode);
    }

    /// <summary>
    /// Tests that when <see cref="ProgramInitializer.GetProgramRunner(IServiceProvider)"/> is called, the
    /// <see cref="IProgramRunner"/> dependency is returned.
    /// </summary>
    [AllCulturesFact]
    public void GetProgramRunner_Called_ReturnsDependency()
    {
        // Act
        var result = ProgramInitializer.Run(
            new[]
            {
                "--projectOrSolution",
                "Project.csproj",
            },
            serviceProvider =>
                ProgramInitializer.GetProgramRunner(serviceProvider)
                    .Should().BeOfType<ProgramRunner>());

        // Assert
        _ = result
            .Should().Be(SuccessCode);
    }
}
