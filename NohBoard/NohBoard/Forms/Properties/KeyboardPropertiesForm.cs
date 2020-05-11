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

using System.Drawing;
using ThoNohT.NohBoard.Keyboard;

namespace ThoNohT.NohBoard.Forms.Properties
{
    using System;
    using System.Windows.Forms;
    using ThoNohT.NohBoard.Controls;
    using ThoNohT.NohBoard.Extra;

    /// <summary>
    /// The form used to update the properties of a keyboard.
    /// </summary>
    public partial class KeyboardPropertiesForm : Form
    {
        #region Fields

        /// <summary>
        /// A backup definition to return to if the user pressed cancel.
        /// </summary>
        private readonly KeyboardDefinition initialDefinition;

        /// <summary>
        /// The currently loaded definition.
        /// </summary>
        private KeyboardDefinition currentDefinition;

        #endregion Fields

        #region Events

        /// <summary>
        /// The event that is invoked when the definition has been changed. Only invoked when the definition is changed
        /// through the user interface, not when it is changed programmatically.
        /// </summary>
        public event Action<KeyboardDefinition> DefinitionChanged;

        /// <summary>
        /// The event that is invoked when the definition is saved.
        /// </summary>
        public event Action DefinitionSaved;

        #endregion Events

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardPropertiesForm" /> class.
        /// </summary>
        public KeyboardPropertiesForm(KeyboardDefinition initialDefinition)
        {
            this.initialDefinition = initialDefinition ?? throw new ArgumentNullException(nameof(initialDefinition));
            this.currentDefinition = this.initialDefinition.Clone();
            this.InitializeComponent();
        }

        /// <summary>
        /// Loads the form, setting the controls to the initial style.
        /// </summary>
        private void MouseSpeedPropertiesForm_Load(object sender, EventArgs e)
        {
            // Mouse speed indicator
            this.txtSize.X = this.initialDefinition.Width;
            this.txtSize.Y = this.initialDefinition.Height;

            // Only add the event handlers after the initial propererties have been set.
            this.txtSize.ValueChanged += this.txtSize_ValueChanged;
        }

        /// <summary>
        /// Handles the change of the location, sets the new location and invokes the changed event.
        /// </summary>
        private void txtSize_ValueChanged(VectorTextBox sender, TPoint newSize)
        {
            this.currentDefinition = this.currentDefinition.Resize(new Size(newSize.X, newSize.Y));

            // Only allow reasonable values to be set.
            if (newSize.X >= 25 && newSize.Y >= 25 && newSize.X <= 4096 && newSize.Y <= 4096)
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
