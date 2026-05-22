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
    using System.Drawing;
    using System.Windows.Forms;

    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            TrapGroup = new GroupBox();
            txtToggleKey = new Button();
            lblToggleKey = new Label();
            chkTrapKeyboard = new CheckBox();
            chkTrapMouse = new CheckBox();
            lblTrapping = new Label();
            lblLanguage = new Label();
            SizeTransparencyGroup = new GroupBox();
            lblKeyboardScale = new Label();
            udKeyboardScale = new NumericUpDown();
            lblKeyboardScalePercent = new Label();
            lblOverlayTransparency = new Label();
            udOverlayTransparency = new NumericUpDown();
            lblOverlayTransparencyPercent = new Label();
            btnCycleLanguage = new Button();
            InputGroup = new GroupBox();
            lblPressHold = new Label();
            udPressHold = new NumericUpDown();
            lblPresHoldDuration = new Label();
            chkMouseFromCenter = new CheckBox();
            udScrollHold = new NumericUpDown();
            udMouseSensitivity = new NumericUpDown();
            lblScrollHold = new Label();
            lblMouseSensititivy = new Label();
            OkButton = new Button();
            ApplyButton = new Button();
            CancelButton2 = new Button();
            btnResetSettings = new Button();
            CapitalizationGroup = new GroupBox();
            chkFollowShiftCapsSensitive = new CheckBox();
            lblFollowShift = new Label();
            chkFollowShiftCapsInsensitive = new CheckBox();
            rdbAlwaysLower = new RadioButton();
            rdbAlwaysCaps = new RadioButton();
            rdbFollowKeystate = new RadioButton();
            GeneralGroup = new GroupBox();
            lblTitle = new Label();
            txtTitle = new TextBox();
            TrapGroup.SuspendLayout();
            SizeTransparencyGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)udKeyboardScale).BeginInit();
            ((System.ComponentModel.ISupportInitialize)udOverlayTransparency).BeginInit();
            InputGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)udPressHold).BeginInit();
            ((System.ComponentModel.ISupportInitialize)udScrollHold).BeginInit();
            ((System.ComponentModel.ISupportInitialize)udMouseSensitivity).BeginInit();
            CapitalizationGroup.SuspendLayout();
            GeneralGroup.SuspendLayout();
            SuspendLayout();
            // 
            // TrapGroup
            // 
            TrapGroup.Controls.Add(txtToggleKey);
            TrapGroup.Controls.Add(lblToggleKey);
            TrapGroup.Controls.Add(chkTrapKeyboard);
            TrapGroup.Controls.Add(chkTrapMouse);
            TrapGroup.Controls.Add(lblTrapping);
            TrapGroup.Location = new Point(250, 100);
            TrapGroup.Margin = new Padding(4, 3, 4, 3);
            TrapGroup.Name = "TrapGroup";
            TrapGroup.Padding = new Padding(4, 3, 4, 3);
            TrapGroup.Size = new Size(235, 180);
            TrapGroup.TabIndex = 1;
            TrapGroup.TabStop = false;
            TrapGroup.Text = "Trapping";
            // 
            // txtToggleKey
            // 
            txtToggleKey.Location = new Point(10, 150);
            txtToggleKey.Margin = new Padding(4, 3, 4, 3);
            txtToggleKey.Name = "txtToggleKey";
            txtToggleKey.Size = new Size(215, 23);
            txtToggleKey.TabIndex = 5;
            txtToggleKey.TabStop = false;
            txtToggleKey.Text = "Scroll Lock";
            txtToggleKey.UseVisualStyleBackColor = true;
            txtToggleKey.Click += txtToggleKey_Click;
            txtToggleKey.KeyUp += txtToggleKey_KeyUp;
            txtToggleKey.Leave += txtToggleKey_Leave;
            // 
            // lblToggleKey
            // 
            lblToggleKey.Location = new Point(10, 125);
            lblToggleKey.Margin = new Padding(4, 0, 4, 0);
            lblToggleKey.Name = "lblToggleKey";
            lblToggleKey.Size = new Size(215, 23);
            lblToggleKey.TabIndex = 3;
            lblToggleKey.Text = "Trap toggle key:";
            lblToggleKey.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // chkTrapKeyboard
            // 
            chkTrapKeyboard.Location = new Point(120, 85);
            chkTrapKeyboard.Margin = new Padding(4, 3, 4, 3);
            chkTrapKeyboard.Name = "chkTrapKeyboard";
            chkTrapKeyboard.Size = new Size(105, 23);
            chkTrapKeyboard.TabIndex = 3;
            chkTrapKeyboard.Text = "Trap Keyboard";
            chkTrapKeyboard.UseVisualStyleBackColor = true;
            chkTrapKeyboard.CheckedChanged += chkTrapKeyboard_CheckedChanged;
            // 
            // chkTrapMouse
            // 
            chkTrapMouse.Location = new Point(10, 85);
            chkTrapMouse.Margin = new Padding(4, 3, 4, 3);
            chkTrapMouse.Name = "chkTrapMouse";
            chkTrapMouse.Size = new Size(105, 23);
            chkTrapMouse.TabIndex = 2;
            chkTrapMouse.Text = "Trap Mouse";
            chkTrapMouse.UseVisualStyleBackColor = true;
            // 
            // lblTrapping
            // 
            lblTrapping.Location = new Point(10, 25);
            lblTrapping.Margin = new Padding(4, 0, 4, 0);
            lblTrapping.Name = "lblTrapping";
            lblTrapping.Size = new Size(215, 46);
            lblTrapping.TabIndex = 0;
            lblTrapping.Text = "Trapping the mouse or keyboard prevents the respective device's input from reaching any other applications.";
            // 
            // lblLanguage
            // 
            lblLanguage.Location = new Point(10, 55);
            lblLanguage.Margin = new Padding(4, 0, 4, 0);
            lblLanguage.Name = "lblLanguage";
            lblLanguage.Size = new Size(130, 23);
            lblLanguage.TabIndex = 2;
            lblLanguage.Text = "Language";
            lblLanguage.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // SizeTransparencyGroup
            // 
            SizeTransparencyGroup.Controls.Add(lblKeyboardScale);
            SizeTransparencyGroup.Controls.Add(udKeyboardScale);
            SizeTransparencyGroup.Controls.Add(lblKeyboardScalePercent);
            SizeTransparencyGroup.Controls.Add(lblOverlayTransparency);
            SizeTransparencyGroup.Controls.Add(udOverlayTransparency);
            SizeTransparencyGroup.Controls.Add(lblOverlayTransparencyPercent);
            SizeTransparencyGroup.Location = new Point(250, 10);
            SizeTransparencyGroup.Margin = new Padding(4, 3, 4, 3);
            SizeTransparencyGroup.Name = "SizeTransparencyGroup";
            SizeTransparencyGroup.Padding = new Padding(4, 3, 4, 3);
            SizeTransparencyGroup.Size = new Size(235, 85);
            SizeTransparencyGroup.TabIndex = 11;
            SizeTransparencyGroup.TabStop = false;
            SizeTransparencyGroup.Text = "Size and transparency";
            // 
            // lblKeyboardScale
            // 
            lblKeyboardScale.Location = new Point(10, 25);
            lblKeyboardScale.Margin = new Padding(4, 0, 4, 0);
            lblKeyboardScale.Name = "lblKeyboardScale";
            lblKeyboardScale.Size = new Size(130, 23);
            lblKeyboardScale.TabIndex = 9;
            lblKeyboardScale.Text = "Scale size";
            lblKeyboardScale.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // udKeyboardScale
            // 
            udKeyboardScale.Location = new Point(145, 25);
            udKeyboardScale.Margin = new Padding(4, 3, 4, 3);
            udKeyboardScale.Maximum = new decimal(new int[] { 300, 0, 0, 0 });
            udKeyboardScale.Minimum = new decimal(new int[] { 25, 0, 0, 0 });
            udKeyboardScale.Name = "udKeyboardScale";
            udKeyboardScale.Size = new Size(50, 23);
            udKeyboardScale.TabIndex = 10;
            udKeyboardScale.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // lblKeyboardScalePercent
            // 
            lblKeyboardScalePercent.Location = new Point(195, 25);
            lblKeyboardScalePercent.Margin = new Padding(4, 0, 4, 0);
            lblKeyboardScalePercent.Name = "lblKeyboardScalePercent";
            lblKeyboardScalePercent.Size = new Size(30, 23);
            lblKeyboardScalePercent.TabIndex = 11;
            lblKeyboardScalePercent.Text = "%";
            lblKeyboardScalePercent.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblOverlayTransparency
            // 
            lblOverlayTransparency.Location = new Point(10, 55);
            lblOverlayTransparency.Margin = new Padding(4, 0, 4, 0);
            lblOverlayTransparency.Name = "lblOverlayTransparency";
            lblOverlayTransparency.Size = new Size(130, 23);
            lblOverlayTransparency.TabIndex = 6;
            lblOverlayTransparency.Text = "Frame and fill transparency";
            lblOverlayTransparency.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // udOverlayTransparency
            // 
            udOverlayTransparency.Location = new Point(145, 55);
            udOverlayTransparency.Margin = new Padding(4, 3, 4, 3);
            udOverlayTransparency.Maximum = new decimal(new int[] { 99, 0, 0, 0 });
            udOverlayTransparency.Name = "udOverlayTransparency";
            udOverlayTransparency.Size = new Size(50, 23);
            udOverlayTransparency.TabIndex = 7;
            // 
            // lblOverlayTransparencyPercent
            // 
            lblOverlayTransparencyPercent.Location = new Point(195, 55);
            lblOverlayTransparencyPercent.Margin = new Padding(4, 0, 4, 0);
            lblOverlayTransparencyPercent.Name = "lblOverlayTransparencyPercent";
            lblOverlayTransparencyPercent.Size = new Size(30, 23);
            lblOverlayTransparencyPercent.TabIndex = 8;
            lblOverlayTransparencyPercent.Text = "%";
            lblOverlayTransparencyPercent.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnCycleLanguage
            // 
            btnCycleLanguage.Location = new Point(145, 55);
            btnCycleLanguage.Margin = new Padding(4, 3, 4, 3);
            btnCycleLanguage.Name = "btnCycleLanguage";
            btnCycleLanguage.Size = new Size(80, 23);
            btnCycleLanguage.TabIndex = 0;
            btnCycleLanguage.TabStop = false;
            btnCycleLanguage.Text = "繁體中文";
            btnCycleLanguage.UseVisualStyleBackColor = true;
            btnCycleLanguage.Click += btnCycleLanguage_Click;
            // 
            // InputGroup
            // 
            InputGroup.Controls.Add(lblPressHold);
            InputGroup.Controls.Add(udPressHold);
            InputGroup.Controls.Add(lblPresHoldDuration);
            InputGroup.Controls.Add(chkMouseFromCenter);
            InputGroup.Controls.Add(udScrollHold);
            InputGroup.Controls.Add(udMouseSensitivity);
            InputGroup.Controls.Add(lblScrollHold);
            InputGroup.Controls.Add(lblMouseSensititivy);
            InputGroup.Location = new Point(10, 100);
            InputGroup.Margin = new Padding(4, 3, 4, 3);
            InputGroup.Name = "InputGroup";
            InputGroup.Padding = new Padding(4, 3, 4, 3);
            InputGroup.Size = new Size(235, 180);
            InputGroup.TabIndex = 2;
            InputGroup.TabStop = false;
            InputGroup.Text = "Input";
            // 
            // lblPressHold
            // 
            lblPressHold.Location = new Point(10, 125);
            lblPressHold.Margin = new Padding(4, 0, 4, 0);
            lblPressHold.Name = "lblPressHold";
            lblPressHold.Size = new Size(215, 23);
            lblPressHold.TabIndex = 12;
            lblPressHold.Text = "Show keypresses for at least";
            lblPressHold.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // udPressHold
            // 
            udPressHold.Location = new Point(10, 150);
            udPressHold.Margin = new Padding(4, 3, 4, 3);
            udPressHold.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            udPressHold.Name = "udPressHold";
            udPressHold.Size = new Size(80, 23);
            udPressHold.TabIndex = 11;
            // 
            // lblPresHoldDuration
            // 
            lblPresHoldDuration.Location = new Point(90, 150);
            lblPresHoldDuration.Margin = new Padding(4, 0, 4, 0);
            lblPresHoldDuration.Name = "lblPresHoldDuration";
            lblPresHoldDuration.Size = new Size(50, 23);
            lblPresHoldDuration.TabIndex = 10;
            lblPresHoldDuration.Text = "ms";
            lblPresHoldDuration.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // chkMouseFromCenter
            // 
            chkMouseFromCenter.Location = new Point(10, 85);
            chkMouseFromCenter.Margin = new Padding(4, 3, 4, 3);
            chkMouseFromCenter.Name = "chkMouseFromCenter";
            chkMouseFromCenter.Size = new Size(215, 35);
            chkMouseFromCenter.TabIndex = 9;
            chkMouseFromCenter.Text = "Calculate mouse speed from center of screen";
            chkMouseFromCenter.UseVisualStyleBackColor = true;
            // 
            // udScrollHold
            // 
            udScrollHold.Location = new Point(145, 55);
            udScrollHold.Margin = new Padding(4, 3, 4, 3);
            udScrollHold.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            udScrollHold.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            udScrollHold.Name = "udScrollHold";
            udScrollHold.Size = new Size(80, 23);
            udScrollHold.TabIndex = 1;
            udScrollHold.Value = new decimal(new int[] { 50, 0, 0, 0 });
            // 
            // udMouseSensitivity
            // 
            udMouseSensitivity.Location = new Point(145, 25);
            udMouseSensitivity.Margin = new Padding(4, 3, 4, 3);
            udMouseSensitivity.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            udMouseSensitivity.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            udMouseSensitivity.Name = "udMouseSensitivity";
            udMouseSensitivity.Size = new Size(80, 23);
            udMouseSensitivity.TabIndex = 0;
            udMouseSensitivity.Value = new decimal(new int[] { 50, 0, 0, 0 });
            // 
            // lblScrollHold
            // 
            lblScrollHold.Location = new Point(10, 55);
            lblScrollHold.Margin = new Padding(4, 0, 4, 0);
            lblScrollHold.Name = "lblScrollHold";
            lblScrollHold.Size = new Size(130, 23);
            lblScrollHold.TabIndex = 1;
            lblScrollHold.Text = "Scroll hold time:";
            lblScrollHold.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblMouseSensititivy
            // 
            lblMouseSensititivy.Location = new Point(10, 25);
            lblMouseSensititivy.Margin = new Padding(4, 0, 4, 0);
            lblMouseSensititivy.Name = "lblMouseSensititivy";
            lblMouseSensititivy.Size = new Size(130, 23);
            lblMouseSensititivy.TabIndex = 0;
            lblMouseSensititivy.Text = "Mouse sensitivity:";
            lblMouseSensititivy.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // OkButton
            // 
            OkButton.Location = new Point(405, 400);
            OkButton.Margin = new Padding(4, 3, 4, 3);
            OkButton.Name = "OkButton";
            OkButton.Size = new Size(80, 23);
            OkButton.TabIndex = 8;
            OkButton.Text = "Ok";
            OkButton.UseVisualStyleBackColor = true;
            OkButton.Click += OkButton_Click;
            // 
            // ApplyButton
            // 
            ApplyButton.Location = new Point(320, 400);
            ApplyButton.Margin = new Padding(4, 3, 4, 3);
            ApplyButton.Name = "ApplyButton";
            ApplyButton.Size = new Size(80, 23);
            ApplyButton.TabIndex = 7;
            ApplyButton.TabStop = false;
            ApplyButton.Text = "Apply";
            ApplyButton.UseVisualStyleBackColor = true;
            ApplyButton.Click += ApplyButton_Click;
            // 
            // CancelButton2
            // 
            CancelButton2.DialogResult = DialogResult.Cancel;
            CancelButton2.Location = new Point(235, 400);
            CancelButton2.Margin = new Padding(4, 3, 4, 3);
            CancelButton2.Name = "CancelButton2";
            CancelButton2.Size = new Size(80, 23);
            CancelButton2.TabIndex = 6;
            CancelButton2.Text = "Cancel";
            CancelButton2.UseVisualStyleBackColor = true;
            // 
            // btnResetSettings
            // 
            btnResetSettings.Location = new Point(10, 400);
            btnResetSettings.Margin = new Padding(4, 3, 4, 3);
            btnResetSettings.Name = "btnResetSettings";
            btnResetSettings.Size = new Size(120, 23);
            btnResetSettings.TabIndex = 5;
            btnResetSettings.Text = "Reset settings";
            btnResetSettings.UseVisualStyleBackColor = true;
            btnResetSettings.Click += btnResetSettings_Click;
            // 
            // CapitalizationGroup
            // 
            CapitalizationGroup.Controls.Add(chkFollowShiftCapsSensitive);
            CapitalizationGroup.Controls.Add(lblFollowShift);
            CapitalizationGroup.Controls.Add(chkFollowShiftCapsInsensitive);
            CapitalizationGroup.Controls.Add(rdbAlwaysLower);
            CapitalizationGroup.Controls.Add(rdbAlwaysCaps);
            CapitalizationGroup.Controls.Add(rdbFollowKeystate);
            CapitalizationGroup.Location = new Point(10, 285);
            CapitalizationGroup.Margin = new Padding(4, 3, 4, 3);
            CapitalizationGroup.Name = "CapitalizationGroup";
            CapitalizationGroup.Padding = new Padding(4, 3, 4, 3);
            CapitalizationGroup.Size = new Size(475, 110);
            CapitalizationGroup.TabIndex = 8;
            CapitalizationGroup.TabStop = false;
            CapitalizationGroup.Text = "Capitalization of Keys";
            // 
            // chkFollowShiftCapsSensitive
            // 
            chkFollowShiftCapsSensitive.Location = new Point(240, 85);
            chkFollowShiftCapsSensitive.Margin = new Padding(4, 3, 4, 3);
            chkFollowShiftCapsSensitive.Name = "chkFollowShiftCapsSensitive";
            chkFollowShiftCapsSensitive.Size = new Size(225, 23);
            chkFollowShiftCapsSensitive.TabIndex = 5;
            chkFollowShiftCapsSensitive.Text = "Caps Lock sensitive keys";
            chkFollowShiftCapsSensitive.UseVisualStyleBackColor = true;
            // 
            // lblFollowShift
            // 
            lblFollowShift.Location = new Point(240, 25);
            lblFollowShift.Margin = new Padding(4, 0, 4, 0);
            lblFollowShift.Name = "lblFollowShift";
            lblFollowShift.Size = new Size(225, 23);
            lblFollowShift.TabIndex = 4;
            lblFollowShift.Text = "Still follow shift for:";
            lblFollowShift.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // chkFollowShiftCapsInsensitive
            // 
            chkFollowShiftCapsInsensitive.Location = new Point(240, 55);
            chkFollowShiftCapsInsensitive.Margin = new Padding(4, 3, 4, 3);
            chkFollowShiftCapsInsensitive.Name = "chkFollowShiftCapsInsensitive";
            chkFollowShiftCapsInsensitive.Size = new Size(225, 23);
            chkFollowShiftCapsInsensitive.TabIndex = 3;
            chkFollowShiftCapsInsensitive.Text = "Caps Lock insensitive keys";
            chkFollowShiftCapsInsensitive.UseVisualStyleBackColor = true;
            // 
            // rdbAlwaysLower
            // 
            rdbAlwaysLower.Location = new Point(10, 85);
            rdbAlwaysLower.Margin = new Padding(4, 3, 4, 3);
            rdbAlwaysLower.Name = "rdbAlwaysLower";
            rdbAlwaysLower.Size = new Size(225, 23);
            rdbAlwaysLower.TabIndex = 2;
            rdbAlwaysLower.TabStop = true;
            rdbAlwaysLower.Text = "Show all buttons lower-case";
            rdbAlwaysLower.UseVisualStyleBackColor = true;
            // 
            // rdbAlwaysCaps
            // 
            rdbAlwaysCaps.Location = new Point(10, 55);
            rdbAlwaysCaps.Margin = new Padding(4, 3, 4, 3);
            rdbAlwaysCaps.Name = "rdbAlwaysCaps";
            rdbAlwaysCaps.Size = new Size(225, 23);
            rdbAlwaysCaps.TabIndex = 1;
            rdbAlwaysCaps.TabStop = true;
            rdbAlwaysCaps.Text = "Show all buttons capitalized";
            rdbAlwaysCaps.UseVisualStyleBackColor = true;
            // 
            // rdbFollowKeystate
            // 
            rdbFollowKeystate.Location = new Point(10, 25);
            rdbFollowKeystate.Margin = new Padding(4, 3, 4, 3);
            rdbFollowKeystate.Name = "rdbFollowKeystate";
            rdbFollowKeystate.Size = new Size(225, 23);
            rdbFollowKeystate.TabIndex = 0;
            rdbFollowKeystate.TabStop = true;
            rdbFollowKeystate.Text = "Follow Caps-Lock and Shift";
            rdbFollowKeystate.UseVisualStyleBackColor = true;
            rdbFollowKeystate.CheckedChanged += rdbFollowKeystate_CheckedChanged;
            // 
            // GeneralGroup
            // 
            GeneralGroup.Controls.Add(lblLanguage);
            GeneralGroup.Controls.Add(btnCycleLanguage);
            GeneralGroup.Controls.Add(lblTitle);
            GeneralGroup.Controls.Add(txtTitle);
            GeneralGroup.Location = new Point(10, 10);
            GeneralGroup.Margin = new Padding(4, 3, 4, 3);
            GeneralGroup.Name = "GeneralGroup";
            GeneralGroup.Padding = new Padding(4, 3, 4, 3);
            GeneralGroup.Size = new Size(235, 85);
            GeneralGroup.TabIndex = 9;
            GeneralGroup.TabStop = false;
            GeneralGroup.Text = "General";
            // 
            // lblTitle
            // 
            lblTitle.Location = new Point(10, 25);
            lblTitle.Margin = new Padding(4, 0, 4, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(130, 23);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Custom window title:";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtTitle
            // 
            txtTitle.Location = new Point(145, 25);
            txtTitle.Margin = new Padding(4, 3, 4, 3);
            txtTitle.Name = "txtTitle";
            txtTitle.Size = new Size(80, 23);
            txtTitle.TabIndex = 0;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = CancelButton2;
            ClientSize = new Size(494, 431);
            Controls.Add(GeneralGroup);
            Controls.Add(SizeTransparencyGroup);
            Controls.Add(CapitalizationGroup);
            Controls.Add(InputGroup);
            Controls.Add(TrapGroup);
            Controls.Add(btnResetSettings);
            Controls.Add(CancelButton2);
            Controls.Add(ApplyButton);
            Controls.Add(OkButton);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Margin = new Padding(4, 3, 4, 3);
            Name = "SettingsForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Settings";
            Deactivate += SettingsForm_Deactivate;
            Load += SettingsForm_Load;
            TrapGroup.ResumeLayout(false);
            SizeTransparencyGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)udKeyboardScale).EndInit();
            ((System.ComponentModel.ISupportInitialize)udOverlayTransparency).EndInit();
            InputGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)udPressHold).EndInit();
            ((System.ComponentModel.ISupportInitialize)udScrollHold).EndInit();
            ((System.ComponentModel.ISupportInitialize)udMouseSensitivity).EndInit();
            CapitalizationGroup.ResumeLayout(false);
            GeneralGroup.ResumeLayout(false);
            GeneralGroup.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTrapping;
        private System.Windows.Forms.GroupBox TrapGroup;
        private System.Windows.Forms.GroupBox InputGroup;
        private System.Windows.Forms.Label lblMouseSensititivy;
        private System.Windows.Forms.Label lblScrollHold;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.Button CancelButton2;
        private System.Windows.Forms.Button btnResetSettings;
        internal System.Windows.Forms.CheckBox chkTrapMouse;
        internal System.Windows.Forms.CheckBox chkTrapKeyboard;
        internal System.Windows.Forms.NumericUpDown udMouseSensitivity;
        internal System.Windows.Forms.NumericUpDown udScrollHold;
        private System.Windows.Forms.Label lblToggleKey;
        private System.Windows.Forms.Button txtToggleKey;
        private GroupBox CapitalizationGroup;
        private RadioButton rdbAlwaysLower;
        private RadioButton rdbAlwaysCaps;
        private RadioButton rdbFollowKeystate;
        private CheckBox chkMouseFromCenter;
        private CheckBox chkFollowShiftCapsSensitive;
        private Label lblFollowShift;
        private CheckBox chkFollowShiftCapsInsensitive;
        private GroupBox GeneralGroup;
        private GroupBox SizeTransparencyGroup;
        private Label lblLanguage;
        private Label lblKeyboardScale;
        private NumericUpDown udKeyboardScale;
        private Label lblKeyboardScalePercent;
        private Label lblTitle;
        private TextBox txtTitle;
        private Label lblPresHoldDuration;
        private NumericUpDown udPressHold;
        private Label lblPressHold;
        private Button btnCycleLanguage;
        private Label lblOverlayTransparency;
        private NumericUpDown udOverlayTransparency;
        private Label lblOverlayTransparencyPercent;
    }
}
