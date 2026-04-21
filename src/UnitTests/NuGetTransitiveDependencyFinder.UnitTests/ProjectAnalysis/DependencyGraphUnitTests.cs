// <copyright file="DependencyGraphUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.ProjectAnalysis;

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using FluentAssertions;
using Moq;
using NuGet.ProjectModel;
using NuGetTransitiveDependencyFinder.ProjectAnalysis;
using NuGetTransitiveDependencyFinder.UnitTests.Utilities.Globalization;
using NuGetTransitiveDependencyFinder.Wrappers;
using Xunit;

/// <summary>
/// Unit tests for the <see cref="DependencyGraph"/> class.
/// </summary>
public partial class DependencyGraphUnitTests
{
    /// <summary>
    /// Tests that when <see cref="DependencyGraph.Create(string)"/> is called and the MSBuild probe produces no
    /// output (signalling that msbuild is unavailable), the <c>dotnet build</c> command is invoked.
    /// </summary>
    [AllCulturesFact]
    public void Create_WhenMSBuildIsUnavailable_InvokesDotNetBuild()
    {
        // Arrange
        var dotNetRunnerMock = new Mock<IDotNetRunner>();
        var processWrapperMock = new Mock<IProcessWrapper>();
        string? capturedArguments = null;
        _ = dotNetRunnerMock
            .Setup(mock => mock.Run(It.IsAny<string>(), It.IsAny<string>()))
            .Callback<string, string>((arguments, _) =>
            {
                capturedArguments = arguments;
                WriteMinimalDependencyGraph(arguments);
            });
        using var dependencyGraph = new DependencyGraph(dotNetRunnerMock.Object, processWrapperMock.Object);

        // Act
        var result = dependencyGraph.Create("/some/project.csproj");

        // Assert
        _ = result
            .Should().NotBeNull();
        _ = capturedArguments
            .Should().StartWith("dotnet build \"/some/project.csproj\"");
        _ = capturedArguments
            .Should().Contain("/target:GenerateRestoreGraphFile");
    }

    /// <summary>
    /// Tests that when <see cref="DependencyGraph.Create(string)"/> is called and the MSBuild probe produces
    /// standard-output text (signalling that msbuild is available), the <c>msbuild</c> command is invoked.
    /// </summary>
    [AllCulturesFact]
    public void Create_WhenMSBuildIsAvailable_InvokesMSBuild()
    {
        // Arrange
        var dotNetRunnerMock = new Mock<IDotNetRunner>();
        var processWrapperMock = new Mock<IProcessWrapper>();
        _ = processWrapperMock
            .Setup(mock => mock.Start(
                It.IsAny<ProcessStartInfo>(),
                It.IsAny<DataReceivedEventHandler>(),
                It.IsAny<DataReceivedEventHandler>()))
            .Callback<ProcessStartInfo, DataReceivedEventHandler, DataReceivedEventHandler>(
                (_, onOutput, _) => onOutput(this, CreateDataReceivedEventArguments("/usr/bin/msbuild")));
        string? capturedArguments = null;
        _ = dotNetRunnerMock
            .Setup(mock => mock.Run(It.IsAny<string>(), It.IsAny<string>()))
            .Callback<string, string>((arguments, _) =>
            {
                capturedArguments = arguments;
                WriteMinimalDependencyGraph(arguments);
            });
        using var dependencyGraph = new DependencyGraph(dotNetRunnerMock.Object, processWrapperMock.Object);

        // Act
        _ = dependencyGraph.Create("/some/project.csproj");

        // Assert
        _ = capturedArguments
            .Should().StartWith("msbuild \"/some/project.csproj\"");
    }

    /// <summary>
    /// Tests that when <see cref="DependencyGraph.Create(string)"/> is called, the working directory passed to the
    /// <see cref="IDotNetRunner"/> is the directory component of the supplied project or solution path.
    /// </summary>
    [AllCulturesFact]
    public void Create_WhenCalled_UsesProjectDirectoryAsWorkingDirectory()
    {
        // Arrange
        var dotNetRunnerMock = new Mock<IDotNetRunner>();
        var processWrapperMock = new Mock<IProcessWrapper>();
        string? capturedWorkingDirectory = null;
        _ = dotNetRunnerMock
            .Setup(mock => mock.Run(It.IsAny<string>(), It.IsAny<string>()))
            .Callback<string, string>((arguments, workingDirectory) =>
            {
                capturedWorkingDirectory = workingDirectory;
                WriteMinimalDependencyGraph(arguments);
            });
        using var dependencyGraph = new DependencyGraph(dotNetRunnerMock.Object, processWrapperMock.Object);

        // Act
        _ = dependencyGraph.Create(Path.Combine("some", "nested", "project.csproj"));

        // Assert
        _ = capturedWorkingDirectory
            .Should().Be(Path.Combine("some", "nested"));
    }

