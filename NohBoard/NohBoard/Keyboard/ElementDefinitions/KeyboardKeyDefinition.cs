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
    /// Represents a key on a mouse.
    /// </summary>
    [DataContract(Name = "KeyboardKey", Namespace = "")]
    public class KeyboardKeyDefinition : KeyDefinition
    {
        #region Properties

        /// <summary>
        /// The text  that should be shown if shift is pressed.
        /// </summary>
        [DataMember]
        public string ShiftText { get; private set; }

        /// <summary>
        /// Indicates whether the <see cref="ShiftText"/> should be shown when caps lock is pressed or not.
        /// </summary>
        /// <remarks>This is typically enabled for letters, but disabled for numbers.</remarks>
        [DataMember]
        public bool ChangeOnCaps { get; private set; }

        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardKeyDefinition" /> class.
        /// </summary>
        /// <param name="id">The identifier of the key.</param>
        /// <param name="boundaries">The boundaries.</param>
        /// <param name="keyCodes">The keycodes.</param>
        /// <param name="normalText">The normal text.</param>
        /// <param name="shiftText">The shift text.</param>
        /// <param name="changeOnCaps">Whether to change to shift text on caps lock.</param>
        /// <param name="textPosition">The new text position.
        /// If not provided, the new position will be recalculated from the bounding box of the key.</param>
        /// <param name="manipulation">The current manipulation.</param>
        public KeyboardKeyDefinition(
            int id,
            List<TPoint> boundaries,
            List<int> keyCodes,
            string normalText,
            string shiftText,
            bool changeOnCaps,
            TPoint textPosition = null,
            ElementManipulation manipulation = null) : base(id, boundaries, keyCodes, normalText, textPosition, manipulation)
        {
            this.ShiftText = shiftText;
            this.ChangeOnCaps = changeOnCaps;
        }


        /// <summary>
        /// Renders the key in the specified surface.
        /// </summary>
        /// <param name="g">The GDI+ surface to render on.</param>
        /// <param name="pressed">A value indicating whether to render the key in its pressed state or not.</param>
        /// <param name="shift">A value indicating whether shift is pressed during the render.</param>
        /// <param name="capsLock">A value indicating whether caps lock is pressed during the render.</param>
        public void Render(Graphics g, bool pressed, bool shift, bool capsLock)
        {
            var style = GlobalSettings.CurrentStyle.TryGetElementStyle<KeyStyle>(this.Id)
                            ?? GlobalSettings.CurrentStyle.DefaultKeyStyle;
            var defaultStyle = GlobalSettings.CurrentStyle.DefaultKeyStyle;
            var subStyle = pressed ? style?.Pressed ?? defaultStyle.Pressed: style?.Loose ?? defaultStyle.Loose;

            var txtSize = g.MeasureString(this.GetText(shift, capsLock), subStyle.Font);
            var txtPoint = new TPoint(
                this.TextPosition.X - (int)(txtSize.Width / 2),
                this.TextPosition.Y - (int)(txtSize.Height / 2));

            // Draw the background
            var backgroundBrush = this.GetBackgroundBrush(subStyle, pressed);
            g.FillPolygon(backgroundBrush, this.Boundaries.ConvertAll<Point>(x => x).ToArray());

            // Draw the text
            g.SetClip(this.GetBoundingBox());
            g.DrawString(this.GetText(shift, capsLock), subStyle.Font, new SolidBrush(subStyle.Text), (Point)txtPoint);
            g.ResetClip();

            // Draw the outline.
            if (subStyle.ShowOutline)
                g.DrawPolygon(
                    new Pen(subStyle.Outline, subStyle.OutlineWidth),
                    this.Boundaries.ConvertAll<Point>(x => x).ToArray());
        }

        /// <summary>
        /// Checks whether this key overlaps with another specified key definition.
        /// </summary>
        /// <param name="otherKey">The other key to check for overlapping on.</param>
        /// <returns><c>True</c> if the keys overlap, <c>false</c> otherwise.</returns>
        public bool BordersWith(KeyboardKeyDefinition otherKey)
        {
            var clipper = new Clipper();

            clipper.AddPath(this.GetPath(), PolyType.ptSubject, true);
            clipper.AddPath(otherKey.GetPath(), PolyType.ptClip, true);

            var union = new List<List<IntPoint>>();
            clipper.Execute(ClipType.ctUnion, union);

            return union.Count == 1;
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
            return new KeyboardKeyDefinition(
                this.Id,
                this.Boundaries.Select(b => b.Translate(dx, dy)).ToList(),
                this.KeyCodes,
                this.Text,
                this.ShiftText,
                this.ChangeOnCaps,
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

            var txtSize = g.MeasureString(this.GetText(false, false), subStyle.Font);
            var txtPoint = new TPoint(
                this.TextPosition.X - (int)(txtSize.Width / 2),
                this.TextPosition.Y - (int)(txtSize.Height / 2));

            // Draw the text
            g.SetClip(this.GetBoundingBox());
            g.DrawString(this.GetText(false, false), subStyle.Font, new SolidBrush(subStyle.Text), (Point)txtPoint);
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

            return new KeyboardKeyDefinition(
                this.Id,
                this.Boundaries.Select((b, i) => i != index ? b : b + diff).ToList(),
                this.KeyCodes,
                this.Text,
                this.ShiftText,
                this.ChangeOnCaps,
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
            return new KeyboardKeyDefinition(
                this.Id,
                this.Boundaries.ToList(),
                this.KeyCodes,
                this.Text,
                this.ShiftText,
                this.ChangeOnCaps,
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

            bool doUpdate(int i) => i == index || i == (index + 1) % this.Boundaries.Count;

            // Project the mouse movement onto the orthogonal vector.
            var edgeBoundaries = this.Boundaries.Where((b, i) => doUpdate(i)).ToList();
            var edgeVector = (SizeF)(edgeBoundaries[1] - edgeBoundaries[0]);
            var othogonalVector = edgeVector.RotateDegrees(90);
            var projectedDiff = ((SizeF)diff).ProjectOn(othogonalVector);

            return new KeyboardKeyDefinition(
                this.Id,
                this.Boundaries.Select((b, i) => !doUpdate(i) ? b : b + projectedDiff).ToList(),
                this.KeyCodes,
                this.Text,
                this.ShiftText,
                this.ChangeOnCaps,
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

            return new KeyboardKeyDefinition(
                this.Id,
                this.Boundaries.Where((b, i) => i != this.RelevantManipulation.Index).ToList(),
                this.KeyCodes,
                this.Text,
                this.ShiftText,
                this.ChangeOnCaps,
                GlobalSettings.Settings.UpdateTextPosition ? null : this.TextPosition,
                this.RelevantManipulation);
        }

        /// <summary>
        /// Adds a boundary on the edge that is highlighted.
        /// </summary>
        /// <param name="location">To location to add the point at.</param>
        /// <returns>The new version of this key definition with the boundary added.</returns>
        public override KeyDefinition AddBoundary(TPoint location)
        {
            if (this.RelevantManipulation == null) return this;
            if (this.RelevantManipulation.Type != ElementManipulationType.MoveEdge)
                throw new Exception("Attempting to add a boundary to something other than an edge.");

            var newBoundaries = this.Boundaries.ToList();
            newBoundaries.Insert(this.RelevantManipulation.Index + 1, location);

            return new KeyboardKeyDefinition(
                this.Id,
                newBoundaries,
                this.KeyCodes,
                this.Text,
                this.ShiftText,
                this.ChangeOnCaps,
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
            return this.UnionWith(keys.ConvertAll(x => (KeyboardKeyDefinition)x));
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
            throw new Exception("Cannot modify  the mouse properties of a keyboard key.");
        }

        /// <summary>
        /// Returns a new version of this element definition with the specified properties changed.
        /// </summary>
        /// <param name="boundaries">The new boundaries, or <c>null</c> if not changed.</param>
        /// <param name="keyCodes">The new key codes, or <c>null</c> if not changed.</param>
        /// <param name="text">The new text, or <c>null</c> if not changed.</param>
        /// <param name="shiftText">The new shift text, or <c>null</c> if not changed.</param>
        /// <param name="changeOnCaps">The new change on caps, or <c>null</c> if not changed.</param>
        /// <param name="textPosition">The new text position, or <c>null</c> if not changed.</param>
        /// <returns>The new element definition.</returns>
        public KeyboardKeyDefinition Modify(
            List<TPoint> boundaries = null, List<int> keyCodes = null, string text = null, string shiftText = null,
            bool? changeOnCaps = null, TPoint textPosition = null)
        {
            return new KeyboardKeyDefinition(
                this.Id,
                boundaries ?? this.Boundaries.Select(x => x.Clone()).ToList(),
                keyCodes ?? this.KeyCodes.ToList(),
                text ?? this.Text,
                shiftText ?? this.ShiftText,
                changeOnCaps ?? this.ChangeOnCaps,
                textPosition ?? this.TextPosition,
                this.CurrentManipulation);
        }

        #endregion Transformations

        #region Private methods

        /// <summary>
        /// Determines whether to use the shift text or the regular text for this key depening on the shift, caps lock
        /// state and the key's properties. Returns the text that should be displayed for this state.
        /// </summary>
        /// <param name="shift">Whether shift is pressed.</param>
        /// <param name="capsLock">Whether caps lock is active.</param>
        /// <returns>The text to display for this key.</returns>
        private string GetText(bool shift, bool capsLock)
        {
            if (GlobalSettings.Settings.Capitalization != CapitalizationMethod.FollowKeys)
            {
                // Caps lock is set based on the capitalization method.
                capsLock = GlobalSettings.Settings.Capitalization == CapitalizationMethod.Capitalize;

                // If follow shift for caps insensitive keys is true, then don't edit the shift value if ChangeOnCaps.
                // If follow shift for caps sensitive keys is true, then don't edit the shift value if not ChangeOnCaps.
                var preserveShift = GlobalSettings.Settings.FollowShiftForCapsInsensitive && !this.ChangeOnCaps
                                    || GlobalSettings.Settings.FollowShiftForCapsSensitive && this.ChangeOnCaps;
                // Shift is ignored, but only if the follow shift is not set for this key's ChangeOnCaps setting.
                shift &= preserveShift;
            }

            var capitalize = this.ChangeOnCaps && (capsLock ^ shift) || !this.ChangeOnCaps && shift;
            return capitalize ? this.ShiftText : this.Text;
        }

        /// <summary>
        /// Updates the key definition to occupy a region of itself plus the specified other keys.
        /// </summary>
        /// <param name="keys">The keys to union with.</param>
        /// <returns>A new key definition with the updated region.</returns>
        private KeyboardKeyDefinition UnionWith(IList<KeyboardKeyDefinition> keys)
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

            return new KeyboardKeyDefinition(
                this.Id,
                newBoundaries,
                this.KeyCodes,
                this.Text,
                this.ShiftText,
                this.ChangeOnCaps);
        }

        /// <summary>
        /// Returns a clone of this element definition.
        /// </summary>
        /// <returns>The cloned element definition.</returns>
        public override ElementDefinition Clone()
        {
            return new KeyboardKeyDefinition(
                this.Id,
                this.Boundaries.Select(x => x.Clone()).ToList(),
                this.KeyCodes.ToList(),
                this.Text,
                this.ShiftText,
                this.ChangeOnCaps,
                this.TextPosition,
                this.CurrentManipulation);
        }

        /// <summary>
        /// Checks whether the definition has changes relative to the specified other definition.
        /// </summary>
        /// <param name="other">The definition to compare against.</param>
        /// <returns>True if the definition has changes, false otherwise.</returns>
        public override bool IsChanged(ElementDefinition other)
        {
            if (!(other is KeyboardKeyDefinition kkd)) return true;

            if (this.Text != kkd.Text) return true;
            if (this.ShiftText != kkd.ShiftText) return true;
            if (this.ChangeOnCaps != kkd.ChangeOnCaps) return true;
            if (this.TextPosition.IsChanged(kkd.TextPosition)) return true;
            if (!this.KeyCodes.ToSet().SetEquals(kkd.KeyCodes)) return true;

            if (this.Boundaries.Count != kkd.Boundaries.Count) return true;

            // Boundary order change is also a change. So loop through them all.
            for (var i = 0; i < this.Boundaries.Count; i++)
            {
                if (this.Boundaries[i].IsChanged(kkd.Boundaries[i])) return true;
            }

            return false;
        }

        #endregion Private methods
    }
}