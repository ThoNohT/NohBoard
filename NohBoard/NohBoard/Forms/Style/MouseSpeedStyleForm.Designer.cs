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
            Keyboard.Styles.MouseSpeedIndicatorStyle mouseSpeedIndicatorStyle3 = new Keyboard.Styles.MouseSpeedIndicatorStyle();
            Extra.SerializableColor serializableColor5 = new Extra.SerializableColor();
            Extra.SerializableColor serializableColor6 = new Extra.SerializableColor();
            AcceptButton2 = new System.Windows.Forms.Button();
            CancelButton2 = new System.Windows.Forms.Button();
            defaultMouseSpeed = new MouseSpeedStylePanel();
            chkOverwrite = new System.Windows.Forms.CheckBox();
            SuspendLayout();
            AcceptButton2.Location = new System.Drawing.Point(128, 190);
            AcceptButton2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            AcceptButton2.Name = "AcceptButton2";
            AcceptButton2.Size = new System.Drawing.Size(80, 23);
            AcceptButton2.TabIndex = 10;
            AcceptButton2.Text = "Accept";
            AcceptButton2.UseVisualStyleBackColor = true;
            AcceptButton2.Click += AcceptButton2_Click;
            CancelButton2.Location = new System.Drawing.Point(40, 190);
            CancelButton2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CancelButton2.Name = "CancelButton2";
            CancelButton2.Size = new System.Drawing.Size(80, 23);
            CancelButton2.TabIndex = 11;
            CancelButton2.Text = "Cancel";
            CancelButton2.UseVisualStyleBackColor = true;
            CancelButton2.Click += CancelButton2_Click;
            serializableColor5.Blue = 0;
            serializableColor5.Green = 0;
            serializableColor5.Red = 0;
            mouseSpeedIndicatorStyle3.InnerColor = serializableColor5;
            serializableColor6.Blue = 0;
            serializableColor6.Green = 0;
            serializableColor6.Red = 0;
            mouseSpeedIndicatorStyle3.OuterColor = serializableColor6;
            mouseSpeedIndicatorStyle3.OutlineWidth = 1;
            defaultMouseSpeed.IndicatorStyle = mouseSpeedIndicatorStyle3;
            defaultMouseSpeed.Location = new System.Drawing.Point(10, 10);
            defaultMouseSpeed.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            defaultMouseSpeed.Name = "defaultMouseSpeed";
            defaultMouseSpeed.Size = new System.Drawing.Size(200, 145);
            defaultMouseSpeed.TabIndex = 16;
            defaultMouseSpeed.Title = "Mouse Speed";
            chkOverwrite.Location = new System.Drawing.Point(10, 160);
            chkOverwrite.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            chkOverwrite.Name = "chkOverwrite";
            chkOverwrite.Size = new System.Drawing.Size(200, 23);
            chkOverwrite.TabIndex = 17;
            chkOverwrite.Text = "Overwrite default style";
            chkOverwrite.UseVisualStyleBackColor = true;
            chkOverwrite.CheckedChanged += chkOverwrite_CheckedChanged;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = CancelButton2;
            ClientSize = new System.Drawing.Size(219, 221);
            Controls.Add(chkOverwrite);
            Controls.Add(defaultMouseSpeed);
            Controls.Add(CancelButton2);
            Controls.Add(AcceptButton2);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "MouseSpeedStyleForm";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Mouse Speed Indicator Style";
            Load += MouseSpeedStyleForm_Load;
            ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button AcceptButton2;
        private System.Windows.Forms.Button CancelButton2;
        private MouseSpeedStylePanel defaultMouseSpeed;
        private System.Windows.Forms.CheckBox chkOverwrite;
    }
}
