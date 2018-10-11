﻿/*
Copyright (C) 2018 by Eric Bataille <e.c.p.bataille@gmail.com>

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
        /// <param name="textPosition">The new text position.
        /// If not provided, the new position will be recalculated from the bounding box of the key.</param>
        /// <param name="manipulation">The current manipulation.</param>
        public MouseScrollDefinition(
            int id,
            List<TPoint> boundaries,
            int keyCode,
            string text,
            TPoint textPosition = null,
            ElementManipulation manipulation = null)
            : base(id, boundaries, keyCode.Singleton(), text, textPosition, manipulation)
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
                this.TextPosition.Translate(dx, dy),
                this.CurrentManipulation);
        }

        /// <summary>
        /// Renders a simple representation of the element while it is being edited. This representation does not depend
        /// on the state of the program and is merely intended to provide a clear overview of the current position and
        /// shape of the element.
        /// </summary>
        /// <param name="g">The graphics context to render to.</param>
        public override void RenderEditing(Graphics g)
        {
            base.RenderEditing(g);
            var style = GlobalSettings.CurrentStyle.TryGetElementStyle<KeyStyle>(this.Id)
                        ?? GlobalSettings.CurrentStyle.DefaultKeyStyle;
            var defaultStyle = GlobalSettings.CurrentStyle.DefaultKeyStyle;
            var subStyle = style?.Loose ?? defaultStyle.Loose;

            var text = this.Text;
            var txtSize = g.MeasureString(text, subStyle.Font);
            var txtPoint = new TPoint(
                this.TextPosition.X - (int)(txtSize.Width / 2),
                this.TextPosition.Y - (int)(txtSize.Height / 2));

            g.SetClip(this.GetBoundingBox());
            g.DrawString(text, subStyle.Font, new SolidBrush(subStyle.Text), (Point)txtPoint);
            g.ResetClip();
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
                GlobalSettings.Settings.UpdateTextPosition ? null : this.TextPosition,
                this.CurrentManipulation);
        }

        /// <summary>
        /// Moves the text inside the key by the specified ditsance.
        /// </summary>
        /// <param name="diff">The distance to move the text.</param>
        /// <returns>A new key definition with the moved text.</returns>
        protected override KeyDefinition MoveText(Size diff)
        {
            return new MouseScrollDefinition(
                this.Id,
                this.Boundaries.ToList(),
                this.KeyCodes.Single(),
                this.Text,
                this.TextPosition + diff,
                this.CurrentManipulation);
        }

        /// <summary>
        /// Moves an edge by the specified distance.
        /// </summary>
        /// <param name="index">The index of the edge as specified by the first of the two boundaries defining it in
        /// <see cref="KeyDefinition.Boundaries"/>.</param>
        /// <param name="diff">The distance to move the edge.</param>
        /// <returns>A new key definition with the moved edge.</returns>
        protected override KeyDefinition MoveEdge(int index, Size diff)
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
                GlobalSettings.Settings.UpdateTextPosition ? null : this.TextPosition,
                this.CurrentManipulation);
        }

        /// <summary>
        /// Removes the highlighted boundary.
        /// </summary>
        /// <returns>The new version of this key definition with the boundary removed.</returns>
        public override KeyDefinition RemoveBoundary()
        {
            if (this.RelevantManipulation == null) return this;
            if (this.RelevantManipulation.Type != ElementManipulationType.MoveBoundary)
                throw new Exception("Attempting to remove something other than a boundary.");

            if (this.Boundaries.Count < 4)
                throw new Exception("Cannot have less than 3 boundary in an element.");

            return new MouseScrollDefinition(
                this.Id,
                this.Boundaries.Where((b, i) => i != this.RelevantManipulation.Index).ToList(),
                this.KeyCodes.Single(),
                this.Text,
                GlobalSettings.Settings.UpdateTextPosition ? null : this.TextPosition,
                this.CurrentManipulation);
        }

        /// <summary>
        /// Adds a boundary on the edge that is highlighted.
        /// </summary>
        /// <param name="location">To location to add the point at.</param>
        /// <returns>The new version of this key definition with the boundary added.</returns>
        public override KeyDefinition AddBoundary(TPoint location)
        {
            if (this.CurrentManipulation == null) return this;
            if (this.CurrentManipulation.Type != ElementManipulationType.MoveEdge)
                throw new Exception("Attempting to add a boundary to something other than an edge.");

            var newBoundaries = this.Boundaries.ToList();
            newBoundaries.Insert(this.CurrentManipulation.Index + 1, location);

            return new MouseScrollDefinition(
                this.Id,
                newBoundaries,
                this.KeyCodes.Single(),
                this.Text,
                GlobalSettings.Settings.UpdateTextPosition ? null : this.TextPosition,
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

        /// <summary>
        /// Returns a new version of this element definition with the specified properties changed.
        /// </summary>
        /// <param name="boundaries">The new boundaries, or <c>null</c> if not changed.</param>
        /// <param name="keyCode">The new key code, or <c>null</c> if not changed.</param>
        /// <param name="text">The new text, or <c>null</c> if not changed.</param>
        /// <param name="textPosition">The new text position, or <c>null</c> if not changed.</param>
        /// <returns>The new element definition.</returns>
        public override KeyDefinition ModifyMouse(List<TPoint> boundaries = null, int? keyCode = null, string text = null, TPoint textPosition = null)
        {
            return new MouseScrollDefinition(
                this.Id,
                boundaries ?? this.Boundaries.Select(x => x.Clone()).ToList(),
                keyCode ?? this.KeyCodes.Single(),
                text ?? this.Text,
                textPosition ?? this.TextPosition,
                this.CurrentManipulation);
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

        /// <summary>
        /// Returns a clone of this element definition.
        /// </summary>
        /// <returns>The cloned element definition.</returns>
        public override ElementDefinition Clone()
        {
            return new MouseScrollDefinition(
                this.Id,
                this.Boundaries.Select(x => x.Clone()).ToList(),
                this.KeyCodes.Single(),
                this.Text,
                this.TextPosition,
                this.CurrentManipulation);
        }

        #endregion Private methods
    }
}