// <copyright file="ConsoleLogger.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGet.TransitiveDependency.Finder.ConsoleApp.Output
{
    using FluentColorConsole;
    using NuGet.TransitiveDependency.Finder.Library.Input;

    /// <summary>
    /// A class for logging, to the console, asynchronous messages that have been created by external processes.
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        /// <inheritdoc/>
        public void LogError(string message) =>
            ColorConsole.WithRedText.WriteLine(message);

        /// <inheritdoc/>
        public void LogOutput(string message) =>
            ColorConsole.WithDarkYellowText.WriteLine(message);
    }
}
