// <copyright file="ProcessWrapper.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.ProjectAnalysis
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
        private readonly Process underlyingProcess;

        /// <summary>
        /// A value tracking whether <see cref="Dispose()"/> has been invoked.
        /// </summary>
        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessWrapper"/> class.
        /// </summary>
        public ProcessWrapper() =>
            this.underlyingProcess = new Process();

        /// <summary>
        /// Finalizes an instance of the <see cref="ProcessWrapper"/> class.
        /// </summary>
        ~ProcessWrapper() =>
            this.Dispose(false);

        /// <inheritdoc/>
        public event EventHandler<DataReceivedEventArgs> ErrorDataReceived
        {
            add =>
                this.underlyingProcess.ErrorDataReceived += new DataReceivedEventHandler(value);
            remove =>
                this.underlyingProcess.ErrorDataReceived -= new DataReceivedEventHandler(value);
        }

        /// <inheritdoc/>
        public event EventHandler<DataReceivedEventArgs> OutputDataReceived
        {
            add =>
                this.underlyingProcess.OutputDataReceived += new DataReceivedEventHandler(value);
            remove =>
                this.underlyingProcess.OutputDataReceived -= new DataReceivedEventHandler(value);
        }

        /// <inheritdoc/>
        public ProcessStartInfo StartInfo
        {
            get =>
                this.underlyingProcess.StartInfo;
            set =>
                this.underlyingProcess.StartInfo = value;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public bool Start() =>
            this.underlyingProcess.Start();

        /// <inheritdoc/>
        public void BeginErrorReadLine() =>
            this.underlyingProcess.BeginErrorReadLine();

        /// <inheritdoc/>
        public void BeginOutputReadLine() =>
            this.underlyingProcess.BeginOutputReadLine();

        /// <inheritdoc/>
        public Task WaitForExitAsync(CancellationToken cancellationToken = default) =>
            this.underlyingProcess.WaitForExitAsync(cancellationToken);

        /// <summary>
        /// Disposes of the resources maintained by the current object.
        /// </summary>
        /// <param name="disposing">A value indicating whether disposal should be performed for managed as well as
        /// unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.underlyingProcess.Dispose();
                }

                this.disposedValue = true;
            }
        }
    }
}
