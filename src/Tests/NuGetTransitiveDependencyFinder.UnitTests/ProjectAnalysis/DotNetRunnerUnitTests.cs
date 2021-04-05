// <copyright file="DotNetRunnerUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.ProjectAnalysis
{
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Moq;
    using NuGetTransitiveDependencyFinder.ProjectAnalysis;
    using NuGetTransitiveDependencyFinder.TestUtilities.Globalization;
    using NuGetTransitiveDependencyFinder.TestUtilities.Logging;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="DotNetRunner"/> class.
    /// </summary>
    public class DotNetRunnerUnitTests
    {
        /// <summary>
        /// Tests that when <see cref="DotNetRunner.RunAsync(string, string)"/> is called, it performs the expected
        /// actions.
        /// </summary>
        /// <param name="startReturnValue">The return value from the <see cref="IProcessWrapper.Start()"/> method
        /// call.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [AllCulturesTheory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task RunAsync_CalledWithAnyStartReturnValue_PerformsExpectedActions(bool startReturnValue)
        {
            // Arrange
            var logger = new MockLogger<DotNetRunner>();
            var processWrapper = new Mock<IProcessWrapper>();
            _ = processWrapper
                .SetupProperty(obj => obj.StartInfo)
                .Setup(obj => obj.Start())
                .Returns(startReturnValue);
            var dotNetRunner = new DotNetRunner(logger, processWrapper.Object);

            // Act
            await dotNetRunner.RunAsync("build", @"..\").ConfigureAwait(false);

            // Assert
            _ = logger.Entries
                .Should().BeEmpty();
            _ = processWrapper.Object.StartInfo
                .Should().NotBeNull();
            _ = processWrapper.Object.StartInfo.FileName
                .Should().Be("dotnet");
            _ = processWrapper.Object.StartInfo.Arguments
                .Should().Be("build");
            _ = processWrapper.Object.StartInfo.RedirectStandardError
                .Should().BeTrue();
            _ = processWrapper.Object.StartInfo.RedirectStandardOutput
                .Should().BeTrue();
            _ = processWrapper.Object.StartInfo.WorkingDirectory
                .Should().Equals(@"..\");
            processWrapper.Verify(obj => obj.StartInfo, Times.Exactly(6));
            processWrapper.Verify(obj => obj.Start(), Times.Once());
            processWrapper.Verify(obj => obj.BeginErrorReadLine(), Times.Once());
            processWrapper.Verify(obj => obj.BeginOutputReadLine(), Times.Once());
            processWrapper.Verify(obj => obj.WaitForExitAsync(It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
