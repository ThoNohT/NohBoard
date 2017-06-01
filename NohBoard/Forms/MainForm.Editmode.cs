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
    using Hooking;
    using Keyboard;
    using Keyboard.ElementDefinitions;
    using Properties;

    /// <summary>
    /// Edit mode part of the main form.
    /// </summary>
    public partial class MainForm
    {
        #region Manipulations

        /// <summary>
        /// The keyboard element that is currently being manipulated.
        /// </summary>
        private Tuple<int, ElementDefinition> currentlyManipulating = null;

        /// <summary>
        /// The currently manipulated element, at the point where the manipulation started.
        /// </summary>
        private ElementDefinition manipulationStart = null;

        /// <summary>
        /// The cumulative distance the current element has been manipulated.
        /// </summary>
        private Size cumulManipulation;

        /// <summary>
        /// The point inside <see cref="currentlyManipulating"/> that is being manipulated. This determines the type of
        /// manipulation that will be performed on the currently manipulating element.
        /// </summary>
        private Point currentManipulationPoint; 

        #endregion Manipulations

        /// <summary>
        /// The element that is currently highlighted by the mouse, but not being manipulated yet.
        /// </summary>
        private ElementDefinition highlightedDefinition = null;

        /// <summary>
        /// The element that is currently selected and can be modified using the keyboard.
        /// </summary>
        private ElementDefinition selectedDefinition = null;

        /// <summary>
        /// The element that is currently most relevant for manipulation. If a definition is selected, that is always
        /// the most relevant, otherwise, a highlighted definition can be manipulated.
        /// </summary>
        private ElementDefinition relevantDefinition => this.selectedDefinition ?? this.highlightedDefinition;

        /// <summary>
        /// A stack containing the previous edits made by the user.
        /// </summary>
        private readonly Stack<KeyboardDefinition> undoHistory = new Stack<KeyboardDefinition>();

        /// <summary>
        /// A stack containing the recently undone edits.
        /// </summary>
        private readonly Stack<KeyboardDefinition> redoHistory = new Stack<KeyboardDefinition>();

        /// <summary>
        /// Turns edit-mode on or off.
        /// </summary>
        private void mnuToggleEditMode_Click(object sender, EventArgs e)
        {
            this.mnuToggleEditMode.Text = this.mnuToggleEditMode.Checked ? "Stop Editing" : "Start Editing";

            if (!this.mnuToggleEditMode.Checked)
            {
                this.currentlyManipulating = null;
                this.highlightedDefinition = null;
                this.selectedDefinition = null;
            }
        }

        #region Mouse manipulations

        /// <summary>
        /// Handles the MouseDown event for the main form, which can start editing an element, the mouse is pointing
        /// at one.
        /// </summary>
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (!this.mnuToggleEditMode.Checked) return;

            ElementDefinition toManipulate;
            if (this.selectedDefinition != null)
            {
                // Try to manipulate the selected definition, if one is selected.
                this.selectedDefinition.StartManipulating(e.Location, KeyboardState.AltDown);
                toManipulate = this.selectedDefinition;
            }
            else
            {
                // If none is selected, allow any key to become the element to manipulate.
                toManipulate = GlobalSettings.CurrentDefinition.Elements
                    .LastOrDefault(x => x.StartManipulating(e.Location, KeyboardState.AltDown));
            }

            if (toManipulate == null)
            {
                this.currentlyManipulating = null;
                this.selectedDefinition = null;
                return;
            }

            var indexToManipulate = GlobalSettings.CurrentDefinition.Elements.IndexOf(toManipulate);
            this.currentlyManipulating = Tuple.Create(indexToManipulate, toManipulate);
            this.highlightedDefinition = null;
            this.manipulationStart = toManipulate;
            this.cumulManipulation = new Size();

            this.PushUndoHistory();
            GlobalSettings.CurrentDefinition = GlobalSettings.CurrentDefinition.RemoveElement(toManipulate);

            this.ResetBackBrushes();
        }

        /// <summary>
        /// Handles the MouseMove event for the main form, which performs all transformations that need to be done
        /// when editing an element.
        /// </summary>
        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (!this.mnuToggleEditMode.Checked) return;

            if (this.currentlyManipulating != null)
            {
                var diff = (TPoint)e.Location - this.currentManipulationPoint;
                this.cumulManipulation += diff;

                this.currentlyManipulating = Tuple.Create(this.currentlyManipulating.Item1, this.manipulationStart.Manipulate(this.cumulManipulation));
                this.currentManipulationPoint = e.Location;
            }
            else
            {
                this.currentManipulationPoint = e.Location;

                // If a definition is selected, don't highlight others.
                if (this.selectedDefinition != null)
                {
                    // Preview the manipulation.
                    this.selectedDefinition.StartManipulating(e.Location, KeyboardState.AltDown, true);
                }
                else
                {
                    this.highlightedDefinition = GlobalSettings.CurrentDefinition.Elements
                        .LastOrDefault(x => x.StartManipulating(e.Location, KeyboardState.AltDown));
                }
            }
        }

        /// <summary>
        /// Handles the MouseUp event for the main form, which will stop editing an element.
        /// </summary>
        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (!this.mnuToggleEditMode.Checked || this.currentlyManipulating == null) return;

            GlobalSettings.CurrentDefinition = GlobalSettings.CurrentDefinition.AddElement(
                this.currentlyManipulating.Item2,
                this.currentlyManipulating.Item1);

            // Whatever was being manipulated (or not yet, but at least pressed down on) will now be selected.
            this.selectedDefinition = this.currentlyManipulating?.Item2;

            this.currentlyManipulating = null;
            this.manipulationStart = null;
            this.currentManipulationPoint = new Point();
            this.ResetBackBrushes();
        }

        #endregion Mouse manipulations

        #region Element z-order moving

        /// <summary>
        /// Moves the currently highlighted element to the top of the z-order. Placing it above every other element.
        /// </summary>
        private void mnuMoveToTop_Click(object sender, EventArgs e)
        {
            GlobalSettings.CurrentDefinition = GlobalSettings.CurrentDefinition
                .MoveElementDown(this.relevantDefinition, int.MaxValue);
            this.ResetBackBrushes();
        }

        /// <summary>
        /// Moves the currently highlighted element up in the z-order.
        /// </summary>
        private void mnuMoveUp_Click(object sender, EventArgs e)
        {
            GlobalSettings.CurrentDefinition = GlobalSettings.CurrentDefinition
                .MoveElementDown(this.relevantDefinition, 1);
            this.ResetBackBrushes();
        }

        /// <summary>
        /// Moves the currently highlighted element down in the z-order.
        /// </summary>
        private void mnuMoveDown_Click(object sender, EventArgs e)
        {
            GlobalSettings.CurrentDefinition = GlobalSettings.CurrentDefinition
                .MoveElementDown(this.relevantDefinition, -1);
            this.ResetBackBrushes();
        }

        /// <summary>
        /// Moves the currently highlighted element to the bottom of the z-order. Placing it below every other element.
        /// </summary>
        private void mnuMoveToBottom_Click(object sender, EventArgs e)
        {
            GlobalSettings.CurrentDefinition = GlobalSettings.CurrentDefinition
                .MoveElementDown(this.relevantDefinition, -int.MaxValue);
            this.ResetBackBrushes();
        }

        #endregion Element z-order moving

        #region Keyboard input handling

        /// <summary>
        /// Handles the key press event. Allows undo/redo, selection cancellation and manipulations using the arrow keys.
        /// </summary>
        /// <param name="msg">A <see cref="Message"/>, passed by reference, that represents the window message to process.</param>
        /// <param name="keyData">One of the <see cref="Keys"/> values that represents the key to process.</param>
        /// <returns><c>true</c> if the character was processed by the control; otherwise, <c>false</c>.</returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!this.mnuToggleEditMode.Checked) return base.ProcessCmdKey(ref msg, keyData);

            var keyCode = keyData & Keys.KeyCode;

            // Manipulations by keyboard keys.
            if (this.selectedDefinition != null && new[] { Keys.Up, Keys.Right, Keys.Down, Keys.Left }.Contains(keyCode))
            {
                this.PushUndoHistory();
                ElementDefinition newDefinition;
                var index = GlobalSettings.CurrentDefinition.Elements.IndexOf(this.selectedDefinition);
                switch (keyCode)
                {
                    case Keys.Right:
                        newDefinition = this.selectedDefinition.Manipulate(new Size(1, 0));
                        break;
                    case Keys.Down:
                        newDefinition = this.selectedDefinition.Manipulate(new Size(0, 1));
                        break;
                    case Keys.Left:
                        newDefinition = this.selectedDefinition.Manipulate(new Size(-1, 0));
                        break;
                    case Keys.Up:
                        newDefinition = this.selectedDefinition.Manipulate(new Size(0, -1));
                        break;
                    default:
                        throw new Exception("If this happens, the if statement around this switch is incorrect.");
                }

                GlobalSettings.CurrentDefinition = GlobalSettings.CurrentDefinition
                    .RemoveElement(this.selectedDefinition).AddElement(newDefinition, index);

                this.selectedDefinition = newDefinition;
                this.ResetBackBrushes();
                this.Refresh();
                return base.ProcessCmdKey(ref msg, keyData);
            }


            // Cancelling selection.
            if (keyCode == Keys.Escape)
            {
                this.selectedDefinition = null;
                return base.ProcessCmdKey(ref msg, keyData);
            }

            // Undo-redo
            if ((keyData & Keys.Control) != 0 && keyCode == Keys.Z)
            {
                if ((keyData & Keys.Shift) == 0)
                {
                    if (!this.undoHistory.Any()) return base.ProcessCmdKey(ref msg, keyData);

                    this.redoHistory.Push(GlobalSettings.CurrentDefinition);
                    GlobalSettings.CurrentDefinition = this.undoHistory.Pop();
                }
                else
                {
                    if (!this.redoHistory.Any()) return base.ProcessCmdKey(ref msg, keyData);

                    this.undoHistory.Push(GlobalSettings.CurrentDefinition);
                    GlobalSettings.CurrentDefinition = this.redoHistory.Pop();
                }

                this.selectedDefinition = null;
                this.highlightedDefinition = null;
                this.ResetBackBrushes();
                this.Refresh();
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Pushes the current keyboard state into the undo history and clears the redo history.
        /// </summary>
        private void PushUndoHistory()
        {
            this.undoHistory.Push(GlobalSettings.CurrentDefinition);
            this.redoHistory.Clear();
        }

        #endregion Keyboard input handling

        #region Element management

        /// <summary>
        /// Adds a new element definition to the current keyboard.
        /// </summary>
        /// <param name="definition">The definition to add.</param>
        private void AddElement(ElementDefinition definition)
        {
            if (!this.mnuToggleEditMode.Checked) return;
            if (this.highlightedDefinition != null) return;

            this.PushUndoHistory();

            GlobalSettings.CurrentDefinition = GlobalSettings.CurrentDefinition
                .AddElement(definition);

            this.ResetBackBrushes();
        }

        /// <summary>
        /// Creates a new keyboard key definition and adds it to the keyboard definition.
        /// </summary>
        private void mnuAddKeyboardKeyDefinition_Click(object sender, EventArgs e)
        {
            var c = this.currentManipulationPoint;
            var w = Constants.DefaultElementSize / 2;
            var boundaries = new List<TPoint>
            {
                new TPoint(c.X - w, c.Y - w), // Top left
                new TPoint(c.X + w, c.Y - w), // Top right
                new TPoint(c.X + w, c.Y + w), // Bottom right
                new TPoint(c.X - w, c.Y + w), // Bottom left
            };

            this.AddElement(
                new KeyboardKeyDefinition(
                    GlobalSettings.CurrentDefinition.GetNextId(),
                    boundaries,
                    new List<int>(),
                    "",
                    "",
                    true));
        }

        /// <summary>
        /// Creates a new mouse key definition and adds it to the keyboard definition.
        /// </summary>
        private void mnuAddMouseKeyDefinition_Click(object sender, EventArgs e)
        {
            var c = this.currentManipulationPoint;
            var w = Constants.DefaultElementSize / 2;
            var boundaries = new List<TPoint>
            {
                new TPoint(c.X - w, c.Y - w), // Top left
                new TPoint(c.X + w, c.Y - w), // Top right
                new TPoint(c.X + w, c.Y + w), // Bottom right
                new TPoint(c.X - w, c.Y + w), // Bottom left
            };

            this.AddElement(
                new MouseKeyDefinition(
                    GlobalSettings.CurrentDefinition.GetNextId(),
                    boundaries,
                    (int) MouseKeyCode.LeftButton,
                    ""));
        }

        /// <summary>
        /// Creates a new mouse scroll definition and adds it to the keyboard definition.
        /// </summary>
        private void mnuAddMouseScrollDefinition_Click(object sender, EventArgs e)
        {
            var c = this.currentManipulationPoint;
            var w = Constants.DefaultElementSize / 2;
            var boundaries = new List<TPoint>
            {
                new TPoint(c.X - w, c.Y - w), // Top left
                new TPoint(c.X + w, c.Y - w), // Top right
                new TPoint(c.X + w, c.Y + w), // Bottom right
                new TPoint(c.X - w, c.Y + w), // Bottom left
            };

            this.AddElement(
                new MouseScrollDefinition(
                    GlobalSettings.CurrentDefinition.GetNextId(),
                    boundaries,
                    (int) MouseScrollKeyCode.ScrollDown,
                    ""));
        }

        /// <summary>
        /// Creates a new mouse speed indicator definition and adds it to the keyboard definition.
        /// </summary>
        private void mnuAddMouseSpeedIndicatorDefinition_Click(object sender, EventArgs e)
        {
            this.AddElement(
                new MouseSpeedIndicatorDefinition(
                    GlobalSettings.CurrentDefinition.GetNextId(),
                    this.currentManipulationPoint,
                    Constants.DefaultElementSize / 2));
        }

        /// <summary>
        /// Removes an element from the current keyboard.
        /// </summary>
        private void mnuRemoveElement_Click(object sender, EventArgs e)
        {
            if (!this.mnuToggleEditMode.Checked) return;
            if (this.highlightedDefinition == null && this.selectedDefinition == null) return;

            this.PushUndoHistory();

            GlobalSettings.CurrentDefinition = GlobalSettings.CurrentDefinition
                .RemoveElement(this.highlightedDefinition ?? this.selectedDefinition);

            // Unset the definition everywhere because it no longer exists.
            this.highlightedDefinition = null;
            this.currentlyManipulating = null;
            this.manipulationStart = null;
            this.selectedDefinition = null;

            this.ResetBackBrushes();
        }

        #endregion Element management

        #region Boundary management

        /// <summary>
        /// Adds a boundary point to the highlighted element.
        /// </summary>
        private void mnuAddBoundaryPoint_Click(object sender, EventArgs e)
        {
            if (!this.mnuToggleEditMode.Checked) return;
            if (!(this.relevantDefinition is KeyDefinition)) return;

            this.PushUndoHistory();

            var def = (KeyDefinition)this.relevantDefinition;
            var index = GlobalSettings.CurrentDefinition.Elements.IndexOf(def);
            var newDef = def.AddBoundary(this.currentManipulationPoint);
            GlobalSettings.CurrentDefinition = GlobalSettings.CurrentDefinition
                .RemoveElement(def).AddElement(newDef, index);

            // If we had a definition selected, the new one should now be selected.
            if (this.selectedDefinition != null) this.selectedDefinition = newDef;

            this.ResetBackBrushes();
        }

        /// <summary>
        /// Removes a boundary point from the highighted element.
        /// </summary>
        private void mnuRemoveBoundaryPoint_Click(object sender, EventArgs e)
        {
            if (!this.mnuToggleEditMode.Checked) return;
            if (!(this.relevantDefinition is KeyDefinition)) return;

            var def = (KeyDefinition)this.relevantDefinition;
            if (def.Boundaries.Count < 4)
            {
                MessageBox.Show(
                    "You cannot remove another boundary, there must be at least 3.",
                    "Error removing boundary",
                    MessageBoxButtons.OK);
                return;
            }

            this.PushUndoHistory();

            var index = GlobalSettings.CurrentDefinition.Elements.IndexOf(def);
            var newDef = def.RemoveBoundary();
            GlobalSettings.CurrentDefinition = GlobalSettings.CurrentDefinition
                .RemoveElement(def).AddElement(newDef, index);

            // If we had a definition selected, the new one should now be selected.
            if (this.selectedDefinition != null) this.selectedDefinition = newDef;

            this.ResetBackBrushes();
        }

        #endregion Boundary management

        #region Element properties

        /// <summary>
        /// Opens the element properties window for the element under the cursor.
        /// </summary>
        private void mnuElementProperties_Click(object sender, EventArgs e)
        {
            // Sanity check, don't try anything if there's no selected element.
            if (this.elementUnderCursor == null) return;

            if (this.elementUnderCursor is MouseKeyDefinition || this.elementUnderCursor is MouseScrollDefinition)
            {
                using (var propertiesForm = new MouseElementPropertiesForm((KeyDefinition) this.elementUnderCursor))
                {
                    propertiesForm.ShowDialog(this);
                    return;
                }
            }

            if (this.elementUnderCursor is MouseSpeedIndicatorDefinition mouseSpeedElement)
            {
                using (var propertiesForm = new MouseSpeedPropertiesForm(mouseSpeedElement))
                {
                    propertiesForm.DefinitionChanged += (def) =>
                    {
                        this.elementUnderCursor = null;
                        this.currentlyManipulating = null;
                        this.selectedDefinition = def;
                        GlobalSettings.CurrentDefinition.Elements[def.Id] = def;
                        this.ResetBackBrushes();
                    };

                    propertiesForm.ShowDialog(this);
                    return;
                }
            }

            if (this.elementUnderCursor is KeyboardKeyDefinition)
            {
                using (var propertiesForm = new KeyboardKeyPropertiesForm())
                {
                    propertiesForm.ShowDialog(this);
                    return;
                }
            }

            throw new Exception("Unknown element, cannot open properties form.");
        }

        #endregion Element properties
    }
}