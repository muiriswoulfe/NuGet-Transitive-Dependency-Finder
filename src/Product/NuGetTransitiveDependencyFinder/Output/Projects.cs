// <copyright file="Projects.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.Output;

/// <summary>
/// A class representing the outputted projects information.
/// </summary>
/// <remarks>The child elements of this class are the <see cref="Project"/> objects.</remarks>
public sealed class Projects : Base<Project>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Projects"/> class.
    /// </summary>
    /// <param name="capacity">The quantity of projects for which the collection initially has adequate
    /// capacity.</param>
    internal Projects(int capacity)
        : base(capacity)
    {
    }

    /// <inheritdoc/>
    internal override bool IsAddValid(Project child) =>
        child.HasChildren;
}
