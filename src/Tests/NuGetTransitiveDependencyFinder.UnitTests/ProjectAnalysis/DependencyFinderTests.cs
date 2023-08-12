// <copyright file="DependencyFinderTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.ProjectAnalysis;

using System.Collections.Generic;
using System.Text.RegularExpressions;
using FluentAssertions;
using Moq;
using NuGet.Frameworks;
using NuGet.ProjectModel;
using NuGet.Versioning;
using NuGetTransitiveDependencyFinder.ProjectAnalysis;
using NuGetTransitiveDependencyFinder.TestUtilities.Globalization;

/// <summary>
/// Unit tests for the <see cref="DependencyFinder"/> class.
/// </summary>
public partial class DependencyFinderTests
{
    /// <summary>
    /// The mock object for <see cref="IAssets"/>.
    /// </summary>
    private readonly Mock<IAssets> assetsMock;

    /// <summary>
    /// The mock object for <see cref="IDependencyGraph"/>.
    /// </summary>
    private readonly Mock<IDependencyGraph> dependencyGraphMock;

    /// <summary>
    /// The <see cref="DependencyFinder"/> test object.
    /// </summary>
    private readonly DependencyFinder dependencyFinder;

    /// <summary>
    /// Initializes a new instance of the <see cref="DependencyFinderTests"/> class.
    /// </summary>
    public DependencyFinderTests()
    {
        this.assetsMock = new Mock<IAssets>();
        this.dependencyGraphMock = new Mock<IDependencyGraph>();
        this.dependencyFinder = new DependencyFinder(this.assetsMock.Object, this.dependencyGraphMock.Object);
    }

    /// <summary>
    /// Tests that when <see cref="DependencyFinder.Run(string, bool, Regex?)"/> is called but no matching dependencies
    /// are found, an empty collection is returned.
    /// </summary>
    [AllCulturesFact]
    public void Run_WithNoMatchingDependencies_ReturnsEmptyProjects()
    {
        // Arrange
        const string projectOrSolutionPath = "C:\\solution.sln";
        _ = this.dependencyGraphMock.Setup(x => x.Create(projectOrSolutionPath)).Returns(new DependencyGraphSpec());

        // Act
        var result = this.dependencyFinder.Run(projectOrSolutionPath, false, null);

        // Assert
        _ = result.HasChildren
            .Should().BeFalse();
    }

    /// <summary>
    /// Tests that when <see cref="DependencyFinder.Run(string, bool, Regex?)"/> is called and matching dependencies are
    /// found but a <see langword="null"/> lock file is provided, an empty collection is returned.
    /// </summary>
    [AllCulturesFact]
    public void Run_WithEmptyLockFile_ReturnsEmptyProjects()
    {
        // Arrange
        const string projectOrSolutionPath = "C:\\project\\solution.sln";
        const string outputPath = "C:\\project\\bin";
        LockFile? lockFile = null;
        var dependencyGraphSpec = new DependencyGraphSpec();
        dependencyGraphSpec.AddProject(
            new PackageSpec(new[] { new TargetFrameworkInformation() })
            {
                RestoreMetadata = new ProjectRestoreMetadata()
                {
                    ProjectStyle = ProjectStyle.PackageReference,
                    OutputPath = outputPath
                }
            });
        _ = this.dependencyGraphMock.Setup(mock => mock.Create(projectOrSolutionPath)).Returns(dependencyGraphSpec);
        _ = this.assetsMock.Setup(mock => mock.Create(projectOrSolutionPath, outputPath)).Returns(lockFile);

        // Act
        var result = this.dependencyFinder.Run(projectOrSolutionPath, false, null);

        // Assert
        _ = result.HasChildren
            .Should().BeFalse();
    }

