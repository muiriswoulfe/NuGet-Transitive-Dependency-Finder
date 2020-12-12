// <copyright file="DependencyUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Output
{
    using NuGet.Versioning;
    using NuGetTransitiveDependencyFinder.Output;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="Dependency"/> class.
    /// </summary>
    public class DependencyUnitTests
    {
        /// <summary>
        /// The default identifier.
        /// </summary>
        private const string DefaultIdentifier = "identifier";

        /// <summary>
        /// The default version.
        /// </summary>
        private static readonly NuGetVersion DefaultVersion = new NuGetVersion("1.0.0");

        /// <summary>
        /// Tests that when <see cref="Dependency.Identifier"/> is called after construction, it returns the value
        /// specified.
        /// </summary>
        /// <param name="value">The value of <see cref="Dependency.Identifier"/>.</param>
        [Theory]
        [InlineData("Identifier 1")]
        [InlineData("Identifier 2")]
        public void Identifer_CalledAfterConstruction_ReturnsValue(string value)
        {
            // Arrange & Act
            var dependency = new Dependency(value, DefaultVersion);

            // Assert
            Assert.Equal(value, dependency.Identifier);
        }

        /// <summary>
        /// Tests that when <see cref="Dependency.Version"/> is called after construction, it returns the value
        /// specified.
        /// </summary>
        /// <param name="value">The value of <see cref="Dependency.Version"/>.</param>
        [Theory]
        [InlineData("1.0.0")]
        [InlineData("2.0.0")]
        public void Version_CalledAfterConstruction_ReturnsValue(string value)
        {
            // Arrange & Act
            var dependency = new Dependency(DefaultIdentifier, new NuGetVersion(value));

            // Assert
            Assert.Equal(value, dependency.Version.ToString());
        }

        /// <summary>
        /// Tests that when <see cref="Dependency.IsTransitive"/> is called after being set, it returns the value
        /// specified.
        /// </summary>
        /// <param name="value">The value of <see cref="Dependency.IsTransitive"/>.</param>
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsTransitive_CalledAfterSetting_ReturnsValue(bool value)
        {
            // Arrange & Act
            var dependency = new Dependency(DefaultIdentifier, DefaultVersion)
            {
                IsTransitive = value,
            };

            // Assert
            Assert.Equal(value, dependency.IsTransitive);
        }
    }
}
