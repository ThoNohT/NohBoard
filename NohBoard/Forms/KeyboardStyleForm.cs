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

namespace ThoNohT.NohBoard.Forms
{
    using System.Windows.Forms;
    using ThoNohT.NohBoard.Extra;
    using ThoNohT.NohBoard.Keyboard;

    public partial class KeyboardStyleForm : Form
    {
        /// <summary>
        /// A backup style to return to if the user presses cancel.
        /// </summary>
        private KeyboardStyle backupStyle;

        private readonly MainForm parent;

        public KeyboardStyleForm(MainForm parent)
        {
            this.parent = parent;
            InitializeComponent();
        }

        private void KeyboardStyleForm_Load(object sender, System.EventArgs e)
        {
            this.backupStyle = GlobalSettings.CurrentStyle.Clone();

            // Keyboard
            this.clrKeyboardBackground.Color = GlobalSettings.CurrentStyle.BackgroundColor;

            // Pressed key
            this.clrPressedBackground.Color = GlobalSettings.CurrentStyle.DefaultKeyStyle.BackgroundPressed;
            this.clrPressedText.Color = GlobalSettings.CurrentStyle.DefaultKeyStyle.TextPressed;
            this.clrPressedOutline.Color = GlobalSettings.CurrentStyle.DefaultKeyStyle.OutlinePressed;
            this.chkPressedOutline.Checked = GlobalSettings.CurrentStyle.DefaultKeyStyle.ShowOutlinePressed;

            // Unpressed key
            this.clrUnpressedBackground.Color = GlobalSettings.CurrentStyle.DefaultKeyStyle.BackgroundLoose;
            this.clrUnpressedText.Color = GlobalSettings.CurrentStyle.DefaultKeyStyle.TextLoose;
            this.clrUnpressedOutline.Color = GlobalSettings.CurrentStyle.DefaultKeyStyle.OutlineLoose;
            this.chkUnpressedOutline.Checked = GlobalSettings.CurrentStyle.DefaultKeyStyle.ShowOutlineLoose;

            // Mouse speed indicator
            this.clrMouseSpeedLow.Color = GlobalSettings.CurrentStyle.DefaultMouseSpeedIndicatorStyle.InnerColor;
            this.clrMouseSpeedHigh.Color = GlobalSettings.CurrentStyle.DefaultMouseSpeedIndicatorStyle.OuterColor;
        }

        private void Control_ColorChanged(ColorChooser sender, System.Drawing.Color color)
        {
            switch (sender.Name)
            {
                case "clrKeyboardBackground":
                    GlobalSettings.CurrentStyle.BackgroundColor = this.clrKeyboardBackground.Color;
                    break;

                case "clrUnpressedBackground":
                    GlobalSettings.CurrentStyle.DefaultKeyStyle.BackgroundLoose = this.clrUnpressedBackground.Color;
                    break;

                case "clrUnpressedText":
                    GlobalSettings.CurrentStyle.DefaultKeyStyle.TextLoose = this.clrUnpressedText.Color;
                    break;

                case "clrUnpressedOutline":
                    GlobalSettings.CurrentStyle.DefaultKeyStyle.OutlineLoose = this.clrUnpressedOutline.Color;
                    break;

                case "clrPressedBackground":
                    GlobalSettings.CurrentStyle.DefaultKeyStyle.BackgroundPressed = this.clrPressedBackground.Color;
                    break;

                case "clrPressedText":
                    GlobalSettings.CurrentStyle.DefaultKeyStyle.TextPressed = this.clrPressedText.Color;
                    break;

                case "clrPressedOutline":
                    GlobalSettings.CurrentStyle.DefaultKeyStyle.OutlinePressed = this.clrPressedOutline.Color;
                    break;

                case "clrMouseSpeedLow":
                    GlobalSettings.CurrentStyle.DefaultMouseSpeedIndicatorStyle.InnerColor =
                        this.clrMouseSpeedLow.Color;
                    break;

                case "clrMouseSpeedHigh":
                    GlobalSettings.CurrentStyle.DefaultMouseSpeedIndicatorStyle.OuterColor =
                        this.clrMouseSpeedHigh.Color;
                    break;

                default:
                    return;
            }

            this.parent.ResetBackBrushes();
        }

        private void AcceptButton2_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CancelButton2_Click(object sender, System.EventArgs e)
        {
            GlobalSettings.CurrentStyle = this.backupStyle;
            this.parent.ResetBackBrushes();
            this.Close();
        }

        private void chkUnpressedOutline_CheckedChanged(object sender, System.EventArgs e)
        {
            GlobalSettings.CurrentStyle.DefaultKeyStyle.ShowOutlineLoose = this.chkUnpressedOutline.Checked;
            this.parent.ResetBackBrushes();
        }

        private void chkPressedOutline_CheckedChanged(object sender, System.EventArgs e)
        {
            GlobalSettings.CurrentStyle.DefaultKeyStyle.ShowOutlinePressed = this.chkPressedOutline.Checked;
            this.parent.ResetBackBrushes();
        }
    }
}