    /// <summary>
    /// Tests that when <see cref="DependencyGraph.Create(string)"/> is called, the MSBuild probe is started with a
    /// <see cref="ProcessStartInfo"/> configured to invoke <c>where</c> or <c>which</c> depending on the platform,
    /// with redirected streams and <see cref="ProcessStartInfo.UseShellExecute"/> set to <see langword="false"/>.
    /// </summary>
    [AllCulturesFact]
    public void Create_WhenCalled_ConfiguresMSBuildProbeCorrectly()
    {
        // Arrange
        var dotNetRunnerMock = new Mock<IDotNetRunner>();
        var processWrapperMock = new Mock<IProcessWrapper>();
        ProcessStartInfo? capturedStartInfo = null;
        _ = processWrapperMock
            .Setup(mock => mock.Start(
                It.IsAny<ProcessStartInfo>(),
                It.IsAny<DataReceivedEventHandler>(),
                It.IsAny<DataReceivedEventHandler>()))
            .Callback<ProcessStartInfo, DataReceivedEventHandler, DataReceivedEventHandler>(
                (startInfo, _, _) => capturedStartInfo = startInfo);
        _ = dotNetRunnerMock
            .Setup(mock => mock.Run(It.IsAny<string>(), It.IsAny<string>()))
            .Callback<string, string>((arguments, _) => WriteMinimalDependencyGraph(arguments));
        using var dependencyGraph = new DependencyGraph(dotNetRunnerMock.Object, processWrapperMock.Object);

        // Act
        _ = dependencyGraph.Create("/some/project.csproj");

        // Assert
        _ = capturedStartInfo
            .Should().NotBeNull();
        var expectedFileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "where" : "which";
        _ = capturedStartInfo!.FileName
            .Should().Be(expectedFileName);
        _ = capturedStartInfo.Arguments
            .Should().Be("msbuild");
        _ = capturedStartInfo.RedirectStandardError
            .Should().BeTrue();
        _ = capturedStartInfo.RedirectStandardOutput
            .Should().BeTrue();
        _ = capturedStartInfo.UseShellExecute
            .Should().BeFalse();
    }

    /// <summary>
    /// Tests that when <see cref="DependencyGraph.Create(string)"/> is called, the MSBuild probe invokes
    /// <see cref="IProcessWrapper.BeginErrorReadLine"/>, <see cref="IProcessWrapper.BeginOutputReadLine"/>, and
    /// <see cref="IProcessWrapper.WaitForExit"/> exactly once.
    /// </summary>
    [AllCulturesFact]
    public void Create_WhenCalled_WaitsForMSBuildProbeToExit()
    {
        // Arrange
        var dotNetRunnerMock = new Mock<IDotNetRunner>();
        var processWrapperMock = new Mock<IProcessWrapper>();
        _ = dotNetRunnerMock
            .Setup(mock => mock.Run(It.IsAny<string>(), It.IsAny<string>()))
            .Callback<string, string>((arguments, _) => WriteMinimalDependencyGraph(arguments));
        using var dependencyGraph = new DependencyGraph(dotNetRunnerMock.Object, processWrapperMock.Object);

        // Act
        _ = dependencyGraph.Create("/some/project.csproj");

        // Assert
        processWrapperMock.Verify(mock => mock.BeginErrorReadLine(), Times.Once());
        processWrapperMock.Verify(mock => mock.BeginOutputReadLine(), Times.Once());
        processWrapperMock.Verify(mock => mock.WaitForExit(), Times.Once());
    }

