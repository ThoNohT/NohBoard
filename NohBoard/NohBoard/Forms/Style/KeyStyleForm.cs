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

    public partial class KeyStyleForm : Form
    {
        #region Fields

        private readonly KeyStyle initialStyle;

        private readonly KeyStyle defaultStyle;

        private readonly KeyStyle currentStyle;

        #endregion Fields

        #region Events

        public new event Action<KeyStyle> StyleChanged;

        public event Action StyleSaved;

        #endregion Events

        #region Constructors

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

        private void KeyStyleForm_Load(object sender, EventArgs e)
        {
            this.ApplyLocalizedUiTexts();

            this.loose.SubStyle = this.initialStyle?.Loose ?? this.defaultStyle.Loose;
            this.pressed.SubStyle = this.initialStyle?.Pressed ?? this.defaultStyle.Pressed;
            this.chkOverwriteLoose.Checked = this.currentStyle?.Loose != null;
            this.chkOverwritePressed.Checked = this.currentStyle?.Pressed != null;
            this.loose.Enabled = this.chkOverwriteLoose.Checked;
            this.pressed.Enabled = this.chkOverwritePressed.Checked;

            this.UpdateOutlineWarning();

            this.pressed.StyleChanged += this.pressedKeys_SubStyleChanged;
            this.loose.StyleChanged += this.looseKeys_SubStyleChanged;
        }

        private void ApplyLocalizedUiTexts()
        {
            this.Text = PropertyDialogsLocalization.StyleKeyStyleTitle;
            this.AcceptButton2.Text = PropertyDialogsLocalization.Accept;
            this.CancelButton2.Text = PropertyDialogsLocalization.Cancel;
            this.loose.Title = PropertyDialogsLocalization.StyleLooseShort;
            this.pressed.Title = PropertyDialogsLocalization.StylePressedShort;
            this.chkOverwriteLoose.Text = PropertyDialogsLocalization.StyleOverwriteDefaultStyle;
            this.chkOverwritePressed.Text = PropertyDialogsLocalization.StyleOverwriteDefaultStyle;
            this.lblOutlineWarning.Text = PropertyDialogsLocalization.StyleOutlineWidthWarning;
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

        private void looseKeys_SubStyleChanged(KeySubStyle style)
        {
            this.currentStyle.Loose = style;
            this.StyleChanged?.Invoke(this.currentStyle);
            this.UpdateOutlineWarning();
        }

        private void pressedKeys_SubStyleChanged(Keyboard.Styles.KeySubStyle style)
        {
            this.currentStyle.Pressed = style;
            this.StyleChanged?.Invoke(this.currentStyle);
            this.UpdateOutlineWarning();
        }

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
