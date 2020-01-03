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

    /// <summary>
    /// A panel containin all the controls for defining a <see cref="KeySubStyle"/>.
    /// </summary>
    public partial class KeySubStylePanel : UserControl
    {
        /// <summary>
        /// Indicates whether the style is being programatically set, this should not raise events.
        /// </summary>
        private bool setting;

        #region Events
        
        /// <summary>
        /// The event that is invoked when the style has been changed. Only invoked when the style is changed through
        /// the user interface, not when it is changed programmatically.
        /// </summary>
        public new event Action<KeySubStyle> StyleChanged;

        #endregion Events

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="KeySubStylePanel" /> class.
        /// </summary>
        public KeySubStylePanel()
        {
            this.InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// The currently defined substyle.
        /// </summary>
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

        /// <summary>
        /// The title of the substyle panel.
        /// </summary>
        public string Title
        {
            get { return this.lblTitle.Text; }
            set { this.lblTitle.Text = value; }
        }

        #endregion Properties

        #region Control event handlers

        /// <summary>
        /// Handles the color changed event of any color chooser.
        /// </summary>
        private void clr_ColorChanged(ColorChooser sender, System.Drawing.Color color)
        {
            if (!this.setting) this.StyleChanged?.Invoke(this.SubStyle);
        }

        /// <summary>
        /// Handles the font changed event of the text font chooser.
        /// </summary>
        private void fntText_FontChanged(FontChooser sender, System.Drawing.Font font, string link)
        {
            if (!this.setting) this.StyleChanged?.Invoke(this.SubStyle);
        }

        /// <summary>
        /// Handles the checked changed event of the show outline checkbox.
        /// </summary>
        private void chkShowOutline_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!this.setting) this.StyleChanged?.Invoke(this.SubStyle);
        }

        /// <summary>
        /// Handles the value changed event of the outline width updown.
        /// </summary>
        private void udOutlineWidth_ValueChanged(object sender, System.EventArgs e)
        {
            if (!this.setting) this.StyleChanged?.Invoke(this.SubStyle);
        }

        /// <summary>
        /// Handles the text changed event of the background image textbox.
        /// </summary>
        private void txtBackgoundImage_TextChanged(object sender, System.EventArgs e)
        {
            if (!this.setting) this.StyleChanged?.Invoke(this.SubStyle);
        }

        #endregion Control event handlers
    }
}
