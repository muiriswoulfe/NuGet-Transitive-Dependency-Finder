// <copyright file="Library.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.Output
{
    using System;
    using NuGet.Versioning;

    /// <summary>
    /// A class representing the outputted .NET framework information for each project.
    /// </summary>
    /// <remarks>The child elements of this class are the transitive NuGet dependencies.</remarks>
    public sealed class Library : IComparable<Library>, IEquatable<Library>
    {
        /// <summary>
        /// The .NET framework ID.
        /// </summary>
        private readonly string identifier;

        /// <summary>
        /// The .NET framework version.
        /// </summary>
        private readonly NuGetVersion version;

        /// <summary>
        /// Initializes a new instance of the <see cref="Library"/> class.
        /// </summary>
        /// <param name="identifier">The .NET framework identifier.</param>
        /// <param name="version">The .NET framework version.</param>
        public Library(string identifier, NuGetVersion version)
        {
            this.identifier = identifier;
            this.version = version;
        }

        /// <summary>
        /// Determines if <see paramref="left"/> is equal to <see paramref="right"/>.
        /// </summary>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <returns><c>true</c> if <see paramref="left"/> is equal to <see paramref="right"/>; otherwise,
        /// <c>false</c>.</returns>
        public static bool operator ==(Library left, Library right) =>
            left.CompareTo(right) == 0;

        /// <summary>
        /// Determines if <see paramref="left"/> is not equal to <see paramref="right"/>.
        /// </summary>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <returns><c>true</c> if <see paramref="left"/> is not equal to <see paramref="right"/>; otherwise,
        /// <c>false</c>.</returns>
        public static bool operator !=(Library left, Library right) =>
            left.CompareTo(right) != 0;

        /// <summary>
        /// Determines if <see paramref="left"/> is less than <see paramref="right"/>.
        /// </summary>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <returns><c>true</c> if <see paramref="left"/> is less than <see paramref="right"/>; otherwise,
        /// <c>false</c>.</returns>
        public static bool operator <(Library left, Library right) =>
            left.CompareTo(right) < 0;

        /// <summary>
        /// Determines if <see paramref="left"/> is less than or equal to <see paramref="right"/>.
        /// </summary>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <returns><c>true</c> if <see paramref="left"/> is less than or equal to <see paramref="right"/>; otherwise,
        /// <c>false</c>.</returns>
        public static bool operator <=(Library left, Library right) =>
            left.CompareTo(right) <= 0;

        /// <summary>
        /// Determines if <see paramref="left"/> is greater than <see paramref="right"/>.
        /// </summary>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <returns><c>true</c> if <see paramref="left"/> is greater than <see paramref="right"/>; otherwise,
        /// <c>false</c>.</returns>
        public static bool operator >(Library left, Library right) =>
            left.CompareTo(right) > 0;

        /// <summary>
        /// Determines if <see paramref="left"/> is greater than or equal to <see paramref="right"/>.
        /// </summary>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <returns><c>true</c> if <see paramref="left"/> is greater than or equal to <see paramref="right"/>;
        /// otherwise, <c>false</c>.</returns>
        public static bool operator >=(Library left, Library right) =>
            left.CompareTo(right) >= 0;

        /// <inheritdoc/>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        public int CompareTo(Library? other) =>
            this.BaseCompareTo(other);

        /// <inheritdoc/>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        public bool Equals(Library? other) =>
            this.BaseCompareTo(other) == 0;

        /// <inheritdoc/>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        public override bool Equals(object? obj) =>
            this.BaseCompareTo(obj) == 0;

        /// <inheritdoc/>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        public override int GetHashCode() =>
            StringComparer.OrdinalIgnoreCase.GetHashCode(this.identifier);

        /// <inheritdoc/>
        public override string ToString() =>
            this.identifier + this.version;

        /// <summary>
        /// Compares the current object to <see paramref="other"/>, returning an integer that indicates their
        /// relationship.
        /// </summary>
        /// <remarks>The result of this method is solely dependent on <see cref="identifier"/>.</remarks>
        /// <param name="other">The object against which to compare the current object.</param>
        /// <returns>A value less than zero if the current object is less than <see paramref="other"/>, zero if the
        /// current object is equal to <see paramref="other"/>, or a value greater than zero if the current object is
        /// greater than <see paramref="other"/>.</returns>
        private int BaseCompareTo(Library? other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (other is null)
            {
                return 1;
            }

            return StringComparer.OrdinalIgnoreCase.Compare(this.identifier, other.identifier);
        }

        /// <summary>
        /// Compares the current object to <see paramref="other"/>, returning an integer that indicates their
        /// relationship.
        /// </summary>
        /// <remarks>The result of this method is solely dependent on <see cref="identifier"/>.</remarks>
        /// <param name="obj">The object against which to compare the current object.</param>
        /// <returns>A value less than zero if the current object is less than <see paramref="other"/>, zero if the
        /// current object is equal to <see paramref="other"/>, or a value greater than zero if the current object is
        /// greater than <see paramref="other"/>.</returns>
        private int BaseCompareTo(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return 0;
            }

            if (obj is null)
            {
                return 1;
            }

            return this.BaseCompareTo(obj as Library);
        }
    }
}
