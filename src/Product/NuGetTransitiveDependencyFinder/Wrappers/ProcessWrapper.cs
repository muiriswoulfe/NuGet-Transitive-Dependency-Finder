// <copyright file="ProcessWrapper.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.Wrappers
{
    using System.Diagnostics;

    /// <summary>
    /// A wrapper class around <see cref="Process"/>, to enable unit testing.
    /// </summary>
    internal class ProcessWrapper : IProcessWrapper
    {
        /// <summary>
        /// The underlying <see cref="Process"/> on which to run the class logic.
        /// </summary>
        private Process? process;

        /// <inheritdoc/>
        public void Start(
            ProcessStartInfo startInfo,
            DataReceivedEventHandler outputDataReceived,
            DataReceivedEventHandler errorDataReceived)
        {
            this.process = new()
            {
                StartInfo = startInfo,
            };
            this.process.OutputDataReceived += outputDataReceived;
            this.process.ErrorDataReceived += errorDataReceived;

            _ = this.process.Start();
        }

        /// <inheritdoc/>
        public void BeginErrorReadLine() =>
            this.process!.BeginErrorReadLine();

        /// <inheritdoc/>
        public void BeginOutputReadLine() =>
            this.process!.BeginOutputReadLine();

        /// <inheritdoc/>
        public void WaitForExit() =>
            this.process!.WaitForExit();
    }
}
