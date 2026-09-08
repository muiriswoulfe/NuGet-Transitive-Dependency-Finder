// <copyright file="Program.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.TestCollateral.RedundantTransitiveDependencies;

using Microsoft.Extensions.Logging;

/// <summary>
/// The main class of the application, which exists solely to force the compiler to retain references to the packages
/// declared in the csproj so that the restore graph contains them.
/// </summary>
internal static class Program
{
    /// <summary>
    /// The entry point of the application.
    /// </summary>
    public static void Main()
    {
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = factory.CreateLogger("Main");
        logger.LogInformation("Hello, World!");
    }
}
