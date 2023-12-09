// <copyright file="ProgramRunner.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ConsoleApp.Process;

using Microsoft.Extensions.Logging;
using NuGetTransitiveDependencyFinder;
using NuGetTransitiveDependencyFinder.ConsoleApp.Input;
using NuGetTransitiveDependencyFinder.ConsoleApp.Output;
using NuGetTransitiveDependencyFinder.ConsoleApp.Resources.Messages;

/// <summary>
/// The class defining the flow of the main application logic.
/// </summary>
/// <param name="commandLineOptions">The command-line options.</param>
/// <param name="dependencyWriter">The writer for NuGet dependency information.</param>
/// <param name="logger">The logger object to which to write the output.</param>
/// <param name="transitiveDependencyFinder">The object that manages the overall process of finding transitive NuGet
/// dependencies.</param>
internal class ProgramRunner(
    ICommandLineOptions commandLineOptions,
    IDependencyWriter dependencyWriter,
    ILogger<ProgramRunner> logger,
    ITransitiveDependencyFinder transitiveDependencyFinder) : IProgramRunner
{
    /// <inheritdoc/>
    public void Run()
    {
        logger.LogInformation(Information.CommencingAnalysis);

        var projects =
            transitiveDependencyFinder.Run(
                commandLineOptions.ProjectOrSolution,
                commandLineOptions.All,
                commandLineOptions.Filter);

        dependencyWriter.Write(projects);
    }
}
