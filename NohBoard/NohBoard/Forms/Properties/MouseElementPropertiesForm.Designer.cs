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

    partial class MouseElementPropertiesForm
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
            txtTextPosition = new ThoNohT.NohBoard.Controls.VectorTextBox();
            lblTextPosition = new System.Windows.Forms.Label();
            txtText = new System.Windows.Forms.TextBox();
            lblText = new System.Windows.Forms.Label();
            cmbKeyCode = new System.Windows.Forms.ComboBox();
            lblKeyCode = new System.Windows.Forms.Label();
            lstBoundaries = new System.Windows.Forms.ListBox();
            lblBoundaries = new System.Windows.Forms.Label();
            CancelButton2 = new System.Windows.Forms.Button();
            AcceptButton2 = new System.Windows.Forms.Button();
            txtBoundaries = new ThoNohT.NohBoard.Controls.VectorTextBox();
            btnAddBoundary = new System.Windows.Forms.Button();
            btnRemoveBoundary = new System.Windows.Forms.Button();
            btnBoundaryUp = new System.Windows.Forms.Button();
            btnBoundaryDown = new System.Windows.Forms.Button();
            btnUpdateBoundary = new System.Windows.Forms.Button();
            btnCenterText = new System.Windows.Forms.Button();
            btnRectangle = new System.Windows.Forms.Button();
            SuspendLayout();
            txtTextPosition.Location = new Point(95, 70);
            txtTextPosition.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtTextPosition.MaxVal = int.MaxValue;
            txtTextPosition.Name = "txtTextPosition";
            txtTextPosition.Separator = ';';
            txtTextPosition.Size = new Size(90, 23);
            txtTextPosition.SpacesAroundSeparator = true;
            txtTextPosition.TabIndex = 2;
            txtTextPosition.Text = "0 ; 0";
            txtTextPosition.X = 0;
            txtTextPosition.Y = 0;
            lblTextPosition.Location = new Point(10, 70);
            lblTextPosition.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblTextPosition.Name = "lblTextPosition";
            lblTextPosition.Size = new Size(80, 23);
            lblTextPosition.TabIndex = 16;
            lblTextPosition.Text = "Text Position:";
            lblTextPosition.TextAlign = ContentAlignment.MiddleLeft;
            txtText.Location = new Point(95, 40);
            txtText.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtText.Name = "txtText";
            txtText.Size = new Size(180, 23);
            txtText.TabIndex = 1;
            lblText.Location = new Point(10, 40);
            lblText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblText.Name = "lblText";
            lblText.Size = new Size(80, 23);
            lblText.TabIndex = 19;
            lblText.Text = "Text:";
            lblText.TextAlign = ContentAlignment.MiddleLeft;
            cmbKeyCode.FormattingEnabled = true;
            cmbKeyCode.Location = new Point(95, 10);
            cmbKeyCode.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cmbKeyCode.Name = "cmbKeyCode";
            cmbKeyCode.Size = new Size(180, 23);
            cmbKeyCode.TabIndex = 0;
            lblKeyCode.Location = new Point(10, 10);
            lblKeyCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblKeyCode.Name = "lblKeyCode";
            lblKeyCode.Size = new Size(80, 23);
            lblKeyCode.TabIndex = 21;
            lblKeyCode.Text = "KeyCode:";
            lblKeyCode.TextAlign = ContentAlignment.MiddleLeft;
            lstBoundaries.FormattingEnabled = true;
            lstBoundaries.IntegralHeight = false;
            lstBoundaries.ItemHeight = 15;
            lstBoundaries.Location = new Point(95, 130);
            lstBoundaries.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            lstBoundaries.Name = "lstBoundaries";
            lstBoundaries.Size = new Size(180, 145);
            lstBoundaries.TabIndex = 10;
            lblBoundaries.Location = new Point(10, 100);
            lblBoundaries.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblBoundaries.Name = "lblBoundaries";
            lblBoundaries.Size = new Size(80, 23);
            lblBoundaries.TabIndex = 23;
            lblBoundaries.Text = "Boundaries:";
            lblBoundaries.TextAlign = ContentAlignment.MiddleLeft;
            CancelButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            CancelButton2.Location = new Point(110, 280);
            CancelButton2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CancelButton2.Name = "CancelButton2";
            CancelButton2.Size = new Size(80, 23);
            CancelButton2.TabIndex = 11;
            CancelButton2.Text = "Cancel";
            CancelButton2.UseVisualStyleBackColor = true;
            CancelButton2.Click += CancelButton2_Click;
            AcceptButton2.Location = new Point(195, 280);
            AcceptButton2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            AcceptButton2.Name = "AcceptButton2";
            AcceptButton2.Size = new Size(80, 23);
            AcceptButton2.TabIndex = 12;
            AcceptButton2.Text = "Accept";
            AcceptButton2.UseVisualStyleBackColor = true;
            AcceptButton2.Click += AcceptButton2_Click;
            txtBoundaries.Location = new Point(95, 100);
            txtBoundaries.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtBoundaries.MaxVal = int.MaxValue;
            txtBoundaries.Name = "txtBoundaries";
            txtBoundaries.Separator = ';';
            txtBoundaries.Size = new Size(180, 23);
            txtBoundaries.SpacesAroundSeparator = true;
            txtBoundaries.TabIndex = 3;
            txtBoundaries.Text = "0 ; 0";
            txtBoundaries.X = 0;
            txtBoundaries.Y = 0;
            btnAddBoundary.Location = new Point(5, 130);
            btnAddBoundary.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnAddBoundary.Name = "btnAddBoundary";
            btnAddBoundary.Size = new Size(80, 23);
            btnAddBoundary.TabIndex = 4;
            btnAddBoundary.Text = "Add";
            btnAddBoundary.UseVisualStyleBackColor = true;
            btnAddBoundary.Click += btnAddBoundary_Click;
            btnRemoveBoundary.Location = new Point(5, 180);
            btnRemoveBoundary.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnRemoveBoundary.Name = "btnRemoveBoundary";
            btnRemoveBoundary.Size = new Size(80, 23);
            btnRemoveBoundary.TabIndex = 6;
            btnRemoveBoundary.Text = "Remove";
            btnRemoveBoundary.UseVisualStyleBackColor = true;
            btnRemoveBoundary.Click += btnRemoveBoundary_Click;
            btnBoundaryUp.Location = new Point(5, 205);
            btnBoundaryUp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnBoundaryUp.Name = "btnBoundaryUp";
            btnBoundaryUp.Size = new Size(80, 23);
            btnBoundaryUp.TabIndex = 7;
            btnBoundaryUp.Text = "Up";
            btnBoundaryUp.UseVisualStyleBackColor = true;
            btnBoundaryUp.Click += btnBoundaryUp_Click;
            btnBoundaryDown.Location = new Point(5, 230);
            btnBoundaryDown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnBoundaryDown.Name = "btnBoundaryDown";
            btnBoundaryDown.Size = new Size(80, 23);
            btnBoundaryDown.TabIndex = 8;
            btnBoundaryDown.Text = "Down";
            btnBoundaryDown.UseVisualStyleBackColor = true;
            btnBoundaryDown.Click += btnBoundaryDown_Click;
            btnUpdateBoundary.Location = new Point(5, 155);
            btnUpdateBoundary.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnUpdateBoundary.Name = "btnUpdateBoundary";
            btnUpdateBoundary.Size = new Size(80, 23);
            btnUpdateBoundary.TabIndex = 5;
            btnUpdateBoundary.Text = "Update";
            btnUpdateBoundary.UseVisualStyleBackColor = true;
            btnUpdateBoundary.Click += btnUpdateBoundary_Click;
            btnCenterText.Location = new Point(195, 70);
            btnCenterText.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnCenterText.Name = "btnCenterText";
            btnCenterText.Size = new Size(80, 23);
            btnCenterText.TabIndex = 3;
            btnCenterText.Text = "Center";
            btnCenterText.UseVisualStyleBackColor = true;
            btnCenterText.Click += btnCenterText_Click;
            btnRectangle.Location = new Point(5, 255);
            btnRectangle.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnRectangle.Name = "btnRectangle";
            btnRectangle.Size = new Size(80, 23);
            btnRectangle.TabIndex = 9;
            btnRectangle.Text = "Rectangle";
            btnRectangle.UseVisualStyleBackColor = true;
            btnRectangle.Click += btnRectangle_Click;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = CancelButton2;
            ClientSize = new Size(284, 311);
            Controls.Add(btnUpdateBoundary);
            Controls.Add(btnCenterText);
            Controls.Add(btnRectangle);
            Controls.Add(btnBoundaryDown);
            Controls.Add(btnBoundaryUp);
            Controls.Add(btnRemoveBoundary);
            Controls.Add(btnAddBoundary);
            Controls.Add(txtBoundaries);
            Controls.Add(CancelButton2);
            Controls.Add(AcceptButton2);
            Controls.Add(lblBoundaries);
            Controls.Add(lstBoundaries);
            Controls.Add(lblKeyCode);
            Controls.Add(cmbKeyCode);
            Controls.Add(lblText);
            Controls.Add(txtText);
            Controls.Add(txtTextPosition);
            Controls.Add(lblTextPosition);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "MouseElementPropertiesForm";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "MouseElementPropertiesForm";
            Load += MouseElementPropertiesForm_Load;
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private Controls.VectorTextBox txtTextPosition;
        private System.Windows.Forms.Label lblTextPosition;
        private System.Windows.Forms.TextBox txtText;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.ComboBox cmbKeyCode;
        private System.Windows.Forms.Label lblKeyCode;
        private System.Windows.Forms.ListBox lstBoundaries;
        private System.Windows.Forms.Label lblBoundaries;
        private System.Windows.Forms.Button CancelButton2;
        private System.Windows.Forms.Button AcceptButton2;
        private Controls.VectorTextBox txtBoundaries;
        private System.Windows.Forms.Button btnAddBoundary;
        private System.Windows.Forms.Button btnRemoveBoundary;
        private System.Windows.Forms.Button btnBoundaryUp;
        private System.Windows.Forms.Button btnBoundaryDown;
        private System.Windows.Forms.Button btnUpdateBoundary;
        private System.Windows.Forms.Button btnCenterText;
        private System.Windows.Forms.Button btnRectangle;
    }
}
