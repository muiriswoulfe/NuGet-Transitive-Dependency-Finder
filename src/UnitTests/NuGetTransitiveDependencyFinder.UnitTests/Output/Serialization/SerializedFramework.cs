// <copyright file="SerializedFramework.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Output.Serialization;

using NuGetTransitiveDependencyFinder.Output;
using Xunit.Abstractions;

/// <summary>
/// A serializable representation of a <see cref="Framework"/> object.
/// </summary>
public sealed class SerializedFramework : IXunitSerializable
{
    /// <summary>
    /// Gets the framework object.
    /// </summary>
    public Framework Framework { get; private set; } = default!;

    /// <summary>
    /// Initializes a new instance of the <see cref="SerializedFramework"/> class.
    /// </summary>
    public SerializedFramework()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SerializedFramework"/> class.
    /// </summary>
    /// <param name="framework">The framework object.</param>
    public SerializedFramework(Framework framework) =>
        this.Framework = framework;

    /// <inheritdoc/>
    public void Deserialize(IXunitSerializationInfo info)
    {
        var identifier = info.GetValue<string>(nameof(this.Framework.Identifier));

        this.Framework = new(new(identifier), []);
    }

    /// <inheritdoc/>
    public void Serialize(IXunitSerializationInfo info)
    {
        info.AddValue(nameof(this.Framework.Identifier), this.Framework.Identifier.Framework);

        // This is a workaround to ensure that the serialization is unique and does not result in tests being skipped.
        info.AddValue("Uniquifier", Guid.NewGuid().ToString());
    }
}
