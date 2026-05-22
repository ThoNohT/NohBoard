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
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public partial class MainForm
    {
        private const int GwlExstyle = -20;

        private const int WsExLayered = 0x00080000;

        private const int WsExTransparent = 0x00000020;

        private static readonly IntPtr HwndTopmost = new IntPtr(-1);

        private static readonly IntPtr HwndNotopmost = new IntPtr(-2);

        private const uint SwpNomove = 0x0002;

        private const uint SwpNosize = 0x0001;

        private const uint SwpNoactivate = 0x0010;

        private const uint SwpShowwindow = 0x0040;

        private bool _overlayLocked;

        private int _savedExStyle = -1;

        private bool _savedTopMost;

        private bool _captionHiddenForOverlayLock;

        private FormBorderStyle _restoreOverlayLockBorderStyle;

        private Rectangle _restoreOverlayLockBounds;

        internal bool OverlayLocked => this._overlayLocked;

        /// <summary>
        /// Locked overlay hides main-form caption as a 1×1 shell; avoid restoring chrome during layout refreshes.
        /// </summary>
        private bool ShouldPreserveLockedCaptionShell()
        {
            return this._overlayLocked
                && this._captionHiddenForOverlayLock
                && this.UsesLayeredOverlay();
        }

        private void InitializeOverlayLock()
        {
            this.HandleCreated += (_, _) =>
            {
                if (this._overlayLocked)
                    this.ApplyOverlayLockStyles();
            };
        }

        private void ToggleOverlayLock()
        {
            this.SetOverlayLock(!this._overlayLocked);
        }

        private void mnuToggleOverlayLock_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;
            this.ToggleOverlayLock();
            this.ApplyLocalizedMainMenu();
        }

        private void SetOverlayLock(bool locked)
        {
            if (locked)
            {
                if (!this.Visible)
                {
                    this.ShowInTaskbar = true;
                    this.Show();
                    this.WindowState = FormWindowState.Normal;
                }
            }

            this._overlayLocked = locked;

            if (this.IsHandleCreated)
                this.ApplyOverlayLockStyles();
        }

        private void ApplyOverlayLockStyles()
        {
            if (!this.IsHandleCreated)
                return;

            var hwnd = this.Handle;
            if (hwnd == IntPtr.Zero)
                return;

            if (this._overlayLocked)
            {
                if (this._savedExStyle < 0)
                {
                    this._savedExStyle = GetWindowLong(hwnd, GwlExstyle);
                    this._savedTopMost = this.TopMost;
                }

                this.HideCaptionChromeForLockedLayeredOverlay();

                if (this._keyboardSurface != null && this._keyboardSurface.IsHandleCreated)
                {
                    var panelHwnd = this._keyboardSurface.Handle;
                    var panelEx = GetWindowLong(panelHwnd, GwlExstyle);
                    SetWindowLong(panelHwnd, GwlExstyle, panelEx & ~WsExLayered);
                }

                this.TopMost = true;
                SetWindowLong(
                    hwnd,
                    GwlExstyle,
                    this._savedExStyle | WsExLayered | WsExTransparent);

                SetWindowPos(
                    hwnd,
                    HwndTopmost,
                    0,
                    0,
                    0,
                    0,
                    SwpNomove | SwpNosize | SwpNoactivate | SwpShowwindow);

                this.ApplyLayeredOverlayPointerStyles(true);
                this.PresentLayeredOverlay();
            }
            else
            {
                this.RestoreCaptionChromeAfterOverlayLock();

                this.TopMost = this._savedTopMost;

                if (this._savedExStyle >= 0)
                    SetWindowLong(hwnd, GwlExstyle, this._savedExStyle);

                this._savedExStyle = -1;

                SetWindowPos(
                    hwnd,
                    this.TopMost ? HwndTopmost : HwndNotopmost,
                    0,
                    0,
                    0,
                    0,
                    SwpNomove | SwpNosize | SwpNoactivate | SwpShowwindow);

                this.ApplyOverlayTransparencyStyle();
            }
        }

        private void HideCaptionChromeForLockedLayeredOverlay()
        {
            if (!this.UsesLayeredOverlay() || this._captionHiddenForOverlayLock)
                return;

            this._restoreOverlayLockBorderStyle = this.FormBorderStyle;
            this._restoreOverlayLockBounds = this.Bounds;

            var anchor = this.PointToScreen(Point.Empty);
            this.SuspendLayout();
            try
            {
                this.FormBorderStyle = FormBorderStyle.None;
                this.MinimumSize = new Size(0, 0);
                this.MaximumSize = Size.Empty;
                this.Bounds = new Rectangle(anchor, new Size(1, 1));
            }
            finally
            {
                this.ResumeLayout(true);
            }

            this._captionHiddenForOverlayLock = true;
        }

        private void RefreshLockedOverlayCaptionHide(int keyboardWidth)
        {
            if (!this._overlayLocked || !this.UsesLayeredOverlay())
                return;

            if (this._captionHiddenForOverlayLock)
                return;

            this.ShrinkMainFormToCaptionOnly(keyboardWidth);
            this.HideCaptionChromeForLockedLayeredOverlay();
        }

        internal void RestoreCaptionChromeAfterOverlayLock()
        {
            if (!this._captionHiddenForOverlayLock)
                return;

            this._captionHiddenForOverlayLock = false;
            this.SuspendLayout();
            try
            {
                this.FormBorderStyle = this._restoreOverlayLockBorderStyle;
                this.Bounds = this._restoreOverlayLockBounds;
            }
            finally
            {
                this.ResumeLayout(true);
            }
        }

        private void ReleaseOverlayLock()
        {
            if (!this._overlayLocked)
                return;

            this.SetOverlayLock(false);
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern int GetWindowLong32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        private static int GetWindowLong(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 8)
                return (int)GetWindowLongPtr64(hWnd, nIndex);

            return GetWindowLong32(hWnd, nIndex);
        }

        private static void SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong)
        {
            if (IntPtr.Size == 8)
                SetWindowLongPtr64(hWnd, nIndex, new IntPtr(dwNewLong));
            else
                SetWindowLong32(hWnd, nIndex, dwNewLong);
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(
            IntPtr hWnd,
            IntPtr hWndInsertAfter,
            int x,
            int y,
            int cx,
            int cy,
            uint uFlags);
    }
}
