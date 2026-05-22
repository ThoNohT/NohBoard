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
    using ThoNohT.NohBoard.Extra;
    using ThoNohT.NohBoard.Keyboard;
    using ThoNohT.NohBoard.Keyboard.Styles;

    public partial class KeyboardStyleForm : Form
    {
        #region Fields

        private readonly KeyboardStyle initialStyle;

        private readonly KeyboardStyle currentStyle;

        #endregion Fields

        #region Events

        public new event Action<KeyboardStyle> StyleChanged;

        public event Action StyleSaved;

        #endregion Events

        #region Constructors

        public KeyboardStyleForm(KeyboardStyle initialStyle)
        {
            this.initialStyle = initialStyle ?? new KeyboardStyle();
            this.currentStyle = this.initialStyle.Clone();
            this.InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        private void KeyboardStyleForm_Load(object sender, System.EventArgs e)
        {
            this.ApplyLocalizedUiTexts();

            this.clrKeyboardBackground.Color = this.initialStyle.BackgroundColor;
            this.txtBackgoundImage.Text = this.initialStyle.BackgroundImageFileName;

            this.looseKeys.SubStyle = this.initialStyle.DefaultKeyStyle.Loose;
            this.pressedKeys.SubStyle = this.initialStyle.DefaultKeyStyle.Pressed;

            this.defaultMouseSpeed.IndicatorStyle = this.initialStyle.DefaultMouseSpeedIndicatorStyle;

            this.defaultMouseSpeed.IndicatorStyleChanged += this.defaultMouseSpeed_IndicatorStyleChanged;
            this.looseKeys.StyleChanged += this.looseKeys_SubStyleChanged;
            this.pressedKeys.StyleChanged += this.pressedKeys_SubStyleChanged;
            this.clrKeyboardBackground.ColorChanged += this.Control_ColorChanged;
            this.txtBackgoundImage.TextChanged += this.txtBackgoundImage_TextChanged;

            this.UpdateOutlineWarning();
        }

        private void ApplyLocalizedUiTexts()
        {
            this.Text = PropertyDialogsLocalization.StyleKeyboardStyleTitle;
            this.AcceptButton2.Text = PropertyDialogsLocalization.Accept;
            this.CancelButton2.Text = PropertyDialogsLocalization.Cancel;
            this.KeyboardGroup.Text = PropertyDialogsLocalization.StyleBackgroundGroup;
            this.lblBackgroundImage.Text = PropertyDialogsLocalization.StyleImageLabel;
            this.lblKeyboard.Text = PropertyDialogsLocalization.StyleKeyboardLabel;
            this.looseKeys.Title = PropertyDialogsLocalization.StyleLooseKeysTitle;
            this.pressedKeys.Title = PropertyDialogsLocalization.StylePressedKeysTitle;
            this.defaultMouseSpeed.Title = PropertyDialogsLocalization.StyleMouseSpeedIndicatorPanelTitle;
            this.clrKeyboardBackground.LabelText = PropertyDialogsLocalization.StyleBackgroundColorLabel;
            this.lblOutlineWarning.Text = PropertyDialogsLocalization.StyleOutlineWidthWarning;
        }

        private void Control_ColorChanged(ColorChooser sender, System.Drawing.Color color)
        {
            if (sender.Name != "clrKeyboardBackground") return;
            this.currentStyle.BackgroundColor = color;
            this.StyleChanged?.Invoke(this.currentStyle);
        }

        private void AcceptButton2_Click(object sender, System.EventArgs e)
        {
            this.StyleSaved?.Invoke();
            this.DialogResult = DialogResult.OK;
        }

        private void CancelButton2_Click(object sender, System.EventArgs e)
        {
            this.StyleChanged?.Invoke(this.initialStyle);
            this.DialogResult = DialogResult.Cancel;
        }

        private void pressedKeys_SubStyleChanged(KeySubStyle style)
        {
            this.currentStyle.DefaultKeyStyle.Pressed = style;
            this.StyleChanged?.Invoke(this.currentStyle);
            this.UpdateOutlineWarning();
        }

        private void looseKeys_SubStyleChanged(KeySubStyle style)
        {
            this.currentStyle.DefaultKeyStyle.Loose = style;
            this.StyleChanged?.Invoke(this.currentStyle);
            this.UpdateOutlineWarning();
        }

        private void defaultMouseSpeed_IndicatorStyleChanged(MouseSpeedIndicatorStyle style)
        {
            this.currentStyle.DefaultMouseSpeedIndicatorStyle = style;
            this.StyleChanged?.Invoke(this.currentStyle);
        }

        private void txtBackgoundImage_TextChanged(object sender, System.EventArgs e)
        {
            this.currentStyle.BackgroundImageFileName = this.txtBackgoundImage.Text.SanitizeFilename();
            this.StyleChanged?.Invoke(this.currentStyle);
        }

        private void UpdateOutlineWarning()
        {
            int OutlineWidth(KeySubStyle subStyle) => subStyle.ShowOutline ? subStyle.OutlineWidth : 0;

            var style = this.currentStyle.DefaultKeyStyle;

            this.lblOutlineWarning.Visible = OutlineWidth(style.Pressed) < OutlineWidth(style.Loose);
        }

        #endregion Methods
    }
}
