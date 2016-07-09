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
    using ThoNohT.NohBoard.Controls;

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
            ThoNohT.NohBoard.Keyboard.Styles.KeySubStyle keySubStyle7 = new ThoNohT.NohBoard.Keyboard.Styles.KeySubStyle();
            ThoNohT.NohBoard.Extra.SerializableColor serializableColor19 = new ThoNohT.NohBoard.Extra.SerializableColor();
            ThoNohT.NohBoard.Extra.SerializableFont serializableFont7 = new ThoNohT.NohBoard.Extra.SerializableFont();
            ThoNohT.NohBoard.Extra.SerializableColor serializableColor20 = new ThoNohT.NohBoard.Extra.SerializableColor();
            ThoNohT.NohBoard.Extra.SerializableColor serializableColor21 = new ThoNohT.NohBoard.Extra.SerializableColor();
            ThoNohT.NohBoard.Keyboard.Styles.KeySubStyle keySubStyle8 = new ThoNohT.NohBoard.Keyboard.Styles.KeySubStyle();
            ThoNohT.NohBoard.Extra.SerializableColor serializableColor22 = new ThoNohT.NohBoard.Extra.SerializableColor();
            ThoNohT.NohBoard.Extra.SerializableFont serializableFont8 = new ThoNohT.NohBoard.Extra.SerializableFont();
            ThoNohT.NohBoard.Extra.SerializableColor serializableColor23 = new ThoNohT.NohBoard.Extra.SerializableColor();
            ThoNohT.NohBoard.Extra.SerializableColor serializableColor24 = new ThoNohT.NohBoard.Extra.SerializableColor();
            this.AcceptButton2 = new System.Windows.Forms.Button();
            this.CancelButton2 = new System.Windows.Forms.Button();
            this.pressed = new ThoNohT.NohBoard.Controls.KeySubStylePanel();
            this.loose = new ThoNohT.NohBoard.Controls.KeySubStylePanel();
            this.chkOverwriteLoose = new System.Windows.Forms.CheckBox();
            this.chkOverwritePressed = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // AcceptButton2
            // 
            this.AcceptButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.AcceptButton2.Location = new System.Drawing.Point(285, 350);
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
            this.CancelButton2.Location = new System.Drawing.Point(204, 350);
            this.CancelButton2.Name = "CancelButton2";
            this.CancelButton2.Size = new System.Drawing.Size(75, 23);
            this.CancelButton2.TabIndex = 11;
            this.CancelButton2.Text = "Cancel";
            this.CancelButton2.UseVisualStyleBackColor = true;
            this.CancelButton2.Click += new System.EventHandler(this.CancelButton2_Click);
            // 
            // pressed
            // 
            this.pressed.Location = new System.Drawing.Point(189, 12);
            this.pressed.Name = "pressed";
            this.pressed.Size = new System.Drawing.Size(171, 309);
            serializableColor19.Blue = ((byte)(0));
            serializableColor19.Green = ((byte)(0));
            serializableColor19.Red = ((byte)(0));
            keySubStyle7.Background = serializableColor19;
            keySubStyle7.BackgroundImageFileName = "";
            serializableFont7.FontFamily = "Microsoft Sans Serif";
            serializableFont7.Size = 8.25F;
            serializableFont7.Style = ThoNohT.NohBoard.Extra.SerializableFontStyle.Regular;
            keySubStyle7.Font = serializableFont7;
            serializableColor20.Blue = ((byte)(0));
            serializableColor20.Green = ((byte)(0));
            serializableColor20.Red = ((byte)(0));
            keySubStyle7.Outline = serializableColor20;
            keySubStyle7.OutlineWidth = 1;
            keySubStyle7.ShowOutline = false;
            serializableColor21.Blue = ((byte)(0));
            serializableColor21.Green = ((byte)(0));
            serializableColor21.Red = ((byte)(0));
            keySubStyle7.Text = serializableColor21;
            this.pressed.SubStyle = keySubStyle7;
            this.pressed.TabIndex = 13;
            this.pressed.Title = "Pressed";
            // 
            // loose
            // 
            this.loose.Location = new System.Drawing.Point(12, 12);
            this.loose.Name = "loose";
            this.loose.Size = new System.Drawing.Size(171, 309);
            serializableColor22.Blue = ((byte)(0));
            serializableColor22.Green = ((byte)(0));
            serializableColor22.Red = ((byte)(0));
            keySubStyle8.Background = serializableColor22;
            keySubStyle8.BackgroundImageFileName = "";
            serializableFont8.FontFamily = "Microsoft Sans Serif";
            serializableFont8.Size = 8.25F;
            serializableFont8.Style = ThoNohT.NohBoard.Extra.SerializableFontStyle.Regular;
            keySubStyle8.Font = serializableFont8;
            serializableColor23.Blue = ((byte)(0));
            serializableColor23.Green = ((byte)(0));
            serializableColor23.Red = ((byte)(0));
            keySubStyle8.Outline = serializableColor23;
            keySubStyle8.OutlineWidth = 1;
            keySubStyle8.ShowOutline = false;
            serializableColor24.Blue = ((byte)(0));
            serializableColor24.Green = ((byte)(0));
            serializableColor24.Red = ((byte)(0));
            keySubStyle8.Text = serializableColor24;
            this.loose.SubStyle = keySubStyle8;
            this.loose.TabIndex = 12;
            this.loose.Title = "Loose";
            // 
            // chkOverwriteLoose
            // 
            this.chkOverwriteLoose.AutoSize = true;
            this.chkOverwriteLoose.Location = new System.Drawing.Point(12, 327);
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
            this.chkOverwritePressed.Location = new System.Drawing.Point(189, 327);
            this.chkOverwritePressed.Name = "chkOverwritePressed";
            this.chkOverwritePressed.Size = new System.Drawing.Size(130, 17);
            this.chkOverwritePressed.TabIndex = 19;
            this.chkOverwritePressed.Text = "Overwrite default style";
            this.chkOverwritePressed.UseVisualStyleBackColor = true;
            this.chkOverwritePressed.CheckedChanged += new System.EventHandler(this.chkOverwritePressed_CheckedChanged);
            // 
            // KeyStyleForm
            // 
            this.AcceptButton = this.AcceptButton2;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelButton2;
            this.ClientSize = new System.Drawing.Size(370, 383);
            this.Controls.Add(this.chkOverwritePressed);
            this.Controls.Add(this.chkOverwriteLoose);
            this.Controls.Add(this.pressed);
            this.Controls.Add(this.loose);
            this.Controls.Add(this.CancelButton2);
            this.Controls.Add(this.AcceptButton2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "KeyStyleForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Keyboard Style";
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
    }
}