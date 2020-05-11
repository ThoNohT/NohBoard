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
            ThoNohT.NohBoard.Keyboard.Styles.MouseSpeedIndicatorStyle mouseSpeedIndicatorStyle1 = new ThoNohT.NohBoard.Keyboard.Styles.MouseSpeedIndicatorStyle();
            ThoNohT.NohBoard.Extra.SerializableColor serializableColor7 = new ThoNohT.NohBoard.Extra.SerializableColor();
            ThoNohT.NohBoard.Extra.SerializableColor serializableColor8 = new ThoNohT.NohBoard.Extra.SerializableColor();
            this.KeyboardGroup = new System.Windows.Forms.GroupBox();
            this.txtBackgoundImage = new System.Windows.Forms.TextBox();
            this.lblBackgroundImage = new System.Windows.Forms.Label();
            this.clrKeyboardBackground = new ThoNohT.NohBoard.Controls.ColorChooser();
            this.AcceptButton2 = new System.Windows.Forms.Button();
            this.CancelButton2 = new System.Windows.Forms.Button();
            this.pressedKeys = new ThoNohT.NohBoard.Controls.KeySubStylePanel();
            this.looseKeys = new ThoNohT.NohBoard.Controls.KeySubStylePanel();
            this.lblKeyboard = new System.Windows.Forms.Label();
            this.defaultMouseSpeed = new ThoNohT.NohBoard.Controls.MouseSpeedStylePanel();
            this.lblOutlineWarning = new System.Windows.Forms.Label();
            this.KeyboardGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // KeyboardGroup
            // 
            this.KeyboardGroup.Controls.Add(this.txtBackgoundImage);
            this.KeyboardGroup.Controls.Add(this.lblBackgroundImage);
            this.KeyboardGroup.Controls.Add(this.clrKeyboardBackground);
            this.KeyboardGroup.Location = new System.Drawing.Point(12, 34);
            this.KeyboardGroup.Name = "KeyboardGroup";
            this.KeyboardGroup.Size = new System.Drawing.Size(171, 80);
            this.KeyboardGroup.TabIndex = 9;
            this.KeyboardGroup.TabStop = false;
            this.KeyboardGroup.Text = "Background";
            // 
            // txtBackgoundImage
            // 
            this.txtBackgoundImage.Location = new System.Drawing.Point(54, 52);
            this.txtBackgoundImage.Name = "txtBackgoundImage";
            this.txtBackgoundImage.Size = new System.Drawing.Size(100, 20);
            this.txtBackgoundImage.TabIndex = 4;
            // 
            // lblBackgroundImage
            // 
            this.lblBackgroundImage.Location = new System.Drawing.Point(6, 48);
            this.lblBackgroundImage.Name = "lblBackgroundImage";
            this.lblBackgroundImage.Size = new System.Drawing.Size(41, 23);
            this.lblBackgroundImage.TabIndex = 3;
            this.lblBackgroundImage.Text = "Image:";
            this.lblBackgroundImage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // clrKeyboardBackground
            // 
            this.clrKeyboardBackground.BackColor = System.Drawing.SystemColors.Control;
            this.clrKeyboardBackground.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(100)))));
            this.clrKeyboardBackground.LabelText = "Background Color";
            this.clrKeyboardBackground.Location = new System.Drawing.Point(7, 19);
            this.clrKeyboardBackground.Name = "clrKeyboardBackground";
            this.clrKeyboardBackground.PreviewShape = ThoNohT.NohBoard.Controls.ColorChooser.Shape.Square;
            this.clrKeyboardBackground.Size = new System.Drawing.Size(158, 26);
            this.clrKeyboardBackground.TabIndex = 2;
            // 
            // AcceptButton2
            // 
            this.AcceptButton2.Location = new System.Drawing.Point(461, 343);
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
            this.CancelButton2.Location = new System.Drawing.Point(380, 343);
            this.CancelButton2.Name = "CancelButton2";
            this.CancelButton2.Size = new System.Drawing.Size(75, 23);
            this.CancelButton2.TabIndex = 11;
            this.CancelButton2.Text = "Cancel";
            this.CancelButton2.UseVisualStyleBackColor = true;
            this.CancelButton2.Click += new System.EventHandler(this.CancelButton2_Click);
            // 
            // pressedKeys
            // 
            this.pressedKeys.Location = new System.Drawing.Point(366, 13);
            this.pressedKeys.Name = "pressedKeys";
            this.pressedKeys.Size = new System.Drawing.Size(171, 331);
            serializableColor1.Blue = ((byte)(0));
            serializableColor1.Green = ((byte)(0));
            serializableColor1.Red = ((byte)(0));
            keySubStyle1.Background = serializableColor1;
            keySubStyle1.BackgroundImageFileName = "";
            serializableFont1.AlternateFontFamily = null;
            serializableFont1.DownloadUrl = null;
            serializableFont1.FontFamily = "Microsoft Sans Serif";
            serializableFont1.Size = 8.25F;
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
            this.pressedKeys.SubStyle = keySubStyle1;
            this.pressedKeys.TabIndex = 13;
            this.pressedKeys.Title = "Pressed Keys";
            // 
            // looseKeys
            // 
            this.looseKeys.Location = new System.Drawing.Point(189, 13);
            this.looseKeys.Name = "looseKeys";
            this.looseKeys.Size = new System.Drawing.Size(171, 331);
            serializableColor4.Blue = ((byte)(0));
            serializableColor4.Green = ((byte)(0));
            serializableColor4.Red = ((byte)(0));
            keySubStyle2.Background = serializableColor4;
            keySubStyle2.BackgroundImageFileName = "";
            serializableFont2.AlternateFontFamily = null;
            serializableFont2.DownloadUrl = null;
            serializableFont2.FontFamily = "Microsoft Sans Serif";
            serializableFont2.Size = 8.25F;
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
            this.looseKeys.SubStyle = keySubStyle2;
            this.looseKeys.TabIndex = 12;
            this.looseKeys.Title = "Loose Keys";
            // 
            // lblKeyboard
            // 
            this.lblKeyboard.AutoEllipsis = true;
            this.lblKeyboard.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKeyboard.Location = new System.Drawing.Point(9, 14);
            this.lblKeyboard.Name = "lblKeyboard";
            this.lblKeyboard.Size = new System.Drawing.Size(174, 18);
            this.lblKeyboard.TabIndex = 15;
            this.lblKeyboard.Text = "Keyboard";
            this.lblKeyboard.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // defaultMouseSpeed
            // 
            serializableColor7.Blue = ((byte)(0));
            serializableColor7.Green = ((byte)(0));
            serializableColor7.Red = ((byte)(0));
            mouseSpeedIndicatorStyle1.InnerColor = serializableColor7;
            serializableColor8.Blue = ((byte)(0));
            serializableColor8.Green = ((byte)(0));
            serializableColor8.Red = ((byte)(0));
            mouseSpeedIndicatorStyle1.OuterColor = serializableColor8;
            mouseSpeedIndicatorStyle1.OutlineWidth = 1;
            this.defaultMouseSpeed.IndicatorStyle = mouseSpeedIndicatorStyle1;
            this.defaultMouseSpeed.Location = new System.Drawing.Point(12, 120);
            this.defaultMouseSpeed.Name = "defaultMouseSpeed";
            this.defaultMouseSpeed.Size = new System.Drawing.Size(171, 144);
            this.defaultMouseSpeed.TabIndex = 16;
            this.defaultMouseSpeed.Title = "MouseSpeedIndicator";
            // 
            // lblOutlineWarning
            // 
            this.lblOutlineWarning.AutoSize = true;
            this.lblOutlineWarning.Location = new System.Drawing.Point(12, 267);
            this.lblOutlineWarning.Name = "lblOutlineWarning";
            this.lblOutlineWarning.Size = new System.Drawing.Size(173, 39);
            this.lblOutlineWarning.TabIndex = 21;
            this.lblOutlineWarning.Text = "Setting a smaller outline for pressed\r\nthan loose keys will show the loose\r\noutli" +
    "ne behind the pressed key.";
            this.lblOutlineWarning.Visible = false;
            // 
            // KeyboardStyleForm
            // 
            this.AcceptButton = this.AcceptButton2;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelButton2;
            this.ClientSize = new System.Drawing.Size(542, 372);
            this.Controls.Add(this.lblOutlineWarning);
            this.Controls.Add(this.defaultMouseSpeed);
            this.Controls.Add(this.lblKeyboard);
            this.Controls.Add(this.pressedKeys);
            this.Controls.Add(this.looseKeys);
            this.Controls.Add(this.CancelButton2);
            this.Controls.Add(this.AcceptButton2);
            this.Controls.Add(this.KeyboardGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "KeyboardStyleForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Keyboard Style";
            this.Load += new System.EventHandler(this.KeyboardStyleForm_Load);
            this.KeyboardGroup.ResumeLayout(false);
            this.KeyboardGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ColorChooser clrKeyboardBackground;
        private System.Windows.Forms.GroupBox KeyboardGroup;
        private System.Windows.Forms.Button AcceptButton2;
        private System.Windows.Forms.Button CancelButton2;
        private KeySubStylePanel looseKeys;
        private KeySubStylePanel pressedKeys;
        private System.Windows.Forms.Label lblKeyboard;
        private MouseSpeedStylePanel defaultMouseSpeed;
        private System.Windows.Forms.TextBox txtBackgoundImage;
        private System.Windows.Forms.Label lblBackgroundImage;
        private System.Windows.Forms.Label lblOutlineWarning;
    }
}