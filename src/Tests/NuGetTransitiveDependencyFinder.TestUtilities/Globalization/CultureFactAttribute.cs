// <copyright file="CultureFactAttribute.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.TestUtilities.Globalization
{
    using Xunit;
    using Xunit.Sdk;

    [XunitTestCaseDiscoverer(
        "NuGetTransitiveDependencyFinder.TestUtilities.Globalization.CultureFactAttributeDiscoverer",
        "NuGetTransitiveDependencyFinder.TestUtilities")]
    public sealed class CultureFactAttribute : FactAttribute
    {
    }
}