    /// <summary>
    /// Tests that when <see cref="DependencyFinder.Run(string, bool, Regex?)"/> is called and matching dependencies are
    /// found but there are no matching target frameworks, an empty collection is returned.
    /// </summary>
    [AllCulturesFact]
    public void Run_WithNoMatchingTargetFrameworks_ReturnsEmptyProjects()
    {
        // Arrange
        const string projectOrSolutionPath = "C:\\project\\solution.sln";
        const string filePath = "C:\\project\\project.csproj";
        const string outputPath = "C:\\project\\bin";
        const string projectName = "Project 1";
        const string frameworkName = ".NETCoreApp";
        var dependencyGraphSpec = new DependencyGraphSpec();
        dependencyGraphSpec.AddProject(
            new PackageSpec(
                new[]
                {
                    new TargetFrameworkInformation
                    {
                        FrameworkName = new NuGetFramework(frameworkName)
                    }
                })
            {
                FilePath = filePath,
                Name = projectName,
                RestoreMetadata = new ProjectRestoreMetadata()
                {
                    ProjectStyle = ProjectStyle.PackageReference,
                    OutputPath = outputPath
                }
            });
        _ = this.dependencyGraphMock.Setup(mock => mock.Create(projectOrSolutionPath)).Returns(dependencyGraphSpec);
        var lockFile = new LockFile()
        {
            Targets = new List<LockFileTarget>(1)
            {
                new LockFileTarget
                {
                    TargetFramework = new NuGetFramework(frameworkName)
                }
            }
        };
        _ = this.assetsMock.Setup(mock => mock.Create(filePath, outputPath)).Returns(lockFile);

        // Act
        var result = this.dependencyFinder.Run(projectOrSolutionPath, false, null);

        // Assert
        _ = result.HasChildren
            .Should().BeFalse();
    }

    /// <summary>
    /// Tests that when <see cref="DependencyFinder.Run(string, bool, Regex?)"/> is called and matching dependencies are
    /// found but there are no matching project file dependency groups, an empty collection is returned.
    /// </summary>
    [AllCulturesFact]
    public void Run_WithNoMatchingProjectFileDependencyGroups_ReturnsEmptyProjects()
    {
        // Arrange
        const string projectOrSolutionPath = "C:\\project\\solution.sln";
        const string filePath = "C:\\project\\project.csproj";
        const string outputPath = "C:\\project\\bin";
        const string projectName = "Project 1";
        const string frameworkName = ".NETCoreApp";
        var dependencyGraphSpec = new DependencyGraphSpec();
        dependencyGraphSpec.AddProject(
            new PackageSpec(
                new[]
                {
                    new TargetFrameworkInformation
                    {
                        FrameworkName = new NuGetFramework(frameworkName)
                    }
                })
            {
                FilePath = filePath,
                Name = projectName,
                RestoreMetadata = new ProjectRestoreMetadata()
                {
                    ProjectStyle = ProjectStyle.PackageReference,
                    OutputPath = outputPath
                }
            });
        _ = this.dependencyGraphMock.Setup(mock => mock.Create(projectOrSolutionPath)).Returns(dependencyGraphSpec);
        var lockFile = new LockFile()
        {
            Targets = new List<LockFileTarget>(1)
            {
                new LockFileTarget
                {
                    TargetFramework = new NuGetFramework(frameworkName),
                    Libraries = new List<LockFileTargetLibrary>(1)
                    {
                        new LockFileTargetLibrary
                        {
                            Name = "Dependency 1"
                        }
                    }
                }
            },
            ProjectFileDependencyGroups = new List<ProjectFileDependencyGroup>(1)
            {
                new ProjectFileDependencyGroup(frameworkName, new List<string>(1) { "Dependency 1" })
            }
        };
        _ = this.assetsMock.Setup(mock => mock.Create(filePath, outputPath)).Returns(lockFile);

        // Act
        var result = this.dependencyFinder.Run(projectOrSolutionPath, false, null);

        // Assert
        _ = result.HasChildren
            .Should().BeFalse();
    }

