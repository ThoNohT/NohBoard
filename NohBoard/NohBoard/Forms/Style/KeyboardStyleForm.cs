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
    using Controls;
    using Extra;
    using Keyboard;
    using ThoNohT.NohBoard.Keyboard.Styles;

    /// <summary>
    /// The form used to change a keyboard's global style.
    /// </summary>
    public partial class KeyboardStyleForm : Form
    {
        #region Fields

        /// <summary>
        /// A backup style to return to if the user presses cancel.
        /// </summary>
        private readonly KeyboardStyle initialStyle;

        /// <summary>
        /// The currently loaded style.
        /// </summary>
        private readonly KeyboardStyle currentStyle;

        #endregion Fields

        #region Events

        /// <summary>
        /// The event that is invoked when the style has been changed. Only invoked when the style is changed through
        /// the user interface, not when it is changed programmatically.
        /// </summary>
        public new event Action<KeyboardStyle> StyleChanged;

        /// <summary>
        /// The event that is invoked when the style is saved.
        /// </summary>
        public event Action StyleSaved;

        #endregion Events

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardStyleForm" /> class.
        /// </summary>
        /// <param name="initialStyle">The initial style.</param>
        public KeyboardStyleForm(KeyboardStyle initialStyle)
        {
            this.initialStyle = initialStyle ?? new KeyboardStyle();
            this.currentStyle = this.initialStyle.Clone();
            this.InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Loads the form, filling the controls with the initial style.
        /// </summary>
        private void KeyboardStyleForm_Load(object sender, System.EventArgs e)
        {
            // Keyboard
            this.clrKeyboardBackground.Color = this.initialStyle.BackgroundColor;
            this.txtBackgoundImage.Text = this.initialStyle.BackgroundImageFileName;

            // Default key styles.
            this.looseKeys.SubStyle = this.initialStyle.DefaultKeyStyle.Loose;
            this.pressedKeys.SubStyle = this.initialStyle.DefaultKeyStyle.Pressed;

            // Mouse speed indicator
            this.defaultMouseSpeed.IndicatorStyle = this.initialStyle.DefaultMouseSpeedIndicatorStyle;

            // Add event handlers after styles have been set.
            this.defaultMouseSpeed.IndicatorStyleChanged += this.defaultMouseSpeed_IndicatorStyleChanged;
            this.looseKeys.StyleChanged += this.looseKeys_SubStyleChanged;
            this.pressedKeys.StyleChanged += this.pressedKeys_SubStyleChanged;
            this.clrKeyboardBackground.ColorChanged += this.Control_ColorChanged;
            this.txtBackgoundImage.TextChanged += this.txtBackgoundImage_TextChanged;

            this.UpdateOutlineWarning();
        }

        /// <summary>
        /// Handles the change of a color changed control, which is the background color control, updating the style.
        /// </summary>
        private void Control_ColorChanged(ColorChooser sender, System.Drawing.Color color)
        {
            if (sender.Name != "clrKeyboardBackground") return;
            this.currentStyle.BackgroundColor = color;
            this.StyleChanged?.Invoke(this.currentStyle);
        }

        /// <summary>
        /// Accepts the current style.
        /// </summary>
        private void AcceptButton2_Click(object sender, System.EventArgs e)
        {
            this.StyleSaved?.Invoke();
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Cancels the current style, reverting to the initial style.
        /// </summary>
        private void CancelButton2_Click(object sender, System.EventArgs e)
        {
            this.StyleChanged?.Invoke(this.initialStyle);
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Handles the change of the default style for pressed keys. Sets the new styles and invokes the changed event.
        /// </summary>
        /// <param name="style">The new style.</param>
        private void pressedKeys_SubStyleChanged(Keyboard.Styles.KeySubStyle style)
        {
            this.currentStyle.DefaultKeyStyle.Pressed = style;
            this.StyleChanged?.Invoke(this.currentStyle);
            this.UpdateOutlineWarning();
        }

        /// <summary>
        /// Handles the change of the default style for loose keys. Sets the new styles and invokes the changed event.
        /// </summary>
        /// <param name="style">The new style.</param>
        private void looseKeys_SubStyleChanged(Keyboard.Styles.KeySubStyle style)
        {
            this.currentStyle.DefaultKeyStyle.Loose = style;
            this.StyleChanged?.Invoke(this.currentStyle);
            this.UpdateOutlineWarning();
        }

        /// <summary>
        /// Handles the change of the default style for mouse speed indicators. Sets the new styles and invokes the
        /// changed event.
        /// </summary>
        /// <param name="style">The new style.</param>
        private void defaultMouseSpeed_IndicatorStyleChanged(Keyboard.Styles.MouseSpeedIndicatorStyle style)
        {
            this.currentStyle.DefaultMouseSpeedIndicatorStyle = style;
            this.StyleChanged?.Invoke(this.currentStyle);
        }

        /// <summary>
        /// Handles the text changed event of the background image textbox.
        /// </summary>
        private void txtBackgoundImage_TextChanged(object sender, System.EventArgs e)
        {
            this.currentStyle.BackgroundImageFileName = this.txtBackgoundImage.Text.SanitizeFilename();
            this.StyleChanged?.Invoke(this.currentStyle);
        }


        /// Updates the visibility of the outline warning.
        /// </summary>
        private void UpdateOutlineWarning()
        {
            int OutlineWidth(KeySubStyle subStyle) => subStyle.ShowOutline ? subStyle.OutlineWidth : 0;

            var style = this.currentStyle.DefaultKeyStyle;

            this.lblOutlineWarning.Visible = OutlineWidth(style.Pressed) < OutlineWidth(style.Loose);
        }

        #endregion Methods
    }
}
