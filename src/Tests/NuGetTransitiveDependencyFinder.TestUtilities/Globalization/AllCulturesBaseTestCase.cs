// <copyright file="AllCulturesBaseTestCase.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.TestUtilities.Globalization;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;
using static System.FormattableString;

/// <summary>
/// A class comprising logic shared between <see cref="AllCulturesFactTestCase"/> and
/// <see cref="AllCulturesTheoryTestCase"/>.
/// </summary>
internal static class AllCulturesBaseTestCase
{
    /// <summary>
    /// The name of the culture field, which is used during serialization and deserialization.
    /// </summary>
    public const string CultureFieldName = "Culture";

    /// <summary>
    /// Deserializes <paramref name="data"/> into culture information.
    /// </summary>
    /// <param name="data">The data store to be deserialized into culture information.</param>
    /// <returns>The deserialized culture information.</returns>
    public static CultureInfo Deserialize(IXunitSerializationInfo data) =>
        data.GetValue<CultureInfo>(CultureFieldName);

    /// <summary>
    /// Serializes culture information into <paramref name="data"/>.
    /// </summary>
    /// <param name="data">The data store into which to serialize the culture information.</param>
    /// <param name="culture">The culture information to serialize.</param>
    public static void Serialize(IXunitSerializationInfo data, CultureInfo culture) =>
        data.AddValue(CultureFieldName, culture);

    /// <summary>
    /// Asynchronously executes a test case, returning zero or more test result messages through the message sink.
    /// </summary>
    /// <param name="baseRunAsyncFunction">The underlying <see cref="XunitTestCase.RunAsync(IMessageSink,
    /// IMessageBus, object[], ExceptionAggregator, System.Threading.CancellationTokenSource)"/> method to run.</param>
    /// <param name="culture">The culture for which to run the test.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation, which contains the summary of the test
    /// case run.</returns>
    public static async Task<RunSummary> RunAsync(Func<Task<RunSummary>> baseRunAsyncFunction, CultureInfo culture)
    {
        var originalCulture = CultureInfo.CurrentCulture;
        var originalUICulture = CultureInfo.CurrentUICulture;

        try
        {
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            return await baseRunAsyncFunction().ConfigureAwait(false);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
            CultureInfo.CurrentUICulture = originalUICulture;
        }
    }

    /// <summary>
    /// Gets a unique identifier for a test case.
    /// </summary>
    /// <param name="baseUniqueId">The unique identifier of the test prior to application of the culture.</param>
    /// <param name="culture">The culture associated with the test.</param>
    /// <returns>The unique identifier.</returns>
    public static string GetUniqueId(string baseUniqueId, CultureInfo culture) =>
        baseUniqueId + GetDisplayNameSuffix(culture);

    /// <summary>
    /// Gets the test display name suffix, which comprises the culture information.
    /// </summary>
    /// <param name="culture">The culture associated with the test.</param>
    public static string GetDisplayNameSuffix(CultureInfo culture) =>
        Invariant($"[{culture.Name}]");

    /// <summary>
    /// Initializes the test traits with the provided culture information.
    /// </summary>
    /// <param name="traits">The test traits to which the culture information are to be applied.</param>
    /// <param name="culture">The culture associated with the test.</param>
    public static void InitializeTraits(Dictionary<string, List<string>> traits, CultureInfo culture) =>
        traits.Add(CultureFieldName, new List<string> { culture.Name });
}
