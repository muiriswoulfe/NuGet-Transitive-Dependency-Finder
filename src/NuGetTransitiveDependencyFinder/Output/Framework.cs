// <copyright file="Framework.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.Output
{
    using System;
    using System.Collections.Generic;
    using NuGet.Frameworks;
    using NuGetTransitiveDependencyFinder.Extensions;

    /// <summary>
    /// A class representing the outputted .NET framework information for each project.
    /// </summary>
    /// <remarks>The child elements of this class are the transitive NuGet dependencies.</remarks>
    public sealed class Framework :
        IdentifiedBase<NuGetFramework, Dependency>,
        IComparable,
        IComparable<Framework>,
        IEquatable<Framework>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Framework"/> class.
        /// </summary>
        /// <param name="identifier">The .NET framework identifier.</param>
        /// <param name="children">The child elements with which to initialize the collection.</param>
        internal Framework(NuGetFramework identifier, IReadOnlyCollection<Dependency> children)
            : base(identifier, children)
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
        public static bool operator ==(Framework? left, Framework? right) =>
            Comparer.IsEqual(left, right, ComparisonFunction);

        /// <summary>
        /// Determines if <see paramref="left"/> is not equal to <see paramref="right"/>.
        /// </summary>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <returns><c>true</c> if <see paramref="left"/> is not equal to <see paramref="right"/>; otherwise,
        /// <c>false</c>.</returns>
        public static bool operator !=(Framework? left, Framework? right) =>
            Comparer.IsNotEqual(left, right, ComparisonFunction);

        /// <summary>
        /// Determines if <see paramref="left"/> is less than <see paramref="right"/>.
        /// </summary>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <returns><c>true</c> if <see paramref="left"/> is less than <see paramref="right"/>; otherwise,
        /// <c>false</c>.</returns>
        public static bool operator <(Framework? left, Framework? right) =>
            Comparer.IsLess(left, right, ComparisonFunction);

        /// <summary>
        /// Determines if <see paramref="left"/> is less than or equal to <see paramref="right"/>.
        /// </summary>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <returns><c>true</c> if <see paramref="left"/> is less than or equal to <see paramref="right"/>; otherwise,
        /// <c>false</c>.</returns>
        public static bool operator <=(Framework? left, Framework? right) =>
            Comparer.IsLessOrEqual(left, right, ComparisonFunction);

        /// <summary>
        /// Determines if <see paramref="left"/> is greater than <see paramref="right"/>.
        /// </summary>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <returns><c>true</c> if <see paramref="left"/> is greater than <see paramref="right"/>; otherwise,
        /// <c>false</c>.</returns>
        public static bool operator >(Framework? left, Framework? right) =>
            Comparer.IsGreater(left, right, ComparisonFunction);

        /// <summary>
        /// Determines if <see paramref="left"/> is greater than or equal to <see paramref="right"/>.
        /// </summary>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <returns><c>true</c> if <see paramref="left"/> is greater than or equal to <see paramref="right"/>;
        /// otherwise, <c>false</c>.</returns>
        public static bool operator >=(Framework? left, Framework? right) =>
            Comparer.IsGreaterOrEqual(left, right, ComparisonFunction);

        /// <inheritdoc/>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        public int CompareTo(Framework? other) =>
            Comparer.CompareTo(this, other, ComparisonFunction);

        /// <inheritdoc/>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        public int CompareTo(object? obj) =>
            Comparer.CompareTo(this, obj, ComparisonFunction, nameof(Framework));

        /// <inheritdoc/>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        public bool Equals(Framework? other) =>
            Comparer.Equals(this, other, ComparisonFunction);

        /// <inheritdoc/>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        public override bool Equals(object? obj) =>
            Comparer.Equals(this, obj, ComparisonFunction);

        /// <inheritdoc/>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        public override int GetHashCode() =>
            this.BaseHashCode;

        /// <inheritdoc/>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        public override string ToString() =>
            $"{this.Identifier.Framework} v{this.Identifier.Version.ToShortenedString()}";

        /// <inheritdoc/>
        internal override bool IsAddValid(Dependency child) =>
            true;
    }
}
