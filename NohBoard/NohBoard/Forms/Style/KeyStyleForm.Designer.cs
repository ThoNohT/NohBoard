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

    partial class KeyStyleForm
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
            ThoNohT.NohBoard.Keyboard.Styles.KeySubStyle keySubStyle1 = new ThoNohT.NohBoard.Keyboard.Styles.KeySubStyle();
            ThoNohT.NohBoard.Extra.SerializableColor serializableColor1 = new ThoNohT.NohBoard.Extra.SerializableColor();
            ThoNohT.NohBoard.Extra.SerializableFont serializableFont1 = new ThoNohT.NohBoard.Extra.SerializableFont();
            ThoNohT.NohBoard.Extra.SerializableColor serializableColor2 = new ThoNohT.NohBoard.Extra.SerializableColor();
            ThoNohT.NohBoard.Extra.SerializableColor serializableColor3 = new ThoNohT.NohBoard.Extra.SerializableColor();
            ThoNohT.NohBoard.Keyboard.Styles.KeySubStyle keySubStyle2 = new ThoNohT.NohBoard.Keyboard.Styles.KeySubStyle();
            ThoNohT.NohBoard.Extra.SerializableColor serializableColor4 = new ThoNohT.NohBoard.Extra.SerializableColor();
            ThoNohT.NohBoard.Extra.SerializableFont serializableFont2 = new ThoNohT.NohBoard.Extra.SerializableFont();
            ThoNohT.NohBoard.Extra.SerializableColor serializableColor5 = new ThoNohT.NohBoard.Extra.SerializableColor();
            ThoNohT.NohBoard.Extra.SerializableColor serializableColor6 = new ThoNohT.NohBoard.Extra.SerializableColor();
            this.AcceptButton2 = new System.Windows.Forms.Button();
            this.CancelButton2 = new System.Windows.Forms.Button();
            this.chkOverwriteLoose = new System.Windows.Forms.CheckBox();
            this.chkOverwritePressed = new System.Windows.Forms.CheckBox();
            this.pressed = new ThoNohT.NohBoard.Controls.KeySubStylePanel();
            this.loose = new ThoNohT.NohBoard.Controls.KeySubStylePanel();
            this.lblOutlineWarning = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // AcceptButton2
            // 
            this.AcceptButton2.Location = new System.Drawing.Point(285, 375);
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
            this.CancelButton2.Location = new System.Drawing.Point(204, 375);
            this.CancelButton2.Name = "CancelButton2";
            this.CancelButton2.Size = new System.Drawing.Size(75, 23);
            this.CancelButton2.TabIndex = 11;
            this.CancelButton2.Text = "Cancel";
            this.CancelButton2.UseVisualStyleBackColor = true;
            this.CancelButton2.Click += new System.EventHandler(this.CancelButton2_Click);
            // 
            // chkOverwriteLoose
            // 
            this.chkOverwriteLoose.AutoSize = true;
            this.chkOverwriteLoose.Location = new System.Drawing.Point(14, 343);
            this.chkOverwriteLoose.Name = "chkOverwriteLoose";
            this.chkOverwriteLoose.Size = new System.Drawing.Size(130, 17);
            this.chkOverwriteLoose.TabIndex = 18;
            this.chkOverwriteLoose.Text = "Overwrite default style";
            this.chkOverwriteLoose.UseVisualStyleBackColor = true;
            this.chkOverwriteLoose.CheckedChanged += new System.EventHandler(this.chkOverwriteLoose_CheckedChanged);
            // 
            // chkOverwritePressed
            // 
            this.chkOverwritePressed.AutoSize = true;
            this.chkOverwritePressed.Location = new System.Drawing.Point(191, 343);
            this.chkOverwritePressed.Name = "chkOverwritePressed";
            this.chkOverwritePressed.Size = new System.Drawing.Size(130, 17);
            this.chkOverwritePressed.TabIndex = 19;
            this.chkOverwritePressed.Text = "Overwrite default style";
            this.chkOverwritePressed.UseVisualStyleBackColor = true;
            this.chkOverwritePressed.CheckedChanged += new System.EventHandler(this.chkOverwritePressed_CheckedChanged);
            // 
            // pressed
            // 
            this.pressed.Location = new System.Drawing.Point(189, 12);
            this.pressed.Name = "pressed";
            this.pressed.Size = new System.Drawing.Size(171, 331);
            serializableColor1.Blue = ((byte)(0));
            serializableColor1.Green = ((byte)(0));
            serializableColor1.Red = ((byte)(0));
            keySubStyle1.Background = serializableColor1;
            keySubStyle1.BackgroundImageFileName = "";
            serializableFont1.AlternateFontFamily = null;
            serializableFont1.DownloadUrl = null;
            serializableFont1.FontFamily = "Courier New";
            serializableFont1.Size = 10F;
            serializableFont1.Style = ThoNohT.NohBoard.Extra.SerializableFontStyle.Regular;
            keySubStyle1.Font = serializableFont1;
            serializableColor2.Blue = ((byte)(0));
            serializableColor2.Green = ((byte)(0));
            serializableColor2.Red = ((byte)(0));
            keySubStyle1.Outline = serializableColor2;
            keySubStyle1.OutlineWidth = 1;
            keySubStyle1.ShowOutline = false;
            serializableColor3.Blue = ((byte)(0));
            serializableColor3.Green = ((byte)(0));
            serializableColor3.Red = ((byte)(0));
            keySubStyle1.Text = serializableColor3;
            this.pressed.SubStyle = keySubStyle1;
            this.pressed.TabIndex = 13;
            this.pressed.Title = "Pressed";
            // 
            // loose
            // 
            this.loose.Location = new System.Drawing.Point(12, 12);
            this.loose.Name = "loose";
            this.loose.Size = new System.Drawing.Size(171, 331);
            serializableColor4.Blue = ((byte)(0));
            serializableColor4.Green = ((byte)(0));
            serializableColor4.Red = ((byte)(0));
            keySubStyle2.Background = serializableColor4;
            keySubStyle2.BackgroundImageFileName = "";
            serializableFont2.AlternateFontFamily = null;
            serializableFont2.DownloadUrl = null;
            serializableFont2.FontFamily = "Courier New";
            serializableFont2.Size = 10F;
            serializableFont2.Style = ThoNohT.NohBoard.Extra.SerializableFontStyle.Regular;
            keySubStyle2.Font = serializableFont2;
            serializableColor5.Blue = ((byte)(0));
            serializableColor5.Green = ((byte)(0));
            serializableColor5.Red = ((byte)(0));
            keySubStyle2.Outline = serializableColor5;
            keySubStyle2.OutlineWidth = 1;
            keySubStyle2.ShowOutline = false;
            serializableColor6.Blue = ((byte)(0));
            serializableColor6.Green = ((byte)(0));
            serializableColor6.Red = ((byte)(0));
            keySubStyle2.Text = serializableColor6;
            this.loose.SubStyle = keySubStyle2;
            this.loose.TabIndex = 12;
            this.loose.Title = "Loose";
            // 
            // lblOutlineWarning
            // 
            this.lblOutlineWarning.AutoSize = true;
            this.lblOutlineWarning.Location = new System.Drawing.Point(9, 363);
            this.lblOutlineWarning.Name = "lblOutlineWarning";
            this.lblOutlineWarning.Size = new System.Drawing.Size(173, 39);
            this.lblOutlineWarning.TabIndex = 20;
            this.lblOutlineWarning.Text = "Setting a smaller outline for pressed\r\nthan loose keys will show the loose\r\noutli" +
    "ne behind the pressed key.";
            this.lblOutlineWarning.Visible = false;
            // 
            // KeyStyleForm
            // 
            this.AcceptButton = this.AcceptButton2;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelButton2;
            this.ClientSize = new System.Drawing.Size(370, 410);
            this.Controls.Add(this.lblOutlineWarning);
            this.Controls.Add(this.chkOverwritePressed);
            this.Controls.Add(this.chkOverwriteLoose);
            this.Controls.Add(this.pressed);
            this.Controls.Add(this.loose);
            this.Controls.Add(this.CancelButton2);
            this.Controls.Add(this.AcceptButton2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "KeyStyleForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Key Style";
            this.Load += new System.EventHandler(this.KeyStyleForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button AcceptButton2;
        private System.Windows.Forms.Button CancelButton2;
        private KeySubStylePanel loose;
        private KeySubStylePanel pressed;
        private System.Windows.Forms.CheckBox chkOverwriteLoose;
        private System.Windows.Forms.CheckBox chkOverwritePressed;
        private System.Windows.Forms.Label lblOutlineWarning;
    }
}