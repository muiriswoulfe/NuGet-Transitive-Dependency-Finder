// <copyright file="ProcessWrapper.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.Wrappers
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A wrapper class around <see cref="Process"/>, to enable unit testing.
    /// </summary>
    internal class ProcessWrapper : IProcessWrapper
    {
        /// <summary>
        /// The underlying <see cref="Process"/> on which to run the class logic.
        /// </summary>
        private readonly Process process;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessWrapper"/> class.
        /// </summary>
        /// <param name="process">The underlying <see cref="Process"/> on which to run the class logic.</param>
        public ProcessWrapper(Process process) =>
            this.process = process;

        /// <inheritdoc/>
        public event EventHandler<DataReceivedEventArgs> ErrorDataReceived
        {
            add =>
                this.process.ErrorDataReceived += new DataReceivedEventHandler(value);
            remove =>
                this.process.ErrorDataReceived -= new DataReceivedEventHandler(value);
        }

        /// <inheritdoc/>
        public event EventHandler<DataReceivedEventArgs> OutputDataReceived
        {
            add =>
                this.process.OutputDataReceived += new DataReceivedEventHandler(value);
            remove =>
                this.process.OutputDataReceived -= new DataReceivedEventHandler(value);
        }

        /// <inheritdoc/>
        public ProcessStartInfo StartInfo
        {
            get =>
                this.process.StartInfo;
            set =>
                this.process.StartInfo = value;
        }

        /// <inheritdoc/>
        public bool Start() =>
            this.process.Start();

        /// <inheritdoc/>
        public void BeginErrorReadLine() =>
            this.process.BeginErrorReadLine();

        /// <inheritdoc/>
        public void BeginOutputReadLine() =>
            this.process.BeginOutputReadLine();

        /// <inheritdoc/>
        public Task WaitForExitAsync(CancellationToken cancellationToken = default) =>
            this.process.WaitForExitAsync(cancellationToken);
    }
}
