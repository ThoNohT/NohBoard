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
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// Double-buffered client area for the keyboard overlay.
    /// </summary>
    internal class KeyboardSurfacePanel : Panel
    {
        public KeyboardSurfacePanel()
        {
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer,
                true);
            this.UpdateStyles();
            this.BackColor = Color.Black;
        }

        /// <summary>
        /// When true, the main form presents pixels via <c>UpdateLayeredWindow</c>; this panel must not paint a background.
        /// </summary>
        internal Func<bool> IsLayeredPresentationActive { get; set; }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (this.IsLayeredPresentationActive?.Invoke() == true)
                return;

            base.OnPaintBackground(e);
        }
    }
}
