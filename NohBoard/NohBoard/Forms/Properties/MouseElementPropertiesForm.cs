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
    using Hooking;
    using Keyboard.ElementDefinitions;

    public partial class MouseElementPropertiesForm : Form
    {
        #region Fields

        /// <summary>
        /// A backup definition to return to if the user pressed cancel.
        /// </summary>
        private readonly KeyDefinition initialDefinition;

        /// <summary>
        /// The currently loaded definition.
        /// </summary>
        private KeyDefinition currentDefinition;

        #endregion Fields

        #region Events

        /// <summary>
        /// The event that is invoked when the definition has been changed. Only invoked when the definition is changed
        /// through the user interface, not when it is changed programmatically.
        /// </summary>
        public event Action<KeyDefinition> DefinitionChanged;

        /// <summary>
        /// The event that is invoked when the definition is saved.
        /// </summary>
        public event Action DefinitionSaved;

        #endregion Events

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseElementPropertiesForm" /> class.
        /// </summary>
        /// <param name="initialDefinition">The definition to show the properties for.</param>
        public MouseElementPropertiesForm(KeyDefinition initialDefinition)
        {
            if (initialDefinition is KeyboardKeyDefinition)
                throw new Exception("A MouseElementePropertiesForm cannot be used for keyboard keys.");

            this.initialDefinition = initialDefinition;
            this.currentDefinition = (KeyDefinition) initialDefinition.Clone();

            this.InitializeComponent();

            if (initialDefinition is MouseKeyDefinition)
            {
                this.Text = "Mouse Key Properties";

                this.cmbKeyCode.Items.Clear();
                this.cmbKeyCode.Items.Add(MouseKeyCode.LeftButton);
                this.cmbKeyCode.Items.Add(MouseKeyCode.MiddleButton);
                this.cmbKeyCode.Items.Add(MouseKeyCode.RightButton);
                this.cmbKeyCode.Items.Add(MouseKeyCode.X1Button);
                this.cmbKeyCode.Items.Add(MouseKeyCode.X2Button);
            }

            if (initialDefinition is MouseScrollDefinition)
            {
                this.Text = "Mouse Scroll Properties";

                this.cmbKeyCode.Items.Clear();
                this.cmbKeyCode.Items.Add(MouseScrollKeyCode.ScrollUp);
                this.cmbKeyCode.Items.Add(MouseScrollKeyCode.ScrollRight);
                this.cmbKeyCode.Items.Add(MouseScrollKeyCode.ScrollDown);
                this.cmbKeyCode.Items.Add(MouseScrollKeyCode.ScrollLeft);
            }
        }

        /// <summary>
        /// Loads the form, setting the controls to the initial style.
        /// </summary>
        private void MouseElementPropertiesForm_Load(object sender, EventArgs e)
        {
            if (this.initialDefinition is MouseKeyDefinition)
                this.cmbKeyCode.SelectedItem = (MouseKeyCode) this.initialDefinition.KeyCodes.Single();
            if (this.initialDefinition is MouseScrollDefinition)
                this.cmbKeyCode.SelectedItem = (MouseScrollKeyCode) this.initialDefinition.KeyCodes.Single();

            this.txtText.Text = this.initialDefinition.Text;
            this.txtTextPosition.X = this.initialDefinition.TextPosition.X;
            this.txtTextPosition.Y = this.initialDefinition.TextPosition.Y;
            this.lstBoundaries.Items.AddRange(this.initialDefinition.Boundaries.Cast<object>().ToArray());

            // Only add the event handlers after the initial propererties have been set.
            this.txtTextPosition.ValueChanged += this.txtTextPosition_ValueChanged;
            this.txtText.TextChanged += this.txtText_TextChanged;
            this.cmbKeyCode.SelectedIndexChanged += this.cmbKeyCode_SelectedIndexChanged;
            this.lstBoundaries.SelectedIndexChanged += this.lstBoundaries_SelectedIndexChanged;

        }

        #region Boundaries

        /// <summary>
        /// Handles selecting an item in the boundaries list.
        /// </summary>
        private void lstBoundaries_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lstBoundaries.SelectedItem == null) return;

            this.txtBoundaries.X = ((TPoint)this.lstBoundaries.SelectedItem).X;
            this.txtBoundaries.Y = ((TPoint)this.lstBoundaries.SelectedItem).Y;
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
                this.currentDefinition.ModifyMouse(boundaries: this.lstBoundaries.Items.Cast<TPoint>().ToList());
            this.DefinitionChanged?.Invoke(this.currentDefinition);
        }

        /// <summary>
        /// Handles updating a boundary, sets the new boundaries and invokes the changed event.
        /// </summary>
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
                this.currentDefinition.ModifyMouse(boundaries: this.lstBoundaries.Items.Cast<TPoint>().ToList());
            this.DefinitionChanged?.Invoke(this.currentDefinition);
        }

        /// <summary>
        /// Handles removing a boundary, sets the new boundaries and invokes the changed event.
        /// </summary>
        private void btnRemoveBoundary_Click(object sender, EventArgs e)
        {
            if (this.lstBoundaries.SelectedItem == null) return;
            if (this.lstBoundaries.Items.Count < 4) return;

            var index = this.lstBoundaries.SelectedIndex;
            this.lstBoundaries.Items.Remove(this.lstBoundaries.SelectedItem);
            this.lstBoundaries.SelectedIndex = Math.Min(this.lstBoundaries.Items.Count - 1, index);

            this.currentDefinition =
                this.currentDefinition.ModifyMouse(boundaries: this.lstBoundaries.Items.Cast<TPoint>().ToList());
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
                this.currentDefinition.ModifyMouse(boundaries: this.lstBoundaries.Items.Cast<TPoint>().ToList());
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
                this.currentDefinition.ModifyMouse(boundaries: this.lstBoundaries.Items.Cast<TPoint>().ToList());
            this.DefinitionChanged?.Invoke(this.currentDefinition);
        }

        #endregion Boundaries

        /// <summary>
        /// Handles changing the key-code, sets the new text and invokes the changed event.
        /// </summary>
        private void cmbKeyCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.initialDefinition is MouseKeyDefinition)
                this.currentDefinition = this.currentDefinition
                    .ModifyMouse(keyCode: (int) (MouseKeyCode) this.cmbKeyCode.SelectedItem);
            if (this.initialDefinition is MouseScrollDefinition)
                this.currentDefinition = this.currentDefinition
                    .ModifyMouse(keyCode: (int) (MouseScrollKeyCode) this.cmbKeyCode.SelectedItem);

            this.DefinitionChanged?.Invoke(this.currentDefinition);
        }

        /// <summary>
        /// Handles changing the text, sets the new text and invokes the changed event.
        /// </summary>
        private void txtText_TextChanged(object sender, EventArgs e)
        {
            this.currentDefinition = this.currentDefinition.ModifyMouse(text: this.txtText.Text);
            this.DefinitionChanged?.Invoke(this.currentDefinition);
        }

        /// <summary>
        /// Handles changing the text position, sets the new text position and invokes the changed event.
        /// </summary>
        private void txtTextPosition_ValueChanged(Controls.VectorTextBox sender, TPoint newValue)
        {
            this.UpdateTextPosition();
        }

        /// <summary>
        /// Handles clicking of the "Center" button for the text location. Sets the input's value to the center of the button and updates the text.
        /// </summary>
        private void btnCenterText_Click(object sender, EventArgs e)
        {
            var bBox = this.currentDefinition.GetBoundingBox();
            var center = new TPoint((bBox.Left + bBox.Right) / 2, (bBox.Top + bBox.Bottom) / 2);

            this.txtTextPosition.X = center.X;
            this.txtTextPosition.Y = center.Y;

            this.UpdateTextPosition();
        }

        /// <summary>
        /// Takes the current text position input value, updates the current defifinition with it and invokes the change events.
        /// </summary>
        private void UpdateTextPosition()
        {
            var newPos = new TPoint(this.txtTextPosition.X, this.txtTextPosition.Y);

            this.currentDefinition = this.currentDefinition.ModifyMouse(textPosition: newPos);
            this.DefinitionChanged?.Invoke(this.currentDefinition);
        }

        /// <summary>
        /// Accepts the current definition.
        /// </summary>
        private void AcceptButton2_Click(object sender, EventArgs e)
        {
            this.DefinitionSaved?.Invoke();
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

        /// <summary>
        /// Handles the click event of the "Rectangle" button, opens the dialog.
        /// </summary>
        private void btnRectangle_Click(object sender, EventArgs e)
        {
            var rectangle = TRectangle.FromPointList(this.lstBoundaries.Items.Cast<TPoint>().ToArray());
            using (var rectangleForm = new RectangleBoundaryForm(rectangle))
            {
                rectangleForm.DimensionsSet += this.OnRectangleDimensionsSet;
                rectangleForm.ShowDialog(this);
            }
        }

        /// <summary>
        /// Called when the user clicks "Apply" in the rectangle dialog. Sets the new boundaries and invokes the changed event.
        /// </summary>
        private void OnRectangleDimensionsSet(TRectangle rectangle)
        {
            this.lstBoundaries.Items.Clear();
            this.lstBoundaries.Items.Add(rectangle.TopLeft);
            this.lstBoundaries.Items.Add(rectangle.TopRight);
            this.lstBoundaries.Items.Add(rectangle.BottomRight);
            this.lstBoundaries.Items.Add(rectangle.BottomLeft);

            this.currentDefinition =
                this.currentDefinition.ModifyMouse(boundaries: this.lstBoundaries.Items.Cast<TPoint>().ToList());
            this.DefinitionChanged?.Invoke(this.currentDefinition);
        }
    }
}
