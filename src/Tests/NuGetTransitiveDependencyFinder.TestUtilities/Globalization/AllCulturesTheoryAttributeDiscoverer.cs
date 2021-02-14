// <copyright file="AllCulturesTheoryAttributeDiscoverer.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.TestUtilities.Globalization
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Xunit.Abstractions;
    using Xunit.Sdk;

    /// <summary>
    /// An attribute discoverer, for applying all cultures to xUnit.net tests marked with
    /// <see cref="AllCulturesTheoryAttribute"/>.
    /// </summary>
    public class AllCulturesTheoryAttributeDiscoverer : TheoryDiscoverer
    {
        /// <summary>
        /// The collection of all cultures present within the system running the tests.
        /// </summary>
        private static readonly CultureInfo[] AllCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

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
            AllCultures.Select(
                culture => new AllCulturesFactTestCase(
                    this.DiagnosticMessageSink,
                    discoveryOptions.MethodDisplayOrDefault(),
                    discoveryOptions.MethodDisplayOptionsOrDefault(),
                    testMethod,
                    culture,
                    dataRow));

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
            AllCultures.Select(
                culture => new AllCulturesTheoryTestCase(
                    this.DiagnosticMessageSink,
                    discoveryOptions.MethodDisplayOrDefault(),
                    discoveryOptions.MethodDisplayOptionsOrDefault(),
                    testMethod,
                    culture));
    }
}
