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

namespace ThoNohT.NohBoard.Keyboard.ElementDefinitions
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.Serialization;
    using ClipperLib;
    using Extra;
    using Styles;

    /// <summary>
    /// Represents a key in a keyboard or on a mouse.
    /// </summary>
    [DataContract(Name = "MouseScroll", Namespace = "")]
    public class MouseScrollDefinition : KeyDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MouseScrollDefinition" /> class.
        /// </summary>
        /// <param name="id">The identifier of the key.</param>
        /// <param name="boundaries">The boundaries.</param>
        /// <param name="keyCode">The keycode.</param>
        /// <param name="text">The text of the key.</param>
        /// <remarks>The position of the text is determined from the bounding box of the key.</remarks>
        public MouseScrollDefinition(int id, List<TPoint> boundaries, int keyCode, string text)
            : base(id, boundaries, keyCode.Singleton(), text)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseScrollDefinition" /> class.
        /// </summary>
        /// <param name="id">The identifier of the key.</param>
        /// <param name="boundaries">The boundaries.</param>
        /// <param name="keyCode">The keycode.</param>
        /// <param name="text">The text of the key.</param>
        /// <param name="manipulation">The current manipulation.</param>
        /// <remarks>The position of the text is determined from the bounding box of the key.</remarks>
        private MouseScrollDefinition(
            int id,
            List<TPoint> boundaries,
            int keyCode,
            string text,
            ElementManipulation manipulation)
            : base(id, boundaries, keyCode.Singleton(), text, manipulation)
        {
        }

        /// <summary>
        /// Renders the key in the specified surface.
        /// </summary>
        /// <param name="g">The GDI+ surface to render on.</param>
        /// <param name="scrollCount">The number of times the direction has been scrolled within the timeout.</param>
        public void Render(Graphics g, int scrollCount)
        {
            var pressed = scrollCount > 0;
            var style = GlobalSettings.CurrentStyle.TryGetElementStyle<KeyStyle>(this.Id)
                            ?? GlobalSettings.CurrentStyle.DefaultKeyStyle;
            var defaultStyle = GlobalSettings.CurrentStyle.DefaultKeyStyle;
            var subStyle = pressed ? style?.Pressed ?? defaultStyle.Pressed : style?.Loose ?? defaultStyle.Loose;

            var text = pressed ? scrollCount.ToString() : this.Text;
            var txtSize = g.MeasureString(text, subStyle.Font);
            var txtPoint = new TPoint(
                this.TextPosition.X - (int)(txtSize.Width / 2),
                this.TextPosition.Y - (int)(txtSize.Height / 2));

            // Draw the background
            var backgroundBrush = this.GetBackgroundBrush(subStyle, pressed);
            g.FillPolygon(backgroundBrush, this.Boundaries.ConvertAll<Point>(x => x).ToArray());

            // Draw the text
            g.SetClip(this.GetBoundingBox());
            g.DrawString(text, subStyle.Font, new SolidBrush(subStyle.Text), (Point)txtPoint);
            g.ResetClip();

            // Draw the outline.
            if (subStyle.ShowOutline)
                g.DrawPolygon(new Pen(subStyle.Outline, 1), this.Boundaries.ConvertAll<Point>(x => x).ToArray());
        }

        #region Transformations

        /// <summary>
        /// Translates the element, moving it the specified distance.
        /// </summary>
        /// <param name="dx">The distance along the x-axis.</param>
        /// <param name="dy">The distance along the y-axis.</param>
        /// <returns>A new <see cref="ElementDefinition"/> that is translated.</returns>
        public override ElementDefinition Translate(int dx, int dy)
        {
            return new MouseScrollDefinition(
                this.Id,
                this.Boundaries.Select(b => b.Translate(dx, dy)).ToList(),
                this.KeyCodes.Single(),
                this.Text,
                this.CurrentManipulation);
        }

        /// <summary>
        /// Moves a boundary point by the specified distance.
        /// </summary>
        /// <param name="index">The index of the boundary point in <see cref="KeyDefinition.Boundaries"/>.</param>
        /// <param name="diff">The distance to move the boundary point.</param>
        /// <returns>A new key definition with the moved boundary.</returns>
        protected override KeyDefinition MoveBoundary(int index, Size diff)
        {
            if (index < 0 || index >= this.Boundaries.Count)
                throw new Exception("Attempting to move a non-existent boundary.");

            return new MouseScrollDefinition(
                this.Id,
                this.Boundaries.Select((b, i) => i != index ? b : b + diff).ToList(),
                this.KeyCodes.Single(),
                this.Text,
                this.CurrentManipulation);
        }

        /// <summary>
        /// Moves an edge by the specified distance.
        /// </summary>
        /// <param name="index">The index of the edge as specified by the first of the two boundaries defining it in
        /// <see cref="KeyDefinition.Boundaries"/>.</param>
        /// <param name="diff">The distance to move the edge.</param>
        /// <returns>A new key definition with the moved edge.</returns>
        protected override ElementDefinition MoveEdge(int index, Size diff)
        {
            if (index < 0 || index >= this.Boundaries.Count)
                throw new Exception("Attempting to move a non-existent edge.");

            Func<int, bool> doUpdate = i => i == index || i == (index + 1) % this.Boundaries.Count;

            // Project the mouse movement onto the orthogonal vector.
            var edgeBoundaries = this.Boundaries.Where((b, i) => doUpdate(i)).ToList();
            var edgeVector = (SizeF)(edgeBoundaries[1] - edgeBoundaries[0]);
            var othogonalVector = edgeVector.RotateDegrees(90);
            var projectedDiff = ((SizeF)diff).ProjectOn(othogonalVector);

            return new MouseScrollDefinition(
                this.Id,
                this.Boundaries.Select((b, i) => !doUpdate(i) ? b : b + projectedDiff).ToList(),
                this.KeyCodes.Single(),
                this.Text,
                this.CurrentManipulation);
        }

        /// <summary>
        /// Updates the key definition to occupy a region of itself plus the specified other keys.
        /// </summary>
        /// <param name="keys">The keys to union with.</param>
        /// <returns>A new key definition with the updated region.</returns>
        public override KeyDefinition UnionWith(List<KeyDefinition> keys)
        {
            return this.UnionWith(keys.ConvertAll(x => (MouseScrollDefinition)x));
        }

        #endregion Transformations

        #region Private methods

        /// <summary>
        /// Updates the key definition to occupy a region of itself plus the specified other keys.
        /// </summary>
        /// <param name="keys">The keys to union with.</param>
        /// <returns>A new key definition with the updated region.</returns>
        private MouseScrollDefinition UnionWith(IList<MouseScrollDefinition> keys)
        {
            var newBoundaries = this.Boundaries.Select(b => new TPoint(b.X, b.Y)).ToList();

            if (keys.Any())
            {
                var cl = new Clipper();
                cl.AddPath(this.GetPath(), PolyType.ptSubject, true);
                cl.AddPaths(keys.Select(x => x.GetPath()).ToList(), PolyType.ptClip, true);
                var union = new List<List<IntPoint>>();
                cl.Execute(ClipType.ctUnion, union);

                if (union.Count > 1)
                    throw new ArgumentException("Cannot union two non-overlapping keys.");

                newBoundaries = union.Single().ConvertAll<TPoint>(x => x);
            }

            return new MouseScrollDefinition(this.Id, newBoundaries, this.KeyCodes.Single(), this.Text);
        }

        #endregion Private methods
    }
}