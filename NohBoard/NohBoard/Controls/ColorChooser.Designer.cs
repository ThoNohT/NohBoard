namespace ThoNohT.NohBoard.Controls
{
    partial class ColorChooser
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            DisplayLabel = new System.Windows.Forms.Label();
            SuspendLayout();
            DisplayLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            DisplayLabel.AutoEllipsis = true;
            DisplayLabel.AutoSize = false;
            DisplayLabel.BackColor = System.Drawing.SystemColors.Control;
            DisplayLabel.Location = new System.Drawing.Point(0, 0);
            DisplayLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            DisplayLabel.Name = "DisplayLabel";
            DisplayLabel.Size = new System.Drawing.Size(200, 23);
            DisplayLabel.TabIndex = 0;
            DisplayLabel.Text = "Pick a color";
            DisplayLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            DisplayLabel.DoubleClick += ColorChooser_DoubleClick;
            DisplayLabel.Layout += DisplayLabel_Layout;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.Control;
            Controls.Add(DisplayLabel);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "ColorChooser";
            Size = new System.Drawing.Size(200, 23);
            DoubleClick += ColorChooser_DoubleClick;
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label DisplayLabel;
    }
}
