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
    using System.Drawing;
    using System.Runtime.Serialization;

    /// <summary>
    /// The style for a key definition.
    /// </summary>
    [DataContract(Name = "KeyStyle", Namespace = "")]
    public class KeyStyle : ElementStyle
    {
        /// <summary>
        /// The <see cref="KeySubStyle"/> for this key when it is loose.
        /// </summary>
        [DataMember]
        public KeySubStyle Loose { get; set; } = new KeySubStyle
        {
            Background = Color.FromArgb(100, 100, 100),
            Text = Color.FromArgb(0, 0, 0),
            Outline = Color.FromArgb(0, 255, 0),
            OutlineWidth = 1
        };

        /// <summary>
        /// The <see cref="KeySubStyle"/> for this key when it is pressed.
        /// </summary>
        [DataMember]
        public KeySubStyle Pressed { get; set; } = new KeySubStyle
        {
            Background = Color.FromArgb(255, 255, 255),
            Text = Color.FromArgb(0, 0, 0),
            Outline = Color.FromArgb(0, 255, 0),
            OutlineWidth = 1
        };

        /// <summary>
        /// Returns a clone of this element style.
        /// </summary>
        /// <returns>The cloned element style.</returns>
        public override ElementStyle Clone()
        {
            return new KeyStyle
            {
                Loose = this.Loose?.Clone(),
                Pressed = this.Pressed?.Clone()
            };
        }

        /// <summary>
        /// Checks whether the style has changes relative to the specified other style.
        /// </summary>
        /// <param name="other">The style to compare against.</param>
        /// <returns>True if the style has changes, false otherwise.</returns>
        public override bool IsChanged(ElementStyle other)
        {
            if (!(other is KeyStyle ks)) return true;

            if (this.Loose is null != ks.Loose is null) return true;
            if (this.Pressed is null != ks.Pressed is null) return true;

            return (this.Loose?.IsChanged(ks.Loose) ?? false) || (this.Pressed?.IsChanged(ks.Pressed) ?? false);
        }
    }
}