/*
Copyright (C) 2017 by Eric Bataille <e.c.p.bataille@gmail.com>

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

namespace ThoNohT.NohBoard.Forms.Properties
{
    using System;
    using System.Linq;
    using System.Windows.Forms;
    using ThoNohT.NohBoard.Forms;
    using ThoNohT.NohBoard.Extra;
    using ThoNohT.NohBoard.Hooking.Interop;
    using Keyboard.ElementDefinitions;

    public partial class KeyboardKeyPropertiesForm : Form
    {
        #region Fields

        private readonly KeyboardKeyDefinition initialDefinition;

        private KeyboardKeyDefinition currentDefinition;

        private bool detectingKeyCode;

        #endregion Fields

        #region Events

        public event Action<KeyboardKeyDefinition> DefinitionChanged;

        public event Action DefinitionSaved;

        #endregion Events

        public KeyboardKeyPropertiesForm(KeyboardKeyDefinition initialDefinition)
        {
            this.initialDefinition = initialDefinition;
            this.currentDefinition = (KeyboardKeyDefinition) initialDefinition.Clone();
            this.InitializeComponent();
        }

        private void KeyboardKeyPropertiesForm_Load(object sender, EventArgs e)
        {
            this.ApplyLocalizedUiTexts();

            this.txtText.Text = this.initialDefinition.Text;
            this.txtShiftText.Text = this.initialDefinition.ShiftText;
            this.txtTextPosition.X = this.initialDefinition.TextPosition.X;
            this.txtTextPosition.Y = this.initialDefinition.TextPosition.Y;
            this.lstBoundaries.Items.AddRange(this.initialDefinition.Boundaries.Cast<object>().ToArray());
            this.lstKeyCodes.Items.AddRange(
                this.initialDefinition.KeyCodes.Select(x => x).Cast<object>().ToArray());
            this.chkChangeOnCaps.Checked = this.initialDefinition.ChangeOnCaps;

            this.lstBoundaries.SelectedIndexChanged += this.lstBoundaries_SelectedIndexChanged;
            this.txtText.TextChanged += this.txtText_TextChanged;
            this.txtTextPosition.ValueChanged += this.txtTextPosition_ValueChanged;
            this.txtShiftText.TextChanged += this.txtShiftText_TextChanged;
            this.lstKeyCodes.SelectedIndexChanged += this.lstKeyCodes_SelectedIndexChanged;
            this.chkChangeOnCaps.CheckedChanged += this.chkChangeOnCaps_CheckedChanged;
        }

        #region Boundaries

        private void lstBoundaries_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lstBoundaries.SelectedItem == null) return;

            this.txtBoundaries.X = ((TPoint) this.lstBoundaries.SelectedItem).X;
            this.txtBoundaries.Y = ((TPoint) this.lstBoundaries.SelectedItem).Y;
        }

        private void btnAddBoundary_Click(object sender, EventArgs e)
        {
            var newBoundary = new TPoint(this.txtBoundaries.X, this.txtBoundaries.Y);
            if (this.lstBoundaries.Items.Cast<TPoint>().Any(p => p.X == newBoundary.X && p.Y == newBoundary.Y)) return;

            var newIndex = Math.Max(0, this.lstBoundaries.SelectedIndex);
            this.lstBoundaries.Items.Insert(newIndex, newBoundary);
            this.lstBoundaries.SelectedIndex = newIndex;

            this.currentDefinition =
                this.currentDefinition.Modify(boundaries: this.lstBoundaries.Items.Cast<TPoint>().ToList());
            this.DefinitionChanged?.Invoke(this.currentDefinition);
        }

        private void btnUpdateBoundary_Click(object sender, EventArgs e)
        {
            if (this.lstBoundaries.SelectedItem == null) return;

            var updateIndex = this.lstBoundaries.SelectedIndex;
            var newBoundary = new TPoint(this.txtBoundaries.X, this.txtBoundaries.Y);

            if (this.lstBoundaries.Items.Cast<TPoint>().Any(p => p.X == newBoundary.X && p.Y == newBoundary.Y)) return;

            this.lstBoundaries.Items.RemoveAt(updateIndex);
            this.lstBoundaries.Items.Insert(updateIndex, newBoundary);
            this.lstBoundaries.SelectedIndex = updateIndex;

            this.currentDefinition =
                this.currentDefinition.Modify(boundaries: this.lstBoundaries.Items.Cast<TPoint>().ToList());
            this.DefinitionChanged?.Invoke(this.currentDefinition);
        }

        private void btnRemoveBoundary_Click(object sender, EventArgs e)
        {
            if (this.lstBoundaries.SelectedItem == null) return;
            if (this.lstBoundaries.Items.Count < 4) return;

            var index = this.lstBoundaries.SelectedIndex;
            this.lstBoundaries.Items.Remove(this.lstBoundaries.SelectedItem);
            this.lstBoundaries.SelectedIndex = Math.Min(this.lstBoundaries.Items.Count - 1, index);

            this.currentDefinition =
                this.currentDefinition.Modify(boundaries: this.lstBoundaries.Items.Cast<TPoint>().ToList());
            this.DefinitionChanged?.Invoke(this.currentDefinition);
        }

        private void btnBoundaryUp_Click(object sender, EventArgs e)
        {
            var item = this.lstBoundaries.SelectedItem;
            var index = this.lstBoundaries.SelectedIndex;

            if (item == null || index == 0) return;

            this.lstBoundaries.Items.Remove(item);
            this.lstBoundaries.Items.Insert(index - 1, item);
            this.lstBoundaries.SelectedIndex = index - 1;

            this.currentDefinition =
                this.currentDefinition.Modify(boundaries: this.lstBoundaries.Items.Cast<TPoint>().ToList());
            this.DefinitionChanged?.Invoke(this.currentDefinition);
        }

        private void btnBoundaryDown_Click(object sender, EventArgs e)
        {
            var item = this.lstBoundaries.SelectedItem;
            var index = this.lstBoundaries.SelectedIndex;

            if (item == null || index == this.lstBoundaries.Items.Count - 1) return;

            this.lstBoundaries.Items.Remove(item);
            this.lstBoundaries.Items.Insert(index + 1, item);
            this.lstBoundaries.SelectedIndex = index + 1;

            this.currentDefinition =
                this.currentDefinition.Modify(boundaries: this.lstBoundaries.Items.Cast<TPoint>().ToList());
            this.DefinitionChanged?.Invoke(this.currentDefinition);
        }

        private void btnRectangle_Click(object sender, EventArgs e)
        {
            var rectangle = TRectangle.FromPointList(this.lstBoundaries.Items.Cast<TPoint>().ToArray());
            using (var rectangleForm = new RectangleBoundaryForm(rectangle))
            {
                rectangleForm.DimensionsSet += OnRectangleDimensionsSet;
                FormPlacement.AlignDialogBesideMainKeyboard(rectangleForm);
                AppModalUi.ShowDialog(rectangleForm, this);
            }
        }

        private void OnRectangleDimensionsSet(TRectangle rectangle)
        {
            this.lstBoundaries.Items.Clear();
            this.lstBoundaries.Items.Add(rectangle.TopLeft);
            this.lstBoundaries.Items.Add(rectangle.TopRight);
            this.lstBoundaries.Items.Add(rectangle.BottomRight);
            this.lstBoundaries.Items.Add(rectangle.BottomLeft);

            this.currentDefinition =
                this.currentDefinition.Modify(boundaries: this.lstBoundaries.Items.Cast<TPoint>().ToList());
            this.DefinitionChanged?.Invoke(this.currentDefinition);
        }

        #endregion Boundaries

        #region KeyCodes

        private void lstKeyCodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lstKeyCodes.SelectedItem != null)
                this.udKeyCode.Value = Convert.ToInt32(this.lstKeyCodes.SelectedItem);
        }

        private void btnAddKeyCode_Click(object sender, EventArgs e)
        {
            var newVal = Convert.ToInt32(this.udKeyCode.Value);
            if (this.lstKeyCodes.Items.Contains(newVal)) return;

            this.lstKeyCodes.Items.Add(newVal);
            this.lstKeyCodes.SelectedIndex = this.lstKeyCodes.Items.Count - 1;

            this.currentDefinition =
                this.currentDefinition.Modify(keyCodes: this.lstKeyCodes.Items.Cast<int>().ToList());
            this.DefinitionChanged?.Invoke(this.currentDefinition);
        }

        private void btnRemoveKeyCode_Click(object sender, EventArgs e)
        {
            if (this.lstKeyCodes.SelectedItem == null) return;

            var index = this.lstKeyCodes.SelectedIndex;
            this.lstKeyCodes.Items.Remove(this.lstKeyCodes.SelectedItem);
            this.lstKeyCodes.SelectedIndex = Math.Min(this.lstKeyCodes.Items.Count - 1, index);

            this.currentDefinition =
                this.currentDefinition.Modify(keyCodes: this.lstKeyCodes.Items.Cast<int>().ToList());
            this.DefinitionChanged?.Invoke(this.currentDefinition);
        }

        private void btnDetectKeyCode_Click(object sender, EventArgs e)
        {
            this.detectingKeyCode = !this.detectingKeyCode;

            if (this.detectingKeyCode)
            {
                this.btnDetectKeyCode.Text = PropertyDialogsLocalization.Detecting;
                Hooking.Interop.HookManager.KeyboardInsert = code =>
                {
                    this.udKeyCode.Value = code;
                    return true;
                };
            }
            else
            {
                this.btnDetectKeyCode.Text = PropertyDialogsLocalization.Detect;
                Hooking.Interop.HookManager.KeyboardInsert = null;
            }
        }

        private void KeyboardKeyPropertiesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.btnDetectKeyCode.Text = PropertyDialogsLocalization.Detect;
            Hooking.Interop.HookManager.KeyboardInsert = null;
        }

        #endregion KeyCodes

        private void txtText_TextChanged(object sender, EventArgs e)
        {
            this.currentDefinition = this.currentDefinition.Modify(text: this.txtText.Text);
            this.DefinitionChanged?.Invoke(this.currentDefinition);
        }

        private void txtShiftText_TextChanged(object sender, EventArgs e)
        {
            this.currentDefinition = this.currentDefinition.Modify(shiftText: this.txtShiftText.Text);
            this.DefinitionChanged?.Invoke(this.currentDefinition);
        }

        private void txtTextPosition_ValueChanged(Controls.VectorTextBox sender, TPoint newValue)
        {
            this.UpdateTextPosition();
        }

        private void btnCenterText_Click(object sender, EventArgs e)
        {
            var bBox = this.currentDefinition.GetBoundingBox();
            var center = new TPoint((bBox.Left + bBox.Right) / 2, (bBox.Top + bBox.Bottom) / 2);

            this.txtTextPosition.X = center.X;
            this.txtTextPosition.Y = center.Y;

            this.UpdateTextPosition();
        }

        private void UpdateTextPosition()
        {
            var newPos = new TPoint(this.txtTextPosition.X, this.txtTextPosition.Y);

            this.currentDefinition = this.currentDefinition.Modify(textPosition: newPos);
            this.DefinitionChanged?.Invoke(this.currentDefinition);
        }

        private void chkChangeOnCaps_CheckedChanged(object sender, EventArgs e)
        {
            this.currentDefinition = this.currentDefinition.Modify(changeOnCaps: this.chkChangeOnCaps.Checked);
            this.DefinitionChanged?.Invoke(this.currentDefinition);
        }

        private void AcceptButton2_Click(object sender, EventArgs e)
        {
            this.DefinitionSaved?.Invoke();
            this.DialogResult = DialogResult.OK;
        }

        private void CancelButton2_Click(object sender, EventArgs e)
        {
            this.DefinitionChanged?.Invoke(this.initialDefinition);
            this.DialogResult = DialogResult.Cancel;
        }

        private void ApplyLocalizedUiTexts()
        {
            this.Text = PropertyDialogsLocalization.KeyboardKeyPropertiesTitle;
            this.CancelButton2.Text = PropertyDialogsLocalization.Cancel;
            this.AcceptButton2.Text = PropertyDialogsLocalization.Accept;
            this.lblBoundaries.Text = PropertyDialogsLocalization.BoundariesLabel;
            this.lblText.Text = PropertyDialogsLocalization.TextLabel;
            this.lblTextPosition.Text = PropertyDialogsLocalization.TextPositionLabel;
            this.lblShiftText.Text = PropertyDialogsLocalization.ShiftTextLabel;
            this.lblKeyCodes.Text = PropertyDialogsLocalization.KeyCodesLabel;
            this.chkChangeOnCaps.Text = PropertyDialogsLocalization.ChangeCapsCapitalization;
            this.btnAddBoundary.Text = PropertyDialogsLocalization.Add;
            this.btnRemoveBoundary.Text = PropertyDialogsLocalization.Remove;
            this.btnBoundaryUp.Text = PropertyDialogsLocalization.Up;
            this.btnBoundaryDown.Text = PropertyDialogsLocalization.Down;
            this.btnUpdateBoundary.Text = PropertyDialogsLocalization.Update;
            this.btnCenterText.Text = PropertyDialogsLocalization.Center;
            this.btnRectangle.Text = PropertyDialogsLocalization.Rectangle;
            this.btnDetectKeyCode.Text = PropertyDialogsLocalization.Detect;
            this.btnAddKeyCode.Text = PropertyDialogsLocalization.Add;
            this.btnRemoveKeyCode.Text = PropertyDialogsLocalization.Remove;
        }
    }
}
