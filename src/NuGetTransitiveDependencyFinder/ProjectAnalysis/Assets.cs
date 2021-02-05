// <copyright file="Assets.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ProjectAnalysis
{
    using System.IO;
    using NuGet.Common;
    using NuGet.ProjectModel;
    using static System.FormattableString;

    /// <summary>
    /// A class representing the contents of a "project.assets.json" file.
    /// </summary>
    public class Assets
    {
        /// <summary>
        /// The object managing the running of .NET commands on project and solution files.
        /// </summary>
        private readonly DotNetRunner dotNetRunner;

        /// <summary>
        /// Initializes a new instance of the <see cref="Assets"/> class.
        /// </summary>
        /// <param name="dotNetRunner">The object managing the running of .NET commands on project and solution
        /// files.</param>
        public Assets(DotNetRunner dotNetRunner) =>
            this.dotNetRunner = dotNetRunner;

        /// <summary>
        /// Creates a <see cref="LockFile"/> object representing the "project.assets.json" file contents.
        /// </summary>
        /// <param name="projectPath">The path of the .NET project file, including the file name.</param>
        /// <param name="outputDirectory">The path of the directory in which to store the project restore
        /// outputs.</param>
        /// <returns>The <see cref="LockFile"/> object.</returns>
        internal LockFile? Create(string projectPath, string outputDirectory)
        {
            var parameters = Invariant($"restore \"{projectPath}\"");
            var projectDirectory = Path.GetDirectoryName(projectPath)!;
            this.dotNetRunner.Run(parameters, projectDirectory);

            var projectAssetsFilePath = Path.Combine(outputDirectory, "project.assets.json");
            return LockFileUtilities.GetLockFile(projectAssetsFilePath, NullLogger.Instance);
        }
    }
}
