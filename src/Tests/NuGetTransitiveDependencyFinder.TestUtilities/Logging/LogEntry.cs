// <copyright file="LogEntry.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.TestUtilities.Logging
{
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// A record representing an entry added to an <see cref="Microsoft.Extensions.Logging.ILogger"/> object.
    /// </summary>
    /// <param name="LogLevel">The level of the log entry.</param>
    /// <param name="EventId">The ID of the event resulting in the log entry.</param>
    /// <param name="Message">The log entry message.</param>
    public record LogEntry(LogLevel LogLevel, EventId EventId, string Message);
}
