/*
Copyright (C) 2016 by Marius Becker <marius.becker.8@gmail.com>

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
    using Extra;

    /// <summary>
    /// The form used to change a key's boundaries to a rectangle.
    /// </summary>
    public partial class RectangleBoundaryForm : Form
    {
        /// <summary>
        /// This event is invoked when the user presses the button to save the current dimensions.
        /// </summary>
        public event Action<TRectangle> DimensionsSet;

        public RectangleBoundaryForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Creates a new instance and fills the input controls with preset values.
        /// </summary>
        public RectangleBoundaryForm(TRectangle rectangle)
        {
            InitializeComponent();

            var position = rectangle.Position;
            this.txtPosition.X = position.X;
            this.txtPosition.Y = position.Y;

            var size = rectangle.Size;
            this.txtSize.X = size.Width;
            this.txtSize.Y = size.Height;
        }

        /// <summary>
        /// Handles the click event of the "Apply" button. Invokes the DimensionsSet event.
        /// </summary>
        private void btnApply_Click(object sender, EventArgs e)
        {
            var position = new TPoint(txtPosition.X, txtPosition.Y);
            var size = new TPoint(txtSize.X, txtSize.Y);
            var rectangle = new TRectangle(position, size);
            this.DimensionsSet?.Invoke(rectangle);

            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Handles the click event of the "Cancel" button. Closes the form without invoking any events.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
