// <copyright file="Assets.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ProjectAnalysis
{
    using System.IO;
    using NuGet.ProjectModel;
    using NuGetTransitiveDependencyFinder.Wrappers;
    using static System.FormattableString;

    /// <summary>
    /// A class representing the contents of a "project.assets.json" file.
    /// </summary>
    internal class Assets : IAssets
    {
        /// <summary>
        /// The object managing the running of .NET commands on project and solution files.
        /// </summary>
        private readonly IDotNetRunner dotNetRunner;

        /// <summary>
        /// The wrapper around <see cref="LockFileUtilities"/>.
        /// </summary>
        private readonly ILockFileUtilitiesWrapper lockFileUtilitiesWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="Assets"/> class.
        /// </summary>
        /// <param name="dotNetRunner">The object managing the running of .NET commands on project and solution
        /// files.</param>
        /// <param name="lockFileUtilitiesWrapper">The wrapper around <see cref="LockFileUtilities"/>.</param>
        public Assets(IDotNetRunner dotNetRunner, ILockFileUtilitiesWrapper lockFileUtilitiesWrapper)
        {
            this.dotNetRunner = dotNetRunner;
            this.lockFileUtilitiesWrapper = lockFileUtilitiesWrapper;
        }

        /// <inheritdoc/>
        public LockFile? Create(string projectPath, string outputDirectory)
        {
            var parameters = Invariant($"restore \"{projectPath}\"");
            var projectDirectory = Path.GetDirectoryName(projectPath)!;
            this.dotNetRunner.Run(parameters, projectDirectory);

            var projectAssetsFilePath = Path.Combine(outputDirectory, "project.assets.json");
            return this.lockFileUtilitiesWrapper.GetLockFile(projectAssetsFilePath);
        }
    }
}
