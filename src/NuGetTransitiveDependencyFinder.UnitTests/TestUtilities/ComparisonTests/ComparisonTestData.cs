// <copyright file="ComparisonTestData.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.TestUtilities.ComparisonTests
{
    /// <summary>
    /// A class encapsulating data for unit testing comparison methods.
    /// </summary>
    /// <typeparam name="TValue">The type to use for the comparisons.</typeparam>
    internal class ComparisonTestData<TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComparisonTestData{TValue}"/> class.
        /// </summary>
        /// <param name="left">The left value for each comparison test.</param>
        /// <param name="right">The right value for each comparison test.</param>
        /// <param name="comparison">The collection of comparisons that should return <c>true</c>.</param>
        public ComparisonTestData(TValue left, TValue right, Comparisons comparison)
        {
            this.Left = left;
            this.Right = right;
            this.Comparison = comparison;
        }

        /// <summary>
        /// Gets the left value for each comparison test.
        /// </summary>
        public TValue Left { get; }

        /// <summary>
        /// Gets the right value for each comparison test.
        /// </summary>
        public TValue Right { get; }

        /// <summary>
        /// Gets the collection of comparisons that should return <c>true</c>.
        /// </summary>
        public Comparisons Comparison { get; }
    }
}
