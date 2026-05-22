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

    partial class KeyboardKeyPropertiesForm
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
            btnBoundaryDown = new System.Windows.Forms.Button();
            btnBoundaryUp = new System.Windows.Forms.Button();
            btnRemoveBoundary = new System.Windows.Forms.Button();
            btnAddBoundary = new System.Windows.Forms.Button();
            CancelButton2 = new System.Windows.Forms.Button();
            AcceptButton2 = new System.Windows.Forms.Button();
            lblBoundaries = new System.Windows.Forms.Label();
            lstBoundaries = new System.Windows.Forms.ListBox();
            lblText = new System.Windows.Forms.Label();
            txtText = new System.Windows.Forms.TextBox();
            lblTextPosition = new System.Windows.Forms.Label();
            lblShiftText = new System.Windows.Forms.Label();
            txtShiftText = new System.Windows.Forms.TextBox();
            lstKeyCodes = new System.Windows.Forms.ListBox();
            btnRemoveKeyCode = new System.Windows.Forms.Button();
            btnAddKeyCode = new System.Windows.Forms.Button();
            udKeyCode = new System.Windows.Forms.NumericUpDown();
            lblKeyCodes = new System.Windows.Forms.Label();
            chkChangeOnCaps = new System.Windows.Forms.CheckBox();
            btnUpdateBoundary = new System.Windows.Forms.Button();
            btnCenterText = new System.Windows.Forms.Button();
            btnRectangle = new System.Windows.Forms.Button();
            btnDetectKeyCode = new System.Windows.Forms.Button();
            txtBoundaries = new ThoNohT.NohBoard.Controls.VectorTextBox();
            txtTextPosition = new ThoNohT.NohBoard.Controls.VectorTextBox();
            ((System.ComponentModel.ISupportInitialize)udKeyCode).BeginInit();
            SuspendLayout();
            btnBoundaryDown.Location = new Point(10, 270);
            btnBoundaryDown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnBoundaryDown.Name = "btnBoundaryDown";
            btnBoundaryDown.Size = new Size(80, 23);
            btnBoundaryDown.TabIndex = 10;
            btnBoundaryDown.Text = "Down";
            btnBoundaryDown.UseVisualStyleBackColor = true;
            btnBoundaryDown.Click += btnBoundaryDown_Click;
            btnBoundaryUp.Location = new Point(10, 245);
            btnBoundaryUp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnBoundaryUp.Name = "btnBoundaryUp";
            btnBoundaryUp.Size = new Size(80, 23);
            btnBoundaryUp.TabIndex = 9;
            btnBoundaryUp.Text = "Up";
            btnBoundaryUp.UseVisualStyleBackColor = true;
            btnBoundaryUp.Click += btnBoundaryUp_Click;
            btnRemoveBoundary.Location = new Point(10, 195);
            btnRemoveBoundary.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnRemoveBoundary.Name = "btnRemoveBoundary";
            btnRemoveBoundary.Size = new Size(80, 23);
            btnRemoveBoundary.TabIndex = 8;
            btnRemoveBoundary.Text = "Remove";
            btnRemoveBoundary.UseVisualStyleBackColor = true;
            btnRemoveBoundary.Click += btnRemoveBoundary_Click;
            btnAddBoundary.Location = new Point(10, 130);
            btnAddBoundary.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnAddBoundary.Name = "btnAddBoundary";
            btnAddBoundary.Size = new Size(80, 23);
            btnAddBoundary.TabIndex = 6;
            btnAddBoundary.Text = "Add";
            btnAddBoundary.UseVisualStyleBackColor = true;
            btnAddBoundary.Click += btnAddBoundary_Click;
            CancelButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            CancelButton2.Location = new Point(390, 305);
            CancelButton2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CancelButton2.Name = "CancelButton2";
            CancelButton2.Size = new Size(80, 23);
            CancelButton2.TabIndex = 18;
            CancelButton2.Text = "Cancel";
            CancelButton2.UseVisualStyleBackColor = true;
            CancelButton2.Click += CancelButton2_Click;
            AcceptButton2.Location = new Point(475, 305);
            AcceptButton2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            AcceptButton2.Name = "AcceptButton2";
            AcceptButton2.Size = new Size(80, 23);
            AcceptButton2.TabIndex = 19;
            AcceptButton2.Text = "Accept";
            AcceptButton2.UseVisualStyleBackColor = true;
            AcceptButton2.Click += AcceptButton2_Click;
            lblBoundaries.Location = new Point(10, 100);
            lblBoundaries.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblBoundaries.Name = "lblBoundaries";
            lblBoundaries.Size = new Size(80, 23);
            lblBoundaries.TabIndex = 36;
            lblBoundaries.Text = "Boundaries:";
            lblBoundaries.TextAlign = ContentAlignment.MiddleLeft;
            lstBoundaries.FormattingEnabled = true;
            lstBoundaries.IntegralHeight = false;
            lstBoundaries.ItemHeight = 15;
            lstBoundaries.Location = new Point(95, 130);
            lstBoundaries.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            lstBoundaries.Name = "lstBoundaries";
            lstBoundaries.Size = new Size(180, 200);
            lstBoundaries.TabIndex = 12;
            lblText.Location = new Point(10, 10);
            lblText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblText.Name = "lblText";
            lblText.Size = new Size(80, 23);
            lblText.TabIndex = 34;
            lblText.Text = "Text:";
            lblText.TextAlign = ContentAlignment.MiddleLeft;
            txtText.Location = new Point(95, 10);
            txtText.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtText.Name = "txtText";
            txtText.Size = new Size(180, 23);
            txtText.TabIndex = 0;
            lblTextPosition.Location = new Point(10, 70);
            lblTextPosition.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblTextPosition.Name = "lblTextPosition";
            lblTextPosition.Size = new Size(80, 23);
            lblTextPosition.TabIndex = 31;
            lblTextPosition.Text = "Text Position:";
            lblTextPosition.TextAlign = ContentAlignment.MiddleLeft;
            lblShiftText.Location = new Point(10, 40);
            lblShiftText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblShiftText.Name = "lblShiftText";
            lblShiftText.Size = new Size(80, 23);
            lblShiftText.TabIndex = 45;
            lblShiftText.Text = "Shift Text:";
            lblShiftText.TextAlign = ContentAlignment.MiddleLeft;
            txtShiftText.Location = new Point(95, 40);
            txtShiftText.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtShiftText.Name = "txtShiftText";
            txtShiftText.Size = new Size(180, 23);
            txtShiftText.TabIndex = 2;
            lstKeyCodes.FormattingEnabled = true;
            lstKeyCodes.IntegralHeight = false;
            lstKeyCodes.ItemHeight = 15;
            lstKeyCodes.Location = new Point(375, 70);
            lstKeyCodes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            lstKeyCodes.Name = "lstKeyCodes";
            lstKeyCodes.Size = new Size(180, 230);
            lstKeyCodes.TabIndex = 17;
            btnRemoveKeyCode.Location = new Point(286, 100);
            btnRemoveKeyCode.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnRemoveKeyCode.Name = "btnRemoveKeyCode";
            btnRemoveKeyCode.Size = new Size(80, 23);
            btnRemoveKeyCode.TabIndex = 15;
            btnRemoveKeyCode.Text = "Remove";
            btnRemoveKeyCode.UseVisualStyleBackColor = true;
            btnRemoveKeyCode.Click += btnRemoveKeyCode_Click;
            btnAddKeyCode.Location = new Point(286, 70);
            btnAddKeyCode.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnAddKeyCode.Name = "btnAddKeyCode";
            btnAddKeyCode.Size = new Size(80, 23);
            btnAddKeyCode.TabIndex = 14;
            btnAddKeyCode.Text = "Add";
            btnAddKeyCode.UseVisualStyleBackColor = true;
            btnAddKeyCode.Click += btnAddKeyCode_Click;
            udKeyCode.Location = new Point(375, 40);
            udKeyCode.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            udKeyCode.Maximum = new decimal(new int[] { 1028, 0, 0, 0 });
            udKeyCode.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            udKeyCode.Name = "udKeyCode";
            udKeyCode.Size = new Size(180, 23);
            udKeyCode.TabIndex = 13;
            udKeyCode.Value = new decimal(new int[] { 1, 0, 0, 0 });
            lblKeyCodes.Location = new Point(290, 40);
            lblKeyCodes.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblKeyCodes.Name = "lblKeyCodes";
            lblKeyCodes.Size = new Size(80, 23);
            lblKeyCodes.TabIndex = 51;
            lblKeyCodes.Text = "Key codes:";
            lblKeyCodes.TextAlign = ContentAlignment.MiddleLeft;
            chkChangeOnCaps.Location = new Point(290, 10);
            chkChangeOnCaps.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            chkChangeOnCaps.Name = "chkChangeOnCaps";
            chkChangeOnCaps.Size = new Size(265, 23);
            chkChangeOnCaps.TabIndex = 1;
            chkChangeOnCaps.Text = "Change capitalization on Caps Lock key";
            chkChangeOnCaps.UseVisualStyleBackColor = true;
            btnUpdateBoundary.Location = new Point(10, 170);
            btnUpdateBoundary.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnUpdateBoundary.Name = "btnUpdateBoundary";
            btnUpdateBoundary.Size = new Size(80, 23);
            btnUpdateBoundary.TabIndex = 7;
            btnUpdateBoundary.Text = "Update";
            btnUpdateBoundary.UseVisualStyleBackColor = true;
            btnUpdateBoundary.Click += btnUpdateBoundary_Click;
            btnCenterText.Location = new Point(195, 70);
            btnCenterText.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnCenterText.Name = "btnCenterText";
            btnCenterText.Size = new Size(80, 23);
            btnCenterText.TabIndex = 4;
            btnCenterText.Text = "Center";
            btnCenterText.UseVisualStyleBackColor = true;
            btnCenterText.Click += btnCenterText_Click;
            btnRectangle.Location = new Point(10, 305);
            btnRectangle.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnRectangle.Name = "btnRectangle";
            btnRectangle.Size = new Size(80, 23);
            btnRectangle.TabIndex = 11;
            btnRectangle.Text = "Rectangle";
            btnRectangle.UseVisualStyleBackColor = true;
            btnRectangle.Click += btnRectangle_Click;
            btnDetectKeyCode.Location = new Point(286, 130);
            btnDetectKeyCode.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnDetectKeyCode.Name = "btnDetectKeyCode";
            btnDetectKeyCode.Size = new Size(80, 23);
            btnDetectKeyCode.TabIndex = 16;
            btnDetectKeyCode.Text = "Detect";
            btnDetectKeyCode.UseVisualStyleBackColor = true;
            btnDetectKeyCode.Click += btnDetectKeyCode_Click;
            txtBoundaries.Location = new Point(95, 100);
            txtBoundaries.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtBoundaries.MaxVal = int.MaxValue;
            txtBoundaries.Name = "txtBoundaries";
            txtBoundaries.Separator = ';';
            txtBoundaries.Size = new Size(180, 23);
            txtBoundaries.SpacesAroundSeparator = true;
            txtBoundaries.TabIndex = 5;
            txtBoundaries.Text = "0 ; 0";
            txtBoundaries.X = 0;
            txtBoundaries.Y = 0;
            txtTextPosition.Location = new Point(95, 70);
            txtTextPosition.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtTextPosition.MaxVal = int.MaxValue;
            txtTextPosition.Name = "txtTextPosition";
            txtTextPosition.Separator = ';';
            txtTextPosition.Size = new Size(90, 23);
            txtTextPosition.SpacesAroundSeparator = true;
            txtTextPosition.TabIndex = 3;
            txtTextPosition.Text = "0 ; 0";
            txtTextPosition.X = 0;
            txtTextPosition.Y = 0;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = CancelButton2;
            ClientSize = new Size(564, 336);
            Controls.Add(btnUpdateBoundary);
            Controls.Add(btnCenterText);
            Controls.Add(btnRectangle);
            Controls.Add(btnDetectKeyCode);
            Controls.Add(chkChangeOnCaps);
            Controls.Add(lblKeyCodes);
            Controls.Add(udKeyCode);
            Controls.Add(btnRemoveKeyCode);
            Controls.Add(btnAddKeyCode);
            Controls.Add(lstKeyCodes);
            Controls.Add(lblShiftText);
            Controls.Add(txtShiftText);
            Controls.Add(btnBoundaryDown);
            Controls.Add(btnBoundaryUp);
            Controls.Add(btnRemoveBoundary);
            Controls.Add(btnAddBoundary);
            Controls.Add(txtBoundaries);
            Controls.Add(CancelButton2);
            Controls.Add(AcceptButton2);
            Controls.Add(lblBoundaries);
            Controls.Add(lstBoundaries);
            Controls.Add(lblText);
            Controls.Add(txtText);
            Controls.Add(txtTextPosition);
            Controls.Add(lblTextPosition);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "KeyboardKeyPropertiesForm";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Keyboard Key Properties";
            FormClosing += KeyboardKeyPropertiesForm_FormClosing;
            Load += KeyboardKeyPropertiesForm_Load;
            ((System.ComponentModel.ISupportInitialize)udKeyCode).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBoundaryDown;
        private System.Windows.Forms.Button btnBoundaryUp;
        private System.Windows.Forms.Button btnRemoveBoundary;
        private System.Windows.Forms.Button btnAddBoundary;
        private Controls.VectorTextBox txtBoundaries;
        private System.Windows.Forms.Button CancelButton2;
        private System.Windows.Forms.Button AcceptButton2;
        private System.Windows.Forms.Label lblBoundaries;
        private System.Windows.Forms.ListBox lstBoundaries;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.TextBox txtText;
        private Controls.VectorTextBox txtTextPosition;
        private System.Windows.Forms.Label lblTextPosition;
        private System.Windows.Forms.Label lblShiftText;
        private System.Windows.Forms.TextBox txtShiftText;
        private System.Windows.Forms.ListBox lstKeyCodes;
        private System.Windows.Forms.Button btnRemoveKeyCode;
        private System.Windows.Forms.Button btnAddKeyCode;
        private System.Windows.Forms.NumericUpDown udKeyCode;
        private System.Windows.Forms.Label lblKeyCodes;
        private System.Windows.Forms.CheckBox chkChangeOnCaps;
        private System.Windows.Forms.Button btnUpdateBoundary;
        private System.Windows.Forms.Button btnCenterText;
        private System.Windows.Forms.Button btnRectangle;
        private System.Windows.Forms.Button btnDetectKeyCode;
    }
}
