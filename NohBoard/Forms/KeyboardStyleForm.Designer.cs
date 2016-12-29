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
            ThoNohT.NohBoard.Keyboard.Styles.KeySubStyle keySubStyle3 = new ThoNohT.NohBoard.Keyboard.Styles.KeySubStyle();
            ThoNohT.NohBoard.Extra.SerializableColor serializableColor9 = new ThoNohT.NohBoard.Extra.SerializableColor();
            ThoNohT.NohBoard.Extra.SerializableFont serializableFont3 = new ThoNohT.NohBoard.Extra.SerializableFont();
            ThoNohT.NohBoard.Extra.SerializableColor serializableColor10 = new ThoNohT.NohBoard.Extra.SerializableColor();
            ThoNohT.NohBoard.Extra.SerializableColor serializableColor11 = new ThoNohT.NohBoard.Extra.SerializableColor();
            ThoNohT.NohBoard.Keyboard.Styles.KeySubStyle keySubStyle4 = new ThoNohT.NohBoard.Keyboard.Styles.KeySubStyle();
            ThoNohT.NohBoard.Extra.SerializableColor serializableColor12 = new ThoNohT.NohBoard.Extra.SerializableColor();
            ThoNohT.NohBoard.Extra.SerializableFont serializableFont4 = new ThoNohT.NohBoard.Extra.SerializableFont();
            ThoNohT.NohBoard.Extra.SerializableColor serializableColor13 = new ThoNohT.NohBoard.Extra.SerializableColor();
            ThoNohT.NohBoard.Extra.SerializableColor serializableColor14 = new ThoNohT.NohBoard.Extra.SerializableColor();
            ThoNohT.NohBoard.Keyboard.Styles.MouseSpeedIndicatorStyle mouseSpeedIndicatorStyle2 = new ThoNohT.NohBoard.Keyboard.Styles.MouseSpeedIndicatorStyle();
            ThoNohT.NohBoard.Extra.SerializableColor serializableColor15 = new ThoNohT.NohBoard.Extra.SerializableColor();
            ThoNohT.NohBoard.Extra.SerializableColor serializableColor16 = new ThoNohT.NohBoard.Extra.SerializableColor();
            this.KeyboardGroup = new System.Windows.Forms.GroupBox();
            this.clrKeyboardBackground = new ThoNohT.NohBoard.Controls.ColorChooser();
            this.AcceptButton2 = new System.Windows.Forms.Button();
            this.CancelButton2 = new System.Windows.Forms.Button();
            this.pressedKeys = new ThoNohT.NohBoard.Controls.KeySubStylePanel();
            this.looseKeys = new ThoNohT.NohBoard.Controls.KeySubStylePanel();
            this.lblKeyboard = new System.Windows.Forms.Label();
            this.defaultMouseSpeed = new ThoNohT.NohBoard.Controls.MouseSpeedStylePanel();
            this.txtBackgoundImage = new System.Windows.Forms.TextBox();
            this.lblBackgroundImage = new System.Windows.Forms.Label();
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
            this.clrKeyboardBackground.ColorChanged += this.Control_ColorChanged;
            // 
            // AcceptButton2
            // 
            this.AcceptButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.AcceptButton2.Location = new System.Drawing.Point(462, 328);
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
            this.CancelButton2.Location = new System.Drawing.Point(381, 328);
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
            this.pressedKeys.Size = new System.Drawing.Size(171, 309);
            serializableColor9.Blue = ((byte)(0));
            serializableColor9.Green = ((byte)(0));
            serializableColor9.Red = ((byte)(0));
            keySubStyle3.Background = serializableColor9;
            keySubStyle3.BackgroundImageFileName = "";
            serializableFont3.FontFamily = "Microsoft Sans Serif";
            serializableFont3.Size = 8.25F;
            serializableFont3.Style = ThoNohT.NohBoard.Extra.SerializableFontStyle.Regular;
            keySubStyle3.Font = serializableFont3;
            serializableColor10.Blue = ((byte)(0));
            serializableColor10.Green = ((byte)(0));
            serializableColor10.Red = ((byte)(0));
            keySubStyle3.Outline = serializableColor10;
            keySubStyle3.OutlineWidth = 1;
            keySubStyle3.ShowOutline = false;
            serializableColor11.Blue = ((byte)(0));
            serializableColor11.Green = ((byte)(0));
            serializableColor11.Red = ((byte)(0));
            keySubStyle3.Text = serializableColor11;
            this.pressedKeys.SubStyle = keySubStyle3;
            this.pressedKeys.TabIndex = 13;
            this.pressedKeys.Title = "Pressed Keys";
            this.pressedKeys.StyleChanged += this.pressedKeys_SubStyleChanged;
            // 
            // looseKeys
            // 
            this.looseKeys.Location = new System.Drawing.Point(189, 13);
            this.looseKeys.Name = "looseKeys";
            this.looseKeys.Size = new System.Drawing.Size(171, 309);
            serializableColor12.Blue = ((byte)(0));
            serializableColor12.Green = ((byte)(0));
            serializableColor12.Red = ((byte)(0));
            keySubStyle4.Background = serializableColor12;
            keySubStyle4.BackgroundImageFileName = "";
            serializableFont4.FontFamily = "Microsoft Sans Serif";
            serializableFont4.Size = 8.25F;
            serializableFont4.Style = ThoNohT.NohBoard.Extra.SerializableFontStyle.Regular;
            keySubStyle4.Font = serializableFont4;
            serializableColor13.Blue = ((byte)(0));
            serializableColor13.Green = ((byte)(0));
            serializableColor13.Red = ((byte)(0));
            keySubStyle4.Outline = serializableColor13;
            keySubStyle4.OutlineWidth = 1;
            keySubStyle4.ShowOutline = false;
            serializableColor14.Blue = ((byte)(0));
            serializableColor14.Green = ((byte)(0));
            serializableColor14.Red = ((byte)(0));
            keySubStyle4.Text = serializableColor14;
            this.looseKeys.SubStyle = keySubStyle4;
            this.looseKeys.TabIndex = 12;
            this.looseKeys.Title = "Loose Keys";
            this.looseKeys.StyleChanged += this.looseKeys_SubStyleChanged;
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
            serializableColor15.Blue = ((byte)(0));
            serializableColor15.Green = ((byte)(0));
            serializableColor15.Red = ((byte)(0));
            mouseSpeedIndicatorStyle2.InnerColor = serializableColor15;
            serializableColor16.Blue = ((byte)(0));
            serializableColor16.Green = ((byte)(0));
            serializableColor16.Red = ((byte)(0));
            mouseSpeedIndicatorStyle2.OuterColor = serializableColor16;
            mouseSpeedIndicatorStyle2.OutlineWidth = 1;
            this.defaultMouseSpeed.IndicatorStyle = mouseSpeedIndicatorStyle2;
            this.defaultMouseSpeed.Location = new System.Drawing.Point(12, 120);
            this.defaultMouseSpeed.Name = "defaultMouseSpeed";
            this.defaultMouseSpeed.Size = new System.Drawing.Size(171, 144);
            this.defaultMouseSpeed.TabIndex = 16;
            this.defaultMouseSpeed.Title = "MouseSpeedIndicator";
            this.defaultMouseSpeed.IndicatorStyleChanged += this.defaultMouseSpeed_IndicatorStyleChanged;
            // 
            // txtBackgoundImage
            // 
            this.txtBackgoundImage.Location = new System.Drawing.Point(54, 52);
            this.txtBackgoundImage.Name = "txtBackgoundImage";
            this.txtBackgoundImage.Size = new System.Drawing.Size(100, 20);
            this.txtBackgoundImage.TabIndex = 4;
            this.txtBackgoundImage.TextChanged += new System.EventHandler(this.txtBackgoundImage_TextChanged);
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
            // KeyboardStyleForm
            // 
            this.AcceptButton = this.AcceptButton2;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelButton2;
            this.ClientSize = new System.Drawing.Size(539, 355);
            this.Controls.Add(this.defaultMouseSpeed);
            this.Controls.Add(this.lblKeyboard);
            this.Controls.Add(this.pressedKeys);
            this.Controls.Add(this.looseKeys);
            this.Controls.Add(this.CancelButton2);
            this.Controls.Add(this.AcceptButton2);
            this.Controls.Add(this.KeyboardGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "KeyboardStyleForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Keyboard Style";
            this.Load += new System.EventHandler(this.KeyboardStyleForm_Load);
            this.KeyboardGroup.ResumeLayout(false);
            this.KeyboardGroup.PerformLayout();
            this.ResumeLayout(false);

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
    }
}