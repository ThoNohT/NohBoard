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

namespace ThoNohT.NohBoard.Forms
{
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// A color chooser.
    /// </summary>
    public partial class ColorChooser : UserControl
    {
        /// <summary>
        /// The delegate to invoke when the color has been changed.
        /// </summary>
        /// <param name="sender">The control that changed the color.</param>
        /// <param name="color">The new color.</param>
        public delegate void ColorChangedEventHandler(ColorChooser sender, Color color);

        /// <summary>
        /// The event that is invoked when the color has been changed. Only invoked when the color is changed through
        /// the user interface, not when it is changed programmatically.
        /// </summary>
        public event ColorChangedEventHandler ColorChanged;

        public enum Shape
        {
            Square,

            Circle
        }
        
        public ColorChooser()
        {
            this.Color = Color.Black;
            this.InitializeComponent();
            this.DisplayLabel.Text = "Pick a color.";

            this.DisplayLabel.Left = this.Height + 2;
            this.DisplayLabel.Width = this.Width - this.Height - 2;

            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

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


        public Shape PreviewShape { get; set; } = Shape.Square;

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

        private void DisplayLabel_Layout(object sender, LayoutEventArgs e)
        {
            this.DisplayLabel.Left = this.Height + 2;
            this.DisplayLabel.Width = this.Width - this.Height - 2;
        }
    }
}
