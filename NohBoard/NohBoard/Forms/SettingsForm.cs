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
    using System;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using Extra;
    using Hooking;
    using ThoNohT.NohBoard.Hooking.Interop;

    public partial class SettingsForm : Form
    {
        private bool capturingKey = false;

        private int trapToggleKey;

        private string previewUiLanguage;

        public SettingsForm()
        {
            this.InitializeComponent();
            this.AttachCaptureCancelMouseHandlers(this);
        }

        private void SettingsForm_Load(object sender, System.EventArgs e)
        {
            var main = Application.OpenForms.OfType<MainForm>().FirstOrDefault(f => !f.IsDisposed);
            if (main != null)
                FormPlacement.AlignModalDialog(main, this);
            else
                this.StartPosition = FormStartPosition.CenterScreen;

            this.previewUiLanguage = UiLanguageCode.Normalize(GlobalSettings.Settings.UiLanguage);
            this.ApplyUiLanguage();

            this.BindControlsFromGlobalSettings();
        }

        private void BindControlsFromGlobalSettings()
        {
            this.udMouseSensitivity.Value = GlobalSettings.Settings.MouseSensitivity;
            this.udScrollHold.Value = GlobalSettings.Settings.ScrollHold;

            this.chkTrapKeyboard.Checked = GlobalSettings.Settings.TrapKeyboard;
            this.chkTrapMouse.Checked = GlobalSettings.Settings.TrapMouse;

            this.trapToggleKey = GlobalSettings.Settings.TrapToggleKeyCode;
            this.txtToggleKey.Text = FormatTrapToggleKeyForDisplay((Keys)this.trapToggleKey);
            this.SyncTrapToggleKeyEditorAlignment();

            switch (GlobalSettings.Settings.Capitalization)
            {
                case CapitalizationMethod.Capitalize:
                    this.rdbAlwaysCaps.Checked = true;
                    break;
                case CapitalizationMethod.Lowercase:
                    this.rdbAlwaysLower.Checked = true;
                    break;
                case CapitalizationMethod.FollowKeys:
                    this.rdbFollowKeystate.Checked = true;
                    break;
            }

            this.chkFollowShiftCapsInsensitive.Enabled = !this.rdbFollowKeystate.Checked;
            this.chkFollowShiftCapsSensitive.Enabled = !this.rdbFollowKeystate.Checked;
            this.chkFollowShiftCapsInsensitive.Checked = GlobalSettings.Settings.FollowShiftForCapsInsensitive;
            this.chkFollowShiftCapsSensitive.Checked = GlobalSettings.Settings.FollowShiftForCapsSensitive;

            this.chkMouseFromCenter.Checked = GlobalSettings.Settings.MouseFromCenter;

            this.txtTitle.Text = GlobalSettings.Settings.WindowTitle ?? string.Empty;

            this.udPressHold.Value = GlobalSettings.Settings.PressHold;

            this.udKeyboardScale.Value = GlobalSettings.Settings.KeyboardScalePercent;
            this.udOverlayTransparency.Value = GlobalSettings.Settings.OverlayTransparencyPercent;
        }

        private static void ApplySettingsDialogDefaultsFromFactory()
        {
            var d = new GlobalSettings();
            var s = GlobalSettings.Settings;
            s.MouseSensitivity = d.MouseSensitivity;
            s.ScrollHold = d.ScrollHold;
            s.TrapKeyboard = d.TrapKeyboard;
            s.TrapMouse = d.TrapMouse;
            s.TrapToggleKeyCode = d.TrapToggleKeyCode;
            s.Capitalization = d.Capitalization;
            s.FollowShiftForCapsInsensitive = d.FollowShiftForCapsInsensitive;
            s.FollowShiftForCapsSensitive = d.FollowShiftForCapsSensitive;
            s.MouseFromCenter = d.MouseFromCenter;
            s.WindowTitle = d.WindowTitle ?? string.Empty;
            s.PressHold = d.PressHold;
            s.KeyboardScalePercent = d.KeyboardScalePercent;
            s.OverlayTransparencyPercent = d.OverlayTransparencyPercent;
            s.UiLanguage = UiLanguageCode.Normalize(d.UiLanguage);
        }

        private void btnResetSettings_Click(object sender, EventArgs e)
        {
            ApplySettingsDialogDefaultsFromFactory();

            Func<Rectangle, Point> getCenter = r => r.Location + new Size(r.Width / 2, r.Height / 2);
            MouseState.SetMouseFromCenter(GlobalSettings.Settings.MouseFromCenter, Screen.AllScreens.Select(x => (x.Bounds, getCenter(x.Bounds))).ToList());

            try
            {
                GlobalSettings.Save();
            }
            catch (Exception ex)
            {
                AppModalUi.ShowMessageBox(
                    this,
                    $"無法儲存設定檔：{Environment.NewLine}{ex.Message}",
                    "NohBoard",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            this.previewUiLanguage = UiLanguageCode.Normalize(GlobalSettings.Settings.UiLanguage);
            this.ApplyUiLanguage();
            this.BindControlsFromGlobalSettings();
            this.SyncTrapToggleKeyEditorAlignment();
            this.ClearActiveButtonFocus();
        }

        private void btnCycleLanguage_Click(object sender, System.EventArgs e)
        {
            this.previewUiLanguage = NextUiLanguage(this.previewUiLanguage);
            this.ApplyUiLanguage();
            this.ClearActiveButtonFocus();
        }

        private static string NextUiLanguage(string current) =>
            current switch
            {
                UiLanguageCode.ZhTw => UiLanguageCode.ZhCn,
                UiLanguageCode.ZhCn => UiLanguageCode.JaJp,
                UiLanguageCode.JaJp => UiLanguageCode.EnUs,
                UiLanguageCode.EnUs => UiLanguageCode.ZhTw,
                _ => UiLanguageCode.ZhTw,
            };

        private void ApplyUiLanguage()
        {
            var lang = this.previewUiLanguage;
            this.Text = UiTranslate.T(lang, "Settings", "設定", "设置", "設定");
            this.lblLanguage.Text = UiTranslate.T(lang, "Language", "語言", "语言", "言語");
            this.btnCycleLanguage.Text = UiTranslate.LanguageDisplayName(lang);

            this.InputGroup.Text = UiTranslate.T(lang, "Input", "輸入", "输入", "入力");
            this.lblMouseSensititivy.Text = UiTranslate.T(lang, "Mouse sensitivity:", "滑鼠靈敏度：", "鼠标灵敏度：", "マウス感度：");
            this.lblScrollHold.Text = UiTranslate.T(lang, "Scroll hold time:", "捲動保持時間：", "滚动保持时间：", "スクロール保持時間：");
            this.chkMouseFromCenter.Text = UiTranslate.T(
                lang,
                "Calculate mouse speed from center of screen",
                "從螢幕中央計算滑鼠速度",
                "从屏幕中央计算鼠标速度",
                "画面中央からマウス速度を計算");
            this.lblPressHold.Text = UiTranslate.T(lang, "Show keypresses for at least", "按鍵至少顯示", "按键至少显示", "キー表示を最低");
            this.lblPresHoldDuration.Text = UiTranslate.T(lang, "ms", "毫秒", "毫秒", "ミリ秒");

            this.GeneralGroup.Text = UiTranslate.T(lang, "General", "一般", "一般", "一般");
            this.SizeTransparencyGroup.Text = UiTranslate.T(lang, "Size and transparency", "大小及透明度", "大小及透明度", "サイズと透明度");
            this.lblTitle.Text = UiTranslate.T(lang, "Custom window title:", "自訂視窗標題：", "自定义窗口标题：", "表示するタイトル：");
            this.lblKeyboardScale.Text = UiTranslate.T(lang, "Scale size", "縮放大小", "缩放大小", "拡大率");

            this.TrapGroup.Text = UiTranslate.T(lang, "Trapping", "攔截", "拦截", "トラップ");
            this.lblTrapping.Text = UiTranslate.T(
                lang,
                "Trapping the mouse or keyboard prevents the respective device's input from reaching any other applications.",
                "攔截滑鼠或鍵盤時，該裝置的輸入將不會傳到其他應用程式。",
                "拦截鼠标或键盘时，该设备的输入将不会传到其他应用程序。",
                "マウスまたはキーボードをトラップすると、その入力は他のアプリに届きません。");
            this.chkTrapMouse.Text = UiTranslate.T(lang, "Trap Mouse", "攔截滑鼠", "拦截鼠标", "マウスをトラップ");
            this.chkTrapKeyboard.Text = UiTranslate.T(lang, "Trap Keyboard", "攔截鍵盤", "拦截键盘", "キーボードをトラップ");
            this.lblToggleKey.Text = UiTranslate.T(lang, "Trap toggle key:", "攔截切換鍵：", "拦截切换键：", "トラップ切替キー：");
            this.lblOverlayTransparency.Text = UiTranslate.T(
                lang,
                "Transparency",
                "透明度",
                "透明度",
                "透明度");
            this.lblKeyboardScalePercent.Text = UiTranslate.T(lang, "%", "%", "%", "％");
            this.lblOverlayTransparencyPercent.Text = UiTranslate.T(lang, "%", "%", "%", "％");

            this.CapitalizationGroup.Text = UiTranslate.T(lang, "Capitalization of Keys", "按鍵大小寫", "按键大小写", "キーの大文字・小文字");
            this.rdbFollowKeystate.Text = UiTranslate.T(lang, "Follow Caps-Lock and Shift", "依照 Caps Lock 與 Shift", "跟随 Caps Lock 与 Shift", "Caps Lock と Shift に従う");
            this.rdbAlwaysCaps.Text = UiTranslate.T(lang, "Show all buttons capitalized", "按鍵一律大寫", "按键一律大写", "常に大文字で表示");
            this.rdbAlwaysLower.Text = UiTranslate.T(lang, "Show all buttons lower-case", "按鍵一律小寫", "按键一律小写", "常に小文字で表示");
            this.lblFollowShift.Text = UiTranslate.T(lang, "Still follow shift for:", "仍依 Shift 切換：", "仍依 Shift 切换：", "Shift に従う：");
            this.chkFollowShiftCapsInsensitive.Text = UiTranslate.T(lang, "Caps Lock insensitive keys", "不受 Caps Lock 影響的鍵", "不受 Caps Lock 影响的键", "Caps Lock の影響を受けないキー");
            this.chkFollowShiftCapsSensitive.Text = UiTranslate.T(lang, "Caps Lock sensitive keys", "受 Caps Lock 影響的鍵", "受 Caps Lock 影响的键", "Caps Lock の影響を受けるキー");

            this.OkButton.Text = UiTranslate.T(lang, "Ok", "確定", "确定", "OK");
            this.ApplyButton.Text = UiTranslate.T(lang, "Apply", "套用", "应用", "適用");
            this.CancelButton2.Text = UiTranslate.T(lang, "Cancel", "取消", "取消", "キャンセル");
            this.btnResetSettings.Text = UiTranslate.T(lang, "Reset settings", "重置設定", "重置设置", "設定をリセット");
        }

        private void ApplyButton_Click(object sender, System.EventArgs e)
        {
            if (!this.TryCommitSettingsToGlobalAndSave())
                return;

            this.ApplyCommittedSettingsToMainWindow();
            this.ClearActiveButtonFocus();
        }

        private void OkButton_Click(object sender, System.EventArgs e)
        {
            if (!this.TryCommitSettingsToGlobalAndSave())
                return;

            this.DialogResult = DialogResult.OK;
        }

        private bool TryCommitSettingsToGlobalAndSave()
        {
            GlobalSettings.Settings.MouseSensitivity = (int)this.udMouseSensitivity.Value;
            GlobalSettings.Settings.ScrollHold = (int)this.udScrollHold.Value;

            GlobalSettings.Settings.TrapKeyboard = this.chkTrapKeyboard.Checked;
            GlobalSettings.Settings.TrapMouse = this.chkTrapMouse.Checked;
            GlobalSettings.Settings.TrapToggleKeyCode = this.trapToggleKey;

            GlobalSettings.Settings.Capitalization = this.rdbFollowKeystate.Checked
                ? CapitalizationMethod.FollowKeys
                : this.rdbAlwaysLower.Checked
                    ? CapitalizationMethod.Lowercase
                    : CapitalizationMethod.Capitalize;
            GlobalSettings.Settings.FollowShiftForCapsInsensitive = this.chkFollowShiftCapsInsensitive.Checked;
            GlobalSettings.Settings.FollowShiftForCapsSensitive = this.chkFollowShiftCapsSensitive.Checked;

            GlobalSettings.Settings.MouseFromCenter = this.chkMouseFromCenter.Checked;

            Func<Rectangle, Point> getCenter = r => r.Location + new Size(r.Width / 2, r.Height / 2);
            MouseState.SetMouseFromCenter(GlobalSettings.Settings.MouseFromCenter, Screen.AllScreens.Select(x => (x.Bounds, getCenter(x.Bounds))).ToList());

            GlobalSettings.Settings.WindowTitle = this.txtTitle.Text;

            GlobalSettings.Settings.PressHold = (int)this.udPressHold.Value;

            GlobalSettings.Settings.KeyboardScalePercent = (int)this.udKeyboardScale.Value;
            GlobalSettings.Settings.OverlayTransparencyPercent = (int)this.udOverlayTransparency.Value;

            GlobalSettings.Settings.UiLanguage = UiLanguageCode.Normalize(this.previewUiLanguage);

            try
            {
                GlobalSettings.Save();
            }
            catch (Exception ex)
            {
                AppModalUi.ShowMessageBox(
                    this,
                    $"無法儲存設定檔：{Environment.NewLine}{ex.Message}",
                    "NohBoard",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void ApplyCommittedSettingsToMainWindow()
        {
            var main = Application.OpenForms.OfType<MainForm>().FirstOrDefault(f => !f.IsDisposed);
            main?.ApplySettings();
        }

        private void txtToggleKey_KeyUp(object sender, KeyEventArgs e)
        {
            if (!this.capturingKey) return;

            if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                this.CancelTrapToggleCaptureAndRestoreText();
                return;
            }

            this.txtToggleKey.Text = FormatTrapToggleKeyForDisplay(e.KeyCode);
            this.trapToggleKey = (int)e.KeyCode;
            this.SyncTrapToggleKeyEditorAlignment();

            e.SuppressKeyPress = true;
            this.EndTrapToggleCapture();
        }

        private void txtToggleKey_Click(object sender, System.EventArgs e)
        {
            HookManager.DisableKeyboardHook();
            HookManager.DisableMouseHook();
            this.capturingKey = true;
            this.txtToggleKey.Text = UiTranslate.T(
                this.previewUiLanguage,
                "Press a key...",
                "請按下按鍵…",
                "请按下按键…",
                "キーを押してください…");
            this.SyncTrapToggleKeyEditorAlignment();
        }

        private static string FormatTrapToggleKeyForDisplay(Keys key)
        {
            if ((int)key == Defines.VK_SCROLL)
                return "Scroll Lock";
            return key.ToString();
        }

        private void SyncTrapToggleKeyEditorAlignment()
        {
            this.txtToggleKey.TextAlign = ContentAlignment.MiddleCenter;
        }

        private void rdbFollowKeystate_CheckedChanged(object sender, System.EventArgs e)
        {
            this.chkFollowShiftCapsInsensitive.Enabled = !this.rdbFollowKeystate.Checked;
            this.chkFollowShiftCapsSensitive.Enabled = !this.rdbFollowKeystate.Checked;
        }

        private void txtToggleKey_Leave(object sender, EventArgs e)
        {
            if (this.capturingKey)
            {
                this.CancelTrapToggleCaptureAndRestoreText();
            }
        }

        private void EndTrapToggleCapture()
        {
            this.capturingKey = false;
            HookManager.EnableMouseHook();
            HookManager.EnableKeyboardHook();
            if (ReferenceEquals(this.ActiveControl, this.txtToggleKey))
            {
                this.ActiveControl = null;
            }
        }

        private void AttachCaptureCancelMouseHandlers(Control root)
        {
            root.MouseDown += this.CancelCaptureOnMouseDown;
            foreach (Control child in root.Controls)
            {
                this.AttachCaptureCancelMouseHandlers(child);
            }
        }

        private void CancelCaptureOnMouseDown(object sender, MouseEventArgs e)
        {
            if (this.ActiveControl is Button activeButton && !ReferenceEquals(sender, activeButton))
            {
                this.ActiveControl = null;
            }

            if (!this.capturingKey) return;
            if (ReferenceEquals(sender, this.txtToggleKey)) return;

            this.CancelTrapToggleCaptureAndRestoreText();
        }

        private void ClearActiveButtonFocus()
        {
            if (this.ActiveControl is Button)
            {
                this.ActiveControl = null;
            }
        }

        private void CancelTrapToggleCaptureAndRestoreText()
        {
            this.EndTrapToggleCapture();
            this.txtToggleKey.Text = FormatTrapToggleKeyForDisplay((Keys)this.trapToggleKey);
            this.SyncTrapToggleKeyEditorAlignment();
        }

        private void SettingsForm_Deactivate(object sender, EventArgs e)
        {
            if (!this.capturingKey) return;
            this.CancelTrapToggleCaptureAndRestoreText();
        }

        private void chkTrapKeyboard_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void lblPresHoldDuration_Click(object sender, EventArgs e)
        {

        }

        private void udMouseSensitivity_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
