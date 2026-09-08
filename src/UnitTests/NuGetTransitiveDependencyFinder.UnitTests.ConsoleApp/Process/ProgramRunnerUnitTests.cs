// <copyright file="ProgramRunnerUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.ConsoleApp.Process;

using System;
using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NuGetTransitiveDependencyFinder.ConsoleApp.Input;
using NuGetTransitiveDependencyFinder.ConsoleApp.Output;
using NuGetTransitiveDependencyFinder.ConsoleApp.Process;
using NuGetTransitiveDependencyFinder.ConsoleApp.Resources.Messages;
using NuGetTransitiveDependencyFinder.Output;
using NuGetTransitiveDependencyFinder.UnitTests.ConsoleApp.TestUtilities;
using NuGetTransitiveDependencyFinder.UnitTests.Utilities.Globalization;
using NuGetTransitiveDependencyFinder.UnitTests.Utilities.Logging;

/// <summary>
/// Unit tests for the <see cref="ProgramRunner"/> class.
/// </summary>
public partial class ProgramRunnerUnitTests
{
    /// <summary>
    /// The mock <see cref="ICommandLineOptions"/> object.
    /// </summary>
    private readonly Mock<ICommandLineOptions> commandLineOptions = new();

    /// <summary>
    /// The mock <see cref="IDependencyWriter"/> object.
    /// </summary>
    private readonly Mock<IDependencyWriter> dependencyWriter = new();

    /// <summary>
    /// The mock <see cref="ILogger{ProgramRunner}"/> object.
    /// </summary>
    private readonly MockLogger<ProgramRunner> logger = new();

    /// <summary>
    /// The mock <see cref="ITransitiveDependencyFinder"/> object.
    /// </summary>
    private readonly Mock<ITransitiveDependencyFinder> transitiveDependencyFinder = new();

    /// <summary>
    /// Gets the filter regular expression for use within the unit tests.
    /// </summary>
    [GeneratedRegex("Filter")]
    private static partial Regex FilterRegex { get; }

    /// <summary>
    /// Tests that when <see cref="ProgramRunner.Run()"/> is called, it performs the expected actions.
    /// </summary>
    [AllCulturesFact]
    public void Run_Called_PerformsExpectedActions()
    {
        // Arrange
        var projects = InternalAccessor.Construct<Projects>(0);
        _ = this.commandLineOptions
            .SetupGet(mock => mock.ProjectOrSolution)
            .Returns("ProjectOrSolution");
        _ = this.commandLineOptions
            .SetupGet(mock => mock.All)
            .Returns(true);
        _ = this.commandLineOptions
            .SetupGet(mock => mock.Filter)
            .Returns(FilterRegex);
        _ = this.transitiveDependencyFinder
            .Setup(mock => mock.Run("ProjectOrSolution", true, FilterRegex))
            .Returns(projects);
        var programRunner = new ProgramRunner(
            this.commandLineOptions.Object,
            this.dependencyWriter.Object,
            this.logger,
            this.transitiveDependencyFinder.Object);

        // Act
        programRunner.Run();

        // Arrange
        _ = this.logger.Entries
            .Should().HaveCount(1);
        _ = this.logger.Entries[0].LogLevel
            .Should().Be(LogLevel.Information);
        _ = this.logger.Entries[0].Message
            .Should().Be(Information.CommencingAnalysis);
        this.commandLineOptions.VerifyGet(mock => mock.ProjectOrSolution, Times.Once);
        this.commandLineOptions.VerifyGet(mock => mock.All, Times.Once);
        this.commandLineOptions.VerifyGet(mock => mock.Filter, Times.Once);
        this.commandLineOptions.VerifyNoOtherCalls();
        this.transitiveDependencyFinder.Verify(mock => mock.Run("ProjectOrSolution", true, FilterRegex), Times.Once);
        this.transitiveDependencyFinder.VerifyNoOtherCalls();
        this.dependencyWriter.Verify(mock => mock.Write(projects), Times.Once);
        this.dependencyWriter.VerifyNoOtherCalls();
    }

