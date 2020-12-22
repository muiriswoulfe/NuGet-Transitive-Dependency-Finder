// <copyright file="CommandLineOptionsUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ConsoleApp.UnitTests.Input
{
    using FluentAssertions;
    using NuGetTransitiveDependencyFinder.ConsoleApp.Input;
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
        [Theory]
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
            _ = value.Should().Be(commandLineOptions.All);
        }

        /// <summary>
        /// Tests that when <see cref="CommandLineOptions.ProjectOrSolution"/> is called after being set, it returns the
        /// value specified.
        /// </summary>
        /// <param name="value">The value of <see cref="CommandLineOptions.ProjectOrSolution"/>.</param>
        [Theory]
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
            _ = value.Should().Be(commandLineOptions.ProjectOrSolution);
        }
    }
}
