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

namespace ThoNohT.NohBoard.Extra
{
    using System.Collections.Generic;
    using ThoNohT.NohBoard.Keyboard;

    /// <summary>
    /// Functionality related to tracking changes to the history of the definition and style.
    /// </summary>
    public partial class GlobalSettings
    {
        /// <summary>
        /// A stack containing the previous edits made by the user.
        /// </summary>
        private Stack<(KeyboardDefinition, KeyboardStyle, ChangeType)> undoHistory;

        /// <summary>
        /// A stack containing the recently undone edits.
        /// </summary>
        private Stack<(KeyboardDefinition, KeyboardStyle, ChangeType)> redoHistory;

        #region Helper properties

        /// <summary>
        /// Gets the last undo history. Unless currently in the process of saving a new change to the undo
        /// history, this is equal to the current definition and style.
        /// </summary>
        private (KeyboardDefinition, KeyboardStyle, ChangeType) LastUndo => this.undoHistory.Peek();

        /// <summary>
        /// The first  entry in the undo history is the initial loaded keyboard, it cannot be undone.
        /// Therefore, an undo can only be done, if there is at least one more entry in the history.
        /// </summary>
        private bool CanUndo => this.undoHistory.Count > 1;

        /// <summary>
        /// Redo is possible as long as there is an entry in the redo history. This is because the current
        /// version is not stored in the redo history.
        /// </summary>
        private bool CanRedo => this.redoHistory.Count > 0;

        #endregion Helper properties

        /// <summary>
        /// Updates the current definition.
        /// </summary>
        /// <param name="newDefinition">The new definition.</param>
        /// <param name="pushUndoHistory">Whether to add an entry to the undo history.</param>
        public void UpdateDefinition(KeyboardDefinition newDefinition, bool pushUndoHistory)
        {
            GlobalSettings.CurrentDefinition = newDefinition;
            if (pushUndoHistory) this.PushUndoHistory(ChangeType.Definition);
        }

        /// <summary>
        /// Updates the current style.
        /// </summary>
        /// <param name="newStyle">The new style.</param>
        /// <param name="pushUndoHistory">Whether to add an entry to the undo history.</param>
        public void UpdateStyle(KeyboardStyle newStyle, bool pushUndoHistory)
        {
            GlobalSettings.CurrentStyle = newStyle;
            if (pushUndoHistory) this.PushUndoHistory(ChangeType.Style);
        }

        /// <summary>
        /// Updates both the current definition and style.
        /// </summary>
        /// <param name="newDefinition">The new definition.</param>
        /// <param name="newStyle">The new style.</param>
        /// <param name="pushUndoHistory">Whether to add an entry to the undo history</param>
        public void UpdateBoth(KeyboardDefinition newDefinition, KeyboardStyle newStyle, bool pushUndoHistory)
        {
            GlobalSettings.CurrentDefinition = newDefinition;
            GlobalSettings.CurrentStyle = newStyle;
            if (pushUndoHistory) this.PushUndoHistory(ChangeType.Both);
        }

        /// <summary>
        /// Initializes the undo history to a single entry with the loaded keyboard.
        /// The redo history starts out empty.
        /// </summary>
        public void InitUndoHistory()
        {
            this.undoHistory = new Stack<(KeyboardDefinition, KeyboardStyle, ChangeType)>();
            this.redoHistory = new Stack<(KeyboardDefinition, KeyboardStyle, ChangeType)>();

            GlobalSettings.UnsavedDefinitionChanges = false;
            GlobalSettings.UnsavedStyleChanges = false;

            this.undoHistory.Push((GlobalSettings.CurrentDefinition, GlobalSettings.CurrentStyle.Clone(), ChangeType.None));
        }

        /// <summary>
        /// Pushes the current keyboard state into the undo history and clears the redo history.
        /// </summary>
        /// <param name="changeType">The change type. This only has effect on whether the definition or style will
        /// be marked as having unsaved changes.</param>
        private void PushUndoHistory(ChangeType changeType)
        {
            // There should always be an undo history, because it is initialized  with a single entry,
            // and never popped beyond this last entry.
            var (lastDef, lastStyle, _) = this.LastUndo;

            // If the previous entry is the same as the current, don't push anything. This can happen if a user clicks
            // on a key definition without moving it, or if they open a style/properties form and save without making
            // any changes.
            if (!GlobalSettings.CurrentDefinition.IsChanged(lastDef) &&
                !GlobalSettings.CurrentStyle.IsChanged(lastStyle))
            {
                return;
            }

            this.undoHistory.Push((GlobalSettings.CurrentDefinition, GlobalSettings.CurrentStyle.Clone(), changeType));

            // If something new is done since the last undo, redoing something else is no longer possible.
            this.redoHistory.Clear();

            if ((changeType & ChangeType.Definition) > 0) GlobalSettings.UnsavedDefinitionChanges = true;
            if ((changeType & ChangeType.Style) > 0) GlobalSettings.UnsavedStyleChanges = true;
        }

        /// <summary>
        /// Undoes the last change.
        /// </summary>
        /// <returns>A value whether undo was done. If there are no actions to undo, false is returned,
        /// otherwise true.</returns>
        public bool Undo()
        {
            // The first history entry is the initial layout, if this is the current one, then there is nothing to undo.
            if (!this.CanUndo) return false;

            // Move the last undo entry to the redo history.
            this.redoHistory.Push(this.undoHistory.Pop());

            var (defToRevertTo, styleToRevertTo, _) = this.LastUndo;

            GlobalSettings.CurrentDefinition = defToRevertTo;
            GlobalSettings.CurrentStyle = styleToRevertTo;

            // If there is no more undo history, then there are also no more pending changes.
            if (!this.CanUndo)
            {
                GlobalSettings.UnsavedDefinitionChanges = false;
                GlobalSettings.UnsavedStyleChanges = false;
            }

            return true;
        }

        /// <summary>
        /// Redoes the last change.
        /// </summary>
        /// <returns>A value whether redo was done. If there are no actions to redo, false is returned,
        /// otherwise true.</returns>
        public bool Redo()
        {
            if (!this.CanRedo) return false;

            var (defToRevertTo, styleToRevertTo, typeToRevert) = this.redoHistory.Pop();

            this.undoHistory.Push((defToRevertTo, styleToRevertTo, typeToRevert));

            GlobalSettings.CurrentDefinition = defToRevertTo;
            GlobalSettings.CurrentStyle = styleToRevertTo;

            // Redoing will always trigger unsaved changes of the type that is being redone.
            GlobalSettings.UnsavedDefinitionChanges |= (typeToRevert & ChangeType.Definition) != 0;
            GlobalSettings.UnsavedStyleChanges |= (typeToRevert & ChangeType.Style) != 0;

            return true;
        }
    }
}
