// <copyright file="Dependency.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.Output
{
    using System;
    using System.Collections.Generic;
    using NuGet.Versioning;

    /// <summary>
    /// A class representing the outputted .NET dependency information for each project and framework combination.
    /// </summary>
    public sealed class Dependency : IComparable, IComparable<Dependency>, IEquatable<Dependency>
    {
        /// <summary>
        /// The comparison logic specific to <see cref="Dependency"/>, which takes two objects of type
        /// <see cref="Dependency"/> and returns an <see cref="int"/>.
        /// </summary>
        private static readonly Func<Dependency, Dependency, int> ComparisonFunction =
            (Dependency current, Dependency other) =>
            {
                var result = Comparer.MapCompareTo(
                    StringComparer.OrdinalIgnoreCase.Compare(current.Identifier, other.Identifier));

                return result != 0
                    ? result
                    : Comparer.MapCompareTo(
                        StringComparer.OrdinalIgnoreCase.Compare(current.Version.ToString(), other.Version.ToString()));
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="Dependency"/> class.
        /// </summary>
        /// <param name="identifier">The dependency identifier.</param>
        /// <param name="version">The dependency version.</param>
        internal Dependency(string identifier, NuGetVersion version)
        {
            this.Identifier = identifier;
            this.Version = version;
            this.Via = new HashSet<Dependency>();
        }

        /// <summary>
        /// Gets the dependency identifier.
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// Gets the dependency version.
        /// </summary>
        public NuGetVersion Version { get; }

        /// <summary>
        /// Gets the set of dependencies that provide this dependency.
        /// </summary>
        public ISet<Dependency> Via { get; }

        /// <summary>
        /// Gets a value indicating whether the dependency is a transitive dependency.
        /// </summary>
        public bool IsTransitive { get; internal set; }

        /// <summary>
        /// Determines if <see paramref="left"/> is equal to <see paramref="right"/>.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <returns><see langword="true"/> if <see paramref="left"/> is equal to <see paramref="right"/>; otherwise,
        /// <see langword="false"/>.</returns>
        public static bool operator ==(Dependency? left, Dependency? right) =>
            Comparer.IsEqual(left, right, ComparisonFunction);

        /// <summary>
        /// Determines if <see paramref="left"/> is not equal to <see paramref="right"/>.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <returns><see langword="true"/> if <see paramref="left"/> is not equal to <see paramref="right"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(Dependency? left, Dependency? right) =>
            Comparer.IsNotEqual(left, right, ComparisonFunction);

        /// <summary>
        /// Determines if <see paramref="left"/> is less than <see paramref="right"/>.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <returns><see langword="true"/> if <see paramref="left"/> is less than <see paramref="right"/>; otherwise,
        /// <see langword="false"/>.</returns>
        public static bool operator <(Dependency? left, Dependency? right) =>
            Comparer.IsLess(left, right, ComparisonFunction);

        /// <summary>
        /// Determines if <see paramref="left"/> is less than or equal to <see paramref="right"/>.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <returns><see langword="true"/> if <see paramref="left"/> is less than or equal to <see paramref="right"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator <=(Dependency? left, Dependency? right) =>
            Comparer.IsLessOrEqual(left, right, ComparisonFunction);

        /// <summary>
        /// Determines if <see paramref="left"/> is greater than <see paramref="right"/>.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <returns><see langword="true"/> if <see paramref="left"/> is greater than <see paramref="right"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator >(Dependency? left, Dependency? right) =>
            Comparer.IsGreater(left, right, ComparisonFunction);

        /// <summary>
        /// Determines if <see paramref="left"/> is greater than or equal to <see paramref="right"/>.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <returns><see langword="true"/> if <see paramref="left"/> is greater than or equal to
        /// <see paramref="right"/>; otherwise, <see langword="false"/>.</returns>
        public static bool operator >=(Dependency? left, Dependency? right) =>
            Comparer.IsGreaterOrEqual(left, right, ComparisonFunction);

        /// <summary>
        /// Compares the current object to <see paramref="other"/>, returning an integer that indicates their
        /// relationship.
        /// </summary>
        /// <param name="other">The object against which to compare the current object.</param>
        /// <returns>A value less than zero if the current object is less than <see paramref="other"/>, zero if the
        /// current object is equal to <see paramref="other"/>, or a value greater than zero if the current object is
        /// greater than <see paramref="other"/>.</returns>
        public int CompareTo(Dependency? other) =>
            Comparer.CompareTo(this, other, ComparisonFunction);

        /// <inheritdoc/>
        /// <exception cref="ArgumentException"><paramref name="obj"/> is not of type
        /// <see cref="Dependency"/>.</exception>
        public int CompareTo(object? obj) =>
            Comparer.CompareTo(this, obj, ComparisonFunction, nameof(Dependency));

        /// <inheritdoc/>
        public bool Equals(Dependency? other) =>
            Comparer.Equals(this, other, ComparisonFunction);

        /// <inheritdoc/>
        public override bool Equals(object? obj) =>
            Comparer.Equals(this, obj, ComparisonFunction);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            const int startingPrime = 3049;
            const int multiplicativePrime = 5039;

            var result = startingPrime;

            unchecked
            {
                result += StringComparer.OrdinalIgnoreCase.GetHashCode(this.Identifier);
                result *= multiplicativePrime;
                result += StringComparer.OrdinalIgnoreCase.GetHashCode(this.Version.ToString());
            }

            return result;
        }

        /// <inheritdoc/>
        public override string ToString() =>
            $"{this.Identifier} v{this.Version}";
    }
}
