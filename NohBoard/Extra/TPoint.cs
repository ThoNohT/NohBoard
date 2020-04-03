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
    using ClipperLib;

    /// <summary>
    /// A simple implementation of a point that can be easily converted to externally used point classes.
    /// </summary>
    [DataContract(Name = "TPoint", Namespace = "")]
    public class TPoint
    {
        /// <summary>
        /// The X coordinate of the point.
        /// </summary>
        [DataMember]
        public int X { get; private set; }

        /// <summary>
        /// The y-coordinate of the point.
        /// </summary>
        [DataMember]
        public int Y { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TPoint" /> class.
        /// </summary>
        /// <param name="x">See <see cref="X"/>.</param>
        /// <param name="y">See <see cref="Y"/>.</param>
        public TPoint(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TPoint" /> class.
        /// </summary>
        /// <param name="x">See <see cref="X"/>.</param>
        /// <param name="y">See <see cref="Y"/>.</param>
        public TPoint(float x, float y)
        {
            this.X = (int)x;
            this.Y = (int)y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TPoint" /> class.
        /// </summary>
        /// <param name="x">See <see cref="X"/>.</param>
        /// <param name="y">See <see cref="Y"/>.</param>
        public TPoint(double x, double y)
        {
            this.X = (int)x;
            this.Y = (int)y;
        }

        #region Conversion

        /// <summary>
        /// Converts an <see cref="TPoint"/> to an <see cref="IntPoint"/>.
        /// </summary>
        /// <param name="point">The point to convert.</param>
        public static implicit operator IntPoint(TPoint point)
        {
            return new IntPoint(point.X, point.Y);
        }

        /// <summary>
        /// Converts an <see cref="TPoint"/> to a <see cref="Point"/>.
        /// </summary>
        /// <param name="point">The point to convert.</param>
        public static implicit operator Point(TPoint point)
        {
            return new Point(point.X, point.Y);
        }

        /// <summary>
        /// Converts an <see cref="IntPoint"/> to a <see cref="TPoint"/>.
        /// </summary>
        /// <param name="intPoint">The point to convert.</param>
        public static implicit operator TPoint(IntPoint intPoint)
        {
            return new TPoint(intPoint.X, intPoint.Y);
        }

        /// <summary>
        /// Converts a <see cref="Point"/> to a <see cref="TPoint"/>.
        /// </summary>
        /// <param name="point">The point to convert.</param>
        public static implicit operator TPoint(Point point)
        {
            return new TPoint(point.X, point.Y);
        } 

        #endregion Conversion

        #region Modification

        /// <summary>
        /// Adds <paramref name="size"/> to <paramref name="point"/>.
        /// </summary>
        /// <param name="point">The point to translate.</param>
        /// <param name="size">The size to translate with.</param>
        /// <returns>The translated point.</returns>
        public static TPoint operator +(TPoint point, Size size)
        {
            return point.Translate(size);
        }

        /// <summary>
        /// Adds <paramref name="size"/> to <paramref name="point"/>.
        /// </summary>
        /// <param name="point">The point to translate.</param>
        /// <param name="size">The size to translate with.</param>
        /// <returns>The translated point.</returns>
        public static TPoint operator +(TPoint point, SizeF size)
        {
            return point.Translate(size);
        }

        /// <summary>
        /// Calculates the distance between <paramref name="point"/> and <paramref name="point2"/>. This is returned
        /// as a <see cref="Size"/>.
        /// </summary>
        /// <param name="point">The point to calculate from.</param>
        /// <param name="point2">The point to subtract.</param>
        /// <returns>The distance betwene the two points.</returns>
        public static Size operator -(TPoint point, TPoint point2)
        {
            return new Size(point.X - point2.X, point.Y - point2.Y);
        }

        /// <summary>
        /// Translates this point.
        /// </summary>
        /// <param name="size">The distance to move the point.</param>
        /// <returns>the translated point.</returns>
        public TPoint Translate(Size size)
        {
            return this.Translate(size.Width, size.Height);
        }

        /// <summary>
        /// Translates this point.
        /// </summary>
        /// <param name="size">The distance to move the point.</param>
        /// <returns>the translated point.</returns>
        public TPoint Translate(SizeF size)
        {
            return this.Translate(size.Width, size.Height);
        }

        /// <summary>
        /// Translates this point.
        /// </summary>
        /// <param name="dx">The distance to move along the x axis.</param>
        /// <param name="dy">The distance to move along the y axis.</param>
        /// <returns>the translated point.</returns>
        public TPoint Translate(int dx, int dy)
        {
            return new TPoint(this.X + dx, this.Y + dy);
        }

        /// <summary>
        /// Translates this point.
        /// </summary>
        /// <param name="dx">The distance to move along the x axis.</param>
        /// <param name="dy">The distance to move along the y axis.</param>
        /// <returns>the translated point.</returns>
        public TPoint Translate(float dx, float dy)
        {
            return new TPoint(this.X + dx, this.Y + dy);
        }

        #endregion Modification

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"({this.X}, {this.Y})";
        }

        /// <summary>
        /// Creates a new version of this <see cref="TPoint"/>.
        /// </summary>
        /// <returns>The cloned <see cref="TPoint"/>.</returns>
        public TPoint Clone()
        {
            return new TPoint(this.X, this.Y);
        }
        /// <summary>
        /// Checks whether the point has changes relative to the specified other point.
        /// </summary>
        /// <param name="other">The point to compare against.</param>
        /// <returns>True if the point has changes, false otherwise.</returns>
        internal bool IsChanged(TPoint other)
        {
            return this.X != other.X || this.Y != other.Y;
        }
    }
}