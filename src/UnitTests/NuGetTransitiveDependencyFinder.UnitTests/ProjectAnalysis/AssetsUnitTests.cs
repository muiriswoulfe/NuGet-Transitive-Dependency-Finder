// <copyright file="AssetsUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.ProjectAnalysis;

using System.Globalization;
using System.IO;
using FluentAssertions;
using Moq;
using NuGet.ProjectModel;
using NuGetTransitiveDependencyFinder.ProjectAnalysis;
using NuGetTransitiveDependencyFinder.UnitTests.Utilities.Globalization;
using NuGetTransitiveDependencyFinder.Wrappers;
using static System.FormattableString;

/// <summary>
/// Unit tests for the <see cref="Assets"/> class.
/// </summary>
public class AssetsUnitTests
{
    /// <summary>
    /// Tests that when <see cref="Assets.Create(string, string)"/> is called, it performs the expected operations.
    /// </summary>
    [AllCulturesFact]
    public void Create_WithDifferentValues_ReturnsCorrectValues()
    {
        // Arrange
        var dotNetRunner = new Mock<IDotNetRunner>();
        var lockFileUtilitiesWrapper = new Mock<ILockFileUtilitiesWrapper>();
        var lockFile = new LockFile();
        _ = lockFileUtilitiesWrapper.Setup(mock => mock.GetLockFile(It.IsAny<string>())).Returns(lockFile);
        var assets = new Assets(dotNetRunner.Object, lockFileUtilitiesWrapper.Object);
        var directorySeparator = Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);
        var inputDirectory = Path.Join(directorySeparator, "input");
        var outputDirectory = Path.Join(directorySeparator, "output");

        // Act
        var result = assets.Create(inputDirectory, outputDirectory);

        // Assert
        _ = result
            .Should().Be(lockFile);
        dotNetRunner
            .Verify(
                mock => mock.Run(Invariant($@"restore ""{inputDirectory}"""), directorySeparator),
                Times.Once);
        var lockFilePath = Path.Join(outputDirectory, "project.assets.json");
        lockFileUtilitiesWrapper.Verify(mock => mock.GetLockFile(lockFilePath), Times.Once);
    }

    /// <summary>
    /// Tests that when <see cref="Assets.Create(string, string)"/> is called with a project path whose directory
    /// cannot be parsed, the wrapper is still invoked with a sensible output path.
    /// </summary>
    [AllCulturesFact]
    public void Create_WithNestedProjectPath_ForwardsCorrectWorkingDirectory()
    {
        // Arrange
        var dotNetRunner = new Mock<IDotNetRunner>();
        var lockFileUtilitiesWrapper = new Mock<ILockFileUtilitiesWrapper>();
        _ = lockFileUtilitiesWrapper
            .Setup(mock => mock.GetLockFile(It.IsAny<string>()))
            .Returns(new LockFile());
        var assets = new Assets(dotNetRunner.Object, lockFileUtilitiesWrapper.Object);
        var projectPath = Path.Combine("repo", "src", "Sample", "Sample.csproj");
        var expectedWorkingDirectory = Path.GetDirectoryName(projectPath)!;
        var outputDirectory = Path.Combine("out", "sample");

        // Act
        var result = assets.Create(projectPath, outputDirectory);

        // Assert
        _ = result
            .Should().NotBeNull();
        dotNetRunner.Verify(
            mock => mock.Run(Invariant($@"restore ""{projectPath}"""), expectedWorkingDirectory),
            Times.Once);
        lockFileUtilitiesWrapper.Verify(
            mock => mock.GetLockFile(Path.Combine(outputDirectory, "project.assets.json")),
            Times.Once);
    }

    /// <summary>
    /// Tests that when <see cref="Assets.Create(string, string)"/> is called and the underlying wrapper returns
    /// <see langword="null"/>, the method propagates <see langword="null"/> unchanged.
    /// </summary>
    [AllCulturesFact]
    public void Create_WhenWrapperReturnsNull_PropagatesNull()
    {
        // Arrange
        var dotNetRunner = new Mock<IDotNetRunner>();
        var lockFileUtilitiesWrapper = new Mock<ILockFileUtilitiesWrapper>();
        _ = lockFileUtilitiesWrapper
            .Setup(mock => mock.GetLockFile(It.IsAny<string>()))
            .Returns((LockFile)null!);
        var assets = new Assets(dotNetRunner.Object, lockFileUtilitiesWrapper.Object);

        // Act
        var result = assets.Create(
            Path.Combine(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture), "p.csproj"),
            Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture));

        // Assert
        _ = result
            .Should().BeNull();
    }

    /// <summary>
    /// Tests that <see cref="Assets.Create(string, string)"/> calls the dotnet runner exactly once per invocation.
    /// </summary>
    [AllCulturesFact]
    public void Create_WhenInvokedOnce_CallsDotNetRunnerExactlyOnce()
    {
        // Arrange
        var dotNetRunner = new Mock<IDotNetRunner>();
        var lockFileUtilitiesWrapper = new Mock<ILockFileUtilitiesWrapper>();
        _ = lockFileUtilitiesWrapper
            .Setup(mock => mock.GetLockFile(It.IsAny<string>()))
            .Returns(new LockFile());
        var assets = new Assets(dotNetRunner.Object, lockFileUtilitiesWrapper.Object);
        var sep = Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);

        // Act
        _ = assets.Create(Path.Combine(sep, "in.csproj"), sep);

        // Assert
        dotNetRunner.Verify(
            mock => mock.Run(It.IsAny<string>(), It.IsAny<string>()),
            Times.Once);
        lockFileUtilitiesWrapper.Verify(
            mock => mock.GetLockFile(It.IsAny<string>()),
            Times.Once);
    }
}
