/*
Copyright (C) 2016 by Eric Bataille <e.c.p.bataille@gmail.com>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

namespace ThoNohT.NohBoard.Hooking
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// A circular buffer that will never grow beyond its specified size. It is pre-filled with a specified default
    /// element.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    public class CircleBuffer<T> : IEnumerable<T>
    {
        /// <summary>
        /// The internally wrapped state.
        /// </summary>
        private readonly Queue<T> state;

        /// <summary>
        /// Initializes a new instance of the <see cref="CircleBuffer{T}" /> class.
        /// </summary>
        /// <param name="size">The size of the buffer.</param>
        /// <param name="defaultElem">The default element to pre-fill the buffer with.</param>
        public CircleBuffer(int size, T defaultElem)
        {
            this.Size = size;
            this.state = new Queue<T>(size);
            for (var i = 0; i < size; i++)
                this.state.Enqueue(defaultElem);
        }

        /// <summary>
        /// Adds an element to the buffer. If the buffer is full, the oldest element is removed first.
        /// </summary>
        /// <param name="elem">The element to add.</param>
        public void Add(T elem)
        {
            lock (this.state)
            {
                this.state.Dequeue();
                this.state.Enqueue(elem);
            }
        }

        /// <summary>
        /// The size of this circle buffer.
        /// </summary>
        public int Size { get; }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            lock (this.state)
            {
                foreach (var elem in this.state) yield return elem;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}