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

    /// <summary>
    /// A native frame window that supplies the title bar and the border around the main window when the main window is
    /// transparent but should keep its native border. The main window itself always stays borderless and is painted
    /// through <c>UpdateLayeredWindow</c> with per-pixel alpha, exactly like the borderless transparent mode, so the
    /// rendering, the mouse interaction and the transparency quality are identical in both modes. This window only
    /// adds the native non-client area around it.
    ///
    /// The client area of this window is fully transparent: it is painted in the transparency key color, which the
    /// system turns into transparent, click-through pixels. Therefore the main window (which is placed exactly over
    /// the client area) and the desktop behind it stay visible, no matter whether this window is above or below the
    /// main window in the z-order. The window is a tool window, so it does not show up in the task bar or in Alt+Tab.
    /// </summary>
    internal sealed class OverlayFrameForm : Form
    {
        #region Constants

        /// <summary>
        /// The window message that is sent for system menu commands.
        /// </summary>
        private const int WmSysCommand = 0x0112;

        /// <summary>
        /// The system menu command that minimizes the window.
        /// </summary>
        private const int ScMinimize = 0xF020;

        /// <summary>
        /// The system menu command that closes the window.
        /// </summary>
        private const int ScClose = 0xF060;

        /// <summary>
        /// The extended window style that makes a window a tool window (no task bar entry, no Alt+Tab entry).
        /// </summary>
        private const int WsExToolWindow = 0x00000080;

        /// <summary>
        /// The <c>ShowWindow</c> command that shows a window without activating it.
        /// </summary>
        private const int SwShowNoActivate = 4;

        /// <summary>
        /// The flag for <c>SetWindowPos</c> that keeps the size of the window unchanged.
        /// </summary>
        private const uint SwpNoSize = 0x0001;

        /// <summary>
        /// The flag for <c>SetWindowPos</c> that keeps the position of the window unchanged.
        /// </summary>
        private const uint SwpNoMove = 0x0002;

        /// <summary>
        /// The flag for <c>SetWindowPos</c> that does not activate the window.
        /// </summary>
        private const uint SwpNoActivate = 0x0010;

        /// <summary>
        /// The color that makes the client area of this window transparent. The color is only used as a transparency
        /// key for the empty client area; the title bar and the border are drawn by the system and keep their normal
        /// appearance.
        /// </summary>
        private static readonly Color ClientAreaKeyColor = Color.FromArgb(255, 1, 0, 0);

        #endregion Constants

        #region Fields

        /// <summary>
        /// The main window that this frame belongs to.
        /// </summary>
        private readonly MainForm mainWindow;

        /// <summary>
        /// Indicates whether the forwarding of the close request to the main window is suppressed. This is the case
        /// while the frame is disposed together with the main window.
        /// </summary>
        private bool suppressCloseForwarding;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OverlayFrameForm" /> class.
        /// </summary>
        /// <param name="mainWindow">The main window that this frame belongs to.</param>
        internal OverlayFrameForm(MainForm mainWindow)
        {
            this.mainWindow = mainWindow;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.ShowInTaskbar = false;
            this.MinimizeBox = true;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.Manual;
            this.AutoScaleMode = AutoScaleMode.None;
            this.KeyPreview = true;

            // The client area is the "hole" through which the main window is visible. Painting it in the transparency
            // key color makes it transparent and click-through, while the title bar and the border are drawn by the
            // system.
            this.BackColor = ClientAreaKeyColor;
            this.TransparencyKey = ClientAreaKeyColor;

            this.SyncTitleAndIcon();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the window is shown without being activated. The frame is only a decoration
        /// of the main window and must not steal the focus when it is shown.
        /// </summary>
        protected override bool ShowWithoutActivation => true;

        /// <summary>
        /// Gets the screen rectangle of the client area of this window. The main window is positioned exactly on this
        /// rectangle.
        /// </summary>
        internal Rectangle ClientScreenRectangle => this.RectangleToScreen(this.ClientRectangle);

        #endregion Properties

        #region Window creation

        /// <summary>
        /// Adds the tool window extended style, so the frame window does not appear in the task bar or in Alt+Tab.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                var createParams = base.CreateParams;
                createParams.ExStyle |= WsExToolWindow;
                return createParams;
            }
        }

        #endregion Window creation

        #region Geometry and visibility

        /// <summary>
        /// Synchronizes the title and the icon of the frame with the main window.
        /// </summary>
        internal void SyncTitleAndIcon()
        {
            if (this.mainWindow == null) return;

            this.Text = this.mainWindow.Text;
            this.Icon = this.mainWindow.Icon;
        }

        /// <summary>
        /// Sizes the frame so that its client area has the given size, and moves it so that its client area is at the
        /// given screen position. The non-client area (title bar and border) is measured after the window handle has
        /// been created, so the client area is aligned pixel exactly, also on a high DPI monitor.
        /// </summary>
        /// <param name="clientSize">The required size of the client area.</param>
        /// <param name="clientScreenLocation">The required screen position of the client area.</param>
        internal void ApplyGeometry(Size clientSize, Point clientScreenLocation)
        {
            // The non-client metrics are only known once the window handle exists.
            var handle = this.Handle;

            this.ClientSize = clientSize;

            var clientOnScreen = this.ClientScreenRectangle;
            this.Location = new Point(
                this.Location.X + clientScreenLocation.X - clientOnScreen.Left,
                this.Location.Y + clientScreenLocation.Y - clientOnScreen.Top);
        }

        /// <summary>
        /// Shows the frame window without activating it.
        /// </summary>
        internal void ShowWithoutActivating()
        {
            if (!this.Visible)
            {
                this.Show();
                return;
            }

            ShowWindow(this.Handle, SwShowNoActivate);
        }

        /// <summary>
        /// Places the frame directly behind the main window in the z-order, so the main window is in front of it and
        /// the title bar and the border of the frame are still visible around it.
        /// </summary>
        /// <param name="mainHandle">The handle of the main window.</param>
        internal void PlaceBehind(IntPtr mainHandle)
        {
            if (!this.IsHandleCreated) return;

            SetWindowPos(this.Handle, mainHandle, 0, 0, 0, 0, SwpNoMove | SwpNoSize | SwpNoActivate);
        }

        /// <summary>
        /// Hides the frame window.
        /// </summary>
        internal void HideFrame()
        {
            if (this.Visible)
            {
                this.Hide();
            }
        }

        /// <summary>
        /// Disposes the frame window without forwarding a close request to the main window.
        /// </summary>
        internal void DisposeFrame()
        {
            this.suppressCloseForwarding = true;
            this.Hide();
            this.Dispose();
        }

        #endregion Geometry and visibility

        #region Window messages and events

        /// <summary>
        /// Handles the system menu commands of the frame: the minimize button minimizes the main window (the frame is
        /// hidden together with it), the close button closes the main window.
        /// </summary>
        /// <param name="m">The window message.</param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WmSysCommand)
            {
                var command = m.WParam.ToInt32() & 0xFFF0;
                if (command == ScMinimize)
                {
                    this.mainWindow.WindowState = FormWindowState.Minimized;
                    return;
                }

                if (command == ScClose)
                {
                    this.RequestMainWindowClose();
                    return;
                }
            }

            base.WndProc(ref m);
        }

        /// <summary>
        /// Makes sure that closing the frame closes the main window instead.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!this.suppressCloseForwarding && e.CloseReason != CloseReason.ApplicationExitCall)
            {
                e.Cancel = true;
                this.RequestMainWindowClose();
                return;
            }

            base.OnFormClosing(e);
        }

        /// <summary>
        /// Forwards the overlay hotkeys (F1/F2) to the main window, so they also work while the frame has the focus.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (this.mainWindow != null && this.mainWindow.HandleOverlayHotkey(e))
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }

            base.OnKeyDown(e);
        }

        /// <summary>
        /// Informs the main window that the focus state of the window group changed.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            this.mainWindow?.NotifyOverlayFrameFocusChanged();
        }

        /// <summary>
        /// Informs the main window that the focus state of the window group changed.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);

            this.mainWindow?.NotifyOverlayFrameFocusChanged();
        }

        /// <summary>
        /// Keeps the main window at the position of the client area of the frame while the user drags the frame by its
        /// title bar.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnMove(EventArgs e)
        {
            base.OnMove(e);

            this.mainWindow?.NotifyOverlayFrameGeometryChanged();
        }

        /// <summary>
        /// Keeps the main window aligned when the frame size changes (for example on a DPI change).
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            this.mainWindow?.NotifyOverlayFrameGeometryChanged();
        }

        /// <summary>
        /// Re-aligns the frame when it is moved to a monitor with a different scaling factor, because the non-client
        /// area metrics change with the DPI.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            base.OnDpiChanged(e);

            this.mainWindow?.NotifyOverlayFrameDpiChanged();
        }

        /// <summary>
        /// Requests the main window to close. The close is posted to the message loop instead of being executed
        /// directly, because closing the main window disposes this frame window.
        /// </summary>
        private void RequestMainWindowClose()
        {
            if (this.mainWindow == null) return;

            this.BeginInvoke(new MethodInvoker(() => this.mainWindow.Close()));
        }

        #endregion Window messages and events

        #region Interop

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(
            IntPtr hWnd,
            IntPtr hWndInsertAfter,
            int x,
            int y,
            int cx,
            int cy,
            uint flags);

        #endregion Interop
    }
}
