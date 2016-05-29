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
    partial class KeyboardStyleForm
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
            this.LooseGroup = new System.Windows.Forms.GroupBox();
            this.chkUnpressedOutline = new System.Windows.Forms.CheckBox();
            this.clrUnpressedOutline = new ThoNohT.NohBoard.Forms.ColorChooser();
            this.clrUnpressedText = new ThoNohT.NohBoard.Forms.ColorChooser();
            this.clrUnpressedBackground = new ThoNohT.NohBoard.Forms.ColorChooser();
            this.PressedGroup = new System.Windows.Forms.GroupBox();
            this.chkPressedOutline = new System.Windows.Forms.CheckBox();
            this.clrPressedBackground = new ThoNohT.NohBoard.Forms.ColorChooser();
            this.clrPressedOutline = new ThoNohT.NohBoard.Forms.ColorChooser();
            this.clrPressedText = new ThoNohT.NohBoard.Forms.ColorChooser();
            this.MouseSpeedGroup = new System.Windows.Forms.GroupBox();
            this.clrMouseSpeedLow = new ThoNohT.NohBoard.Forms.ColorChooser();
            this.clrMouseSpeedHigh = new ThoNohT.NohBoard.Forms.ColorChooser();
            this.KeyboardGroup = new System.Windows.Forms.GroupBox();
            this.clrKeyboardBackground = new ThoNohT.NohBoard.Forms.ColorChooser();
            this.AcceptButton2 = new System.Windows.Forms.Button();
            this.CancelButton2 = new System.Windows.Forms.Button();
            this.LooseGroup.SuspendLayout();
            this.PressedGroup.SuspendLayout();
            this.MouseSpeedGroup.SuspendLayout();
            this.KeyboardGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // LooseGroup
            // 
            this.LooseGroup.Controls.Add(this.chkUnpressedOutline);
            this.LooseGroup.Controls.Add(this.clrUnpressedOutline);
            this.LooseGroup.Controls.Add(this.clrUnpressedText);
            this.LooseGroup.Controls.Add(this.clrUnpressedBackground);
            this.LooseGroup.Location = new System.Drawing.Point(171, 12);
            this.LooseGroup.Name = "LooseGroup";
            this.LooseGroup.Size = new System.Drawing.Size(153, 142);
            this.LooseGroup.TabIndex = 0;
            this.LooseGroup.TabStop = false;
            this.LooseGroup.Text = "Unpressed keys";
            // 
            // chkUnpressedOutline
            // 
            this.chkUnpressedOutline.AutoSize = true;
            this.chkUnpressedOutline.Location = new System.Drawing.Point(7, 117);
            this.chkUnpressedOutline.Name = "chkUnpressedOutline";
            this.chkUnpressedOutline.Size = new System.Drawing.Size(89, 17);
            this.chkUnpressedOutline.TabIndex = 3;
            this.chkUnpressedOutline.Text = "Show Outline";
            this.chkUnpressedOutline.UseVisualStyleBackColor = true;
            this.chkUnpressedOutline.CheckedChanged += new System.EventHandler(this.chkUnpressedOutline_CheckedChanged);
            // 
            // clrUnpressedOutline
            // 
            this.clrUnpressedOutline.BackColor = System.Drawing.SystemColors.Control;
            this.clrUnpressedOutline.Color = System.Drawing.Color.Lime;
            this.clrUnpressedOutline.LabelText = "Outline Color";
            this.clrUnpressedOutline.Location = new System.Drawing.Point(6, 84);
            this.clrUnpressedOutline.Name = "clrUnpressedOutline";
            this.clrUnpressedOutline.PreviewShape = ThoNohT.NohBoard.Forms.ColorChooser.Shape.Square;
            this.clrUnpressedOutline.Size = new System.Drawing.Size(141, 26);
            this.clrUnpressedOutline.TabIndex = 2;
            this.clrUnpressedOutline.ColorChanged += new ThoNohT.NohBoard.Forms.ColorChooser.ColorChangedEventHandler(this.Control_ColorChanged);
            // 
            // clrUnpressedText
            // 
            this.clrUnpressedText.BackColor = System.Drawing.SystemColors.Control;
            this.clrUnpressedText.Color = System.Drawing.Color.Black;
            this.clrUnpressedText.ForeColor = System.Drawing.Color.Black;
            this.clrUnpressedText.LabelText = "Text Color";
            this.clrUnpressedText.Location = new System.Drawing.Point(6, 52);
            this.clrUnpressedText.Name = "clrUnpressedText";
            this.clrUnpressedText.PreviewShape = ThoNohT.NohBoard.Forms.ColorChooser.Shape.Square;
            this.clrUnpressedText.Size = new System.Drawing.Size(141, 26);
            this.clrUnpressedText.TabIndex = 1;
            this.clrUnpressedText.ColorChanged += new ThoNohT.NohBoard.Forms.ColorChooser.ColorChangedEventHandler(this.Control_ColorChanged);
            // 
            // clrUnpressedBackground
            // 
            this.clrUnpressedBackground.BackColor = System.Drawing.SystemColors.Control;
            this.clrUnpressedBackground.Color = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.clrUnpressedBackground.LabelText = "Background Color";
            this.clrUnpressedBackground.Location = new System.Drawing.Point(7, 20);
            this.clrUnpressedBackground.Name = "clrUnpressedBackground";
            this.clrUnpressedBackground.PreviewShape = ThoNohT.NohBoard.Forms.ColorChooser.Shape.Square;
            this.clrUnpressedBackground.Size = new System.Drawing.Size(140, 26);
            this.clrUnpressedBackground.TabIndex = 0;
            this.clrUnpressedBackground.ColorChanged += new ThoNohT.NohBoard.Forms.ColorChooser.ColorChangedEventHandler(this.Control_ColorChanged);
            // 
            // PressedGroup
            // 
            this.PressedGroup.Controls.Add(this.chkPressedOutline);
            this.PressedGroup.Controls.Add(this.clrPressedBackground);
            this.PressedGroup.Controls.Add(this.clrPressedOutline);
            this.PressedGroup.Controls.Add(this.clrPressedText);
            this.PressedGroup.Location = new System.Drawing.Point(330, 12);
            this.PressedGroup.Name = "PressedGroup";
            this.PressedGroup.Size = new System.Drawing.Size(153, 142);
            this.PressedGroup.TabIndex = 1;
            this.PressedGroup.TabStop = false;
            this.PressedGroup.Text = "Pressed keys";
            // 
            // chkPressedOutline
            // 
            this.chkPressedOutline.AutoSize = true;
            this.chkPressedOutline.Location = new System.Drawing.Point(6, 117);
            this.chkPressedOutline.Name = "chkPressedOutline";
            this.chkPressedOutline.Size = new System.Drawing.Size(89, 17);
            this.chkPressedOutline.TabIndex = 7;
            this.chkPressedOutline.Text = "Show Outline";
            this.chkPressedOutline.UseVisualStyleBackColor = true;
            this.chkPressedOutline.CheckedChanged += new System.EventHandler(this.chkPressedOutline_CheckedChanged);
            // 
            // clrPressedBackground
            // 
            this.clrPressedBackground.BackColor = System.Drawing.SystemColors.Control;
            this.clrPressedBackground.Color = System.Drawing.Color.White;
            this.clrPressedBackground.LabelText = "Background Color";
            this.clrPressedBackground.Location = new System.Drawing.Point(6, 20);
            this.clrPressedBackground.Name = "clrPressedBackground";
            this.clrPressedBackground.PreviewShape = ThoNohT.NohBoard.Forms.ColorChooser.Shape.Square;
            this.clrPressedBackground.Size = new System.Drawing.Size(141, 26);
            this.clrPressedBackground.TabIndex = 4;
            this.clrPressedBackground.ColorChanged += new ThoNohT.NohBoard.Forms.ColorChooser.ColorChangedEventHandler(this.Control_ColorChanged);
            // 
            // clrPressedOutline
            // 
            this.clrPressedOutline.BackColor = System.Drawing.SystemColors.Control;
            this.clrPressedOutline.Color = System.Drawing.Color.Lime;
            this.clrPressedOutline.LabelText = "Outline Color";
            this.clrPressedOutline.Location = new System.Drawing.Point(5, 84);
            this.clrPressedOutline.Name = "clrPressedOutline";
            this.clrPressedOutline.PreviewShape = ThoNohT.NohBoard.Forms.ColorChooser.Shape.Square;
            this.clrPressedOutline.Size = new System.Drawing.Size(142, 26);
            this.clrPressedOutline.TabIndex = 6;
            this.clrPressedOutline.ColorChanged += new ThoNohT.NohBoard.Forms.ColorChooser.ColorChangedEventHandler(this.Control_ColorChanged);
            // 
            // clrPressedText
            // 
            this.clrPressedText.BackColor = System.Drawing.SystemColors.Control;
            this.clrPressedText.Color = System.Drawing.Color.Black;
            this.clrPressedText.ForeColor = System.Drawing.Color.Black;
            this.clrPressedText.LabelText = "Text Color";
            this.clrPressedText.Location = new System.Drawing.Point(5, 52);
            this.clrPressedText.Name = "clrPressedText";
            this.clrPressedText.PreviewShape = ThoNohT.NohBoard.Forms.ColorChooser.Shape.Square;
            this.clrPressedText.Size = new System.Drawing.Size(142, 26);
            this.clrPressedText.TabIndex = 5;
            this.clrPressedText.ColorChanged += new ThoNohT.NohBoard.Forms.ColorChooser.ColorChangedEventHandler(this.Control_ColorChanged);
            // 
            // MouseSpeedGroup
            // 
            this.MouseSpeedGroup.Controls.Add(this.clrMouseSpeedLow);
            this.MouseSpeedGroup.Controls.Add(this.clrMouseSpeedHigh);
            this.MouseSpeedGroup.Location = new System.Drawing.Point(12, 74);
            this.MouseSpeedGroup.Name = "MouseSpeedGroup";
            this.MouseSpeedGroup.Size = new System.Drawing.Size(153, 88);
            this.MouseSpeedGroup.TabIndex = 8;
            this.MouseSpeedGroup.TabStop = false;
            this.MouseSpeedGroup.Text = "Mouse Speed Indicator";
            // 
            // clrMouseSpeedLow
            // 
            this.clrMouseSpeedLow.BackColor = System.Drawing.SystemColors.Control;
            this.clrMouseSpeedLow.Color = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.clrMouseSpeedLow.LabelText = "Color 1 (slow speed)";
            this.clrMouseSpeedLow.Location = new System.Drawing.Point(6, 20);
            this.clrMouseSpeedLow.Name = "clrMouseSpeedLow";
            this.clrMouseSpeedLow.PreviewShape = ThoNohT.NohBoard.Forms.ColorChooser.Shape.Square;
            this.clrMouseSpeedLow.Size = new System.Drawing.Size(141, 26);
            this.clrMouseSpeedLow.TabIndex = 4;
            this.clrMouseSpeedLow.ColorChanged += new ThoNohT.NohBoard.Forms.ColorChooser.ColorChangedEventHandler(this.Control_ColorChanged);
            // 
            // clrMouseSpeedHigh
            // 
            this.clrMouseSpeedHigh.BackColor = System.Drawing.SystemColors.Control;
            this.clrMouseSpeedHigh.Color = System.Drawing.Color.White;
            this.clrMouseSpeedHigh.ForeColor = System.Drawing.Color.Black;
            this.clrMouseSpeedHigh.LabelText = "Color 2 (high speed)";
            this.clrMouseSpeedHigh.Location = new System.Drawing.Point(5, 52);
            this.clrMouseSpeedHigh.Name = "clrMouseSpeedHigh";
            this.clrMouseSpeedHigh.PreviewShape = ThoNohT.NohBoard.Forms.ColorChooser.Shape.Square;
            this.clrMouseSpeedHigh.Size = new System.Drawing.Size(142, 26);
            this.clrMouseSpeedHigh.TabIndex = 5;
            this.clrMouseSpeedHigh.ColorChanged += new ThoNohT.NohBoard.Forms.ColorChooser.ColorChangedEventHandler(this.Control_ColorChanged);
            // 
            // KeyboardGroup
            // 
            this.KeyboardGroup.Controls.Add(this.clrKeyboardBackground);
            this.KeyboardGroup.Location = new System.Drawing.Point(12, 12);
            this.KeyboardGroup.Name = "KeyboardGroup";
            this.KeyboardGroup.Size = new System.Drawing.Size(153, 56);
            this.KeyboardGroup.TabIndex = 9;
            this.KeyboardGroup.TabStop = false;
            this.KeyboardGroup.Text = "Keyboard";
            // 
            // clrKeyboardBackground
            // 
            this.clrKeyboardBackground.BackColor = System.Drawing.SystemColors.Control;
            this.clrKeyboardBackground.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(100)))));
            this.clrKeyboardBackground.LabelText = "Background Color";
            this.clrKeyboardBackground.Location = new System.Drawing.Point(7, 19);
            this.clrKeyboardBackground.Name = "clrKeyboardBackground";
            this.clrKeyboardBackground.PreviewShape = ThoNohT.NohBoard.Forms.ColorChooser.Shape.Square;
            this.clrKeyboardBackground.Size = new System.Drawing.Size(140, 26);
            this.clrKeyboardBackground.TabIndex = 2;
            this.clrKeyboardBackground.ColorChanged += new ThoNohT.NohBoard.Forms.ColorChooser.ColorChangedEventHandler(this.Control_ColorChanged);
            // 
            // AcceptButton2
            // 
            this.AcceptButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.AcceptButton2.Location = new System.Drawing.Point(408, 160);
            this.AcceptButton2.Name = "AcceptButton2";
            this.AcceptButton2.Size = new System.Drawing.Size(75, 23);
            this.AcceptButton2.TabIndex = 10;
            this.AcceptButton2.Text = "Accept";
            this.AcceptButton2.UseVisualStyleBackColor = true;
            this.AcceptButton2.Click += new System.EventHandler(this.AcceptButton2_Click);
            // 
            // CancelButton2
            // 
            this.CancelButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton2.Location = new System.Drawing.Point(327, 160);
            this.CancelButton2.Name = "CancelButton2";
            this.CancelButton2.Size = new System.Drawing.Size(75, 23);
            this.CancelButton2.TabIndex = 11;
            this.CancelButton2.Text = "Cancel";
            this.CancelButton2.UseVisualStyleBackColor = true;
            this.CancelButton2.Click += new System.EventHandler(this.CancelButton2_Click);
            // 
            // KeyboardStyleForm
            // 
            this.AcceptButton = this.AcceptButton2;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelButton2;
            this.ClientSize = new System.Drawing.Size(495, 193);
            this.Controls.Add(this.CancelButton2);
            this.Controls.Add(this.AcceptButton2);
            this.Controls.Add(this.KeyboardGroup);
            this.Controls.Add(this.MouseSpeedGroup);
            this.Controls.Add(this.PressedGroup);
            this.Controls.Add(this.LooseGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "KeyboardStyleForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Keyboard Style";
            this.Load += new System.EventHandler(this.KeyboardStyleForm_Load);
            this.LooseGroup.ResumeLayout(false);
            this.LooseGroup.PerformLayout();
            this.PressedGroup.ResumeLayout(false);
            this.PressedGroup.PerformLayout();
            this.MouseSpeedGroup.ResumeLayout(false);
            this.KeyboardGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox LooseGroup;
        private System.Windows.Forms.CheckBox chkUnpressedOutline;
        private ColorChooser clrUnpressedOutline;
        private ColorChooser clrUnpressedText;
        private ColorChooser clrUnpressedBackground;
        private System.Windows.Forms.GroupBox PressedGroup;
        private System.Windows.Forms.CheckBox chkPressedOutline;
        private ColorChooser clrPressedBackground;
        private ColorChooser clrPressedOutline;
        private ColorChooser clrPressedText;
        private ColorChooser clrKeyboardBackground;
        private System.Windows.Forms.GroupBox MouseSpeedGroup;
        private ColorChooser clrMouseSpeedLow;
        private ColorChooser clrMouseSpeedHigh;
        private System.Windows.Forms.GroupBox KeyboardGroup;
        private System.Windows.Forms.Button AcceptButton2;
        private System.Windows.Forms.Button CancelButton2;
    }
}