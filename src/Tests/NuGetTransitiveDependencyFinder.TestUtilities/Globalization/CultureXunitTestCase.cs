// <copyright file="CultureXunitTestCase.cs" company="Muiris Woulfe">
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

    public class CultureXunitTestCase : XunitTestCase
    {
        private string culture = "en-US";

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete(
            "Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
        public CultureXunitTestCase()
        {
        }

        public CultureXunitTestCase(
            IMessageSink diagnosticMessageSink,
            TestMethodDisplay defaultMethodDisplay,
            TestMethodDisplayOptions defaultMethodDisplayOptions,
            ITestMethod testMethod,
            string culture)
            : base(diagnosticMessageSink, defaultMethodDisplay, defaultMethodDisplayOptions, testMethod) =>
            this.Initialize(culture);

        private static CultureInfo CurrentCulture
        {
            get => CultureInfo.CurrentCulture;
            set => CultureInfo.CurrentCulture = value;
        }

        private static CultureInfo CurrentUICulture
        {
            get => CultureInfo.CurrentUICulture;
            set => CultureInfo.CurrentUICulture = value;
        }

        public override void Deserialize(IXunitSerializationInfo data)
        {
            base.Deserialize(data);

            this.Initialize(data.GetValue<string>("Culture"));
        }

        public override void Serialize(IXunitSerializationInfo data)
        {
            base.Serialize(data);

            data.AddValue("Culture", this.culture);
        }

        public override async Task<RunSummary> RunAsync(
            IMessageSink diagnosticMessageSink,
            IMessageBus messageBus,
            object[] constructorArguments,
            ExceptionAggregator aggregator,
            CancellationTokenSource cancellationTokenSource)
        {
            var originalCulture = CurrentCulture;
            var originalUICulture = CurrentUICulture;

            try
            {
                var cultureInfo = new CultureInfo(this.culture);
                CurrentCulture = cultureInfo;
                CurrentUICulture = cultureInfo;

                return await base.RunAsync(
                    diagnosticMessageSink,
                    messageBus,
                    constructorArguments,
                    aggregator,
                    cancellationTokenSource).ConfigureAwait(false);
            }
            finally
            {
                CurrentCulture = originalCulture;
                CurrentUICulture = originalUICulture;
            }
        }

        protected override string GetUniqueID() =>
            Invariant($"{base.GetUniqueID()}[{this.culture}]");

        private void Initialize(string culture)
        {
            this.culture = culture;

            this.Traits.Add("Culture", new List<string> { this.culture });

            this.DisplayName += Invariant($"[{culture}]");
        }
    }
}
