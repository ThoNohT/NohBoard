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
    using System.Drawing.Imaging;

    /// <summary>
    /// Applies overlay transparency to frame and fill colors (not key text).
    /// </summary>
    public static class OverlayTransparency
    {
        public const int MaxTransparencyPercent = 99;

        /// <summary>
        /// Clamps a transparency percentage to 0–99. 100% would hide image-based keyboard styles completely.
        /// </summary>
        public static int ClampPercent(int percent) => Math.Max(0, Math.Min(MaxTransparencyPercent, percent));

        /// <summary>
        /// Kept for callers that distinguish "hide" from "fade"; 100% is normalized to 99%, so this stays false.
        /// </summary>
        public static bool HidesFrameAndFill(int transparencyPercent) => false;

        /// <summary>
        /// Applies transparency to a color. 0% = unchanged, 99% = almost transparent but still visible.
        /// </summary>
        public static Color Apply(Color color, int transparencyPercent)
        {
            var p = ClampPercent(transparencyPercent);
            if (p == 0) return color;

            var factor = 255 - (p * 255 / 100);
            var a = (byte)(color.A * factor / 255);
            return Color.FromArgb(a, color.R, color.G, color.B);
        }

        /// <summary>
        /// Returns a brush with transparency applied. Solid brushes are copied; other brush types are returned as-is.
        /// </summary>
        public static Brush ApplyToBrush(Brush brush, int transparencyPercent)
        {
            if (HidesFrameAndFill(transparencyPercent))
                return Brushes.Transparent;

            if (ClampPercent(transparencyPercent) == 0) return brush;

            if (brush is SolidBrush solid) return new SolidBrush(Apply(solid.Color, transparencyPercent));

            return brush;
        }

        /// <summary>
        /// Creates a pen for key outlines with transparency applied.
        /// </summary>
        public static Pen CreateOutlinePen(Color outline, float width, int transparencyPercent) =>
            new Pen(Apply(outline, transparencyPercent), width);

        /// <summary>
        /// Color matrix that scales alpha for images (background image, texture fills).
        /// </summary>
        public static ColorMatrix CreateAlphaColorMatrix(int transparencyPercent)
        {
            var p = ClampPercent(transparencyPercent);
            var scale = (float)(100 - p) / 100f;
            // Scale RGB and A together so semi-transparent pixels are not drawn as full-brightness white.
            return new ColorMatrix
            {
                Matrix00 = scale,
                Matrix11 = scale,
                Matrix22 = scale,
                Matrix33 = scale,
                Matrix44 = 1f
            };
        }

        /// <summary>
        /// Draws an image with transparency applied.
        /// </summary>
        public static void DrawImage(Graphics g, Image image, Rectangle dest, int transparencyPercent)
        {
            if (ClampPercent(transparencyPercent) == 0)
            {
                g.DrawImage(image, dest);
                return;
            }

            using var attributes = new ImageAttributes();
            attributes.SetColorMatrix(CreateAlphaColorMatrix(transparencyPercent));
            g.DrawImage(
                image,
                dest,
                0,
                0,
                image.Width,
                image.Height,
                GraphicsUnit.Pixel,
                attributes);
        }
    }
}
