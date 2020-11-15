// <copyright file="Base.cs" company="Muiris Woulfe">
// © Muiris Woulfe
// Licensed under the MIT License
// </copyright>

namespace NuGet.TransitiveDependency.Finder.Output
{
    using System.Collections.Generic;

    /// <summary>
    /// A base class representing outputted information.
    /// </summary>
    /// <typeparam name="TChild">The type of the collection elements, which comprise the child elements of the current
    /// object.</typeparam>
    public abstract class Base<TChild>
        where TChild : notnull
    {
        /// <summary>
        /// The collection of child elements.
        /// </summary>
        private readonly List<TChild> children;

        /// <summary>
        /// A value indicating whether the collection of child elements is sorted.
        /// </summary>
        private bool areChildrenSorted = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="Base{TChild}"/> class.
        /// </summary>
        /// <param name="capacity">The quantity of child elements for which the collection has adequate initial
        /// capacity.</param>
        protected Base(int capacity) =>
            this.children = new List<TChild>(capacity);

        /// <summary>
        /// Gets a value indicating whether the object has child elements.
        /// </summary>
        public bool HasChildren =>
            this.children.Count != 0;

        /// <summary>
        /// Gets the sorted collection of child elements.
        /// </summary>
        /// <remarks>Invoking this property may involve sorting the collection, which involves an O(n log n) operation,
        /// where n is the count of the child elements in the collection.</remarks>
        public IReadOnlyCollection<TChild> SortedChildren
        {
            get
            {
                if (!this.areChildrenSorted)
                {
                    this.children.Sort();
                    this.areChildrenSorted = true;
                }

                return this.children;
            }
        }

        /// <summary>
        /// Adds a child element to the current collection, on the condition that <see cref="IsAddValid(TChild)"/>
        /// returns <c>true</c>.
        /// </summary>
        /// <param name="child">The child element to be added.</param>
        public void Add(TChild child)
        {
            if (this.IsAddValid(child))
            {
                this.children.Add(child);
                this.areChildrenSorted = false;
            }
        }

        /// <summary>
        /// Determines whether performing an <see cref="Add(TChild)"/> operation on the specified child element is
        /// valid.
        /// </summary>
        /// <param name="child">The child element to check.</param>
        /// <returns>A value indicating whether the child element should be added the collection.</returns>
        protected abstract bool IsAddValid(TChild child);
    }
}
