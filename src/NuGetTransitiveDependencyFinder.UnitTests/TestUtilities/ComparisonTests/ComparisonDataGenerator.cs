// <copyright file="ComparisonDataGenerator.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.TestUtilities.ComparisonTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class encapsulating <c>static</c> methods for unit testing comparison methods.
    /// </summary>
    internal static class ComparisonDataGenerator
    {
        /// <summary>
        /// Generates the data for testing the operators.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="defaultValue">The default test value.</param>
        /// <param name="lesserValue">The lesser test value, which must occur prior to the default test value according
        /// to an ordered sort.</param>
        /// <returns>The generated data.</returns>
        public static IList<ComparisonTestData<TValue>> GenerateOperatorTestData<TValue>(
            TValue defaultValue,
            TValue lesserValue)
            where TValue : class, ICloneable =>
            new List<ComparisonTestData<TValue>>
            {
                new ComparisonTestData<TValue>(null, null, Comparisons.Equal),
                new ComparisonTestData<TValue>(defaultValue, null, Comparisons.GreaterThan),
                new ComparisonTestData<TValue>(null, defaultValue, Comparisons.LessThan),
                new ComparisonTestData<TValue>(defaultValue, defaultValue, Comparisons.Equal),
                new ComparisonTestData<TValue>(defaultValue, defaultValue.Clone() as TValue, Comparisons.Equal),
                new ComparisonTestData<TValue>(lesserValue, defaultValue, Comparisons.LessThan),
                new ComparisonTestData<TValue>(defaultValue, lesserValue, Comparisons.GreaterThan),
            };

        /// <summary>
        /// Generates the data for testing <c>operator ==</c>.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="operatorTestData">The operator test data to be filtered.</param>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> GenerateOperatorEqualTestData<TValue>(
            IList<ComparisonTestData<TValue>> operatorTestData) =>
            FilterData(operatorTestData, Comparisons.Equal, true);

        /// <summary>
        /// Generates the data for testing <c>operator !=</c>.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="operatorTestData">The operator test data to be filtered.</param>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> GenerateOperatorNotEqualTestData<TValue>(
            IList<ComparisonTestData<TValue>> operatorTestData) =>
            FilterData(operatorTestData, Comparisons.Equal, false);

        /// <summary>
        /// Generates the data for testing <c>operator &lt;</c>.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="operatorTestData">The operator test data to be filtered.</param>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> GenerateOperatorLessThanTestData<TValue>(
            IList<ComparisonTestData<TValue>> operatorTestData) =>
            FilterData(operatorTestData, Comparisons.LessThan, true);

        /// <summary>
        /// Generates the data for testing <c>operator &lt;=</c>.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="operatorTestData">The operator test data to be filtered.</param>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> GenerateOperatorLessThanOrEqualTestData<TValue>(
            IList<ComparisonTestData<TValue>> operatorTestData) =>
            FilterData(operatorTestData, Comparisons.LessThan | Comparisons.Equal, true);

        /// <summary>
        /// Generates the data for testing <c>operator &gt;</c>.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="operatorTestData">The operator test data to be filtered.</param>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> GenerateOperatorGreaterThanTestData<TValue>(
            IList<ComparisonTestData<TValue>> operatorTestData) =>
            FilterData(operatorTestData, Comparisons.GreaterThan, true);

        /// <summary>
        /// Generates the data for testing <c>operator &gt;=</c>.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="operatorTestData">The operator test data to be filtered.</param>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> GenerateOperatorGreaterThanOrEqualTestData<TValue>(
            IList<ComparisonTestData<TValue>> operatorTestData) =>
            FilterData(operatorTestData, Comparisons.GreaterThan | Comparisons.Equal, true);

        /// <summary>
        /// Generates the data for testing <see cref="IComparable{TValue}.CompareTo"/>.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="operatorTestData">The operator test data to be filtered.</param>
        /// <exception cref="NotSupportedException">Not supported.</exception>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> GenerateCompareToTestData<TValue>(
            IList<ComparisonTestData<TValue>> operatorTestData)
        {
            foreach (var operatorTestDatum in operatorTestData
                .Where(operatorTestDatum => operatorTestDatum.Left is not null))
            {
                var compareToResult = operatorTestDatum.Comparison switch
                {
                    Comparisons.None => throw new NotSupportedException(),
                    Comparisons.Equal => 0,
                    Comparisons.LessThan => -1,
                    Comparisons.GreaterThan => 1,
                    _ => throw new NotSupportedException(),
                };

                yield return new object[] { operatorTestDatum.Left, operatorTestDatum.Right, compareToResult };
            }
        }

        /// <summary>
        /// Generates the data for testing <see cref="IEquatable{TValue}.Equals"/>.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="operatorTestData">The operator test data to be filtered.</param>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> GenerateEqualsTestData<TValue>(
            IList<ComparisonTestData<TValue>> operatorTestData)
        {
            foreach (var operatorTestDatum in operatorTestData
                .Where(operatorTestDatum => operatorTestDatum.Left is not null))
            {
                yield return new object[]
                {
                    operatorTestDatum.Left,
                    operatorTestDatum.Right,
                    (operatorTestDatum.Comparison & Comparisons.Equal) != 0,
                };
            }
        }

        /// <summary>
        /// Generates the data for testing <see cref="object.GetHashCode()"/>.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="defaultValue">The default test value.</param>
        /// <param name="lesserValue">The lesser test value, which must occur prior to the default test value according
        /// to an ordered sort.</param>
        /// <returns>The generated data.</returns>
        public static IEnumerable<object[]> GenerateGetHashCodeTestData<TValue>(
            TValue defaultValue,
            TValue lesserValue)
            where TValue : ICloneable =>
            new object[][]
            {
                new object[] { defaultValue, defaultValue },
                new object[] { lesserValue, lesserValue },
                new object[] { defaultValue, defaultValue.Clone() },
            };

        /// <summary>
        /// Filters the <paramref name="operatorTestData"/> using <paramref name="match"/>.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="operatorTestData">The operator test data to be filtered.</param>
        /// <param name="match">The match to apply to <paramref name="operatorTestData"/> when filtering the
        /// data.</param>
        /// <param name="expectedMatchResult">The result expected from applying <paramref name="match"/>.</param>
        /// <returns>The filtered data.</returns>
        private static IEnumerable<object[]> FilterData<TValue>(
            IList<ComparisonTestData<TValue>> operatorTestData,
            Comparisons match,
            bool expectedMatchResult)
        {
            foreach (var operatorTestDatum in operatorTestData)
            {
                var matchResult = (operatorTestDatum.Comparison & match) != 0;
                yield return new object[]
                {
                    operatorTestDatum.Left,
                    operatorTestDatum.Right,
                    matchResult == expectedMatchResult,
                };
            }
        }
    }
}
