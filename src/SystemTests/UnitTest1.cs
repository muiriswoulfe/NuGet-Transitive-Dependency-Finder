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
            Arguments = $"dotnet-transitive-dependency-finder " +
                "--projectOrSolution ../../../../../../SystemTests/NoTransitiveDependencies/NuGetTransitiveDependencyFinder.SystemTests.NoTransitiveDependencies.csproj",
            WorkingDirectory = $"../../../../../src/Product/NuGetTransitiveDependencyFinder.ConsoleApp/bin/{configuration}/net7.0/",
            RedirectStandardOutput = true,
            UseShellExecute = false
        };

        // Start the process
        var process = new Process { StartInfo = startInfo };
        var result = process.Start();

        // Read the output
        var output = process.StandardOutput.ReadToEnd();

        // Wait for the process to exit
        process.WaitForExit();

        // Assert that the output is what we expect
        Assert.True(result);
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
            Arguments = $"dotnet-transitive-dependency-finder " +
                "--projectOrSolution ../../../../../../SystemTests/TransitiveDependencies/NuGetTransitiveDependencyFinder.SystemTests.TransitiveDependencies.csproj",
            WorkingDirectory = $"../../../../../src/Product/NuGetTransitiveDependencyFinder.ConsoleApp/bin/{configuration}/net7.0/",
            RedirectStandardOutput = true,
            UseShellExecute = false
        };

        // Start the process
        var process = new Process { StartInfo = startInfo };
        var result = process.Start();

        // Read the output
        var output = process.StandardOutput.ReadToEnd();

        // Wait for the process to exit
        process.WaitForExit();

        // Assert that the output is what we expect
        Assert.True(result);
        Assert.Equal("expected output", output);
    }
}

