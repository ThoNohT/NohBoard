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

namespace ThoNohT.NohBoard.Keyboard.Styles
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents the style of any element that can be placed on a keyboard.
    /// </summary>
    [KnownType(typeof(KeyStyle))]
    [KnownType(typeof(MouseSpeedIndicatorStyle))]
    [DataContract(Name = "ElementStyle", Namespace = "")]
    public abstract class ElementStyle
    {
        /// <summary>
        /// Returns a clone of this element style.
        /// </summary>
        /// <returns>The cloned element style.</returns>
        public abstract ElementStyle Clone();

        /// <summary>
        /// Checks whether the style has changes relative to the specified other style.
        /// </summary>
        /// <param name="other">The style to compare against.</param>
        /// <returns>True if the style has changes, false otherwise.</returns>
        public abstract bool IsChanged(ElementStyle other);
    }
}