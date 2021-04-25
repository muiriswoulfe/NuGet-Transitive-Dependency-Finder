// <copyright file="IProcessWrapper.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.Wrappers
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A wrapper interface around <see cref="Process"/>, to enable unit testing.
    /// </summary>
    internal interface IProcessWrapper
    {
        /// <summary>
        /// An event for handling when the process writes to its redirected standard error stream.
        /// </summary>
        public event EventHandler<DataReceivedEventArgs> ErrorDataReceived;

        /// <summary>
        /// An event for handling when the process writes to its redirected standard output stream.
        /// </summary>
        public event EventHandler<DataReceivedEventArgs> OutputDataReceived;

        /// <summary>
        /// Gets or sets the properties to pass to the <see cref="Start()"/> method.
        /// </summary>
        /// <exception cref="ArgumentNullException">The value that specifies the <see cref="StartInfo"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">The <see cref="Start()"/> method was not used to start
        /// the process.</exception>
        /// <returns>The <see cref="ProcessStartInfo"/> that represents the data with which to start the process. These
        /// arguments include the name of the executable file or document used to start the process.</returns>
        public ProcessStartInfo StartInfo { get; set; }

        /// <summary>
        /// Starts or reuses the process resource that is specified by the <see cref="StartInfo"/> property of this
        /// <see cref="ProcessWrapper"/> component and associates it with the component.
        /// </summary>
        /// <exception cref="InvalidOperationException">No file name was specified in the <see cref="ProcessWrapper"/>
        /// component's <see cref="StartInfo"/>, or the <see cref="ProcessStartInfo.UseShellExecute"/> member of the
        /// <see cref="StartInfo"/> property is <see langword="true"/> while
        /// <see cref="ProcessStartInfo.RedirectStandardInput"/>, <see cref="ProcessStartInfo.RedirectStandardOutput"/>,
        /// or <see cref="ProcessStartInfo.RedirectStandardError"/> is <see langword="true" />.</exception>
        /// <exception cref="Win32Exception">There was an error in opening the associated file.</exception>
        /// <exception cref="ObjectDisposedException">The process object has already been disposed.</exception>
        /// <exception cref="PlatformNotSupportedException">Method is not supported on operating systems without shell
        /// support, such as Nano Server.</exception>
        /// <returns><see langword="true"/> if a process resource is started; <see langword="false"/> if no new process
        /// resource is started (for example, if an existing process is reused).</returns>
        public bool Start();

        /// <summary>
        /// Begins asynchronous read operations on the redirected <see cref="Process.StandardError"/> stream of the
        /// application.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="ProcessStartInfo.RedirectStandardError"/>
        /// property is <see langword="false"/>, or an asynchronous read operation is already in progress on the
        /// <see cref="Process.StandardError"/> stream, or the <see cref="Process.StandardError"/> stream has been used
        /// by a synchronous read operation.</exception>
        public void BeginErrorReadLine();

        /// <summary>
        /// Begins asynchronous read operations on the redirected <see cref="Process.StandardOutput"/> stream of the
        /// application.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="ProcessStartInfo.RedirectStandardOutput"/>
        /// property is <see langword="false"/>, or an asynchronous read operation is already in progress on the
        /// <see cref="Process.StandardOutput"/> stream, or the <see cref="Process.StandardOutput"/> stream has been
        /// used by a synchronous read operation.</exception>
        public void BeginOutputReadLine();

        /// <summary>
        /// Instructs the process component to wait for the associated process to exit, or for the
        /// <paramref name="cancellationToken"/> to be cancelled.
        /// </summary>
        /// <param name="cancellationToken">An optional token to cancel the asynchronous operation.</param>
        /// <returns>A task that will complete when the process has exited, cancellation has been requested, or an error
        /// occurs.</returns>
        public Task WaitForExitAsync(CancellationToken cancellationToken = default);
    }
}
