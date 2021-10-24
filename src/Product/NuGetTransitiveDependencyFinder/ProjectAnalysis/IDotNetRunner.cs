// <copyright file="IDotNetRunner.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ProjectAnalysis
{
    /// <summary>
    /// An interface that manages the running of .NET commands on project and solution files.
    /// </summary>
    internal interface IDotNetRunner
    {
        /// <summary>
        /// Runs the .NET process.
        /// </summary>
        /// <param name="parameters">The parameters to pass to the "dotnet" command, excluding the file name of the
        /// executable.</param>
        /// <param name="workingDirectory">The path of the directory in which to store the files created after running
        /// the "dotnet" command.</param>
        public void Run(string parameters, string workingDirectory);
    }
}
