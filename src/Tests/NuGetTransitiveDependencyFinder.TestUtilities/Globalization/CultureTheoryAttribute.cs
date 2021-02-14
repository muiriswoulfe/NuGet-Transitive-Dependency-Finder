// <copyright file="CultureTheoryAttribute.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.TestUtilities.Globalization
{
    using Xunit;
    using Xunit.Sdk;

    /// <summary>
    /// An attribute whose application to an xUnit.net test will result in that test being run for all cultures present
    /// within the system running the tests.
    /// </summary>
    [XunitTestCaseDiscoverer(
        "NuGetTransitiveDependencyFinder.TestUtilities.Globalization.CultureTheoryAttributeDiscoverer",
        "NuGetTransitiveDependencyFinder.TestUtilities.Globalization")]
    public sealed class CultureTheoryAttribute : TheoryAttribute
    {
    }
}
