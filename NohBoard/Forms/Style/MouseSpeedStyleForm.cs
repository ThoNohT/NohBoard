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

    /// <summary>
    /// The form used to define the style of a mouse speed indicator.
    /// </summary>
    public partial class MouseSpeedStyleForm : Form
    {
        #region Fields

        /// <summary>
        /// A backup style to return to if the user presses cancel.
        /// </summary>
        private readonly MouseSpeedIndicatorStyle initialStyle;

        /// <summary>
        /// The default style to revert to if the override checkbox is unchecked.
        /// </summary>
        private readonly MouseSpeedIndicatorStyle defaultStyle;

        /// <summary>
        /// The currently loaded style.
        /// </summary>
        private MouseSpeedIndicatorStyle currentStyle;

        #endregion Fields

        #region Events

        /// <summary>
        /// The event that is invoked when the style has been changed. Only invoked when the style is changed through
        /// the user interface, not when it is changed programmatically.
        /// </summary>
        public new event Action<MouseSpeedIndicatorStyle> StyleChanged;

        /// <summary>
        /// The event that is invoked when the style is saved.
        /// </summary>
        public event Action StyleSaved;

        #endregion Events

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseSpeedStyleForm" /> class.
        /// </summary>
        /// <param name="initialStyle">The initial style.</param>
        /// <param name="defaultStyle">The default style to revert to when the override checkbox is unchecked.</param>
        public MouseSpeedStyleForm(MouseSpeedIndicatorStyle initialStyle, MouseSpeedIndicatorStyle defaultStyle)
        {
            this.defaultStyle = defaultStyle ?? throw new ArgumentNullException(nameof(defaultStyle));
            this.initialStyle = initialStyle;

            this.currentStyle = (MouseSpeedIndicatorStyle)initialStyle?.Clone();
            this.InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Loads the form, setting the controls to the initial style.
        /// </summary>
        private void MouseSpeedStyleForm_Load(object sender, EventArgs e)
        {
            // Mouse speed indicator
            this.defaultMouseSpeed.IndicatorStyle = this.currentStyle ?? this.defaultStyle;
            this.chkOverwrite.Checked = this.currentStyle != null;
            this.defaultMouseSpeed.Enabled = this.chkOverwrite.Checked;

            // Only add the event handler after the initial style has been set.
            this.defaultMouseSpeed.IndicatorStyleChanged += this.defaultMouseSpeed_IndicatorStyleChanged;
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
        /// Handles change of the style, sets the new style and invokes the changed event.
        /// </summary>
        /// <param name="style">The new style.</param>
        private void defaultMouseSpeed_IndicatorStyleChanged(MouseSpeedIndicatorStyle style)
        {
            this.currentStyle = style;
            this.StyleChanged?.Invoke(style);
        }

        /// <summary>
        /// Toggles the overwriting of the default style.
        /// </summary>
        private void chkOverwrite_CheckedChanged(object sender, EventArgs e)
        {
            this.defaultMouseSpeed.Enabled = this.chkOverwrite.Checked;

            if (this.chkOverwrite.Checked)
            {
                this.currentStyle = this.initialStyle ?? this.defaultStyle;
                this.defaultMouseSpeed.IndicatorStyle = this.currentStyle;
            }
            else
            {
                this.currentStyle = null;
            }

            this.StyleChanged?.Invoke(this.currentStyle);
        }

        #endregion Methods
    }
}
