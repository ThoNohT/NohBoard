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
        /// The current manipulation being performed on this element.
        /// </summary>
        protected ElementManipulation CurrentManipulation;

        /// <summary>
        /// The current manipulation to be previewed on this element.
        /// </summary>
        protected ElementManipulation PreviewManipulation;

        /// <summary>
        /// The most relevant manipulation at the moment.
        /// </summary>
        public ElementManipulation RelevantManipulation => this.PreviewManipulation ?? this.CurrentManipulation;

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
        /// Returns the type of manipulation that will happen when interacting with the element at the specified point.
        /// </summary>
        /// <param name="point">The point to start manipulating.</param>
        /// <param name="altDown">Whether any alt key is pressed.</param>
        /// <param name="preview">whether to set the preview manipulation, or the real one.</param>
        /// <returns>The manipulation type for the specified point. <c>null</c> if no manipulation would happen
        /// at this point.</returns>
        /// <remarks>Manipulation preview is used to show what would be modified on a selected element. We cannot
        /// keep updating the element manipulation as the mouse moves, but do want to provide a visual indicator.</remarks>
        public abstract bool StartManipulating(Point point, bool altDown, bool preview = false);
        
        // TODO: Add StopManipulating?

        /// <summary>
        /// Renders a simple representation of the element while it is being edited. This representation does not depend
        /// on the state of the program and is merely intended to provide a clear overview of the current position and
        /// shape of the element.
        /// </summary>
        /// <param name="g">The graphics context to render to.</param>
        public abstract void RenderEditing(Graphics g);

        /// <summary>
        /// Renders a simple representation of the element while it is being highlighted in edit mode.
        /// </summary>
        /// <param name="g">The graphics context to render to.</param>
        public abstract void RenderHighlight(Graphics g);

        /// <summary>
        /// Renders a simple representation of the element while it is selected in edit mode.
        /// </summary>
        /// <param name="g">The graphics context to render to.</param>
        public abstract void RenderSelected(Graphics g);

        /// <summary>
        /// Manipulates the element according to its current manipulation state. If no manipulation state is set,
        /// the element is returned without any changes. Otherwise, the element itself will determine what to modify
        /// about its position or shape and return the updated version of itself.
        /// </summary>
        /// <param name="diff">The distance to manipulate the element by.</param>
        /// <returns>The updated element.</returns>
        public abstract ElementDefinition Manipulate(Size diff);

        /// <summary>
        /// Returns a clone of this element definition.
        /// </summary>
        /// <returns>The cloned element definition.</returns>
        public abstract ElementDefinition Clone();

        /// <summary>
        /// Sets the new manipulation for this element.
        /// </summary>
        /// <param name="manipulation">The new manipulation.</param>
        /// <param name="preview">Whether or not it is a previe manipulation.</param>
        protected void SetManipulation(ElementManipulation manipulation, bool preview)
        {
            if (preview) this.PreviewManipulation = manipulation;
            else
            {
                this.CurrentManipulation = manipulation;
                this.PreviewManipulation = null;
            }
        }
    }
}