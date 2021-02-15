// <copyright file="ComparisonDataGenerator.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Output.TestUtilities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Xunit;

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
        /// <param name="clonedDefaultValue">A clone of <see paramref="defaultValue"/>, where the object contents are
        /// identical, but the object reference is not.</param>
        /// <param name="lesserValue">The lesser test value, which occurs prior to <see paramref="defaultValue"/>
        /// according to an ordered sort.</param>
        /// <param name="supplementalTestData">The supplemental class-specific test data to be added to the generic
        /// test data.</param>
        /// <returns>The generated data.</returns>
        public static IReadOnlyCollection<ComparisonTestData<TValue>> GenerateOperatorTestData<TValue>(
            TValue defaultValue,
            TValue clonedDefaultValue,
            TValue lesserValue,
            IReadOnlyCollection<ComparisonTestData<TValue>> supplementalTestData)
            where TValue : class
        {
            const int initialCapacity = 7;

            var result = new List<ComparisonTestData<TValue>>(initialCapacity + supplementalTestData.Count)
            {
                new(null, null, Comparisons.Equal),
                new(defaultValue, null, Comparisons.GreaterThan),
                new(null, defaultValue, Comparisons.LessThan),
                new(defaultValue, defaultValue, Comparisons.Equal),
                new(defaultValue, clonedDefaultValue, Comparisons.Equal),
                new(lesserValue, defaultValue, Comparisons.LessThan),
                new(defaultValue, lesserValue, Comparisons.GreaterThan),
            };
            result.AddRange(supplementalTestData);

            return result;
        }

        /// <summary>
        /// Generates the data for testing <c>operator ==</c>.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="operatorTestData">The operator test data to be filtered.</param>
        /// <returns>The generated data.</returns>
        public static TheoryData<TValue?, TValue?, bool> GenerateOperatorEqualTestData<TValue>(
            IReadOnlyCollection<ComparisonTestData<TValue>> operatorTestData) =>
            FilterData(operatorTestData, Comparisons.Equal, true);

        /// <summary>
        /// Generates the data for testing <c>operator !=</c>.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="operatorTestData">The operator test data to be filtered.</param>
        /// <returns>The generated data.</returns>
        public static TheoryData<TValue?, TValue?, bool> GenerateOperatorNotEqualTestData<TValue>(
            IReadOnlyCollection<ComparisonTestData<TValue>> operatorTestData) =>
            FilterData(operatorTestData, Comparisons.Equal, false);

        /// <summary>
        /// Generates the data for testing <c>operator &lt;</c>.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="operatorTestData">The operator test data to be filtered.</param>
        /// <returns>The generated data.</returns>
        public static TheoryData<TValue?, TValue?, bool> GenerateOperatorLessThanTestData<TValue>(
            IReadOnlyCollection<ComparisonTestData<TValue>> operatorTestData) =>
            FilterData(operatorTestData, Comparisons.LessThan, true);

        /// <summary>
        /// Generates the data for testing <c>operator &lt;=</c>.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="operatorTestData">The operator test data to be filtered.</param>
        /// <returns>The generated data.</returns>
        public static TheoryData<TValue?, TValue?, bool> GenerateOperatorLessThanOrEqualTestData<TValue>(
            IReadOnlyCollection<ComparisonTestData<TValue>> operatorTestData) =>
            FilterData(operatorTestData, Comparisons.LessThan | Comparisons.Equal, true);

        /// <summary>
        /// Generates the data for testing <c>operator &gt;</c>.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="operatorTestData">The operator test data to be filtered.</param>
        /// <returns>The generated data.</returns>
        public static TheoryData<TValue?, TValue?, bool> GenerateOperatorGreaterThanTestData<TValue>(
            IReadOnlyCollection<ComparisonTestData<TValue>> operatorTestData) =>
            FilterData(operatorTestData, Comparisons.GreaterThan, true);

        /// <summary>
        /// Generates the data for testing <c>operator &gt;=</c>.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="operatorTestData">The operator test data to be filtered.</param>
        /// <returns>The generated data.</returns>
        public static TheoryData<TValue?, TValue?, bool> GenerateOperatorGreaterThanOrEqualTestData<TValue>(
            IReadOnlyCollection<ComparisonTestData<TValue>> operatorTestData) =>
            FilterData(operatorTestData, Comparisons.GreaterThan | Comparisons.Equal, true);

        /// <summary>
        /// Generates the data for testing <see cref="IComparable{TValue}.CompareTo"/>.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="operatorTestData">The operator test data to be filtered.</param>
        /// <returns>The generated data.</returns>
        public static TheoryData<TValue, TValue?, int> GenerateCompareToTestData<TValue>(
            IReadOnlyCollection<ComparisonTestData<TValue>> operatorTestData)
        {
            var result = new TheoryData<TValue, TValue?, int>();

            foreach (var operatorTestDatum in operatorTestData
                .Where(operatorTestDatum => operatorTestDatum.Left is not null))
            {
                result.Add(
                    operatorTestDatum.Left!,
                    operatorTestDatum.Right,
                    ConvertComparisonsToCompareToValue(operatorTestDatum.Comparison));
            }

            return result;
        }

        /// <summary>
        /// Generates the data for testing <see cref="IEquatable{TValue}.Equals"/>.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="operatorTestData">The operator test data to be filtered.</param>
        /// <returns>The generated data.</returns>
        public static TheoryData<TValue, TValue?, bool> GenerateEqualsTestData<TValue>(
            IReadOnlyCollection<ComparisonTestData<TValue>> operatorTestData)
        {
            var result = new TheoryData<TValue, TValue?, bool>();

            foreach (var operatorTestDatum in operatorTestData
                .Where(operatorTestDatum => operatorTestDatum.Left is not null))
            {
                result.Add(
                    operatorTestDatum.Left!,
                    operatorTestDatum.Right,
                    (operatorTestDatum.Comparison & Comparisons.Equal) != 0);
            }

            return result;
        }

        /// <summary>
        /// Generates the data for testing <see cref="object.GetHashCode()"/>.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="defaultValue">The default test value.</param>
        /// <param name="clonedDefaultValue">A clone of <see paramref="defaultValue"/>, where the object contents are
        /// identical, but the object reference is not.</param>
        /// <param name="lesserValue">The lesser test value, which occurs prior to <see paramref="defaultValue"/>
        /// according to an ordered sort.</param>
        /// <param name="supplementalTestData">The supplemental class-specific test data to be added to the generic
        /// test data.</param>
        /// <returns>The generated data.</returns>
        public static TheoryData<TValue, TValue> GenerateGetHashCodeTestData<TValue>(
            TValue defaultValue,
            TValue clonedDefaultValue,
            TValue lesserValue,
            TheoryData<TValue, TValue> supplementalTestData)
        {
            supplementalTestData.Add(defaultValue!, defaultValue!);
            supplementalTestData.Add(lesserValue!, lesserValue!);
            supplementalTestData.Add(defaultValue!, clonedDefaultValue!);
            return supplementalTestData;
        }

        /// <summary>
        /// Filters the <paramref name="operatorTestData"/> using <paramref name="match"/>.
        /// </summary>
        /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
        /// <param name="operatorTestData">The operator test data to be filtered.</param>
        /// <param name="match">The match to apply to <paramref name="operatorTestData"/> when filtering the
        /// data.</param>
        /// <param name="expectedMatchResult">The result expected from applying <paramref name="match"/>.</param>
        /// <returns>The filtered data.</returns>
        private static TheoryData<TValue?, TValue?, bool> FilterData<TValue>(
            IReadOnlyCollection<ComparisonTestData<TValue>> operatorTestData,
            Comparisons match,
            bool expectedMatchResult)
        {
            var result = new TheoryData<TValue?, TValue?, bool>();

            foreach (var operatorTestDatum in operatorTestData)
            {
                var matchResult = (operatorTestDatum.Comparison & match) != 0;
                result.Add(operatorTestDatum.Left, operatorTestDatum.Right, matchResult == expectedMatchResult);
            }

            return result;
        }

        /// <summary>
        /// Converts a <see cref="Comparisons"/> value to the equivalent result from a call to
        /// <see cref="IComparable{TValue}.CompareTo"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        /// <exception cref="InvalidEnumArgumentException"><paramref name="value"/> is set to an unacceptable
        /// value.</exception>
        private static int ConvertComparisonsToCompareToValue(Comparisons value) =>
            value switch
            {
                Comparisons.None =>
                    throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(Comparisons)),
                Comparisons.Equal =>
                    0,
                Comparisons.LessThan =>
                    -1,
                Comparisons.GreaterThan =>
                    1,
                _ =>
                    throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(Comparisons)),
            };
    }
}
