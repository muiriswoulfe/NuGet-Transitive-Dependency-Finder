// <copyright file="ProgramRunnerUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.ConsoleApp.Process
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.Extensions.Logging;
    using Moq;
    using NuGetTransitiveDependencyFinder.ConsoleApp.Input;
    using NuGetTransitiveDependencyFinder.ConsoleApp.Output;
    using NuGetTransitiveDependencyFinder.ConsoleApp.Process;
    using NuGetTransitiveDependencyFinder.ConsoleApp.Resources.Messages;
    using NuGetTransitiveDependencyFinder.Output;
    using NuGetTransitiveDependencyFinder.TestUtilities.Globalization;
    using NuGetTransitiveDependencyFinder.TestUtilities.Logging;
    using NuGetTransitiveDependencyFinder.UnitTests.ConsoleApp.TestUtilities;

    /// <summary>
    /// Unit tests for the <see cref="ProgramRunner"/> class.
    /// </summary>
    public class ProgramRunnerUnitTests
    {
        /// <summary>
        /// The mock <see cref="ICommandLineOptions"/> object.
        /// </summary>
        private readonly Mock<ICommandLineOptions> commandLineOptions = new();

        /// <summary>
        /// The mock <see cref="IDependencyWriter"/> object.
        /// </summary>
        private readonly Mock<IDependencyWriter> dependencyWriter = new();

        /// <summary>
        /// The mock <see cref="ILogger{ProgramRunner}"/> object.
        /// </summary>
        private readonly MockLogger<ProgramRunner> logger = new();

        /// <summary>
        /// The mock <see cref="ITransitiveDependencyFinder"/> object.
        /// </summary>
        private readonly Mock<ITransitiveDependencyFinder> transitiveDependencyFinder = new();

        /// <summary>
        /// Tests that when <see cref="ProgramRunner.RunAsync()"/> is called, it performs the expected actions.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [AllCulturesFact]
        public async Task RunAsync_Called_PerformsExpectedActions()
        {
            // Arrange
            var projects = InternalAccessor.Construct<Projects>(0);
            _ = this.commandLineOptions
                .SetupGet(mock => mock.ProjectOrSolution)
                .Returns("ProjectOrSolution");
            _ = this.commandLineOptions
                .SetupGet(mock => mock.All)
                .Returns(true);
            _ = this.transitiveDependencyFinder
                .Setup(mock => mock.RunAsync("ProjectOrSolution", true))
                .Returns(Task.FromResult(projects));
            var programRunner = new ProgramRunner(
                this.commandLineOptions.Object,
                this.dependencyWriter.Object,
                this.logger,
                this.transitiveDependencyFinder.Object);

            // Act
            await programRunner.RunAsync().ConfigureAwait(false);

            // Arrange
            _ = this.logger.Entries
                .Should().HaveCount(1);
            _ = this.logger.Entries[0].LogLevel
                .Should().Be(LogLevel.Information);
            _ = this.logger.Entries[0].Message
                .Should().Be(Information.CommencingAnalysis);
            this.commandLineOptions.VerifyGet(mock => mock.ProjectOrSolution, Times.Once);
            this.commandLineOptions.VerifyGet(mock => mock.All, Times.Once);
            this.commandLineOptions.VerifyNoOtherCalls();
            this.transitiveDependencyFinder.Verify(mock => mock.RunAsync("ProjectOrSolution", true), Times.Once);
            this.transitiveDependencyFinder.VerifyNoOtherCalls();
            this.dependencyWriter.Verify(mock => mock.Write(projects), Times.Once);
            this.dependencyWriter.VerifyNoOtherCalls();
        }
    }
}
