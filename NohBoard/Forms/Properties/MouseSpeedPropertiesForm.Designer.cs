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
            this.vtxtLocation = new ThoNohT.NohBoard.Controls.VectorTextBox();
            this.lblRadius = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // CancelButton2
            // 
            this.CancelButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton2.Location = new System.Drawing.Point(85, 63);
            this.CancelButton2.Name = "CancelButton2";
            this.CancelButton2.Size = new System.Drawing.Size(75, 23);
            this.CancelButton2.TabIndex = 13;
            this.CancelButton2.Text = "Cancel";
            this.CancelButton2.UseVisualStyleBackColor = true;
            // 
            // AcceptButton2
            // 
            this.AcceptButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.AcceptButton2.Location = new System.Drawing.Point(166, 63);
            this.AcceptButton2.Name = "AcceptButton2";
            this.AcceptButton2.Size = new System.Drawing.Size(75, 23);
            this.AcceptButton2.TabIndex = 12;
            this.AcceptButton2.Text = "Accept";
            this.AcceptButton2.UseVisualStyleBackColor = true;
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
            // vtxtLocation
            // 
            this.vtxtLocation.Location = new System.Drawing.Point(69, 6);
            this.vtxtLocation.MaxVal = 2147483647;
            this.vtxtLocation.Name = "vtxtLocation";
            this.vtxtLocation.Separator = ';';
            this.vtxtLocation.Size = new System.Drawing.Size(172, 20);
            this.vtxtLocation.SpacesAroundSeparator = true;
            this.vtxtLocation.TabIndex = 15;
            this.vtxtLocation.X = 0;
            this.vtxtLocation.Y = 0;
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
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(69, 32);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(172, 20);
            this.numericUpDown1.TabIndex = 17;
            // 
            // MouseSpeedPropertiesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 98);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.lblRadius);
            this.Controls.Add(this.vtxtLocation);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.CancelButton2);
            this.Controls.Add(this.AcceptButton2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MouseSpeedPropertiesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Mouse Speed Indicator Properties";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CancelButton2;
        private System.Windows.Forms.Button AcceptButton2;
        private System.Windows.Forms.Label lblLocation;
        private Controls.VectorTextBox vtxtLocation;
        private System.Windows.Forms.Label lblRadius;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
    }
}