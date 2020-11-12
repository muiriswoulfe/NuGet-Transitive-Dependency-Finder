// <copyright file="ColorConsole.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGet.TransitiveDependency.Finder.ConsoleApp.Output
{
    using System;

    /// <summary>
    /// A class for writing text to the console with the specified color.
    /// </summary>
    internal static class ColorConsole
    {
        /// <summary>
        /// Writes text to the console with the specified color.
        /// </summary>
        /// <param name="text">The text to write to the console.</param>
        /// <param name="color">The color of the text.</param>
        public static void WriteLine(string text, ConsoleColor color)
        {
            try
            {
                Console.ForegroundColor = color;
                Console.WriteLine(text);
            }
            finally
            {
                Console.ResetColor();
            }
        }
    }
}
