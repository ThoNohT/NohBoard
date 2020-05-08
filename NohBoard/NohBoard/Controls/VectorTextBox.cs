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

namespace ThoNohT.NohBoard.Controls
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using Extra;

    /// <summary>
    /// A text box that allows the user to type vectors.
    /// </summary>
    public class VectorTextBox : TextBox
    {
        #region Events

        /// <summary>
        /// The event that is invoked when the value has been changed. Only invoked when the font is changed through
        /// the user interface, not when it is changed programmatically.
        /// </summary>
        public event Action<VectorTextBox, TPoint> ValueChanged;

        #endregion Events

        #region Fields

        /// <summary>
        /// Private fields for the properties.
        /// </summary>
        private int maxVal = int.MaxValue;
        private int x;
        private int y;
        private bool spacesAroundSeparator = true;
        private char separator = ';';

        #endregion Fields

        #region Properties

        /// <summary>
        /// The separator character.
        /// </summary>
        public char Separator
        {
            get { return this.separator; }
            set
            {
                this.separator = value;
                this.UpdateText();
            }
        }

        /// <summary>
        /// Whether there should be a space around the separator character.
        /// </summary>
        public bool SpacesAroundSeparator
        {
            get { return this.spacesAroundSeparator; }
            set
            {
                this.spacesAroundSeparator = value;
                this.UpdateText();
            }
        }

        /// <summary>
        /// The X value of the vector.
        /// </summary>
        public int X
        {
            get { return this.x; }
            set
            {
                this.x = value;
                this.UpdateText();
            }
        }

        /// <summary>
        /// The Y value of the vector.
        /// </summary>
        public int Y
        {
            get { return this.y; }
            set
            {
                this.y = value;
                this.UpdateText();
            }
        }

        /// <summary>
        /// The maximum values for the X and Y components.
        /// </summary>
        public int MaxVal
        {
            get { return this.maxVal; }
            set
            {
                this.maxVal = value;
                if (this.X > this.maxVal) this.X = this.maxVal;
                if (this.Y > this.maxVal) this.Y = this.maxVal;
            }
        }

        /// <summary>
        /// A formatted separator.
        /// </summary>
        private string sep => this.SpacesAroundSeparator ? $" {this.Separator} " : this.Separator.ToString();

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorTextBox" /> class.
        /// </summary>
        public VectorTextBox()
        {
            this.KeyPress += this.IgnoreKey;
            this.KeyDown += this.HandleKeyDown;
            this.GotFocus += this.HandleFocus;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// When receiving focus. Make sure any 0 in front of which the current selection lies, is highlighted so we
        /// don't need to worry about typing numbers in front of a 0.
        /// </summary>
        private void HandleFocus(object sender, EventArgs e)
        {
            if (this.SelectionLength > 0) return;

            if (this.Text == string.Empty)
                this.Text = $"{this.x}{this.sep}{this.y}";

            var split = this.Text.Split(new[] { this.Separator, ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (this.x == 0 && this.SelectionStart == 0)
                this.SelectionLength = 1;

            if (this.y == 0 && this.SelectionStart == split[0].Length + this.sep.Length)
                this.SelectionLength = 1;
        }

        /// <summary>
        /// Handles the key pressed event. Allows only valid values to be typed.
        /// </summary>
        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            var key = e.KeyCode;
            if (key == Keys.Oem1) key = (Keys)';'; // Apparently pressing the semicolon results in a special keycode.
            var newChar = (char)key;
            var cursor = this.SelectionStart;
            var cursorLength = this.SelectionLength;
            var oldText = this.Text;
            var split = this.Text.Split(new[] { this.Separator, ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Ignore all whitelisted keys.
            var whitelistedKeys = new[] { Keys.Right, Keys.Left, Keys.Home, Keys.End };
            if (whitelistedKeys.Contains(key))
            {
                e.Handled = false;
                return;
            }

            if (key == Keys.Back)
            {
                // Backspace
                if (cursorLength == 0 && cursor == 0) return;
                var toDelete = cursorLength > 0
                    ? oldText.Substring(cursor, cursorLength) : oldText.Substring(cursor - 1, 1);

                // Don't try to delete a separator.
                var newMid = toDelete.Contains(this.separator.ToString()) ? this.sep : string.Empty;

                var newLeft = cursorLength > 0 || cursor == 0
                    ? oldText.Substring(0, cursor) : oldText.Substring(0, cursor - 1);
                var newRight = cursorLength > 0 || cursor == 0
                    ? oldText.Substring(cursor + cursorLength) : oldText.Substring(cursor);
                if (newLeft == string.Empty && newRight.IndexOf(this.separator) < 1) newLeft = "0";

                this.Text = $"{newLeft}{newMid}{newRight}";

                if (cursorLength == 0 && cursor > 0)
                    cursor--;
            }
            else if (key == Keys.Delete)
            {
                // Delete
                if (cursor == oldText.Length) return;
                var toDelete = cursorLength > 0
                    ? oldText.Substring(cursor, cursorLength) : oldText.Substring(cursor, 1);

                // Don't try to delete a separator.
                var newMid = toDelete.Contains(this.separator.ToString()) ? this.sep : string.Empty;

                var newLeft = oldText.Substring(0, cursor);
                var newRight = cursorLength > 0 || cursor == oldText.Length
                    ? oldText.Substring(cursor + cursorLength) : oldText.Substring(cursor + 1);
                if (newLeft == string.Empty && newRight.IndexOf(this.separator) < 1) newLeft = "0";

                if (cursorLength == 0 && cursor < oldText.Length && newMid != string.Empty)
                    cursor++;

                this.Text = $"{newLeft}{newMid}{newRight}";
            }
            else if (newChar.ToString().Equals(this.Separator.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                // Separator
                if (!oldText.Contains(this.Separator.ToString()))
                {
                    this.Text += this.Separator;
                }
                else
                {
                    // Move it behind the separator.
                    if (cursor == split[0].Length && cursor < split[0].Length + 1)
                        cursor = split[0].Length + this.sep.Length;
                    else if (this.SpacesAroundSeparator)
                        cursor = split[0].Length + this.sep.Length - 1;
                }
            }
            else if (newChar == ' ')
            {
                // Space
                if (!this.spacesAroundSeparator) return;

                if (cursor == split[0].Length || cursor == split[0].Length + this.sep.Length - 1)
                    cursor++;
            }
            else if (newChar >= 96 && newChar <= 107 || Regex.IsMatch(newChar.ToString(), @"\d"))
            {
                // Map numpad keys to regular numbers.
                if (newChar >= 96 && newChar <= 107) newChar = (char)(newChar - 48);

                // Regular numbers
                if (split.Any() && cursor > split[0].Length && cursor < split[0].Length + this.sep.Length)
                    return;

                // Number or separator
                this.Text = cursorLength > 0
                    ? $"{oldText.Substring(0, cursor)}{newChar}{oldText.Substring(cursor + cursorLength)}"
                    : $"{oldText.Substring(0, cursor)}{newChar}{oldText.Substring(cursor)}";

                cursor++;
            }
            else
            {
                // Any other key should be completely ignored.
                return;
            }

            // Format the new text
            if (this.Text == string.Empty)
            {
                this.Text = $"0{this.sep}0";
                this.SelectionStart = 0;
                this.SelectionLength = 1;
            }

            this.FormatText(cursor);
        }

        /// <summary>
        /// Formats the text inside the textbox and sets the cursor to the specified position.
        /// The cursor position and selection length are adapted according to the formatting changes that are made.
        /// </summary>
        /// <param name="cursor">The position to set the cursor to.</param>
        private void FormatText(int cursor)
        {
            var split = this.Text.Split(new[] { this.Separator, ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Determine and constrain the new values.
            var newX = long.Parse(split[0]);
            if (newX > this.maxVal) newX = this.maxVal;
            var newY = split.Length > 1 ? long.Parse(split[1]) : 0;
            if (newY > this.maxVal) newY = this.maxVal;

            // Determine where the cursor should go.
            var makeSelection = false;
            if (cursor <= split[0].Length)
            {
                // We're in the X part.
                var leadingZeroes = split[0].Length - newX.ToString().Length;
                cursor -= leadingZeroes;

                if (newX == 0)
                {
                    makeSelection = true;
                    cursor = Math.Max(0, cursor - 1);
                }
            }
            else
            {
                // We're in the Y part.
                if (split.Length > 1)
                {
                    var leadingZeroes = split[1].Length - newY.ToString().Length;
                    cursor -= leadingZeroes;
                }

                if (newY == 0 && cursor == newX.ToString().Length + this.sep.Length)
                    makeSelection = true;
            }

            // Update the text and set the cursor.
            this.Text = $"{newX}{this.sep}{newY}";

            this.SelectionStart = cursor;
            this.SelectionLength = makeSelection ? 1 : 0;

            // Set new values.
            if (this.x == (int)newX && this.y == (int)newY) return;
            this.x = (int)newX;
            this.y = (int)newY;
            this.ValueChanged?.Invoke(this, new TPoint(this.x, this.y));
        }

        /// <summary>
        /// Updates the text based on the new values and separator settings.
        /// </summary>
        private void UpdateText()
        {
            var cursor = this.SelectionStart;
            this.Text = $"{this.x}{this.sep}{this.y}";
            this.SelectionStart = cursor;
        }

        /// <summary>
        /// Ignores any keypress events because they are already handled in PreviewKeyPress.
        /// </summary>
        private void IgnoreKey(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        #endregion Methods
    }
}