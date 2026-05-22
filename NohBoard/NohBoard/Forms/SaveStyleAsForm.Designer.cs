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
    using System.Drawing;

    partial class SaveStyleAsForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            CancelButton2 = new System.Windows.Forms.Button();
            SaveButton = new System.Windows.Forms.Button();
            StyleCombo = new System.Windows.Forms.ComboBox();
            lblName = new System.Windows.Forms.Label();
            chkGlobal = new System.Windows.Forms.CheckBox();
            SuspendLayout();
            CancelButton2.Location = new Point(95, 70);
            CancelButton2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CancelButton2.Name = "CancelButton2";
            CancelButton2.Size = new Size(80, 23);
            CancelButton2.TabIndex = 9;
            CancelButton2.Text = "Cancel";
            CancelButton2.UseVisualStyleBackColor = true;
            SaveButton.Location = new Point(185, 70);
            SaveButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(80, 23);
            SaveButton.TabIndex = 8;
            SaveButton.Text = "Save";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += SaveButton_Click;
            StyleCombo.FormattingEnabled = true;
            StyleCombo.Location = new Point(85, 10);
            StyleCombo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            StyleCombo.Name = "StyleCombo";
            StyleCombo.Size = new Size(180, 23);
            StyleCombo.TabIndex = 7;
            lblName.Location = new Point(10, 10);
            lblName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblName.Name = "lblName";
            lblName.Size = new Size(70, 23);
            lblName.TabIndex = 6;
            lblName.Text = "Name:";
            lblName.TextAlign = ContentAlignment.MiddleLeft;
            chkGlobal.Location = new Point(10, 40);
            chkGlobal.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            chkGlobal.Name = "chkGlobal";
            chkGlobal.Size = new Size(255, 23);
            chkGlobal.TabIndex = 10;
            chkGlobal.Text = "Save as global style";
            chkGlobal.UseVisualStyleBackColor = true;
            chkGlobal.CheckedChanged += chkGlobal_CheckedChanged;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = CancelButton2;
            ClientSize = new Size(274, 101);
            Controls.Add(chkGlobal);
            Controls.Add(CancelButton2);
            Controls.Add(SaveButton);
            Controls.Add(StyleCombo);
            Controls.Add(lblName);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "SaveStyleAsForm";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Save Keyboard Style";
            Load += SaveStyleAsForm_Load;
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CancelButton2;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.ComboBox StyleCombo;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.CheckBox chkGlobal;
    }
}
