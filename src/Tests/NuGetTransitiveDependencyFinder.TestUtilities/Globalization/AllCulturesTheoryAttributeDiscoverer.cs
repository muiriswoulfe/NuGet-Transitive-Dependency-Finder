// <copyright file="AllCulturesTheoryAttributeDiscoverer.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.TestUtilities.Globalization
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Xunit.Abstractions;
    using Xunit.Sdk;

    /// <summary>
    /// An attribute discoverer, for applying all cultures to xUnit.net tests marked with
    /// <see cref="AllCulturesTheoryAttribute"/>.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Performance",
        "CA1812:AvoidUninstantiatedInternalClasses",
        Justification = "Instantiated via reflection.")]
    internal class AllCulturesTheoryAttributeDiscoverer : TheoryDiscoverer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllCulturesTheoryAttributeDiscoverer"/> class.
        /// </summary>
        /// <param name="diagnosticMessageSink">The message sink that receives the test result messages.</param>
        public AllCulturesTheoryAttributeDiscoverer(IMessageSink diagnosticMessageSink)
            : base(diagnosticMessageSink)
        {
        }

        /// <summary>
        /// Creates test cases for a single row of data.
        /// </summary>
        /// <param name="discoveryOptions">The discovery options to use.</param>
        /// <param name="testMethod">The test method to which the current test case belongs.</param>
        /// <param name="theoryAttribute">The theory attribute attached to the test method.</param>
        /// <param name="dataRow">The row of data for the test case.</param>
        /// <returns>The test case to run.</returns>
        protected override IEnumerable<IXunitTestCase> CreateTestCasesForDataRow(
            ITestFrameworkDiscoveryOptions discoveryOptions,
            ITestMethod testMethod,
            IAttributeInfo theoryAttribute,
            object[] dataRow) =>
            AllCulturesBaseAttributeDiscoverer.CreateFactTestCases(
                this.DiagnosticMessageSink,
                discoveryOptions,
                testMethod,
                dataRow);

        /// <summary>
        /// Creates test cases for the entire theory. This is used when one or more of the theory data items are not
        /// serializable or when theory pre-enumeration skipping has been requested.
        /// </summary>
        /// <param name="discoveryOptions">The discovery options to use.</param>
        /// <param name="testMethod">The test method to which the current test case belongs.</param>
        /// <param name="theoryAttribute">The theory attribute attached to the test method.</param>
        /// <returns>The full suite of test cases to run.</returns>
        protected override IEnumerable<IXunitTestCase> CreateTestCasesForTheory(
            ITestFrameworkDiscoveryOptions discoveryOptions,
            ITestMethod testMethod,
            IAttributeInfo theoryAttribute) =>
            AllCulturesBaseAttributeDiscoverer.CreateTheoryTestCases(
                this.DiagnosticMessageSink,
                discoveryOptions,
                testMethod);
    }
}
