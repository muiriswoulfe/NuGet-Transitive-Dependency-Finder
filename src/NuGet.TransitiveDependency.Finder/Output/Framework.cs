// <copyright file="Framework.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGet.TransitiveDependency.Finder.Output
{
    using System;
    using NuGet.Frameworks;

    /// <summary>
    /// A class representing the outputted .NET framework information for each project.
    /// </summary>
    /// <remarks>The child elements of this class are the transitive NuGet dependencies.</remarks>
    public sealed class Framework :
        IdentifiedBase<NuGetFramework, string>,
        IComparable<Framework>,
        IEquatable<Framework>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Framework"/> class.
        /// </summary>
        /// <param name="capacity">The quantity of transitive NuGet dependencies for which the collection initially has
        /// adequate capacity.</param>
        /// <param name="identifier">The .NET framework identifier.</param>
        public Framework(int capacity, NuGetFramework identifier)
            : base(capacity, identifier)
        {
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
        public static bool operator ==(Framework left, Framework right) =>
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
        public static bool operator !=(Framework left, Framework right) =>
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
        public static bool operator <(Framework left, Framework right) =>
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
        public static bool operator <=(Framework left, Framework right) =>
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
        public static bool operator >(Framework left, Framework right) =>
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
        public static bool operator >=(Framework left, Framework right) =>
            left.CompareTo(right) >= 0;

        /// <inheritdoc/>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        public int CompareTo(Framework? other) =>
            this.BaseCompareTo(other);

        /// <inheritdoc/>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        public bool Equals(Framework? other) =>
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
            this.BaseHashCode;

        /// <inheritdoc/>
        protected override bool IsAddValid(string child) =>
            true;
    }
}
