/*
Copyright (C) 2016 by Eric Bataille <e.c.p.bataille@gmail.com>

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

namespace ThoNohT.NohBoard.Forms
{
    partial class SaveStyleAsForm
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
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.SaveButton = new System.Windows.Forms.Button();
            this.StyleCombo = new System.Windows.Forms.ComboBox();
            this.lblName = new System.Windows.Forms.Label();
            this.chkGlobal = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // CancelButton2
            // 
            this.CancelButton2.Location = new System.Drawing.Point(87, 62);
            this.CancelButton2.Name = "CancelButton2";
            this.CancelButton2.Size = new System.Drawing.Size(75, 23);
            this.CancelButton2.TabIndex = 9;
            this.CancelButton2.Text = "Cancel";
            this.CancelButton2.UseVisualStyleBackColor = true;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(168, 62);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 8;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // StyleCombo
            // 
            this.StyleCombo.FormattingEnabled = true;
            this.StyleCombo.Location = new System.Drawing.Point(73, 12);
            this.StyleCombo.Name = "StyleCombo";
            this.StyleCombo.Size = new System.Drawing.Size(170, 21);
            this.StyleCombo.TabIndex = 7;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(9, 15);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 6;
            this.lblName.Text = "Name:";
            // 
            // chkGlobal
            // 
            this.chkGlobal.AutoSize = true;
            this.chkGlobal.Location = new System.Drawing.Point(12, 39);
            this.chkGlobal.Name = "chkGlobal";
            this.chkGlobal.Size = new System.Drawing.Size(120, 17);
            this.chkGlobal.TabIndex = 10;
            this.chkGlobal.Text = "Save as global style";
            this.chkGlobal.UseVisualStyleBackColor = true;
            this.chkGlobal.CheckedChanged += new System.EventHandler(this.chkGlobal_CheckedChanged);
            // 
            // SaveStyleAsForm
            // 
            this.AcceptButton = this.SaveButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelButton2;
            this.ClientSize = new System.Drawing.Size(255, 94);
            this.Controls.Add(this.chkGlobal);
            this.Controls.Add(this.CancelButton2);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.StyleCombo);
            this.Controls.Add(this.lblName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SaveStyleAsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Save Keyboard Style";
            this.Load += new System.EventHandler(this.SaveStyleAsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CancelButton2;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.ComboBox StyleCombo;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.CheckBox chkGlobal;
    }
}