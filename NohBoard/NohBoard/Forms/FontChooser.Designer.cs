namespace ThoNohT.NohBoard.Forms
{
    partial class FontChooser
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DisplayLabel = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // DisplayLabel
            // 
            DisplayLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            DisplayLabel.AutoEllipsis = true;
            DisplayLabel.BackColor = System.Drawing.SystemColors.Control;
            DisplayLabel.Location = new System.Drawing.Point(0, 0);
            DisplayLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            DisplayLabel.Name = "DisplayLabel";
            DisplayLabel.Size = new System.Drawing.Size(200, 23);
            DisplayLabel.TabIndex = 0;
            DisplayLabel.Text = "Pick a Font";
            DisplayLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            DisplayLabel.DoubleClick += FontChooser_DoubleClick;
            DisplayLabel.Layout += DisplayLabel_Layout;
            // 
            // FontChooser
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.Control;
            Controls.Add(DisplayLabel);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FontChooser";
            Size = new System.Drawing.Size(200, 23);
            DoubleClick += FontChooser_DoubleClick;
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label DisplayLabel;
    }
}