    /// <summary>
    /// Tests that when the standard-error handler registered during the MSBuild probe receives a non-empty payload,
    /// it throws an <see cref="InvalidOperationException"/>.
    /// </summary>
    [AllCulturesFact]
    public void Create_WhenStandardErrorHandlerReceivesData_ThrowsInvalidOperationException()
    {
        // Arrange
        var dotNetRunnerMock = new Mock<IDotNetRunner>();
        var processWrapperMock = new Mock<IProcessWrapper>();
        DataReceivedEventHandler? capturedErrorHandler = null;
        _ = processWrapperMock
            .Setup(mock => mock.Start(
                It.IsAny<ProcessStartInfo>(),
                It.IsAny<DataReceivedEventHandler>(),
                It.IsAny<DataReceivedEventHandler>()))
            .Callback<ProcessStartInfo, DataReceivedEventHandler, DataReceivedEventHandler>(
                (_, _, onError) => capturedErrorHandler = onError);
        _ = dotNetRunnerMock
            .Setup(mock => mock.Run(It.IsAny<string>(), It.IsAny<string>()))
            .Callback<string, string>((arguments, _) => WriteMinimalDependencyGraph(arguments));
        using var dependencyGraph = new DependencyGraph(dotNetRunnerMock.Object, processWrapperMock.Object);
        _ = dependencyGraph.Create("/some/project.csproj");

        // Act
        Action action = () => capturedErrorHandler!(this, CreateDataReceivedEventArguments("build error"));

        // Assert
        _ = action
            .Should().Throw<InvalidOperationException>()
            .WithMessage("build error");
    }

