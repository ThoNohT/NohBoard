using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThoNohT.NohBoard.Extra
{
    /// <summary>
    /// Represents a rectangle, defind by 4 integer values.
    /// </summary>
    public class TRectangle
    {
        #region Properties

        public int Left { get; private set; }
        public int Top { get; private set; }
        public int Right { get; private set; }
        public int Bottom { get; private set; }

        #endregion Properties

        #region Constructors

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

        public TRectangle(int left, int top, int right, int bottom)
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }

        public TRectangle(TPoint position, TPoint size)
        {
            this.Left = position.X;
            this.Top = position.Y;
            this.Right = position.X + size.X;
            this.Bottom = position.Y + size.Y;
        }

        #endregion Constructors

        #region Corner points

        /// <summary>
        /// Alias for TopLeft.
        /// </summary>
        public TPoint Position
        {
            get
            {
                return this.TopLeft;
            }
        }

        /// <summary>
        /// Returns a TPoint where X is the width and Y is the height of the rectangle.
        /// </summary>
        public Size Size
        {
            get
            {
                return new Size(this.Right - this.Left, this.Bottom - this.Top);
            }
        }

        /// <summary>
        /// Returns the top left corner of the rectangle as a TPoint.
        /// </summary>
        public TPoint TopLeft
        {
            get
            {
                return new TPoint(this.Left, this.Top);
            }
        }

        /// <summary>
        /// Returns the top left corner of the rectangle as a TPoint.
        /// </summary>
        public TPoint TopRight
        {
            get
            {
                return new TPoint(this.Right, this.Top);
            }
        }

        /// <summary>
        /// Returns the bottom left corner of the rectangle as a TPoint.
        /// </summary>
        public TPoint BottomLeft
        {
            get
            {
                return new TPoint(this.Left, this.Bottom);
            }
        }

        /// <summary>
        /// Returns the bottom right corner of the rectangle as a TPoint.
        /// </summary>
        public TPoint BottomRight
        {
            get
            {
                return new TPoint(this.Right, this.Bottom);
            }
        }

        #endregion Corner points
    }
}
