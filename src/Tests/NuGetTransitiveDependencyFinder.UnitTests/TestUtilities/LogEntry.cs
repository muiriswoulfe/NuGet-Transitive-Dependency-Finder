// <copyright file="LogEntry.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.TestUtilities
{
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// A record representing an entry added to an <see cref="Microsoft.Extensions.Logging.ILogger"/> object.
    /// </summary>
    internal record LogEntry(LogLevel LogLevel, EventId EventId, string Message);
}
