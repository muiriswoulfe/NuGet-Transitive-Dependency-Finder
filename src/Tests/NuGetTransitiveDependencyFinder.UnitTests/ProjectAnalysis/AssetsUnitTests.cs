// <copyright file="AssetsUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.ProjectAnalysis
{
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Moq;
    using NuGet.ProjectModel;
    using NuGetTransitiveDependencyFinder.ProjectAnalysis;
    using NuGetTransitiveDependencyFinder.TestUtilities.Globalization;

    /// <summary>
    /// Unit tests for the <see cref="Assets"/> class.
    /// </summary>
    public class AssetsUnitTests
    {
        /// <summary>
        /// Tests that when <see cref="Assets.CreateAsync(string, string)"/> is called, it performs the expected
        /// operations.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [AllCulturesFact]
        public async Task CreateAsync_WithDifferentValues_ReturnsCorrectValues()
        {
            // Arrange
            var dotNetRunner = new Mock<IDotNetRunner>();
            var lockFileUtilitiesWrapper = new Mock<ILockFileUtilitiesWrapper>();
            var lockFile = new LockFile();
            _ = lockFileUtilitiesWrapper.Setup(mock => mock.GetLockFile(It.IsAny<string>())).Returns(lockFile);
            var assets = new Assets(dotNetRunner.Object, lockFileUtilitiesWrapper.Object);

            // Act
            var result = await assets.CreateAsync("/input", "/output").ConfigureAwait(false);

            // Assert
            _ = result
                .Should().Equals(lockFile);
            var pathSeparator = Path.PathSeparator.ToString(CultureInfo.InvariantCulture);
            dotNetRunner.Verify(mock => mock.RunAsync(@"restore ""/input""", pathSeparator), Times.Once);
            lockFileUtilitiesWrapper.Verify(mock => mock.GetLockFile("/output/project.assets.json"), Times.Once);
        }
    }
}
