// <copyright file="DependencyGraph.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ProjectAnalysis
{
    using System;
    using System.IO;
    using NuGet.ProjectModel;
    using static System.FormattableString;

    /// <summary>
    /// A class representing a dependency graph of .NET projects and their NuGet dependencies.
    /// </summary>
    internal sealed class DependencyGraph : IDependencyGraph
    {
        /// <summary>
        /// The object managing the running of .NET commands on project and solution files.
        /// </summary>
        private readonly IDotNetRunner dotNetRunner;

        /// <summary>
        /// A temporary file for storing the dependency graph information.
        /// </summary>
        /// <remarks>This file will be deleted when <see cref="Dispose()"/> is invoked.</remarks>
        private readonly string filePath;

        /// <summary>
        /// A value tracking whether <see cref="Dispose()"/> has been invoked.
        /// </summary>
        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyGraph"/> class.
        /// </summary>
        /// <param name="dotNetRunner">The object managing the running of .NET commands on project and solution
        /// files.</param>
        public DependencyGraph(IDotNetRunner dotNetRunner)
        {
            this.dotNetRunner = dotNetRunner;

            this.filePath = Path.GetRandomFileName();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DependencyGraph"/> class.
        /// </summary>
        ~DependencyGraph() =>
            this.Dispose(false);

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public DependencyGraphSpec Create(string projectOrSolutionPath)
        {
            var projectOrSolutionDirectory = Path.GetDirectoryName(projectOrSolutionPath)!;
            var arguments =
                Invariant($"msbuild \"{projectOrSolutionPath}\" /maxCpuCount /target:GenerateRestoreGraphFile ") +
                Invariant($"/property:RestoreGraphOutputPath=\"{this.filePath}\"");
            this.dotNetRunner.Run(arguments, projectOrSolutionDirectory);

            return DependencyGraphSpec.Load(this.filePath);
        }

        /// <summary>
        /// Disposes of the resources maintained by the current object.
        /// </summary>
        /// <param name="disposing">A value indicating whether disposal should be performed for managed as well as
        /// unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    File.Delete(this.filePath);
                }

                this.disposedValue = true;
            }
        }
    }
}
