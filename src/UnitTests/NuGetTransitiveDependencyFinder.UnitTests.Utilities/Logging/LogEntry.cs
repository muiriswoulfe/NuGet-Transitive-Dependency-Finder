// <copyright file="LogEntry.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Utilities.Logging;

using Microsoft.Extensions.Logging;

/// <summary>
/// A record representing an entry added to an <see cref="ILogger"/> object.
/// </summary>
public record LogEntry(LogLevel LogLevel, EventId EventId, string Message);
