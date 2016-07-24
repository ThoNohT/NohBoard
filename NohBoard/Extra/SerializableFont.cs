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
    using System;
    using System.Drawing;
    using System.Runtime.Serialization;
    using System.Windows.Forms;

    /// <summary>
    /// Represents a font, stored in a way that it can be serialized.
    /// </summary>
    [DataContract(Name = "Font", Namespace = "")]
    public class SerializableFont
    {
        /// <summary>
        /// The font family.
        /// </summary>
        [DataMember]
        public string FontFamily { get; set; }
        
        /// <summary>
        /// The font size.
        /// </summary>
        [DataMember]
        public float Size { get; set; }

        /// <summary>
        /// The font styles.
        /// </summary>
        [DataMember]
        public SerializableFontStyle Style { get; set; }

        /// <summary>
        /// Converts a <see cref="SerializableFont"/> to a <see cref="Font"/>.
        /// </summary>
        /// <param name="src">The font to convert.</param>
        public static implicit operator Font(SerializableFont src)
        {
            return new Font(new FontFamily(src.FontFamily), src.Size, (FontStyle)src.Style);
        }

        /// <summary>
        /// Converts a <see cref="Font"/> to a <see cref="SerializableFont"/>.
        /// </summary>
        /// <param name="src">The font to convert.</param>
        public static implicit operator SerializableFont(Font src)
        {
            return new SerializableFont
            {
                FontFamily = src.FontFamily.Name,
                Size = src.Size,
                Style = (SerializableFontStyle)src.Style
            };
        }

        /// <summary>
        /// Creates a clone of this serializable font.
        /// </summary>
        /// <returns>The cloned font.</returns>
        public SerializableFont Clone()
        {
            return (SerializableFont)this.MemberwiseClone();
        }
    }

    /// <summary>
    /// The possible styles of a font, stored in a way that it can be serialized.
    /// </summary>
    [DataContract(Name = "FontStyle", Namespace = "")]
    public enum SerializableFontStyle
    {
        /// <summary>
        /// Regular, no special styles.
        /// </summary>
        [EnumMember]
        Regular = 0,

        /// <summary>
        /// Bold style.
        /// </summary>
        [EnumMember]
        Bold = 1,

        /// <summary>
        /// Italic style.
        /// </summary>
        [EnumMember]
        Italic = 2,

        /// <summary>
        /// Underlined style.
        /// </summary>
        [EnumMember]
        Underline = 4,

        /// <summary>
        /// Strikeout style.
        /// </summary>
        [EnumMember]
        Strikeout = 8
    }
}