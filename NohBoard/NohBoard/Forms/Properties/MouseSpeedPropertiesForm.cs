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
    using System.Windows.Forms;
    using Keyboard.ElementDefinitions;
    using ThoNohT.NohBoard.Controls;
    using ThoNohT.NohBoard.Extra;

    /// <summary>
    /// The form used to update the properties of a mouse speed indicator.
    /// </summary>
    public partial class MouseSpeedPropertiesForm : Form
    {
        #region Fields

        /// <summary>
        /// A backup definition to return to if the user pressed cancel.
        /// </summary>
        private readonly MouseSpeedIndicatorDefinition initialDefinition;

        /// <summary>
        /// The currently loaded definition.
        /// </summary>
        private MouseSpeedIndicatorDefinition currentDefinition;

        #endregion Fields

        #region Events

        /// <summary>
        /// The event that is invoked when the definition has been changed. Only invoked when the definition is changed
        /// through the user interface, not when it is changed programmatically.
        /// </summary>
        public event Action<MouseSpeedIndicatorDefinition> DefinitionChanged;

        /// <summary>
        /// The event that is invoked when the definition is saved.
        /// </summary>
        public event Action DefinitionSaved;

        #endregion Events

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseSpeedPropertiesForm" /> class.
        /// </summary>
        public MouseSpeedPropertiesForm(MouseSpeedIndicatorDefinition initialDefinition)
        {
            this.initialDefinition = initialDefinition ?? throw new ArgumentNullException(nameof(initialDefinition));
            this.currentDefinition = (MouseSpeedIndicatorDefinition)this.initialDefinition.Clone();
            this.InitializeComponent();
        }

        /// <summary>
        /// Loads the form, setting the controls to the initial style.
        /// </summary>
        private void MouseSpeedPropertiesForm_Load(object sender, EventArgs e)
        {
            // Mouse speed indicator
            this.txtLocation.X = this.initialDefinition.Location.X;
            this.txtLocation.Y = this.initialDefinition.Location.Y;
            this.udRadius.Value = this.initialDefinition.Radius;

            // Only add the event handlers after the initial propererties have been set.
            this.txtLocation.ValueChanged += this.txtLocation_ValueChanged;
            this.udRadius.ValueChanged += this.udRadius_ValueChanged;
        }

        /// <summary>
        /// Handles the change of the location, sets the new location and invokes the changed event.
        /// </summary>
        private void txtLocation_ValueChanged(VectorTextBox sender, TPoint newLocation)
        {
            var diff = newLocation - this.currentDefinition.Location;
            this.currentDefinition = (MouseSpeedIndicatorDefinition)this.currentDefinition.Translate(diff.Width, diff.Height);
            this.DefinitionChanged?.Invoke(this.currentDefinition);
        }

        /// <summary>
        /// Handles the change of the radius, sets the new radius and invokes the changed event.
        /// </summary>
        private void udRadius_ValueChanged(object sender, EventArgs e)
        {
            this.currentDefinition = this.currentDefinition.SetRadius((int)this.udRadius.Value);
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
    }
}
