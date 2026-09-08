// <copyright file="CommandLineHelpUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.ConsoleApp.Resources.Messages;

using FluentAssertions;
using NuGetTransitiveDependencyFinder.ConsoleApp.Resources.Messages;
using NuGetTransitiveDependencyFinder.UnitTests.Utilities.Globalization;

/// <summary>
/// Unit tests for the <see cref="CommandLineHelp"/> class.
/// </summary>
public class CommandLineHelpUnitTests
{
    /// <summary>
    /// Tests that when <see cref="CommandLineHelp.All"/> is called, it returns the expected string.
    /// </summary>
    [AllCulturesFact]
    public void All_Called_ReturnsExpectedString()
    {
        // Act
        var result = CommandLineHelp.All;

        // Assert
        _ = result
            .Should().Be(
                "Indicates that all NuGet dependencies, including non-transitive dependencies, should be listed.");
    }

    /// <summary>
    /// Tests that when <see cref="CommandLineHelp.ProjectOrSolution"/> is called, it returns the expected string.
    /// </summary>
    [AllCulturesFact]
    public void ProjectOrSolution_Called_ReturnsExpectedString()
    {
        // Act
        var result = CommandLineHelp.ProjectOrSolution;

        // Assert
        _ = result
            .Should().Be("The file name of the .NET project or solution to analyze.");
    }

    /// <summary>
    /// Tests that when <see cref="CommandLineHelp.Filter"/> is called, it returns a non-empty string, verifying the
    /// accessor resolves the <c>CommandLineHelp_Filter</c> resource and does not regress on the accessor's presence
    /// or resource-lookup behaviour.
    /// </summary>
    [AllCulturesFact]
    public void Filter_Called_ReturnsNonEmptyString()
    {
        // Act
        var result = CommandLineHelp.Filter;

        // Assert
        _ = result
            .Should().NotBeNullOrWhiteSpace();
    }
}
