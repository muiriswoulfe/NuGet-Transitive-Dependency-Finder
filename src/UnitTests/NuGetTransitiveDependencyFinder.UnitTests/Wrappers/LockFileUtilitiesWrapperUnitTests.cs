// <copyright file="LockFileUtilitiesWrapperUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Wrappers;

using System.IO;
using FluentAssertions;
using NuGetTransitiveDependencyFinder.UnitTests.Utilities.Globalization;
using NuGetTransitiveDependencyFinder.UnitTests.Utilities.Logging;
using NuGetTransitiveDependencyFinder.Utilities;
using NuGetTransitiveDependencyFinder.Wrappers;

/// <summary>
/// Unit tests for the <see cref="LockFileUtilitiesWrapper"/> class.
/// </summary>
public class LockFileUtilitiesWrapperUnitTests
{
    /// <summary>
    /// Tests that when <see cref="LockFileUtilitiesWrapper.GetLockFile(string)"/> is called with a path referring to a
    /// file that does not exist, it returns <see langword="null"/>.
    /// </summary>
    [AllCulturesFact]
    public void GetLockFile_WithNonExistentFile_ReturnsNull()
    {
        // Arrange
        var logger = new MockLogger<NuGetLogger>();
        var nuGetLogger = new NuGetLogger(logger);
        var wrapper = new LockFileUtilitiesWrapper(nuGetLogger);
        var missingPath = Path.Combine(
            Path.GetTempPath(),
            $"nuget-tdf-{System.Guid.NewGuid()}-project.assets.json");

        // Act
        var result = wrapper.GetLockFile(missingPath);

        // Assert
        _ = result
            .Should().BeNull();
    }

    /// <summary>
    /// Tests that when <see cref="LockFileUtilitiesWrapper.GetLockFile(string)"/> is called with a path referring to a
    /// minimally valid lock file, a non-null <see cref="NuGet.ProjectModel.LockFile"/> is returned.
    /// </summary>
    [AllCulturesFact]
    public void GetLockFile_WithMinimalLockFile_ReturnsNonNullLockFile()
    {
        // Arrange
        var logger = new MockLogger<NuGetLogger>();
        var nuGetLogger = new NuGetLogger(logger);
        var wrapper = new LockFileUtilitiesWrapper(nuGetLogger);
        var tempPath = Path.Combine(
            Path.GetTempPath(),
            $"nuget-tdf-{System.Guid.NewGuid()}-project.assets.json");
        File.WriteAllText(tempPath, "{\"version\":3}");

        try
        {
            // Act
            var result = wrapper.GetLockFile(tempPath);

            // Assert
            _ = result
                .Should().NotBeNull();
        }
        finally
        {
            File.Delete(tempPath);
        }
    }
}
