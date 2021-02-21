// <copyright file="ComparisonTestData.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Output.TestUtilities
{
    /// <summary>
    /// A class encapsulating data for unit testing comparison methods.
    /// </summary>
    /// <param name="Left">The left operand.</param>
    /// <param name="Right">The right operand.</param>
    /// <param name="Comparison">The comparison operators for which the two operands should return <c>true</c>.</param>
    internal record ComparisonTestData<TValue>(TValue? Left, TValue? Right, Comparisons Comparison);
}
