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
    /// A font chooser.
    /// </summary>
    public partial class FontChooser : UserControl
    {
        #region Fields

        /// <summary>
        /// The selected font.
        /// </summary>
        private Font font;

        #endregion Fields

        #region Events

        /// <summary>
        /// The event that is invoked when the font has been changed. Only invoked when the font is changed through
        /// the user interface, not when it is changed programmatically.
        /// </summary>
        public new event Action<FontChooser, Font, string> FontChanged;

        #endregion Events

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FontChooser" /> class.
        /// </summary>
        public FontChooser()
        {
            this.InitializeComponent();
            this.Font = DefaultFont;
            this.DisplayLabel.Text = "Pick a font.";

            var nl = Environment.NewLine;
            var tooltip = new ToolTip();
            tooltip.SetToolTip(
                this.txtLink,
                "If a font is used that is not present on Windows systems by default, this field can be used " + nl +
                "to provide an URL to download the font from. The user can then click on the link, download the " + nl +
                "font and be able to use the style with the correct font. Note that every link only needs to be " + nl +
                "provided once, even if it is used in multiple key definitions.");

            this.DisplayLabel.Left = this.Height + 2;
            this.DisplayLabel.Width = this.Width - this.Height - 2;

            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// The selected font.
        /// </summary>
        public new Font Font
        {
            get { return this.font; }
            set
            {
                this.font = value;
                this.DisplayLabel.Font = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// The text to display.
        /// </summary>
        public string LabelText
        {
            get { return this.DisplayLabel.Text; }
            set { this.DisplayLabel.Text = value; }
        }

        /// <summary>
        /// The link to download the font.
        /// </summary>
        public string Link
        {
            get { return string.IsNullOrWhiteSpace(this.txtLink.Text) ? null : this.txtLink.Text; }
            set { this.txtLink.Text = value ?? string.Empty; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Handles a double click event. Shows a font dialog to set the new font.
        /// </summary>
        /// <param name="sender">The control that sent the event.</param>
        /// <param name="e">The event arguments.</param>
        private void FontChooser_DoubleClick(object sender, EventArgs e)
        {
            var picker = new FontDialog
            {
                FontMustExist = true,
                Font = this.Font,
            };

            if (picker.ShowDialog() == DialogResult.OK)
                this.Font = picker.Font;

            this.Refresh();

            this.FontChanged?.Invoke(this, picker.Font, this.Link);
        }

        /// <summary>
        /// Handles changing the link text. Updates the DownloadLink of the font.
        /// </summary>
        private void txtLink_TextChanged(object sender, EventArgs e)
        {
            this.FontChanged?.Invoke(this, this.Font, this.Link);
        }

        /// <summary>
        /// Handles the layouting of the control. The font display label is sized to entirely fill the entire control.
        /// </summary>
        private void DisplayLabel_Layout(object sender, LayoutEventArgs e)
        {
            this.DisplayLabel.Left = 2;
            this.DisplayLabel.Width = this.Width - 2;
        }

        #endregion Methods
    }
}
