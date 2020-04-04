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
    partial class KeyboardKeyPropertiesForm
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
            this.btnBoundaryDown = new System.Windows.Forms.Button();
            this.btnBoundaryUp = new System.Windows.Forms.Button();
            this.btnRemoveBoundary = new System.Windows.Forms.Button();
            this.btnAddBoundary = new System.Windows.Forms.Button();
            this.CancelButton2 = new System.Windows.Forms.Button();
            this.AcceptButton2 = new System.Windows.Forms.Button();
            this.lblBoundaries = new System.Windows.Forms.Label();
            this.lstBoundaries = new System.Windows.Forms.ListBox();
            this.lblText = new System.Windows.Forms.Label();
            this.txtText = new System.Windows.Forms.TextBox();
            this.lblTextPosition = new System.Windows.Forms.Label();
            this.lblShiftText = new System.Windows.Forms.Label();
            this.txtShiftText = new System.Windows.Forms.TextBox();
            this.lstKeyCodes = new System.Windows.Forms.ListBox();
            this.btnRemoveKeyCode = new System.Windows.Forms.Button();
            this.btnAddKeyCode = new System.Windows.Forms.Button();
            this.udKeyCode = new System.Windows.Forms.NumericUpDown();
            this.lblKeyCodes = new System.Windows.Forms.Label();
            this.chkChangeOnCaps = new System.Windows.Forms.CheckBox();
            this.btnUpdateBoundary = new System.Windows.Forms.Button();
            this.btnCenterText = new System.Windows.Forms.Button();
            this.btnRectangle = new System.Windows.Forms.Button();
            this.btnDetectKeyCode = new System.Windows.Forms.Button();
            this.txtBoundaries = new ThoNohT.NohBoard.Controls.VectorTextBox();
            this.txtTextPosition = new ThoNohT.NohBoard.Controls.VectorTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.udKeyCode)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBoundaryDown
            // 
            this.btnBoundaryDown.Location = new System.Drawing.Point(4, 232);
            this.btnBoundaryDown.Name = "btnBoundaryDown";
            this.btnBoundaryDown.Size = new System.Drawing.Size(75, 23);
            this.btnBoundaryDown.TabIndex = 10;
            this.btnBoundaryDown.Text = "Down";
            this.btnBoundaryDown.UseVisualStyleBackColor = true;
            this.btnBoundaryDown.Click += new System.EventHandler(this.btnBoundaryDown_Click);
            // 
            // btnBoundaryUp
            // 
            this.btnBoundaryUp.Location = new System.Drawing.Point(4, 203);
            this.btnBoundaryUp.Name = "btnBoundaryUp";
            this.btnBoundaryUp.Size = new System.Drawing.Size(75, 23);
            this.btnBoundaryUp.TabIndex = 9;
            this.btnBoundaryUp.Text = "Up";
            this.btnBoundaryUp.UseVisualStyleBackColor = true;
            this.btnBoundaryUp.Click += new System.EventHandler(this.btnBoundaryUp_Click);
            // 
            // btnRemoveBoundary
            // 
            this.btnRemoveBoundary.Location = new System.Drawing.Point(4, 174);
            this.btnRemoveBoundary.Name = "btnRemoveBoundary";
            this.btnRemoveBoundary.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveBoundary.TabIndex = 8;
            this.btnRemoveBoundary.Text = "Remove";
            this.btnRemoveBoundary.UseVisualStyleBackColor = true;
            this.btnRemoveBoundary.Click += new System.EventHandler(this.btnRemoveBoundary_Click);
            // 
            // btnAddBoundary
            // 
            this.btnAddBoundary.Location = new System.Drawing.Point(4, 116);
            this.btnAddBoundary.Name = "btnAddBoundary";
            this.btnAddBoundary.Size = new System.Drawing.Size(75, 23);
            this.btnAddBoundary.TabIndex = 6;
            this.btnAddBoundary.Text = "Add";
            this.btnAddBoundary.UseVisualStyleBackColor = true;
            this.btnAddBoundary.Click += new System.EventHandler(this.btnAddBoundary_Click);
            // 
            // CancelButton2
            // 
            this.CancelButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton2.Location = new System.Drawing.Point(328, 261);
            this.CancelButton2.Name = "CancelButton2";
            this.CancelButton2.Size = new System.Drawing.Size(75, 23);
            this.CancelButton2.TabIndex = 18;
            this.CancelButton2.Text = "Cancel";
            this.CancelButton2.UseVisualStyleBackColor = true;
            this.CancelButton2.Click += new System.EventHandler(this.CancelButton2_Click);
            // 
            // AcceptButton2
            // 
            this.AcceptButton2.Location = new System.Drawing.Point(405, 261);
            this.AcceptButton2.Name = "AcceptButton2";
            this.AcceptButton2.Size = new System.Drawing.Size(75, 23);
            this.AcceptButton2.TabIndex = 19;
            this.AcceptButton2.Text = "Accept";
            this.AcceptButton2.UseVisualStyleBackColor = true;
            this.AcceptButton2.Click += new System.EventHandler(this.AcceptButton2_Click);
            // 
            // lblBoundaries
            // 
            this.lblBoundaries.AutoSize = true;
            this.lblBoundaries.Location = new System.Drawing.Point(8, 93);
            this.lblBoundaries.Name = "lblBoundaries";
            this.lblBoundaries.Size = new System.Drawing.Size(63, 13);
            this.lblBoundaries.TabIndex = 36;
            this.lblBoundaries.Text = "Boundaries:";
            // 
            // lstBoundaries
            // 
            this.lstBoundaries.FormattingEnabled = true;
            this.lstBoundaries.Location = new System.Drawing.Point(85, 116);
            this.lstBoundaries.Name = "lstBoundaries";
            this.lstBoundaries.Size = new System.Drawing.Size(156, 160);
            this.lstBoundaries.TabIndex = 12;
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(8, 14);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(31, 13);
            this.lblText.TabIndex = 34;
            this.lblText.Text = "Text:";
            // 
            // txtText
            // 
            this.txtText.Location = new System.Drawing.Point(85, 11);
            this.txtText.Name = "txtText";
            this.txtText.Size = new System.Drawing.Size(156, 20);
            this.txtText.TabIndex = 0;
            // 
            // lblTextPosition
            // 
            this.lblTextPosition.AutoSize = true;
            this.lblTextPosition.Location = new System.Drawing.Point(8, 67);
            this.lblTextPosition.Name = "lblTextPosition";
            this.lblTextPosition.Size = new System.Drawing.Size(71, 13);
            this.lblTextPosition.TabIndex = 31;
            this.lblTextPosition.Text = "Text Position:";
            // 
            // lblShiftText
            // 
            this.lblShiftText.AutoSize = true;
            this.lblShiftText.Location = new System.Drawing.Point(8, 40);
            this.lblShiftText.Name = "lblShiftText";
            this.lblShiftText.Size = new System.Drawing.Size(55, 13);
            this.lblShiftText.TabIndex = 45;
            this.lblShiftText.Text = "Shift Text:";
            // 
            // txtShiftText
            // 
            this.txtShiftText.Location = new System.Drawing.Point(85, 37);
            this.txtShiftText.Name = "txtShiftText";
            this.txtShiftText.Size = new System.Drawing.Size(156, 20);
            this.txtShiftText.TabIndex = 2;
            // 
            // lstKeyCodes
            // 
            this.lstKeyCodes.FormattingEnabled = true;
            this.lstKeyCodes.Location = new System.Drawing.Point(328, 64);
            this.lstKeyCodes.Name = "lstKeyCodes";
            this.lstKeyCodes.Size = new System.Drawing.Size(151, 186);
            this.lstKeyCodes.TabIndex = 17;
            // 
            // btnRemoveKeyCode
            // 
            this.btnRemoveKeyCode.Location = new System.Drawing.Point(247, 93);
            this.btnRemoveKeyCode.Name = "btnRemoveKeyCode";
            this.btnRemoveKeyCode.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveKeyCode.TabIndex = 15;
            this.btnRemoveKeyCode.Text = "Remove";
            this.btnRemoveKeyCode.UseVisualStyleBackColor = true;
            this.btnRemoveKeyCode.Click += new System.EventHandler(this.btnRemoveKeyCode_Click);
            // 
            // btnAddKeyCode
            // 
            this.btnAddKeyCode.Location = new System.Drawing.Point(247, 64);
            this.btnAddKeyCode.Name = "btnAddKeyCode";
            this.btnAddKeyCode.Size = new System.Drawing.Size(75, 23);
            this.btnAddKeyCode.TabIndex = 14;
            this.btnAddKeyCode.Text = "Add";
            this.btnAddKeyCode.UseVisualStyleBackColor = true;
            this.btnAddKeyCode.Click += new System.EventHandler(this.btnAddKeyCode_Click);
            // 
            // udKeyCode
            // 
            this.udKeyCode.Location = new System.Drawing.Point(329, 38);
            this.udKeyCode.Maximum = new decimal(new int[] {
            1028,
            0,
            0,
            0});
            this.udKeyCode.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udKeyCode.Name = "udKeyCode";
            this.udKeyCode.Size = new System.Drawing.Size(151, 20);
            this.udKeyCode.TabIndex = 13;
            this.udKeyCode.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblKeyCodes
            // 
            this.lblKeyCodes.AutoSize = true;
            this.lblKeyCodes.Location = new System.Drawing.Point(247, 40);
            this.lblKeyCodes.Name = "lblKeyCodes";
            this.lblKeyCodes.Size = new System.Drawing.Size(60, 13);
            this.lblKeyCodes.TabIndex = 51;
            this.lblKeyCodes.Text = "Key codes:";
            // 
            // chkChangeOnCaps
            // 
            this.chkChangeOnCaps.AutoSize = true;
            this.chkChangeOnCaps.Location = new System.Drawing.Point(250, 9);
            this.chkChangeOnCaps.Name = "chkChangeOnCaps";
            this.chkChangeOnCaps.Size = new System.Drawing.Size(216, 17);
            this.chkChangeOnCaps.TabIndex = 1;
            this.chkChangeOnCaps.Text = "Change capitalization on Caps Lock key";
            this.chkChangeOnCaps.UseVisualStyleBackColor = true;
            // 
            // btnUpdateBoundary
            // 
            this.btnUpdateBoundary.Location = new System.Drawing.Point(4, 145);
            this.btnUpdateBoundary.Name = "btnUpdateBoundary";
            this.btnUpdateBoundary.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateBoundary.TabIndex = 7;
            this.btnUpdateBoundary.Text = "Update";
            this.btnUpdateBoundary.UseVisualStyleBackColor = true;
            this.btnUpdateBoundary.Click += new System.EventHandler(this.btnUpdateBoundary_Click);
            // 
            // btnCenterText
            // 
            this.btnCenterText.Location = new System.Drawing.Point(168, 62);
            this.btnCenterText.Name = "btnCenterText";
            this.btnCenterText.Size = new System.Drawing.Size(73, 23);
            this.btnCenterText.TabIndex = 4;
            this.btnCenterText.Text = "Center";
            this.btnCenterText.UseVisualStyleBackColor = true;
            this.btnCenterText.Click += new System.EventHandler(this.btnCenterText_Click);
            // 
            // btnRectangle
            // 
            this.btnRectangle.Location = new System.Drawing.Point(4, 261);
            this.btnRectangle.Name = "btnRectangle";
            this.btnRectangle.Size = new System.Drawing.Size(75, 23);
            this.btnRectangle.TabIndex = 11;
            this.btnRectangle.Text = "Rectangle";
            this.btnRectangle.UseVisualStyleBackColor = true;
            this.btnRectangle.Click += new System.EventHandler(this.btnRectangle_Click);
            // 
            // btnDetectKeyCode
            // 
            this.btnDetectKeyCode.Location = new System.Drawing.Point(247, 122);
            this.btnDetectKeyCode.Name = "btnDetectKeyCode";
            this.btnDetectKeyCode.Size = new System.Drawing.Size(75, 23);
            this.btnDetectKeyCode.TabIndex = 16;
            this.btnDetectKeyCode.Text = "Detect";
            this.btnDetectKeyCode.UseVisualStyleBackColor = true;
            this.btnDetectKeyCode.Click += new System.EventHandler(this.btnDetectKeyCode_Click);
            // 
            // txtBoundaries
            // 
            this.txtBoundaries.Location = new System.Drawing.Point(85, 90);
            this.txtBoundaries.MaxVal = 2147483647;
            this.txtBoundaries.Name = "txtBoundaries";
            this.txtBoundaries.Separator = ';';
            this.txtBoundaries.Size = new System.Drawing.Size(156, 20);
            this.txtBoundaries.SpacesAroundSeparator = true;
            this.txtBoundaries.TabIndex = 5;
            this.txtBoundaries.Text = "0 ; 0";
            this.txtBoundaries.X = 0;
            this.txtBoundaries.Y = 0;
            // 
            // txtTextPosition
            // 
            this.txtTextPosition.Location = new System.Drawing.Point(85, 64);
            this.txtTextPosition.MaxVal = 2147483647;
            this.txtTextPosition.Name = "txtTextPosition";
            this.txtTextPosition.Separator = ';';
            this.txtTextPosition.Size = new System.Drawing.Size(156, 20);
            this.txtTextPosition.SpacesAroundSeparator = true;
            this.txtTextPosition.TabIndex = 3;
            this.txtTextPosition.Text = "0 ; 0";
            this.txtTextPosition.X = 0;
            this.txtTextPosition.Y = 0;
            // 
            // KeyboardKeyPropertiesForm
            // 
            this.AcceptButton = this.AcceptButton2;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelButton2;
            this.ClientSize = new System.Drawing.Size(492, 293);
            this.Controls.Add(this.btnUpdateBoundary);
            this.Controls.Add(this.btnCenterText);
            this.Controls.Add(this.btnRectangle);
            this.Controls.Add(this.btnDetectKeyCode);
            this.Controls.Add(this.chkChangeOnCaps);
            this.Controls.Add(this.lblKeyCodes);
            this.Controls.Add(this.udKeyCode);
            this.Controls.Add(this.btnRemoveKeyCode);
            this.Controls.Add(this.btnAddKeyCode);
            this.Controls.Add(this.lstKeyCodes);
            this.Controls.Add(this.lblShiftText);
            this.Controls.Add(this.txtShiftText);
            this.Controls.Add(this.btnBoundaryDown);
            this.Controls.Add(this.btnBoundaryUp);
            this.Controls.Add(this.btnRemoveBoundary);
            this.Controls.Add(this.btnAddBoundary);
            this.Controls.Add(this.txtBoundaries);
            this.Controls.Add(this.CancelButton2);
            this.Controls.Add(this.AcceptButton2);
            this.Controls.Add(this.lblBoundaries);
            this.Controls.Add(this.lstBoundaries);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.txtText);
            this.Controls.Add(this.txtTextPosition);
            this.Controls.Add(this.lblTextPosition);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "KeyboardKeyPropertiesForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Keyboard Key Properties";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.KeyboardKeyPropertiesForm_FormClosing);
            this.Load += new System.EventHandler(this.KeyboardKeyPropertiesForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.udKeyCode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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