// <copyright file="LockFileUtilitiesWrapper.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.Wrappers;

using System.Diagnostics;
using NuGet.ProjectModel;
using INuGetLogger = NuGet.Common.ILogger;

/// <summary>
/// A wrapper class around <see cref="Process"/>, to enable unit testing.
/// </summary>
/// <param name="nuGetLogger">The object managing logging from within the NuGet infrastructure.</param>
internal class LockFileUtilitiesWrapper(INuGetLogger nuGetLogger) : ILockFileUtilitiesWrapper
{
    /// <inheritdoc/>
    public LockFile GetLockFile(string lockFilePath) =>
        LockFileUtilities.GetLockFile(lockFilePath, nuGetLogger);
}
