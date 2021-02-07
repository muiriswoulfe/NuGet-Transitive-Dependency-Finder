// <copyright file="IAssets.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ProjectAnalysis
{
    using NuGet.ProjectModel;

    /// <summary>
    /// An interface representing the contents of a "project.assets.json" file.
    /// </summary>
    internal interface IAssets
    {
        /// <summary>
        /// Creates a <see cref="LockFile"/> object representing the "project.assets.json" file contents.
        /// </summary>
        /// <param name="projectPath">The path of the .NET project file, including the file name.</param>
        /// <param name="outputDirectory">The path of the directory in which to store the project restore
        /// outputs.</param>
        /// <returns>The <see cref="LockFile"/> object.</returns>
        public LockFile? Create(string projectPath, string outputDirectory);
    }
}
