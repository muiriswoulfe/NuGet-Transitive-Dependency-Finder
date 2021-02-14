// <copyright file="CultureXunitTheoryTestCaseRunner.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.TestUtilities.Globalization
{
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit.Abstractions;
    using Xunit.Sdk;

    /// <summary>
    /// An xUnit.net theory test case runner for theory test methods to be run with a single culture.
    /// </summary>
    public class CultureXunitTheoryTestCaseRunner : XunitTheoryTestCaseRunner
    {
        /// <summary>
        /// Gets the culture for which to run the current test.
        /// </summary>
        private readonly CultureInfo culture;

        /// <summary>
        /// The original <see cref="CultureInfo.CurrentCulture"/> prior to running the test.
        /// </summary>
        private CultureInfo originalCulture = CultureInfo.InvariantCulture;

        /// <summary>
        /// The original <see cref="CultureInfo.CurrentUICulture"/> prior to running the test.
        /// </summary>
        private CultureInfo originalUICulture = CultureInfo.InvariantCulture;

        /// <summary>
        /// Initializes a new instance of the <see cref="CultureXunitTheoryTestCaseRunner"/> class.
        /// </summary>
        /// <param name="testCase">The test case to run.</param>
        /// <param name="displayName">The display name of the test case.</param>
        /// <param name="skipReason">The reason for skipping the test, if it is to be skipped.</param>
        /// <param name="constructorArguments">The arguments to pass to the constructor.</param>
        /// <param name="diagnosticMessageSink">The message sink that receives the test result messages.</param>
        /// <param name="messageBus">The message bus to which to report the results.</param>
        /// <param name="aggregator">The error aggregator to use for catching exceptions.</param>
        /// <param name="cancellationTokenSource">The cancellation token source for ascertaining whether a cancellation
        /// has been requested.</param>
        public CultureXunitTheoryTestCaseRunner(
            CultureXunitTheoryTestCase testCase,
            string displayName,
            string skipReason,
            object[] constructorArguments,
            IMessageSink diagnosticMessageSink,
            IMessageBus messageBus,
            ExceptionAggregator aggregator,
            CancellationTokenSource cancellationTokenSource)
            : base(
                  testCase,
                  displayName,
                  skipReason,
                  constructorArguments,
                  diagnosticMessageSink,
                  messageBus,
                  aggregator,
                  cancellationTokenSource) =>
            this.culture = testCase.Culture;

        /// <summary>
        /// Initializes the test case state by setting the culture information.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override Task AfterTestCaseStartingAsync()
        {
            this.originalCulture = CultureInfo.CurrentCulture;
            this.originalUICulture = CultureInfo.CurrentUICulture;

            CultureInfo.CurrentCulture = this.culture;
            CultureInfo.CurrentUICulture = this.culture;

            return base.AfterTestCaseStartingAsync();
        }

        /// <summary>
        /// Destructs the test case state by restoring the original culture information.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override Task BeforeTestCaseFinishedAsync()
        {
            CultureInfo.CurrentUICulture = this.originalUICulture;
            CultureInfo.CurrentCulture = this.originalCulture;

            return base.BeforeTestCaseFinishedAsync();
        }
    }
}
