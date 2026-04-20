// <copyright file="DotNetRunnerUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.ProjectAnalysis;

using System.Diagnostics;
using FluentAssertions;
using Moq;
using NuGetTransitiveDependencyFinder.ProjectAnalysis;
using NuGetTransitiveDependencyFinder.UnitTests.Utilities.Globalization;
using NuGetTransitiveDependencyFinder.UnitTests.Utilities.Logging;
using NuGetTransitiveDependencyFinder.Wrappers;

/// <summary>
/// Unit tests for the <see cref="DotNetRunner"/> class.
/// </summary>
public class DotNetRunnerUnitTests
{
    /// <summary>
    /// Tests that when <see cref="DotNetRunner.Run(string, string)"/> is called, it performs the expected actions.
    /// </summary>
    [AllCulturesFact]
    public void Run_CalledWithAnyStartReturnValue_PerformsExpectedActions()
    {
        // Arrange
        var logger = new MockLogger<DotNetRunner>();
        var processWrapper = new Mock<IProcessWrapper>();
        _ = processWrapper
            .Setup(
                obj => obj.Start(
                    It.IsAny<ProcessStartInfo>(),
                    It.IsAny<DataReceivedEventHandler>(),
                    It.IsAny<DataReceivedEventHandler>()));
        var dotNetRunner = new DotNetRunner(logger, processWrapper.Object);

        // Act
        dotNetRunner.Run("build", @"..\");

        // Assert
        _ = logger.Entries
            .Should().BeEmpty();
        processWrapper.Verify(
            obj => obj.Start(
                It.IsAny<ProcessStartInfo>(),
                It.IsAny<DataReceivedEventHandler>(),
                It.IsAny<DataReceivedEventHandler>()),
            Times.Once());
        processWrapper.Verify(obj => obj.BeginErrorReadLine(), Times.Once());
        processWrapper.Verify(obj => obj.BeginOutputReadLine(), Times.Once());
        processWrapper.Verify(obj => obj.WaitForExit(), Times.Once());
    }

    /// <summary>
    /// Tests that when <see cref="DotNetRunner.Run(string, string)"/> is called, the process is started with the
    /// expected <see cref="ProcessStartInfo"/> values: arguments, working directory, and redirected streams.
    /// </summary>
    [AllCulturesFact]
    public void Run_WhenInvoked_StartsProcessWithExpectedStartInfo()
    {
        // Arrange
        const string arguments = "restore \"/some/project.csproj\"";
        const string workingDirectory = "/some";
        var logger = new MockLogger<DotNetRunner>();
        var processWrapper = new Mock<IProcessWrapper>();
        ProcessStartInfo? capturedStartInfo = null;
        _ = processWrapper
            .Setup(
                obj => obj.Start(
                    It.IsAny<ProcessStartInfo>(),
                    It.IsAny<DataReceivedEventHandler>(),
                    It.IsAny<DataReceivedEventHandler>()))
            .Callback<ProcessStartInfo, DataReceivedEventHandler, DataReceivedEventHandler>(
                (startInfo, _, _) => capturedStartInfo = startInfo);
        var dotNetRunner = new DotNetRunner(logger, processWrapper.Object);

        // Act
        dotNetRunner.Run(arguments, workingDirectory);

        // Assert
        _ = capturedStartInfo
            .Should().NotBeNull();
        _ = capturedStartInfo!.FileName
            .Should().Be("dotnet");
        _ = capturedStartInfo.Arguments
            .Should().Be(arguments);
        _ = capturedStartInfo.WorkingDirectory
            .Should().Be(workingDirectory);
        _ = capturedStartInfo.RedirectStandardOutput
            .Should().BeTrue();
        _ = capturedStartInfo.RedirectStandardError
            .Should().BeTrue();
    }

    /// <summary>
    /// Tests that when <see cref="DotNetRunner.Run(string, string)"/> is called, the error and output handlers it
    /// registers are wired to the logger via <see cref="MockLogger{T}"/>, routing error data to
    /// <see cref="Microsoft.Extensions.Logging.LogLevel.Error"/> and output data to
    /// <see cref="Microsoft.Extensions.Logging.LogLevel.Trace"/>.
    /// </summary>
    [AllCulturesFact]
    public void Run_WhenHandlersInvoked_ForwardsToLoggerWithCorrectLevels()
    {
        // Arrange
        var logger = new MockLogger<DotNetRunner>();
        var processWrapper = new Mock<IProcessWrapper>();
        DataReceivedEventHandler? outputHandler = null;
        DataReceivedEventHandler? errorHandler = null;
        _ = processWrapper
            .Setup(
                obj => obj.Start(
                    It.IsAny<ProcessStartInfo>(),
                    It.IsAny<DataReceivedEventHandler>(),
                    It.IsAny<DataReceivedEventHandler>()))
            .Callback<ProcessStartInfo, DataReceivedEventHandler, DataReceivedEventHandler>(
                (_, onOut, onErr) =>
                {
                    outputHandler = onOut;
                    errorHandler = onErr;
                });
        var dotNetRunner = new DotNetRunner(logger, processWrapper.Object);

        // Act
        dotNetRunner.Run("build", "/");
        outputHandler!(this, CreateDataReceivedEventArguments("out-line"));
        errorHandler!(this, CreateDataReceivedEventArguments("err-line"));

        // Assert
        _ = logger.Entries
            .Should().HaveCount(2);
        _ = logger.Entries[0].LogLevel
            .Should().Be(Microsoft.Extensions.Logging.LogLevel.Trace);
        _ = logger.Entries[0].Message
            .Should().Be("out-line");
        _ = logger.Entries[1].LogLevel
            .Should().Be(Microsoft.Extensions.Logging.LogLevel.Error);
        _ = logger.Entries[1].Message
            .Should().Be("err-line");
    }

    /// <summary>
    /// Tests that when the output/error handlers fire with <see langword="null"/> data, they log an empty message
    /// rather than throwing.
    /// </summary>
    [AllCulturesFact]
    public void Run_WhenHandlersReceiveNullData_LogsEmptyMessageWithoutThrowing()
    {
        // Arrange
        var logger = new MockLogger<DotNetRunner>();
        var processWrapper = new Mock<IProcessWrapper>();
        DataReceivedEventHandler? outputHandler = null;
        DataReceivedEventHandler? errorHandler = null;
        _ = processWrapper
            .Setup(
                obj => obj.Start(
                    It.IsAny<ProcessStartInfo>(),
                    It.IsAny<DataReceivedEventHandler>(),
                    It.IsAny<DataReceivedEventHandler>()))
            .Callback<ProcessStartInfo, DataReceivedEventHandler, DataReceivedEventHandler>(
                (_, onOut, onErr) =>
                {
                    outputHandler = onOut;
                    errorHandler = onErr;
                });
        var dotNetRunner = new DotNetRunner(logger, processWrapper.Object);

        // Act
        dotNetRunner.Run("build", "/");
        outputHandler!(this, CreateDataReceivedEventArguments(null));
        errorHandler!(this, CreateDataReceivedEventArguments(null));

        // Assert
        _ = logger.Entries
            .Should().HaveCount(2);
        _ = logger.Entries[0].Message
            .Should().BeEmpty();
        _ = logger.Entries[1].Message
            .Should().BeEmpty();
    }

    /// <summary>
    /// Creates a <see cref="DataReceivedEventArgs"/> instance via reflection for the <paramref name="data"/> value.
    /// This is necessary because <see cref="DataReceivedEventArgs"/> has no public constructor.
    /// </summary>
    /// <param name="data">The data payload.</param>
    /// <returns>A <see cref="DataReceivedEventArgs"/> carrying <paramref name="data"/>.</returns>
    private static DataReceivedEventArgs CreateDataReceivedEventArguments(string? data)
    {
        var constructor = typeof(DataReceivedEventArgs).GetConstructor(
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
            binder: null,
            types: [typeof(string)],
            modifiers: null);
        return (DataReceivedEventArgs)constructor!.Invoke([data]);
    }
}
