// <copyright file="CommandLineOptionsUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.ConsoleApp.Input
{
    using FluentAssertions;
    using NuGetTransitiveDependencyFinder.ConsoleApp.Input;
    using NuGetTransitiveDependencyFinder.TestUtilities.Globalization;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="CommandLineOptions"/> class.
    /// </summary>
    public class CommandLineOptionsUnitTests
    {
        /// <summary>
        /// Tests that when <see cref="CommandLineOptions.All"/> is called after being set, it returns the value
        /// specified.
        /// </summary>
        /// <param name="value">The value of <see cref="CommandLineOptions.All"/>.</param>
        [CultureTheory]
        [InlineData(true)]
        [InlineData(false)]
        public void All_AfterSetting_ReturnsValue(bool value)
        {
            // Arrange & Act
            var commandLineOptions = new CommandLineOptions
            {
                All = value,
            };

            // Assert
            _ = commandLineOptions.All.Should().Be(value);
        }

        /// <summary>
        /// Tests that when <see cref="CommandLineOptions.ProjectOrSolution"/> is called after being set, it returns the
        /// value specified.
        /// </summary>
        /// <param name="value">The value of <see cref="CommandLineOptions.ProjectOrSolution"/>.</param>
        [CultureTheory]
        [InlineData(null)]
        [InlineData("ProjectOrSolution")]
        public void ProjectOrSolution_AfterSetting_ReturnsValue(string value)
        {
            // Arrange & Act
            var commandLineOptions = new CommandLineOptions
            {
                ProjectOrSolution = value,
            };

            // Assert
            _ = commandLineOptions.ProjectOrSolution.Should().Be(value);
        }
    }
}
