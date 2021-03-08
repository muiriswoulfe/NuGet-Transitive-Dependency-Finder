// <copyright file="AssetsUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.ProjectAnalysis
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Moq;
    using NuGetTransitiveDependencyFinder.ProjectAnalysis;
    using NuGetTransitiveDependencyFinder.TestUtilities.Globalization;
    using NuGetTransitiveDependencyFinder.TestUtilities.Logging;
    using NuGetTransitiveDependencyFinder.Utilities;

    /// <summary>
    /// Unit tests for the <see cref="Assets"/> class.
    /// </summary>
    public class AssetsUnitTests
    {
        /// <summary>
        /// Tests that when <see cref="Assets.CreateAsync(string, string)"/> is called with different values,
        /// it returns the correct value each time.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [AllCulturesFact]
        public async Task CreateAsync_WithDifferentValues_ReturnsCorrectValues()
        {
            // Arrange
            var dotNetRunner = new Mock<IDotNetRunner>();
            var logger = new MockLogger<NuGetLogger>();
            var nuGetLogger = new NuGetLogger(logger);
            var assets = new Assets(dotNetRunner.Object, nuGetLogger);

            // Act
            var result = await assets.CreateAsync(@"C:\a", "b").ConfigureAwait(false);

            // Assert
            _ = result
                .Should().BeNull(); // to change
            _ = logger.Entries
                .Should().BeEmpty();
            dotNetRunner.Verify(mock => mock.RunAsync(@"restore ""C:\a""", string.Empty), Times.Once);
        }

        /// <summary>
        /// Tests that when <see cref="Assets.CreateAsync(string, string)"/> is called with different values,
        /// it returns the correct value each time.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [AllCulturesFact]
        public async Task CreateAsync_WithDifferentValues_ReturnsCorrectValues2Async()
        {
            // Arrange
            var dotNetRunner = new Mock<IDotNetRunner>();
            var logger = new MockLogger<NuGetLogger>();
            var nuGetLogger = new NuGetLogger(logger);
            var assets = new Assets(dotNetRunner.Object, nuGetLogger);

            // Act
            var result = await assets.CreateAsync("../../../../../../NuGetTransitiveDependencyFinder.sln", "b2").ConfigureAwait(false);

            // Assert
            _ = result
                .Should().BeNull(); // to change
            _ = logger.Entries
                .Should().BeEmpty();
            dotNetRunner.Verify(mock => mock.RunAsync(@"restore ""../../../../../../NuGetTransitiveDependencyFinder.sln""", "../../../../../.."), Times.Once);
        }
    }
}
