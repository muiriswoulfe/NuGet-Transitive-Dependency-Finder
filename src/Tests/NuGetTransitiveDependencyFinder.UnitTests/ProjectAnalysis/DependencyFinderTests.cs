// <copyright file="DependencyFinderTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.ProjectAnalysis;

using System.Text.RegularExpressions;
using FluentAssertions;
using Moq;
using NuGet.ProjectModel;
using NuGetTransitiveDependencyFinder.ProjectAnalysis;
using NuGetTransitiveDependencyFinder.TestUtilities.Globalization;

/// <summary>
/// Unit tests for the <see cref="DependencyFinder"/> class.
/// </summary>
public class DependencyFinderTests
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
    /// found, a valid collection is returned.
    /// </summary>
    [AllCulturesFact]
    public void Run_WithMatchingDependencies_ReturnsProjects()
    {
        // Arrange
        const string projectOrSolutionPath = "C:\\solution.sln";
        var dependencyGraphSpec = new DependencyGraphSpec();
        dependencyGraphSpec.AddProject(
            new PackageSpec(new[] { new TargetFrameworkInformation() })
            {
                RestoreMetadata = new ProjectRestoreMetadata()
                {
                    ProjectStyle = ProjectStyle.PackageReference
                }
            });
        _ = this.dependencyGraphMock.Setup(x => x.Create(projectOrSolutionPath)).Returns(dependencyGraphSpec);

        // Act
        var result = this.dependencyFinder.Run(projectOrSolutionPath, false, null);

        // Assert
        _ = result.HasChildren
            .Should().BeTrue();
    }

    //// [Fact]
    //// public void Run_WithAssetsFile_ReturnsProjects()
    //// {
    ////     // Arrange
    ////     const string projectOrSolutionPath = "path/to/project";
    ////     var projects = new[] { new PackageSpec(projectOrSolutionPath) };
    ////     var assetsFile = new LockFile();
    ////     this.dependencyGraphMock.Setup(x => x.Create(projectOrSolutionPath)).Returns(new DependencyGraphSpec(projects));
    ////     this.assetsMock.Setup(x => x.Create(projectOrSolutionPath, It.IsAny<string>())).Returns(assetsFile);

    ////     // Act
    ////     var result = this.dependencyFinder.Run(projectOrSolutionPath, false, null);

    ////     // Assert
    ////     result.Should().NotBeEmpty();
    //// }

    //// [Fact]
    //// public void Run_WithNoProjectDependencyGroup_ReturnsEmptyProject()
    //// {
    ////     // Arrange
    ////     var projectOrSolutionPath = "path/to/project";
    ////     var projects = new[] { new PackageSpec(projectOrSolutionPath) };
    ////     var assetsFile = new LockFile();
    ////     this.dependencyGraphMock.Setup(x => x.Create(projectOrSolutionPath)).Returns(new DependencyGraphSpec(projects));
    ////     this.assetsMock.Setup(x => x.Create(projectOrSolutionPath, It.IsAny<string>())).Returns(assetsFile);

    ////     // Act
    ////     var result = this.dependencyFinder.Run(projectOrSolutionPath, false, null);

    ////     // Assert
    ////     result.Should().ContainSingle();
    ////     result[0].Name.Should().Be(projectOrSolutionPath);
    ////     result[0].Frameworks.Should().BeEmpty();
    //// }

    //// [Fact]
    //// public void Run_WithProjectDependencyGroup_ReturnsProject()
    //// {
    ////     // Arrange
    ////     const string projectOrSolutionPath = "path/to/project";
    ////     const string projectName = "projectName";
    ////     const string targetFramework = "net5.0";
    ////     var projects = new[] { new PackageSpec(projectOrSolutionPath) { Name = projectName } };
    ////     var assetsFile = new LockFile
    ////     {
    ////         Targets = new List<LockFileTarget>
    ////             {
    ////                 new LockFileTarget
    ////                 {
    ////                     TargetFramework = targetFramework,
    ////                     Libraries = new List<LockFileTargetLibrary>
    ////                     {
    ////                         new LockFileTargetLibrary
    ////                         {
    ////                             Name = "library1",
    ////                             Version = "1.0.0",
    ////                             Dependencies = new List<PackageDependency>
    ////                             {
    ////                                 new PackageDependency("dependency1", VersionRange.Parse("1.0.0"))
    ////                             }
    ////                         }
    ////                     }
    ////                 }
    ////             },
    ////         ProjectFileDependencyGroups = new List<ProjectFileDependencyGroup>
    ////             {
    ////                 new ProjectFileDependencyGroup(targetFramework, new[] { "dependency1" })
    ////             }
    ////     };
    ////     this.dependencyGraphMock.Setup(x => x.Create(projectOrSolutionPath)).Returns(new DependencyGraphSpec(projects));
    ////     this.assetsMock.Setup(x => x.Create(projectOrSolutionPath, It.IsAny<string>())).Returns(assetsFile);

    ////     // Act
    ////     var result = this.dependencyFinder.Run(projectOrSolutionPath, false, null);

    ////     // Assert
    ////     result.Should().ContainSingle();
    ////     result[0].Name.Should().Be(projectName);
    ////     result[0].Frameworks.Should().ContainSingle();
    ////     result[0].Frameworks[0].Name.Should().Be(targetFramework);
    ////     result[0].Frameworks[0].Dependencies.Should().ContainSingle();
    ////     result[0].Frameworks[0].Dependencies[0].Identifier.Should().Be("library1");
    ////     result[0].Frameworks[0].Dependencies[0].Via.Should().BeEmpty();
    ////     result[0].Frameworks[0].Dependencies[0].IsTransitive.Should().BeFalse();
    //// }

    //// [Fact]
    //// public void Run_WithProjectDependencyGroupAndTransitiveDependencies_ReturnsProject()
    //// {
    ////     // Arrange
    ////     const string projectOrSolutionPath = "path/to/project";
    ////     const string projectName = "projectName";
    ////     const string targetFramework = "net5.0";
    ////     var projects = new[] { new PackageSpec(projectOrSolutionPath) { Name = projectName } };
    ////     var assetsFile = new LockFile
    ////     {
    ////         Targets = new List<LockFileTarget>
    ////             {
    ////                 new LockFileTarget
    ////                 {
    ////                     TargetFramework = targetFramework,
    ////                     Libraries = new List<LockFileTargetLibrary>
    ////                     {
    ////                         new LockFileTargetLibrary
    ////                         {
    ////                             Name = "library1",
    ////                             Version = "1.0.0",
    ////                             Dependencies = new List<PackageDependency>
    ////                             {
    ////                                 new PackageDependency("dependency1", VersionRange.Parse("1.0.0"))
    ////                             }
    ////                         },
    ////                         new LockFileTargetLibrary
    ////                         {
    ////                             Name = "library2",
    ////                             Version = "1.0.0",
    ////                             Dependencies = new List<PackageDependency>
    ////                             {
    ////                                 new PackageDependency("library1", VersionRange.Parse("1.0.0"))
    ////                             }
    ////                         }
    ////                     }
    ////                 }
    ////             },
    ////         ProjectFileDependencyGroups = new List<ProjectFileDependencyGroup>
    ////             {
    ////                 new ProjectFileDependencyGroup(targetFramework, new[] { "dependency1" })
    ////             }
    ////     };
    ////     this.dependencyGraphMock.Setup(x => x.Create(projectOrSolutionPath)).Returns(new DependencyGraphSpec(projects));
    ////     this.assetsMock.Setup(x => x.Create(projectOrSolutionPath, It.IsAny<string>())).Returns(assetsFile);

    ////     // Act
    ////     var result = this.dependencyFinder.Run(projectOrSolutionPath, false, null);

    ////     // Assert
    ////     result.Should().ContainSingle();
    ////     result[0].Name.Should().Be(projectName);
    ////     result[0].Frameworks.Should().ContainSingle();
    ////     result[0].Frameworks[0].Name.Should().Be(targetFramework);
    ////     result[0].Frameworks[0].Dependencies.Should().HaveCount(2);
    ////     result[0].Frameworks[0].Dependencies.Should().ContainSingle(x => x.Identifier == "library1" && !x.IsTransitive);
    ////     result[0].Frameworks[0].Dependencies.Should().ContainSingle(x => x.Identifier == "library2" && x.IsTransitive);
    ////     result[0].Frameworks[0].Dependencies.Single(x => x.Identifier == "library2").Via.Should().ContainSingle(x => x.Identifier == "library1");
    //// }

    //// [Fact]
    //// public void Run_WithProjectDependencyGroupAndFilteredTransitiveDependencies_ReturnsProject()
    //// {
    ////     // Arrange
    ////     const string projectOrSolutionPath = "path/to/project";
    ////     const string projectName = "projectName";
    ////     const string targetFramework = "net5.0";
    ////     var projects = new[] { new PackageSpec(projectOrSolutionPath) { Name = projectName } };
    ////     var assetsFile = new LockFile
    ////     {
    ////         Targets = new List<LockFileTarget>
    ////             {
    ////                 new LockFileTarget
    ////                 {
    ////                     TargetFramework = targetFramework,
    ////                     Libraries = new List<LockFileTargetLibrary>
    ////                     {
    ////                         new LockFileTargetLibrary
    ////                         {
    ////                             Name = "library1",
    ////                             Version = "1.0.0",
    ////                             Dependencies = new List<PackageDependency>
    ////                             {
    ////                                 new PackageDependency("dependency1", VersionRange.Parse("1.0.0"))
    ////                             }
    ////                         },
    ////                         new LockFileTargetLibrary
    ////                         {
    ////                             Name = "library2",
    ////                             Version = "1.0.0",
    ////                             Dependencies = new List<PackageDependency>
    ////                             {
    ////                                 new PackageDependency("library1", VersionRange.Parse("1.0.0"))
    ////                             }
    ////                         }
    ////                     }
    ////                 }
    ////             },
    ////         ProjectFileDependencyGroups = new List<ProjectFileDependencyGroup>
    ////             {
    ////                 new ProjectFileDependencyGroup(targetFramework, new[] { "dependency1" })
    ////             }
    ////     };
    ////     this.dependencyGraphMock.Setup(x => x.Create(projectOrSolutionPath)).Returns(new DependencyGraphSpec(projects));
    ////     this.assetsMock.Setup(x => x.Create(projectOrSolutionPath, It.IsAny<string>())).Returns(assetsFile);

    ////     // Act
    ////     var result = this.dependencyFinder.Run(projectOrSolutionPath, false, new Regex("^library1$"));

    ////     // Assert
    ////     result.Should().ContainSingle();
    ////     result[0].Name.Should().Be(projectName);
    ////     result[0].Frameworks.Should().ContainSingle();
    ////     result[0].Frameworks[0].Name.Should().Be(targetFramework);
    ////     result[0].Frameworks[0].Dependencies.Should().ContainSingle(x => x.Identifier == "library1" && !x.IsTransitive);
    ////     result[0].Frameworks[0].Dependencies.Should().NotContain(x => x.Identifier == "library2");
    //// }

    //// [Fact]
    //// public void Run_WithCollateAllDependencies_ReturnsAllDependencies()
    //// {
    ////     // Arrange
    ////     const string projectOrSolutionPath = "path/to/project";
    ////     const string projectName = "projectName";
    ////     const string targetFramework = "net5.0";
    ////     var projects = new[] { new PackageSpec(projectOrSolutionPath) { Name = projectName } };
    ////     var assetsFile = new LockFile
    ////     {
    ////         Targets = new List<LockFileTarget>
    ////             {
    ////                 new LockFileTarget
    ////                 {
    ////                     TargetFramework = targetFramework,
    ////                     Libraries = new List<LockFileTargetLibrary>
    ////                     {
    ////                         new LockFileTargetLibrary
    ////                         {
    ////                             Name = "library1",
    ////                             Version = "1.0.0",
    ////                             Dependencies = new List<PackageDependency>
    ////                             {
    ////                                 new PackageDependency("dependency1", VersionRange.Parse("1.0.0"))
    ////                             }
    ////                         },
    ////                         new LockFileTargetLibrary
    ////                         {
    ////                             Name = "library2",
    ////                             Version = "1.0.0",
    ////                             Dependencies = new List<PackageDependency>
    ////                             {
    ////                                 new PackageDependency("library1", VersionRange.Parse("1.0.0"))
    ////                             }
    ////                         }
    ////                     }
    ////                 }
    ////             },
    ////         ProjectFileDependencyGroups = new List<ProjectFileDependencyGroup>
    ////             {
    ////                 new ProjectFileDependencyGroup(targetFramework, new[] { "dependency1" })
    ////             }
    ////     };
    ////     this.dependencyGraphMock.Setup(x => x.Create(projectOrSolutionPath)).Returns(new DependencyGraphSpec(projects));
    ////     this.assetsMock.Setup(x => x.Create(projectOrSolutionPath, It.IsAny<string>())).Returns(assetsFile);

    ////     // Act
    ////     var result = this.dependencyFinder.Run(projectOrSolutionPath, true, null);

    ////     // Assert
    ////     result.Should().ContainSingle();
    ////     result[0].Name.Should().Be(projectName);
    ////     result[0].Frameworks.Should().ContainSingle();
    ////     result[0].Frameworks[0].Name.Should().Be(targetFramework);
    ////     result[0].Frameworks[0].Dependencies.Should().HaveCount(2);
    ////     result[0].Frameworks[0].Dependencies.Should().ContainSingle(x => x.Identifier == "library1" && !x.IsTransitive);
    ////     result[0].Frameworks[0].Dependencies.Should().ContainSingle(x => x.Identifier == "library2" && x.IsTransitive);
    ////     result[0].Frameworks[0].Dependencies.Single(x => x.Identifier == "library2").Via.Should().ContainSingle(x => x.Identifier == "library1");
    //// }

    //// [Fact]
    //// public void CreateProjects_ReturnsPackageReferenceProjects()
    //// {
    ////     // Arrange
    ////     const string projectOrSolutionPath = "path/to/project";
    ////     var projects = new[]
    ////     {
    ////             new PackageSpec(projectOrSolutionPath) { RestoreMetadata = new ProjectRestoreMetadata { ProjectStyle = ProjectStyle.PackageReference } },
    ////             new PackageSpec(projectOrSolutionPath) { RestoreMetadata = new ProjectRestoreMetadata { ProjectStyle = ProjectStyle.DotnetCliTool } }
    ////         };
    ////     this.dependencyGraphMock.Setup(x => x.Create(projectOrSolutionPath)).Returns(new DependencyGraphSpec(projects));

    ////     // Act
    ////     var result = this.dependencyFinder.Run(projectOrSolutionPath, false, null);

    ////     // Assert
    ////     result.Should().ContainSingle();
    ////     result[0].Name.Should().Be(projectOrSolutionPath);
    ////     result[0].Frameworks.Should().BeEmpty();
    //// }

    //// [Fact]
    //// public void CreateProjectDependencyGraph_ReturnsDependencyGraphSpec()
    //// {
    ////     // Arrange
    ////     const string projectOrSolutionPath = "path/to/project";
    ////     var projects = new[] { new PackageSpec(projectOrSolutionPath) };
    ////     this.dependencyGraphMock.Setup(x => x.Create(projectOrSolutionPath)).Returns(new DependencyGraphSpec(projects));

    ////     // Act
    ////     var result = this.dependencyFinder.Run(projectOrSolutionPath, false, null);

    ////     // Assert
    ////     result.Should().ContainSingle();
    ////     result[0].Name.Should().Be(projectOrSolutionPath);
    ////     result[0].Frameworks.Should().BeEmpty();
    //// }

    //// [Fact]
    //// public void CreateAssetsFile_ReturnsLockFile()
    //// {
    ////     // Arrange
    ////     const string projectOrSolutionPath = "path/to/project";
    ////     const string project = new PackageSpec(projectOrSolutionPath);
    ////     const string outputPath = "output/path";
    ////     this.assetsMock.Setup(x => x.Create(projectOrSolutionPath, outputPath)).Returns(new LockFile());

    ////     // Act
    ////     var result = this.dependencyFinder.Run(projectOrSolutionPath, false, null);

    ////     // Assert
    ////     result.Should().ContainSingle();
    ////     result[0].Name.Should().Be(projectOrSolutionPath);
    ////     result[0].Frameworks.Should().BeEmpty();
    //// }

    //// [Fact]
    //// public void PopulateDependencies_PopulatesDependencies()
    //// {
    ////     // Arrange
    ////     var libraries = new Dictionary<string, LockFileTargetLibrary>
    ////         {
    ////             { "library1", new LockFileTargetLibrary { Name = "library1", Version = "1.0.0" } },
    ////             { "library2", new LockFileTargetLibrary { Name = "library2", Version = "1.0.0", Dependencies = new List<PackageDependency> { new PackageDependency("library1", VersionRange.Parse("1.0.0")) } } }
    ////         };

    ////     // Act
    ////     this.dependencyFinder.Run("path/to/project", false, null);
    ////     this.dependencyFinder["dependencies"].Should().ContainKey("library1");
    ////     this.dependencyFinder["dependencies"].Should().ContainKey("library2");
    ////     this.dependencyFinder["dependencies"]["library1"].Identifier.Should().Be("library1");
    ////     this.dependencyFinder["dependencies"]["library1"].Version.Should().Be("1.0.0");
    ////     this.dependencyFinder["dependencies"]["library1"].Via.Should().BeEmpty();
    ////     this.dependencyFinder["dependencies"]["library2"].Identifier.Should().Be("library2");
    ////     this.dependencyFinder["dependencies"]["library2"].Version.Should().Be("1.0.0");
    ////     this.dependencyFinder["dependencies"]["library2"].Via.Should().BeEmpty();
    //// }

    //// [Fact]
    //// public void RecordDependency_RecordsDependency()
    //// {
    ////     // Arrange
    ////     var library = new LockFileTargetLibrary
    ////     {
    ////         Name = "library",
    ////         Version = "1.0.0",
    ////         Dependencies = new List<PackageDependency> { new PackageDependency("dependency", VersionRange.Parse("1.0.0")) }
    ////     };
    ////     var libraries = new Dictionary<string, LockFileTargetLibrary>
    ////         {
    ////             { "library", library },
    ////             { "dependency", new LockFileTargetLibrary { Name = "dependency", Version = "1.0.0" } }
    ////         };

    ////     // Act
    ////     this.dependencyFinder.Run("path/to/project", false, null);
    ////     this.dependencyFinder["dependencies"]["library"].Via.Should().BeEmpty();
    ////     this.dependencyFinder.RecordDependency(library, null, libraries);
    ////     this.dependencyFinder["dependencies"]["library"].Via.Should().BeEmpty();
    ////     this.dependencyFinder.RecordDependency(libraries["dependency"], this.dependencyFinder["dependencies"]["library"], libraries);
    ////     this.dependencyFinder["dependencies"]["library"].Via.Should().ContainSingle();
    ////     this.dependencyFinder["dependencies"]["library"].Via[0].Identifier.Should().Be("dependency");
    //// }

    //// [Fact]
    //// public void FindTransitiveDependencies_ReturnsTransitiveDependencies()
    //// {
    ////     // Arrange
    ////     var framework = new TargetFrameworkInformation(".NETCoreApp,Version=v3.1", "netcoreapp3.1");
    ////     var dependencies = new List<LockFileTargetLibrary>
    ////     {
    ////     }
    //// }
}
