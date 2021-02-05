// <copyright file="ProgramRunner.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ConsoleApp
{
    using Microsoft.Extensions.Logging;
    using NuGetTransitiveDependencyFinder;
    using NuGetTransitiveDependencyFinder.ConsoleApp.Input;
    using NuGetTransitiveDependencyFinder.ConsoleApp.Output;
    using NuGetTransitiveDependencyFinder.ConsoleApp.Resources.Messages;

    /// <summary>
    /// The class defining the flow of the main application logic.
    /// </summary>
    internal class ProgramRunner
    {
        /// <summary>
        /// The command-line options.
        /// </summary>
        private readonly CommandLineOptions commandLineOptions;

        /// <summary>
        /// The writer for NuGet dependency information.
        /// </summary>
        private readonly DependencyWriter dependencyWriter;

        /// <summary>
        /// The logger object to which to write the output.
        /// </summary>
        private readonly ILogger<ProgramRunner> logger;

        /// <summary>
        /// The object that manages the overall process of finding transitive NuGet dependencies.
        /// </summary>
        private readonly TransitiveDependencyFinder transitiveDependencyFinder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramRunner"/> class.
        /// </summary>
        /// <param name="commandLineOptions">The command-line options.</param>
        /// <param name="dependencyWriter">The writer for NuGet dependency information.</param>
        /// <param name="logger">The logger object to which to write the output.</param>
        /// <param name="transitiveDependencyFinder">The object that manages the overall process of finding transitive
        /// NuGet dependencies.</param>
        public ProgramRunner(
            CommandLineOptions commandLineOptions,
            DependencyWriter dependencyWriter,
            ILogger<ProgramRunner> logger,
            TransitiveDependencyFinder transitiveDependencyFinder)
        {
            this.commandLineOptions = commandLineOptions;
            this.dependencyWriter = dependencyWriter;
            this.logger = logger;
            this.transitiveDependencyFinder = transitiveDependencyFinder;
        }

        /// <summary>
        /// Runs the main application logic.
        /// </summary>
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
