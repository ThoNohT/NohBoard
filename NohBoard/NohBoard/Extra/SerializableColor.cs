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

namespace ThoNohT.NohBoard.Extra
{
    using System.Drawing;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a color, stored in a way that it can be serialized.
    /// </summary>
    [DataContract(Name = "Color", Namespace = "")]
    public class SerializableColor
    {
        /// <summary>
        /// The red component.
        /// </summary>
        [DataMember]
        public byte Red { get; set; }

        /// <summary>
        /// The green component.
        /// </summary>
        [DataMember]
        public byte Green { get; set; }

        /// <summary>
        /// The blue component.
        /// </summary>
        [DataMember]
        public byte Blue { get; set; }

        /// <summary>
        /// The serialized alpha component. This is null when the color is fully opaque, so that styles that do not
        /// use transparency do not change their serialized format.
        /// </summary>
        [DataMember(Name = "Alpha", EmitDefaultValue = false)]
        private byte? serializedAlpha;

        /// <summary>
        /// The alpha component of the color. Colors loaded from styles that do not specify an alpha are fully opaque.
        /// </summary>
        public byte Alpha
        {
            get => this.serializedAlpha ?? 255;
            set => this.serializedAlpha = value == 255 ? (byte?)null : value;
        }

        /// <summary>
        /// Whether this color is not fully opaque.
        /// </summary>
        public bool IsTransparent => this.Alpha != 255;

        /// <summary>
        /// Converts a <see cref="SerializableColor"/> to a <see cref="Color"/>.
        /// </summary>
        /// <param name="src">The color to convert.</param>
        public static implicit operator Color(SerializableColor src)
        {
            return Color.FromArgb(src.Alpha, src.Red, src.Green, src.Blue);
        }

        /// <summary>
        /// Converts a <see cref="Color"/> to a <see cref="SerializableColor"/>.
        /// </summary>
        /// <param name="src">The color to convert.</param>
        public static implicit operator SerializableColor(Color src)
        {
            return new SerializableColor
            {
                Red = src.R,
                Green = src.G,
                Blue = src.B,
                Alpha = src.A
            };
        }

        /// <summary>
        /// Creates a clone of this serializable color.
        /// </summary>
        /// <returns>The cloned color.</returns>
        public SerializableColor Clone()
        {
            return (SerializableColor)this.MemberwiseClone();
        }

        /// <summary>
        /// Checks if this color has changes relative with the specified other color.
        /// </summary>
        /// <param name="other">The color to compare against.</param>
        /// <returns>True if the color has changes, false otherwise.</returns>
        public bool IsChanged(SerializableColor other)
        {
            return this.Red != other.Red || this.Green != other.Green || this.Blue != other.Blue
                || this.Alpha != other.Alpha;
        }
    }

}