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
        /// <remarks>The position of the text is determined from the bounding box of the key.</remarks>
        protected KeyDefinition(int id, List<TPoint> boundaries, List<int> keyCodes, string text)
        {
            this.Id = id;
            this.Boundaries = boundaries;
            this.KeyCodes = keyCodes;
            this.Text = text;

            var bb = this.GetBoundingBoxImpl();
            this.TextPosition = (TPoint)bb.Location + new Size(bb.Width / 2, bb.Height / 2);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyDefinition" /> class.
        /// </summary>
        /// <param name="id">The identifier of the key.</param>
        /// <param name="boundaries">The boundaries.</param>
        /// <param name="keyCodes">The keycodes.</param>
        /// <param name="text">The text of the key.</param>
        /// <param name="manipulation">The current manipulation.</param>
        /// <remarks>The position of the text is determined from the bounding box of the key.</remarks>
        protected KeyDefinition(
            int id,
            List<TPoint> boundaries,
            List<int> keyCodes,
            string text,
            ElementManipulation manipulation)
        {
            this.Id = id;
            this.Boundaries = boundaries;
            this.KeyCodes = keyCodes;
            this.Text = text;
            this.CurrentManipulation = manipulation;

            var bb = this.GetBoundingBoxImpl();
            // TODO: re-calculate text position based on its previous value.
            this.TextPosition = (TPoint)bb.Location + new Size(bb.Width / 2, bb.Height / 2);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyDefinition" /> class.
        /// </summary>
        /// <param name="id">The identifier of the key.</param>
        /// <param name="boundaries">The boundaries.</param>
        /// <param name="keyCodes">The keycodes.</param>
        /// <param name="text">The text of the key.</param>
        /// <param name="textPosition">The position of the text.</param>
        protected KeyDefinition(int id, List<TPoint> boundaries, List<int> keyCodes, string text, TPoint textPosition)
        {
            this.Id = id;
            this.Boundaries = boundaries;
            this.KeyCodes = keyCodes;
            this.Text = text;
            this.TextPosition = textPosition;
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

        // TODO: Add StopManipulating?
        public override bool StartManipulating(Point point)
        {
            if (!this.Inside(point)) return false;

            // At a boundary point if x and y are within -4 to +4 of the point.
            var activeBoundary = this.Boundaries.FirstOrDefault(
                b => point.X <= b.X + 4 &&
                     point.X >= b.X - 4 &&
                     point.Y <= b.Y + 4 &&
                     point.Y >= b.Y - 4);

            if (activeBoundary != null)
            {
                this.CurrentManipulation = new ElementManipulation
                {
                    Type = ElementManipulationType.MoveBoundary,
                    Index = this.Boundaries.IndexOf(activeBoundary)
                };

                return true;
            }

            this.CurrentManipulation = new ElementManipulation
            {
                Type = ElementManipulationType.Translate,
                Index = 0
            };

            return true;
        }

        public override ElementDefinition Manipulate(Size diff)
        {
            if (this.CurrentManipulation == null) return this;

            switch (this.CurrentManipulation.Type)
            {
                case ElementManipulationType.Translate:
                    return this.Translate(diff.Width, diff.Height);

                case ElementManipulationType.MoveBoundary:
                    return this.MoveBoundary(this.CurrentManipulation.Index, diff);

                default:
                    return this;
            }
        }

        protected abstract KeyDefinition MoveBoundary(int index, Size diff);

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
        /// Rmeturns the background brush for the key.
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

        #endregion Private methods
    }
}