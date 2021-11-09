// <copyright file="Comparisons.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Output.TestUtilities;

using System;

/// <summary>
/// An enumeration of comparisons, for use within the unit tests.
/// </summary>
[Flags]
internal enum Comparisons
{
    /// <summary>
    /// An enumeration value indicating that no comparisons apply.
    /// </summary>
    None = 0,

    /// <summary>
    /// An enumeration value indicating that the left value is equal to the right value.
    /// </summary>
    Equal = 1 << 0,

    /// <summary>
    /// An enumeration value indicating that the left value is less than the right value.
    /// </summary>
    LessThan = 1 << 1,

    /// <summary>
    /// An enumeration value indicating that the left value is greater than the right value.
    /// </summary>
    GreaterThan = 1 << 2,
}
