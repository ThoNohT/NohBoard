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
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using ThoNohT.NohBoard;
    using ThoNohT.NohBoard.Extra;

    public partial class MainForm
    {
        private const string TrayAboutUrl = "https://github.com/ThoNohT/NohBoard";

        private NotifyIcon _trayIcon;
        private ContextMenuStrip _trayMenu;
        private ToolStripMenuItem _mnuTrayShow;
        private ToolStripMenuItem _mnuTrayLock;
        private ToolStripMenuItem _mnuTraySettings;
        private ToolStripMenuItem _mnuTrayAbout;
        private ToolStripMenuItem _mnuTrayExit;
        private bool _exitingApp;
        private bool _exitingFromPeer;
        private bool _hidingToTray;

        private void InitializeTray()
        {
            this._trayIcon = new NotifyIcon(this.components)
            {
                Icon = this.Icon,
                Visible = true,
                Text = this.Text,
            };
            this._trayIcon.MouseClick += this.TrayIcon_MouseClick;
            this._trayIcon.DoubleClick += (_, _) =>
            {
                if (!this._overlayLocked)
                    this.RestoreFromTray();
            };

            this._trayMenu = new ContextMenuStrip();
            this._mnuTrayShow = new ToolStripMenuItem();
            this._mnuTrayShow.Click += (_, _) => this.RestoreFromTray();
            this._mnuTrayLock = new ToolStripMenuItem();
            this._mnuTrayLock.Click += (_, _) => this.ToggleOverlayLockFromTray();
            this._mnuTraySettings = new ToolStripMenuItem();
            this._mnuTraySettings.Click += (_, _) => this.TraySettings_Click();
            this._mnuTrayAbout = new ToolStripMenuItem();
            this._mnuTrayAbout.Click += (_, _) => this.TrayAbout_Click();
            this._mnuTrayExit = new ToolStripMenuItem();
            this._mnuTrayExit.Click += (_, _) => this.ExitApplication();
            this._trayMenu.Items.Add(this._mnuTrayShow);
            this._trayMenu.Items.Add(this._mnuTrayLock);
            this._trayMenu.Items.Add(this._mnuTraySettings);
            this._trayMenu.Items.Add(this._mnuTrayAbout);
            this._trayMenu.Items.Add(this._mnuTrayExit);
            this._trayIcon.ContextMenuStrip = this._trayMenu;
            this._trayMenu.Opening += (_, _) => this.IncrementSuspendLayeredKeyboardUpdates();
            this._trayMenu.Closed += (_, _) => this.DecrementSuspendLayeredKeyboardUpdates();

            this.Resize += this.MainForm_Resize;
            this.ApplyLocalizedTrayMenu();
        }

        private void ApplyLocalizedTrayMenu()
        {
            if (this._trayIcon == null)
                return;

            var L = UiTranslate.Lang;
            this._trayIcon.Text = this.Text;
            this._mnuTrayShow.Text = UiTranslate.T(L, "&Show", "顯示(&S)", "显示(&S)", "表示(&S)");
            this.ApplyLocalizedOverlayLockMenuText();
            this._mnuTrayLock.Text = this.mnuToggleOverlayLock.Text;
            this._mnuTraySettings.Text = UiTranslate.T(L, "&Settings", "設定(&S)", "设置(&S)", "設定(&S)");
            this._mnuTrayAbout.Text = UiTranslate.T(L, "&About", "關於(&A)", "关于(&A)", "バージョン情報(&A)");
            this._mnuTrayExit.Text = UiTranslate.T(L, "E&xit", "離開(&X)", "退出(&X)", "終了(&X)");
        }

        private void TraySettings_Click()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(this.TraySettings_Click));
                return;
            }

            this.OpenSettingsDialog();
        }

        private void ToggleOverlayLockFromTray()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(this.ToggleOverlayLockFromTray));
                return;
            }

            this.ToggleOverlayLock();
            this.ApplyLocalizedTrayMenu();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this._hidingToTray || this._exitingApp)
                return;

            if (this.WindowState == FormWindowState.Minimized)
                this.HideToTray();
        }

        private void HideToTray()
        {
            if (this._hidingToTray || !this.Visible)
                return;

            this._hidingToTray = true;
            try
            {
                this.ShowInTaskbar = false;
                this.Hide();
                this.WindowState = FormWindowState.Normal;
            }
            finally
            {
                this._hidingToTray = false;
            }
        }

        private void TrayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.RestoreFromTray();
        }

        private void RestoreFromTray()
        {
            if (this._overlayLocked)
                return;

            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
            FormPlacement.MoveMainFormToDefaultPosition(this);
            this.Show();
            FormPlacement.FocusMainForm(this);
            this.BeginInvoke(new Action(() => FormPlacement.FocusMainForm(this)));
        }

        private void TrayAbout_Click()
        {
            try
            {
                Process.Start(new ProcessStartInfo { FileName = TrayAboutUrl, UseShellExecute = true });
            }
            catch
            {
            }
        }

        private static bool HasUnsavedChangesPendingUserPrompt() =>
            (GlobalSettings.UnsavedDefinitionChanges || GlobalSettings.UnsavedStyleChanges) && !CrashHandler.Crashed;

        private bool TryConfirmDiscardUnsaved()
        {
            if (!HasUnsavedChangesPendingUserPrompt())
                return true;

            var result = this.ShowAppMessageBox(
                UiTranslate.T(
                    "You have unsaved changes. If you exit now you will lose them. Are you sure you want to exit?",
                    "有尚未儲存的變更。現在結束將會遺失這些變更。確定要結束嗎？",
                    "有未保存的更改。若现在退出将丢失这些更改。确定要退出吗？",
                    "未保存の変更があります。終了すると失われます。終了しますか？"),
                UiTranslate.T("Discard changes", "放棄變更", "放弃更改", "変更の破棄"),
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning);

            return result == DialogResult.OK;
        }

        internal void RequestQuitFromPeer()
        {
            if (this._exitingApp || this.IsDisposed)
                return;

            if (this.InvokeRequired)
            {
                try
                {
                    this.BeginInvoke(new Action(this.RequestQuitFromPeer));
                }
                catch
                {
                }

                return;
            }

            this._exitingFromPeer = true;
            this._exitingApp = true;

            this.ReleaseOverlayLock();
            this.DisposeTray();

            try
            {
                Hooking.Interop.HookManager.DisableMouseHook();
                Hooking.Interop.HookManager.DisableKeyboardHook();
            }
            catch
            {
            }

            try
            {
                GlobalSettings.Save();
            }
            catch
            {
            }

            SingleInstanceGuard.Release();
            Environment.Exit(0);
        }

        private void ExitApplication()
        {
            if (!this.TryConfirmDiscardUnsaved())
                return;

            this._exitingApp = true;
            this.BeginInvoke(new Action(() => this.Close()));
        }

        private void DisposeTray()
        {
            if (this._trayIcon == null)
                return;

            this._trayIcon.Visible = false;
            this._trayIcon.Dispose();
            this._trayIcon = null;
        }
    }
}
