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
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents an element that can be placed on a keyboard. This can be a key or another custom element.
    /// </summary>
    [DataContract(Name = "ElementDefinition", Namespace = "")]
    [KnownType(typeof(KeyDefinition))]
    [KnownType(typeof(MouseSpeedIndicatorDefinition))]
    public abstract class ElementDefinition
    {
        /// <summary>
        /// Compare this against the dependency counter to know when to update brushes.
        /// </summary>
        protected int StyleVersion = 0;

        /// <summary>
        /// Gets or sets the identifier of the element.
        /// </summary>
        [DataMember]
        public int Id { get; protected set; }

        /// <summary>
        /// Returns the bounding box of this element.
        /// </summary>
        /// <returns>A rectangle representing the bounding box of the element.</returns>
        public abstract Rectangle GetBoundingBox();

        /// <summary>
        /// Translates the element, moving it the specified distance.
        /// </summary>
        /// <param name="dx">The distance along the x-axis.</param>
        /// <param name="dy">The distance along the y-axis.</param>
        /// <returns>A new <see cref="ElementDefinition"/> that is translated.</returns>
        public abstract ElementDefinition Translate(int dx, int dy);

        /// <summary>
        /// Calculates whether the specified point is inside this element.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>True if the point is inside the element, false otherwise.</returns>
        public abstract bool Inside(Point point);

        /// <summary>
        /// TODO: Documentation.
        /// </summary>
        /// <param name="g"></param>
        public abstract void RenderEditing(Graphics g);

        // TODO: RenderHighlight

        // TODO: RenderSelected
    }
}