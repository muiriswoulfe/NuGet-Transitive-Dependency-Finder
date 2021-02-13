// <copyright file="CultureFactAttributeDiscoverer.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.TestUtilities.Globalization
{
    using System.Collections.Generic;
    using System.Linq;
    using Xunit.Abstractions;
    using Xunit.Sdk;

    public class CultureFactAttributeDiscoverer : IXunitTestCaseDiscoverer
    {
        private readonly IMessageSink diagnosticMessageSink;

        public CultureFactAttributeDiscoverer(IMessageSink diagnosticMessageSink) =>
            this.diagnosticMessageSink = diagnosticMessageSink;

        public IEnumerable<IXunitTestCase> Discover(
            ITestFrameworkDiscoveryOptions discoveryOptions,
            ITestMethod testMethod,
            IAttributeInfo factAttribute)
        {
            var cultures = new[] { "en-US", "fr-FR" };

            var methodDisplay = discoveryOptions.MethodDisplayOrDefault();
            var methodDisplayOptions = discoveryOptions.MethodDisplayOptionsOrDefault();

            return cultures.Select(
                culture => new CultureXunitTestCase(
                    this.diagnosticMessageSink,
                    methodDisplay,
                    methodDisplayOptions,
                    testMethod,
                    culture));
        }
    }
}
