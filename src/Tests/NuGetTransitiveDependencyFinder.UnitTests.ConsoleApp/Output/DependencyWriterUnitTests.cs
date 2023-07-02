// <copyright file="DependencyWriterUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.ConsoleApp.Output;

using System;
using System.Globalization;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NuGet.Frameworks;
using NuGet.Versioning;
using NuGetTransitiveDependencyFinder.ConsoleApp.Output;
using NuGetTransitiveDependencyFinder.ConsoleApp.Resources.Messages;
using NuGetTransitiveDependencyFinder.Output;
using NuGetTransitiveDependencyFinder.TestUtilities.Globalization;
using NuGetTransitiveDependencyFinder.TestUtilities.Logging;
using NuGetTransitiveDependencyFinder.UnitTests.ConsoleApp.TestUtilities;
using static System.FormattableString;

/// <summary>
/// Unit tests for the <see cref="DependencyWriter"/> class.
/// </summary>
public class DependencyWriterUnitTests
{
    /// <summary>
    /// The default project names for use within the unit tests.
    /// </summary>
    private static readonly string[] ProjectNames =
    {
        "Project A",
        "Project B",
    };

    /// <summary>
    /// The default framework names for use within the unit tests.
    /// </summary>
    private static readonly string[] FrameworkNames =
    {
        "Framework A",
        "Framework B",
    };

    /// <summary>
    /// The default dependency names for use within the unit tests.
    /// </summary>
    private static readonly string[] DependencyNames =
    {
        "Dependency A",
        "Dependency B",
    };

    /// <summary>
    /// The default framework identifiers for use within the unit tests.
    /// </summary>
    private static readonly NuGetFramework[] FrameworkIdentifiers =
    {
        new(FrameworkNames[0], new(2, 1, 0, 0)),
        new(FrameworkNames[1], new(3, 2, 1, 2)),
    };

    /// <summary>
    /// The default dependency versions for use within the unit tests.
    /// </summary>
    private static readonly NuGetVersion[] DependencyVersions =
    {
        new(4, 3, 0, 0),
        new(5, 4, 1, 2),
    };

    /// <summary>
    /// The frameworks expected to be returned by the <see cref="DependencyWriter"/>.
    /// </summary>
    private static readonly string[] ExpectedFrameworks =
    {
        Invariant($"    {FrameworkNames[0]} v2.1"),
        Invariant($"    {FrameworkNames[1]} v3.2.1.2"),
    };

    /// <summary>
    /// The dependencies expected to be returned by the <see cref="DependencyWriter"/>.
    /// </summary>
    private static readonly string[] ExpectedDependencies =
    {
        Invariant($"        {DependencyNames[0]} v4.3.0"),
        Invariant($"        {DependencyNames[1]} v5.4.1.2"),
    };

    /// <summary>
    /// The mock <see cref="ILogger{DependencyWriter}"/> object.
    /// </summary>
    private readonly MockLogger<DependencyWriter> logger = new();

    /// <summary>
    /// Tests that when <see cref="DependencyWriter.Write(Projects)"/> is called with no dependencies, it writes the
    /// expected output.
    /// </summary>
    [AllCulturesFact]
    public void Write_WithNoDependencies_WritesExpectedOutput()
    {
        // Arrange
        var projects = InternalAccessor.Construct<Projects>(0);
        var dependencyWriter = new DependencyWriter(this.logger);

        // Act
        dependencyWriter.Write(projects);

        // Arrange
        _ = this.logger.Entries
            .Should().HaveCount(1);
        _ = this.logger.Entries[0].LogLevel
            .Should().Be(LogLevel.Information);
        _ = this.logger.Entries[0].Message
            .Should().Be(Information.NoDependencies);
    }

    /// <summary>
    /// Tests that when <see cref="DependencyWriter.Write(Projects)"/> is called with a <see cref="Project"/> containing
    /// no children, it writes the expected output.
    /// </summary>
    [AllCulturesFact]
    public void Write_WithProjectContainingNoChildren_WritesExpectedOutput()
    {
        // Arrange
        var project = InternalAccessor.Construct<Project>(ProjectNames[0], 0);
        var projects = InternalAccessor.Construct<Projects>(1);
        projects.Add(project);
        var dependencyWriter = new DependencyWriter(this.logger);

        // Act
        dependencyWriter.Write(projects);

        // Arrange
        _ = this.logger.Entries
            .Should().HaveCount(1);
        _ = this.logger.Entries[0].LogLevel
            .Should().Be(LogLevel.Information);
        _ = this.logger.Entries[0].Message
            .Should().Be(Information.NoDependencies);
    }

