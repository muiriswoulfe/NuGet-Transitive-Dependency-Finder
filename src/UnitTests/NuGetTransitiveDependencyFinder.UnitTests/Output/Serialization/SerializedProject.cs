// <copyright file="SerializedProject.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Output.Serialization;

using NuGetTransitiveDependencyFinder.Output;
using Xunit.Abstractions;

/// <summary>
/// A serializable representation of a <see cref="Project"/> object.
/// </summary>
public sealed class SerializedProject : IXunitSerializable
{
    /// <summary>
    /// Gets the project object.
    /// </summary>
    public Project Project { get; private set; } = default!;

    /// <summary>
    /// Initializes a new instance of the <see cref="SerializedProject"/> class.
    /// </summary>
    public SerializedProject()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SerializedProject"/> class.
    /// </summary>
    /// <param name="project">The project object.</param>
    public SerializedProject(Project project) =>
        this.Project = project;

    /// <inheritdoc/>
    public void Deserialize(IXunitSerializationInfo info)
    {
        var identifier = info.GetValue<string>(nameof(this.Project.Identifier));

        this.Project = new(identifier, 0);
    }

    /// <inheritdoc/>
    public void Serialize(IXunitSerializationInfo info)
    {
        info.AddValue(nameof(this.Project.Identifier), this.Project.Identifier);

        // This is a workaround to ensure that the serialization is unique and does not result in tests being skipped.
        info.AddValue("Uniquifier", Guid.NewGuid().ToString());
    }
}
