// <copyright file="AllCulturesFactAttributeDiscoverer.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.TestUtilities.Globalization;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit.Abstractions;
using Xunit.Sdk;

/// <summary>
/// An attribute discoverer, for applying all cultures to xUnit.net tests marked with
/// <see cref="AllCulturesFactAttribute"/>.
/// </summary>
[SuppressMessage(
    "Microsoft.Performance",
    "CA1812:AvoidUninstantiatedInternalClasses",
    Justification = "Instantiated via reflection.")]
internal class AllCulturesFactAttributeDiscoverer : IXunitTestCaseDiscoverer
{
    /// <summary>
    /// The message sink that receives the test result messages.
    /// </summary>
    private readonly IMessageSink diagnosticMessageSink;

    /// <summary>
    /// Initializes a new instance of the <see cref="AllCulturesFactAttributeDiscoverer"/> class.
    /// </summary>
    /// <param name="diagnosticMessageSink">The message sink that receives the test result messages.</param>
    public AllCulturesFactAttributeDiscoverer(IMessageSink diagnosticMessageSink) =>
        this.diagnosticMessageSink = diagnosticMessageSink;

    /// <summary>
    /// Discovers the full suite of test cases, where each test case validates a single culture, corresponding to each
    /// test method marked with <see cref="AllCulturesFactAttribute"/>.
    /// </summary>
    /// <param name="discoveryOptions">The discovery options to use.</param>
    /// <param name="testMethod">The test method to which the current test case belongs.</param>
    /// <param name="factAttribute">The fact attribute attached to the test method.</param>
    /// <returns>The full suite of test cases to run.</returns>
    public IEnumerable<IXunitTestCase> Discover(
        ITestFrameworkDiscoveryOptions discoveryOptions,
        ITestMethod testMethod,
        IAttributeInfo factAttribute) =>
        AllCulturesBaseAttributeDiscoverer.CreateFactTestCases(
            this.diagnosticMessageSink,
            discoveryOptions,
            testMethod);
}
