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
    using ThoNohT.NohBoard.Keyboard.Styles;

    /// <summary>
    /// Represents a key in a keyboard or on a mouse.
    /// </summary>
    [DataContract(Name = "Key", Namespace = "")]
    [KnownType(typeof(KeyboardKeyDefinition))]
    [KnownType(typeof(MouseKeyDefinition))]
    [KnownType(typeof(MouseScrollDefinition))]
    public abstract class KeyDefinition : ElementDefinition
    {
        #region Fields

        /// <summary>
        /// The background brushes used for the key's pressed and unpressed state.
        /// </summary>
        private Dictionary<bool, Brush> backgroundBrushes;

        #endregion Fields

        #region Properties

        /// <summary>
        /// The boundaries of the key. This is a list of coordinates exactly defining the polygon
        /// that represents the key.
        /// </summary>
        [DataMember]
        public List<TPoint> Boundaries { get; private set; }

        /// <summary>
        /// The position of the text. Indicates the point where the center of the text should be located.
        /// </summary>
        [DataMember]
        public TPoint TextPosition { get; private set; }

        /// <summary>
        /// The keycodes of the key.
        /// </summary>
        [DataMember]
        public List<int> KeyCodes { get; private set; }

        /// <summary>
        /// The text that should be shown.
        /// </summary>
        [DataMember]
        public string Text { get; private set; }

        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyDefinition" /> class.
        /// </summary>
        /// <param name="id">The identifier of the key.</param>
        /// <param name="boundaries">The boundaries.</param>
        /// <param name="keyCodes">The keycodes.</param>
        /// <param name="text">The text of the key.</param>
        /// <param name="textPosition">The new text position.
        /// If not provided, the new position will be recalculated from the bounding box of the key.</param>
        /// <param name="manipulation">The current manipulation.</param>
        protected KeyDefinition(
            int id,
            List<TPoint> boundaries,
            List<int> keyCodes,
            string text,
            TPoint textPosition = null,
            ElementManipulation manipulation = null)
        {
            this.Id = id;
            this.Boundaries = boundaries;
            this.KeyCodes = keyCodes;
            this.Text = text;
            this.CurrentManipulation = manipulation;

            var bb = this.GetBoundingBoxImpl();
            // TODO: Re-calculate text position based on its previous value.
            this.TextPosition = textPosition ?? (TPoint)bb.Location + new Size(bb.Width / 2, bb.Height / 2);
        }

        /// <summary>
        /// Returns the bounding box of this element.
        /// </summary>
        /// <returns>A rectangle representing the bounding box of the element.</returns>
        public override Rectangle GetBoundingBox()
        {
            return this.GetBoundingBoxImpl();
        }

        /// <summary>
        /// Checks whether this key overlaps with another specified key definition.
        /// </summary>
        /// <param name="otherKey">The other key to check for overlapping on.</param>
        /// <returns><c>True</c> if the keys overlap, <c>false</c> otherwise.</returns>
        public bool BordersWith(KeyDefinition otherKey)
        {
            var clipper = new Clipper();

            clipper.AddPath(this.GetPath(), PolyType.ptSubject, true);
            clipper.AddPath(otherKey.GetPath(), PolyType.ptClip, true);

            var union = new List<List<IntPoint>>();
            clipper.Execute(ClipType.ctUnion, union);

            return union.Count == 1;
        }

        /// <summary>
        /// Calculates whether the specified point is inside this element.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>True if the point is inside the element, false otherwise.</returns>
        public override bool Inside(Point point)
        {
            return Clipper.PointInPolygon((TPoint)point, this.GetPath()) != 0; // -1 and +1 are fine, 0 is not.
        }

        /// <summary>
        /// Renders a simple representation of the element while it is being edited. This representation does not depend
        /// on the state of the program and is merely intended to provide a clear overview of the current position and
        /// shape of the element.
        /// </summary>
        /// <param name="g">The graphics context to render to.</param>
        public override void RenderEditing(Graphics g)
        {
            g.FillPolygon(Brushes.Silver, this.Boundaries.ConvertAll<Point>(x => x).ToArray());
            g.DrawPolygon(new Pen(Brushes.White, 1), this.Boundaries.ConvertAll<Point>(x => x).ToArray());
        }

        /// <summary>
        /// Renders a simple representation of the element while it is being highlighted in edit mode.
        /// </summary>
        /// <param name="g">The graphics context to render to.</param>
        public override void RenderHighlight(Graphics g)
        {
            g.FillPolygon(Constants.HighlightBrush, this.Boundaries.ConvertAll<Point>(x => x).ToArray());

            if (this.RelevantManipulation == null) return;

            switch (this.RelevantManipulation.Type)
            {
                case ElementManipulationType.MoveBoundary:
                    var boundary = this.Boundaries[this.RelevantManipulation.Index];
                    g.FillRectangle(Brushes.White, boundary.X - 3, boundary.Y - 3, 6, 6);
                    break;

                case ElementManipulationType.MoveEdge:
                    var index = this.RelevantManipulation.Index;
                    Func<int, bool> doUpdate = i => i == index || i == (index + 1) % this.Boundaries.Count;
                    var edgeBoundaries = this.Boundaries.Where((b, i) => doUpdate(i)).ToList();
                    g.DrawLine(new Pen(Color.White, 3), edgeBoundaries[0], edgeBoundaries[1]);
                    break;
            }
        }

        /// <summary>
        /// Renders a simple representation of the element while it is selected in edit mode.
        /// </summary>
        /// <param name="g">The graphics context to render to.</param>
        public override void RenderSelected(Graphics g)
        {
            g.DrawPolygon(new Pen(Constants.SelectedColor, 3), this.Boundaries.ConvertAll<Point>(x => x).ToArray());

            if (this.RelevantManipulation == null) return;

            switch (this.RelevantManipulation.Type)
            {
                case ElementManipulationType.MoveBoundary:
                    var boundary = this.Boundaries[this.RelevantManipulation.Index];
                    var specialBrush = new SolidBrush(Constants.SelectedColorSpecial);
                    g.FillRectangle(specialBrush, boundary.X - 3, boundary.Y - 3, 6, 6);
                    break;

                case ElementManipulationType.MoveEdge:
                    var index = this.RelevantManipulation.Index;
                    Func<int, bool> doUpdate = i => i == index || i == (index + 1) % this.Boundaries.Count;
                    var edgeBoundaries = this.Boundaries.Where((b, i) => doUpdate(i)).ToList();
                    g.DrawLine(new Pen(Constants.SelectedColorSpecial, 4), edgeBoundaries[0], edgeBoundaries[1]);
                    break;
            }
        }

        /// <summary>
        /// Returns the type of manipulation that will happen when interacting with the element at the specified point.
        /// </summary>
        /// <param name="point">The point to start manipulating.</param>
        /// <param name="altDown">Whether any alt key is pressed.</param>
        /// <param name="preview">whether to set the preview manipulation, or the real one.</param>
        /// <param name="translateOnly">Whether to ignore any special manipulations and only use translate.</param>
        /// <returns>The manipulation type for the specified point. <c>null</c> if no manipulation would happen
        /// at this point.</returns>
        /// <remarks>Manipulation preview is used to show what would be modified on a selected element. We cannot
        /// keep updating the element manipulation as the mouse moves, but do want to provide a visual indicator.</remarks>
        public override bool StartManipulating(Point point, bool altDown, bool preview = false, bool translateOnly = false)
        {
            if (!this.Inside(point))
            {
                this.PreviewManipulation = null;
                return false;
            }

            // At a boundary point if x and y are within -4 to +4 of the point.
            var activeBoundary = this.Boundaries.FirstOrDefault(
                b => point.X <= b.X + 4 &&
                     point.X >= b.X - 4 &&
                     point.Y <= b.Y + 4 &&
                     point.Y >= b.Y - 4);

            if (activeBoundary != null  && !translateOnly)
            {
                this.SetManipulation(
                    new ElementManipulation
                    {
                        Type = ElementManipulationType.MoveBoundary,
                        Index = this.Boundaries.IndexOf(activeBoundary)
                    },
                    preview);
                return true;
            }

            // On an edge if distances from point to both boundaries in an edge is the same as the distance between the
            // two boundaries.
            var boundaries2 = this.Boundaries.Skip(1).ToList();
            boundaries2.Add(this.Boundaries.First());

            var activeEdge = this.Boundaries.Zip(boundaries2, Tuple.Create)
                .FirstOrDefault(
                    e =>
                    {
                        if (Math.Min(e.Item1.X, e.Item2.X) - 4 > point.X) return false;
                        if (Math.Max(e.Item1.X, e.Item2.X) + 4 < point.X) return false;
                        if (Math.Min(e.Item1.Y, e.Item2.Y) - 4 > point.Y) return false;
                        if (Math.Max(e.Item1.Y, e.Item2.Y) + 4 < point.Y) return false;

                        var ac = (e.Item1 - point).Length();
                        var cb = (e.Item2 - point).Length();
                        var ab = (e.Item2 - e.Item1).Length();

                        return Math.Abs(ac + cb - ab) < 4;
                    });

            if (activeEdge != null && !translateOnly)
            {
                this.SetManipulation(
                    new ElementManipulation
                    {
                        Type = ElementManipulationType.MoveEdge,
                        Index = this.Boundaries.IndexOf(activeEdge.Item1)
                    },
                    preview);
                return true;
            }

            // When inside an element, and alt is pressed, we want to move text.
            if (altDown)
            {
                this.SetManipulation(
                    new ElementManipulation
                    {
                        Type = ElementManipulationType.MoveText,
                        Index = 0
                    },
                    preview);
                return true;
            }

            // Otherwise, we are simply moving the element.
            this.SetManipulation(
                new ElementManipulation
                {
                    Type = ElementManipulationType.Translate,
                    Index = 0
                },
                preview);

            return true;
        }

        /// <summary>
        /// Manipulates the element according to its current manipulation state. If no manipulation state is set,
        /// the element is returned without any changes. Otherwise, the element itself will determine what to modify
        /// about its position or shape and return the updated version of itself.
        /// </summary>
        /// <param name="diff">The distance to manipulate the element by.</param>
        /// <returns>The updated element.</returns>
        public override ElementDefinition Manipulate(Size diff)
        {
            if (this.CurrentManipulation == null) return this;

            switch (this.CurrentManipulation.Type)
            {
                case ElementManipulationType.Translate:
                    return this.Translate(diff.Width, diff.Height);

                case ElementManipulationType.MoveBoundary:
                    return this.MoveBoundary(this.RelevantManipulation.Index, diff);

                case ElementManipulationType.MoveEdge:
                    return this.MoveEdge(this.RelevantManipulation.Index, diff);

                case ElementManipulationType.MoveText:
                    return this.MoveText(diff);

                default:
                    return this;
            }
        }

        /// <summary>
        /// Moves the text inside the key by the specified ditsance.
        /// </summary>
        /// <param name="diff">The distance to move the text.</param>
        /// <returns>A new key definition with the moved text.</returns>
        protected abstract KeyDefinition MoveText(Size diff);

        /// <summary>
        /// Moves an edge by the specified distance.
        /// </summary>
        /// <param name="index">The index of the edge as specified by the first of the two boundaries defining it in
        /// <see cref="Boundaries"/>.</param>
        /// <param name="diff">The distance to move the edge.</param>
        /// <returns>A new key definition with the moved edge.</returns>
        protected abstract KeyDefinition MoveEdge(int index, Size diff);

        /// <summary>
        /// Moves a boundary point by the specified distance.
        /// </summary>
        /// <param name="index">The index of the boundary point in <see cref="Boundaries"/>.</param>
        /// <param name="diff">The distance to move the boundary point.</param>
        /// <returns>A new key definition with the moved boundary.</returns>
        protected abstract KeyDefinition MoveBoundary(int index, Size diff);

        /// <summary>
        /// Adds a boundary on the edge that is highlighted.
        /// </summary>
        /// <param name="location">To location to add the point at.</param>
        /// <returns>The new version of this key definition with the boundary added.</returns>
        public abstract KeyDefinition AddBoundary(TPoint location);

        /// <summary>
        /// Removes the highlighted boundary.
        /// </summary>
        /// <returns>The new version of this key definition with the boundary removed.</returns>
        public abstract KeyDefinition RemoveBoundary();

        /// <summary>
        /// Updates the key definition to occupy a region of itself plus the specified other keys.
        /// </summary>
        /// <param name="keys">The keys to union with.</param>
        /// <returns>A new key definition with the updated region.</returns>
        public abstract KeyDefinition UnionWith(List<KeyDefinition> keys);

        #region Private methods

        /// <summary>
        /// Returns a Clipper recognized path for the boundaries of this key.
        /// </summary>
        /// <returns>The path.</returns>
        protected List<IntPoint> GetPath()
        {
            return this.Boundaries.ConvertAll<IntPoint>(x => x);
        }

        /// <summary>
        /// Returns the background brush for the key.
        /// </summary>
        /// <param name="subStyle">The substyle to use for rendering the key.</param>
        /// <param name="pressed">Whether the is pressed.</param>
        /// <returns>A brush to use when rendering the background for the key.</returns>
        protected Brush GetBackgroundBrush(KeySubStyle subStyle, bool pressed)
        {
            if (this.backgroundBrushes == null) this.backgroundBrushes = new Dictionary<bool, Brush>();
            if (this.StyleVersion != GlobalSettings.StyleDependencyCounter)
            {
                this.backgroundBrushes.Clear();
                this.StyleVersion = GlobalSettings.StyleDependencyCounter;
            }

            if (this.backgroundBrushes.ContainsKey(pressed))
                return this.backgroundBrushes[pressed];

            var brush = subStyle.GetBackgroundBrush(this.GetBoundingBox());

            this.backgroundBrushes.Add(pressed, brush);
            return brush;
        }

        /// <summary>
        /// Returns the bounding box of this element.
        /// </summary>
        /// <returns>A rectangle representing the bounding box of the element.</returns>
        private Rectangle GetBoundingBoxImpl()
        {
            var xPositions = this.Boundaries.Select(b => b.X).ToList();
            var yPositions = this.Boundaries.Select(b => b.Y).ToList();

            var min = new Point(xPositions.Min(), yPositions.Min());
            var max = new Point(xPositions.Max(), yPositions.Max());

            return new Rectangle(min, new Size(max.X - min.X, max.Y - min.Y));
        }

        /// <summary>
        /// Returns a new version of this element definition with the specified properties changed.
        /// Specific method for mouse keys that do not have the additional properties of keyboard keys.
        /// </summary>
        /// <param name="boundaries">The new boundaries, or <c>null</c> if not changed.</param>
        /// <param name="keyCode">The new key code, or <c>null</c> if not changed.</param>
        /// <param name="text">The new text, or <c>null</c> if not changed.</param>
        /// <param name="textPosition">The new text position, or <c>null</c> if not changed.</param>
        /// <returns>The new element definition.</returns>
        public abstract KeyDefinition ModifyMouse(
            List<TPoint> boundaries = null, int? keyCode = null, string text = null, TPoint textPosition = null);

        #endregion Private methods
    }
}