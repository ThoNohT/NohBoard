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
    using ThoNohT.NohBoard.Extra;
    using ThoNohT.NohBoard.Forms;
    using ThoNohT.NohBoard.Hooking.Interop;

    public partial class FontChooser : UserControl
    {
        #region Fields

        private Font font;

        #endregion Fields

        #region Events

        public new event Action<FontChooser, Font, string> FontChanged;

        #endregion Events

        #region Constructors

        public FontChooser()
        {
            this.InitializeComponent();
            this.lblPrompt.Text = PropertyDialogsLocalization.StylePickFontLabel;
            this.lblPrompt.Font = SystemFonts.MessageBoxFont;
            this.lblLink.Text = PropertyDialogsLocalization.StyleFontLinkLabel;
            this.Font = DefaultFont;

            this.LayoutFontPreviewRow();

            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        #endregion Constructors

        #region Properties

        public new Font Font
        {
            get { return this.font; }
            set
            {
                this.font = value;
                this.UpdateFontPreview();
                this.Refresh();
            }
        }

        public string LabelText
        {
            get { return this.lblPrompt.Text; }
            set { this.lblPrompt.Text = value; }
        }

        public string Link
        {
            get { return string.IsNullOrWhiteSpace(this.txtLink.Text) ? null : this.txtLink.Text; }
            set { this.txtLink.Text = value ?? string.Empty; }
        }

        #endregion Properties

        #region Methods

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.LayoutFontPreviewRow();
        }

        private void FontChooser_DoubleClick(object sender, EventArgs e)
        {
            var picker = new FontDialog
            {
                FontMustExist = true,
                Font = this.Font,
            };

            if (AppModalUi.ShowCommonDialog(picker, this.FindForm()) == DialogResult.OK)
                this.Font = picker.Font;

            this.Refresh();

            this.FontChanged?.Invoke(this, picker.Font, this.Link);
        }

        private void txtLink_TextChanged(object sender, EventArgs e)
        {
            this.FontChanged?.Invoke(this, this.Font, this.Link);
        }

        private void UpdateFontPreview()
        {
            if (this.font == null)
            {
                this.DisplayLabel.Font = SystemFonts.MessageBoxFont;
                this.DisplayLabel.Text = string.Empty;
                return;
            }

            this.DisplayLabel.Font = this.font;
            this.DisplayLabel.Text = PropertyDialogsLocalization.StyleFontPreviewSample;
        }

        private void LayoutFontPreviewRow()
        {
            const int margin = 5;
            var width = Math.Max(0, this.Width - margin * 2);
            this.lblPrompt.Left = margin;
            this.lblPrompt.Width = width;
            this.DisplayLabel.Left = margin;
            this.DisplayLabel.Width = width;
        }

        private void DisplayLabel_Layout(object sender, LayoutEventArgs e)
        {
            this.LayoutFontPreviewRow();
        }

        #endregion Methods
    }
}
