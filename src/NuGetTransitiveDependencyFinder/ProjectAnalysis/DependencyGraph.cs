// <copyright file="DependencyGraph.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ProjectAnalysis
{
    using System;
    using System.IO;
    using Microsoft.Extensions.Logging;
    using NuGet.ProjectModel;
    using static System.FormattableString;

    /// <summary>
    /// A class representing a dependency graph of .NET projects and their NuGet dependencies.
    /// </summary>
    internal sealed class DependencyGraph : IDisposable
    {
        /// <summary>
        /// The logger factory from which a logger will be constructed.
        /// </summary>
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// A temporary file for storing the dependency graph information.
        /// </summary>
        /// <remarks>This file will be deleted when <see cref="Dispose()"/> is invoked.</remarks>
        private readonly string filePath;

        /// <summary>
        /// The path of the directory containing the .NET project or solution file.
        /// </summary>
        private readonly string projectOrSolutionDirectory;

        /// <summary>
        /// The command-line parameters for the .NET process.
        /// </summary>
        private readonly string arguments;

        /// <summary>
        /// A value tracking whether <see cref="Dispose()"/> has been invoked.
        /// </summary>
        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyGraph"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory from which a logger will be constructed.</param>
        /// <param name="projectOrSolutionPath">The path of the .NET project or solution file, including the file
        /// name.</param>
        public DependencyGraph(ILoggerFactory loggerFactory, string projectOrSolutionPath)
        {
            this.loggerFactory = loggerFactory;
            this.filePath = Path.GetTempFileName();
            this.projectOrSolutionDirectory = Path.GetDirectoryName(projectOrSolutionPath) !;

            this.arguments =
                Invariant($"msbuild \"{projectOrSolutionPath}\" /maxCpuCount /target:GenerateRestoreGraphFile ") +
                Invariant($"/property:RestoreGraphOutputPath=\"{this.filePath}\"");
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DependencyGraph"/> class.
        /// </summary>
        ~DependencyGraph() =>
            this.Dispose(false);

        /// <summary>
        /// Creates a <see cref="DependencyGraphSpec"/> object representing the project dependency graph.
        /// </summary>
        /// <returns>The <see cref="DependencyGraphSpec"/> object.</returns>
        public DependencyGraphSpec Create()
        {
            var dotNetRunner = new DotNetRunner(this.loggerFactory, this.arguments, this.projectOrSolutionDirectory);
            dotNetRunner.Run();

            return DependencyGraphSpec.Load(this.filePath);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of the resources maintained by the current object.
        /// </summary>
        /// <param name="disposing">A value indicating whether disposal should be performed for managed as well as
        /// unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (this.disposedValue)
            {
                return;
            }

            if (disposing)
            {
                File.Delete(this.filePath);
            }

            this.disposedValue = true;
        }
    }
}
