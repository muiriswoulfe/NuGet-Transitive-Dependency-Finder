// <copyright file="ILockFileUtilitiesWrapper.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ProjectAnalysis
{
    using NuGet.ProjectModel;

    /// <summary>
    /// A wrapper interface around <see cref="LockFileUtilities"/>, to enable unit testing.
    /// </summary>
    internal interface ILockFileUtilitiesWrapper
    {
        /// <summary>
        /// Gets the NuGet restoration process lock file.
        /// </summary>
        /// <param name="lockFilePath">The path to the lock file.</param>
        /// <returns>An object representing the lock file.</returns>
        public LockFile GetLockFile(string lockFilePath);
    }
}
