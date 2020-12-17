// <copyright file="VersionUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Output
{
    using System;
    using System.Collections.Generic;
    using NuGetTransitiveDependencyFinder.UnitTests.TestUtilities.ComparisonTests;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="Version"/> class.
    /// </summary>
    public class VersionUnitTests
    {
        /// <summary>
        /// The default identifier.
        /// </summary>
        private static readonly Version DefaultValue = new Version(1, 0, 0, 0);

        /// <summary>
        /// The lesser identifier.
        /// </summary>
        private static readonly Version LesserValue = new Version(0, 9, 9, 9);

        /// <summary>
        /// The data for testing the operators.
        /// </summary>
        private static readonly IList<ComparisonTestData<Version>> OperatorTestData =
            ComparisonDataGenerator.GenerateOperatorTestData(DefaultValue, LesserValue);

        /// <summary>
        /// Gets the data for testing <see cref="Version.operator =="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> OperatorEqualTestData =>
            ComparisonDataGenerator.GenerateOperatorEqualTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Version.operator !="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> OperatorNotEqualTestData =>
            ComparisonDataGenerator.GenerateOperatorNotEqualTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Version.operator &lt;"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> OperatorLessThanTestData =>
            ComparisonDataGenerator.GenerateOperatorLessThanTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Version.operator &lt;="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> OperatorLessThanOrEqualTestData =>
            ComparisonDataGenerator.GenerateOperatorLessThanOrEqualTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Version.operator &gt;"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> OperatorGreaterThanTestData =>
            ComparisonDataGenerator.GenerateOperatorGreaterThanTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Version.operator &gt;="/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> OperatorGreaterThanOrEqualTestData =>
            ComparisonDataGenerator.GenerateOperatorGreaterThanOrEqualTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="IComparable{Version}.CompareTo"/>.
        /// </summary>
        /// <exception cref="NotSupportedException">Not supported.</exception>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> CompareToTestData =>
            ComparisonDataGenerator.GenerateCompareToTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="IEquatable{Version}.Equals"/>.
        /// </summary>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> EqualsTestData =>
            ComparisonDataGenerator.GenerateEqualsTestData(OperatorTestData);

        /// <summary>
        /// Gets the data for testing <see cref="Version.GetHashCode()"/>.
        /// </summary>
        public static IEnumerable<object[]> GetHashCodeTestData =>
            ComparisonDataGenerator.GenerateGetHashCodeTestData(DefaultValue, LesserValue);

        /// <summary>
        /// Tests that when <see cref="Version.operator =="/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected response.</param>
        [Theory]
        [MemberData(nameof(OperatorEqualTestData))]
        public void OperatorEqual_CalledWithDifferentValues_ReturnsExpectedValues(
            Version left,
            Version right,
            bool expected) =>
            ComparisonTester.OperatorEqual_CalledWithDifferentValues_ReturnsExpectedValues(left, right, expected);

        /// <summary>
        /// Tests that when <see cref="Version.operator !="/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected response.</param>
        [Theory]
        [MemberData(nameof(OperatorNotEqualTestData))]
        public void OperatorNotEqual_CalledWithDifferentValues_ReturnsExpectedValues(
            Version left,
            Version right,
            bool expected) =>
            ComparisonTester.OperatorNotEqual_CalledWithDifferentValues_ReturnsExpectedValues(left, right, expected);

        /// <summary>
        /// Tests that when <see cref="Version.operator &lt;"/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected response.</param>
        [Theory]
        [MemberData(nameof(OperatorLessThanTestData))]
        public void OperatorLessThan_CalledWithDifferentValues_ReturnsExpectedValues(
            Version left,
            Version right,
            bool expected) =>
            ComparisonTester.Operator_CalledWithDifferentValues_ReturnsExpectedValues(
                (Version left, Version right) => left < right,
                left,
                right,
                expected);

        /// <summary>
        /// Tests that when <see cref="Version.operator &lt;="/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected response.</param>
        [Theory]
        [MemberData(nameof(OperatorLessThanOrEqualTestData))]
        public void OperatorLessThanOrEqual_CalledWithDifferentValues_ReturnsExpectedValues(
            Version left,
            Version right,
            bool expected) =>
            ComparisonTester.Operator_CalledWithDifferentValues_ReturnsExpectedValues(
                (Version left, Version right) => left <= right,
                left,
                right,
                expected);

        /// <summary>
        /// Tests that when <see cref="Version.operator &gt;"/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected response.</param>
        [Theory]
        [MemberData(nameof(OperatorGreaterThanTestData))]
        public void OperatorGreaterThan_CalledWithDifferentValues_ReturnsExpectedValues(
            Version left,
            Version right,
            bool expected) =>
            ComparisonTester.Operator_CalledWithDifferentValues_ReturnsExpectedValues(
                (Version left, Version right) => left > right,
                left,
                right,
                expected);

        /// <summary>
        /// Tests that when <see cref="Version.operator &gt;="/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected response.</param>
        [Theory]
        [MemberData(nameof(OperatorGreaterThanOrEqualTestData))]
        public void OperatorGreaterThanOrEqual_CalledWithDifferentValues_ReturnsExpectedValues(
            Version left,
            Version right,
            bool expected) =>
            ComparisonTester.Operator_CalledWithDifferentValues_ReturnsExpectedValues(
                (Version left, Version right) => left >= right,
                left,
                right,
                expected);

        /// <summary>
        /// Tests that when <see cref="IComparable{Version}.CompareTo"/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected response.</param>
        [Theory]
        [MemberData(nameof(CompareToTestData))]
        public void CompareTo_CalledWithDifferentValues_ReturnsExpectedValues(
            Version left,
            Version right,
            int expected) =>
            ComparisonTester.CompareTo_CalledWithDifferentValues_ReturnsExpectedValues(left, right, expected);

        /// <summary>
        /// Tests that when <see cref="IComparable.CompareTo"/> is called with different values against an
        /// <c>object</c>, it returns the expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected response.</param>
        [Theory]
        [MemberData(nameof(CompareToTestData))]
        public void CompareToObject_CalledWithDifferentValues_ReturnsExpectedValues(
            Version left,
            object right,
            int expected) =>
            ComparisonTester.CompareToObject_CalledWithDifferentValues_ReturnsExpectedValues(left, right, expected);

        /// <summary>
        /// Tests that when <see cref="IComparable.CompareTo"/> is called with different object types, it throws an
        /// <see cref="ArgumentException"/>.
        /// </summary>
        [Fact]
        public void CompareToObject_CalledWithDifferentObjectTypes_ThrowsArgumentException() =>
            ComparisonTester.CompareToObject_CalledWithDifferentObjectTypes_ThrowsArgumentException(DefaultValue);

        /// <summary>
        /// Tests that when <see cref="IEquatable{Version}.Equals"/> is called with different values, it returns the
        /// expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected response.</param>
        [Theory]
        [MemberData(nameof(EqualsTestData))]
        public void Equals_CalledWithDifferentValues_ReturnsExpectedValues(
            Version left,
            Version right,
            bool expected) =>
            ComparisonTester.Equals_CalledWithDifferentValues_ReturnsExpectedValues(left, right, expected);

        /// <summary>
        /// Tests that when <see cref="IEquatable{Version}.Equals"/> is called with different values against an
        /// <c>object</c>, it returns the expected value in each case.
        /// </summary>
        /// <param name="left">The left operand to compare.</param>
        /// <param name="right">The right operand to compare.</param>
        /// <param name="expected">The expected response.</param>
        [Theory]
        [MemberData(nameof(EqualsTestData))]
        public void EqualsObject_CalledWithDifferentValues_ReturnsExpectedValues(
            Version left,
            object right,
            bool expected) =>
            ComparisonTester.EqualsObject_CalledWithDifferentValues_ReturnsExpectedValues(left, right, expected);

        /// <summary>
        /// Tests that when <see cref="IEquatable{Version}.Equals"/> is called with different object types, it returns
        /// <c>false</c>.
        /// </summary>
        [Fact]
        public void EqualsObject_CalledWithDifferentObjectTypes_ReturnsFalse() =>
            ComparisonTester.EqualsObject_CalledWithDifferentObjectTypes_ReturnsFalse(DefaultValue);

        /// <summary>
        /// Tests that when <see cref="Version.GetHashCode()"/> is called with identical objects, it returns the same
        /// value each time.
        /// </summary>
        /// <param name="value1">The first value for which to compute a hash code.</param>
        /// <param name="value2">The second value for which to compute a hash code.</param>
        [Theory]
        [MemberData(nameof(GetHashCodeTestData))]
        public void GetHashCode_CalledWithIdenticalObjects_ReturnsSameValue(
            Version value1,
            Version value2) =>
            ComparisonTester.GetHashCode_CalledWithIdenticalObjects_ReturnsSameValue(value1, value2);

        /// <summary>
        /// Tests that when <see cref="Version.GetHashCode()"/> is called with different objects, it returns different
        /// values for each object.
        /// </summary>
        [Fact]
        public void GetHashCode_CalledWithDifferentObjects_ReturnsDifferentValues() =>
            ComparisonTester.GetHashCode_CalledWithDifferentObjects_ReturnsDifferentValues(DefaultValue, LesserValue);
    }
}
