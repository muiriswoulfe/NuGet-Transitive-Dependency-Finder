// <copyright file="Writer.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGet.TransitiveDependency.Finder.ConsoleApp.Output
{
    using Microsoft.Extensions.Logging;
    using NuGet.TransitiveDependency.Finder.ConsoleApp.Resources;
    using NuGet.TransitiveDependency.Finder.Library.Output;

    /// <summary>
    /// A class for writing transitive NuGet dependency information.
    /// </summary>
    internal class Writer
    {
        /// <summary>
        /// The logger object to which to send the output.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Writer"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory from which a logger will be created.</param>
        public Writer(ILoggerFactory loggerFactory) =>
            this.logger = loggerFactory.CreateLogger(nameof(Writer));

        /// <summary>
        /// Writes the transitive NuGet dependency information.
        /// </summary>
        /// <param name="projects">The details of the projects.</param>
        public void Write(Projects projects)
        {
            const int FrameworkIndentationLevel = 1;
            const int TransitiveDependencyIndentationLevel = 2;

            if (!projects.HasChildren)
            {
                this.logger.LogInformation(Strings.Information.NoTransitiveNuGetDependencies);
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

                    foreach (var transitiveDependency in framework.SortedChildren)
                    {
                        this.logger.LogInformation(
                            CreatePrefix(TransitiveDependencyIndentationLevel) + transitiveDependency);
                    }
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
    }
}
