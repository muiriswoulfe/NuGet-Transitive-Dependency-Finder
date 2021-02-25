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
        /// The string prefix for each framework, which comprises one level of indentation.
        /// </summary>
        private static readonly string FrameworkPrefix = CreatePrefix(1);

        /// <summary>
        /// The string prefix for each dependency, which comprises two levels of indentation.
        /// </summary>
        private static readonly string DependencyPrefix = CreatePrefix(2);

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
                    var frameworkString = framework.ToString();
                    this.logger.LogInformation(FrameworkPrefix + frameworkString);
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
            foreach (var dependency in dependencies)
            {
                if (dependency.IsTransitive)
                {
                    this.logger.LogWarning(
                        DependencyPrefix +
                        string.Format(CultureInfo.CurrentCulture, Information.TransitiveDependency, dependency));
                }
                else
                {
                    var dependencyString = dependency.ToString();
                    this.logger.LogDebug(DependencyPrefix + dependencyString);
                }
            }
        }
    }
}
