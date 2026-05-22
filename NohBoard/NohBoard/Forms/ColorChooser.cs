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
    using ThoNohT.NohBoard.Hooking.Interop;

    public partial class ColorChooser : UserControl
    {
        public delegate void ColorChangedEventHandler(ColorChooser sender, Color color);

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

        public Color Color { get; set; }

        public string LabelText
        {
            get { return this.DisplayLabel.Text; }
            set { this.DisplayLabel.Text = value; }
        }

        public Shape PreviewShape { get; set; } = Shape.Square;

        protected override void OnPaint(PaintEventArgs e)
        {
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

            if (AppModalUi.ShowCommonDialog(picker, this.FindForm()) == DialogResult.OK)
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
