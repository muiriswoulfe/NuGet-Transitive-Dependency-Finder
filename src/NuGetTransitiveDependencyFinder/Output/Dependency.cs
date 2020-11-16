// <copyright file="Dependency.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.Output
{
    using NuGet.Versioning;

    /// <summary>
    /// A class representing the outputted .NET dependency information for each project and framework combination.
    /// </summary>
    public class Dependency
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Dependency"/> class.
        /// </summary>
        /// <param name="identifier">The dependency identifier.</param>
        /// <param name="version">The dependency version.</param>
        public Dependency(string identifier, NuGetVersion version)
        {
            this.Identifier = identifier;
            this.Version = version;
        }

        /// <summary>
        /// Gets the dependency identifier.
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// Gets the dependency version.
        /// </summary>
        public NuGetVersion Version { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the dependency is a transitive dependency.
        /// </summary>
        public bool IsTransitiveDependency { get; set; }
    }
}
