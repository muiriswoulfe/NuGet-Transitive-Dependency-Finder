// <copyright file="DependencyWriter.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ConsoleApp.Output
{
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.Extensions.Logging;
    using NuGetTransitiveDependencyFinder.ConsoleApp.Resources.Messages;
    using NuGetTransitiveDependencyFinder.Output;

    /// <summary>
    /// A class for writing NuGet dependency information.
    /// </summary>
    internal class DependencyWriter : IDependencyWriter
    {
        /// <summary>
        /// The logger object to which to write the output.
        /// </summary>
        private readonly ILogger<DependencyWriter> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyWriter"/> class.
        /// </summary>
        /// <param name="logger">The logger object to which to write the output.</param>
        public DependencyWriter(ILogger<DependencyWriter> logger) =>
            this.logger = logger;

        /// <inheritdoc/>
        public void Write(Projects projects)
        {
            const int frameworkIndentationLevel = 1;

            if (!projects.HasChildren)
            {
                this.logger.LogInformation(Information.NoDependencies);
                return;
            }

            foreach (var project in projects.SortedChildren)
            {
                this.logger.LogInformation(project.ToString());
                foreach (var framework in project.SortedChildren)
                {
                    this.logger.LogInformation(CreatePrefix(frameworkIndentationLevel) + framework);
                    if (!framework.HasChildren)
                    {
                        continue;
                    }

                    this.WriteDependencies(framework.SortedChildren);
                }

                this.logger.LogInformation(string.Empty);
            }
        }

        /// <summary>
        /// Creates the string prefixes, which comprise indentation formed by an appropriate number of spaces.
        /// </summary>
        /// <param name="indentLevel">The level to which to indent.</param>
        /// <returns>A string comprising the appropriate level of indentation.</returns>
        private static string CreatePrefix(int indentLevel)
        {
            const int indentationSize = 4;

            return new(' ', indentLevel * indentationSize);
        }

        /// <summary>
        /// Writes a collection of dependencies.
        /// </summary>
        /// <param name="dependencies">The collection of dependencies.</param>
        private void WriteDependencies(IReadOnlyCollection<Dependency> dependencies)
        {
            const int dependencyIndentationLevel = 2;

            foreach (var dependency in dependencies)
            {
                if (dependency.IsTransitive)
                {
                    this.logger.LogWarning(
                        CreatePrefix(dependencyIndentationLevel) +
                        string.Format(CultureInfo.CurrentCulture, Information.TransitiveDependency, dependency));
                }
                else
                {
                    this.logger.LogDebug(CreatePrefix(dependencyIndentationLevel) + dependency);
                }
            }
        }
    }
}
