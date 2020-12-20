// <copyright file="IdentifiedBase.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.Output
{
    using System;
    using System.Collections.Generic;
    using static System.FormattableString;

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
        /// Compares the current object to <see paramref="other"/>, returning an integer that indicates their
        /// relationship.
        /// </summary>
        /// <remarks>The result of this method is solely dependent on <see cref="Identifier"/>.</remarks>
        /// <param name="other">The object against which to compare the current object.</param>
        /// <returns>A value less than zero if the current object is less than <see paramref="other"/>, zero if the
        /// current object is equal to <see paramref="other"/>, or a value greater than zero if the current object is
        /// greater than <see paramref="other"/>.</returns>
        internal int BaseCompareTo(IdentifiedBase<TIdentifier, TChild>? other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (other is null)
            {
                return 1;
            }

            return MapCompareTo(
                StringComparer.OrdinalIgnoreCase.Compare(this.Identifier.ToString(), other.Identifier.ToString()));
        }

        /// <summary>
        /// Compares the current object to <see paramref="other"/>, returning an integer that indicates their
        /// relationship.
        /// </summary>
        /// <remarks>The result of this method is solely dependent on <see cref="Identifier"/>.</remarks>
        /// <exception cref="ArgumentException"><paramref name="obj"/> is not of type
        /// <see cref="IdentifiedBase{TIdentifier, TChild}"/>.</exception>
        /// <param name="obj">The object against which to compare the current object.</param>
        /// <param name="className">The name of the calling class, which will be used within the exception
        /// message.</param>
        /// <returns>A value less than zero if the current object is less than <see paramref="other"/>, zero if the
        /// current object is equal to <see paramref="other"/>, or a value greater than zero if the current object is
        /// greater than <see paramref="other"/>.</returns>
        internal int BaseCompareTo(object? obj, string className)
        {
            if (obj is null)
            {
                return 1;
            }

            if (obj is not IdentifiedBase<TIdentifier, TChild>)
            {
                throw new ArgumentException(Invariant($"Object must be of type {className}."), nameof(obj));
            }

            if (ReferenceEquals(this, obj))
            {
                return 0;
            }

            return this.BaseCompareTo(obj as IdentifiedBase<TIdentifier, TChild>);
        }

        /// <summary>
        /// Determines whether performing an <see cref="Base{TChild}.Add(TChild)"/> operation on the specified child
        /// element is valid.
        /// </summary>
        /// <param name="child">The child element to check.</param>
        /// <returns>A value indicating whether the child element should be added the collection.</returns>
        internal abstract override bool IsAddValid(TChild child);

        /// <summary>
        /// Maps a value returned from <see cref="StringComparer.Compare(string?, string?)"/>, which can span a
        /// considerable range, to the range [-1, 1] expected from <see cref="IComparable{Dependency}.CompareTo"/>.
        /// </summary>
        /// <param name="value">The value to map.</param>
        /// <returns>The mapped value, which will be in the range [-1, 1].</returns>
        private static int MapCompareTo(int value)
        {
            if (value > 0)
            {
                return 1;
            }

            if (value < 0)
            {
                return -1;
            }

            return value;
        }
    }
}
