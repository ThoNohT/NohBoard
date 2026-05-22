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
            lblSize = new System.Windows.Forms.Label();
            txtSize = new ThoNohT.NohBoard.Controls.VectorTextBox();
            SuspendLayout();
            CancelButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            CancelButton2.Location = new System.Drawing.Point(90, 40);
            CancelButton2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CancelButton2.Name = "CancelButton2";
            CancelButton2.Size = new System.Drawing.Size(80, 23);
            CancelButton2.TabIndex = 3;
            CancelButton2.Text = "Cancel";
            CancelButton2.UseVisualStyleBackColor = true;
            CancelButton2.Click += CancelButton2_Click;
            AcceptButton2.Location = new System.Drawing.Point(175, 40);
            AcceptButton2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            AcceptButton2.Name = "AcceptButton2";
            AcceptButton2.Size = new System.Drawing.Size(80, 23);
            AcceptButton2.TabIndex = 4;
            AcceptButton2.Text = "Accept";
            AcceptButton2.UseVisualStyleBackColor = true;
            AcceptButton2.Click += AcceptButton2_Click;
            lblSize.Location = new System.Drawing.Point(10, 10);
            lblSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblSize.Name = "lblSize";
            lblSize.Size = new System.Drawing.Size(60, 23);
            lblSize.TabIndex = 14;
            lblSize.Text = "Size:";
            lblSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            txtSize.Location = new System.Drawing.Point(75, 10);
            txtSize.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtSize.MaxVal = int.MaxValue;
            txtSize.Name = "txtSize";
            txtSize.Separator = ';';
            txtSize.Size = new System.Drawing.Size(180, 23);
            txtSize.SpacesAroundSeparator = true;
            txtSize.TabIndex = 1;
            txtSize.Text = "0 ; 0";
            txtSize.X = 0;
            txtSize.Y = 0;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = CancelButton2;
            ClientSize = new System.Drawing.Size(264, 71);
            Controls.Add(txtSize);
            Controls.Add(lblSize);
            Controls.Add(CancelButton2);
            Controls.Add(AcceptButton2);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "KeyboardPropertiesForm";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Keyboard Properties";
            Load += KeyboardPropertiesForm_Load;
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CancelButton2;
        private System.Windows.Forms.Button AcceptButton2;
        private System.Windows.Forms.Label lblSize;
        private Controls.VectorTextBox txtSize;
    }
}
