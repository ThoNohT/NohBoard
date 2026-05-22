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
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using ThoNohT.NohBoard.Hooking.Interop;

    public partial class FontChooser : UserControl
    {
        private Font font;

        public delegate void FontChangedEventHandler(FontChooser sender, Font font);

        public new event FontChangedEventHandler FontChanged;
        
        public FontChooser()
        {
            this.InitializeComponent();
            this.Font = DefaultFont;
            this.DisplayLabel.Text = "Pick a font.";

            this.DisplayLabel.Left = this.Height + 2;
            this.DisplayLabel.Width = this.Width - this.Height - 2;

            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        public new Font Font
        {
            get { return this.font; }
            set
            {
                Console.WriteLine($"Font changed to {value.Name}");
                this.font = value;
                this.DisplayLabel.Font = value;
                this.Refresh();
            }
        }

        public string LabelText
        {
            get { return this.DisplayLabel.Text; }
            set { this.DisplayLabel.Text = value; }
        }
        
        private void FontChooser_DoubleClick(object sender, System.EventArgs e)
        {
            var picker = new FontDialog
            {
                FontMustExist = true,
                Font = this.Font
            };

            if (AppModalUi.ShowCommonDialog(picker, this.FindForm()) == DialogResult.OK)
                this.Font = picker.Font;

            this.Refresh();

            this.FontChanged?.Invoke(this, picker.Font);
        }

        private void DisplayLabel_Layout(object sender, LayoutEventArgs e)
        {
            this.DisplayLabel.Left = 2;
            this.DisplayLabel.Width = this.Width - 2;
        }
    }
}
