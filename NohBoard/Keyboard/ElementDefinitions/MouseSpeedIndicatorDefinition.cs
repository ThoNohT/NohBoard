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
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.Serialization;
    using Extra;
    using Styles;

    /// <summary>
    /// Represents an indicator that can display the current mouse speed.
    /// </summary>
    [DataContract(Name = "MouseSpeedIndicator", Namespace = "")]
    public class MouseSpeedIndicatorDefinition : ElementDefinition
    {
        #region Properties

        /// <summary>
        /// The location of the element. This indicates the center of the element.
        /// </summary>
        [DataMember]
        public TPoint Location { get; private set; }

        /// <summary>
        /// The radius of the element.
        /// </summary>
        [DataMember]
        public int Radius { get; private set; }

        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseSpeedIndicatorDefinition" /> class.
        /// </summary>
        /// <param name="id">The identifier of the key.</param>
        /// <param name="location">The location.</param>
        /// <param name="radius">The radius.</param>
        public MouseSpeedIndicatorDefinition(int id, TPoint location, int radius)
        {
            this.Id = id;
            this.Location = location;
            this.Radius = radius;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseSpeedIndicatorDefinition" /> class.
        /// </summary>
        /// <param name="id">The identifier of the key.</param>
        /// <param name="location">The location.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="manipulation">The current element manipulation.</param>
        private MouseSpeedIndicatorDefinition(int id, TPoint location, int radius, ElementManipulation manipulation)
        {
            this.Id = id;
            this.Location = location;
            this.Radius = radius;
            this.CurrentManipulation = manipulation;
        }

        /// <summary>
        /// Renders the key in the specified surface.
        /// </summary>
        /// <param name="g">The GDI+ surface to render on.</param>
        /// <param name="speed">The speed of the mouse.</param>
        public void Render(Graphics g, SizeF speed)
        {
            var subStyle = GlobalSettings.CurrentStyle.TryGetElementStyle<MouseSpeedIndicatorStyle>(this.Id)
                           ?? GlobalSettings.CurrentStyle.DefaultMouseSpeedIndicatorStyle;

            // Small circles have a fifth of the radius of the full control.
            var smallRadius = (float) this.Radius / 5;

            // The sensitivity is a factor over the mouse speed.
            var sensitivity = GlobalSettings.Settings.MouseSensitivity / (float) 100;

            // The total length is determined by the sensitivity, speed and radius. But never more than the radius.
            var pointerLength = (int) Math.Min(this.Radius, sensitivity * speed.Length() * this.Radius);

            var colorMultiplier = Math.Max(0, Math.Min(1, (float) pointerLength / this.Radius));

            Color color1 = subStyle.InnerColor;
            Color outerColor = subStyle.OuterColor;
            // The second color should be averaged over the two specified colours, based upon how far out the thingymabob is.
            var color2 = Color.FromArgb(
                (int) (color1.R * (1 - colorMultiplier) + outerColor.R * colorMultiplier),
                (int) (color1.G * (1 - colorMultiplier) + outerColor.G * colorMultiplier),
                (int) (color1.B * (1 - colorMultiplier) + outerColor.B * colorMultiplier));

            // Draw the edge.
            g.DrawEllipse(
                new Pen(color1, subStyle.OutlineWidth),
                Geom.CircleToRectangle(this.Location, this.Radius));

            // Only calculate the pointer data if it has some length.
            if (pointerLength > 0)
            {
                // Determine the angle of the pointer.
                var angle = speed.GetAngle();

                // Determine the location of the pointer end.
                var pointerEnd = this.Location.CircularTranslate(pointerLength, angle);

                // If the pointer doesn't end where it starts, draw it.
                if (pointerEnd.X != this.Location.X || pointerEnd.Y != this.Location.Y)
                {
                    // Draw the pointer, as a pie.
                    g.FillPie(
                        new LinearGradientBrush(this.Location, pointerEnd, color1, color2),
                        Geom.CircleToRectangle(this.Location, pointerLength),
                        Geom.RadToDeg(angle) - 10,
                        20);

                    // Draw a circle on the outter edge in the direction of the pointer.
                    var pointerEdge = this.Location.CircularTranslate(this.Radius, angle);
                    g.FillEllipse(new SolidBrush(color2), Geom.CircleToRectangle(pointerEdge, (int) smallRadius));
                }
            }

            // Draw the circle in the center.
            g.FillEllipse(new SolidBrush(color1), Geom.CircleToRectangle(this.Location, (int) smallRadius));
        }

        /// <summary>
        /// Renders a simple representation of the element while it is being edited. This representation does not depend
        /// on the state of the program and is merely intended to provide a clear overview of the current position and
        /// shape of the element.
        /// </summary>
        /// <param name="g">The graphics context to render to.</param>
        public override void RenderEditing(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.Silver), Geom.CircleToRectangle(this.Location, this.Radius));
            g.DrawEllipse(new Pen(Color.White, 1), Geom.CircleToRectangle(this.Location, this.Radius));
            g.FillEllipse(new SolidBrush(Color.White), Geom.CircleToRectangle(this.Location, this.Radius / 5));
        }

        /// <summary>
        /// Renders a simple representation of the element while it is being highlighted in edit mode.
        /// </summary>
        /// <param name="g">The graphics context to render to.</param>
        public override void RenderHighlight(Graphics g)
        {
            g.FillEllipse(Constants.HighlightBrush, Geom.CircleToRectangle(this.Location, this.Radius));

            if (this.RelevantManipulation?.Type == ElementManipulationType.Scale)
                g.DrawEllipse(new Pen(Color.White, 3), Geom.CircleToRectangle(this.Location, this.Radius));
        }

        /// <summary>
        /// Renders a simple representation of the element while it is selected in edit mode.
        /// </summary>
        /// <param name="g">The graphics context to render to.</param>
        public override void RenderSelected(Graphics g)
        {
            g.DrawEllipse(new Pen(Constants.SelectedColor, 2), Geom.CircleToRectangle(this.Location, this.Radius));
            g.FillEllipse(
                new SolidBrush(Constants.SelectedColor),
                Geom.CircleToRectangle(this.Location, this.Radius / 5));

            if (this.RelevantManipulation?.Type == ElementManipulationType.Scale)
                g.DrawEllipse(
                    new Pen(Constants.SelectedColorSpecial, 3),
                    Geom.CircleToRectangle(this.Location, this.Radius));
        }

        /// <summary>
        /// Returns the bounding box of this element.
        /// </summary>
        /// <returns>A rectangle representing the bounding box of the element.</returns>
        public override Rectangle GetBoundingBox()
        {
            return new Rectangle(
                new Point(this.Location.X - this.Radius, this.Location.Y - this.Radius),
                new Size(2 * this.Radius, 2 * this.Radius));
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
            return new MouseSpeedIndicatorDefinition(
                this.Id,
                new Point(this.Location.X + dx, this.Location.Y + dy),
                this.Radius,
                this.CurrentManipulation);
        }

        /// <summary>
        /// Sets the radius of the element.
        /// </summary>
        /// <param name="newRadius">The new radius.</param>
        /// <returns>A new <see cref="MouseSpeedIndicatorDefinition"/> with the specified radius.</returns>
        public MouseSpeedIndicatorDefinition SetRadius(int newRadius)
        {
            return new MouseSpeedIndicatorDefinition(
                this.Id,
                this.Location,
                newRadius,
                this.CurrentManipulation);
        }

        /// <summary>
        /// Calculates whether the specified point is inside this element.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>True if the point is inside the element, false otherwise.</returns>
        public override bool Inside(Point point)
        {
            var bb = this.GetBoundingBox();
            return point.X >= bb.Left && point.X <= bb.Right && point.Y >= bb.Top && point.Y <= bb.Bottom;
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
            SizeF d = point - this.Location;
            if (d.Length() > this.Radius + 2)
            {
                this.PreviewManipulation = null;
                return false;
            }

            // Scale if mouse over the outter edge.
            if (Math.Sqrt(Math.Abs(Math.Pow(d.Width, 2) + Math.Pow(d.Height, 2) - Math.Pow(this.Radius, 2))) < 16 && !translateOnly)
            {
                this.SetManipulation(
                    new ElementManipulation
                    {
                        Type = ElementManipulationType.Scale,
                        Index = 0,
                        Direction = d.GetUnitVector()

                    },
                    preview);
                return true;
            }

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
            if (this.RelevantManipulation == null) return this;

            switch (this.RelevantManipulation.Type)
            {
                case ElementManipulationType.Translate:
                    return this.Translate(diff.Width, diff.Height);

                case ElementManipulationType.Scale:
                    return this.Scale(diff, this.RelevantManipulation.Direction);

                default:
                    return this;
            }
        }

        /// <summary>
        /// Scales the element by the specified distance into the specified direction. Scaling is done from the center,
        /// direction is used to project the diff on.
        /// </summary>
        /// <param name="diff">The distance to perform the scale on.</param>
        /// <param name="direction">The direction to project the diff on to determine the new radius from.</param>
        /// <returns></returns>
        private ElementDefinition Scale(Size diff, SizeF direction)
        {
            var distanceToGrabPoint = direction.Multiply(this.Radius);
            var grabPoint = this.Location + distanceToGrabPoint;

            var movedGrabPoint = grabPoint + ((SizeF) diff).ProjectOn(direction);
            var movedDistance = (movedGrabPoint - this.Location).Length();

            return new MouseSpeedIndicatorDefinition(
                this.Id,
                this.Location,
                (int) movedDistance,
                this.CurrentManipulation);
        }

        #endregion Transformations

        /// <summary>
        /// Returns a clone of this element definition.
        /// </summary>
        /// <returns>The cloned element definition.</returns>
        public override ElementDefinition Clone()
        {
            return new MouseSpeedIndicatorDefinition(
                this.Id,
                this.Location.Clone(),
                this.Radius,
                this.CurrentManipulation);
        }

        /// <summary>
        /// Checks whether the definition has changes relative to the specified other definition.
        /// </summary>
        /// <param name="other">The definition to compare against.</param>
        /// <returns>True if the definition has changes, false otherwise.</returns>
        public override bool IsChanged(ElementDefinition other)
        {
            if (!(other is MouseSpeedIndicatorDefinition msid)) return true;

            return this.Location.IsChanged(msid.Location) || this.Radius != msid.Radius;
        }
    }
}