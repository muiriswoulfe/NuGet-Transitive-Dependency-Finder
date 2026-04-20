// <copyright file="ProcessWrapperUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Wrappers;

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using FluentAssertions;
using NuGetTransitiveDependencyFinder.UnitTests.Utilities.Globalization;
using NuGetTransitiveDependencyFinder.Wrappers;

/// <summary>
/// Unit tests for the <see cref="ProcessWrapper"/> class.
/// </summary>
public class ProcessWrapperUnitTests
{
    /// <summary>
    /// Tests that when <see cref="ProcessWrapper.Start(ProcessStartInfo, DataReceivedEventHandler,
    /// DataReceivedEventHandler)"/> is called with a valid <c>dotnet --version</c> invocation, it completes
    /// successfully and produces non-empty standard output.
    /// </summary>
    [AllCulturesFact]
    public void Start_WithDotNetVersionCommand_CompletesAndProducesOutput()
    {
        // Arrange
        var wrapper = new ProcessWrapper();
        var stdOut = new StringBuilder();
        var stdErr = new StringBuilder();
        var fileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "dotnet.exe" : "dotnet";
        var startInfo = new ProcessStartInfo(fileName, "--version")
        {
            RedirectStandardError = true,
            RedirectStandardOutput = true,
        };

        // Act
        wrapper.Start(
            startInfo,
            (_, args) =>
            {
                if (args.Data is not null)
                {
                    stdOut.AppendLine(args.Data);
                }
            },
            (_, args) =>
            {
                if (args.Data is not null)
                {
                    stdErr.AppendLine(args.Data);
                }
            });
        wrapper.BeginErrorReadLine();
        wrapper.BeginOutputReadLine();
        wrapper.WaitForExit();

        // Assert
        _ = stdOut.ToString().Trim()
            .Should().NotBeEmpty();
    }

    /// <summary>
    /// Tests that when <see cref="ProcessWrapper.BeginErrorReadLine"/> is called before
    /// <see cref="ProcessWrapper.Start(ProcessStartInfo, DataReceivedEventHandler, DataReceivedEventHandler)"/>, it
    /// throws a <see cref="NullReferenceException"/>.
    /// </summary>
    [AllCulturesFact]
    public void BeginErrorReadLine_CalledBeforeStart_ThrowsNullReferenceException()
    {
        // Arrange
        var wrapper = new ProcessWrapper();

        // Act
        Action action = wrapper.BeginErrorReadLine;

        // Assert
        _ = action
            .Should().Throw<NullReferenceException>();
    }

    /// <summary>
    /// Tests that when <see cref="ProcessWrapper.BeginOutputReadLine"/> is called before
    /// <see cref="ProcessWrapper.Start(ProcessStartInfo, DataReceivedEventHandler, DataReceivedEventHandler)"/>, it
    /// throws a <see cref="NullReferenceException"/>.
    /// </summary>
    [AllCulturesFact]
    public void BeginOutputReadLine_CalledBeforeStart_ThrowsNullReferenceException()
    {
        // Arrange
        var wrapper = new ProcessWrapper();

        // Act
        Action action = wrapper.BeginOutputReadLine;

        // Assert
        _ = action
            .Should().Throw<NullReferenceException>();
    }

    /// <summary>
    /// Tests that when <see cref="ProcessWrapper.WaitForExit"/> is called before
    /// <see cref="ProcessWrapper.Start(ProcessStartInfo, DataReceivedEventHandler, DataReceivedEventHandler)"/>, it
    /// throws a <see cref="NullReferenceException"/>.
    /// </summary>
    [AllCulturesFact]
    public void WaitForExit_CalledBeforeStart_ThrowsNullReferenceException()
    {
        // Arrange
        var wrapper = new ProcessWrapper();

        // Act
        Action action = wrapper.WaitForExit;

        // Assert
        _ = action
            .Should().Throw<NullReferenceException>();
    }
}
