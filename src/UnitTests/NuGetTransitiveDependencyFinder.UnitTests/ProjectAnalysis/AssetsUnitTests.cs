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
}
