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

namespace ThoNohT.NohBoard.Forms.Style
{
    using System;
    using System.Windows.Forms;
    using Keyboard.Styles;
    using ThoNohT.NohBoard.Extra;

    /// <summary>
    /// The form used to update the style of a key.
    /// </summary>
    public partial class KeyStyleForm : Form
    {
        #region Fields

        /// <summary>
        /// A backup style to return to if the user presses cancel.
        /// </summary>
        private readonly KeyStyle initialStyle;

        /// <summary>
        /// The style to revert to if the overwrite checkbox is unchecked.
        /// </summary>
        private readonly KeyStyle defaultStyle;

        /// <summary>
        /// The currently loaded style.
        /// </summary>
        private readonly KeyStyle currentStyle;

        #endregion Fields

        #region Events

        /// <summary>
        /// The event that is invoked when the style has been changed. Only invoked when the style is changed through
        /// the user interface, not when it is changed programmatically.
        /// </summary>
        public new event Action<KeyStyle> StyleChanged;

        /// <summary>
        /// The event that is invoked when the style is saved.
        /// </summary>
        public event Action StyleSaved;

        #endregion Events

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyStyleForm" /> class.
        /// </summary>
        /// <param name="initialStyle">The initial style.</param>
        /// <param name="defaultStyle">The default style to revert to when the override checkbox is unchecked.</param>
        public KeyStyleForm(KeyStyle initialStyle, KeyStyle defaultStyle)
        {
            if (defaultStyle == null) throw new ArgumentNullException(nameof(defaultStyle));
            if (defaultStyle.Loose == null) throw new ArgumentNullException(nameof(defaultStyle));
            if (defaultStyle.Pressed == null) throw new ArgumentNullException(nameof(defaultStyle));

            this.initialStyle = ((KeyStyle)initialStyle?.Clone()) ?? new KeyStyle
            {
                Loose = null,
                Pressed = null
            };
            this.defaultStyle = (KeyStyle)defaultStyle?.Clone();
            this.currentStyle = (KeyStyle)this.initialStyle.Clone();
            this.InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Loads the form, setting the controls to the initial style.
        /// </summary>
        private void KeyStyleForm_Load(object sender, EventArgs e)
        {
            // Default key styles.
            this.loose.SubStyle = this.initialStyle?.Loose ?? this.defaultStyle.Loose;
            this.pressed.SubStyle = this.initialStyle?.Pressed ?? this.defaultStyle.Pressed;
            this.chkOverwriteLoose.Checked = this.currentStyle?.Loose != null;
            this.chkOverwritePressed.Checked = this.currentStyle?.Pressed != null;
            this.loose.Enabled = this.chkOverwriteLoose.Checked;
            this.pressed.Enabled = this.chkOverwritePressed.Checked;

            this.UpdateOutlineWarning();

            // Only add the event handlers after the initial style has been set.
            this.pressed.StyleChanged += this.pressedKeys_SubStyleChanged;
            this.loose.StyleChanged += this.looseKeys_SubStyleChanged;
        }

        /// <summary>
        /// Accepts the current style.
        /// </summary>
        private void AcceptButton2_Click(object sender, EventArgs e)
        {
            this.StyleSaved?.Invoke();
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Cancels the current style, reverting to the initial style.
        /// </summary>
        private void CancelButton2_Click(object sender, EventArgs e)
        {
            this.StyleChanged?.Invoke(this.initialStyle);
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Handles change of the loose style, sets the new style and invokes the changed event.
        /// </summary>
        /// <param name="style">The new style.</param>
        private void looseKeys_SubStyleChanged(KeySubStyle style)
        {
            this.currentStyle.Loose = style;
            this.StyleChanged?.Invoke(this.currentStyle);
            this.UpdateOutlineWarning();
        }

        /// <summary>
        /// Handles change of the pressed style, sets the new style and invokes the changed event.
        /// </summary>
        /// <param name="style">The new style.</param>
        private void pressedKeys_SubStyleChanged(Keyboard.Styles.KeySubStyle style)
        {
            this.currentStyle.Pressed = style;
            this.StyleChanged?.Invoke(this.currentStyle);
            this.UpdateOutlineWarning();
        }

        /// <summary>
        /// Toggles the overwriting of the default style for loose keys.
        /// </summary>
        private void chkOverwriteLoose_CheckedChanged(object sender, EventArgs e)
        {
            this.loose.Enabled = this.chkOverwriteLoose.Checked;

            if (this.chkOverwriteLoose.Checked)
            {
                this.currentStyle.Loose = this.initialStyle.Loose ?? this.defaultStyle.Loose;
                this.loose.SubStyle = this.currentStyle.Loose;
            }
            else
            {
                this.currentStyle.Loose = null;
            }

            this.StyleChanged?.Invoke(this.currentStyle);
            this.UpdateOutlineWarning();
        }

        /// <summary>
        /// Toggles the overwriting of the default style for pressed keys.
        /// </summary>
        private void chkOverwritePressed_CheckedChanged(object sender, EventArgs e)
        {
            this.pressed.Enabled = this.chkOverwritePressed.Checked;

            if (this.chkOverwritePressed.Checked)
            {
                this.currentStyle.Pressed = this.initialStyle.Pressed ?? this.defaultStyle.Pressed;
                this.pressed.SubStyle = this.currentStyle.Pressed;
            }
            else
            {
                this.currentStyle.Pressed = null;
            }

            this.StyleChanged?.Invoke(this.currentStyle);

            this.UpdateOutlineWarning();
        }

        /// <summary>
        /// Updates the visibility of the outline warning.
        /// </summary>
        private void UpdateOutlineWarning()
        {
            int OutlineWidth(KeySubStyle subStyle, KeySubStyle globalSubStyle) =>
                (subStyle ?? globalSubStyle).ShowOutline ? (subStyle ?? globalSubStyle).OutlineWidth : 0;

            var def = GlobalSettings.CurrentStyle.DefaultKeyStyle;

            this.lblOutlineWarning.Visible =
                OutlineWidth(this.currentStyle.Pressed, def.Pressed) < OutlineWidth(this.currentStyle.Loose, def.Loose);
        }

        #endregion Methods
    }
}
