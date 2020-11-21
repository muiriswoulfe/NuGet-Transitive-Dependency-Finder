<!-- © Muiris Woulfe. Licensed under the MIT License. -->

# NuGet Transitive Dependency Finder

![Build Status][buildbadge]
[![SonarCloud Quality Gate Status][sonarcloudbadge]][sonarcloud]

**The NuGet Transitive Dependency Finder analyzes .NET solutions to find
superfluous dependencies that have been explicitly added to projects. The goal
is to simplify dependency management.**

.NET developers can use this application to find and remove these transitive
dependencies. This serves to simplify NuGet package upgrades by avoiding
conflicts between explicitly specified package dependencies and those
dependencies implicitly specified as part of a package dependency chain.

The solution comprises two projects for finding transitive dependencies:

- [`NuGetTransitiveDependencyFinder`][codelibrary]. This project provides a .NET
  library that can be used in any application or service for finding transitive
  dependencies.
- [`NuGetTransitiveDependencyFinder.ConsoleApp`][codeconsoleapp]. This project
  runs the NuGet transitive dependency finder logic against a specified .NET
  solution. It is expected to be the standard mechanism through which consumers
  use the NuGet Transitive Dependency Finder.

It is not always possible to remove all transitive dependencies. Some transitive
dependencies are required to explicitly specify a dependency version different
from that included as part of a package dependency chain, in order to avoid
version conflicts. Therefore, removal of these transitive dependencies should be
performed iteratively to ascertain which can be removed without introducing
errors or warnings into your solution build process.

## Building

### Installing Dependencies

To build the NuGet Transitive Dependency Finder, you will need to install:

- [Git][git]
- [.NET Core SDK 3.1.403][netcoresdk] or later

You can simplify the process by also installing one of the following:

- [Visual Studio Code][vscode] with the [C# Extension][vscodecsharp]
- [Visual Studio][vs]

### Downloading the Code

The NuGet Transitive Dependency Finder is [hosted on GitHub][github]. You can
clone it directly using:

```Batchfile
git clone git@github.com:muiriswoulfe/NuGet-Transitive-Dependency-Finder.git
```

### Build Process

The easiest way to build the NuGet Transitive Dependency Finder is to open
[NuGetTransitiveDependencyFinder.sln][codesolution] in one of the following:

- [Visual Studio Code][vscode] with the [C# Extension][vscodecsharp]
- [Visual Studio][vs]

#### Visual Studio Code

1. Select *File* > *Open...*
1. Navigate to your local copy of
   [NuGetTransitiveDependencyFinder.sln][codesolution] and click *Open*.
1. Select *View* > *Command Palette...*
1. In the *Command Palette*, enter *Task: Run Build Task*.
1. In the next *Command Palette* view, enter *Build Debug* or *Build Release*
   depending on which configuration you wish to build. Most consumers should use
   the Release configuration.

#### Visual Studio

1. Select *File* > *Open* > *Project/Solution...*
1. Navigate to your local copy of
   [NuGetTransitiveDependencyFinder.sln][codesolution] and click *Open*.
1. In the toolbar, click the *Solution Configurations* dropdown to select the
   configuration you wish to build. Most consumers should use the Release
   configuration.
1. In the *Solution Explorer* window, right-click on the solution file and
   select *Build Solution*.

#### Command Line

To build directly from the command line, enter the command appropriate to which
configuration you wish to build:

```Batchfile
dotnet build --configuration NuGetTransitiveDependencyFinder.sln Debug
dotnet build --configuration NuGetTransitiveDependencyFinder.sln Release
```

Most consumers should use the Release configuration.

## Using

Once you have a built a copy of the solution, navigate to the folder
containing the build outputs and enter:

```Batchfile
dotnet NuGetTransitiveDependencyFinder.ConsoleApp.dll <SolutionToAnalyze>
```

`<SolutionToAnalyze>` should be replaced by the relative or absolute path of
the .NET solution you wish to analyze for transitive NuGet dependencies.

If you wish to view the entire set of dependencies for each project, including
both transitive and non-transitive dependencies, you can enter:

```Batchfile
dotnet NuGetTransitiveDependencyFinder.ConsoleApp.dll <SolutionToAnalyze> --all
```

This mode is particularly useful for running before and after the removal of
transitive dependencies, as it can be used to detect if the removal of a
dependency resulted in the change of a dependency version. This can occur
because the removal of a transitive dependency results in the dependency being
pulled in from another dependency, and the version specified in that dependency
may differ from the one previously used. To avoid this and therefore mitigate
risk when removing transitive dependency, you can run the NuGet Transitive
Dependency Finder in `--all` mode prior to removal and after removal, to
ascertain whether any dependencies have changed version. This comparison can be
facilitated by a diffing tool.

You can also request the full set of command-line options by entering:

```Batchfile
dotnet NuGetTransitiveDependencyFinder.ConsoleApp.dll --help
```

# SonarCloud Status

The complete SonarCloud analysis for the NuGet Transitive Dependency Finder can
be located [here][sonarcloud].

![SonarCloud Maintainability Rating][sonarcloudmaintainability]

![SonarCloud Reliability Rating][sonarcloudreliability]

![SonarCloud Security Rating][sonarcloudsecurity]

![SonarCloud Bugs][sonarcloudbugs]

![SonarCloud Code Smells][sonarcloudcodesmells]

![SonarCloud Technical Debt][sonarcloudtechnicaldebt]

![SonarCloud Vulnerabilities][sonarcloudvulnerabilities]

![SonarCloud Duplicated Lines][sonarcloudduplicatedlines]

![SonarCloud Test Coverage][sonarcloudtestcoverage]

![SonarCloud Lines of Code][sonarcloudlinesofcode]

[buildbadge]: https://github.com/muiriswoulfe/NuGet-Transitive-Dependency-Finder/workflows/Build/badge.svg
[sonarcloudbadge]: https://sonarcloud.io/api/project_badges/measure?project=muiriswoulfe_NuGet-Transitive-Dependency-Finder&metric=alert_status
[sonarcloud]: https://sonarcloud.io/dashboard?id=muiriswoulfe_NuGet-Transitive-Dependency-Finder
[codelibrary]:  https://github.com/muiriswoulfe/NuGet-Transitive-Dependency-Finder/tree/main/src/NuGetTransitiveDependencyFinder
[codeconsoleapp]: https://github.com/muiriswoulfe/NuGet-Transitive-Dependency-Finder/tree/main/src/NuGetTransitiveDependencyFinder.ConsoleApp
[codesolution]: https://github.com/muiriswoulfe/NuGet-Transitive-Dependency-Finder/blob/main/NuGetTransitiveDependencyFinder.sln
[git]: https://git-scm.com/
[github]: https://github.com/muiriswoulfe/NuGet-Transitive-Dependency-Finder
[netcoresdk]: https://dotnet.microsoft.com/download/dotnet-core/3.1
[vs]: https://visualstudio.microsoft.com/
[vscode]: https://code.visualstudio.com/
[vscodecsharp]: https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp
[sonarcloudmaintainability]: https://sonarcloud.io/api/project_badges/measure?project=muiriswoulfe_NuGet-Transitive-Dependency-Finder&metric=sqale_rating
[sonarcloudreliability]: https://sonarcloud.io/api/project_badges/measure?project=muiriswoulfe_NuGet-Transitive-Dependency-Finder&metric=reliability_rating
[sonarcloudsecurity]: https://sonarcloud.io/api/project_badges/measure?project=muiriswoulfe_NuGet-Transitive-Dependency-Finder&metric=security_rating
[sonarcloudbugs]: https://sonarcloud.io/api/project_badges/measure?project=muiriswoulfe_NuGet-Transitive-Dependency-Finder&metric=bugs
[sonarcloudcodesmells]: https://sonarcloud.io/api/project_badges/measure?project=muiriswoulfe_NuGet-Transitive-Dependency-Finder&metric=code_smells
[sonarcloudtechnicaldebt]: https://sonarcloud.io/api/project_badges/measure?project=muiriswoulfe_NuGet-Transitive-Dependency-Finder&metric=sqale_index
[sonarcloudvulnerabilities]: https://sonarcloud.io/api/project_badges/measure?project=muiriswoulfe_NuGet-Transitive-Dependency-Finder&metric=vulnerabilities
[sonarcloudduplicatedlines]: https://sonarcloud.io/api/project_badges/measure?project=muiriswoulfe_NuGet-Transitive-Dependency-Finder&metric=duplicated_lines_density
[sonarcloudtestcoverage]: https://sonarcloud.io/api/project_badges/measure?project=muiriswoulfe_NuGet-Transitive-Dependency-Finder&metric=coverage
[sonarcloudlinesofcode]: https://sonarcloud.io/api/project_badges/measure?project=muiriswoulfe_NuGet-Transitive-Dependency-Finder&metric=ncloc