    /// <summary>
    /// Tests that when <see cref="DependencyWriter.Write(Projects)"/> is called with a <see cref="Framework"/>
    /// containing no children, it writes the expected output.
    /// </summary>
    [AllCulturesFact]
    public void Write_WithFrameworkContainingNoChildren_WritesExpectedOutput()
    {
        // Arrange
        var framework = InternalAccessor.Construct<Framework>(FrameworkIdentifiers[0], Array.Empty<Dependency>());
        var project = InternalAccessor.Construct<Project>(ProjectNames[0], 1);
        var projects = InternalAccessor.Construct<Projects>(1);
        project.Add(framework);
        projects.Add(project);
        var dependencyWriter = new DependencyWriter(this.logger);

        // Act
        dependencyWriter.Write(projects);

        // Arrange
        _ = this.logger.Entries
            .Should().HaveCount(1);
        _ = this.logger.Entries[0].LogLevel
            .Should().Be(LogLevel.Information);
        _ = this.logger.Entries[0].Message
            .Should().Be(Information.NoDependencies);
    }

    /// <summary>
    /// Tests that when <see cref="DependencyWriter.Write(Projects)"/> is called with a simple collection of
    /// dependencies, it writes the expected output.
    /// </summary>
    [AllCulturesFact]
    public void Write_WithSimpleDependencies_WritesExpectedOutput()
    {
        // Arrange
        var dependency = InternalAccessor.Construct<Dependency>(DependencyNames[0], DependencyVersions[0]);
        var framework = InternalAccessor.Construct<Framework>(FrameworkIdentifiers[0], new[] { dependency });
        var project = InternalAccessor.Construct<Project>(ProjectNames[0], 1);
        var projects = InternalAccessor.Construct<Projects>(1);
        project.Add(framework);
        projects.Add(project);
        var dependencyWriter = new DependencyWriter(this.logger);

        // Act
        dependencyWriter.Write(projects);

        // Arrange
        _ = this.logger.Entries
            .Should().HaveCount(4);
        _ = this.logger.Entries[0].LogLevel
            .Should().Be(LogLevel.Information);
        _ = this.logger.Entries[0].Message
            .Should().Be(ProjectNames[0]);
        _ = this.logger.Entries[1].LogLevel
            .Should().Be(LogLevel.Information);
        _ = this.logger.Entries[1].Message
            .Should().Be(ExpectedFrameworks[0]);
        _ = this.logger.Entries[2].LogLevel
            .Should().Be(LogLevel.Debug);
        _ = this.logger.Entries[2].Message
            .Should().Be(ExpectedDependencies[0]);
        _ = this.logger.Entries[3].LogLevel
            .Should().Be(LogLevel.Information);
        _ = this.logger.Entries[3].Message
            .Should().Be(string.Empty);
    }

    /// <summary>
    /// Tests that when <see cref="DependencyWriter.Write(Projects)"/> is called with a simple collection of transitive
    /// dependencies, it writes the expected output.
    /// </summary>
    [AllCulturesFact]
    public void Write_WithSimpleTransitiveDependencies_WritesExpectedOutput()
    {
        // Arrange
        var dependency = InternalAccessor.Construct<Dependency>(DependencyNames[0], DependencyVersions[0]);
        var framework = InternalAccessor.Construct<Framework>(FrameworkIdentifiers[0], new[] { dependency });
        var project = InternalAccessor.Construct<Project>(ProjectNames[0], 1);
        var projects = InternalAccessor.Construct<Projects>(1);
        dependency.SetProperty(nameof(dependency.IsTransitive), true);
        project.Add(framework);
        projects.Add(project);
        var dependencyWriter = new DependencyWriter(this.logger);

        // Act
        dependencyWriter.Write(projects);

        // Arrange
        _ = this.logger.Entries
            .Should().HaveCount(4);
        _ = this.logger.Entries[0].LogLevel
            .Should().Be(LogLevel.Information);
        _ = this.logger.Entries[0].Message
            .Should().Be(ProjectNames[0]);
        _ = this.logger.Entries[1].LogLevel
            .Should().Be(LogLevel.Information);
        _ = this.logger.Entries[1].Message
            .Should().Be(ExpectedFrameworks[0]);
        _ = this.logger.Entries[2].LogLevel
            .Should().Be(LogLevel.Warning);
        _ = this.logger.Entries[2].Message
            .Should().Be(
                string.Format(
                    CultureInfo.CurrentCulture,
                    Information.TransitiveDependency,
                    ExpectedDependencies[0]));
        _ = this.logger.Entries[3].LogLevel
            .Should().Be(LogLevel.Information);
        _ = this.logger.Entries[3].Message
            .Should().Be(string.Empty);
    }

