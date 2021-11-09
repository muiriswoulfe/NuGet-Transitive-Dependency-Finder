// <copyright file="IDependencyWriter.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ConsoleApp.Output;

using NuGetTransitiveDependencyFinder.Output;

/// <summary>
/// An interface for writing NuGet dependency information.
/// </summary>
internal interface IDependencyWriter
{
    /// <summary>
    /// Writes the NuGet dependency information.
    /// </summary>
    /// <param name="projects">The details of the projects.</param>
    public void Write(Projects projects);
}
