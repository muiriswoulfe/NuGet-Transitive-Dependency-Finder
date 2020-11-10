<!-- © Muiris Woulfe. Licensed under the MIT License. -->

# NuGet Transitive Dependency Finder

![Build Status][buildbadge]

**The NuGet Transitive Dependency Finder analyzes .NET solutions to find
superfluous dependencies that have been explicitly added to projects. The goal
is to simplify dependency management.**

.NET developers can use this application to find and remove these transitive
dependencies. This serves to simplify NuGet package upgrades by avoiding
conflicts between explicitly specified package dependencies and those
dependencies implicitly specified as part of a package dependency chain.

The solution comprises two projects for finding transitive dependencies:

- [`NuGet.TransitiveDependency.Finder.ConsoleApp`][codeconsoleapp]. This project
  solution. It is expected to be the standard mechanism through which consumers
  use the NuGet Transitive Dependency Finder.
- [`NuGet.TransitiveDependency.Finder.Library`][codelibrary]. This project
  provides a .NET Standard library that can be used in any application or
  service for finding transitive dependencies.

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
2. Navigate to your local copy of
   [NuGetTransitiveDependencyFinder.sln][codesolution] and click *Open*.
3. Select *View* > *Command Palette...*
4. In the *Command Palette*, enter *Task: Run Build Task*.
5. In the next *Command Palette* view, enter *Build Debug* or *Build Release*
   depending on which configuration you wish to build. Most consumers should use
   the Release configuration.

#### Visual Studio

1. Select *File* > *Open* > *Project/Solution...*
2. Navigate to your local copy of
   [NuGetTransitiveDependencyFinder.sln][codesolution] and click *Open*.
3. In the toolbar, click the *Solution Configurations* dropdown to select the
   configuration you wish to build. Most consumers should use the Release
   configuration.
4. In the *Solution Explorer* window, right-click on the solution file and
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

Once you have a built a version copy the solution, navigate to the folder
containing the build outputs and enter:

```Batchfile
dotnet NuGet.TransitiveDependency.Finder.ConsoleApp.dll <SolutionToAnalyze>
```

`<SolutionToAnalyze>` should be replaced by the relative or absolute path of
the .NET solution you wish to analyze for transitive NuGet dependencies.

[buildbadge]: https://github.com/muiriswoulfe/NuGet-Transitive-Dependency-Finder/workflows/Build/badge.svg
[codeconsoleapp]: https://github.com/muiriswoulfe/NuGet-Transitive-Dependency-Finder/tree/main/src/NuGet.TransitiveDependency.Finder.ConsoleApp
[codelibrary]:  https://github.com/muiriswoulfe/NuGet-Transitive-Dependency-Finder/tree/main/src/NuGet.TransitiveDependency.Finder.Library
[codesolution]: https://github.com/muiriswoulfe/NuGet-Transitive-Dependency-Finder/blob/main/NuGetTransitiveDependencyFinder.sln
[git]: https://git-scm.com/
[github]: https://github.com/muiriswoulfe/NuGet-Transitive-Dependency-Finder
[netcoresdk]: https://dotnet.microsoft.com/download/dotnet-core/3.1
[vs]: https://visualstudio.microsoft.com/
[vscode]: https://code.visualstudio.com/
[vscodecsharp]: https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp
