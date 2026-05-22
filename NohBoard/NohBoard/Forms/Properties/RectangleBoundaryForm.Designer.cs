namespace ThoNohT.NohBoard.Forms.Properties
{
    using System.Drawing;

    partial class RectangleBoundaryForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblPosition = new System.Windows.Forms.Label();
            lblSize = new System.Windows.Forms.Label();
            btnCancel = new System.Windows.Forms.Button();
            btnApply = new System.Windows.Forms.Button();
            txtSize = new ThoNohT.NohBoard.Controls.VectorTextBox();
            txtPosition = new ThoNohT.NohBoard.Controls.VectorTextBox();
            SuspendLayout();
            lblPosition.Location = new Point(10, 10);
            lblPosition.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblPosition.Name = "lblPosition";
            lblPosition.Size = new Size(80, 23);
            lblPosition.TabIndex = 0;
            lblPosition.Text = "Position";
            lblPosition.TextAlign = ContentAlignment.MiddleLeft;
            lblSize.Location = new Point(10, 40);
            lblSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblSize.Name = "lblSize";
            lblSize.Size = new Size(80, 23);
            lblSize.TabIndex = 1;
            lblSize.Text = "Size";
            lblSize.TextAlign = ContentAlignment.MiddleLeft;
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.Location = new Point(110, 70);
            btnCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(80, 23);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            btnApply.Location = new Point(195, 70);
            btnApply.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnApply.Name = "btnApply";
            btnApply.Size = new Size(80, 23);
            btnApply.TabIndex = 5;
            btnApply.Text = "Apply";
            btnApply.UseVisualStyleBackColor = true;
            btnApply.Click += btnApply_Click;
            txtSize.Location = new Point(95, 40);
            txtSize.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtSize.MaxVal = int.MaxValue;
            txtSize.Name = "txtSize";
            txtSize.Separator = ';';
            txtSize.Size = new Size(180, 23);
            txtSize.SpacesAroundSeparator = true;
            txtSize.TabIndex = 3;
            txtSize.Text = "0 ; 0";
            txtSize.X = 0;
            txtSize.Y = 0;
            txtPosition.Location = new Point(95, 10);
            txtPosition.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtPosition.MaxVal = int.MaxValue;
            txtPosition.Name = "txtPosition";
            txtPosition.Separator = ';';
            txtPosition.Size = new Size(180, 23);
            txtPosition.SpacesAroundSeparator = true;
            txtPosition.TabIndex = 2;
            txtPosition.Text = "0 ; 0";
            txtPosition.X = 0;
            txtPosition.Y = 0;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(284, 101);
            Controls.Add(btnApply);
            Controls.Add(btnCancel);
            Controls.Add(txtSize);
            Controls.Add(txtPosition);
            Controls.Add(lblSize);
            Controls.Add(lblPosition);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "RectangleBoundaryForm";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Rectangle";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPosition;
        private System.Windows.Forms.Label lblSize;
        private Controls.VectorTextBox txtPosition;
        private Controls.VectorTextBox txtSize;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
    }
}
