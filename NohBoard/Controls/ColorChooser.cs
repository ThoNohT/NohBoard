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

namespace ThoNohT.NohBoard.Controls
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// A color chooser.
    /// </summary>
    public partial class ColorChooser : UserControl
    {
        #region Events

        /// <summary>
        /// The event that is invoked when the color has been changed. Only invoked when the color is changed through
        /// the user interface, not when it is changed programmatically.
        /// </summary>
        public event Action<ColorChooser, Color> ColorChanged;

        #endregion Events

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorChooser" /> class.
        /// </summary>
        public ColorChooser()
        {
            this.Color = Color.Black;
            this.InitializeComponent();
            this.DisplayLabel.Text = "Pick a color.";

            this.DisplayLabel.Left = this.Height + 2;
            this.DisplayLabel.Width = this.Width - this.Height - 2;

            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// The selected color.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// The text to display.
        /// </summary>
        public string LabelText
        {
            get { return this.DisplayLabel.Text; }
            set { this.DisplayLabel.Text = value; }
        }

        /// <summary>
        /// An enumeration listing the possible shapes for the preview.
        /// </summary>
        public enum Shape
        {
            Square,

            Circle
        }

        /// <summary>
        /// The shape of the color preview area.
        /// </summary>
        public Shape PreviewShape { get; set; } = Shape.Square;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Draws the preview area when painting the control.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // Draw the square at the start.
            switch (this.PreviewShape)
            {
                case Shape.Square:
                    e.Graphics.FillRectangle(new SolidBrush(this.Color), 0, 0, this.Height, this.Height);
                    break;
                case Shape.Circle:
                    e.Graphics.FillEllipse(new SolidBrush(this.Color), 0, 0, this.Height, this.Height);
                    break;
            }

            base.OnPaint(e);
        }

        /// <summary>
        /// Handles a double-click, shows a color chooser to set the new color.
        /// </summary>
        private void ColorChooser_DoubleClick(object sender, System.EventArgs e)
        {
            var picker = new ColorDialog
            {
                Color = this.Color, FullOpen = true
            };

            if (picker.ShowDialog(this) == DialogResult.OK)
                this.Color = picker.Color;

            this.Refresh();

            this.ColorChanged?.Invoke(this, picker.Color);
        }

        /// <summary>
        /// Handles the layouting of the control. The preview area is fit in the left part of the control.
        /// </summary>
        private void DisplayLabel_Layout(object sender, LayoutEventArgs e)
        {
            this.DisplayLabel.Left = this.Height + 2;
            this.DisplayLabel.Width = this.Width - this.Height - 2;
        }

        #endregion Methods
    }
}
