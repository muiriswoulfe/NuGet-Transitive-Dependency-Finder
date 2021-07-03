// <copyright file="IProgramRunner.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ConsoleApp.Process
{
    using System.Threading.Tasks;

    /// <summary>
    /// The interface defining the flow of the main application logic.
    /// </summary>
    internal interface IProgramRunner
    {
        /// <summary>
        /// Runs the main application logic.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task RunAsync();
    }
}
