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

namespace ThoNohT.NohBoard.Forms.Style
{
    using Controls;

    partial class MouseSpeedStyleForm
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
            ThoNohT.NohBoard.Keyboard.Styles.MouseSpeedIndicatorStyle mouseSpeedIndicatorStyle2 = new ThoNohT.NohBoard.Keyboard.Styles.MouseSpeedIndicatorStyle();
            ThoNohT.NohBoard.Extra.SerializableColor serializableColor3 = new ThoNohT.NohBoard.Extra.SerializableColor();
            ThoNohT.NohBoard.Extra.SerializableColor serializableColor4 = new ThoNohT.NohBoard.Extra.SerializableColor();
            this.AcceptButton2 = new System.Windows.Forms.Button();
            this.CancelButton2 = new System.Windows.Forms.Button();
            this.defaultMouseSpeed = new ThoNohT.NohBoard.Controls.MouseSpeedStylePanel();
            this.chkOverwrite = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // AcceptButton2
            // 
            this.AcceptButton2.Location = new System.Drawing.Point(108, 185);
            this.AcceptButton2.Name = "AcceptButton2";
            this.AcceptButton2.Size = new System.Drawing.Size(75, 23);
            this.AcceptButton2.TabIndex = 10;
            this.AcceptButton2.Text = "Accept";
            this.AcceptButton2.UseVisualStyleBackColor = true;
            this.AcceptButton2.Click += new System.EventHandler(this.AcceptButton2_Click);
            // 
            // CancelButton2
            // 
            this.CancelButton2.Location = new System.Drawing.Point(27, 185);
            this.CancelButton2.Name = "CancelButton2";
            this.CancelButton2.Size = new System.Drawing.Size(75, 23);
            this.CancelButton2.TabIndex = 11;
            this.CancelButton2.Text = "Cancel";
            this.CancelButton2.UseVisualStyleBackColor = true;
            this.CancelButton2.Click += new System.EventHandler(this.CancelButton2_Click);
            // 
            // defaultMouseSpeed
            // 
            serializableColor3.Blue = ((byte)(0));
            serializableColor3.Green = ((byte)(0));
            serializableColor3.Red = ((byte)(0));
            mouseSpeedIndicatorStyle2.InnerColor = serializableColor3;
            serializableColor4.Blue = ((byte)(0));
            serializableColor4.Green = ((byte)(0));
            serializableColor4.Red = ((byte)(0));
            mouseSpeedIndicatorStyle2.OuterColor = serializableColor4;
            mouseSpeedIndicatorStyle2.OutlineWidth = 1;
            this.defaultMouseSpeed.IndicatorStyle = mouseSpeedIndicatorStyle2;
            this.defaultMouseSpeed.Location = new System.Drawing.Point(12, 12);
            this.defaultMouseSpeed.Name = "defaultMouseSpeed";
            this.defaultMouseSpeed.Size = new System.Drawing.Size(171, 144);
            this.defaultMouseSpeed.TabIndex = 16;
            this.defaultMouseSpeed.Title = "Mouse Speed";
            // 
            // chkOverwrite
            // 
            this.chkOverwrite.AutoSize = true;
            this.chkOverwrite.Location = new System.Drawing.Point(12, 162);
            this.chkOverwrite.Name = "chkOverwrite";
            this.chkOverwrite.Size = new System.Drawing.Size(130, 17);
            this.chkOverwrite.TabIndex = 17;
            this.chkOverwrite.Text = "Overwrite default style";
            this.chkOverwrite.UseVisualStyleBackColor = true;
            this.chkOverwrite.CheckedChanged += new System.EventHandler(this.chkOverwrite_CheckedChanged);
            // 
            // MouseSpeedStyleForm
            // 
            this.AcceptButton = this.AcceptButton2;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelButton2;
            this.ClientSize = new System.Drawing.Size(197, 219);
            this.Controls.Add(this.chkOverwrite);
            this.Controls.Add(this.defaultMouseSpeed);
            this.Controls.Add(this.CancelButton2);
            this.Controls.Add(this.AcceptButton2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MouseSpeedStyleForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Mouse Speed Indicator Style";
            this.Load += new System.EventHandler(this.MouseSpeedStyleForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button AcceptButton2;
        private System.Windows.Forms.Button CancelButton2;
        private MouseSpeedStylePanel defaultMouseSpeed;
        private System.Windows.Forms.CheckBox chkOverwrite;
    }
}