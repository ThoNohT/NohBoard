using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThoNohT.NohBoard.Forms.Properties
{
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
        /// <param name="rectangle"></param>
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
