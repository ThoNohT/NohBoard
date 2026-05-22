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
    using System.Drawing;

    partial class MouseSpeedPropertiesForm
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            CancelButton2 = new System.Windows.Forms.Button();
            AcceptButton2 = new System.Windows.Forms.Button();
            lblLocation = new System.Windows.Forms.Label();
            txtLocation = new ThoNohT.NohBoard.Controls.VectorTextBox();
            lblRadius = new System.Windows.Forms.Label();
            udRadius = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)udRadius).BeginInit();
            SuspendLayout();
            CancelButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            CancelButton2.Location = new Point(110, 70);
            CancelButton2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CancelButton2.Name = "CancelButton2";
            CancelButton2.Size = new Size(80, 23);
            CancelButton2.TabIndex = 3;
            CancelButton2.Text = "Cancel";
            CancelButton2.UseVisualStyleBackColor = true;
            CancelButton2.Click += CancelButton2_Click;
            AcceptButton2.Location = new Point(195, 70);
            AcceptButton2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            AcceptButton2.Name = "AcceptButton2";
            AcceptButton2.Size = new Size(80, 23);
            AcceptButton2.TabIndex = 4;
            AcceptButton2.Text = "Accept";
            AcceptButton2.UseVisualStyleBackColor = true;
            AcceptButton2.Click += AcceptButton2_Click;
            lblLocation.Location = new Point(10, 10);
            lblLocation.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            lblLocation.Name = "lblLocation";
            lblLocation.Size = new Size(80, 23);
            lblLocation.TabIndex = 14;
            lblLocation.Text = "Location:";
            lblLocation.TextAlign = ContentAlignment.MiddleLeft;
            txtLocation.Location = new Point(95, 10);
            txtLocation.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            txtLocation.MaxVal = int.MaxValue;
            txtLocation.Name = "txtLocation";
            txtLocation.Separator = ';';
            txtLocation.Size = new Size(180, 23);
            txtLocation.SpacesAroundSeparator = true;
            txtLocation.TabIndex = 1;
            txtLocation.Text = "0 ; 0";
            txtLocation.X = 0;
            txtLocation.Y = 0;
            lblRadius.Location = new Point(10, 40);
            lblRadius.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            lblRadius.Name = "lblRadius";
            lblRadius.Size = new Size(80, 23);
            lblRadius.TabIndex = 16;
            lblRadius.Text = "Radius:";
            lblRadius.TextAlign = ContentAlignment.MiddleLeft;
            udRadius.Location = new Point(95, 40);
            udRadius.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            udRadius.Name = "udRadius";
            udRadius.Size = new Size(180, 23);
            udRadius.TabIndex = 2;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = CancelButton2;
            ClientSize = new Size(284, 101);
            Controls.Add(udRadius);
            Controls.Add(lblRadius);
            Controls.Add(txtLocation);
            Controls.Add(lblLocation);
            Controls.Add(CancelButton2);
            Controls.Add(AcceptButton2);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            Name = "MouseSpeedPropertiesForm";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Mouse Speed Indicator Properties";
            Load += MouseSpeedPropertiesForm_Load;
            ((System.ComponentModel.ISupportInitialize)udRadius).EndInit();
            ResumeLayout(false);
            PerformLayout();

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
