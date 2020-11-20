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
    /// A class for writing transitive NuGet dependency information.
    /// </summary>
    internal class DependencyWriter
    {
        /// <summary>
        /// The logger object to which to write the output.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyWriter"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory from which a logger will be created.</param>
        public DependencyWriter(ILoggerFactory loggerFactory) =>
            this.logger = loggerFactory.CreateLogger(nameof(DependencyWriter));

        /// <summary>
        /// Writes the transitive NuGet dependency information.
        /// </summary>
        /// <param name="projects">The details of the projects.</param>
        public void Write(Projects projects)
        {
            const int FrameworkIndentationLevel = 1;

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
                    this.logger.LogInformation(CreatePrefix(FrameworkIndentationLevel) + framework);
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
            const int IndentationSize = 4;

            return new string(' ', indentLevel * IndentationSize);
        }

        /// <summary>
        /// Populates a localized string by replacing the placeholders with the appropriate dependency information.
        /// </summary>
        /// <param name="localizedString">The localized string to be populated.</param>
        /// <param name="dependency">The dependency information with which to populate the localized string.</param>
        /// <returns>The localized string with all placeholders populated.</returns>
        private static string PopulateLocalizedString(string localizedString, Dependency dependency) =>
            string.Format(CultureInfo.CurrentCulture, localizedString, dependency.Identifier, dependency.Version);

        /// <summary>
        /// Writes a collection of dependencies.
        /// </summary>
        /// <param name="dependencies">The collection of dependencies.</param>
        private void WriteDependencies(IReadOnlyCollection<Dependency> dependencies)
        {
            const int DependencyIndentationLevel = 2;

            foreach (var dependency in dependencies)
            {
                if (dependency.IsTransitive)
                {
                    this.logger.LogWarning(
                        CreatePrefix(DependencyIndentationLevel) +
                        string.Format(CultureInfo.CurrentCulture, Information.TransitiveDependency, dependency));
                }
                else
                {
                    this.logger.LogDebug(CreatePrefix(DependencyIndentationLevel) + dependency);
                }
            }
        }
    }
}
