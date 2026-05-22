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
    using System.Windows.Forms;
    using ThoNohT.NohBoard.Extra;
    using ThoNohT.NohBoard.Keyboard.Styles;

    public partial class KeySubStylePanel : UserControl
    {
        private bool setting;

        #region Events

        public new event Action<KeySubStyle> StyleChanged;

        #endregion Events

        #region Constructors

        public KeySubStylePanel()
        {
            this.InitializeComponent();
            this.ApplyLocalizedChrome();
        }

        #endregion Constructors

        private void ApplyLocalizedChrome()
        {
            this.grpBackground.Text = PropertyDialogsLocalization.StyleBackgroundGroup;
            this.lblBackgroundImage.Text = PropertyDialogsLocalization.StyleImageLabel;
            this.grpText.Text = PropertyDialogsLocalization.StylePanelTextGroup;
            this.grpOutline.Text = PropertyDialogsLocalization.StylePanelOutlineGroup;
            this.lblOutlineWidth.Text = PropertyDialogsLocalization.StyleOutlineWidthLabel;
            this.chkShowOutline.Text = PropertyDialogsLocalization.StyleShowOutline;
            this.clrText.LabelText = PropertyDialogsLocalization.StyleTextColorLabel;
            this.clrBackground.LabelText = PropertyDialogsLocalization.StyleBackgroundColorLabel;
            this.clrOutline.LabelText = PropertyDialogsLocalization.StyleOutlineColorLabel;
            this.fntText.LabelText = PropertyDialogsLocalization.StylePickFontLabel;
        }

        #region Properties

        public KeySubStyle SubStyle
        {
            get
            {
                return new KeySubStyle
                {
                    Background = this.clrBackground.Color,
                    BackgroundImageFileName = this.txtBackgoundImage.Text.SanitizeFilename(),

                    Text = this.clrText.Color,
                    Font = new SerializableFont(this.fntText.Font, this.fntText.Link),

                    Outline = this.clrOutline.Color,
                    ShowOutline = this.chkShowOutline.Checked,
                    OutlineWidth = (int)this.udOutlineWidth.Value
                };
            }
            set
            {
                this.setting = true;

                this.clrBackground.Color = value.Background;
                this.txtBackgoundImage.Text = value.BackgroundImageFileName.SanitizeFilename();

                this.clrText.Color = value.Text;
                this.fntText.Font = value.Font;
                this.fntText.Link = value.Font.DownloadUrl;

                this.clrOutline.Color = value.Outline;
                this.chkShowOutline.Checked = value.ShowOutline;
                this.udOutlineWidth.Value = value.OutlineWidth;

                this.setting = false;
            }
        }

        public string Title
        {
            get { return this.lblTitle.Text; }
            set { this.lblTitle.Text = value; }
        }

        #endregion Properties

        #region Control event handlers

        private void clr_ColorChanged(ColorChooser sender, System.Drawing.Color color)
        {
            if (!this.setting) this.StyleChanged?.Invoke(this.SubStyle);
        }

        private void fntText_FontChanged(FontChooser sender, System.Drawing.Font font, string link)
        {
            if (!this.setting) this.StyleChanged?.Invoke(this.SubStyle);
        }

        private void chkShowOutline_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!this.setting) this.StyleChanged?.Invoke(this.SubStyle);
        }

        private void udOutlineWidth_ValueChanged(object sender, System.EventArgs e)
        {
            if (!this.setting) this.StyleChanged?.Invoke(this.SubStyle);
        }

        private void txtBackgoundImage_TextChanged(object sender, System.EventArgs e)
        {
            if (!this.setting) this.StyleChanged?.Invoke(this.SubStyle);
        }

        #endregion Control event handlers

        private void fntText_Load(object sender, EventArgs e)
        {

        }

        private void clrBackground_Load(object sender, EventArgs e)
        {

        }
    }
}