    /// <summary>
    /// To do.
    /// </summary>
    [AllCulturesFact]
    public void Run_WithNoMatchingProjectDependencies_ReturnsEmptyProjects()
    {
        // Arrange
        const string projectOrSolutionPath = "C:\\project\\solution.sln";
        const string filePath = "C:\\project\\project.csproj";
        const string outputPath = "C:\\project\\bin";
        const string projectName = "Project 1";
        const string frameworkName = ".NETCoreApp";
        Version frameworkVersion = new(7, 0);
        var dependencyGraphSpec = new DependencyGraphSpec();
        dependencyGraphSpec.AddProject(
            new PackageSpec(
                new[]
                {
                    new TargetFrameworkInformation
                    {
                        FrameworkName = new NuGetFramework(frameworkName, frameworkVersion)
                    }
                })
            {
                FilePath = filePath,
                Name = projectName,
                RestoreMetadata = new ProjectRestoreMetadata()
                {
                    ProjectStyle = ProjectStyle.PackageReference,
                    OutputPath = outputPath
                }
            });
        _ = this.dependencyGraphMock.Setup(mock => mock.Create(projectOrSolutionPath)).Returns(dependencyGraphSpec);
        var lockFile = new LockFile()
        {
            ProjectFileDependencyGroups = new[]
            {
                new ProjectFileDependencyGroup($"{frameworkName},Version=v{frameworkVersion}", new string[] { })
            },
            Targets = new List<LockFileTarget>(1)
            {
                new LockFileTarget
                {
                    TargetFramework = new NuGetFramework(frameworkName, frameworkVersion),
                    Libraries = new List<LockFileTargetLibrary>
                    {
                        new LockFileTargetLibrary
                        {
                            Name = "Newtonsoft.Json",
                            Version = NuGetVersion.Parse("12.0.3")
                        }
                    }
                }
            }
        };
        _ = this.assetsMock.Setup(mock => mock.Create(filePath, outputPath)).Returns(lockFile);

        // Act
        var result = this.dependencyFinder.Run(projectOrSolutionPath, false, MatchingProjectsRegex());

        // Assert
        _ = result.HasChildren
            .Should().BeFalse();
    }

    /// <summary>
    /// To do.
    /// </summary>
    [AllCulturesFact]
    public void Run_WithMatchingProjectDependenciesButNoTransitiveDependencies_ReturnsEmptyProjects()
    {
        // Arrange
        const string projectOrSolutionPath = "C:\\project\\solution.sln";
        const string filePath = "C:\\project\\project.csproj";
        const string outputPath = "C:\\project\\bin";
        const string projectName = "Project 1";
        const string frameworkName = ".NETCoreApp";
        Version frameworkVersion = new(7, 0);
        var dependencyGraphSpec = new DependencyGraphSpec();
        dependencyGraphSpec.AddProject(
            new PackageSpec(
                new[]
                {
                    new TargetFrameworkInformation
                    {
                        FrameworkName = new NuGetFramework(frameworkName, frameworkVersion)
                    }
                })
            {
                FilePath = filePath,
                Name = projectName,
                RestoreMetadata = new ProjectRestoreMetadata()
                {
                    ProjectStyle = ProjectStyle.PackageReference,
                    OutputPath = outputPath
                }
            });
        _ = this.dependencyGraphMock.Setup(mock => mock.Create(projectOrSolutionPath)).Returns(dependencyGraphSpec);
        var lockFile = new LockFile()
        {
            ProjectFileDependencyGroups = new[]
            {
                new ProjectFileDependencyGroup($"{frameworkName},Version=v{frameworkVersion}", new string[]
                {
                    "Newtonsoft.Json,Version=v12.0.3"
                })
            },
            Targets = new List<LockFileTarget>(1)
            {
                new LockFileTarget
                {
                    TargetFramework = new NuGetFramework(frameworkName, frameworkVersion),
                    Libraries = new List<LockFileTargetLibrary>
                    {
                        new LockFileTargetLibrary
                        {
                            Name = "Newtonsoft.Json",
                            Version = NuGetVersion.Parse("12.0.3")
                        }
                    }
                }
            }
        };
        _ = this.assetsMock.Setup(mock => mock.Create(filePath, outputPath)).Returns(lockFile);

        // Act
        var result = this.dependencyFinder.Run(projectOrSolutionPath, false, MatchingProjectsRegex());

        // Assert
        _ = result.HasChildren
            .Should().BeFalse();
    }

    /// <summary>
    /// A regular expression representing the package <c>Newtonsoft.Json</c>, which is used by the unit tests.
    /// </summary>
    /// <returns>The regular expression.</returns>
    [GeneratedRegex("Newtonsoft\\.Json")]
    private static partial Regex MatchingProjectsRegex();
}
