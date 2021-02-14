// <copyright file="CultureXunitTheoryTestCase.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.TestUtilities.Globalization
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit.Abstractions;
    using Xunit.Sdk;
    using static System.FormattableString;

    /// <summary>
    /// An xUnit.net theory test case, which covers a theory test method to be run with a single culture.
    /// </summary>
    public class CultureXunitTheoryTestCase : XunitTheoryTestCase
    {
        /// <summary>
        /// Gets the culture for which to run the current test.
        /// </summary>
        public CultureInfo Culture { get; private set; } = CultureInfo.InvariantCulture;

        /// <summary>
        /// Initializes a new instance of the <see cref="CultureXunitTheoryTestCase"/> class.
        /// </summary>
        /// <remarks>This constructor should never be explicitly called, as it is only provided to enable
        /// deserialization.</remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Required for deserialization.")]
        public CultureXunitTheoryTestCase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CultureXunitTheoryTestCase"/> class.
        /// </summary>
        /// <param name="diagnosticMessageSink">The message sink that receives the test result messages.</param>
        /// <param name="defaultMethodDisplay">The default method display to use,  for when no customization has been
        /// performed.</param>
        /// <param name="defaultMethodDisplayOptions">The default method display options to use, for when no
        /// customization has been performed.</param>
        /// <param name="testMethod">The test method to which the current test case belongs.</param>
        /// <param name="culture">The culture for which to run the current test.</param>
        public CultureXunitTheoryTestCase(
            IMessageSink diagnosticMessageSink,
            TestMethodDisplay defaultMethodDisplay,
            TestMethodDisplayOptions defaultMethodDisplayOptions,
            ITestMethod testMethod,
            CultureInfo culture)
            : base(diagnosticMessageSink, defaultMethodDisplay, defaultMethodDisplayOptions, testMethod) =>
            this.Initialize(culture);

        /// <summary>
        /// Deserializes <paramref name="data"/> into the current object.
        /// </summary>
        /// <param name="data">The data store to be deserialized into the current object.</param>
        public override void Deserialize(IXunitSerializationInfo data)
        {
            base.Deserialize(data);

            this.Initialize(data.GetValue<CultureInfo>("Culture"));
        }

        /// <summary>
        /// Serializes the current object into <paramref name="data"/>.
        /// </summary>
        /// <param name="data">The data store into which to serialize the current object.</param>
        public override void Serialize(IXunitSerializationInfo data)
        {
            base.Serialize(data);

            data.AddValue("Culture", this.Culture);
        }

        /// <summary>
        /// Asychronously executes the test case, returning zero or more test result messages through the message sink.
        /// </summary>
        /// <param name="diagnosticMessageSink">The message sink that receives the test result messages.</param>
        /// <param name="messageBus">The message bus to which to report the results.</param>
        /// <param name="constructorArguments">The arguments to pass to the constructor.</param>
        /// <param name="aggregator">The error aggregator to use for catching exceptions.</param>
        /// <param name="cancellationTokenSource">The cancellation token source for ascertaining whether a cancellation
        /// has been requested.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation, which contains the summary of the
        /// test case run.</returns>
        public override Task<RunSummary> RunAsync(
            IMessageSink diagnosticMessageSink,
            IMessageBus messageBus,
            object[] constructorArguments,
            ExceptionAggregator aggregator,
            CancellationTokenSource cancellationTokenSource) =>
            new CultureXunitTheoryTestCaseRunner(
                this,
                this.DisplayName,
                this.SkipReason,
                constructorArguments,
                diagnosticMessageSink,
                messageBus,
                aggregator,
                cancellationTokenSource).RunAsync();

        /// <summary>
        /// Gets a unique identifier for the test case.
        /// </summary>
        /// <returns>The unique identifier.</returns>
        protected override string GetUniqueID() =>
            Invariant($"{base.GetUniqueID()}[{this.Culture.Name}]");

        /// <summary>
        /// Initializes the data stored within the current instance of the <see cref="CultureXunitTheoryTestCase"/>
        /// class.
        /// </summary>
        /// <param name="culture">The culture for which to run the current test.</param>
        private void Initialize(CultureInfo culture)
        {
            this.Culture = culture;
            this.Traits.Add("Culture", new List<string> { this.Culture.Name });
            this.DisplayName += Invariant($"[{this.Culture.Name}]");
        }
    }
}