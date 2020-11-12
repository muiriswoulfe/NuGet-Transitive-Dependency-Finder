// <copyright file="Program.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGet.TransitiveDependency.Finder.ConsoleApp
{
    using System;
    using NuGet.TransitiveDependency.Finder.ConsoleApp.Output;
    using NuGet.TransitiveDependency.Finder.ConsoleApp.Resources;
    using NuGet.TransitiveDependency.Finder.Library;

    /// <summary>
    /// The main class of the application, defining the entry point and all interaction.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The entry point of the application.
        /// </summary>
        /// <param name="parameters">A collection of command-line parameters.</param>
        public static void Main(string[] parameters)
        {
            if (parameters.Length != 1)
            {
                ColorConsole.WriteLine(Strings.Error.MissingParameter, ConsoleColor.Red);
                return;
            }

            Console.WriteLine(Strings.Information.CommencingAnalysis);
            var finder = new TransitiveDependencyFinder(new ConsoleLogger());
            var projects = finder.Run(parameters[0]);
            ConsoleWriter.Write(projects);
        }
    }
}
