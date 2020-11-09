// <copyright file="Assets.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGet.TransitiveDependency.Finder.Library.ProjectAnalysis
{
    using System.IO;
    using NuGet.Common;
    using NuGet.ProjectModel;
    using static System.FormattableString;
    using ILogger = NuGet.TransitiveDependency.Finder.Library.Input.ILogger;

    /// <summary>
    /// A class representing the contents of a "project.assets.json" file.
    /// </summary>
    internal class Assets
    {
        /// <summary>
        /// The logger for asynchronous messages that have been created by external processes.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The path of the directory containing the .NET project file.
        /// </summary>
        private readonly string projectDirectory;

        /// <summary>
        /// The path of the directory in which to store the project restore outputs.
        /// </summary>
        private readonly string outputDirectory;

        /// <summary>
        /// The command-line parameters for the .NET process.
        /// </summary>
        private readonly string parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="Assets"/> class.
        /// </summary>
        /// <param name="logger">The logger for asynchronous messages that have been created by external
        /// processes.</param>
        /// <param name="projectPath">The path of the .NET project file, including the file name.</param>
        /// <param name="outputDirectory">The path of the directory in which to store the project restore
        /// outputs.</param>
        public Assets(ILogger logger, string projectPath, string outputDirectory)
        {
            this.logger = logger;
            this.projectDirectory = Path.GetDirectoryName(projectPath) !;
            this.outputDirectory = outputDirectory;
            this.parameters = Invariant($"restore \"{projectPath}\"");
        }

        /// <summary>
        /// Creates a <see cref="LockFile"/> object representing the "project.assets.json" file contents.
        /// </summary>
        /// <returns>The <see cref="LockFile"/> object.</returns>
        public LockFile? Create()
        {
            var dotNetRunner = new DotNetRunner(this.logger, this.parameters, this.projectDirectory);
            dotNetRunner.Run();

            var projectAssetsFilePath = Path.Combine(this.outputDirectory, "project.assets.json");
            return LockFileUtilities.GetLockFile(projectAssetsFilePath, NullLogger.Instance);
        }
    }
}
