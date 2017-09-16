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

namespace ThoNohT.NohBoard.Keyboard.ElementDefinitions
{
    using System.Drawing;

    /// <summary>
    /// Indicates in what way an element is being manipulated.
    /// </summary>
    public class ElementManipulation
    {
        /// <summary>
        /// The manipulation type.
        /// </summary>
        public ElementManipulationType Type { get; set; }

        /// <summary>
        /// The index of the thing inside the element that is being manipulated.
        /// Relevant for MoveBoundary, MoveEdge.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// The direction of the manipulation.
        /// Relevant for nothing yet
        /// </summary>
        public SizeF Direction { get; set; }
    }

    /// <summary>
    /// The type of manipulation being performed.
    /// </summary>
    public enum ElementManipulationType
    {
        /// <summary>
        /// Translate the entire element.
        /// </summary>
        Translate,

        /// <summary>
        /// Move a single boundary in the element.
        /// </summary>
        MoveBoundary,

        /// <summary>
        /// Move an edge in the element.
        /// </summary>
        MoveEdge,

        /// <summary>
        /// Scale an element.
        /// </summary>
        Scale,

        /// <summary>
        /// Move text within an element.
        /// </summary>
        MoveText,
    }
}