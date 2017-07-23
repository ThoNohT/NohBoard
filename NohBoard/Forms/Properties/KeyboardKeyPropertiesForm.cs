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
    using Extra;
    using Keyboard.ElementDefinitions;

    /// <summary>
    /// The form used to update the properties of a keyboard key.
    /// </summary>
    public partial class KeyboardKeyPropertiesForm : Form
    {
        #region Fields

        /// <summary>
        /// A backup definition to return to if the user pressed cancel.
        /// </summary>
        private readonly KeyboardKeyDefinition initialDefinition;

        /// <summary>
        /// The currently loaded definition.
        /// </summary>
        private KeyboardKeyDefinition currentDefinition;

        #endregion Fields

        #region Events

        /// <summary>
        /// The event that is invoked when the definition has been changed. Only invoked when the definition is changed
        /// through the user interface, not when it is changed programmatically.
        /// </summary>
        public event Action<KeyboardKeyDefinition> DefinitionChanged;

        #endregion Events

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardKeyPropertiesForm" /> class.
        /// </summary>
        public KeyboardKeyPropertiesForm(KeyboardKeyDefinition initialDefinition)
        {
            this.initialDefinition = initialDefinition;
            this.currentDefinition = (KeyboardKeyDefinition) initialDefinition.Clone();
            this.InitializeComponent();
        }

        /// <summary>
        /// Loads the form, setting the controls to the initial style.
        /// </summary>
        private void KeyboardKeyPropertiesForm_Load(object sender, EventArgs e)
        {
            this.txtText.Text = this.initialDefinition.Text;
            this.txtShiftText.Text = this.initialDefinition.ShiftText;
            this.txtTextPosition.X = this.initialDefinition.TextPosition.X;
            this.txtTextPosition.Y = this.initialDefinition.TextPosition.Y;
            this.lstBoundaries.Items.AddRange(this.initialDefinition.Boundaries.Cast<object>().ToArray());
            this.lstKeyCodes.Items.AddRange(
                this.initialDefinition.KeyCodes.Select(x => x).Cast<object>().ToArray());

            // Only add the event handlers after the initial properties have been set.
            this.lstBoundaries.SelectedIndexChanged += this.lstBoundaries_SelectedIndexChanged;
            this.txtText.TextChanged += this.txtText_TextChanged;
            this.txtTextPosition.ValueChanged += this.txtTextPosition_ValueChanged;
            this.txtShiftText.TextChanged += this.txtShiftText_TextChanged;
            this.lstKeyCodes.SelectedIndexChanged += this.lstKeyCodes_SelectedIndexChanged;
        }

        #region Boundaries

        /// <summary>
        /// Handles selecting an item in the boundaries list.
        /// </summary>
        private void lstBoundaries_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lstBoundaries.SelectedItem == null) return;

            this.txtBoundaries.X = ((TPoint) this.lstBoundaries.SelectedItem).X;
            this.txtBoundaries.Y = ((TPoint) this.lstBoundaries.SelectedItem).Y;
        }

        /// <summary>
        /// Handles adding a boundary, sets the new boundaries and invokes the changed event.
        /// </summary>
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

        /// <summary>
        /// Handles removing a boundary, sets the new boundaries and invokes the changed event.
        /// </summary>
        private void btnRemoveBoundary_Click(object sender, EventArgs e)
        {
            if (this.lstBoundaries.SelectedItem == null) return;

            var index = this.lstBoundaries.SelectedIndex;
            this.lstBoundaries.Items.Remove(this.lstBoundaries.SelectedItem);
            this.lstBoundaries.SelectedIndex = Math.Min(this.lstBoundaries.Items.Count - 1, index);

            this.currentDefinition =
                this.currentDefinition.Modify(boundaries: this.lstBoundaries.Items.Cast<TPoint>().ToList());
            this.DefinitionChanged?.Invoke(this.currentDefinition);
        }

        /// <summary>
        /// Handles moving a boundary up in the list, sets the new boundaries and invokes the changed event.
        /// </summary>
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

        /// <summary>
        /// Handles moving a boundary down in the list, sets the new boundaries and invokes the changed event.
        /// </summary>
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

        #endregion Boundaries

        #region KeyCodes

        /// <summary>
        /// Handles selecting an item in the boundaries list.
        /// </summary>
        private void lstKeyCodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lstKeyCodes.SelectedItem != null)
                this.udKeyCode.Value = Convert.ToInt32(this.lstKeyCodes.SelectedItem);
        }

        /// <summary>
        /// Handles adding a key code, sets the new key codes and invokes the changed event.
        /// </summary>
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

        /// <summary>
        /// Handles removing a key code, sets the new key codes and invokes the changed event.
        /// </summary>
        private void btnRemoveKeyCode_Click(object sender, EventArgs e)
        {
            if (this.lstKeyCodes.SelectedItem == null) return;

            var index = this.lstKeyCodes.SelectedIndex;
            this.lstKeyCodes.Items.Remove(this.lstKeyCodes.SelectedItem);

            this.lstKeyCodes.Items.Remove(this.lstKeyCodes.SelectedItem);
            this.lstKeyCodes.SelectedIndex = Math.Min(this.lstKeyCodes.Items.Count - 1, index);

            this.currentDefinition =
                this.currentDefinition.Modify(keyCodes: this.lstKeyCodes.Items.Cast<int>().ToList());
            this.DefinitionChanged?.Invoke(this.currentDefinition);
        }

        #endregion KeyCodes

        /// <summary>
        /// Handles changing the text, sets the new text and invokes the changed event.
        /// </summary>
        private void txtText_TextChanged(object sender, EventArgs e)
        {
            this.currentDefinition = this.currentDefinition.Modify(text: this.txtText.Text);
            this.DefinitionChanged?.Invoke(this.currentDefinition);
        }

        /// <summary>
        /// Handles changing the shift text, sets the new shift text and invokes the changed event.
        /// </summary>
        private void txtShiftText_TextChanged(object sender, EventArgs e)
        {
            this.currentDefinition = this.currentDefinition.Modify(shiftText: this.txtShiftText.Text);
            this.DefinitionChanged?.Invoke(this.currentDefinition);
        }

        /// <summary>
        /// Handles changing the text position, sets the new text position and invokes the changed event.
        /// </summary>
        private void txtTextPosition_ValueChanged(Controls.VectorTextBox sender, TPoint newValue)
        {
            this.currentDefinition = this.currentDefinition.Modify(textPosition: newValue);
            this.DefinitionChanged?.Invoke(this.currentDefinition);
        }

        /// <summary>
        /// Accepts the current definition.
        /// </summary>
        private void AcceptButton2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Cancels the current definition, reverting to the initial definition.
        /// </summary>
        private void CancelButton2_Click(object sender, EventArgs e)
        {
            this.DefinitionChanged?.Invoke(this.initialDefinition);
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