    /// <summary>
    /// Tests that when the standard-error handler registered during the MSBuild probe receives a
    /// <see langword="null"/> or whitespace-only payload, it does not throw.
    /// </summary>
    /// <param name="data">The payload to pass to the error handler.</param>
    [AllCulturesTheory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WhenStandardErrorHandlerReceivesEmptyData_DoesNotThrow(string? data)
    {
        // Arrange
        var dotNetRunnerMock = new Mock<IDotNetRunner>();
        var processWrapperMock = new Mock<IProcessWrapper>();
        DataReceivedEventHandler? capturedErrorHandler = null;
        _ = processWrapperMock
            .Setup(mock => mock.Start(
                It.IsAny<ProcessStartInfo>(),
                It.IsAny<DataReceivedEventHandler>(),
                It.IsAny<DataReceivedEventHandler>()))
            .Callback<ProcessStartInfo, DataReceivedEventHandler, DataReceivedEventHandler>(
                (_, _, onError) => capturedErrorHandler = onError);
        _ = dotNetRunnerMock
            .Setup(mock => mock.Run(It.IsAny<string>(), It.IsAny<string>()))
            .Callback<string, string>((arguments, _) => WriteMinimalDependencyGraph(arguments));
        using var dependencyGraph = new DependencyGraph(dotNetRunnerMock.Object, processWrapperMock.Object);
        _ = dependencyGraph.Create("/some/project.csproj");

        // Act
        Action action = () => capturedErrorHandler!(this, CreateDataReceivedEventArguments(data));

        // Assert
        _ = action
            .Should().NotThrow();
    }

    /// <summary>
    /// Tests that when the standard-output handler registered during the MSBuild probe receives a whitespace-only
    /// payload, MSBuild is treated as unavailable (the <c>dotnet build</c> command is invoked).
    /// </summary>
    [AllCulturesFact]
    public void Create_WhenStandardOutputReceivesWhiteSpace_TreatsMSBuildAsUnavailable()
    {
        // Arrange
        var dotNetRunnerMock = new Mock<IDotNetRunner>();
        var processWrapperMock = new Mock<IProcessWrapper>();
        _ = processWrapperMock
            .Setup(mock => mock.Start(
                It.IsAny<ProcessStartInfo>(),
                It.IsAny<DataReceivedEventHandler>(),
                It.IsAny<DataReceivedEventHandler>()))
            .Callback<ProcessStartInfo, DataReceivedEventHandler, DataReceivedEventHandler>(
                (_, onOutput, _) => onOutput(this, CreateDataReceivedEventArguments("   ")));
        string? capturedArguments = null;
        _ = dotNetRunnerMock
            .Setup(mock => mock.Run(It.IsAny<string>(), It.IsAny<string>()))
            .Callback<string, string>((arguments, _) =>
            {
                capturedArguments = arguments;
                WriteMinimalDependencyGraph(arguments);
            });
        using var dependencyGraph = new DependencyGraph(dotNetRunnerMock.Object, processWrapperMock.Object);

        // Act
        _ = dependencyGraph.Create("/some/project.csproj");

        // Assert
        _ = capturedArguments
            .Should().StartWith("dotnet build");
    }

    /// <summary>
    /// Tests that the MSBuild availability check is reset between sequential <see cref="DependencyGraph.Create(string)"/>
    /// invocations: if the first probe succeeds (MSBuild available) and the second emits no output (MSBuild
    /// unavailable), the second call must invoke <c>dotnet build</c>. This guards against regressions where the
    /// internal probe-output buffer is not reset between calls, causing the second probe to incorrectly report MSBuild
    /// as available based on stale state.
    /// </summary>
    [AllCulturesFact]
    public void Create_SecondCallAfterMSBuildWasAvailable_ResetsProbeStateWhenNoLongerAvailable()
    {
        // Arrange
        var dotNetRunnerMock = new Mock<IDotNetRunner>();
        var processWrapperMock = new Mock<IProcessWrapper>();
        var probeInvocationCount = 0;
        _ = processWrapperMock
            .Setup(mock => mock.Start(
                It.IsAny<ProcessStartInfo>(),
                It.IsAny<DataReceivedEventHandler>(),
                It.IsAny<DataReceivedEventHandler>()))
            .Callback<ProcessStartInfo, DataReceivedEventHandler, DataReceivedEventHandler>(
                (_, onOutput, _) =>
                {
                    probeInvocationCount++;
                    if (probeInvocationCount == 1)
                    {
                        onOutput(this, CreateDataReceivedEventArguments("/usr/bin/msbuild"));
                    }
                });
        var capturedArguments = new System.Collections.Generic.List<string>();
        _ = dotNetRunnerMock
            .Setup(mock => mock.Run(It.IsAny<string>(), It.IsAny<string>()))
            .Callback<string, string>((arguments, _) =>
            {
                capturedArguments.Add(arguments);
                WriteMinimalDependencyGraph(arguments);
            });
        using var dependencyGraph = new DependencyGraph(dotNetRunnerMock.Object, processWrapperMock.Object);

        // Act
        _ = dependencyGraph.Create("/some/project.csproj");
        _ = dependencyGraph.Create("/some/project.csproj");

        // Assert
        _ = capturedArguments
            .Should().HaveCount(2);
        _ = capturedArguments[0]
            .Should().StartWith("msbuild ");
        _ = capturedArguments[1]
            .Should().StartWith("dotnet build ");
    }

    /// <summary>
    /// Tests that when <see cref="DependencyGraph.Dispose()"/> is called before <see cref="DependencyGraph.Create"/>,
    /// it does not throw because the temporary file has not yet been created.
    /// </summary>
    [AllCulturesFact]
    public void Dispose_CalledBeforeCreate_DoesNotThrow()
    {
        // Arrange
        var dependencyGraph = new DependencyGraph(
            new Mock<IDotNetRunner>().Object,
            new Mock<IProcessWrapper>().Object);

        // Act
        Action action = dependencyGraph.Dispose;

        // Assert
        _ = action
            .Should().NotThrow();
    }

    /// <summary>
    /// Tests that <see cref="DependencyGraph.Dispose()"/> is idempotent — calling it twice does not throw.
    /// </summary>
    [AllCulturesFact]
    public void Dispose_CalledTwice_DoesNotThrow()
    {
        // Arrange
        var dependencyGraph = new DependencyGraph(
            new Mock<IDotNetRunner>().Object,
            new Mock<IProcessWrapper>().Object);
        dependencyGraph.Dispose();

        // Act
        Action action = dependencyGraph.Dispose;

        // Assert
        _ = action
            .Should().NotThrow();
    }

    /// <summary>
    /// Tests that when <see cref="DependencyGraph.Create(string)"/> succeeds and <see cref="DependencyGraph.Dispose"/>
    /// is subsequently invoked, the temporary file created for the dependency graph is deleted.
    /// </summary>
    [AllCulturesFact]
    public void Dispose_AfterCreate_DeletesTemporaryFile()
    {
        // Arrange
        var dotNetRunnerMock = new Mock<IDotNetRunner>();
        var processWrapperMock = new Mock<IProcessWrapper>();
        string? capturedFilePath = null;
        _ = dotNetRunnerMock
            .Setup(mock => mock.Run(It.IsAny<string>(), It.IsAny<string>()))
            .Callback<string, string>((arguments, _) =>
                capturedFilePath = WriteMinimalDependencyGraph(arguments));
        var dependencyGraph = new DependencyGraph(dotNetRunnerMock.Object, processWrapperMock.Object);
        _ = dependencyGraph.Create("/some/project.csproj");
        File.Exists(capturedFilePath!).Should().BeTrue();

        // Act
        dependencyGraph.Dispose();

        // Assert
        _ = File.Exists(capturedFilePath!)
            .Should().BeFalse();
    }

    /// <summary>
    /// Tests that when <see cref="DependencyGraph"/> instances are constructed, they each use a distinct temporary
    /// file path. This guards against regressions where the temporary file path is made static or otherwise shared,
    /// which would cause concurrent or sequential graph instances to corrupt each other's output.
    /// </summary>
    [AllCulturesFact]
    public void Create_AcrossInstances_UsesDistinctTemporaryFilePaths()
    {
        // Arrange
        static DependencyGraph CreateInstance(out System.Collections.Generic.List<string> capturedArgumentsBuffer)
        {
            var capturedArguments = new System.Collections.Generic.List<string>();
            capturedArgumentsBuffer = capturedArguments;
            var dotNetRunnerMock = new Mock<IDotNetRunner>();
            var processWrapperMock = new Mock<IProcessWrapper>();
            _ = dotNetRunnerMock
                .Setup(mock => mock.Run(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((arguments, _) =>
                {
                    capturedArguments.Add(arguments);
                    WriteMinimalDependencyGraph(arguments);
                });
            return new DependencyGraph(dotNetRunnerMock.Object, processWrapperMock.Object);
        }

        using var first = CreateInstance(out var firstArguments);
        using var second = CreateInstance(out var secondArguments);

        // Act
        _ = first.Create("/some/project.csproj");
        _ = second.Create("/some/project.csproj");

        // Assert
        var firstPath = ExtractRestoreGraphOutputPath(firstArguments[0]);
        var secondPath = ExtractRestoreGraphOutputPath(secondArguments[0]);
        _ = firstPath
            .Should().NotBeNullOrWhiteSpace();
        _ = secondPath
            .Should().NotBe(firstPath);
    }

    /// <summary>
    /// Extracts the value of <c>/property:RestoreGraphOutputPath=</c> from a captured build-command argument string.
    /// </summary>
    /// <param name="arguments">The captured build-command argument string.</param>
    /// <returns>The restore-graph output path.</returns>
    private static string ExtractRestoreGraphOutputPath(string arguments)
    {
        const string marker = "/property:RestoreGraphOutputPath=\"";
        var index = arguments.IndexOf(marker, StringComparison.Ordinal);
        if (index < 0)
        {
            return string.Empty;
        }

        var start = index + marker.Length;
        var end = arguments.IndexOf('"', start);
        return end < 0 ? string.Empty : arguments[start..end];
    }

    /// <summary>
    /// Tests that <see cref="DependencyGraph"/> implements <see cref="IDependencyGraph"/> and
    /// <see cref="IDisposable"/>.
    /// </summary>
    [AllCulturesFact]
    public void Type_ImplementsExpectedInterfaces() =>
        _ = typeof(DependencyGraph)
            .Should().Implement<IDependencyGraph>()
            .And.Implement<IDisposable>();

    /// <summary>
    /// Creates a <see cref="DataReceivedEventArgs"/> instance via reflection for the <paramref name="data"/> value.
    /// This is necessary because <see cref="DataReceivedEventArgs"/> has no public constructor.
    /// </summary>
    /// <param name="data">The data payload.</param>
    /// <returns>A <see cref="DataReceivedEventArgs"/> carrying <paramref name="data"/>.</returns>
    private static DataReceivedEventArgs CreateDataReceivedEventArguments(string? data)
    {
        var constructor = typeof(DataReceivedEventArgs).GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic,
            binder: null,
            types: [typeof(string)],
            modifiers: null);
        return (DataReceivedEventArgs)constructor!.Invoke([data]);
    }

    /// <summary>
    /// Extracts the <c>RestoreGraphOutputPath</c> value from an MSBuild arguments string and writes a minimal valid
    /// <see cref="DependencyGraphSpec"/> file there so that <see cref="DependencyGraphSpec.Load(string)"/> succeeds.
    /// </summary>
    /// <param name="arguments">The arguments passed to the <see cref="IDotNetRunner"/>.</param>
    /// <returns>The path at which the minimal dependency graph was written.</returns>
    private static string WriteMinimalDependencyGraph(string arguments)
    {
        var match = RestoreGraphOutputPathRegex().Match(arguments);
        match.Success.Should().BeTrue();
        var path = match.Groups[1].Value;
        new DependencyGraphSpec().Save(path);
        return path;
    }

    /// <summary>
    /// Gets a regular expression that matches the <c>RestoreGraphOutputPath</c> value embedded in an MSBuild
    /// arguments string.
    /// </summary>
    [GeneratedRegex("RestoreGraphOutputPath=\"([^\"]+)\"")]
    private static partial Regex RestoreGraphOutputPathRegex();
}
