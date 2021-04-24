// <copyright file="LockFileUtilitiesWrapper.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ProjectAnalysis
{
    using System.Diagnostics;
    using NuGet.ProjectModel;
    using INuGetLogger = NuGet.Common.ILogger;

    /// <summary>
    /// A wrapper class around <see cref="Process"/>, to enable unit testing.
    /// </summary>
    internal class LockFileUtilitiesWrapper : ILockFileUtilitiesWrapper
    {
        /// <summary>
        /// The object managing logging from within the NuGet infrastructure.
        /// </summary>
        private readonly INuGetLogger nuGetLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LockFileUtilitiesWrapper"/> class.
        /// </summary>
        /// <param name="nuGetLogger">The object managing logging from within the NuGet infrastructure.</param>
        public LockFileUtilitiesWrapper(INuGetLogger nuGetLogger) =>
            this.nuGetLogger = nuGetLogger;

        /// <inheritdoc/>
        public LockFile GetLockFile(string lockFilePath) =>
            LockFileUtilities.GetLockFile(lockFilePath, this.nuGetLogger);
    }
}
