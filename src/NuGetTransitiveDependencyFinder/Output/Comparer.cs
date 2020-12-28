// <copyright file="Comparer.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.Output
{
    using System;
    using static System.FormattableString;

    /// <summary>
    /// A base class representing outputted information inclusive of an identifier.
    /// </summary>
    internal static class Comparer
    {
        /// <summary>
        /// Determines if <see paramref="left"/> is equal to <see paramref="right"/>.
        /// </summary>
        /// <typeparam name="TValue">The type to compare.</typeparam>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="function">The comparison logic specific to <c>TValue</c> which takes two objects of type
        /// <c>TValue</c> and returns an <see cref="int"/>.</param>
        /// <returns><c>true</c> if <see paramref="left"/> is equal to <see paramref="right"/>; otherwise,
        /// <c>false</c>.</returns>
        public static bool IsEqual<TValue>(TValue? left, TValue? right, Func<TValue, TValue, int> function)
        {
            if (left is null)
            {
                return right is null;
            }

            return CompareTo(left, right, function) == 0;
        }

        /// <summary>
        /// Determines if <see paramref="left"/> is not equal to <see paramref="right"/>.
        /// </summary>
        /// <typeparam name="TValue">The type to compare.</typeparam>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="function">The comparison logic specific to <c>TValue</c> which takes two objects of type
        /// <c>TValue</c> and returns an <see cref="int"/>.</param>
        /// <returns><c>true</c> if <see paramref="left"/> is not equal to <see paramref="right"/>; otherwise,
        /// <c>false</c>.</returns>
        public static bool IsNotEqual<TValue>(TValue? left, TValue? right, Func<TValue, TValue, int> function) =>
            !IsEqual(left, right, function);

        /// <summary>
        /// Determines if <see paramref="left"/> is less than <see paramref="right"/>.
        /// </summary>
        /// <typeparam name="TValue">The type to compare.</typeparam>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="function">The comparison logic specific to <c>TValue</c> which takes two objects of type
        /// <c>TValue</c> and returns an <see cref="int"/>.</param>
        /// <returns><c>true</c> if <see paramref="left"/> is less than <see paramref="right"/>; otherwise,
        /// <c>false</c>.</returns>
        public static bool IsLess<TValue>(TValue? left, TValue? right, Func<TValue, TValue, int> function)
        {
            if (left is null)
            {
                return right is not null;
            }

            return CompareTo(left, right, function) < 0;
        }

        /// <summary>
        /// Determines if <see paramref="left"/> is less than or equal to <see paramref="right"/>.
        /// </summary>
        /// <typeparam name="TValue">The type to compare.</typeparam>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="function">The comparison logic specific to <c>TValue</c> which takes two objects of type
        /// <c>TValue</c> and returns an <see cref="int"/>.</param>
        /// <returns><c>true</c> if <see paramref="left"/> is less than or equal to <see paramref="right"/>; otherwise,
        /// <c>false</c>.</returns>
        public static bool IsLessOrEqual<TValue>(TValue? left, TValue? right, Func<TValue, TValue, int> function) =>
            IsEqual(left, right, function) || IsLess(left, right, function);

        /// <summary>
        /// Determines if <see paramref="left"/> is greater than <see paramref="right"/>.
        /// </summary>
        /// <typeparam name="TValue">The type to compare.</typeparam>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="function">The comparison logic specific to <c>TValue</c> which takes two objects of type
        /// <c>TValue</c> and returns an <see cref="int"/>.</param>
        /// <returns><c>true</c> if <see paramref="left"/> is greater than <see paramref="right"/>; otherwise,
        /// <c>false</c>.</returns>
        public static bool IsGreater<TValue>(TValue? left, TValue? right, Func<TValue, TValue, int> function)
        {
            if (left is null)
            {
                return false;
            }

            return CompareTo(left, right, function) > 0;
        }

        /// <summary>
        /// Determines if <see paramref="left"/> is greater than or equal to <see paramref="right"/>.
        /// </summary>
        /// <typeparam name="TValue">The type to compare.</typeparam>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="function">The comparison logic specific to <c>TValue</c> which takes two objects of type
        /// <c>TValue</c> and returns an <see cref="int"/>.</param>
        /// <returns><c>true</c> if <see paramref="left"/> is greater than or equal to <see paramref="right"/>;
        /// otherwise, <c>false</c>.</returns>
        public static bool IsGreaterOrEqual<TValue>(TValue? left, TValue? right, Func<TValue, TValue, int> function) =>
            IsEqual(left, right, function) || IsGreater(left, right, function);

        /// <summary>
        /// Compares the current object to <see paramref="other"/>, returning an integer that indicates their
        /// relationship.
        /// </summary>
        /// <typeparam name="TValue">The type to compare.</typeparam>
        /// <param name="current">The object corresponding to <c>this</c>, from the context of the caller.</param>
        /// <param name="other">The object against which to compare <see paramref="current"/>.</param>
        /// <param name="function">The comparison logic specific to <c>TValue</c> which takes two objects of type
        /// <c>TValue</c> and returns an <see cref="int"/>.</param>
        /// <returns>A value less than zero if <see paramref="current"/> is less than <see paramref="other"/>, zero if
        /// <see paramref="current"/> is equal to <see paramref="other"/>, or a value greater than zero if
        /// <see paramref="current"/> is greater than <see paramref="other"/>.</returns>
        public static int CompareTo<TValue>(TValue current, TValue? other, Func<TValue, TValue, int> function)
        {
            if (ReferenceEquals(current, other))
            {
                return 0;
            }

            if (other is null)
            {
                return 1;
            }

            return function(current, other);
        }

        /// <summary>
        /// Compares the current object to <see paramref="other"/>, returning an integer that indicates their
        /// relationship.
        /// </summary>
        /// <typeparam name="TValue">The type to compare.</typeparam>
        /// <exception cref="ArgumentException"><paramref name="obj"/> is not of type <c>TValue</c>.</exception>
        /// <param name="current">The object corresponding to <c>this</c>, from the context of the caller.</param>
        /// <param name="obj">The object against which to compare <see paramref="current"/>.</param>
        /// <param name="function">The comparison logic specific to <c>TValue</c> which takes two objects of type
        /// <c>TValue</c> and returns an <see cref="int"/>.</param>
        /// <param name="className">The name of the calling class, which will be used within the exception
        /// message.</param>
        /// <returns>A value less than zero if <see paramref="current"/> is less than <see paramref="obj"/>, zero if
        /// <see paramref="current"/> is equal to <see paramref="obj"/>, or a value greater than zero if
        /// <see paramref="current"/> is greater than <see paramref="obj"/>.</returns>
        public static int CompareTo<TValue>(
            TValue current,
            object? obj,
            Func<TValue, TValue, int> function,
            string className)
            where TValue : class
        {
            if (obj is null)
            {
                return 1;
            }

            if (obj is not TValue)
            {
                throw new ArgumentException(Invariant($"Object must be of type {className}."), nameof(obj));
            }

            if (ReferenceEquals(current, obj))
            {
                return 0;
            }

            return CompareTo(current, obj as TValue, function);
        }

        /// <summary>
        /// Determines if the current object is equal to <see paramref="other"/>.
        /// </summary>
        /// <typeparam name="TValue">The type to compare.</typeparam>
        /// <param name="current">The object corresponding to <c>this</c>, from the context of the caller.</param>
        /// <param name="other">The object against which to compare <see paramref="current"/>.</param>
        /// <param name="function">The comparison logic specific to <c>TValue</c> which takes two objects of type
        /// <c>TValue</c> and returns an <see cref="int"/>.</param>
        /// <returns><c>true</c> if <see paramref="current"/> is equal to <see paramref="other"/>; otherwise,
        /// <c>false</c>.</returns>
        public static bool Equals<TValue>(TValue current, TValue? other, Func<TValue, TValue, int> function) =>
            CompareTo(current, other, function) == 0;

        /// <summary>
        /// Determines if the current object is equal to <see paramref="obj"/>.
        /// </summary>
        /// <typeparam name="TValue">The type to compare.</typeparam>
        /// <param name="current">The object corresponding to <c>this</c>, from the context of the caller.</param>
        /// <param name="obj">The object against which to compare <see paramref="current"/>.</param>
        /// <param name="function">The comparison logic specific to <c>TValue</c> which takes two objects of type
        /// <c>TValue</c> and returns an <see cref="int"/>.</param>
        /// <returns><c>true</c> if <see paramref="current"/> is equal to <see paramref="obj"/>; otherwise,
        /// <c>false</c>.</returns>
        public static bool Equals<TValue>(TValue current, object? obj, Func<TValue, TValue, int> function)
            where TValue : class =>
            obj is TValue && CompareTo(current, obj as TValue, function, string.Empty) == 0;

        /// <summary>
        /// Maps a value returned from <see cref="StringComparer.Compare(string?, string?)"/>, which can span a
        /// considerable range, to the range [-1, 1] expected from <see cref="IComparable{Dependency}.CompareTo"/>.
        /// </summary>
        /// <param name="value">The value to map.</param>
        /// <returns>The mapped value, which will be in the range [-1, 1].</returns>
        public static int MapCompareTo(int value)
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
