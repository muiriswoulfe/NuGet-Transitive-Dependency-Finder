// <copyright file="DotNetRunnerUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.ProjectAnalysis;

using System.Diagnostics;
using FluentAssertions;
using Moq;
using NuGetTransitiveDependencyFinder.ProjectAnalysis;
using NuGetTransitiveDependencyFinder.TestUtilities.Globalization;
using NuGetTransitiveDependencyFinder.TestUtilities.Logging;
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
}
