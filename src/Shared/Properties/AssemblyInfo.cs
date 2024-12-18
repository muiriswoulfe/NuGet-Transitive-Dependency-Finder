// <copyright file="AssemblyInfo.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using NuGetTransitiveDependencyFinder.Properties;

[assembly: AssemblyCompany("Muiris Woulfe")]
[assembly: AssemblyCopyright("© Muiris Woulfe. Licensed under the MIT License.")]
[assembly: AssemblyDescription(
    "The NuGet Transitive Dependency Finder analyzes .NET projects and solutions to find superfluous dependencies " +
    "that have been explicitly added to projects. The goal is to simplify dependency management.")]
[assembly: AssemblyFileVersion(AssemblyAttributes.Version)]
[assembly: AssemblyInformationalVersion(AssemblyAttributes.Version)]
[assembly: AssemblyMetadata("RepositoryUrl", "https://github.com/muiriswoulfe/NuGet-Transitive-Dependency-Finder")]
[assembly: AssemblyVersion(AssemblyAttributes.Version)]
[assembly: ComVisible(false)]
[assembly: TargetFramework(".NETCoreApp,Version=v9.0", FrameworkDisplayName = "")]
[assembly: NeutralResourcesLanguage("en-US")]
