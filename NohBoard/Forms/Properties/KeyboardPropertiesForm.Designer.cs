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
    partial class KeyboardPropertiesForm
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
            this.lblSize = new System.Windows.Forms.Label();
            this.txtSize = new ThoNohT.NohBoard.Controls.VectorTextBox();
            this.SuspendLayout();
            // 
            // CancelButton2
            // 
            this.CancelButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton2.Location = new System.Drawing.Point(112, 37);
            this.CancelButton2.Margin = new System.Windows.Forms.Padding(4);
            this.CancelButton2.Name = "CancelButton2";
            this.CancelButton2.Size = new System.Drawing.Size(100, 28);
            this.CancelButton2.TabIndex = 3;
            this.CancelButton2.Text = "Cancel";
            this.CancelButton2.UseVisualStyleBackColor = true;
            this.CancelButton2.Click += new System.EventHandler(this.CancelButton2_Click);
            // 
            // AcceptButton2
            // 
            this.AcceptButton2.Location = new System.Drawing.Point(220, 37);
            this.AcceptButton2.Margin = new System.Windows.Forms.Padding(4);
            this.AcceptButton2.Name = "AcceptButton2";
            this.AcceptButton2.Size = new System.Drawing.Size(100, 28);
            this.AcceptButton2.TabIndex = 4;
            this.AcceptButton2.Text = "Accept";
            this.AcceptButton2.UseVisualStyleBackColor = true;
            this.AcceptButton2.Click += new System.EventHandler(this.AcceptButton2_Click);
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Location = new System.Drawing.Point(16, 11);
            this.lblSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(39, 17);
            this.lblSize.TabIndex = 14;
            this.lblSize.Text = "Size:";
            // 
            // txtSize
            // 
            this.txtSize.Location = new System.Drawing.Point(92, 7);
            this.txtSize.Margin = new System.Windows.Forms.Padding(4);
            this.txtSize.MaxVal = 2147483647;
            this.txtSize.Name = "txtSize";
            this.txtSize.Separator = ';';
            this.txtSize.Size = new System.Drawing.Size(228, 22);
            this.txtSize.SpacesAroundSeparator = true;
            this.txtSize.TabIndex = 1;
            this.txtSize.Text = "0 ; 0";
            this.txtSize.X = 0;
            this.txtSize.Y = 0;
            // 
            // KeyboardPropertiesForm
            // 
            this.AcceptButton = this.AcceptButton2;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelButton2;
            this.ClientSize = new System.Drawing.Size(337, 81);
            this.Controls.Add(this.txtSize);
            this.Controls.Add(this.lblSize);
            this.Controls.Add(this.CancelButton2);
            this.Controls.Add(this.AcceptButton2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "KeyboardPropertiesForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Keyboard Properties";
            this.Load += new System.EventHandler(this.MouseSpeedPropertiesForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CancelButton2;
        private System.Windows.Forms.Button AcceptButton2;
        private System.Windows.Forms.Label lblSize;
        private Controls.VectorTextBox txtSize;
    }
}