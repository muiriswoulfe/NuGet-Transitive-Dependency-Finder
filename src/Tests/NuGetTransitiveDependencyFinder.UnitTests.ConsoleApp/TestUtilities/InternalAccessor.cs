// <copyright file="InternalAccessor.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGetTransitiveDependencyFinder.UnitTests.ConsoleApp.TestUtilities
{
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// A class for accessing <see langword="internal"/> functionality not visible to the current project.
    /// </summary>
    /// <remarks>This functionality is required for accessing <see langword="internal"/> functionality within the
    /// <c>NuGetTransitiveDependencyFinder</c> project. It is not possible to make that project's
    /// <see langword="internal"/> functionality visible to the current project via
    /// <see cref="InternalsVisibleToAttribute"/> as doing so leads to a class name duplication related to
    /// <c>NuGetTransitiveDependencyFinder.Properties.AssemblyAttributes</c>.</remarks>
    public static class InternalAccessor
    {
        /// <summary>
        /// Constructs the requested object.
        /// </summary>
        /// <typeparam name="TObject">The object to construct.</typeparam>
        /// <param name="arguments">The arguments to pass to the constructor.</param>
        /// <returns>The constructed object.</returns>
        public static TObject Construct<TObject>(params object[] arguments)
            where TObject : class
        {
            var type = typeof(TObject);
            var instance = type.Assembly.CreateInstance(
                type.FullName!,
                false,
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                arguments,
                CultureInfo.InvariantCulture,
                null);
            return (instance as TObject)!;
        }

        /// <summary>
        /// Sets a value via a property.
        /// </summary>
        /// <param name="obj">The object on which to invoke the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="value">The value to which to set the property.</param>
        public static void SetProperty(this object obj, string propertyName, object value) =>
            obj.GetType().InvokeMember(
                propertyName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty,
                null,
                obj,
                new[] { value },
                CultureInfo.InvariantCulture);
    }
}
