// <copyright file="ComparisonTestData.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Output.TestUtilities;

/// <summary>
/// A record encapsulating data for unit testing comparison methods.
/// </summary>
internal record ComparisonTestData<TValue>(TValue? Left, TValue? Right, Comparisons Comparison);
