// <copyright file="Assets.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ProjectAnalysis
{
    using System.IO;
    using NuGet.ProjectModel;
    using static System.FormattableString;
    using INuGetLogger = NuGet.Common.ILogger;

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
        /// The object managing logging from within the NuGet infrastructure.
        /// </summary>
        private readonly INuGetLogger nuGetLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Assets"/> class.
        /// </summary>
        /// <param name="dotNetRunner">The object managing the running of .NET commands on project and solution
        /// files.</param>
        /// <param name="nuGetLogger">The object managing logging from within the NuGet infrastructure.</param>
        public Assets(IDotNetRunner dotNetRunner, INuGetLogger nuGetLogger)
        {
            this.dotNetRunner = dotNetRunner;
            this.nuGetLogger = nuGetLogger;
        }

        /// <inheritdoc/>
        public LockFile? Create(string projectPath, string outputDirectory)
        {
            var parameters = Invariant($"restore \"{projectPath}\"");
            var projectDirectory = Path.GetDirectoryName(projectPath)!;
            this.dotNetRunner.Run(parameters, projectDirectory);

            var projectAssetsFilePath = Path.Combine(outputDirectory, "project.assets.json");
            return LockFileUtilities.GetLockFile(projectAssetsFilePath, this.nuGetLogger);
        }
    }
}
