// <copyright file="IProcessWrapper.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.Wrappers;

using System;
using System.ComponentModel;
using System.Diagnostics;

/// <summary>
/// A wrapper interface around <see cref="Process"/>, to enable unit testing.
/// </summary>
internal interface IProcessWrapper
{
    /// <summary>
    /// Starts the process resource and associates it with the component.
    /// </summary>
    /// <param name="startInfo">The properties to pass to the process.</param>
    /// <param name="outputDataReceived">An event for handling when the process writes to its redirected standard output
    /// stream.</param>
    /// <param name="errorDataReceived">An event for handling when the process writes to its redirected standard error
    /// stream.</param>
    /// <exception cref="InvalidOperationException">No file name was specified in the <see cref="ProcessWrapper"/>
    /// component's <paramref name="startInfo"/>, or the <see cref="ProcessStartInfo.UseShellExecute"/> member of the
    /// <paramref name="startInfo"/> property is <see langword="true"/> while
    /// <see cref="ProcessStartInfo.RedirectStandardInput"/>, <see cref="ProcessStartInfo.RedirectStandardOutput"/>,
    /// or <see cref="ProcessStartInfo.RedirectStandardError"/> is <see langword="true" />.</exception>
    /// <exception cref="Win32Exception">There was an error in opening the associated file.</exception>
    /// <exception cref="ObjectDisposedException">The process object has already been disposed.</exception>
    /// <exception cref="PlatformNotSupportedException">Method is not supported on operating systems without shell
    /// support, such as Nano Server.</exception>
    public void Start(
        ProcessStartInfo startInfo,
        DataReceivedEventHandler outputDataReceived,
        DataReceivedEventHandler errorDataReceived);

    /// <summary>
    /// Begins asynchronous read operations on the redirected <see cref="Process.StandardError"/> stream of the
    /// application.
    /// </summary>
    /// <exception cref="InvalidOperationException">The <see cref="ProcessStartInfo.RedirectStandardError"/> property is
    /// <see langword="false"/>, or an asynchronous read operation is already in progress on the
    /// <see cref="Process.StandardError"/> stream, or the <see cref="Process.StandardError"/> stream has been used by a
    /// synchronous read operation.</exception>
    public void BeginErrorReadLine();

    /// <summary>
    /// Begins asynchronous read operations on the redirected <see cref="Process.StandardOutput"/> stream of the
    /// application.
    /// </summary>
    /// <exception cref="InvalidOperationException">The <see cref="ProcessStartInfo.RedirectStandardOutput"/> property
    /// is <see langword="false"/>, or an asynchronous read operation is already in progress on the
    /// <see cref="Process.StandardOutput"/> stream, or the <see cref="Process.StandardOutput"/> stream has been used by
    /// a synchronous read operation.</exception>
    public void BeginOutputReadLine();

    /// <summary>
    /// Instructs the process component to wait for the associated process to exit.
    /// </summary>
    public void WaitForExit();
}
