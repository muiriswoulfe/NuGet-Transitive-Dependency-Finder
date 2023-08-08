namespace NuGetTransitiveDependencyFinder.SystemTests;

using System.Diagnostics;
using Xunit;

public class UnitTest1
{
    [Fact]
    public void NoTransitiveDependencies()
    {
#if DEBUG
        const string configuration = "Debug";
#else
        const string configuration = "Release";
#endif

        // Set up the process start info
        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"../../src/Product/NuGetTransitiveDependencyFinder.ConsoleApp/bin/{configuration}/net7.0/dotnet-transitive-dependency-finder " +
                "--projectOrSolution ../NoTransitiveDependencies/NuGetTransitiveDependencyFinder.SystemTests.NoTransitiveDependencies.csproj",
            RedirectStandardOutput = true,
            UseShellExecute = false
        };

        // Start the process
        var process = new Process { StartInfo = startInfo };
        process.Start();

        // Read the output
        var output = process.StandardOutput.ReadToEnd();

        // Wait for the process to exit
        process.WaitForExit();

        // Assert that the output is what we expect
        Assert.Equal("expected output", output);
    }

    [Fact]
    public void TransitiveDependencies()
    {
#if DEBUG
        const string configuration = "Debug";
#else
        const string configuration = "Release";
#endif

        // Set up the process start info
        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"../../src/Product/NuGetTransitiveDependencyFinder.ConsoleApp/bin/{configuration}/net7.0/dotnet-transitive-dependency-finder " +
                "--projectOrSolution ../TransitiveDependencies/NuGetTransitiveDependencyFinder.SystemTests.TransitiveDependencies.csproj",
            RedirectStandardOutput = true,
            UseShellExecute = false
        };

        // Start the process
        var process = new Process { StartInfo = startInfo };
        process.Start();

        // Read the output
        var output = process.StandardOutput.ReadToEnd();

        // Wait for the process to exit
        process.WaitForExit();

        // Assert that the output is what we expect
        Assert.Equal("expected output", output);
    }
}

