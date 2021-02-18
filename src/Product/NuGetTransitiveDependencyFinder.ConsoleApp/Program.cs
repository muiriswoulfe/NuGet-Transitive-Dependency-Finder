// <copyright file="Program.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ConsoleApp
{
    using NuGetTransitiveDependencyFinder.ConsoleApp.Process;

    /// <summary>
    /// The main class of the application, defining the entry point and the basic operation of the application.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The entry point of the application.
        /// </summary>
        /// <param name="parameters">A collection of command-line parameters.</param>
        /// <returns>A status code where 0 represents success and 1 represents failure.</returns>
        public static int Main(string[] parameters) =>
            ProgramInitializer.Run(
                parameters,
                serviceProvider => ProgramInitializer.GetProgramRunner(serviceProvider).Run());
    }
}
