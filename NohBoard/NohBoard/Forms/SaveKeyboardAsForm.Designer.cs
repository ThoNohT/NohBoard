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

    partial class SaveKeyboardAsForm
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
            lblCategory = new System.Windows.Forms.Label();
            lblName = new System.Windows.Forms.Label();
            CategoryCombo = new System.Windows.Forms.ComboBox();
            DefinitionCombo = new System.Windows.Forms.ComboBox();
            SaveButton = new System.Windows.Forms.Button();
            CancelButton2 = new System.Windows.Forms.Button();
            SuspendLayout();
            lblCategory.Location = new Point(10, 10);
            lblCategory.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new Size(70, 23);
            lblCategory.TabIndex = 0;
            lblCategory.Text = "Category:";
            lblCategory.TextAlign = ContentAlignment.MiddleLeft;
            lblName.Location = new Point(10, 40);
            lblName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblName.Name = "lblName";
            lblName.Size = new Size(70, 23);
            lblName.TabIndex = 1;
            lblName.Text = "Name:";
            lblName.TextAlign = ContentAlignment.MiddleLeft;
            CategoryCombo.FormattingEnabled = true;
            CategoryCombo.Location = new Point(85, 10);
            CategoryCombo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CategoryCombo.Name = "CategoryCombo";
            CategoryCombo.Size = new Size(180, 23);
            CategoryCombo.TabIndex = 2;
            CategoryCombo.TextChanged += CategoryCombo_TextChanged;
            DefinitionCombo.FormattingEnabled = true;
            DefinitionCombo.Location = new Point(85, 40);
            DefinitionCombo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            DefinitionCombo.Name = "DefinitionCombo";
            DefinitionCombo.Size = new Size(180, 23);
            DefinitionCombo.TabIndex = 3;
            SaveButton.Location = new Point(185, 70);
            SaveButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(80, 23);
            SaveButton.TabIndex = 4;
            SaveButton.Text = "Save";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += SaveButton_Click;
            CancelButton2.Location = new Point(100, 70);
            CancelButton2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CancelButton2.Name = "CancelButton2";
            CancelButton2.Size = new Size(80, 23);
            CancelButton2.TabIndex = 5;
            CancelButton2.Text = "Cancel";
            CancelButton2.UseVisualStyleBackColor = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = CancelButton2;
            ClientSize = new Size(274, 101);
            Controls.Add(CancelButton2);
            Controls.Add(SaveButton);
            Controls.Add(DefinitionCombo);
            Controls.Add(CategoryCombo);
            Controls.Add(lblName);
            Controls.Add(lblCategory);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "SaveKeyboardAsForm";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Save Keyboard Definition";
            Load += SaveKeyboardAsForm_Load;
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.ComboBox CategoryCombo;
        private System.Windows.Forms.ComboBox DefinitionCombo;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button CancelButton2;
    }
}
