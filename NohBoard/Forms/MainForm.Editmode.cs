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
    using Extra;
    using Hooking;
    using Keyboard;
    using Keyboard.ElementDefinitions;
    using Keyboard.Styles;
    using Properties;
    using Style;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    /// <summary>
    /// Edit mode part of the main form.
    /// </summary>
    public partial class MainForm
    {
        #region Manipulations

        /// <summary>
        /// The keyboard element that is currently being manipulated.
        /// </summary>
        private (int id, ElementDefinition definition)? currentlyManipulating = null;

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

        /// <summary>
        /// The position to translate every element in the keyboard from. <c>null</c> if not moving everything.
        /// </summary>
        private TPoint movingEverythingFrom;

        /// <summary>
        /// The starting point form which everything was being moved.
        /// </summary>
        private KeyboardDefinition movingEverythingStart;

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
        /// The element that was last copied.
        /// </summary>
        private ElementDefinition clipboard;

        /// <summary>
        /// Whether the main menu is open. This variable is set to true when the main menu is opened. The next
        /// keyboard/mouse hook events will only arrive after the menu is closed again. This causes strange jumps
        /// in the selected element, the element under the cursor will be selected if none was before.
        /// This variable is set to false when the left button goes up, so the first mouse down / move and up are ignored.
        /// </summary>
        private bool menuOpen;

        /// <summary>
        /// Turns edit-mode on or off.
        /// </summary>
        private void mnuToggleEditMode_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

            this.mnuToggleEditMode.Text = this.mnuToggleEditMode.Checked ? "Stop Editing" : "Start Editing";
            this.FormBorderStyle =
                this.mnuToggleEditMode.Checked ? FormBorderStyle.Sizable : FormBorderStyle.FixedSingle;

            if (!this.mnuToggleEditMode.Checked)
            {
                this.currentlyManipulating = null;
                this.highlightedDefinition = null;
                this.selectedDefinition = null;
            }
        }

        /// <summary>
        /// Toggles updating the text position of an element when a boundary or edge is updated.
        /// </summary>
        private void mnuUpdateTextPosition_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

            GlobalSettings.Settings.UpdateTextPosition = this.mnuUpdateTextPosition.Checked;
            GlobalSettings.Save();
        }

        #region Mouse manipulations

        /// <summary>
        /// Handles the MouseDown event for the main form, which can start editing an element, the mouse is pointing
        /// at one.
        /// </summary>
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (!this.mnuToggleEditMode.Checked || this.menuOpen) return;

            ElementDefinition toManipulate;
            if (this.selectedDefinition != null)
            {
                // Try to manipulate the selected definition, if one is selected.
                this.selectedDefinition.StartManipulating(e.Location, KeyboardState.AltDown, translateOnly: KeyboardState.CtrlDown);
                toManipulate = this.selectedDefinition;
            }
            else
            {
                // If none is selected, allow any key to become the element to manipulate.
                toManipulate = GlobalSettings.CurrentDefinition.Elements
                    .LastOrDefault(x => x.StartManipulating(e.Location, KeyboardState.AltDown, translateOnly: KeyboardState.CtrlDown));
            }

            if (toManipulate == null)
            {
                this.currentlyManipulating = null;
                this.selectedDefinition = null;

                // Moving everything at once.
                if (KeyboardState.AltDown)
                {
                    this.movingEverythingStart = GlobalSettings.CurrentDefinition.Clone();
                    this.movingEverythingFrom = e.Location;
                }

                return;
            }

            var indexToManipulate = GlobalSettings.CurrentDefinition.Elements.IndexOf(toManipulate);
            this.currentlyManipulating = (indexToManipulate, toManipulate);
            this.highlightedDefinition = null;
            this.manipulationStart = toManipulate;
            this.cumulManipulation = new Size();

            // Don't save history, we're in the middle of a transformation.
            GlobalSettings.Settings.UpdateDefinition(GlobalSettings.CurrentDefinition.RemoveElement(toManipulate), false);

            this.ResetBackBrushes();
        }

        /// <summary>
        /// Handles the MouseMove event for the main form, which performs all transformations that need to be done
        /// when editing an element.
        /// </summary>
        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (!this.mnuToggleEditMode.Checked || this.menuOpen) return;

            // Moving everything at once.
            if (this.movingEverythingFrom != null)
            {
                var newDef = this.movingEverythingStart.Clone();
                newDef.Elements.Clear();
                foreach (var element in this.movingEverythingStart.Elements)
                {
                    var diff = e.Location - this.movingEverythingFrom;
                    newDef.Elements.Add(element.Translate(diff.Width, diff.Height));
                }

                // Don't push history, we're in the middle of a transformation.
                GlobalSettings.Settings.UpdateDefinition(newDef, false);
                this.ResetBackBrushes();
            }

            if (this.currentlyManipulating != null)
            {
                var diff = (TPoint)e.Location - this.currentManipulationPoint;
                this.cumulManipulation += diff;

                this.currentlyManipulating = (this.currentlyManipulating.Value.id, this.manipulationStart.Manipulate(this.cumulManipulation));
                this.currentManipulationPoint = e.Location;
            }
            else
            {
                this.currentManipulationPoint = e.Location;

                // If a definition is selected, don't highlight others.
                if (this.selectedDefinition != null)
                {
                    // Preview the manipulation.
                    this.selectedDefinition.StartManipulating(e.Location, KeyboardState.AltDown, true, KeyboardState.CtrlDown);
                }
                else
                {
                    this.highlightedDefinition = GlobalSettings.CurrentDefinition.Elements
                        .LastOrDefault(x => x.StartManipulating(e.Location, KeyboardState.AltDown, translateOnly: KeyboardState.CtrlDown));
                }
            }
        }

        /// <summary>
        /// Handles the MouseUp event for the main form, which will stop editing an element.
        /// </summary>
        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            // Always reset this value when releasing the mouse.
            if (this.movingEverythingFrom != null)
            {
                // Store the current state in undo history.
                GlobalSettings.Settings.UpdateDefinition(GlobalSettings.CurrentDefinition, true);
                this.movingEverythingFrom = null;
                this.ResetBackBrushes();
            }

            if (e.Button != MouseButtons.Left) return;
            this.menuOpen = false;

            if (!this.mnuToggleEditMode.Checked || this.currentlyManipulating == null) return;

            GlobalSettings.Settings.UpdateDefinition(
                GlobalSettings.CurrentDefinition.AddElement(
                    this.currentlyManipulating.Value.definition,
                    this.currentlyManipulating.Value.id),
                true);

            if (this.cumulManipulation.Length() == 0 && this.selectedDefinition != null)
            {
                var elementsUnderCursor = GlobalSettings.CurrentDefinition.Elements
                  .Where(x => x.StartManipulating(e.Location, KeyboardState.AltDown, translateOnly: KeyboardState.CtrlDown))
                  .Reverse();

                var nextelementUnderCursor = elementsUnderCursor
                    .SkipWhile(el => el.Id != this.currentlyManipulating.Value.definition.Id).Skip(1).FirstOrDefault()
                    ?? elementsUnderCursor.FirstOrDefault();
                this.selectedDefinition = nextelementUnderCursor;
            }
            else
            {
                // Whatever was being manipulated (or not yet, but at least pressed down on) will now be selected.
                this.selectedDefinition = this.currentlyManipulating.Value.definition;
            }
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
            this.menuOpen = false;

            GlobalSettings.Settings.UpdateDefinition(
                GlobalSettings.CurrentDefinition.MoveElementDown(this.relevantDefinition, int.MaxValue), true);
            this.ResetBackBrushes();
        }

        /// <summary>
        /// Moves the currently highlighted element up in the z-order.
        /// </summary>
        private void mnuMoveUp_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

            GlobalSettings.Settings.UpdateDefinition(
                GlobalSettings.CurrentDefinition.MoveElementDown(this.relevantDefinition, 1), true);
            this.ResetBackBrushes();
        }

        /// <summary>
        /// Moves the currently highlighted element down in the z-order.
        /// </summary>
        private void mnuMoveDown_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

            GlobalSettings.Settings.UpdateDefinition(
                GlobalSettings.CurrentDefinition.MoveElementDown(this.relevantDefinition, -1), true);
            this.ResetBackBrushes();
        }

        /// <summary>
        /// Moves the currently highlighted element to the bottom of the z-order. Placing it below every other element.
        /// </summary>
        private void mnuMoveToBottom_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

            GlobalSettings.Settings.UpdateDefinition(
                GlobalSettings.CurrentDefinition.MoveElementDown(this.relevantDefinition, -int.MaxValue), true);
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

                GlobalSettings.Settings.UpdateDefinition(
                    GlobalSettings.CurrentDefinition
                        .RemoveElement(this.selectedDefinition).AddElement(newDefinition, index),
                    true);

                this.selectedDefinition = newDefinition;
                this.ResetBackBrushes();
                this.Refresh();
                return base.ProcessCmdKey(ref msg, keyData);
            }


            // Cancelling selection.
            if (keyCode == Keys.Escape || keyCode == Keys.Enter)
            {
                this.selectedDefinition = null;
                return base.ProcessCmdKey(ref msg, keyData);
            }

            // Undo-redo
            if ((keyData & Keys.Control) != 0 && keyCode == Keys.Z)
            {
                if ((keyData & Keys.Shift) == 0)
                {
                    if (GlobalSettings.Settings.Undo())
                    {
                        this.ClientSize = new Size(
                          GlobalSettings.CurrentDefinition.Width,
                          GlobalSettings.CurrentDefinition.Height);
                    }
                    else
                    {
                        return base.ProcessCmdKey(ref msg, keyData);
                    }
                }
                else
                {
                    if (GlobalSettings.Settings.Redo())
                    {
                        this.ClientSize = new Size(
                          GlobalSettings.CurrentDefinition.Width,
                          GlobalSettings.CurrentDefinition.Height);
                    }
                    else
                    {
                        return base.ProcessCmdKey(ref msg, keyData);
                    }
                }

                this.selectedDefinition = null;
                this.highlightedDefinition = null;
                this.ResetBackBrushes();
                this.Refresh();
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion Keyboard input handling

        #region Element management

        /// <summary>
        /// Handles setting the menu open variable to false when esc is pressed.
        /// </summary>
        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            // Esc closes the menu too.
            if (e.KeyCode == Keys.Escape)
                this.menuOpen = false;

            // Copy the selected element.
            if (e.KeyCode == Keys.C && e.Control && this.mnuToggleEditMode.Checked)
            {
                if (this.selectedDefinition == null) return;
                this.clipboard = this.selectedDefinition;
            }

            // Paste whatever element is in the clipboard, at the cursor position.
            if (e.KeyCode == Keys.V && e.Control && this.mnuToggleEditMode.Checked)
            {
                if (this.clipboard == null) return;

                var newPos = this.PointToClient(MousePosition);
                var validArea = new Rectangle(0, 0, GlobalSettings.CurrentDefinition.Width,
                        GlobalSettings.CurrentDefinition.Height);
                if (!validArea.Contains(newPos)) return;

                var oldPos = this.clipboard.GetBoundingBox().GetCenter();
                var dist = newPos - oldPos;

                // Copy the element with a new id, this also clones the element to facilitate multiple pastes.
                var elementToAdd = this.clipboard
                    .SetId(GlobalSettings.CurrentDefinition.GetNextId())
                    .Translate(dist.Width, dist.Height);

                var newDefinition = GlobalSettings.CurrentDefinition.AddElement(elementToAdd);

                // Set the style if the original one had a custom style.
                if (GlobalSettings.CurrentStyle.ElementIsStyled(this.clipboard.Id))
                {
                    var newStyle = GlobalSettings.CurrentStyle
                        .SetElementStyle(elementToAdd.Id, GlobalSettings.CurrentStyle.ElementStyles[this.clipboard.Id]);
                    GlobalSettings.Settings.UpdateBoth(newDefinition, newStyle, true);
                }
                else
                {
                    GlobalSettings.Settings.UpdateDefinition(newDefinition, true);
                }

                this.selectedDefinition = elementToAdd;

                this.ResetBackBrushes();
            }
        }

        /// <summary>
        /// Adds a new element definition to the current keyboard.
        /// </summary>
        /// <param name="definition">The definition to add.</param>
        private void AddElement(ElementDefinition definition)
        {
            if (!this.mnuToggleEditMode.Checked) return;
            if (this.highlightedDefinition != null) return;

            GlobalSettings.Settings.UpdateDefinition(GlobalSettings.CurrentDefinition.AddElement(definition), true);

            this.ResetBackBrushes();
        }

        /// <summary>
        /// Creates a new keyboard key definition and adds it to the keyboard definition.
        /// </summary>
        private void mnuAddKeyboardKeyDefinition_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

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
            this.menuOpen = false;

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
            this.menuOpen = false;

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
            this.menuOpen = false;

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
            this.menuOpen = false;

            if (!this.mnuToggleEditMode.Checked) return;
            if (this.highlightedDefinition == null && this.selectedDefinition == null) return;

            var definitionToRemove = this.highlightedDefinition ?? this.selectedDefinition;
            var newDefinition = GlobalSettings.CurrentDefinition.RemoveElement(definitionToRemove);

            // Remove the style if the element had a style.
            if (GlobalSettings.CurrentStyle.ElementIsStyled(definitionToRemove.Id))
            {
                var newStyle = GlobalSettings.CurrentStyle.RemoveElementStyle(definitionToRemove.Id);
                GlobalSettings.Settings.UpdateBoth(newDefinition, newStyle, true);
            }
            else
            {
                GlobalSettings.Settings.UpdateDefinition(newDefinition, true);
            }

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
            this.menuOpen = false;

            if (!this.mnuToggleEditMode.Checked) return;
            if (!(this.relevantDefinition is KeyDefinition)) return;

            var def = (KeyDefinition)this.relevantDefinition;
            var index = GlobalSettings.CurrentDefinition.Elements.IndexOf(def);
            var newDef = def.AddBoundary(this.currentManipulationPoint);
            GlobalSettings.Settings.UpdateDefinition(
                GlobalSettings.CurrentDefinition.RemoveElement(def).AddElement(newDef, index), true);

            // If we had a definition selected, the new one should now be selected.
            if (this.selectedDefinition != null) this.selectedDefinition = newDef;

            this.ResetBackBrushes();
        }

        /// <summary>
        /// Removes a boundary point from the highighted element.
        /// </summary>
        private void mnuRemoveBoundaryPoint_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

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

            var index = GlobalSettings.CurrentDefinition.Elements.IndexOf(def);
            var newDef = def.RemoveBoundary();
            GlobalSettings.Settings.UpdateDefinition(
                GlobalSettings.CurrentDefinition.RemoveElement(def).AddElement(newDef, index), true);

            // If we had a definition selected, the new one should now be selected.
            if (this.selectedDefinition != null) this.selectedDefinition = newDef;

            this.ResetBackBrushes();
        }

        #endregion Boundary management

        #region Properties

        /// <summary>
        /// Opens the element properties window for the element under the cursor.
        /// </summary>
        private void mnuElementProperties_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

            // Sanity check, don't try anything if there's no selected element.
            var relevantElement = this.selectedDefinition ?? this.elementUnderCursor;
            if (relevantElement == null) return;

            // Local function to handle definition changed.
            void OnDefinitionChanged(ElementDefinition def)
            {
                this.elementUnderCursor = null;
                this.currentlyManipulating = null;
                if (def.Equals(this.selectedDefinition))
                {
                    this.selectedDefinition = def;
                }

                var index = GlobalSettings.CurrentDefinition.Elements.IndexOf(def);
                GlobalSettings.Settings.UpdateDefinition(
                    GlobalSettings.CurrentDefinition.RemoveElement(def).AddElement(def, index), false);

                this.ResetBackBrushes();
            }

            // Local function to handle saving of the changed definition.
            void OnDefinitionSaved()
            {
                GlobalSettings.Settings.UpdateDefinition(GlobalSettings.CurrentDefinition, true);
            }

            if (relevantElement is MouseKeyDefinition || relevantElement is MouseScrollDefinition)
            {
                using (var propertiesForm = new MouseElementPropertiesForm((KeyDefinition) relevantElement))
                {
                    propertiesForm.DefinitionChanged += OnDefinitionChanged;
                    propertiesForm.DefinitionSaved += OnDefinitionSaved;

                    propertiesForm.ShowDialog(this);
                    return;
                }
            }

            if (relevantElement is MouseSpeedIndicatorDefinition mouseSpeedElement)
            {
                using (var propertiesForm = new MouseSpeedPropertiesForm(mouseSpeedElement))
                {
                    propertiesForm.DefinitionChanged += OnDefinitionChanged;
                    propertiesForm.DefinitionSaved += OnDefinitionSaved;

                    propertiesForm.ShowDialog(this);
                    return;
                }
            }

            if (relevantElement is KeyboardKeyDefinition keyboardKeyElement)
            {
                using (var propertiesForm = new KeyboardKeyPropertiesForm(keyboardKeyElement))
                {
                    propertiesForm.DefinitionChanged += OnDefinitionChanged;
                    propertiesForm.DefinitionSaved += OnDefinitionSaved;

                    propertiesForm.ShowDialog(this);
                    return;
                }
            }

            throw new Exception("Unknown element, cannot open properties form.");
        }

        /// <summary>
        /// Opens the keyboard properties form.
        /// </summary>
        private void mnuKeyboardProperties_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

            using (var propertiesForm = new KeyboardPropertiesForm(GlobalSettings.CurrentDefinition))
            {
                propertiesForm.DefinitionChanged += def =>
                {
                    this.elementUnderCursor = null;
                    this.currentlyManipulating = null;

                    GlobalSettings.Settings.UpdateDefinition(def, false);

                    this.ClientSize = new Size(def.Width, def.Height);

                    this.ResetBackBrushes();
                };

                propertiesForm.DefinitionSaved += () =>
                {
                    GlobalSettings.Settings.UpdateDefinition(GlobalSettings.CurrentDefinition, true);
                };

                propertiesForm.ShowDialog(this);
            }
        }

        /// <summary>
        /// Handles resizing the form. This changes the size of the keyboard.
        /// </summary>
        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            if (!this.mnuToggleEditMode.Checked) return;

            GlobalSettings.Settings.UpdateDefinition(GlobalSettings.CurrentDefinition.Resize(this.ClientSize), true);

            this.ResetBackBrushes();
        }

        #endregion Properties

        #region Styles

        /// <summary>
        /// Opens the edit element style form for the element currently under the cursor.
        /// </summary>
        private void mnuEditElementStyle_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

            if (GlobalSettings.Settings.LoadedStyle == null)
            {
                MessageBox.Show("Please load or save a style before editing element styles.");
                return;
            }

            // Sanity check, don't try anything if there's no selected element.
            var relevantElement = this.selectedDefinition ?? this.elementUnderCursor;
            if (relevantElement == null) return;
            var id = relevantElement.Id;

            if (relevantElement is KeyDefinition)
            {
                using (var styleForm = new KeyStyleForm(
                    GlobalSettings.CurrentStyle.TryGetElementStyle<KeyStyle>(id),
                    GlobalSettings.CurrentStyle.DefaultKeyStyle))
                {
                    styleForm.StyleChanged += style =>
                    {
                        if (style.Loose == null && style.Pressed == null)
                        {
                            if (GlobalSettings.CurrentStyle.ElementIsStyled(id))
                            {
                                // Remove existing style.
                                GlobalSettings.Settings
                                    .UpdateStyle(GlobalSettings.CurrentStyle.RemoveElementStyle(id), false);
                            }
                        }
                        else
                        {
                            // Set style.
                            GlobalSettings.Settings
                                .UpdateStyle(GlobalSettings.CurrentStyle.SetElementStyle(id, style), false);
                        }

                        this.ResetBackBrushes();
                    };

                    styleForm.StyleSaved += () =>
                    {
                        GlobalSettings.Settings.UpdateStyle(GlobalSettings.CurrentStyle, true);
                    };

                    styleForm.ShowDialog(this);
                }
            }

            if (relevantElement is MouseSpeedIndicatorDefinition)
            {
                using (var styleForm = new MouseSpeedStyleForm(
                    GlobalSettings.CurrentStyle.TryGetElementStyle<MouseSpeedIndicatorStyle>(id),
                    GlobalSettings.CurrentStyle.DefaultMouseSpeedIndicatorStyle))
                {
                    styleForm.StyleChanged += style =>
                    {
                        if (style == null)
                        {
                            if (GlobalSettings.CurrentStyle.ElementIsStyled(id))
                            {
                                // Remove existing style.
                                GlobalSettings.Settings
                                    .UpdateStyle(GlobalSettings.CurrentStyle.RemoveElementStyle(id), false);
                            }
                        }
                        else
                        {
                            // Set style.
                            GlobalSettings.Settings
                                .UpdateStyle(GlobalSettings.CurrentStyle.SetElementStyle(id, style), false);
                        }

                        this.ResetBackBrushes();
                    };

                    styleForm.StyleSaved += () =>
                    {
                        GlobalSettings.Settings.UpdateStyle(GlobalSettings.CurrentStyle, true);
                    };

                    styleForm.ShowDialog(this);
                }
            }
        }

        /// <summary>
        /// Opens the edit keyboard style form.
        /// </summary>
        private void mnuEditKeyboardStyle_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

            if (GlobalSettings.Settings.LoadedStyle == null)
            {
                MessageBox.Show("Please load or save a style before editing the keyboard style.");
                return;
            }

            using (var styleForm = new KeyboardStyleForm(GlobalSettings.CurrentStyle))
            {
                styleForm.StyleChanged += style =>
                {
                    GlobalSettings.Settings.UpdateStyle(style, false);
                    this.ResetBackBrushes();
                };

                styleForm.StyleSaved += () => {
                    GlobalSettings.Settings.UpdateStyle(GlobalSettings.CurrentStyle, true);
                };
                styleForm.ShowDialog(this);
            }
        }

        /// <summary>
        /// Saves the current style to its default name.
        /// </summary>
        private void mnuSaveStyleToName_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

            GlobalSettings.CurrentStyle.Save(false);
            GlobalSettings.Settings.LoadedStyle = GlobalSettings.CurrentStyle.Name;
            GlobalSettings.Settings.LoadedGlobalStyle = false;
        }

        /// <summary>
        /// Saves the current style as a global style to its default name.
        /// </summary>
        private void mnuSaveToGlobalStyleName_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

            GlobalSettings.CurrentStyle.Save(true);
            GlobalSettings.Settings.LoadedStyle = GlobalSettings.CurrentStyle.Name;
            GlobalSettings.Settings.LoadedGlobalStyle = true;
        }

        /// <summary>
        /// Opens the save style as form to save the style under a custom name.
        /// </summary>
        private void mnuSaveStyleAs_Click(object sender, EventArgs e)
        {
            using (var saveForm = new SaveStyleAsForm())
            {
                saveForm.ShowDialog(this);
                saveForm.Dispose();
            }
        }

        #endregion Styles
    }
}