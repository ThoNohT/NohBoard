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
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using Extra;
    using Keyboard;
    using Keyboard.ElementDefinitions;

    public partial class MainForm
    {
        /// <summary>
        /// The keyboard element that is currently being manipulated.
        /// </summary>
        private ElementDefinition currentlyManipulating = null;

        /// <summary>
        /// The point inside <see cref="currentlyManipulating"/> that is being manipulated. This determines whether the
        /// manipulation is a move, an edge drag, or a point drag.
        /// </summary>
        private Point? currentManipulationPoint = null;

        /// <summary>
        /// A stack containing the previous edits made by the user.
        /// </summary>
        private readonly Stack<KeyboardDefinition> EditHistory = new Stack<KeyboardDefinition>();

        /// <summary>
        /// Turns edit-mode on or off.
        /// </summary>
        private void mnuToggleEditMode_Click(object sender, EventArgs e)
        {
            this.mnuToggleEditMode.Text = this.mnuToggleEditMode.Checked ? "Stop Editing" : "Start Editing";
        }

        /// <summary>
        /// Handles the MouseDown event for the main form, which can start editing an element, the mouse is pointing
        /// at one.
        /// </summary>
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (!this.mnuToggleEditMode.Checked) return;

            this.currentlyManipulating = GlobalSettings.CurrentDefinition.Elements
                .LastOrDefault(x => x.StartManipulating(e.Location));
            if (this.currentlyManipulating == null) return;

            this.currentManipulationPoint = e.Location;
            this.EditHistory.Push(GlobalSettings.CurrentDefinition);
            GlobalSettings.CurrentDefinition = GlobalSettings.CurrentDefinition
                .RemoveElement(this.currentlyManipulating);

            this.ResetBackBrushes();
        }

        /// <summary>
        /// Handles the MouseMove event for the main form, which performs all transformations that need to be done
        /// when editing an element.
        /// </summary>
        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (!this.mnuToggleEditMode.Checked || this.currentlyManipulating == null) return;

            var diff = (TPoint)e.Location - this.currentManipulationPoint;

            this.currentlyManipulating = this.currentlyManipulating.Manipulate(diff);

            this.currentManipulationPoint = e.Location;
        }

        /// <summary>
        /// Handles the MouseUp event for the main form, which will stop editing an element.
        /// </summary>
        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (!this.mnuToggleEditMode.Checked || this.currentlyManipulating == null) return;

            // TODO: Insert the element back at its previous location. Add move forward/backward/to front/to back functionality.
            GlobalSettings.CurrentDefinition = GlobalSettings.CurrentDefinition.AddElement(this.currentlyManipulating);

            this.currentlyManipulating = null;
            this.currentManipulationPoint = null;
            this.ResetBackBrushes();
        }
    }
}