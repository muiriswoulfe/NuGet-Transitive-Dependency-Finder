// <copyright file="AllCulturesTheoryAttribute.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Utilities.Globalization;

using System;
using Xunit;
using Xunit.Sdk;

/// <summary>
/// An attribute whose application to an xUnit.net theory test will result in that test being run for all cultures
/// present within the system running the tests.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
[XunitTestCaseDiscoverer("Xunit.Sdk.TheoryDiscoverer", "xunit.execution.{Platform}")]
public sealed class AllCulturesTheoryAttribute : TheoryAttribute;
