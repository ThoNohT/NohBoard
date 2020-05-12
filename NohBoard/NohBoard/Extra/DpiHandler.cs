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

    /// <summary>
    /// Helpers for handling changes from the default DPI.
    /// </summary>
    public static class DpiHandler
    {
        /// <summary>
        /// The DPI that is considered 100%.
        /// </summary>
        private const int defaultDpi = 96;

        /// <summary>
        /// Compensates a size for the DPI settings in the specified graphics instance.
        /// </summary>
        /// <param name="size">The size to compensate.</param>
        /// <param name="graphics">The instance of graphics to get the scale from.</param>
        /// <returns>The compensated size.</returns>
        public static Size DpiCompensate(this Size size, Graphics graphics)
        {
            var scaleX = graphics.DpiX / defaultDpi;
            var scaleY = graphics.DpiY / defaultDpi;

            return new Size((int)(size.Width *scaleX), (int)(size.Height * scaleY));
        }

        /// <summary>
        /// Uncompensates a size for the DPI settings in the specified graphics instance.
        /// </summary>
        /// <param name="size">The size to compensate.</param>
        /// <param name="graphics">The instance of graphics to get the scale from.</param>
        /// <returns>The uncompensated size.</returns>
        public static Size DpiUncompensate(this Size size, Graphics graphics)
        {
            var scaleX = graphics.DpiX / defaultDpi;
            var scaleY = graphics.DpiY / defaultDpi;

            return new Size((int)(size.Width / scaleX), (int)(size.Height / scaleY));
        }

        /// <summary>
        /// Uncompensates a point for the DPI settings in the specified graphics instance.
        /// </summary>
        /// <param name="point">The point to compensate.</param>
        /// <param name="graphics">The instance of graphics to get the scale from.</param>
        /// <returns>The uncompensated point.</returns>
        public static Point DpiUncompensate(this Point point, Graphics graphics)
        {
            var scaleX = graphics.DpiX / defaultDpi;
            var scaleY = graphics.DpiY / defaultDpi;

            return new Point((int)(point.X / scaleX), (int)(point.Y / scaleY));
        }

        /// <summary>
        /// Compensates a width for the DPI settings in the specified graphics instance.
        /// </summary>
        /// <param name="size">The width to compensate.</param>
        /// <param name="graphics">The instance of graphics to get the scale from.</param>
        /// <returns>The compensated width.</returns>
        public static int DpiCompensateWidth(this int width, Graphics graphics)
        {
            return (int)(width * (graphics.DpiX / defaultDpi));
        }

        /// <summary>
        /// Compensates a width for the DPI settings in the specified graphics instance.
        /// </summary>
        /// <param name="size">The width to compensate.</param>
        /// <param name="graphics">The instance of graphics to get the scale from.</param>
        /// <returns>The compensated width.</returns>
        public static float DpiCompensateWidth(this float width, Graphics graphics)
        {
            return width  / (graphics.DpiX / defaultDpi);
        }

        /// <summary>
        /// Compensates a font for the DPI settings in the specified graphics instance.
        /// </summary>
        /// <param name="point">The font to compensate.</param>
        /// <param name="graphics">The instance of graphics to get the scale from.</param>
        /// <returns>The compensate font.</returns>
        public static SerializableFont DpiCompensate(this SerializableFont font, Graphics graphics)
        {
            var clone = font.Clone();
            clone.Size = clone.Size.DpiCompensateWidth(graphics);
            return clone;
        }
    }
}
