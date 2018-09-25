/*
Copyright (C) 2016 by Marius Becker <marius.becker.8@gmail.com>

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

    /// <summary>
    /// Represents a rectangle, defined by 4 integer values.
    /// </summary>
    public class TRectangle
    {
        #region Constructors

        /// <summary>
        /// Creates a new <see cref="TRectangle" /> instance from its left, top, right and bottom edges.
        /// </summary>
        public TRectangle(int left, int top, int right, int bottom)
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }

        /// <summary>
        /// Creates a new <see cref="TRectangle" /> instance from a position and size.
        /// </summary>
        public TRectangle(TPoint position, TPoint size)
        {
            this.Left = position.X;
            this.Top = position.Y;
            this.Right = position.X + size.X;
            this.Bottom = position.Y + size.Y;
        }

        #endregion Constructors

        #region Edges

        /// <summary>
        /// X-coordinate of the left edge
        /// </summary>
        public int Left { get; private set; }

        /// <summary>
        /// Y-coordinate of the top edge
        /// </summary>
        public int Top { get; private set; }

        /// <summary>
        /// X-coordinate of the rightedge
        /// </summary>
        public int Right { get; private set; }

        /// <summary>
        /// Y-coordinate of the bottom edge
        /// </summary>
        public int Bottom { get; private set; }

        #endregion Edges

        #region Corner points

        /// <summary>
        /// Alias for TopLeft.
        /// </summary>
        public TPoint Position => this.TopLeft;

        /// <summary>
        /// Returns a Size object with the Width and Height properties of this rectangle.
        /// </summary>
        public Size Size => new Size(this.Right - this.Left, this.Bottom - this.Top);

        /// <summary>
        /// Returns the top left corner of the rectangle as a TPoint.
        /// </summary>
        public TPoint TopLeft => new TPoint(this.Left, this.Top);

        /// <summary>
        /// Returns the top right corner of the rectangle as a TPoint.
        /// </summary>
        public TPoint TopRight => new TPoint(this.Right, this.Top);

        /// <summary>
        /// Returns the bottom left corner of the rectangle as a TPoint.
        /// </summary>
        public TPoint BottomLeft => new TPoint(this.Left, this.Bottom);

        /// <summary>
        /// Returns the bottom right corner of the rectangle as a TPoint.
        /// </summary>
        public TPoint BottomRight => new TPoint(this.Right, this.Bottom);

        #endregion Corner points

        #region Methods

        /// <summary>
        /// Creates a rectangle that surrounds all points in a list.
        /// </summary>
        public static TRectangle FromPointList(TPoint[] points)
        {
            var left = int.MaxValue;
            var top = int.MaxValue;
            var right = int.MinValue;
            var bottom = int.MinValue;

            foreach (var point in points)
            {
                left = Math.Min(left, point.X);
                top = Math.Min(top, point.Y);
                right = Math.Max(right, point.X);
                bottom = Math.Max(bottom, point.Y);
            }

            return new TRectangle(left, top, right, bottom);
        }

        #endregion Methods
    }
}
