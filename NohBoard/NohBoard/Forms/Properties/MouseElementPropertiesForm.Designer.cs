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
    partial class MouseElementPropertiesForm
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
            this.txtTextPosition = new ThoNohT.NohBoard.Controls.VectorTextBox();
            this.lblTextPosition = new System.Windows.Forms.Label();
            this.txtText = new System.Windows.Forms.TextBox();
            this.lblText = new System.Windows.Forms.Label();
            this.cmbKeyCode = new System.Windows.Forms.ComboBox();
            this.lblKeyCode = new System.Windows.Forms.Label();
            this.lstBoundaries = new System.Windows.Forms.ListBox();
            this.lblBoundaries = new System.Windows.Forms.Label();
            this.CancelButton2 = new System.Windows.Forms.Button();
            this.AcceptButton2 = new System.Windows.Forms.Button();
            this.txtBoundaries = new ThoNohT.NohBoard.Controls.VectorTextBox();
            this.btnAddBoundary = new System.Windows.Forms.Button();
            this.btnRemoveBoundary = new System.Windows.Forms.Button();
            this.btnBoundaryUp = new System.Windows.Forms.Button();
            this.btnBoundaryDown = new System.Windows.Forms.Button();
            this.btnUpdateBoundary = new System.Windows.Forms.Button();
            this.btnCenterText = new System.Windows.Forms.Button();
            this.btnRectangle = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtTextPosition
            // 
            this.txtTextPosition.Location = new System.Drawing.Point(85, 65);
            this.txtTextPosition.MaxVal = 2147483647;
            this.txtTextPosition.Name = "txtTextPosition";
            this.txtTextPosition.Separator = ';';
            this.txtTextPosition.Size = new System.Drawing.Size(77, 20);
            this.txtTextPosition.SpacesAroundSeparator = true;
            this.txtTextPosition.TabIndex = 2;
            this.txtTextPosition.Text = "0 ; 0";
            this.txtTextPosition.X = 0;
            this.txtTextPosition.Y = 0;
            // 
            // lblTextPosition
            // 
            this.lblTextPosition.AutoSize = true;
            this.lblTextPosition.Location = new System.Drawing.Point(8, 68);
            this.lblTextPosition.Name = "lblTextPosition";
            this.lblTextPosition.Size = new System.Drawing.Size(71, 13);
            this.lblTextPosition.TabIndex = 16;
            this.lblTextPosition.Text = "Text Position:";
            // 
            // txtText
            // 
            this.txtText.Location = new System.Drawing.Point(85, 39);
            this.txtText.Name = "txtText";
            this.txtText.Size = new System.Drawing.Size(156, 20);
            this.txtText.TabIndex = 1;
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(8, 42);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(31, 13);
            this.lblText.TabIndex = 19;
            this.lblText.Text = "Text:";
            // 
            // cmbKeyCode
            // 
            this.cmbKeyCode.FormattingEnabled = true;
            this.cmbKeyCode.Location = new System.Drawing.Point(85, 12);
            this.cmbKeyCode.Name = "cmbKeyCode";
            this.cmbKeyCode.Size = new System.Drawing.Size(156, 21);
            this.cmbKeyCode.TabIndex = 0;
            // 
            // lblKeyCode
            // 
            this.lblKeyCode.AutoSize = true;
            this.lblKeyCode.Location = new System.Drawing.Point(8, 15);
            this.lblKeyCode.Name = "lblKeyCode";
            this.lblKeyCode.Size = new System.Drawing.Size(53, 13);
            this.lblKeyCode.TabIndex = 21;
            this.lblKeyCode.Text = "KeyCode:";
            // 
            // lstBoundaries
            // 
            this.lstBoundaries.FormattingEnabled = true;
            this.lstBoundaries.Location = new System.Drawing.Point(85, 117);
            this.lstBoundaries.Name = "lstBoundaries";
            this.lstBoundaries.Size = new System.Drawing.Size(156, 134);
            this.lstBoundaries.TabIndex = 10;
            // 
            // lblBoundaries
            // 
            this.lblBoundaries.AutoSize = true;
            this.lblBoundaries.Location = new System.Drawing.Point(8, 94);
            this.lblBoundaries.Name = "lblBoundaries";
            this.lblBoundaries.Size = new System.Drawing.Size(63, 13);
            this.lblBoundaries.TabIndex = 23;
            this.lblBoundaries.Text = "Boundaries:";
            // 
            // CancelButton2
            // 
            this.CancelButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton2.Location = new System.Drawing.Point(85, 262);
            this.CancelButton2.Name = "CancelButton2";
            this.CancelButton2.Size = new System.Drawing.Size(75, 23);
            this.CancelButton2.TabIndex = 11;
            this.CancelButton2.Text = "Cancel";
            this.CancelButton2.UseVisualStyleBackColor = true;
            this.CancelButton2.Click += new System.EventHandler(this.CancelButton2_Click);
            // 
            // AcceptButton2
            // 
            this.AcceptButton2.Location = new System.Drawing.Point(166, 262);
            this.AcceptButton2.Name = "AcceptButton2";
            this.AcceptButton2.Size = new System.Drawing.Size(75, 23);
            this.AcceptButton2.TabIndex = 12;
            this.AcceptButton2.Text = "Accept";
            this.AcceptButton2.UseVisualStyleBackColor = true;
            this.AcceptButton2.Click += new System.EventHandler(this.AcceptButton2_Click);
            // 
            // txtBoundaries
            // 
            this.txtBoundaries.Location = new System.Drawing.Point(85, 91);
            this.txtBoundaries.MaxVal = 2147483647;
            this.txtBoundaries.Name = "txtBoundaries";
            this.txtBoundaries.Separator = ';';
            this.txtBoundaries.Size = new System.Drawing.Size(156, 20);
            this.txtBoundaries.SpacesAroundSeparator = true;
            this.txtBoundaries.TabIndex = 3;
            this.txtBoundaries.Text = "0 ; 0";
            this.txtBoundaries.X = 0;
            this.txtBoundaries.Y = 0;
            // 
            // btnAddBoundary
            // 
            this.btnAddBoundary.Location = new System.Drawing.Point(4, 117);
            this.btnAddBoundary.Name = "btnAddBoundary";
            this.btnAddBoundary.Size = new System.Drawing.Size(75, 23);
            this.btnAddBoundary.TabIndex = 4;
            this.btnAddBoundary.Text = "Add";
            this.btnAddBoundary.UseVisualStyleBackColor = true;
            this.btnAddBoundary.Click += new System.EventHandler(this.btnAddBoundary_Click);
            // 
            // btnRemoveBoundary
            // 
            this.btnRemoveBoundary.Location = new System.Drawing.Point(4, 175);
            this.btnRemoveBoundary.Name = "btnRemoveBoundary";
            this.btnRemoveBoundary.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveBoundary.TabIndex = 6;
            this.btnRemoveBoundary.Text = "Remove";
            this.btnRemoveBoundary.UseVisualStyleBackColor = true;
            this.btnRemoveBoundary.Click += new System.EventHandler(this.btnRemoveBoundary_Click);
            // 
            // btnBoundaryUp
            // 
            this.btnBoundaryUp.Location = new System.Drawing.Point(4, 204);
            this.btnBoundaryUp.Name = "btnBoundaryUp";
            this.btnBoundaryUp.Size = new System.Drawing.Size(75, 23);
            this.btnBoundaryUp.TabIndex = 7;
            this.btnBoundaryUp.Text = "Up";
            this.btnBoundaryUp.UseVisualStyleBackColor = true;
            this.btnBoundaryUp.Click += new System.EventHandler(this.btnBoundaryUp_Click);
            // 
            // btnBoundaryDown
            // 
            this.btnBoundaryDown.Location = new System.Drawing.Point(4, 233);
            this.btnBoundaryDown.Name = "btnBoundaryDown";
            this.btnBoundaryDown.Size = new System.Drawing.Size(75, 23);
            this.btnBoundaryDown.TabIndex = 8;
            this.btnBoundaryDown.Text = "Down";
            this.btnBoundaryDown.UseVisualStyleBackColor = true;
            this.btnBoundaryDown.Click += new System.EventHandler(this.btnBoundaryDown_Click);
            // 
            // btnUpdateBoundary
            // 
            this.btnUpdateBoundary.Location = new System.Drawing.Point(4, 146);
            this.btnUpdateBoundary.Name = "btnUpdateBoundary";
            this.btnUpdateBoundary.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateBoundary.TabIndex = 5;
            this.btnUpdateBoundary.Text = "Update";
            this.btnUpdateBoundary.UseVisualStyleBackColor = true;
            this.btnUpdateBoundary.Click += new System.EventHandler(this.btnUpdateBoundary_Click);
            // 
            // btnCenterText
            // 
            this.btnCenterText.Location = new System.Drawing.Point(168, 63);
            this.btnCenterText.Name = "btnCenterText";
            this.btnCenterText.Size = new System.Drawing.Size(73, 23);
            this.btnCenterText.TabIndex = 3;
            this.btnCenterText.Text = "Center";
            this.btnCenterText.UseVisualStyleBackColor = true;
            this.btnCenterText.Click += new System.EventHandler(this.btnCenterText_Click);
            // 
            // btnRectangle
            // 
            this.btnRectangle.Location = new System.Drawing.Point(4, 262);
            this.btnRectangle.Name = "btnRectangle";
            this.btnRectangle.Size = new System.Drawing.Size(75, 23);
            this.btnRectangle.TabIndex = 9;
            this.btnRectangle.Text = "Rectangle";
            this.btnRectangle.UseVisualStyleBackColor = true;
            this.btnRectangle.Click += new System.EventHandler(this.btnRectangle_Click);
            // 
            // MouseElementPropertiesForm
            // 
            this.AcceptButton = this.AcceptButton2;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelButton2;
            this.ClientSize = new System.Drawing.Size(253, 292);
            this.Controls.Add(this.btnUpdateBoundary);
            this.Controls.Add(this.btnCenterText);
            this.Controls.Add(this.btnRectangle);
            this.Controls.Add(this.btnBoundaryDown);
            this.Controls.Add(this.btnBoundaryUp);
            this.Controls.Add(this.btnRemoveBoundary);
            this.Controls.Add(this.btnAddBoundary);
            this.Controls.Add(this.txtBoundaries);
            this.Controls.Add(this.CancelButton2);
            this.Controls.Add(this.AcceptButton2);
            this.Controls.Add(this.lblBoundaries);
            this.Controls.Add(this.lstBoundaries);
            this.Controls.Add(this.lblKeyCode);
            this.Controls.Add(this.cmbKeyCode);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.txtText);
            this.Controls.Add(this.txtTextPosition);
            this.Controls.Add(this.lblTextPosition);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MouseElementPropertiesForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MouseElementPropertiesForm";
            this.Load += new System.EventHandler(this.MouseElementPropertiesForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

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