// <copyright file="MockLogger.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.TestUtilities.Logging
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// A mock implementation of <see cref="ILogger{TCategoryName}"/>.
    /// </summary>
    /// <typeparam name="TCategoryName">The log category name.</typeparam>
    public class MockLogger<TCategoryName> : ILogger<TCategoryName>
    {
        /// <summary>
        /// The collection of log entries.
        /// </summary>
        private readonly List<LogEntry> entries = new();

        /// <summary>
        /// Gets the collection of log entries.
        /// </summary>
        public IReadOnlyList<LogEntry> Entries =>
            this.entries;

        /// <summary>
        /// Begins a logical operation scope.
        /// </summary>
        /// <typeparam name="TState">The type of the state for which to begin the scope.</typeparam>
        /// <exception cref="NotSupportedException">Always thrown, as the method has not been implemented within the
        /// mock.</exception>
        /// <param name="state">The identifier of the scope.</param>
        /// <returns>A disposable object that ends the logical operation scope on disposal.</returns>
        public IDisposable BeginScope<TState>(TState state) =>
            throw new NotSupportedException();

        /// <summary>
        /// Determines if logging for the specified <paramref name="logLevel"/> is enabled.
        /// </summary>
        /// <exception cref="NotSupportedException">Always thrown, as the method has not been implemented within the
        /// mock.</exception>
        /// <param name="logLevel">The level to be checked.</param>
        /// <returns><see langword="true"/> if enabled; <see langword="false"/> otherwise.</returns>
        public bool IsEnabled(LogLevel logLevel) =>
            throw new NotSupportedException();

        /// <summary>
        /// Writes a log entry.
        /// </summary>
        /// <typeparam name="TState">The type of the object to be written.</typeparam>
        /// <param name="logLevel">The level.</param>
        /// <param name="eventId">The ID of the event.</param>
        /// <param name="state">The entry to be written.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">A function to create a message from <paramref name="state"/> and
        /// <paramref name="exception"/>.</param>
        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter) =>
            this.entries.Add(new(logLevel, eventId, formatter(state, exception)));
    }
}
