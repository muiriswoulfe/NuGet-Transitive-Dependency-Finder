// <copyright file="Project.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.Output
{
    using System;

    /// <summary>
    /// A class representing the outputted project information.
    /// </summary>
    /// <remarks>The child elements of this class are the <see cref="Framework"/> objects.</remarks>
    public sealed class Project :
        IdentifiedBase<string, Framework>,
        IComparable,
        IComparable<Project>,
        IEquatable<Project>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Project"/> class.
        /// </summary>
        /// <param name="identifier">The project identifier.</param>
        /// <param name="capacity">The quantity of .NET framework identifiers for which the collection initially has
        /// adequate capacity.</param>
        internal Project(string identifier, int capacity)
            : base(identifier, capacity)
        {
        }

        /// <summary>
        /// Determines if <see paramref="left"/> is equal to <see paramref="right"/>.
        /// </summary>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <returns><see langword="true"/> if <see paramref="left"/> is equal to <see paramref="right"/>; otherwise,
        /// <see langword="false"/>.</returns>
        public static bool operator ==(Project? left, Project? right) =>
            Comparer.IsEqual(left, right, ComparisonFunction);

        /// <summary>
        /// Determines if <see paramref="left"/> is not equal to <see paramref="right"/>.
        /// </summary>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <returns><see langword="true"/> if <see paramref="left"/> is not equal to <see paramref="right"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(Project? left, Project? right) =>
            Comparer.IsNotEqual(left, right, ComparisonFunction);

        /// <summary>
        /// Determines if <see paramref="left"/> is less than <see paramref="right"/>.
        /// </summary>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <returns><see langword="true"/> if <see paramref="left"/> is less than <see paramref="right"/>; otherwise,
        /// <see langword="false"/>.</returns>
        public static bool operator <(Project? left, Project? right) =>
            Comparer.IsLess(left, right, ComparisonFunction);

        /// <summary>
        /// Determines if <see paramref="left"/> is less than or equal to <see paramref="right"/>.
        /// </summary>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <returns><see langword="true"/> if <see paramref="left"/> is less than or equal to <see paramref="right"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator <=(Project? left, Project? right) =>
            Comparer.IsLessOrEqual(left, right, ComparisonFunction);

        /// <summary>
        /// Determines if <see paramref="left"/> is greater than <see paramref="right"/>.
        /// </summary>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <returns><see langword="true"/> if <see paramref="left"/> is greater than <see paramref="right"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator >(Project? left, Project? right) =>
            Comparer.IsGreater(left, right, ComparisonFunction);

        /// <summary>
        /// Determines if <see paramref="left"/> is greater than or equal to <see paramref="right"/>.
        /// </summary>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <returns><see langword="true"/> if <see paramref="left"/> is greater than or equal to
        /// <see paramref="right"/>; otherwise, <see langword="false"/>.</returns>
        public static bool operator >=(Project? left, Project? right) =>
            Comparer.IsGreaterOrEqual(left, right, ComparisonFunction);

        /// <inheritdoc/>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        public int CompareTo(Project? other) =>
            Comparer.CompareTo(this, other, ComparisonFunction);

        /// <inheritdoc/>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        public int CompareTo(object? obj) =>
            Comparer.CompareTo(this, obj, ComparisonFunction, nameof(Project));

        /// <inheritdoc/>
        /// <remarks>The result of this method is solely dependent on
        /// <see cref="IdentifiedBase{TIdentifier, TChild}.Identifier"/>.</remarks>
        public bool Equals(Project? other) =>
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
        internal override bool IsAddValid(Framework child) =>
            child.HasChildren;
    }
}
