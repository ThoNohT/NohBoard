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
            var smallRadius = (float)this.Radius / 5;

            // The sensitivity is a factor over the mouse speed.
            var sensitivity = GlobalSettings.Settings.MouseSensitivity / (float)100;

            // The total length is determined by the sensitivity, speed and radius. But never more than the radius.
            var pointerLength = (int)Math.Min(this.Radius, sensitivity * speed.GetLength() * this.Radius);

            var colorMultiplier = Math.Max(0, Math.Min(1, (float)pointerLength / this.Radius));

            Color color1 = subStyle.InnerColor;
            Color outerColor = subStyle.OuterColor;
            // The second color should be averaged over the two specified colours, based upon how far out the thingymabob is.
            var color2 = Color.FromArgb(
                (int)(color1.R * (1 - colorMultiplier) + outerColor.R * colorMultiplier),
                (int)(color1.G * (1 - colorMultiplier) + outerColor.G * colorMultiplier),
                (int)(color1.B * (1 - colorMultiplier) + outerColor.B * colorMultiplier));

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
                    g.FillEllipse(new SolidBrush(color2), Geom.CircleToRectangle(pointerEdge, (int)smallRadius));
                }
            }

            // Draw the circle in the center.
            g.FillEllipse(new SolidBrush(color1), Geom.CircleToRectangle(this.Location, (int)smallRadius));
        }

        

        /// <summary>
        /// TODO: Document.
        /// </summary>
        /// <param name="g"></param>
        public override void RenderEditing(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.Silver), Geom.CircleToRectangle(this.Location, this.Radius));
            g.DrawEllipse(new Pen(Color.White, 1), Geom.CircleToRectangle(this.Location, this.Radius));
            g.FillEllipse(new SolidBrush(Color.White), Geom.CircleToRectangle(this.Location, this.Radius / 5));
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
        /// Calculates whether the specified point is inside this element.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>True if the point is inside the element, false otherwise.</returns>
        public override bool Inside(Point point)
        {
            var bb = this.GetBoundingBox();
            return point.X >= bb.Left && point.X <= bb.Right && point.Y >= bb.Top && point.Y <= bb.Bottom;
        }

        public override bool StartManipulating(Point point)
        {
            if (!this.Inside(point)) return false;

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

                default:
                    return this;
            }
        }

        #endregion Transformations
    }
}