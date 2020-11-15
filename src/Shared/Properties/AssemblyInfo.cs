// <copyright file="AssemblyInfo.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using NuGetTransitiveDependencyFinder.Properties;

[assembly: AssemblyCompanyAttribute("Muiris Woulfe")]
[assembly: AssemblyCopyrightAttribute("© Muiris Woulfe. Licensed under the MIT License.")]
[assembly: AssemblyDescriptionAttribute(
    "The NuGet Transitive Dependency Finder analyzes .NET solutions to find superfluous dependencies that have been " +
    "explicitly added to projects. The goal is to simplify dependency management.")]
[assembly: AssemblyFileVersionAttribute(AssemblyAttributes.Version)]
[assembly: AssemblyInformationalVersionAttribute(AssemblyAttributes.Version)]
[assembly: AssemblyMetadataAttribute(
    "RepositoryUrl",
    "https://github.com/muiriswoulfe/NuGet-Transitive-Dependency-Finder.git")]
[assembly: AssemblyVersionAttribute(AssemblyAttributes.Version)]
[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]
[assembly: TargetFrameworkAttribute(".NETCoreApp,Version=v5.0", FrameworkDisplayName = "")]
[assembly: NeutralResourcesLanguageAttribute("en-US")]
