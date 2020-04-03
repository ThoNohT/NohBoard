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
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a font, stored in a way that it can be serialized.
    /// </summary>
    [DataContract(Name = "Font", Namespace = "")]
    public class SerializableFont
    {
        /// <summary>
        /// Creates an empty serializable font.
        /// </summary>
        public SerializableFont()
        {
        }

        /// <summary>
        /// Creates a font from a <see cref="Font"/> and a download URL.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="downloadUrl">The download URL.</param>
        public SerializableFont(Font font, string downloadUrl)
        {
            this.FontFamily = font.FontFamily.Name;
            this.Style = (SerializableFontStyle)font.Style;
            this.Size = font.Size;
            this.DownloadUrl = downloadUrl;
        }

        /// <summary>
        /// The font family.
        /// </summary>
        [DataMember]
        public string FontFamily { get; set; }

        /// <summary>
        /// An alternate font family to use for rendering. If not set, <see cref="FontFamily"/> is used. If
        /// <see cref="FontFamily"/> does not exist on the system, it cannot be used. Therefore, this property can be
        /// filled with another font that does exist to fall back on.
        /// </summary>
        public string AlternateFontFamily { get; set; }

        /// <summary>
        /// The font family that is used for rendering. Uses <see cref="AlternateFontFamily"/> if it has a value. Otherwise
        /// <see cref="FontFamily"/>.
        /// </summary>
        public string UsedFontFamily => this.AlternateFontFamily ?? this.FontFamily;

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
        /// The url to download the font from. If <c>null</c>, the font cannot be downloaded and will only be used if
        /// it exists locally.
        /// </summary>
        [DataMember]
        public string DownloadUrl { get; set; }

        /// <summary>
        /// Converts a <see cref="SerializableFont"/> to a <see cref="Font"/>.
        /// </summary>
        /// <param name="src">The font to convert.</param>
        public static implicit operator Font(SerializableFont src)
        {
            return new Font(new FontFamily(src.UsedFontFamily), src.Size, (FontStyle)src.Style);
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

        /// <summary>
        /// Checks whether the font has changes relative to the specified other font.
        /// </summary>
        /// <param name="other">The font to compare against.</param>
        /// <returns>True if the font has changes, false otherwise.</returns>
        public bool IsChanged(SerializableFont other)
        {
            return this.FontFamily != other.FontFamily ||
                this.AlternateFontFamily != other.AlternateFontFamily ||
                this.DownloadUrl != other.DownloadUrl;
        }

        /// <summary>
        /// An equality comparer that just compares the font family.
        /// </summary>
        public class FamilyComparer : IEqualityComparer<SerializableFont>
        {
            /// </inheritdoc>
            public bool Equals(SerializableFont x, SerializableFont y)
            {
                return Equals(x.FontFamily, y.FontFamily);
            }

            /// </inheritdoc>
            public int GetHashCode(SerializableFont obj)
            {
                return obj.FontFamily.GetHashCode();
            }
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