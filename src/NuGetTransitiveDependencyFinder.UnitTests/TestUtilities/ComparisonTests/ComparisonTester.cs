// <copyright file="ComparisonTester.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.TestUtilities.ComparisonTests
{
    using System;
    using FluentAssertions;
    using NuGetTransitiveDependencyFinder.Output;

    /// <summary>
    /// A class for unit testing comparison operators.
    /// </summary>
    internal static class ComparisonTester
    {
        /// <summary>
        /// Tests that when <see cref="Dependency.operator =="/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected response.</param>
        public static void OperatorEqual_CalledWithDifferentValues_ReturnsExpectedValues<TValue>(
            TValue left,
            TValue right,
            bool expected)
            where TValue : class
        {
            // Act
            var result = left == right;

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Dependency.operator !="/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected response.</param>
        public static void OperatorNotEqual_CalledWithDifferentValues_ReturnsExpectedValues<TValue>(
            TValue left,
            TValue right,
            bool expected)
            where TValue : class
        {
            // Act
            var result = left != right;

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="Dependency.operator &lt;"/>, <see cref="Dependency.operator &lt;="/>,
        /// <see cref="Dependency.operator &gt;"/> or <see cref="Dependency.operator &gt;="/> is called with different
        /// values, it returns the expected value in each case.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="function">The operator function to run.</param>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected response.</param>
        public static void Operator_CalledWithDifferentValues_ReturnsExpectedValues<TValue>(
            Func<TValue, TValue, bool> function,
            TValue left,
            TValue right,
            bool expected)
        {
            // Act
            var result = function(left, right);

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="IComparable{TValue}.CompareTo"/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected response.</param>
        public static void CompareTo_CalledWithDifferentValues_ReturnsExpectedValues<TValue>(
            TValue left,
            TValue right,
            int expected)
            where TValue : IComparable<TValue>
        {
            // Act
            var result = left.CompareTo(right);

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="IComparable.CompareTo"/> is called with different values against an
        /// <c>object</c>, it returns the expected value in each case.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected response.</param>
        public static void CompareToObject_CalledWithDifferentValues_ReturnsExpectedValues<TValue>(
            TValue left,
            object right,
            int expected)
            where TValue : IComparable
        {
            // Act
            var result = left.CompareTo(right);

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="IComparable.CompareTo"/> is called with different object types, it throws an
        /// <see cref="ArgumentException"/>.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="left">The left operand to compare.</param>
        public static void CompareToObject_CalledWithDifferentObjectTypes_ThrowsArgumentException<TValue>(
            TValue left)
            where TValue : IComparable
        {
            // Act
            Action action = () => left.CompareTo("value");

            // Assert
            _ = action.Should().Throw<ArgumentException>()
                .WithMessage("Object must be of type Version.");
        }

        /// <summary>
        /// Tests that when <see cref="IEquatable{TValue}.Equals"/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected response.</param>
        public static void Equals_CalledWithDifferentValues_ReturnsExpectedValues<TValue>(
            TValue left,
            TValue right,
            bool expected)
        {
            // Act
            var result = left.Equals(right);

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="IEquatable{TValue}.Equals"/> is called with different values against an
        /// <c>object</c>, it returns the expected value in each case.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected response.</param>
        public static void EqualsObject_CalledWithDifferentValues_ReturnsExpectedValues<TValue>(
            TValue left,
            object right,
            bool expected)
        {
            // Act
            var result = left.Equals(right);

            // Assert
            _ = result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that when <see cref="IEquatable{TValue}.Equals"/> is called with different object types, it returns
        /// <c>false</c>.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="left">The left operand to compare.</param>
        public static void EqualsObject_CalledWithDifferentObjectTypes_ReturnsFalse<TValue>(
            TValue left)
        {
            // Act
            var result = left.Equals("value");

            // Assert
            _ = result.Should().Be(false);
        }

        /// <summary>
        /// Tests that when <see cref="object.GetHashCode()"/> is called with identical objects, it returns the same
        /// value each time.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="value1">The first value for which to compute a hash code.</param>
        /// <param name="value2">The second value for which to compute a hash code.</param>
        public static void GetHashCode_CalledWithIdenticalObjects_ReturnsSameValue<TValue>(
            TValue value1,
            TValue value2)
        {
            // Act
            var result1 = value1.GetHashCode();
            var result2 = value2.GetHashCode();

            // Assert
            _ = result1.Should().Be(result2);
        }

        /// <summary>
        /// Tests that when <see cref="object.GetHashCode()"/> is called with different objects, it returns different
        /// values for each object.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="value1">The first value for which to compute a hash code.</param>
        /// <param name="value2">The second value for which to compute a hash code.</param>
        public static void GetHashCode_CalledWithDifferentObjects_ReturnsDifferentValues<TValue>(
            TValue value1,
            TValue value2)
        {
            // Act
            var result1 = value1.GetHashCode();
            var result2 = value2.GetHashCode();

            // Assert
            _ = result1.Should().NotBe(result2);
        }
    }
}
