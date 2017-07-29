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
            this.SuspendLayout();
            // 
            // txtTextPosition
            // 
            this.txtTextPosition.Location = new System.Drawing.Point(113, 80);
            this.txtTextPosition.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtTextPosition.MaxVal = 2147483647;
            this.txtTextPosition.Name = "txtTextPosition";
            this.txtTextPosition.Separator = ';';
            this.txtTextPosition.Size = new System.Drawing.Size(207, 22);
            this.txtTextPosition.SpacesAroundSeparator = true;
            this.txtTextPosition.TabIndex = 3;
            this.txtTextPosition.Text = "0 ; 0";
            this.txtTextPosition.X = 0;
            this.txtTextPosition.Y = 0;
            // 
            // lblTextPosition
            // 
            this.lblTextPosition.AutoSize = true;
            this.lblTextPosition.Location = new System.Drawing.Point(11, 84);
            this.lblTextPosition.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTextPosition.Name = "lblTextPosition";
            this.lblTextPosition.Size = new System.Drawing.Size(93, 17);
            this.lblTextPosition.TabIndex = 16;
            this.lblTextPosition.Text = "Text Position:";
            // 
            // txtText
            // 
            this.txtText.Location = new System.Drawing.Point(113, 48);
            this.txtText.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtText.Name = "txtText";
            this.txtText.Size = new System.Drawing.Size(207, 22);
            this.txtText.TabIndex = 2;
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(11, 52);
            this.lblText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(39, 17);
            this.lblText.TabIndex = 19;
            this.lblText.Text = "Text:";
            // 
            // cmbKeyCode
            // 
            this.cmbKeyCode.FormattingEnabled = true;
            this.cmbKeyCode.Location = new System.Drawing.Point(113, 15);
            this.cmbKeyCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbKeyCode.Name = "cmbKeyCode";
            this.cmbKeyCode.Size = new System.Drawing.Size(207, 24);
            this.cmbKeyCode.TabIndex = 1;
            // 
            // lblKeyCode
            // 
            this.lblKeyCode.AutoSize = true;
            this.lblKeyCode.Location = new System.Drawing.Point(11, 18);
            this.lblKeyCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblKeyCode.Name = "lblKeyCode";
            this.lblKeyCode.Size = new System.Drawing.Size(69, 17);
            this.lblKeyCode.TabIndex = 21;
            this.lblKeyCode.Text = "KeyCode:";
            // 
            // lstBoundaries
            // 
            this.lstBoundaries.FormattingEnabled = true;
            this.lstBoundaries.ItemHeight = 16;
            this.lstBoundaries.Location = new System.Drawing.Point(113, 144);
            this.lstBoundaries.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstBoundaries.Name = "lstBoundaries";
            this.lstBoundaries.Size = new System.Drawing.Size(207, 132);
            this.lstBoundaries.TabIndex = 9;
            // 
            // lblBoundaries
            // 
            this.lblBoundaries.AutoSize = true;
            this.lblBoundaries.Location = new System.Drawing.Point(11, 116);
            this.lblBoundaries.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBoundaries.Name = "lblBoundaries";
            this.lblBoundaries.Size = new System.Drawing.Size(84, 17);
            this.lblBoundaries.TabIndex = 23;
            this.lblBoundaries.Text = "Boundaries:";
            // 
            // CancelButton2
            // 
            this.CancelButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton2.Location = new System.Drawing.Point(113, 284);
            this.CancelButton2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CancelButton2.Name = "CancelButton2";
            this.CancelButton2.Size = new System.Drawing.Size(100, 28);
            this.CancelButton2.TabIndex = 10;
            this.CancelButton2.Text = "Cancel";
            this.CancelButton2.UseVisualStyleBackColor = true;
            this.CancelButton2.Click += new System.EventHandler(this.CancelButton2_Click);
            // 
            // AcceptButton2
            // 
            this.AcceptButton2.Location = new System.Drawing.Point(221, 284);
            this.AcceptButton2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.AcceptButton2.Name = "AcceptButton2";
            this.AcceptButton2.Size = new System.Drawing.Size(100, 28);
            this.AcceptButton2.TabIndex = 11;
            this.AcceptButton2.Text = "Accept";
            this.AcceptButton2.UseVisualStyleBackColor = true;
            this.AcceptButton2.Click += new System.EventHandler(this.AcceptButton2_Click);
            // 
            // txtBoundaries
            // 
            this.txtBoundaries.Location = new System.Drawing.Point(113, 112);
            this.txtBoundaries.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBoundaries.MaxVal = 2147483647;
            this.txtBoundaries.Name = "txtBoundaries";
            this.txtBoundaries.Separator = ';';
            this.txtBoundaries.Size = new System.Drawing.Size(207, 22);
            this.txtBoundaries.SpacesAroundSeparator = true;
            this.txtBoundaries.TabIndex = 4;
            this.txtBoundaries.Text = "0 ; 0";
            this.txtBoundaries.X = 0;
            this.txtBoundaries.Y = 0;
            // 
            // btnAddBoundary
            // 
            this.btnAddBoundary.Location = new System.Drawing.Point(5, 144);
            this.btnAddBoundary.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddBoundary.Name = "btnAddBoundary";
            this.btnAddBoundary.Size = new System.Drawing.Size(100, 28);
            this.btnAddBoundary.TabIndex = 5;
            this.btnAddBoundary.Text = "Add";
            this.btnAddBoundary.UseVisualStyleBackColor = true;
            this.btnAddBoundary.Click += new System.EventHandler(this.btnAddBoundary_Click);
            // 
            // btnRemoveBoundary
            // 
            this.btnRemoveBoundary.Location = new System.Drawing.Point(5, 180);
            this.btnRemoveBoundary.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRemoveBoundary.Name = "btnRemoveBoundary";
            this.btnRemoveBoundary.Size = new System.Drawing.Size(100, 28);
            this.btnRemoveBoundary.TabIndex = 6;
            this.btnRemoveBoundary.Text = "Remove";
            this.btnRemoveBoundary.UseVisualStyleBackColor = true;
            this.btnRemoveBoundary.Click += new System.EventHandler(this.btnRemoveBoundary_Click);
            // 
            // btnBoundaryUp
            // 
            this.btnBoundaryUp.Location = new System.Drawing.Point(5, 215);
            this.btnBoundaryUp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBoundaryUp.Name = "btnBoundaryUp";
            this.btnBoundaryUp.Size = new System.Drawing.Size(100, 28);
            this.btnBoundaryUp.TabIndex = 7;
            this.btnBoundaryUp.Text = "Up";
            this.btnBoundaryUp.UseVisualStyleBackColor = true;
            this.btnBoundaryUp.Click += new System.EventHandler(this.btnBoundaryUp_Click);
            // 
            // btnBoundaryDown
            // 
            this.btnBoundaryDown.Location = new System.Drawing.Point(5, 251);
            this.btnBoundaryDown.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBoundaryDown.Name = "btnBoundaryDown";
            this.btnBoundaryDown.Size = new System.Drawing.Size(100, 28);
            this.btnBoundaryDown.TabIndex = 8;
            this.btnBoundaryDown.Text = "Down";
            this.btnBoundaryDown.UseVisualStyleBackColor = true;
            this.btnBoundaryDown.Click += new System.EventHandler(this.btnBoundaryDown_Click);
            // 
            // MouseElementPropertiesForm
            // 
            this.AcceptButton = this.AcceptButton2;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelButton2;
            this.ClientSize = new System.Drawing.Size(337, 326);
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
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MouseElementPropertiesForm";
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
    }
}