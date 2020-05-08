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
    using Extra;

    /// <summary>
    /// The substyle for a key. This is part of the style, defining its style for either the key's pressed or unpressed
    /// state.
    /// </summary>
    [DataContract(Name = "KeySubStyle", Namespace = "")]
    public class KeySubStyle
    {
        #region Properties

        /// <summary>
        /// The background color.
        /// </summary>
        [DataMember]
        public SerializableColor Background { get; set; }

        /// <summary>
        /// The text color.
        /// </summary>
        [DataMember]
        public SerializableColor Text { get; set; }

        /// <summary>
        /// The outline color.
        /// </summary>
        [DataMember]
        public SerializableColor Outline { get; set; }

        /// <summary>
        /// A value indicating whether to draw the outline.
        /// </summary>
        [DataMember]
        public bool ShowOutline { get; set; }

        /// <summary>
        /// The width of the outliner.
        /// </summary>
        [DataMember]
        public int OutlineWidth { get; set; } = 1;

        /// <summary>
        /// The font of the key's text.
        /// </summary>
        [DataMember]
        public SerializableFont Font { get; set; } = new Font(FontFamily.GenericMonospace, 10);

        /// <summary>
        /// The filename of the background image, relative to the style's images folder.
        /// </summary>
        [DataMember]
        public string BackgroundImageFileName { get; set; }

        #endregion Properties

        /// <summary>
        /// Returns a clone of this key substyle.
        /// </summary>
        /// <returns>The cloned key substyle.</returns>
        public KeySubStyle Clone()
        {
            return new KeySubStyle
            {
                Background = this.Background.Clone(),
                BackgroundImageFileName = this.BackgroundImageFileName,
                Text = this.Text.Clone(),
                Outline = this.Outline.Clone(),
                OutlineWidth = this.OutlineWidth,
                ShowOutline = this.ShowOutline,
                Font = this.Font.Clone()
            };
        }

        /// <summary>
        /// Returns the appropriate background brush to use for a key, given its dimensions.
        /// </summary>
        /// <param name="boundingBox">The bounding box of the key.</param>
        /// <returns>The appropriate brush.</returns>
        public Brush GetBackgroundBrush(Rectangle boundingBox)
        {
            return this.BackgroundImageFileName == null || !FileHelper.StyleImageExists(this.BackgroundImageFileName)
                ? new SolidBrush(this.Background)
                : this.BrushFromImage(boundingBox, this.BackgroundImageFileName);
        }

        /// <summary>
        /// Creates a brush from an image.
        /// </summary>
        /// <param name="boundingBox">The bounding box to fit the brush in.</param>
        /// <param name="fileName">The filename to load the image from. This filename should be relative to the images
        /// folder of the current style.</param>
        /// <returns>The brush created from the image.</returns>
        private Brush BrushFromImage(Rectangle boundingBox, string fileName)
        {
            var img = ImageCache.Get(fileName);
            var gu = GraphicsUnit.Pixel;
            var imgBb = img.GetBounds(ref gu);

            // Create a texture brush from the image.
            var tex = new TextureBrush(img, imgBb);
            tex.TranslateTransform(boundingBox.Left, boundingBox.Top);
            tex.ScaleTransform(boundingBox.Width / imgBb.Width, boundingBox.Height / imgBb.Height);

            return tex;
        }

        /// <summary>
        /// Checks whether the style has changes relative to the specified other style.
        /// </summary>
        /// <param name="other">The style to compare against.</param>
        /// <returns>True if the style has changes, false otherwise.</returns>
        public bool IsChanged(KeySubStyle other)
        {
            if (this.Background.IsChanged(other.Background)) return true;
            if (this.BackgroundImageFileName != other.BackgroundImageFileName) return true;
            if (this.Font.IsChanged(other.Font)) return true;
            if (this.Text != other.Text) return true;
            if (this.Outline.IsChanged(other.Outline)) return true;
            if (this.OutlineWidth != other.OutlineWidth) return true;
            if (this.ShowOutline != other.ShowOutline) return true;

            return false;
        }
    }
}