    /// <summary>
    /// Tests that when <see cref="DependencyWriter.Write(Projects)"/> is called with a complex collection of
    /// dependencies, it writes the expected output.
    /// </summary>
    [AllCulturesFact]
    public void Write_WithComplexDependencies_WritesExpectedOutput()
    {
        // Arrange
        var dependency1 = InternalAccessor.Construct<Dependency>(DependencyNames[0], DependencyVersions[0]);
        var dependency2 = InternalAccessor.Construct<Dependency>(DependencyNames[1], DependencyVersions[1]);
        dependency2.SetProperty(nameof(dependency2.IsTransitive), true);
        var dependencies = new[] { dependency1, dependency2 };
        var framework1 = InternalAccessor.Construct<Framework>(FrameworkIdentifiers[0], dependencies);
        var framework2 = InternalAccessor.Construct<Framework>(FrameworkIdentifiers[1], dependencies);
        var project1 = InternalAccessor.Construct<Project>(ProjectNames[0], 2);
        var project2 = InternalAccessor.Construct<Project>(ProjectNames[1], 2);
        var projects = InternalAccessor.Construct<Projects>(2);
        project1.Add(framework1);
        project1.Add(framework2);
        project2.Add(framework1);
        project2.Add(framework2);
        projects.Add(project1);
        projects.Add(project2);
        var dependencyWriter = new DependencyWriter(this.logger);

        // Act
        dependencyWriter.Write(projects);

        // Arrange
        _ = this.logger.Entries
            .Should().HaveCount(16);
        _ = this.logger.Entries[0].LogLevel
            .Should().Be(LogLevel.Information);
        _ = this.logger.Entries[0].Message
            .Should().Be(ProjectNames[0]);
        _ = this.logger.Entries[1].LogLevel
            .Should().Be(LogLevel.Information);
        _ = this.logger.Entries[1].Message
            .Should().Be(ExpectedFrameworks[0]);
        _ = this.logger.Entries[2].LogLevel
            .Should().Be(LogLevel.Debug);
        _ = this.logger.Entries[2].Message
            .Should().Be(ExpectedDependencies[0]);
        _ = this.logger.Entries[3].LogLevel
            .Should().Be(LogLevel.Warning);
        _ = this.logger.Entries[3].Message
            .Should().Be(
                string.Format(
                    CultureInfo.CurrentCulture,
                    Information.TransitiveDependency,
                    ExpectedDependencies[1]));
        _ = this.logger.Entries[4].LogLevel
            .Should().Be(LogLevel.Information);
        _ = this.logger.Entries[4].Message
            .Should().Be(ExpectedFrameworks[1]);
        _ = this.logger.Entries[5].LogLevel
            .Should().Be(LogLevel.Debug);
        _ = this.logger.Entries[5].Message
            .Should().Be(ExpectedDependencies[0]);
        _ = this.logger.Entries[6].LogLevel
            .Should().Be(LogLevel.Warning);
        _ = this.logger.Entries[6].Message
            .Should().Be(
                string.Format(
                    CultureInfo.CurrentCulture,
                    Information.TransitiveDependency,
                    ExpectedDependencies[1]));
        _ = this.logger.Entries[7].LogLevel
            .Should().Be(LogLevel.Information);
        _ = this.logger.Entries[7].Message
            .Should().Be(string.Empty);
        _ = this.logger.Entries[8].LogLevel
            .Should().Be(LogLevel.Information);
        _ = this.logger.Entries[8].Message
            .Should().Be(ProjectNames[1]);
        _ = this.logger.Entries[9].LogLevel
            .Should().Be(LogLevel.Information);
        _ = this.logger.Entries[9].Message
            .Should().Be(ExpectedFrameworks[0]);
        _ = this.logger.Entries[10].LogLevel
            .Should().Be(LogLevel.Debug);
        _ = this.logger.Entries[10].Message
            .Should().Be(ExpectedDependencies[0]);
        _ = this.logger.Entries[11].LogLevel
            .Should().Be(LogLevel.Warning);
        _ = this.logger.Entries[11].Message
            .Should().Be(
                string.Format(
                    CultureInfo.CurrentCulture,
                    Information.TransitiveDependency,
                    ExpectedDependencies[1]));
        _ = this.logger.Entries[12].LogLevel
            .Should().Be(LogLevel.Information);
        _ = this.logger.Entries[12].Message
            .Should().Be(ExpectedFrameworks[1]);
        _ = this.logger.Entries[13].LogLevel
            .Should().Be(LogLevel.Debug);
        _ = this.logger.Entries[13].Message
            .Should().Be(ExpectedDependencies[0]);
        _ = this.logger.Entries[14].LogLevel
            .Should().Be(LogLevel.Warning);
        _ = this.logger.Entries[14].Message
            .Should().Be(
                string.Format(
                    CultureInfo.CurrentCulture,
                    Information.TransitiveDependency,
                    ExpectedDependencies[1]));
        _ = this.logger.Entries[15].LogLevel
            .Should().Be(LogLevel.Information);
        _ = this.logger.Entries[15].Message
            .Should().Be(string.Empty);
    }
}
