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
    using System.Windows.Forms;

    partial class SettingsForm
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
            this.TrapGroup = new System.Windows.Forms.GroupBox();
            this.txtToggleKey = new System.Windows.Forms.TextBox();
            this.lblToggleKey = new System.Windows.Forms.Label();
            this.chkTrapKeyboard = new System.Windows.Forms.CheckBox();
            this.chkTrapMouse = new System.Windows.Forms.CheckBox();
            this.lblTrapping = new System.Windows.Forms.Label();
            this.InputGroup = new System.Windows.Forms.GroupBox();
            this.lblPressHold = new System.Windows.Forms.Label();
            this.udPressHold = new System.Windows.Forms.NumericUpDown();
            this.lblPresHoldDuration = new System.Windows.Forms.Label();
            this.chkMouseFromCenter = new System.Windows.Forms.CheckBox();
            this.udScrollHold = new System.Windows.Forms.NumericUpDown();
            this.udMouseSensitivity = new System.Windows.Forms.NumericUpDown();
            this.lblScrollHold = new System.Windows.Forms.Label();
            this.lblMouseSensititivy = new System.Windows.Forms.Label();
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelButton2 = new System.Windows.Forms.Button();
            this.CapitalizationGroup = new System.Windows.Forms.GroupBox();
            this.chkFollowShiftCapsSensitive = new System.Windows.Forms.CheckBox();
            this.lblFollowShift = new System.Windows.Forms.Label();
            this.chkFollowShiftCapsInsensitive = new System.Windows.Forms.CheckBox();
            this.rdbAlwaysLower = new System.Windows.Forms.RadioButton();
            this.rdbAlwaysCaps = new System.Windows.Forms.RadioButton();
            this.rdbFollowKeystate = new System.Windows.Forms.RadioButton();
            this.GeneralGroup = new System.Windows.Forms.GroupBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.TrapGroup.SuspendLayout();
            this.InputGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udPressHold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udScrollHold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udMouseSensitivity)).BeginInit();
            this.CapitalizationGroup.SuspendLayout();
            this.GeneralGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // TrapGroup
            // 
            this.TrapGroup.Controls.Add(this.txtToggleKey);
            this.TrapGroup.Controls.Add(this.lblToggleKey);
            this.TrapGroup.Controls.Add(this.chkTrapKeyboard);
            this.TrapGroup.Controls.Add(this.chkTrapMouse);
            this.TrapGroup.Controls.Add(this.lblTrapping);
            this.TrapGroup.Location = new System.Drawing.Point(206, 68);
            this.TrapGroup.Name = "TrapGroup";
            this.TrapGroup.Size = new System.Drawing.Size(203, 136);
            this.TrapGroup.TabIndex = 1;
            this.TrapGroup.TabStop = false;
            this.TrapGroup.Text = "Trapping";
            // 
            // txtToggleKey
            // 
            this.txtToggleKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtToggleKey.Location = new System.Drawing.Point(99, 89);
            this.txtToggleKey.Multiline = true;
            this.txtToggleKey.Name = "txtToggleKey";
            this.txtToggleKey.ReadOnly = true;
            this.txtToggleKey.Size = new System.Drawing.Size(92, 34);
            this.txtToggleKey.TabIndex = 5;
            this.txtToggleKey.TabStop = false;
            this.txtToggleKey.Text = "ScrollLock";
            this.txtToggleKey.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtToggleKey.DoubleClick += new System.EventHandler(this.txtToggleKey_DoubleClick);
            this.txtToggleKey.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtToggleKey_KeyUp);
            // 
            // lblToggleKey
            // 
            this.lblToggleKey.AutoSize = true;
            this.lblToggleKey.Location = new System.Drawing.Point(9, 97);
            this.lblToggleKey.Name = "lblToggleKey";
            this.lblToggleKey.Size = new System.Drawing.Size(84, 13);
            this.lblToggleKey.TabIndex = 3;
            this.lblToggleKey.Text = "Trap toggle key:";
            // 
            // chkTrapKeyboard
            // 
            this.chkTrapKeyboard.AutoSize = true;
            this.chkTrapKeyboard.Location = new System.Drawing.Point(95, 67);
            this.chkTrapKeyboard.Name = "chkTrapKeyboard";
            this.chkTrapKeyboard.Size = new System.Drawing.Size(96, 17);
            this.chkTrapKeyboard.TabIndex = 3;
            this.chkTrapKeyboard.Text = "Trap Keyboard";
            this.chkTrapKeyboard.UseVisualStyleBackColor = true;
            // 
            // chkTrapMouse
            // 
            this.chkTrapMouse.AutoSize = true;
            this.chkTrapMouse.Location = new System.Drawing.Point(6, 67);
            this.chkTrapMouse.Name = "chkTrapMouse";
            this.chkTrapMouse.Size = new System.Drawing.Size(83, 17);
            this.chkTrapMouse.TabIndex = 2;
            this.chkTrapMouse.Text = "Trap Mouse";
            this.chkTrapMouse.UseVisualStyleBackColor = true;
            // 
            // lblTrapping
            // 
            this.lblTrapping.Location = new System.Drawing.Point(6, 20);
            this.lblTrapping.Name = "lblTrapping";
            this.lblTrapping.Size = new System.Drawing.Size(194, 52);
            this.lblTrapping.TabIndex = 0;
            this.lblTrapping.Text = "Trapping the mouse or keyboard prevents the respective device\'s input from reachi" +
    "ng any other applications.";
            // 
            // InputGroup
            // 
            this.InputGroup.Controls.Add(this.lblPressHold);
            this.InputGroup.Controls.Add(this.udPressHold);
            this.InputGroup.Controls.Add(this.lblPresHoldDuration);
            this.InputGroup.Controls.Add(this.chkMouseFromCenter);
            this.InputGroup.Controls.Add(this.udScrollHold);
            this.InputGroup.Controls.Add(this.udMouseSensitivity);
            this.InputGroup.Controls.Add(this.lblScrollHold);
            this.InputGroup.Controls.Add(this.lblMouseSensititivy);
            this.InputGroup.Location = new System.Drawing.Point(13, 13);
            this.InputGroup.Name = "InputGroup";
            this.InputGroup.Size = new System.Drawing.Size(187, 191);
            this.InputGroup.TabIndex = 2;
            this.InputGroup.TabStop = false;
            this.InputGroup.Text = "Input";
            // 
            // lblPressHold
            // 
            this.lblPressHold.AutoSize = true;
            this.lblPressHold.Location = new System.Drawing.Point(6, 105);
            this.lblPressHold.Name = "lblPressHold";
            this.lblPressHold.Size = new System.Drawing.Size(142, 13);
            this.lblPressHold.TabIndex = 12;
            this.lblPressHold.Text = "Show keypresses for at least";
            // 
            // udPressHold
            // 
            this.udPressHold.Location = new System.Drawing.Point(26, 123);
            this.udPressHold.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.udPressHold.Name = "udPressHold";
            this.udPressHold.Size = new System.Drawing.Size(49, 20);
            this.udPressHold.TabIndex = 11;
            // 
            // lblPresHoldDuration
            // 
            this.lblPresHoldDuration.AutoSize = true;
            this.lblPresHoldDuration.Location = new System.Drawing.Point(81, 125);
            this.lblPresHoldDuration.Name = "lblPresHoldDuration";
            this.lblPresHoldDuration.Size = new System.Drawing.Size(20, 13);
            this.lblPresHoldDuration.TabIndex = 10;
            this.lblPresHoldDuration.Text = "ms";
            // 
            // chkMouseFromCenter
            // 
            this.chkMouseFromCenter.Location = new System.Drawing.Point(10, 67);
            this.chkMouseFromCenter.Name = "chkMouseFromCenter";
            this.chkMouseFromCenter.Size = new System.Drawing.Size(166, 35);
            this.chkMouseFromCenter.TabIndex = 9;
            this.chkMouseFromCenter.Text = "Calculate mouse speed from center of screen";
            this.chkMouseFromCenter.UseVisualStyleBackColor = true;
            // 
            // udScrollHold
            // 
            this.udScrollHold.Location = new System.Drawing.Point(104, 42);
            this.udScrollHold.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.udScrollHold.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udScrollHold.Name = "udScrollHold";
            this.udScrollHold.Size = new System.Drawing.Size(72, 20);
            this.udScrollHold.TabIndex = 1;
            this.udScrollHold.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // udMouseSensitivity
            // 
            this.udMouseSensitivity.Location = new System.Drawing.Point(104, 20);
            this.udMouseSensitivity.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.udMouseSensitivity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udMouseSensitivity.Name = "udMouseSensitivity";
            this.udMouseSensitivity.Size = new System.Drawing.Size(72, 20);
            this.udMouseSensitivity.TabIndex = 0;
            this.udMouseSensitivity.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // lblScrollHold
            // 
            this.lblScrollHold.AutoSize = true;
            this.lblScrollHold.Location = new System.Drawing.Point(7, 44);
            this.lblScrollHold.Name = "lblScrollHold";
            this.lblScrollHold.Size = new System.Drawing.Size(81, 13);
            this.lblScrollHold.TabIndex = 1;
            this.lblScrollHold.Text = "Scroll hold time:";
            // 
            // lblMouseSensititivy
            // 
            this.lblMouseSensititivy.AutoSize = true;
            this.lblMouseSensititivy.Location = new System.Drawing.Point(7, 20);
            this.lblMouseSensititivy.Name = "lblMouseSensititivy";
            this.lblMouseSensititivy.Size = new System.Drawing.Size(90, 13);
            this.lblMouseSensititivy.TabIndex = 0;
            this.lblMouseSensititivy.Text = "Mouse sensitivity:";
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(334, 306);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 7;
            this.OkButton.Text = "Ok";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // CancelButton2
            // 
            this.CancelButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton2.Location = new System.Drawing.Point(253, 306);
            this.CancelButton2.Name = "CancelButton2";
            this.CancelButton2.Size = new System.Drawing.Size(75, 23);
            this.CancelButton2.TabIndex = 6;
            this.CancelButton2.Text = "Cancel";
            this.CancelButton2.UseVisualStyleBackColor = true;
            // 
            // CapitalizationGroup
            // 
            this.CapitalizationGroup.Controls.Add(this.chkFollowShiftCapsSensitive);
            this.CapitalizationGroup.Controls.Add(this.lblFollowShift);
            this.CapitalizationGroup.Controls.Add(this.chkFollowShiftCapsInsensitive);
            this.CapitalizationGroup.Controls.Add(this.rdbAlwaysLower);
            this.CapitalizationGroup.Controls.Add(this.rdbAlwaysCaps);
            this.CapitalizationGroup.Controls.Add(this.rdbFollowKeystate);
            this.CapitalizationGroup.Location = new System.Drawing.Point(10, 210);
            this.CapitalizationGroup.Name = "CapitalizationGroup";
            this.CapitalizationGroup.Size = new System.Drawing.Size(396, 91);
            this.CapitalizationGroup.TabIndex = 8;
            this.CapitalizationGroup.TabStop = false;
            this.CapitalizationGroup.Text = "Capitalization of Keys";
            // 
            // chkFollowShiftCapsSensitive
            // 
            this.chkFollowShiftCapsSensitive.AutoSize = true;
            this.chkFollowShiftCapsSensitive.Location = new System.Drawing.Point(199, 65);
            this.chkFollowShiftCapsSensitive.Name = "chkFollowShiftCapsSensitive";
            this.chkFollowShiftCapsSensitive.Size = new System.Drawing.Size(146, 17);
            this.chkFollowShiftCapsSensitive.TabIndex = 5;
            this.chkFollowShiftCapsSensitive.Text = "Caps Lock sensitive keys";
            this.chkFollowShiftCapsSensitive.UseVisualStyleBackColor = true;
            // 
            // lblFollowShift
            // 
            this.lblFollowShift.AutoSize = true;
            this.lblFollowShift.Location = new System.Drawing.Point(199, 19);
            this.lblFollowShift.Name = "lblFollowShift";
            this.lblFollowShift.Size = new System.Drawing.Size(93, 13);
            this.lblFollowShift.TabIndex = 4;
            this.lblFollowShift.Text = "Still follow shift for:";
            // 
            // chkFollowShiftCapsInsensitive
            // 
            this.chkFollowShiftCapsInsensitive.AutoSize = true;
            this.chkFollowShiftCapsInsensitive.Location = new System.Drawing.Point(199, 42);
            this.chkFollowShiftCapsInsensitive.Name = "chkFollowShiftCapsInsensitive";
            this.chkFollowShiftCapsInsensitive.Size = new System.Drawing.Size(154, 17);
            this.chkFollowShiftCapsInsensitive.TabIndex = 3;
            this.chkFollowShiftCapsInsensitive.Text = "Caps Lock insensitive keys";
            this.chkFollowShiftCapsInsensitive.UseVisualStyleBackColor = true;
            // 
            // rdbAlwaysLower
            // 
            this.rdbAlwaysLower.AutoSize = true;
            this.rdbAlwaysLower.Location = new System.Drawing.Point(10, 65);
            this.rdbAlwaysLower.Name = "rdbAlwaysLower";
            this.rdbAlwaysLower.Size = new System.Drawing.Size(157, 17);
            this.rdbAlwaysLower.TabIndex = 2;
            this.rdbAlwaysLower.TabStop = true;
            this.rdbAlwaysLower.Text = "Show all buttons lower-case";
            this.rdbAlwaysLower.UseVisualStyleBackColor = true;
            // 
            // rdbAlwaysCaps
            // 
            this.rdbAlwaysCaps.AutoSize = true;
            this.rdbAlwaysCaps.Location = new System.Drawing.Point(10, 42);
            this.rdbAlwaysCaps.Name = "rdbAlwaysCaps";
            this.rdbAlwaysCaps.Size = new System.Drawing.Size(156, 17);
            this.rdbAlwaysCaps.TabIndex = 1;
            this.rdbAlwaysCaps.TabStop = true;
            this.rdbAlwaysCaps.Text = "Show all buttons capitalized";
            this.rdbAlwaysCaps.UseVisualStyleBackColor = true;
            // 
            // rdbFollowKeystate
            // 
            this.rdbFollowKeystate.AutoSize = true;
            this.rdbFollowKeystate.Location = new System.Drawing.Point(10, 19);
            this.rdbFollowKeystate.Name = "rdbFollowKeystate";
            this.rdbFollowKeystate.Size = new System.Drawing.Size(154, 17);
            this.rdbFollowKeystate.TabIndex = 0;
            this.rdbFollowKeystate.TabStop = true;
            this.rdbFollowKeystate.Text = "Follow Caps-Lock and Shift";
            this.rdbFollowKeystate.UseVisualStyleBackColor = true;
            this.rdbFollowKeystate.CheckedChanged += new System.EventHandler(this.rdbFollowKeystate_CheckedChanged);
            // 
            // GeneralGroup
            // 
            this.GeneralGroup.Controls.Add(this.lblTitle);
            this.GeneralGroup.Controls.Add(this.txtTitle);
            this.GeneralGroup.Location = new System.Drawing.Point(206, 12);
            this.GeneralGroup.Name = "GeneralGroup";
            this.GeneralGroup.Size = new System.Drawing.Size(203, 50);
            this.GeneralGroup.TabIndex = 9;
            this.GeneralGroup.TabStop = false;
            this.GeneralGroup.Text = "General";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(7, 22);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(68, 13);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Window title:";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(81, 19);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(92, 20);
            this.txtTitle.TabIndex = 0;
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.OkButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelButton2;
            this.ClientSize = new System.Drawing.Size(422, 342);
            this.Controls.Add(this.GeneralGroup);
            this.Controls.Add(this.CapitalizationGroup);
            this.Controls.Add(this.CancelButton2);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.InputGroup);
            this.Controls.Add(this.TrapGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SettingsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.TrapGroup.ResumeLayout(false);
            this.TrapGroup.PerformLayout();
            this.InputGroup.ResumeLayout(false);
            this.InputGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udPressHold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udScrollHold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udMouseSensitivity)).EndInit();
            this.CapitalizationGroup.ResumeLayout(false);
            this.CapitalizationGroup.PerformLayout();
            this.GeneralGroup.ResumeLayout(false);
            this.GeneralGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTrapping;
        private System.Windows.Forms.GroupBox TrapGroup;
        private System.Windows.Forms.GroupBox InputGroup;
        private System.Windows.Forms.Label lblMouseSensititivy;
        private System.Windows.Forms.Label lblScrollHold;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelButton2;
        internal System.Windows.Forms.CheckBox chkTrapMouse;
        internal System.Windows.Forms.CheckBox chkTrapKeyboard;
        internal System.Windows.Forms.NumericUpDown udMouseSensitivity;
        internal System.Windows.Forms.NumericUpDown udScrollHold;
        private System.Windows.Forms.Label lblToggleKey;
        private System.Windows.Forms.TextBox txtToggleKey;
        private GroupBox CapitalizationGroup;
        private RadioButton rdbAlwaysLower;
        private RadioButton rdbAlwaysCaps;
        private RadioButton rdbFollowKeystate;
        private CheckBox chkMouseFromCenter;
        private CheckBox chkFollowShiftCapsSensitive;
        private Label lblFollowShift;
        private CheckBox chkFollowShiftCapsInsensitive;
        private GroupBox GeneralGroup;
        private Label lblTitle;
        private TextBox txtTitle;
        private Label lblPresHoldDuration;
        private NumericUpDown udPressHold;
        private Label lblPressHold;
    }
}