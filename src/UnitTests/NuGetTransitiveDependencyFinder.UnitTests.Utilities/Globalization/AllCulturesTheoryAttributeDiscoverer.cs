// <copyright file="AllCulturesTheoryAttributeDiscoverer.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Utilities.Globalization;

using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit.Sdk;

/// <summary>
/// An attribute discoverer, for applying all cultures to xUnit.net tests marked with
/// <see cref="AllCulturesTheoryAttribute"/>.
/// </summary>
/// <param name="diagnosticMessageSink">The message sink that receives the test result messages.</param>
internal class AllCulturesTheoryAttributeDiscoverer(
    IMessageSink diagnosticMessageSink) : TheoryDiscoverer(diagnosticMessageSink)
{
    /// <summary>
    /// Discovers the full suite of test cases, where each test case validates a single culture, corresponding to each
    /// test method marked with <see cref="AllCulturesTheoryAttribute"/>.
    /// </summary>
    /// <param name="discoveryOptions">The discovery options to use.</param>
    /// <param name="testMethod">The test method to which the current test case belongs.</param>
    /// <param name="theoryAttribute">The theory attribute attached to the test method.</param>
    /// <returns>The full suite of test cases to run.</returns>
    public override IEnumerable<IXunitTestCase> Discover(
        ITestFrameworkDiscoveryOptions discoveryOptions,
        ITestMethod testMethod,
        IAttributeInfo theoryAttribute) =>
        AllCulturesBaseAttributeDiscoverer.CreateTheoryTestCases(
            this.DiagnosticMessageSink,
            discoveryOptions,
            testMethod);
}
