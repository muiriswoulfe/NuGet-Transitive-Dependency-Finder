// <copyright file="StringsUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.ConsoleApp.Resources
{
    using System.Linq;
    using FluentAssertions;
    using NuGetTransitiveDependencyFinder.ConsoleApp.Resources;
    using NuGetTransitiveDependencyFinder.ConsoleApp.Resources.Messages;
    using NuGetTransitiveDependencyFinder.TestUtilities.Globalization;
    using static System.FormattableString;

    /// <summary>
    /// Unit tests for the <see cref="Strings"/> class.
    /// </summary>
    public class StringsUnitTests
    {
        /// <summary>
        /// The collection of all resources in the application.
        /// </summary>
        private static readonly string[] AllResources =
        {
            Invariant($"{nameof(CommandLineHelp)}_{nameof(CommandLineHelp.All)}"),
            Invariant($"{nameof(CommandLineHelp)}_{nameof(CommandLineHelp.ProjectOrSolution)}"),
            Invariant($"{nameof(Information)}_{nameof(Information.CommencingAnalysis)}"),
            Invariant($"{nameof(Information)}_{nameof(Information.NoDependencies)}"),
            Invariant($"{nameof(Information)}_{nameof(Information.TransitiveDependency)}"),
        };

        /// <summary>
        /// Tests that when <see cref="Strings.GetString(string)"/> is called with a valid resource name, it returns a
        /// unique non-<c>null</c> string.
        /// </summary>
        [AllCulturesFact]
        public void GetString_CalledWithValidResourceName_ReturnsUniqueNonNullString()
        {
            // Act
            var result = AllResources.Select(resource => Strings.GetString(resource));

            // Assert
            _ = result.Should().OnlyHaveUniqueItems();
            _ = result.Should().NotContainNulls();
        }

        /// <summary>
        /// Tests that when <see cref="Strings.GetString(string)"/> is called with an invalid resource name, it returns
        /// <c>null</c>.
        /// </summary>
        [AllCulturesFact]
        public void GetString_CalledWithInvalidResourceName_ReturnsNull()
        {
            // Act
            var result = Strings.GetString("Invalid");

            // Assert
            _ = result.Should().BeNull();
        }
    }
}
