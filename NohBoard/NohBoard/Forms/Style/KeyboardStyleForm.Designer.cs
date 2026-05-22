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
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            Keyboard.Styles.KeySubStyle keySubStyle1 = new Keyboard.Styles.KeySubStyle();
            Extra.SerializableColor serializableColor1 = new Extra.SerializableColor();
            Extra.SerializableFont serializableFont1 = new Extra.SerializableFont();
            Extra.SerializableColor serializableColor2 = new Extra.SerializableColor();
            Extra.SerializableColor serializableColor3 = new Extra.SerializableColor();
            Keyboard.Styles.KeySubStyle keySubStyle2 = new Keyboard.Styles.KeySubStyle();
            Extra.SerializableColor serializableColor4 = new Extra.SerializableColor();
            Extra.SerializableFont serializableFont2 = new Extra.SerializableFont();
            Extra.SerializableColor serializableColor5 = new Extra.SerializableColor();
            Extra.SerializableColor serializableColor6 = new Extra.SerializableColor();
            Keyboard.Styles.MouseSpeedIndicatorStyle mouseSpeedIndicatorStyle1 = new Keyboard.Styles.MouseSpeedIndicatorStyle();
            Extra.SerializableColor serializableColor7 = new Extra.SerializableColor();
            Extra.SerializableColor serializableColor8 = new Extra.SerializableColor();
            KeyboardGroup = new System.Windows.Forms.GroupBox();
            txtBackgoundImage = new System.Windows.Forms.TextBox();
            lblBackgroundImage = new System.Windows.Forms.Label();
            clrKeyboardBackground = new ColorChooser();
            AcceptButton2 = new System.Windows.Forms.Button();
            CancelButton2 = new System.Windows.Forms.Button();
            pressedKeys = new KeySubStylePanel();
            looseKeys = new KeySubStylePanel();
            lblKeyboard = new System.Windows.Forms.Label();
            defaultMouseSpeed = new MouseSpeedStylePanel();
            lblOutlineWarning = new System.Windows.Forms.Label();
            KeyboardGroup.SuspendLayout();
            SuspendLayout();
            KeyboardGroup.Controls.Add(txtBackgoundImage);
            KeyboardGroup.Controls.Add(lblBackgroundImage);
            KeyboardGroup.Controls.Add(clrKeyboardBackground);
            KeyboardGroup.Location = new System.Drawing.Point(10, 35);
            KeyboardGroup.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            KeyboardGroup.Name = "KeyboardGroup";
            KeyboardGroup.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            KeyboardGroup.Size = new System.Drawing.Size(200, 90);
            KeyboardGroup.TabIndex = 9;
            KeyboardGroup.TabStop = false;
            KeyboardGroup.Text = "Background";
            txtBackgoundImage.Location = new System.Drawing.Point(70, 55);
            txtBackgoundImage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtBackgoundImage.Name = "txtBackgoundImage";
            txtBackgoundImage.Size = new System.Drawing.Size(125, 23);
            txtBackgoundImage.TabIndex = 4;
            lblBackgroundImage.Location = new System.Drawing.Point(5, 55);
            lblBackgroundImage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblBackgroundImage.Name = "lblBackgroundImage";
            lblBackgroundImage.Size = new System.Drawing.Size(60, 23);
            lblBackgroundImage.TabIndex = 3;
            lblBackgroundImage.Text = "Image:";
            lblBackgroundImage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            clrKeyboardBackground.BackColor = System.Drawing.SystemColors.Control;
            clrKeyboardBackground.Color = System.Drawing.Color.FromArgb(0, 0, 100);
            clrKeyboardBackground.LabelText = "Background Color";
            clrKeyboardBackground.Location = new System.Drawing.Point(10, 23);
            clrKeyboardBackground.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            clrKeyboardBackground.Name = "clrKeyboardBackground";
            clrKeyboardBackground.PreviewShape = ColorChooser.Shape.Square;
            clrKeyboardBackground.Size = new System.Drawing.Size(185, 23);
            clrKeyboardBackground.TabIndex = 2;
            AcceptButton2.Location = new System.Drawing.Point(550, 410);
            AcceptButton2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            AcceptButton2.Name = "AcceptButton2";
            AcceptButton2.Size = new System.Drawing.Size(80, 23);
            AcceptButton2.TabIndex = 10;
            AcceptButton2.Text = "Accept";
            AcceptButton2.UseVisualStyleBackColor = true;
            AcceptButton2.Click += AcceptButton2_Click;
            CancelButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            CancelButton2.Location = new System.Drawing.Point(465, 410);
            CancelButton2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CancelButton2.Name = "CancelButton2";
            CancelButton2.Size = new System.Drawing.Size(80, 23);
            CancelButton2.TabIndex = 11;
            CancelButton2.Text = "Cancel";
            CancelButton2.UseVisualStyleBackColor = true;
            CancelButton2.Click += CancelButton2_Click;
            pressedKeys.Location = new System.Drawing.Point(430, 10);
            pressedKeys.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            pressedKeys.Name = "pressedKeys";
            pressedKeys.Size = new System.Drawing.Size(200, 395);
            serializableColor1.Blue = 0;
            serializableColor1.Green = 0;
            serializableColor1.Red = 0;
            keySubStyle1.Background = serializableColor1;
            keySubStyle1.BackgroundImageFileName = "";
            serializableFont1.AlternateFontFamily = null;
            serializableFont1.DownloadUrl = null;
            serializableFont1.FontFamily = "Microsoft Sans Serif";
            serializableFont1.Size = 8.25F;
            serializableFont1.Style = Extra.SerializableFontStyle.Regular;
            keySubStyle1.Font = serializableFont1;
            serializableColor2.Blue = 0;
            serializableColor2.Green = 0;
            serializableColor2.Red = 0;
            keySubStyle1.Outline = serializableColor2;
            keySubStyle1.OutlineWidth = 1;
            keySubStyle1.ShowOutline = false;
            serializableColor3.Blue = 0;
            serializableColor3.Green = 0;
            serializableColor3.Red = 0;
            keySubStyle1.Text = serializableColor3;
            pressedKeys.SubStyle = keySubStyle1;
            pressedKeys.TabIndex = 13;
            pressedKeys.Title = "Pressed Keys";
            looseKeys.Location = new System.Drawing.Point(220, 10);
            looseKeys.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            looseKeys.Name = "looseKeys";
            looseKeys.Size = new System.Drawing.Size(200, 395);
            serializableColor4.Blue = 0;
            serializableColor4.Green = 0;
            serializableColor4.Red = 0;
            keySubStyle2.Background = serializableColor4;
            keySubStyle2.BackgroundImageFileName = "";
            serializableFont2.AlternateFontFamily = null;
            serializableFont2.DownloadUrl = null;
            serializableFont2.FontFamily = "Microsoft Sans Serif";
            serializableFont2.Size = 8.25F;
            serializableFont2.Style = Extra.SerializableFontStyle.Regular;
            keySubStyle2.Font = serializableFont2;
            serializableColor5.Blue = 0;
            serializableColor5.Green = 0;
            serializableColor5.Red = 0;
            keySubStyle2.Outline = serializableColor5;
            keySubStyle2.OutlineWidth = 1;
            keySubStyle2.ShowOutline = false;
            serializableColor6.Blue = 0;
            serializableColor6.Green = 0;
            serializableColor6.Red = 0;
            keySubStyle2.Text = serializableColor6;
            looseKeys.SubStyle = keySubStyle2;
            looseKeys.TabIndex = 12;
            looseKeys.Title = "Loose Keys";
            lblKeyboard.AutoEllipsis = true;
            lblKeyboard.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblKeyboard.Location = new System.Drawing.Point(10, 10);
            lblKeyboard.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblKeyboard.Name = "lblKeyboard";
            lblKeyboard.Size = new System.Drawing.Size(200, 23);
            lblKeyboard.TabIndex = 15;
            lblKeyboard.Text = "Keyboard";
            lblKeyboard.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            serializableColor7.Blue = 0;
            serializableColor7.Green = 0;
            serializableColor7.Red = 0;
            mouseSpeedIndicatorStyle1.InnerColor = serializableColor7;
            serializableColor8.Blue = 0;
            serializableColor8.Green = 0;
            serializableColor8.Red = 0;
            mouseSpeedIndicatorStyle1.OuterColor = serializableColor8;
            mouseSpeedIndicatorStyle1.OutlineWidth = 1;
            defaultMouseSpeed.IndicatorStyle = mouseSpeedIndicatorStyle1;
            defaultMouseSpeed.Location = new System.Drawing.Point(10, 135);
            defaultMouseSpeed.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            defaultMouseSpeed.Name = "defaultMouseSpeed";
            defaultMouseSpeed.Size = new System.Drawing.Size(200, 145);
            defaultMouseSpeed.TabIndex = 16;
            defaultMouseSpeed.Title = "MouseSpeedIndicator";
            lblOutlineWarning.Location = new System.Drawing.Point(10, 290);
            lblOutlineWarning.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblOutlineWarning.Name = "lblOutlineWarning";
            lblOutlineWarning.Size = new System.Drawing.Size(200, 50);
            lblOutlineWarning.TabIndex = 21;
            lblOutlineWarning.Text = "Setting a smaller outline for pressed\r\nthan loose keys will show the loose\r\noutline behind the pressed key.";
            lblOutlineWarning.Visible = false;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = CancelButton2;
            ClientSize = new System.Drawing.Size(639, 441);
            Controls.Add(lblOutlineWarning);
            Controls.Add(defaultMouseSpeed);
            Controls.Add(lblKeyboard);
            Controls.Add(pressedKeys);
            Controls.Add(looseKeys);
            Controls.Add(CancelButton2);
            Controls.Add(AcceptButton2);
            Controls.Add(KeyboardGroup);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "KeyboardStyleForm";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Keyboard Style";
            Load += KeyboardStyleForm_Load;
            KeyboardGroup.ResumeLayout(false);
            KeyboardGroup.PerformLayout();
            ResumeLayout(false);

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