    /// <summary>
    /// Tests that when <see cref="ProgramRunner.Run()"/> is called with a <see langword="null"/> filter and
    /// <see cref="CommandLineOptions.All"/> set to <see langword="false"/>, the correct arguments are passed through.
    /// </summary>
    [AllCulturesFact]
    public void Run_WithNullFilterAndAllFalse_PropagatesArguments()
    {
        // Arrange
        var projects = InternalAccessor.Construct<Projects>(0);
        _ = this.commandLineOptions
            .SetupGet(mock => mock.ProjectOrSolution)
            .Returns("Project.csproj");
        _ = this.commandLineOptions
            .SetupGet(mock => mock.All)
            .Returns(false);
        _ = this.commandLineOptions
            .SetupGet(mock => mock.Filter)
            .Returns((Regex?)null);
        _ = this.transitiveDependencyFinder
            .Setup(mock => mock.Run("Project.csproj", false, null))
            .Returns(projects);
        var programRunner = new ProgramRunner(
            this.commandLineOptions.Object,
            this.dependencyWriter.Object,
            this.logger,
            this.transitiveDependencyFinder.Object);

        // Act
        programRunner.Run();

        // Assert
        this.transitiveDependencyFinder.Verify(mock => mock.Run("Project.csproj", false, null), Times.Once);
        this.dependencyWriter.Verify(mock => mock.Write(projects), Times.Once);
    }

    /// <summary>
    /// Tests that when <see cref="ProgramRunner.Run()"/> is called and
    /// <see cref="ITransitiveDependencyFinder.Run(string?, bool, Regex?)"/> throws an exception, the exception
    /// propagates and the dependency writer is not invoked.
    /// </summary>
    [AllCulturesFact]
    public void Run_WhenFinderThrows_PropagatesExceptionAndDoesNotWrite()
    {
        // Arrange
        _ = this.commandLineOptions
            .SetupGet(mock => mock.ProjectOrSolution)
            .Returns("Project.csproj");
        _ = this.transitiveDependencyFinder
            .Setup(mock => mock.Run(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Regex?>()))
            .Throws(new InvalidOperationException("boom"));
        var programRunner = new ProgramRunner(
            this.commandLineOptions.Object,
            this.dependencyWriter.Object,
            this.logger,
            this.transitiveDependencyFinder.Object);

        // Act
        Action action = programRunner.Run;

        // Assert
        _ = action
            .Should().Throw<InvalidOperationException>()
            .WithMessage("boom");
        this.dependencyWriter.Verify(mock => mock.Write(It.IsAny<Projects>()), Times.Never);
    }

    /// <summary>
    /// Tests that when <see cref="ProgramRunner.Run()"/> is called, the <c>CommencingAnalysis</c> log message is
    /// emitted <em>before</em> <see cref="ITransitiveDependencyFinder.Run(string?, bool, Regex?)"/> is invoked.
    /// </summary>
    [AllCulturesFact]
    public void Run_WhenCalled_EmitsCommencingAnalysisBeforeInvokingFinder()
    {
        // Arrange
        var projects = InternalAccessor.Construct<Projects>(0);
        var wasLoggedBeforeFinder = false;
        _ = this.commandLineOptions
            .SetupGet(mock => mock.ProjectOrSolution)
            .Returns("Project.csproj");
        _ = this.transitiveDependencyFinder
            .Setup(mock => mock.Run(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Regex?>()))
            .Callback(() => wasLoggedBeforeFinder = this.logger.Entries.Count == 1)
            .Returns(projects);
        var programRunner = new ProgramRunner(
            this.commandLineOptions.Object,
            this.dependencyWriter.Object,
            this.logger,
            this.transitiveDependencyFinder.Object);

        // Act
        programRunner.Run();

        // Assert
        _ = wasLoggedBeforeFinder
            .Should().BeTrue();
    }
}
