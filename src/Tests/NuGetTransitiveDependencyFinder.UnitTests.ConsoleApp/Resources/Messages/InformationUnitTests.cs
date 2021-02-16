// <copyright file="InformationUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.ConsoleApp.Resources.Messages
{
    using FluentAssertions;
    using NuGetTransitiveDependencyFinder.ConsoleApp.Resources.Messages;
    using NuGetTransitiveDependencyFinder.TestUtilities.Globalization;

    /// <summary>
    /// Unit tests for the <see cref="Information"/> class.
    /// </summary>
    public class InformationUnitTests
    {
        /// <summary>
        /// Tests that when <see cref="Information.CommencingAnalysis"/> is called, it returns the expected string.
        /// </summary>
        [AllCulturesFact]
        public void CommencingAnalysis_Called_ReturnsExpectedString()
        {
            // Act
            var result = Information.CommencingAnalysis;

            // Assert
            _ = result.Should().Be("Commencing analysis...");
        }

        /// <summary>
        /// Tests that when <see cref="Information.NoDependencies"/> is called, it returns the expected string.
        /// </summary>
        [AllCulturesFact]
        public void NoDependencies_Called_ReturnsExpectedString()
        {
            // Act
            var result = Information.NoDependencies;

            // Assert
            _ = result.Should().Be("No NuGet dependencies found.");
        }

        /// <summary>
        /// Tests that when <see cref="Information.TransitiveDependency"/> is called, it returns the expected string.
        /// </summary>
        [AllCulturesFact]
        public void TransitiveDependency_Called_ReturnsExpectedString()
        {
            // Act
            var result = Information.TransitiveDependency;

            // Assert
            _ = result.Should().Be("{0} (Transitive)");
        }
    }
}
