// <copyright file="SerializedVersion.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Output.Serialization;

using Xunit.Abstractions;

/// <summary>
/// A serializable representation of a <see cref="Version"/> object.
/// </summary>
public sealed class SerializedVersion : IXunitSerializable
{
    /// <summary>
    /// Gets the version object.
    /// </summary>
    public Version Version { get; } = default!;

    /// <summary>
    /// Initializes a new instance of the <see cref="SerializedVersion"/> class.
    /// </summary>
    public SerializedVersion()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SerializedVersion"/> class.
    /// </summary>
    /// <param name="version">The version object.</param>
    public SerializedVersion(Version version) =>
        this.Version = version;

    /// <inheritdoc/>
    public void Deserialize(IXunitSerializationInfo info) =>
        throw new NotImplementedException($"Deserialization is unavailable for ${nameof(SerializedVersion)}.");

    /// <inheritdoc/>
    public void Serialize(IXunitSerializationInfo info)
    {
        info.AddValue(nameof(this.Version.Major), this.Version.Major);
        info.AddValue(nameof(this.Version.Minor), this.Version.Minor);
        info.AddValue(nameof(this.Version.Build), this.Version.Build);
        info.AddValue(nameof(this.Version.Revision), this.Version.Revision);

        // This is a workaround to ensure that the serialization is unique and does not result in tests being skipped.
        info.AddValue("Uniquifier", Guid.NewGuid().ToString());
    }
}
