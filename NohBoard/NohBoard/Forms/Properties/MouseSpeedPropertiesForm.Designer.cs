/*
Copyright (C) 2017 by Eric Bataille <e.c.p.bataille@gmail.com>

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
            this.CancelButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton2.Location = new System.Drawing.Point(113, 78);
            this.CancelButton2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CancelButton2.Name = "CancelButton2";
            this.CancelButton2.Size = new System.Drawing.Size(100, 28);
            this.CancelButton2.TabIndex = 3;
            this.CancelButton2.Text = "Cancel";
            this.CancelButton2.UseVisualStyleBackColor = true;
            this.CancelButton2.Click += new System.EventHandler(this.CancelButton2_Click);
            // 
            // AcceptButton2
            // 
            this.AcceptButton2.Location = new System.Drawing.Point(221, 78);
            this.AcceptButton2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.AcceptButton2.Name = "AcceptButton2";
            this.AcceptButton2.Size = new System.Drawing.Size(100, 28);
            this.AcceptButton2.TabIndex = 4;
            this.AcceptButton2.Text = "Accept";
            this.AcceptButton2.UseVisualStyleBackColor = true;
            this.AcceptButton2.Click += new System.EventHandler(this.AcceptButton2_Click);
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(16, 11);
            this.lblLocation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(66, 17);
            this.lblLocation.TabIndex = 14;
            this.lblLocation.Text = "Location:";
            // 
            // txtLocation
            // 
            this.txtLocation.Location = new System.Drawing.Point(92, 7);
            this.txtLocation.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtLocation.MaxVal = 2147483647;
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Separator = ';';
            this.txtLocation.Size = new System.Drawing.Size(228, 22);
            this.txtLocation.SpacesAroundSeparator = true;
            this.txtLocation.TabIndex = 1;
            this.txtLocation.Text = "0 ; 0";
            this.txtLocation.X = 0;
            this.txtLocation.Y = 0;
            // 
            // lblRadius
            // 
            this.lblRadius.AutoSize = true;
            this.lblRadius.Location = new System.Drawing.Point(16, 42);
            this.lblRadius.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRadius.Name = "lblRadius";
            this.lblRadius.Size = new System.Drawing.Size(56, 17);
            this.lblRadius.TabIndex = 16;
            this.lblRadius.Text = "Radius:";
            // 
            // udRadius
            // 
            this.udRadius.Location = new System.Drawing.Point(92, 39);
            this.udRadius.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.udRadius.Name = "udRadius";
            this.udRadius.Size = new System.Drawing.Size(229, 22);
            this.udRadius.TabIndex = 2;
            // 
            // MouseSpeedPropertiesForm
            // 
            this.AcceptButton = this.AcceptButton2;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelButton2;
            this.ClientSize = new System.Drawing.Size(337, 121);
            this.Controls.Add(this.udRadius);
            this.Controls.Add(this.lblRadius);
            this.Controls.Add(this.txtLocation);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.CancelButton2);
            this.Controls.Add(this.AcceptButton2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MouseSpeedPropertiesForm";
            this.ShowInTaskbar = false;
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