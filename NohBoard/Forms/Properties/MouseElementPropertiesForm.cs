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
    using Hooking;
    using Keyboard.ElementDefinitions;

    public partial class MouseElementPropertiesForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MouseElementPropertiesForm" /> class.
        /// </summary>
        /// <param name="definition">The definition to show the properties for.</param>
        public MouseElementPropertiesForm(KeyDefinition definition)
        {
            this.InitializeComponent();

            if (definition is KeyboardKeyDefinition)
                throw new Exception("A MouseElementePropertiesForm cannot be used for keyboard keys.");

            if (definition is MouseKeyDefinition)
            {
                this.Text = "Mouse Key Properties";

                this.cmbKeyCode.Items.Clear();
                this.cmbKeyCode.Items.Add(MouseKeyCode.LeftButton);
                this.cmbKeyCode.Items.Add(MouseKeyCode.MiddleButton);
                this.cmbKeyCode.Items.Add(MouseKeyCode.RightButton);
                this.cmbKeyCode.Items.Add(MouseKeyCode.X1Button);
                this.cmbKeyCode.Items.Add(MouseKeyCode.X2Button);
            }

            if (definition is MouseScrollDefinition)
            {
                this.Text = "Mouse Scroll Properties";

                this.cmbKeyCode.Items.Clear();
                this.cmbKeyCode.Items.Add(MouseScrollKeyCode.ScrollUp);
                this.cmbKeyCode.Items.Add(MouseScrollKeyCode.ScrollRight);
                this.cmbKeyCode.Items.Add(MouseScrollKeyCode.ScrollDown);
                this.cmbKeyCode.Items.Add(MouseScrollKeyCode.ScrollLeft);
            }
            
            // TODO: Prefill properties from definition.
        }
    }
}
