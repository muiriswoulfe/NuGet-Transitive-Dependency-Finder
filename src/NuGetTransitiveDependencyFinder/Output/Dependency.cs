// <copyright file="Dependency.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.Output
{
    using System;
    using NuGet.Versioning;
    using static System.FormattableString;

    /// <summary>
    /// A class representing the outputted .NET dependency information for each project and framework combination.
    /// </summary>
    public sealed class Dependency : IComparable, IComparable<Dependency>, IEquatable<Dependency>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Dependency"/> class.
        /// </summary>
        /// <param name="identifier">The dependency identifier.</param>
        /// <param name="version">The dependency version.</param>
        internal Dependency(string identifier, NuGetVersion version)
        {
            this.Identifier = identifier;
            this.Version = version;
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
        /// Gets a value indicating whether the dependency is a transitive dependency.
        /// </summary>
        public bool IsTransitive { get; internal set; }

        /// <summary>
        /// Determines if <see paramref="left"/> is equal to <see paramref="right"/>.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <returns><c>true</c> if <see paramref="left"/> is equal to <see paramref="right"/>; otherwise,
        /// <c>false</c>.</returns>
        public static bool operator ==(Dependency? left, Dependency? right)
        {
            if (left is null)
            {
                return right is null;
            }

            return left.CompareTo(right) == 0;
        }

        /// <summary>
        /// Determines if <see paramref="left"/> is not equal to <see paramref="right"/>.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <returns><c>true</c> if <see paramref="left"/> is not equal to <see paramref="right"/>; otherwise,
        /// <c>false</c>.</returns>
        public static bool operator !=(Dependency? left, Dependency? right) =>
            !(left == right);

        /// <summary>
        /// Determines if <see paramref="left"/> is less than <see paramref="right"/>.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <returns><c>true</c> if <see paramref="left"/> is less than <see paramref="right"/>; otherwise,
        /// <c>false</c>.</returns>
        public static bool operator <(Dependency? left, Dependency? right)
        {
            if (left is null)
            {
                return right is not null;
            }

            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Determines if <see paramref="left"/> is less than or equal to <see paramref="right"/>.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <returns><c>true</c> if <see paramref="left"/> is less than or equal to <see paramref="right"/>; otherwise,
        /// <c>false</c>.</returns>
        public static bool operator <=(Dependency? left, Dependency? right) =>
            (left == right) || (left < right);

        /// <summary>
        /// Determines if <see paramref="left"/> is greater than <see paramref="right"/>.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <returns><c>true</c> if <see paramref="left"/> is greater than <see paramref="right"/>; otherwise,
        /// <c>false</c>.</returns>
        public static bool operator >(Dependency? left, Dependency? right) =>
            left?.CompareTo(right) > 0;

        /// <summary>
        /// Determines if <see paramref="left"/> is greater than or equal to <see paramref="right"/>.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <returns><c>true</c> if <see paramref="left"/> is greater than or equal to <see paramref="right"/>;
        /// otherwise, <c>false</c>.</returns>
        public static bool operator >=(Dependency? left, Dependency? right) =>
            (left == right) || (left > right);

        /// <summary>
        /// Compares the current object to <see paramref="other"/>, returning an integer that indicates their
        /// relationship.
        /// </summary>
        /// <param name="other">The object against which to compare the current object.</param>
        /// <returns>A value less than zero if the current object is less than <see paramref="other"/>, zero if the
        /// current object is equal to <see paramref="other"/>, or a value greater than zero if the current object is
        /// greater than <see paramref="other"/>.</returns>
        public int CompareTo(Dependency? other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (other is null)
            {
                return 1;
            }

            var result = MapCompareTo(StringComparer.OrdinalIgnoreCase.Compare(this.Identifier, other.Identifier));
            return result != 0
                ? result
                : MapCompareTo(
                    StringComparer.OrdinalIgnoreCase.Compare(this.Version.ToString(), other.Version.ToString()));
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentException"><paramref name="obj"/> is not of type
        /// <see cref="Dependency"/>.</exception>
        public int CompareTo(object? obj)
        {
            if (obj is null)
            {
                return 1;
            }

            if (obj is not Dependency)
            {
                throw new ArgumentException(Invariant($"Object must be of type {nameof(Dependency)}."), nameof(obj));
            }

            if (ReferenceEquals(this, obj))
            {
                return 0;
            }

            return this.CompareTo(obj as Dependency);
        }

        /// <inheritdoc/>
        public bool Equals(Dependency? other) =>
            this.CompareTo(other) == 0;

        /// <inheritdoc/>
        public override bool Equals(object? obj) =>
            obj is Dependency && this.CompareTo(obj) == 0;

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
