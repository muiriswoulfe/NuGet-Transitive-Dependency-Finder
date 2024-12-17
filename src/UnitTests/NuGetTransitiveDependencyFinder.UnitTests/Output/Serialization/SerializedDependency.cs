// <copyright file="SerializedDependency.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Output.Serialization;

using NuGetTransitiveDependencyFinder.Output;
using Xunit.Abstractions;

/// <summary>
/// A serializable representation of a <see cref="Dependency"/> object.
/// </summary>
public sealed class SerializedDependency : IXunitSerializable
{
    /// <summary>
    /// Gets the dependency object.
    /// </summary>
    public Dependency Dependency { get; private set; } = default!;

    /// <summary>
    /// Initializes a new instance of the <see cref="SerializedDependency"/> class.
    /// </summary>
    public SerializedDependency()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SerializedDependency"/> class.
    /// </summary>
    /// <param name="dependency">The dependency object.</param>
    public SerializedDependency(Dependency dependency) =>
        this.Dependency = dependency;

    /// <inheritdoc/>
    public void Deserialize(IXunitSerializationInfo info)
    {
        var identifier = info.GetValue<string>(nameof(this.Dependency.Identifier));

        this.Dependency = new(identifier);
    }

    /// <inheritdoc/>
    public void Serialize(IXunitSerializationInfo info)
    {
        info.AddValue(nameof(this.Dependency.Identifier), this.Dependency.Identifier);

        // This is a workaround to ensure that the serialization is unique and does not result in tests being skipped.
        info.AddValue("Uniquifier", Guid.NewGuid().ToString());
    }
}
