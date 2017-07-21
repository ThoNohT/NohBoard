namespace ThoNohT.NohBoard.Forms.Properties
{
    partial class MouseSpeedPropertiesForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CancelButton2 = new System.Windows.Forms.Button();
            this.AcceptButton2 = new System.Windows.Forms.Button();
            this.lblLocation = new System.Windows.Forms.Label();
            this.txtLocation = new ThoNohT.NohBoard.Controls.VectorTextBox();
            this.lblRadius = new System.Windows.Forms.Label();
            this.udRadius = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.udRadius)).BeginInit();
            this.SuspendLayout();
            // 
            // CancelButton2
            // 
            this.CancelButton2.Location = new System.Drawing.Point(85, 63);
            this.CancelButton2.Name = "CancelButton2";
            this.CancelButton2.Size = new System.Drawing.Size(75, 23);
            this.CancelButton2.TabIndex = 3;
            this.CancelButton2.Text = "Cancel";
            this.CancelButton2.UseVisualStyleBackColor = true;
            this.CancelButton2.Click += new System.EventHandler(this.CancelButton2_Click);
            // 
            // AcceptButton2
            // 
            this.AcceptButton2.Location = new System.Drawing.Point(166, 63);
            this.AcceptButton2.Name = "AcceptButton2";
            this.AcceptButton2.Size = new System.Drawing.Size(75, 23);
            this.AcceptButton2.TabIndex = 4;
            this.AcceptButton2.Text = "Accept";
            this.AcceptButton2.UseVisualStyleBackColor = true;
            this.AcceptButton2.Click += new System.EventHandler(this.AcceptButton2_Click);
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(12, 9);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(51, 13);
            this.lblLocation.TabIndex = 14;
            this.lblLocation.Text = "Location:";
            // 
            // txtLocation
            // 
            this.txtLocation.Location = new System.Drawing.Point(69, 6);
            this.txtLocation.MaxVal = 2147483647;
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Separator = ';';
            this.txtLocation.Size = new System.Drawing.Size(172, 20);
            this.txtLocation.SpacesAroundSeparator = true;
            this.txtLocation.TabIndex = 1;
            this.txtLocation.Text = "0 ; 0";
            this.txtLocation.X = 0;
            this.txtLocation.Y = 0;
            // 
            // lblRadius
            // 
            this.lblRadius.AutoSize = true;
            this.lblRadius.Location = new System.Drawing.Point(12, 34);
            this.lblRadius.Name = "lblRadius";
            this.lblRadius.Size = new System.Drawing.Size(43, 13);
            this.lblRadius.TabIndex = 16;
            this.lblRadius.Text = "Radius:";
            // 
            // udRadius
            // 
            this.udRadius.Location = new System.Drawing.Point(69, 32);
            this.udRadius.Name = "udRadius";
            this.udRadius.Size = new System.Drawing.Size(172, 20);
            this.udRadius.TabIndex = 2;
            // 
            // MouseSpeedPropertiesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 98);
            this.Controls.Add(this.udRadius);
            this.Controls.Add(this.lblRadius);
            this.Controls.Add(this.txtLocation);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.CancelButton2);
            this.Controls.Add(this.AcceptButton2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MouseSpeedPropertiesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Mouse Speed Indicator Properties";
            this.Load += new System.EventHandler(this.MouseSpeedPropertiesForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.udRadius)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CancelButton2;
        private System.Windows.Forms.Button AcceptButton2;
        private System.Windows.Forms.Label lblLocation;
        private Controls.VectorTextBox txtLocation;
        private System.Windows.Forms.Label lblRadius;
        private System.Windows.Forms.NumericUpDown udRadius;
    }
}