// <copyright file="IProgramRunner.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ConsoleApp.Process
{
    /// <summary>
    /// The interface defining the flow of the main application logic.
    /// </summary>
    internal interface IProgramRunner
    {
        /// <summary>
        /// Runs the main application logic.
        /// </summary>
        public void Run();
    }
}
