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
            Keyboard.Styles.KeySubStyle keySubStyle3 = new Keyboard.Styles.KeySubStyle();
            Extra.SerializableColor serializableColor7 = new Extra.SerializableColor();
            Extra.SerializableFont serializableFont3 = new Extra.SerializableFont();
            Extra.SerializableColor serializableColor8 = new Extra.SerializableColor();
            Extra.SerializableColor serializableColor9 = new Extra.SerializableColor();
            Keyboard.Styles.KeySubStyle keySubStyle4 = new Keyboard.Styles.KeySubStyle();
            Extra.SerializableColor serializableColor10 = new Extra.SerializableColor();
            Extra.SerializableFont serializableFont4 = new Extra.SerializableFont();
            Extra.SerializableColor serializableColor11 = new Extra.SerializableColor();
            Extra.SerializableColor serializableColor12 = new Extra.SerializableColor();
            AcceptButton2 = new System.Windows.Forms.Button();
            CancelButton2 = new System.Windows.Forms.Button();
            chkOverwriteLoose = new System.Windows.Forms.CheckBox();
            chkOverwritePressed = new System.Windows.Forms.CheckBox();
            pressed = new KeySubStylePanel();
            loose = new KeySubStylePanel();
            lblOutlineWarning = new System.Windows.Forms.Label();
            SuspendLayout();
            AcceptButton2.Location = new System.Drawing.Point(340, 475);
            AcceptButton2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            AcceptButton2.Name = "AcceptButton2";
            AcceptButton2.Size = new System.Drawing.Size(80, 23);
            AcceptButton2.TabIndex = 10;
            AcceptButton2.Text = "Accept";
            AcceptButton2.UseVisualStyleBackColor = true;
            AcceptButton2.Click += AcceptButton2_Click;
            CancelButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            CancelButton2.Location = new System.Drawing.Point(255, 475);
            CancelButton2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CancelButton2.Name = "CancelButton2";
            CancelButton2.Size = new System.Drawing.Size(80, 23);
            CancelButton2.TabIndex = 11;
            CancelButton2.Text = "Cancel";
            CancelButton2.UseVisualStyleBackColor = true;
            CancelButton2.Click += CancelButton2_Click;
            chkOverwriteLoose.Location = new System.Drawing.Point(10, 420);
            chkOverwriteLoose.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            chkOverwriteLoose.Name = "chkOverwriteLoose";
            chkOverwriteLoose.Size = new System.Drawing.Size(200, 23);
            chkOverwriteLoose.TabIndex = 18;
            chkOverwriteLoose.Text = "Overwrite default style";
            chkOverwriteLoose.UseVisualStyleBackColor = true;
            chkOverwriteLoose.CheckedChanged += chkOverwriteLoose_CheckedChanged;
            chkOverwritePressed.Location = new System.Drawing.Point(220, 420);
            chkOverwritePressed.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            chkOverwritePressed.Name = "chkOverwritePressed";
            chkOverwritePressed.Size = new System.Drawing.Size(200, 23);
            chkOverwritePressed.TabIndex = 19;
            chkOverwritePressed.Text = "Overwrite default style";
            chkOverwritePressed.UseVisualStyleBackColor = true;
            chkOverwritePressed.CheckedChanged += chkOverwritePressed_CheckedChanged;
            pressed.Location = new System.Drawing.Point(220, 10);
            pressed.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            pressed.Name = "pressed";
            pressed.Size = new System.Drawing.Size(200, 395);
            serializableColor7.Blue = 0;
            serializableColor7.Green = 0;
            serializableColor7.Red = 0;
            keySubStyle3.Background = serializableColor7;
            keySubStyle3.BackgroundImageFileName = "";
            serializableFont3.AlternateFontFamily = null;
            serializableFont3.DownloadUrl = null;
            serializableFont3.FontFamily = "Courier New";
            serializableFont3.Size = 10F;
            serializableFont3.Style = Extra.SerializableFontStyle.Regular;
            keySubStyle3.Font = serializableFont3;
            serializableColor8.Blue = 0;
            serializableColor8.Green = 0;
            serializableColor8.Red = 0;
            keySubStyle3.Outline = serializableColor8;
            keySubStyle3.OutlineWidth = 1;
            keySubStyle3.ShowOutline = false;
            serializableColor9.Blue = 0;
            serializableColor9.Green = 0;
            serializableColor9.Red = 0;
            keySubStyle3.Text = serializableColor9;
            pressed.SubStyle = keySubStyle3;
            pressed.TabIndex = 13;
            pressed.Title = "Pressed";
            loose.Location = new System.Drawing.Point(10, 10);
            loose.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            loose.Name = "loose";
            loose.Size = new System.Drawing.Size(200, 395);
            serializableColor10.Blue = 0;
            serializableColor10.Green = 0;
            serializableColor10.Red = 0;
            keySubStyle4.Background = serializableColor10;
            keySubStyle4.BackgroundImageFileName = "";
            serializableFont4.AlternateFontFamily = null;
            serializableFont4.DownloadUrl = null;
            serializableFont4.FontFamily = "Courier New";
            serializableFont4.Size = 10F;
            serializableFont4.Style = Extra.SerializableFontStyle.Regular;
            keySubStyle4.Font = serializableFont4;
            serializableColor11.Blue = 0;
            serializableColor11.Green = 0;
            serializableColor11.Red = 0;
            keySubStyle4.Outline = serializableColor11;
            keySubStyle4.OutlineWidth = 1;
            keySubStyle4.ShowOutline = false;
            serializableColor12.Blue = 0;
            serializableColor12.Green = 0;
            serializableColor12.Red = 0;
            keySubStyle4.Text = serializableColor12;
            loose.SubStyle = keySubStyle4;
            loose.TabIndex = 12;
            loose.Title = "Loose";
            lblOutlineWarning.Location = new System.Drawing.Point(10, 450);
            lblOutlineWarning.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblOutlineWarning.Name = "lblOutlineWarning";
            lblOutlineWarning.Size = new System.Drawing.Size(200, 50);
            lblOutlineWarning.TabIndex = 20;
            lblOutlineWarning.Text = "Setting a smaller outline for pressed\r\nthan loose keys will show the loose\r\noutline behind the pressed key.";
            lblOutlineWarning.Visible = false;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = CancelButton2;
            ClientSize = new System.Drawing.Size(429, 506);
            Controls.Add(lblOutlineWarning);
            Controls.Add(chkOverwritePressed);
            Controls.Add(chkOverwriteLoose);
            Controls.Add(pressed);
            Controls.Add(loose);
            Controls.Add(CancelButton2);
            Controls.Add(AcceptButton2);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "KeyStyleForm";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Key Style";
            Load += KeyStyleForm_Load;
            ResumeLayout(false);

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
