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
    using System;
    using System.Drawing;
    using System.Runtime.Serialization;
    using Extra;

    /// <summary>
    /// The style for a mouse speed indicator.
    /// </summary>
    [DataContract(Name = "MouseSpeedIndicatorStyle", Namespace = "")]
    public class MouseSpeedIndicatorStyle : ElementStyle
    {
        #region Properties

        /// <summary>
        /// The inner color of the mouse speed indicator. This is also the color as shown when no movement occurs.
        /// </summary>
        [DataMember]
        public SerializableColor InnerColor { get; set; } = Color.FromArgb(100, 100, 100);

        /// <summary>
        /// The outer color of the mouse speed indicator. This is the color of the outer edge of the indicator when
        /// there is maximum movement.
        /// </summary>
        [DataMember]
        public SerializableColor OuterColor { get; set; } = Color.FromArgb(255, 255, 255);

        /// <summary>
        /// The width of the outline.
        /// </summary>
        [DataMember]
        public int OutlineWidth { get; set; } = 1;

        #endregion Properties

        /// <summary>
        /// Returns a clone of this mouse speed indicator style.
        /// </summary>
        /// <returns>The cloned mouse speed indicator style.</returns>
        public override ElementStyle Clone()
        {
            return new MouseSpeedIndicatorStyle
            {
                InnerColor = this.InnerColor.Clone(),
                OuterColor = this.OuterColor.Clone(),
                OutlineWidth = this.OutlineWidth
            };
        }

        /// <summary>
        /// Checks whether the style has changes relative to the specified other style.
        /// </summary>
        /// <param name="other">The style to compare against.</param>
        /// <returns>True if the style has changes, false otherwise.</returns>
        public override bool IsChanged(ElementStyle other)
        {
            if (!(other is MouseSpeedIndicatorStyle msis)) return true;

            return this.InnerColor.IsChanged(msis.InnerColor) ||
                this.OuterColor.IsChanged(msis.OuterColor) ||
                this.OutlineWidth != msis.OutlineWidth;
        }
    }
}