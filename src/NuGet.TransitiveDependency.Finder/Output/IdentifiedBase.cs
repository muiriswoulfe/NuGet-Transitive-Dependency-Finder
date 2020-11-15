// <copyright file="IdentifiedBase.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGet.TransitiveDependency.Finder.Output
{
    using System;

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
        /// <param name="capacity">The quantity of child elements for which the collection initially has adequate
        /// capacity.</param>
        /// <param name="identifier">The identifier of the object.</param>
        protected IdentifiedBase(int capacity, TIdentifier identifier)
            : base(capacity) =>
            this.Identifier = identifier;

        /// <summary>
        /// Gets the identifier of the object.
        /// </summary>
        public TIdentifier Identifier { get; }

        /// <summary>
        /// Gets a hash code for the current object.
        /// </summary>
        /// <remarks>The result of this method is solely dependent on <see cref="Identifier"/>.</remarks>
        protected int BaseHashCode =>
            StringComparer.OrdinalIgnoreCase.GetHashCode(this.Identifier.ToString() !);

        /// <inheritdoc/>
        /// <remarks>The result of this method is solely dependent on <see cref="Identifier"/>.</remarks>
        public override string ToString() =>
            this.Identifier.ToString() !;

        /// <summary>
        /// Compares the current object to <see paramref="other"/>, returning an integer that indicates their
        /// relationship.
        /// </summary>
        /// <remarks>The result of this method is solely dependent on <see cref="Identifier"/>.</remarks>
        /// <param name="other">The object against which to compare the current object.</param>
        /// <returns>A value less than zero if the current object is less than <see paramref="other"/>, zero if the
        /// current object is equal to <see paramref="other"/>, or a value greater than zero if the current object is
        /// greater than <see paramref="other"/>.</returns>
        protected int BaseCompareTo(IdentifiedBase<TIdentifier, TChild>? other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (other is null)
            {
                return 1;
            }

            return StringComparer.OrdinalIgnoreCase.Compare(this.Identifier.ToString(), other.Identifier.ToString());
        }

        /// <summary>
        /// Compares the current object to <see paramref="other"/>, returning an integer that indicates their
        /// relationship.
        /// </summary>
        /// <remarks>The result of this method is solely dependent on <see cref="Identifier"/>.</remarks>
        /// <param name="obj">The object against which to compare the current object.</param>
        /// <returns>A value less than zero if the current object is less than <see paramref="other"/>, zero if the
        /// current object is equal to <see paramref="other"/>, or a value greater than zero if the current object is
        /// greater than <see paramref="other"/>.</returns>
        protected int BaseCompareTo(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return 0;
            }

            if (obj is null)
            {
                return 1;
            }

            return this.BaseCompareTo(obj as IdentifiedBase<TIdentifier, TChild>);
        }

        /// <summary>
        /// Determines whether performing an <see cref="Base{TChild}.Add(TChild)"/> operation on the specified child
        /// element is valid.
        /// </summary>
        /// <param name="child">The child element to check.</param>
        /// <returns>A value indicating whether the child element should be added the collection.</returns>
        protected override abstract bool IsAddValid(TChild child);
    }
}
