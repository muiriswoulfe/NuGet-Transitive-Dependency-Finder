// <copyright file="AllCulturesBaseAttributeDiscoverer.cs" company="Muiris Woulfe">
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
    /// A class comprising logic shared between <see cref="AllCulturesFactAttributeDiscoverer"/> and
    /// <see cref="AllCulturesTheoryAttributeDiscoverer"/>.
    /// </summary>
    internal static class AllCulturesBaseAttributeDiscoverer
    {
        /// <summary>
        /// The collection of all cultures present within the system running the tests.
        /// </summary>
        private static readonly CultureInfo[] AllCultures =
#if DEBUG
            { CultureInfo.GetCultureInfo("en-US") };
#else
            CultureInfo.GetCultures(CultureTypes.AllCultures);
#endif

        /// <summary>
        /// Creates the full suite of test cases, where each test case validates a single culture, corresponding
        /// to each fact test method.
        /// </summary>
        /// <param name="diagnosticMessageSink">The message sink that receives the test result messages.</param>
        /// <param name="discoveryOptions">The discovery options to use.</param>
        /// <param name="testMethod">The test method to which the current test case belongs.</param>
        /// <param name="dataRow">The row of data for the test case.</param>
        /// <returns>The full suite of test cases to run.</returns>
        public static IEnumerable<IXunitTestCase> CreateFactTestCases(
            IMessageSink diagnosticMessageSink,
            ITestFrameworkDiscoveryOptions discoveryOptions,
            ITestMethod testMethod,
            object[]? dataRow = null) =>
            AllCultures.Select(
                culture => new AllCulturesFactTestCase(
                    diagnosticMessageSink,
                    discoveryOptions.MethodDisplayOrDefault(),
                    discoveryOptions.MethodDisplayOptionsOrDefault(),
                    testMethod,
                    culture,
                    dataRow));

        /// <summary>
        /// Creates the full suite of test cases, where each test case validates a single culture, corresponding
        /// to each theory test method.
        /// </summary>
        /// <param name="diagnosticMessageSink">The message sink that receives the test result messages.</param>
        /// <param name="discoveryOptions">The discovery options to use.</param>
        /// <param name="testMethod">The test method to which the current test case belongs.</param>
        /// <returns>The full suite of test cases to run.</returns>
        public static IEnumerable<IXunitTestCase> CreateTheoryTestCases(
            IMessageSink diagnosticMessageSink,
            ITestFrameworkDiscoveryOptions discoveryOptions,
            ITestMethod testMethod) =>
            AllCultures.Select(
                culture => new AllCulturesTheoryTestCase(
                    diagnosticMessageSink,
                    discoveryOptions.MethodDisplayOrDefault(),
                    discoveryOptions.MethodDisplayOptionsOrDefault(),
                    testMethod,
                    culture));
    }
}
