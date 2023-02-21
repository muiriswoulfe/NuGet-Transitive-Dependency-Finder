// <copyright file="AllCulturesFactTestCase.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.TestUtilities.Globalization;

using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

/// <summary>
/// An xUnit.net test case, which covers a test method to be run with a single culture.
/// </summary>
internal class AllCulturesFactTestCase : XunitTestCase
{
    /// <summary>
    /// The culture for which to run the current test.
    /// </summary>
    private CultureInfo culture = CultureInfo.InvariantCulture;

    /// <summary>
    /// Initializes a new instance of the <see cref="AllCulturesFactTestCase"/> class.
    /// </summary>
    /// <remarks>This constructor should never be explicitly called, as it is only provided to enable
    /// deserialization.</remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable S1133 // Deprecated code should be removed
    [Obsolete("Required for deserialization.")]
#pragma warning restore S1133 // Deprecated code should be removed
    public AllCulturesFactTestCase()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AllCulturesFactTestCase"/> class.
    /// </summary>
    /// <param name="diagnosticMessageSink">The message sink that receives the test result messages.</param>
    /// <param name="defaultMethodDisplay">The default method display to use, for when no customization has been
    /// performed.</param>
    /// <param name="defaultMethodDisplayOptions">The default method display options to use, for when no customization
    /// has been performed.</param>
    /// <param name="testMethod">The test method to which the current test case belongs.</param>
    /// <param name="culture">The culture for which to run the current test.</param>
    /// <param name="testMethodArguments">The arguments for the test method.</param>
    public AllCulturesFactTestCase(
        IMessageSink diagnosticMessageSink,
        TestMethodDisplay defaultMethodDisplay,
        TestMethodDisplayOptions defaultMethodDisplayOptions,
        ITestMethod testMethod,
        CultureInfo culture,
        object[]? testMethodArguments = null)
        : base(
            diagnosticMessageSink,
            defaultMethodDisplay,
            defaultMethodDisplayOptions,
            testMethod,
            testMethodArguments) =>
        this.Initialize(culture);

    /// <summary>
    /// Deserializes <paramref name="data"/> into the current object.
    /// </summary>
    /// <param name="data">The data store to be deserialized into the current object.</param>
    public override void Deserialize(IXunitSerializationInfo data)
    {
        base.Deserialize(data);

        this.Initialize(AllCulturesBaseTestCase.Deserialize(data));
    }

    /// <summary>
    /// Serializes the current object into <paramref name="data"/>.
    /// </summary>
    /// <param name="data">The data store into which to serialize the current object.</param>
    public override void Serialize(IXunitSerializationInfo data)
    {
        base.Serialize(data);

        AllCulturesBaseTestCase.Serialize(data, this.culture);
    }

    /// <summary>
    /// Asynchronously executes the test case, returning zero or more test result messages through the message sink.
    /// </summary>
    /// <param name="diagnosticMessageSink">The message sink that receives the test result messages.</param>
    /// <param name="messageBus">The message bus to which to report the results.</param>
    /// <param name="constructorArguments">The arguments to pass to the constructor.</param>
    /// <param name="aggregator">The error aggregator to use for catching exceptions.</param>
    /// <param name="cancellationTokenSource">The cancellation token source for ascertaining whether a cancellation has
    /// been requested.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation, which contains the summary of the test
    /// case run.</returns>
    public override Task<RunSummary> RunAsync(
        IMessageSink diagnosticMessageSink,
        IMessageBus messageBus,
        object[] constructorArguments,
        ExceptionAggregator aggregator,
        CancellationTokenSource cancellationTokenSource) =>
        AllCulturesBaseTestCase.RunAsync(
            () => base.RunAsync(
                diagnosticMessageSink,
                messageBus,
                constructorArguments,
                aggregator,
                cancellationTokenSource),
            this.culture);

    /// <summary>
    /// Gets a unique identifier for the test case.
    /// </summary>
    /// <returns>The unique identifier.</returns>
    protected override string GetUniqueID() =>
        AllCulturesBaseTestCase.GetUniqueId(base.GetUniqueID(), this.culture);

    /// <summary>
    /// Initializes the data stored within the current instance of the <see cref="AllCulturesFactTestCase"/> class.
    /// </summary>
    /// <param name="culture">The culture for which to run the current test.</param>
    private void Initialize(CultureInfo culture)
    {
        this.culture = culture;
        this.DisplayName += AllCulturesBaseTestCase.GetDisplayNameSuffix(culture);
        AllCulturesBaseTestCase.InitializeTraits(this.Traits, culture);
    }
}
