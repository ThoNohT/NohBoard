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

    public partial class MouseSpeedStyleForm : Form
    {
        #region Fields

        private readonly MouseSpeedIndicatorStyle initialStyle;

        private readonly MouseSpeedIndicatorStyle defaultStyle;

        private MouseSpeedIndicatorStyle currentStyle;

        #endregion Fields

        #region Events

        public new event Action<MouseSpeedIndicatorStyle> StyleChanged;

        public event Action StyleSaved;

        #endregion Events

        #region Constructors

        public MouseSpeedStyleForm(MouseSpeedIndicatorStyle initialStyle, MouseSpeedIndicatorStyle defaultStyle)
        {
            this.defaultStyle = defaultStyle ?? throw new ArgumentNullException(nameof(defaultStyle));
            this.initialStyle = initialStyle;

            this.currentStyle = (MouseSpeedIndicatorStyle)initialStyle?.Clone();
            this.InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        private void MouseSpeedStyleForm_Load(object sender, EventArgs e)
        {
            this.ApplyLocalizedUiTexts();

            this.defaultMouseSpeed.IndicatorStyle = this.currentStyle ?? this.defaultStyle;
            this.chkOverwrite.Checked = this.currentStyle != null;
            this.defaultMouseSpeed.Enabled = this.chkOverwrite.Checked;

            this.defaultMouseSpeed.IndicatorStyleChanged += this.defaultMouseSpeed_IndicatorStyleChanged;
        }

        private void ApplyLocalizedUiTexts()
        {
            this.Text = PropertyDialogsLocalization.StyleMouseSpeedIndicatorStyleTitle;
            this.AcceptButton2.Text = PropertyDialogsLocalization.Accept;
            this.CancelButton2.Text = PropertyDialogsLocalization.Cancel;
            this.defaultMouseSpeed.Title = PropertyDialogsLocalization.StyleMouseSpeedShort;
            this.chkOverwrite.Text = PropertyDialogsLocalization.StyleOverwriteDefaultStyle;
        }

        private void AcceptButton2_Click(object sender, EventArgs e)
        {
            this.StyleSaved?.Invoke();
            this.DialogResult = DialogResult.OK;
        }

        private void CancelButton2_Click(object sender, EventArgs e)
        {
            this.StyleChanged?.Invoke(this.initialStyle);
            this.DialogResult = DialogResult.Cancel;
        }

        private void defaultMouseSpeed_IndicatorStyleChanged(MouseSpeedIndicatorStyle style)
        {
            this.currentStyle = style;
            this.StyleChanged?.Invoke(style);
        }

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
