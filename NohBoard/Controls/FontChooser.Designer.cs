namespace ThoNohT.NohBoard.Controls
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
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
            this.DisplayLabel = new System.Windows.Forms.Label();
            this.lblLink = new System.Windows.Forms.Label();
            this.txtLink = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // DisplayLabel
            // 
            this.DisplayLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DisplayLabel.AutoEllipsis = true;
            this.DisplayLabel.BackColor = System.Drawing.SystemColors.Control;
            this.DisplayLabel.Location = new System.Drawing.Point(38, 0);
            this.DisplayLabel.Name = "DisplayLabel";
            this.DisplayLabel.Size = new System.Drawing.Size(182, 26);
            this.DisplayLabel.TabIndex = 0;
            this.DisplayLabel.Text = "Pick a Font";
            this.DisplayLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.DisplayLabel.DoubleClick += new System.EventHandler(this.FontChooser_DoubleClick);
            this.DisplayLabel.Layout += new System.Windows.Forms.LayoutEventHandler(this.DisplayLabel_Layout);
            // 
            // lblLink
            // 
            this.lblLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblLink.AutoSize = true;
            this.lblLink.Location = new System.Drawing.Point(5, 33);
            this.lblLink.Name = "lblLink";
            this.lblLink.Size = new System.Drawing.Size(30, 13);
            this.lblLink.TabIndex = 1;
            this.lblLink.Text = "Link:";
            // 
            // txtLink
            // 
            this.txtLink.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLink.Location = new System.Drawing.Point(41, 30);
            this.txtLink.Name = "txtLink";
            this.txtLink.Size = new System.Drawing.Size(176, 20);
            this.txtLink.TabIndex = 2;
            this.txtLink.TextChanged += new System.EventHandler(this.txtLink_TextChanged);
            // 
            // FontChooser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.txtLink);
            this.Controls.Add(this.lblLink);
            this.Controls.Add(this.DisplayLabel);
            this.Name = "FontChooser";
            this.Size = new System.Drawing.Size(220, 50);
            this.DoubleClick += new System.EventHandler(this.FontChooser_DoubleClick);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label DisplayLabel;
        private System.Windows.Forms.Label lblLink;
        private System.Windows.Forms.TextBox txtLink;
    }
}
