// <copyright file="DependencyFinderUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.ProjectAnalysis;

using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Moq;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Versioning;
using NuGetTransitiveDependencyFinder.ProjectAnalysis;
using NuGetTransitiveDependencyFinder.UnitTests.Utilities.Globalization;

/// <summary>
/// Unit tests for the <see cref="DependencyFinder"/> class.
/// </summary>
public partial class DependencyFinderUnitTests
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
    /// Initializes a new instance of the <see cref="DependencyFinderUnitTests"/> class.
    /// </summary>
    public DependencyFinderUnitTests()
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
        _ = this.dependencyGraphMock
            .Setup(x => x.Create(projectOrSolutionPath))
            .Returns(new DependencyGraphSpec());

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
            new PackageSpec([new TargetFrameworkInformation()])
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
                [
                    new TargetFrameworkInformation
                    {
                        FrameworkName = new NuGetFramework(frameworkName)
                    }
                ])
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
            Targets =
            [
                new()
                {
                    TargetFramework = new NuGetFramework(frameworkName)
                }
            ]
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
                [
                    new TargetFrameworkInformation
                    {
                        FrameworkName = new NuGetFramework(frameworkName)
                    }
                ])
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
            Targets =
            [
                new()
                {
                    TargetFramework = new NuGetFramework(frameworkName),
                    Libraries =
                    [
                        new()
                        {
                            Name = "Dependency 1"
                        }
                    ]
                }
            ],
            ProjectFileDependencyGroups =
            [
                new(frameworkName, ["Dependency 1"])
            ]
        };
        _ = this.assetsMock.Setup(mock => mock.Create(filePath, outputPath)).Returns(lockFile);

        // Act
        var result = this.dependencyFinder.Run(projectOrSolutionPath, false, null);

        // Assert
        _ = result.HasChildren
            .Should().BeFalse();
    }

    /// <summary>
    /// Tests that when <see cref="DependencyFinder.Run(string, bool, Regex?)"/> is called and no matching dependencies
    /// are found, an empty collection is returned.
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
                [
                    new TargetFrameworkInformation
                    {
                        FrameworkName = new NuGetFramework(frameworkName, frameworkVersion)
                    }
                ])
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
            ProjectFileDependencyGroups =
            [
                new ProjectFileDependencyGroup($"{frameworkName},Version=v{frameworkVersion}", [])
            ],
            Targets =
            [
                new()
                {
                    TargetFramework = new NuGetFramework(frameworkName, frameworkVersion),
                    Libraries =
                    [
                        new()
                        {
                            Name = "Newtonsoft.Json",
                            Version = NuGetVersion.Parse("12.0.3")
                        }
                    ]
                }
            ]
        };
        _ = this.assetsMock.Setup(mock => mock.Create(filePath, outputPath)).Returns(lockFile);

        // Act
        var result = this.dependencyFinder.Run(projectOrSolutionPath, false, MatchingProjectsRegex);

        // Assert
        _ = result.HasChildren
            .Should().BeFalse();
    }

    /// <summary>
    /// Tests that when <see cref="DependencyFinder.Run(string, bool, Regex?)"/> is called and matching dependencies are
    /// found but there are no transitive dependencies, an empty collection is returned.
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
                [
                    new TargetFrameworkInformation
                    {
                        FrameworkName = new NuGetFramework(frameworkName, frameworkVersion)
                    }
                ])
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
            ProjectFileDependencyGroups =
            [
                new ProjectFileDependencyGroup($"{frameworkName},Version=v{frameworkVersion}",
                [
                    "Newtonsoft.Json,Version=v12.0.3"
                ])
            ],
            Targets =
            [
                new()
                {
                    TargetFramework = new NuGetFramework(frameworkName, frameworkVersion),
                    Libraries =
                    [
                        new()
                        {
                            Name = "Newtonsoft.Json",
                            Version = NuGetVersion.Parse("12.0.3")
                        }
                    ]
                }
            ]
        };
        _ = this.assetsMock.Setup(mock => mock.Create(filePath, outputPath)).Returns(lockFile);

        // Act
        var result = this.dependencyFinder.Run(projectOrSolutionPath, false, MatchingProjectsRegex);

        // Assert
        _ = result.HasChildren
            .Should().BeFalse();
    }

    /// <summary>
    /// Tests that when <see cref="DependencyFinder.Run(string, bool, Regex?)"/> is called with a graph where a direct
    /// project dependency pulls in a transitive library, the transitive library is surfaced as a child dependency.
    /// </summary>
    [AllCulturesFact]
    public void Run_WithTransitiveLibrary_MarksLibraryAsTransitive()
    {
        // Arrange
        const string projectOrSolutionPath = "C:\\project\\solution.sln";
        const string filePath = "C:\\project\\project.csproj";
        const string outputPath = "C:\\project\\bin";
        const string frameworkName = ".NETCoreApp";
        Version frameworkVersion = new(7, 0);
        var dependencyGraphSpec = new DependencyGraphSpec();
        dependencyGraphSpec.AddProject(
            new PackageSpec(
                [
                    new TargetFrameworkInformation
                    {
                        FrameworkName = new NuGetFramework(frameworkName, frameworkVersion),
                    },
                ])
            {
                FilePath = filePath,
                Name = "Project 1",
                RestoreMetadata = new ProjectRestoreMetadata()
                {
                    ProjectStyle = ProjectStyle.PackageReference,
                    OutputPath = outputPath,
                },
            });
        _ = this.dependencyGraphMock.Setup(mock => mock.Create(projectOrSolutionPath)).Returns(dependencyGraphSpec);

        var direct = new LockFileTargetLibrary
        {
            Name = "Direct",
            Version = NuGetVersion.Parse("1.0.0"),
            Dependencies =
            [
                new PackageDependency("Transitive", new VersionRange(NuGetVersion.Parse("1.0.0"))),
            ],
        };
        var transitive = new LockFileTargetLibrary
        {
            Name = "Transitive",
            Version = NuGetVersion.Parse("1.0.0"),
        };
        var lockFile = new LockFile
        {
            ProjectFileDependencyGroups =
            [
                new ProjectFileDependencyGroup(
                    $"{frameworkName},Version=v{frameworkVersion}",
                    ["Direct >= 1.0.0", "Transitive >= 1.0.0"]),
            ],
            Targets =
            [
                new LockFileTarget
                {
                    TargetFramework = new NuGetFramework(frameworkName, frameworkVersion),
                    Libraries = [direct, transitive],
                },
            ],
        };
        _ = this.assetsMock.Setup(mock => mock.Create(filePath, outputPath)).Returns(lockFile);

        // Act
        var result = this.dependencyFinder.Run(projectOrSolutionPath, false, null);

        // Assert
        _ = result.HasChildren
            .Should().BeTrue();
        var dependencies = result.SortedChildren
            .SelectMany(project => project.SortedChildren)
            .SelectMany(framework => framework.SortedChildren)
            .ToList();
        _ = dependencies
            .Should().ContainSingle(dependency => dependency.Identifier == "Transitive");
    }

    /// <summary>
    /// Tests that when <see cref="DependencyFinder.Run(string, bool, Regex?)"/> is called with
    /// <c>collateAllDependencies</c> set to <see langword="true"/>, both direct and transitive libraries are returned.
    /// </summary>
    [AllCulturesFact]
    public void Run_WithCollateAllDependenciesTrue_ReturnsBothDirectAndTransitive()
    {
        // Arrange
        const string projectOrSolutionPath = "C:\\project\\solution.sln";
        const string filePath = "C:\\project\\project.csproj";
        const string outputPath = "C:\\project\\bin";
        const string frameworkName = ".NETCoreApp";
        Version frameworkVersion = new(7, 0);
        var dependencyGraphSpec = new DependencyGraphSpec();
        dependencyGraphSpec.AddProject(
            new PackageSpec(
                [
                    new TargetFrameworkInformation
                    {
                        FrameworkName = new NuGetFramework(frameworkName, frameworkVersion),
                    },
                ])
            {
                FilePath = filePath,
                Name = "Project 1",
                RestoreMetadata = new ProjectRestoreMetadata()
                {
                    ProjectStyle = ProjectStyle.PackageReference,
                    OutputPath = outputPath,
                },
            });
        _ = this.dependencyGraphMock.Setup(mock => mock.Create(projectOrSolutionPath)).Returns(dependencyGraphSpec);

        var direct = new LockFileTargetLibrary
        {
            Name = "Direct",
            Version = NuGetVersion.Parse("1.0.0"),
            Dependencies =
            [
                new PackageDependency("Transitive", new VersionRange(NuGetVersion.Parse("1.0.0"))),
            ],
        };
        var transitive = new LockFileTargetLibrary
        {
            Name = "Transitive",
            Version = NuGetVersion.Parse("1.0.0"),
        };
        var lockFile = new LockFile
        {
            ProjectFileDependencyGroups =
            [
                new ProjectFileDependencyGroup(
                    $"{frameworkName},Version=v{frameworkVersion}",
                    ["Direct >= 1.0.0"]),
            ],
            Targets =
            [
                new LockFileTarget
                {
                    TargetFramework = new NuGetFramework(frameworkName, frameworkVersion),
                    Libraries = [direct, transitive],
                },
            ],
        };
        _ = this.assetsMock.Setup(mock => mock.Create(filePath, outputPath)).Returns(lockFile);

        // Act
        var result = this.dependencyFinder.Run(projectOrSolutionPath, true, null);

        // Assert
        var dependencies = result.SortedChildren
            .SelectMany(project => project.SortedChildren)
            .SelectMany(framework => framework.SortedChildren)
            .Select(dependency => dependency.Identifier)
            .ToList();
        _ = dependencies
            .Should().Contain(["Direct", "Transitive"]);
    }

    /// <summary>
    /// Tests that when <see cref="DependencyFinder.Run(string, bool, Regex?)"/> is called with a filter, only
    /// dependencies whose identifiers match the filter are returned.
    /// </summary>
    [AllCulturesFact]
    public void Run_WithFilter_ReturnsOnlyMatchingDependencies()
    {
        // Arrange
        const string projectOrSolutionPath = "C:\\project\\solution.sln";
        const string filePath = "C:\\project\\project.csproj";
        const string outputPath = "C:\\project\\bin";
        const string frameworkName = ".NETCoreApp";
        Version frameworkVersion = new(7, 0);
        var dependencyGraphSpec = new DependencyGraphSpec();
        dependencyGraphSpec.AddProject(
            new PackageSpec(
                [
                    new TargetFrameworkInformation
                    {
                        FrameworkName = new NuGetFramework(frameworkName, frameworkVersion),
                    },
                ])
            {
                FilePath = filePath,
                Name = "Project 1",
                RestoreMetadata = new ProjectRestoreMetadata()
                {
                    ProjectStyle = ProjectStyle.PackageReference,
                    OutputPath = outputPath,
                },
            });
        _ = this.dependencyGraphMock.Setup(mock => mock.Create(projectOrSolutionPath)).Returns(dependencyGraphSpec);

        var matching = new LockFileTargetLibrary
        {
            Name = "Matching",
            Version = NuGetVersion.Parse("1.0.0"),
        };
        var other = new LockFileTargetLibrary
        {
            Name = "Other",
            Version = NuGetVersion.Parse("1.0.0"),
        };
        var lockFile = new LockFile
        {
            ProjectFileDependencyGroups =
            [
                new ProjectFileDependencyGroup(
                    $"{frameworkName},Version=v{frameworkVersion}",
                    ["Matching >= 1.0.0", "Other >= 1.0.0"]),
            ],
            Targets =
            [
                new LockFileTarget
                {
                    TargetFramework = new NuGetFramework(frameworkName, frameworkVersion),
                    Libraries = [matching, other],
                },
            ],
        };
        _ = this.assetsMock.Setup(mock => mock.Create(filePath, outputPath)).Returns(lockFile);

        // Act
        var result = this.dependencyFinder.Run(projectOrSolutionPath, true, MatchingRegex);

        // Assert
        var dependencies = result.SortedChildren
            .SelectMany(project => project.SortedChildren)
            .SelectMany(framework => framework.SortedChildren)
            .Select(dependency => dependency.Identifier)
            .ToList();
        _ = dependencies
            .Should().Contain("Matching");
        _ = dependencies
            .Should().NotContain("Other");
    }

    /// <summary>
    /// Tests that when <see cref="DependencyFinder.Run(string, bool, Regex?)"/> is called, projects whose
    /// <c>ProjectStyle</c> is not <see cref="ProjectStyle.PackageReference"/> are skipped.
    /// </summary>
    [AllCulturesFact]
    public void Run_WithNonPackageReferenceProject_SkipsProject()
    {
        // Arrange
        const string projectOrSolutionPath = "C:\\project\\solution.sln";
        var dependencyGraphSpec = new DependencyGraphSpec();
        dependencyGraphSpec.AddProject(
            new PackageSpec([new TargetFrameworkInformation()])
            {
                RestoreMetadata = new ProjectRestoreMetadata()
                {
                    ProjectStyle = ProjectStyle.Unknown,
                    OutputPath = "C:\\ignored",
                },
            });
        _ = this.dependencyGraphMock.Setup(mock => mock.Create(projectOrSolutionPath)).Returns(dependencyGraphSpec);

        // Act
        var result = this.dependencyFinder.Run(projectOrSolutionPath, false, null);

        // Assert
        _ = result.HasChildren
            .Should().BeFalse();
        this.assetsMock.Verify(
            mock => mock.Create(It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
    }

    /// <summary>
    /// Tests that when <see cref="DependencyFinder.Run(string, bool, Regex?)"/> is called and the target library has
    /// a <see langword="null"/> <see cref="LockFileTargetLibrary.Version"/>, the library is silently skipped during
    /// dependency recording, exercising the early-return guard in <c>RecordDependency</c>.
    /// </summary>
    [AllCulturesFact]
    public void Run_WithLibraryHavingNullVersion_SilentlySkipsLibrary()
    {
        // Arrange
        const string projectOrSolutionPath = "C:\\project\\solution.sln";
        const string filePath = "C:\\project\\project.csproj";
        const string outputPath = "C:\\project\\bin";
        const string frameworkName = ".NETCoreApp";
        Version frameworkVersion = new(7, 0);
        var dependencyGraphSpec = new DependencyGraphSpec();
        dependencyGraphSpec.AddProject(
            new PackageSpec(
                [
                    new TargetFrameworkInformation
                    {
                        FrameworkName = new NuGetFramework(frameworkName, frameworkVersion),
                    },
                ])
            {
                FilePath = filePath,
                Name = "Project 1",
                RestoreMetadata = new ProjectRestoreMetadata()
                {
                    ProjectStyle = ProjectStyle.PackageReference,
                    OutputPath = outputPath,
                },
            });
        _ = this.dependencyGraphMock.Setup(mock => mock.Create(projectOrSolutionPath)).Returns(dependencyGraphSpec);

        var libraryWithoutVersion = new LockFileTargetLibrary
        {
            Name = "NoVersion",
        };
        var lockFile = new LockFile
        {
            ProjectFileDependencyGroups =
            [
                new ProjectFileDependencyGroup(
                    $"{frameworkName},Version=v{frameworkVersion}",
                    ["NoVersion >= 1.0.0"]),
            ],
            Targets =
            [
                new LockFileTarget
                {
                    TargetFramework = new NuGetFramework(frameworkName, frameworkVersion),
                    Libraries = [libraryWithoutVersion],
                },
            ],
        };
        _ = this.assetsMock.Setup(mock => mock.Create(filePath, outputPath)).Returns(lockFile);

        // Act
        var result = this.dependencyFinder.Run(projectOrSolutionPath, true, null);

        // Assert
        var dependencies = result.SortedChildren
            .SelectMany(project => project.SortedChildren)
            .SelectMany(framework => framework.SortedChildren)
            .ToList();
        _ = dependencies
            .Should().BeEmpty();
    }

    /// <summary>
    /// Tests that when <see cref="DependencyFinder.Run(string, bool, Regex?)"/> is called and the matching
    /// <see cref="ProjectFileDependencyGroup"/> is not the first group in the collection, the non-matching group is
    /// skipped and the correct matching group is used. This exercises the <see langword="false"/> branch of the
    /// framework-matching predicate in <c>FirstOrDefault</c>.
    /// </summary>
    [AllCulturesFact]
    public void Run_WithNonMatchingProjectFileDependencyGroupBeforeMatchingGroup_UsesMatchingGroup()
    {
        // Arrange
        const string projectOrSolutionPath = "C:\\project\\solution.sln";
        const string filePath = "C:\\project\\project.csproj";
        const string outputPath = "C:\\project\\bin";
        const string frameworkName = ".NETCoreApp";
        Version frameworkVersion = new(7, 0);
        var dependencyGraphSpec = new DependencyGraphSpec();
        dependencyGraphSpec.AddProject(
            new PackageSpec(
                [
                    new TargetFrameworkInformation
                    {
                        FrameworkName = new NuGetFramework(frameworkName, frameworkVersion),
                    },
                ])
            {
                FilePath = filePath,
                Name = "Project 1",
                RestoreMetadata = new ProjectRestoreMetadata()
                {
                    ProjectStyle = ProjectStyle.PackageReference,
                    OutputPath = outputPath,
                },
            });
        _ = this.dependencyGraphMock.Setup(mock => mock.Create(projectOrSolutionPath)).Returns(dependencyGraphSpec);

        var library = new LockFileTargetLibrary
        {
            Name = "Library",
            Version = NuGetVersion.Parse("1.0.0"),
        };
        var lockFile = new LockFile
        {
            ProjectFileDependencyGroups =
            [
                // Non-matching group first — lambda must return false for this entry.
                new ProjectFileDependencyGroup(
                    ".NETFramework,Version=v4.7.2",
                    ["OtherLibrary >= 1.0.0"]),
                // Matching group second — lambda returns true, FirstOrDefault selects it.
                new ProjectFileDependencyGroup(
                    $"{frameworkName},Version=v{frameworkVersion}",
                    ["Library >= 1.0.0"]),
            ],
            Targets =
            [
                new LockFileTarget
                {
                    TargetFramework = new NuGetFramework(frameworkName, frameworkVersion),
                    Libraries = [library],
                },
            ],
        };
        _ = this.assetsMock.Setup(mock => mock.Create(filePath, outputPath)).Returns(lockFile);

        // Act
        var result = this.dependencyFinder.Run(projectOrSolutionPath, true, null);

        // Assert
        var dependencies = result.SortedChildren
            .SelectMany(project => project.SortedChildren)
            .SelectMany(framework => framework.SortedChildren)
            .ToList();
        _ = dependencies
            .Should().ContainSingle(dependency => dependency.Identifier == "Library");
    }

    /// <summary>
    /// Tests that when <see cref="DependencyFinder.Run(string, bool, Regex?)"/> is called with a dependency named
    /// <c>NETStandard.Library</c>, that dependency is never marked as transitive, even if it appears as a direct
    /// dependency pulled in by another library. This exercises the explicit NETStandard.Library exclusion in
    /// <c>FindTransitiveDependencies</c>.
    /// </summary>
    [AllCulturesFact]
    public void Run_WithNetStandardLibrary_DoesNotMarkItAsTransitive()
    {
        // Arrange
        const string projectOrSolutionPath = "C:\\project\\solution.sln";
        const string filePath = "C:\\project\\project.csproj";
        const string outputPath = "C:\\project\\bin";
        const string frameworkName = ".NETCoreApp";
        Version frameworkVersion = new(7, 0);
        var dependencyGraphSpec = new DependencyGraphSpec();
        dependencyGraphSpec.AddProject(
            new PackageSpec(
                [
                    new TargetFrameworkInformation
                    {
                        FrameworkName = new NuGetFramework(frameworkName, frameworkVersion),
                    },
                ])
            {
                FilePath = filePath,
                Name = "Project 1",
                RestoreMetadata = new ProjectRestoreMetadata()
                {
                    ProjectStyle = ProjectStyle.PackageReference,
                    OutputPath = outputPath,
                },
            });
        _ = this.dependencyGraphMock.Setup(mock => mock.Create(projectOrSolutionPath)).Returns(dependencyGraphSpec);

        var direct = new LockFileTargetLibrary
        {
            Name = "Direct",
            Version = NuGetVersion.Parse("1.0.0"),
            Dependencies =
            [
                new PackageDependency("NETStandard.Library", new VersionRange(NuGetVersion.Parse("2.0.0"))),
            ],
        };
        var netStandard = new LockFileTargetLibrary
        {
            Name = "NETStandard.Library",
            Version = NuGetVersion.Parse("2.0.0"),
        };
        var lockFile = new LockFile
        {
            ProjectFileDependencyGroups =
            [
                new ProjectFileDependencyGroup(
                    $"{frameworkName},Version=v{frameworkVersion}",
                    ["Direct >= 1.0.0", "NETStandard.Library >= 2.0.0"]),
            ],
            Targets =
            [
                new LockFileTarget
                {
                    TargetFramework = new NuGetFramework(frameworkName, frameworkVersion),
                    Libraries = [direct, netStandard],
                },
            ],
        };
        _ = this.assetsMock.Setup(mock => mock.Create(filePath, outputPath)).Returns(lockFile);

        // Act: transitive-only mode (collateAllDependencies=false). NETStandard.Library has Via populated
        // because Direct declares it as a dependency, so without the explicit exclusion it would be flagged.
        var result = this.dependencyFinder.Run(projectOrSolutionPath, false, null);

        // Assert
        _ = result.HasChildren
            .Should().BeFalse();
    }

    /// <summary>
    /// Tests that when <see cref="DependencyFinder.Run(string, bool, Regex?)"/> is called with a dependency named
    /// using differently cased <c>netstandard.library</c>, the case-insensitive exclusion still applies.
    /// </summary>
    [AllCulturesFact]
    public void Run_WithNetStandardLibraryDifferentCasing_DoesNotMarkItAsTransitive()
    {
        // Arrange
        const string projectOrSolutionPath = "C:\\project\\solution.sln";
        const string filePath = "C:\\project\\project.csproj";
        const string outputPath = "C:\\project\\bin";
        const string frameworkName = ".NETCoreApp";
        Version frameworkVersion = new(7, 0);
        var dependencyGraphSpec = new DependencyGraphSpec();
        dependencyGraphSpec.AddProject(
            new PackageSpec(
                [
                    new TargetFrameworkInformation
                    {
                        FrameworkName = new NuGetFramework(frameworkName, frameworkVersion),
                    },
                ])
            {
                FilePath = filePath,
                Name = "Project 1",
                RestoreMetadata = new ProjectRestoreMetadata()
                {
                    ProjectStyle = ProjectStyle.PackageReference,
                    OutputPath = outputPath,
                },
            });
        _ = this.dependencyGraphMock.Setup(mock => mock.Create(projectOrSolutionPath)).Returns(dependencyGraphSpec);

        var direct = new LockFileTargetLibrary
        {
            Name = "Direct",
            Version = NuGetVersion.Parse("1.0.0"),
            Dependencies =
            [
                new PackageDependency("netstandard.library", new VersionRange(NuGetVersion.Parse("2.0.0"))),
            ],
        };
        var netStandard = new LockFileTargetLibrary
        {
            Name = "netstandard.library",
            Version = NuGetVersion.Parse("2.0.0"),
        };
        var lockFile = new LockFile
        {
            ProjectFileDependencyGroups =
            [
                new ProjectFileDependencyGroup(
                    $"{frameworkName},Version=v{frameworkVersion}",
                    ["Direct >= 1.0.0", "netstandard.library >= 2.0.0"]),
            ],
            Targets =
            [
                new LockFileTarget
                {
                    TargetFramework = new NuGetFramework(frameworkName, frameworkVersion),
                    Libraries = [direct, netStandard],
                },
            ],
        };
        _ = this.assetsMock.Setup(mock => mock.Create(filePath, outputPath)).Returns(lockFile);

        // Act
        var result = this.dependencyFinder.Run(projectOrSolutionPath, false, null);

        // Assert
        _ = result.HasChildren
            .Should().BeFalse();
    }

    /// <summary>
    /// Tests that when <see cref="DependencyFinder.Run(string, bool, System.Text.RegularExpressions.Regex?)"/>
    /// is called against a project targeting multiple frameworks, dependency state from an earlier framework does
    /// not leak into a later framework's result. This pins down the <c>this.dependencies.Clear()</c> invariant at
    /// the start of each framework iteration: removing that call would cause the transitive dependency recorded
    /// for the first framework to be surfaced against the second framework even though the second framework does
    /// not reference it.
    /// </summary>
    [AllCulturesFact]
    public void Run_WithMultipleFrameworks_DoesNotLeakDependenciesBetweenFrameworks()
    {
        // Arrange
        const string projectOrSolutionPath = "C:\\project\\solution.sln";
        const string filePath = "C:\\project\\project.csproj";
        const string outputPath = "C:\\project\\bin";
        const string frameworkName = ".NETCoreApp";
        Version firstFrameworkVersion = new(7, 0);
        Version secondFrameworkVersion = new(8, 0);
        var firstFramework = new NuGetFramework(frameworkName, firstFrameworkVersion);
        var secondFramework = new NuGetFramework(frameworkName, secondFrameworkVersion);
        var dependencyGraphSpec = new DependencyGraphSpec();
        dependencyGraphSpec.AddProject(
            new PackageSpec(
                [
                    new TargetFrameworkInformation
                    {
                        FrameworkName = firstFramework,
                    },
                    new TargetFrameworkInformation
                    {
                        FrameworkName = secondFramework,
                    },
                ])
            {
                FilePath = filePath,
                Name = "Project 1",
                RestoreMetadata = new ProjectRestoreMetadata()
                {
                    ProjectStyle = ProjectStyle.PackageReference,
                    OutputPath = outputPath,
                },
            });
        _ = this.dependencyGraphMock.Setup(mock => mock.Create(projectOrSolutionPath)).Returns(dependencyGraphSpec);

        var firstDirect = new LockFileTargetLibrary
        {
            Name = "FirstDirect",
            Version = NuGetVersion.Parse("1.0.0"),
            Dependencies =
            [
                new PackageDependency("FirstTransitive", new VersionRange(NuGetVersion.Parse("1.0.0"))),
            ],
        };
        var firstTransitive = new LockFileTargetLibrary
        {
            Name = "FirstTransitive",
            Version = NuGetVersion.Parse("1.0.0"),
        };
        var secondDirect = new LockFileTargetLibrary
        {
            Name = "SecondDirect",
            Version = NuGetVersion.Parse("1.0.0"),
        };
        var lockFile = new LockFile
        {
            ProjectFileDependencyGroups =
            [
                new ProjectFileDependencyGroup(
                    $"{frameworkName},Version=v{firstFrameworkVersion}",
                    ["FirstDirect >= 1.0.0"]),
                new ProjectFileDependencyGroup(
                    $"{frameworkName},Version=v{secondFrameworkVersion}",
                    ["SecondDirect >= 1.0.0"]),
            ],
            Targets =
            [
                new LockFileTarget
                {
                    TargetFramework = firstFramework,
                    Libraries = [firstDirect, firstTransitive],
                },
                new LockFileTarget
                {
                    TargetFramework = secondFramework,
                    Libraries = [secondDirect],
                },
            ],
        };
        _ = this.assetsMock.Setup(mock => mock.Create(filePath, outputPath)).Returns(lockFile);

        // Act
        var result = this.dependencyFinder.Run(projectOrSolutionPath, true, null);

        // Assert
        var firstFrameworkDependencies = result.SortedChildren
            .SelectMany(project => project.SortedChildren)
            .Where(framework => framework.Identifier == firstFramework)
            .SelectMany(framework => framework.SortedChildren)
            .Select(dependency => dependency.Identifier)
            .ToList();
        var secondFrameworkDependencies = result.SortedChildren
            .SelectMany(project => project.SortedChildren)
            .Where(framework => framework.Identifier == secondFramework)
            .SelectMany(framework => framework.SortedChildren)
            .Select(dependency => dependency.Identifier)
            .ToList();
        _ = firstFrameworkDependencies
            .Should().BeEquivalentTo("FirstDirect", "FirstTransitive");
        _ = secondFrameworkDependencies
            .Should().BeEquivalentTo("SecondDirect");
    }

    /// <summary>
    /// Gets a regular expression matching dependencies named <c>Matching</c>, used by the unit tests.
    /// </summary>
    [GeneratedRegex("^Matching$")]
    private static partial Regex MatchingRegex { get; }

    /// <summary>
    /// Gets a regular expression representing the package <c>Newtonsoft.Json</c>, which is used by the unit tests.
    /// </summary>
    [GeneratedRegex("Newtonsoft\\.Json")]
    private static partial Regex MatchingProjectsRegex { get; }
}
