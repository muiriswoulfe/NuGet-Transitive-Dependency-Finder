// <copyright file="ProgramRunner.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ConsoleApp.Process
{
    using Microsoft.Extensions.Logging;
    using NuGetTransitiveDependencyFinder;
    using NuGetTransitiveDependencyFinder.ConsoleApp.Input;
    using NuGetTransitiveDependencyFinder.ConsoleApp.Output;
    using NuGetTransitiveDependencyFinder.ConsoleApp.Resources.Messages;

    /// <summary>
    /// The class defining the flow of the main application logic.
    /// </summary>
    internal class ProgramRunner : IProgramRunner
    {
        /// <summary>
        /// The command-line options.
        /// </summary>
        private readonly ICommandLineOptions commandLineOptions;

        /// <summary>
        /// The writer for NuGet dependency information.
        /// </summary>
        private readonly IDependencyWriter dependencyWriter;

        /// <summary>
        /// The logger object to which to write the output.
        /// </summary>
        private readonly ILogger<ProgramRunner> logger;

        /// <summary>
        /// The object that manages the overall process of finding transitive NuGet dependencies.
        /// </summary>
        private readonly ITransitiveDependencyFinder transitiveDependencyFinder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramRunner"/> class.
        /// </summary>
        /// <param name="commandLineOptions">The command-line options.</param>
        /// <param name="dependencyWriter">The writer for NuGet dependency information.</param>
        /// <param name="logger">The logger object to which to write the output.</param>
        /// <param name="transitiveDependencyFinder">The object that manages the overall process of finding transitive
        /// NuGet dependencies.</param>
        public ProgramRunner(
            ICommandLineOptions commandLineOptions,
            IDependencyWriter dependencyWriter,
            ILogger<ProgramRunner> logger,
            ITransitiveDependencyFinder transitiveDependencyFinder)
        {
            this.commandLineOptions = commandLineOptions;
            this.dependencyWriter = dependencyWriter;
            this.logger = logger;
            this.transitiveDependencyFinder = transitiveDependencyFinder;
        }

        /// <inheritdoc/>
        public void Run()
        {
            this.logger.LogInformation(Information.CommencingAnalysis);

            var projects =
                this.transitiveDependencyFinder.Run(
                    this.commandLineOptions.ProjectOrSolution!,
                    this.commandLineOptions.All);

            this.dependencyWriter.Write(projects);
        }
    }
}
