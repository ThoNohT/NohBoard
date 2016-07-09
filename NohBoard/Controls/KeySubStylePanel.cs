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
    using System.Windows.Forms;
    using ThoNohT.NohBoard.Extra;
    using ThoNohT.NohBoard.Keyboard.Styles;

    /// <summary>
    /// A panel containin all the controls for defining a <see cref="KeySubStyle"/>.
    /// </summary>
    public partial class KeySubStylePanel : UserControl
    {
        #region Events

        /// <summary>
        /// The delegate to invoke when the substyle has been changed.
        /// </summary>
        /// <param name="style">The new substyle.</param>
        public delegate void StyleChangedEventHandler(KeySubStyle style);

        /// <summary>
        /// The event that is invoked when the style has been changed. Only invoked when the style is changed through
        /// the user interface, not when it is changed programmatically.
        /// </summary>
        public new event StyleChangedEventHandler StyleChanged;

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
                    Font = this.fntText.Font,

                    Outline = this.clrOutline.Color,
                    ShowOutline = this.chkShowOutline.Checked,
                    OutlineWidth = (int)this.udOutlineWidth.Value
                };
            }
            set
            {
                this.clrBackground.Color = value.Background;
                this.txtBackgoundImage.Text = value.BackgroundImageFileName.SanitizeFilename();

                this.clrText.Color = value.Text;
                this.fntText.Font = value.Font;

                this.clrOutline.Color = value.Outline;
                this.chkShowOutline.Checked = value.ShowOutline;
                this.udOutlineWidth.Value = value.OutlineWidth;
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
            this.StyleChanged?.Invoke(this.SubStyle);
        }

        /// <summary>
        /// Handles the font changed event of the text font chooser.
        /// </summary>
        private void fntText_FontChanged(FontChooser sender, System.Drawing.Font font)
        {
            this.StyleChanged?.Invoke(this.SubStyle);
        }

        /// <summary>
        /// Handles the checked changed event of the show outline checkbox.
        /// </summary>
        private void chkShowOutline_CheckedChanged(object sender, System.EventArgs e)
        {
            this.StyleChanged?.Invoke(this.SubStyle);
        }

        /// <summary>
        /// Handles the value changed event of the outline width updown.
        /// </summary>
        private void udOutlineWidth_ValueChanged(object sender, System.EventArgs e)
        {
            this.StyleChanged?.Invoke(this.SubStyle);
        }

        /// <summary>
        /// Handles the text changed event of the background image textbox.
        /// </summary>
        private void txtBackgoundImage_TextChanged(object sender, System.EventArgs e)
        {
            this.StyleChanged?.Invoke(this.SubStyle);
        }

        #endregion Control event handlers
    }
}
