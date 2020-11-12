// <copyright file="ConsoleWriter.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGet.TransitiveDependency.Finder.ConsoleApp.Output
{
    using System;
    using NuGet.TransitiveDependency.Finder.ConsoleApp.Resources;
    using NuGet.TransitiveDependency.Finder.Library.Output;

    /// <summary>
    /// A class for writing transitive NuGet dependency information to the console.
    /// </summary>
    internal static class ConsoleWriter
    {
        /// <summary>
        /// Writes the transitive NuGet dependency information to the console.
        /// </summary>
        /// <param name="projects">The details of the projects.</param>
        public static void Write(Projects projects)
        {
            if (!projects.HasChildren)
            {
                Console.WriteLine(Strings.Information.NoTransitiveNuGetDependencies);
                return;
            }

            foreach (var project in projects.SortedChildren)
            {
                Console.WriteLine(project);
                foreach (var framework in project.SortedChildren)
                {
                    Console.WriteLine(CreatePrefix(1) + framework);
                    if (!framework.HasChildren)
                    {
                        continue;
                    }

                    foreach (var transitiveDependency in framework.SortedChildren)
                    {
                        Console.WriteLine(CreatePrefix(2) + transitiveDependency);
                    }
                }

                Console.WriteLine();
            }
        }

        /// <summary>
        /// Creates the string prefixes, which comprise indentation formed by an appropriate number of spaces.
        /// </summary>
        /// <param name="indentLevel">The level to which to indent.</param>
        /// <returns>A string comprising the appropriate level of indentation.</returns>
        private static string CreatePrefix(int indentLevel) =>
            new string(' ', indentLevel * 4);
    }
}
