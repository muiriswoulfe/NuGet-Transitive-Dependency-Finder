// <copyright file="IdentifiedBase.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.Output;

using System;
using System.Collections.Generic;

/// <summary>
/// A base class representing outputted information inclusive of an identifier.
/// </summary>
/// <typeparam name="TIdentifier">The type of the identifier.</typeparam>
/// <typeparam name="TChild">The type of the collection elements, which comprise the child elements of the current
/// object.</typeparam>
public abstract class IdentifiedBase<TIdentifier, TChild> : Base<TChild>
    where TIdentifier : notnull
    where TChild : notnull
{
    /// <summary>
    /// The comparison logic specific to <see cref="IdentifiedBase{TIdentifier, TChild}"/>, which takes two objects of
    /// type <see cref="IdentifiedBase{TIdentifier, TChild}"/> and returns an <see cref="int"/>.
    /// </summary>
    internal static readonly Func<IdentifiedBase<TIdentifier, TChild>, IdentifiedBase<TIdentifier, TChild>, int>
        InternalComparisonFunction =
        (IdentifiedBase<TIdentifier, TChild> current, IdentifiedBase<TIdentifier, TChild> other) =>
            Comparer.MapCompareTo(
                StringComparer.OrdinalIgnoreCase.Compare(
                    current.Identifier.ToString(),
                    other.Identifier.ToString()));

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentifiedBase{TIdentifier, TChild}"/> class.
    /// </summary>
    /// <param name="identifier">The identifier of the object.</param>
    /// <param name="capacity">The quantity of child elements for which the collection initially has adequate
    /// capacity.</param>
    internal IdentifiedBase(TIdentifier identifier, int capacity)
        : base(capacity) =>
        this.Identifier = identifier;

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentifiedBase{TIdentifier, TChild}"/> class.
    /// </summary>
    /// <param name="identifier">The identifier of the object.</param>
    /// <param name="children">The child elements with which to initialize the collection.</param>
    internal IdentifiedBase(TIdentifier identifier, IReadOnlyCollection<TChild> children)
        : base(children) =>
        this.Identifier = identifier;

    /// <summary>
    /// Gets the identifier of the object.
    /// </summary>
    public TIdentifier Identifier { get; }

    /// <summary>
    /// Gets the comparison logic specific to <see cref="IdentifiedBase{TIdentifier, TChild}"/>, which takes two objects
    /// of type <see cref="IdentifiedBase{TIdentifier, TChild}"/> and returns an <see cref="int"/>.
    /// </summary>
    internal static Func<IdentifiedBase<TIdentifier, TChild>, IdentifiedBase<TIdentifier, TChild>, int>
        ComparisonFunction =>
        InternalComparisonFunction;

    /// <summary>
    /// Gets a hash code for the current object.
    /// </summary>
    /// <remarks>The result of this method is solely dependent on <see cref="Identifier"/>.</remarks>
    internal int BaseHashCode =>
        StringComparer.OrdinalIgnoreCase.GetHashCode(this.Identifier.ToString()!);

    /// <inheritdoc/>
    /// <remarks>The result of this method is solely dependent on <see cref="Identifier"/>.</remarks>
    public override string ToString() =>
        this.Identifier.ToString()!;

    /// <summary>
    /// Determines whether performing an <see cref="Base{TChild}.Add(TChild)"/> operation on the specified child element
    /// is valid.
    /// </summary>
    /// <param name="child">The child element to check.</param>
    /// <returns>A value indicating whether the child element should be added the collection.</returns>
    internal abstract override bool IsAddValid(TChild child);
}
