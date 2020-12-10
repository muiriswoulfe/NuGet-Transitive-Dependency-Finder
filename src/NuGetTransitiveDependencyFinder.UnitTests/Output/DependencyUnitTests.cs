// <copyright file="DependencyUnitTests.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.Output
{
    using System;
    using NuGetTransitiveDependencyFinder.Output;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="Dependency"/> class.
    /// </summary>
    public class DependencyUnitTests
    {
        /// <summary>
        /// Tests that ABC.
        /// </summary>
        [Fact]
        public void ConstructorWithNullVersionThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new Dependency("identifier", null));
            Assert.Equal("version", exception.ParamName);
        }
    }
}
