// <copyright file="ILogger.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGet.TransitiveDependency.Finder.Library.Input
{
    /// <summary>
    /// An interface for logging asynchronous messages that have been created by external processes.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs messages sent to the Standard Error stream.
        /// </summary>
        /// <param name="message">The message.</param>
        void LogError(string message);

        /// <summary>
        /// Logs messages sent to the Standard Output stream.
        /// </summary>
        /// <param name="message">The message.</param>
        void LogOutput(string message);
    }
}
