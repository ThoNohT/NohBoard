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
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Linq;
    using System.Windows.Forms;
    using Extra;
    using Hooking;
    using Hooking.Interop;
    using Keyboard.ElementDefinitions;

    public partial class MainForm
    {
        private LayeredOverlayForm _layeredOverlay;

        private Bitmap _layeredBitmap;

        private bool _layeredOverlayMouseWired;

        private bool _keyboardSurfaceDetachedFromMainForm;

        private bool _layeredOverlayModalMode;

        private Form _activeAppModalDialog;

        private readonly HashSet<Form> _wiredModalDialogs = new HashSet<Form>();

        private int _suspendLayeredKeyboardUpdates;

        private void InitializeLayeredOverlay()
        {
            this._keyboardSurface.IsLayeredPresentationActive = () => this.UsesLayeredOverlay();
            this.LocationChanged += (_, _) =>
            {
                this.SyncLayeredOverlayBounds();
                if (this.UsesLayeredOverlay())
                    this.InvalidateKeyboardSurface();
            };
            this.SizeChanged += (_, _) => this.SyncLayeredOverlayBounds();
            this.VisibleChanged += (_, _) => this.OnMainFormVisibleChanged();
            this.Shown += (_, _) =>
            {
                if (this.UsesLayeredOverlay())
                    this.ApplyOverlayTransparencyStyle();
            };

            this.MainMenu.Opening += (_, _) => this.IncrementSuspendLayeredKeyboardUpdates();
            this.MainMenu.Closed += (_, _) => this.DecrementSuspendLayeredKeyboardUpdates();
        }

        internal void IncrementSuspendLayeredKeyboardUpdates() => this._suspendLayeredKeyboardUpdates++;

        internal void DecrementSuspendLayeredKeyboardUpdates()
        {
            if (this._suspendLayeredKeyboardUpdates > 0)
                this._suspendLayeredKeyboardUpdates--;
        }

        private bool ShouldDeferLayeredKeyboardRepaint()
        {
            return this._suspendLayeredKeyboardUpdates > 0
                || (this._resetBackBrushesTimer?.Enabled ?? false);
        }

        private bool ShouldDeferTimerDrivenRepaint()
        {
            return HookManager.IsModalUiActive || this._suspendLayeredKeyboardUpdates > 0;
        }

        internal bool IsLayeredOverlayActive => this.UsesLayeredOverlay();

        private bool UsesLayeredOverlay()
        {
            if (GlobalSettings.Settings == null || GlobalSettings.CurrentDefinition == null)
                return false;

            return GlobalSettings.Settings.OverlayTransparencyPercent > 0;
        }

        internal void ApplyOverlayTransparencyStyle()
        {
            if (!this.IsHandleCreated || GlobalSettings.Settings == null)
                return;

            var preserveLockedShell = this.ShouldPreserveLockedCaptionShell();

            if (!this.UsesLayeredOverlay() && this._captionHiddenForOverlayLock)
                this.RestoreCaptionChromeAfterOverlayLock();

            if (!preserveLockedShell)
                this.ClearMainFormLayeredStyles();

            if (this._keyboardSurface != null && this._keyboardSurface.IsHandleCreated)
            {
                var panelHwnd = this._keyboardSurface.Handle;
                var panelEx = GetWindowLong(panelHwnd, GwlExstyle);
                SetWindowLong(panelHwnd, GwlExstyle, panelEx & ~(WsExLayered | WsExTransparent));
            }

            if (this.UsesLayeredOverlay())
            {
                if (preserveLockedShell)
                    this.DetachKeyboardSurfaceFromMainForm();
                else
                    this.ApplyKeyboardWindowLayout();

                this.RefreshLayeredOverlayPresentation();

                if (this._layeredOverlayModalMode
                    && this._activeAppModalDialog != null
                    && !this._activeAppModalDialog.IsDisposed
                    && this._activeAppModalDialog.IsHandleCreated)
                {
                    this.PlaceDialogAboveLayeredOverlay(this._activeAppModalDialog);
                    this.ScheduleEnsureModalDialogFocus(this._activeAppModalDialog);
                }
            }
            else
            {
                this._layeredOverlay?.Hide();
                this.ApplyKeyboardWindowLayout();
                this.InvalidateKeyboardSurface();
            }

            if (this._overlayLocked)
                this.ApplyOverlayLockStyles();

            this.ApplyTaskSwitcherIconicPreviewPreference();
        }

        private void RefreshLayeredOverlayPresentation()
        {
            if (!this.UsesLayeredOverlay())
                return;

            this.EnsureLayeredOverlayForm();
            this._layeredOverlay.Enabled = true;
            this._layeredOverlay.Show();
            this.SyncLayeredOverlayBounds();
            this.PresentLayeredOverlay();
            this.BringLayeredOverlayToFront();
            this.WireLayeredOverlayContextMenu();
            this.ApplyLayeredOverlayInteractionStyles();
        }

        private void ApplyKeyboardWindowLayout()
        {
            var definition = GlobalSettings.CurrentDefinition;
            if (definition == null)
                return;

            this.SuspendLayout();
            try
            {
                var scaledSize = this.GetScaledKeyboardSize(definition);
                if (this.UsesLayeredOverlay())
                {
                    this.DetachKeyboardSurfaceFromMainForm();
                    if (this._overlayLocked)
                        this.RefreshLockedOverlayCaptionHide(scaledSize.Width);
                    else
                        this.ShrinkMainFormToCaptionOnly(scaledSize.Width);
                }
                else
                {
                    this.AttachKeyboardSurfaceToMainForm();
                    this.ClientSize = scaledSize;
                }
            }
            finally
            {
                this.ResumeLayout(true);
            }
        }

        private void DetachKeyboardSurfaceFromMainForm()
        {
            this._keyboardSurface.Visible = false;
            this._keyboardSurface.Dock = DockStyle.None;

            if (!this._keyboardSurfaceDetachedFromMainForm && this.Controls.Contains(this._keyboardSurface))
            {
                this.Controls.Remove(this._keyboardSurface);
                this._keyboardSurfaceDetachedFromMainForm = true;
            }
        }

        private void AttachKeyboardSurfaceToMainForm()
        {
            if (this._keyboardSurfaceDetachedFromMainForm)
            {
                this.Controls.Add(this._keyboardSurface);
                this._keyboardSurfaceDetachedFromMainForm = false;
            }

            this._keyboardSurface.Dock = DockStyle.Fill;
            this._keyboardSurface.Visible = true;
        }

        private float GetKeyboardScaleFactor()
        {
            var percent = GlobalSettings.Settings?.KeyboardScalePercent ?? 100;
            percent = Math.Max(25, Math.Min(300, percent));
            return percent / 100f;
        }

        private Size GetScaledKeyboardSize(Keyboard.KeyboardDefinition definition)
        {
            if (definition == null)
                return Size.Empty;

            var scale = this.GetKeyboardScaleFactor();
            return new Size(
                Math.Max(1, (int)Math.Round(definition.Width * scale)),
                Math.Max(1, (int)Math.Round(definition.Height * scale)));
        }

        private Point ScalePointToKeyboardPoint(Point point)
        {
            var scale = this.GetKeyboardScaleFactor();
            if (Math.Abs(scale - 1f) < 0.001f)
                return point;

            return new Point(
                (int)Math.Round(point.X / scale),
                (int)Math.Round(point.Y / scale));
        }

        private void ShrinkMainFormToCaptionOnly(int keyboardWidth)
        {
            var chrome = this.Size - this.ClientSize;
            this.MinimumSize = new Size(0, 0);
            this.MaximumSize = Size.Empty;
            this.ClientSize = new Size(Math.Max(keyboardWidth, 120), 0);
            this.Size = new Size(Math.Max(keyboardWidth, 120) + chrome.Width, chrome.Height);
        }

        internal void BeginLayeredOverlayModalMode()
        {
            if (!this.UsesLayeredOverlay())
                return;

            this._layeredOverlayModalMode = true;

            if (this.ShouldPreserveLockedCaptionShell())
                this.RefreshLayeredOverlayPresentation();
            else
            {
                this.ApplyKeyboardWindowLayout();
                this.RefreshLayeredOverlayPresentation();
            }
        }

        internal DialogResult ShowAppModalDialog(Form dialog)
        {
            HookManager.EnterModalUiScope();

            var beganLayeredModal = false;
            try
            {
                FormPlacement.AlignModalDialog(this, dialog);

                this.WireModalDialogBehavior(dialog);

                if (this.IsLayeredOverlayActive)
                {
                    this.BeginLayeredOverlayModalMode();
                    beganLayeredModal = true;
                }

                this._activeAppModalDialog = dialog;
                try
                {
                    return dialog.ShowDialog(this);
                }
                finally
                {
                    this.FlushResetBackBrushes(restoreModalFocus: false);
                    this._activeAppModalDialog = null;
                }
            }
            finally
            {
                HookManager.ExitModalUiScope();

                if (beganLayeredModal)
                    this.EndLayeredOverlayModalMode();
            }
        }

        internal DialogResult ShowAppMessageBox(
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon = MessageBoxIcon.None)
        {
            HookManager.EnterModalUiScope();

            var beganLayeredModal = false;
            try
            {
                if (this.IsLayeredOverlayActive)
                {
                    this.BeginLayeredOverlayModalMode();
                    beganLayeredModal = true;
                }

                return MessageBox.Show(this, text, caption, buttons, icon);
            }
            finally
            {
                HookManager.ExitModalUiScope();

                if (beganLayeredModal)
                    this.EndLayeredOverlayModalMode();
            }
        }

        internal void PrepareDialogAboveLayeredOverlay(Form dialog)
        {
            if (dialog == null || dialog.IsDisposed)
                return;

            this.WireModalDialogBehavior(dialog);
        }

        private void WireModalDialogBehavior(Form dialog)
        {
            if (dialog == null || dialog.IsDisposed || !this._wiredModalDialogs.Add(dialog))
                return;

            dialog.FormClosed += (_, _) => this._wiredModalDialogs.Remove(dialog);
            dialog.Shown += (_, _) =>
            {
                dialog.BeginInvoke(
                    new Action(
                        () =>
                        {
                            if (dialog.IsDisposed)
                                return;

                            if (this.IsLayeredOverlayActive)
                                this.PlaceDialogAboveLayeredOverlay(dialog);

                            this.EnsureModalDialogFocus(dialog);
                        }));
            };
        }

        private void ScheduleEnsureModalDialogFocus(Form dialog)
        {
            if (dialog == null || dialog.IsDisposed)
                return;

            if (!this.IsHandleCreated)
            {
                this.EnsureModalDialogFocus(dialog);
                return;
            }

            this.BeginInvoke(new Action(() => this.EnsureModalDialogFocus(dialog)));
        }

        private void EnsureModalDialogFocus(Form dialog)
        {
            if (dialog == null || dialog.IsDisposed || !dialog.Visible)
                return;

            if (this.DialogHasTextInputFocus(dialog))
                return;

            FormPlacement.FocusMainForm(dialog);

            if (dialog.ContainsFocus)
                return;

            if (dialog.CancelButton is Control cancel && cancel.CanSelect)
                dialog.ActiveControl = cancel;
            else
                dialog.SelectNextControl(null, true, true, true, false);
        }

        private bool DialogHasTextInputFocus(Form dialog)
        {
            var focused = dialog.ActiveControl;
            if (focused == null)
                return false;

            while (focused is ContainerControl container && container.ActiveControl != null)
                focused = container.ActiveControl;

            return focused is TextBoxBase;
        }

        internal void PlaceDialogAboveLayeredOverlay(Form dialog)
        {
            if (dialog == null || dialog.IsDisposed)
                return;

            if (this._layeredOverlay == null
                || !this._layeredOverlay.IsHandleCreated
                || !dialog.IsHandleCreated)
            {
                return;
            }

            SetWindowPos(
                dialog.Handle,
                this._layeredOverlay.Handle,
                0,
                0,
                0,
                0,
                SwpNomove | SwpNosize | SwpNoactivate);
        }

        internal void EndLayeredOverlayModalMode()
        {
            if (!this._layeredOverlayModalMode)
                return;

            this._layeredOverlayModalMode = false;

            if (!this.Visible)
                return;

            if (this.UsesLayeredOverlay())
            {
                if (this.ShouldPreserveLockedCaptionShell())
                {
                    this.RefreshLayeredOverlayPresentation();
                    this.ApplyOverlayLockStyles();
                }
                else
                {
                    this.ApplyOverlayTransparencyStyle();
                }
            }

            this.RestoreOverlayInteractionAfterModal();

            if (this.UsesLayeredOverlay() && !this._overlayLocked)
                this.ApplyLayeredOverlayPointerStyles(false);
        }

        internal void RestoreOverlayInteractionAfterModal()
        {
            this.WireLayeredOverlayContextMenu();
            this.ApplyLayeredOverlayInteractionStyles();

            if (this.UsesLayeredOverlay() && this._layeredOverlay != null)
            {
                this._layeredOverlay.Enabled = true;
                this.PresentLayeredOverlay();
            }
        }

        private void WireLayeredOverlayContextMenu()
        {
            if (this._layeredOverlay == null || this._layeredOverlay.IsDisposed)
                return;

            this._layeredOverlay.ContextMenuStrip = this.MainMenu;
        }

        private Point GetContextMenuClientPoint()
        {
            if (this.IsLayeredOverlayActive
                && this._layeredOverlay != null
                && !this._layeredOverlay.IsDisposed
                && this._layeredOverlay.Visible)
            {
                return this.ScalePointToKeyboardPoint(this._layeredOverlay.PointToClient(Cursor.Position));
            }

            return this.ScalePointToKeyboardPoint(this.PointToClient(Cursor.Position));
        }

        private void ApplyLayeredOverlayInteractionStyles()
        {
            if (!this.UsesLayeredOverlay())
                return;

            this.ApplyLayeredOverlayPointerStyles(this._overlayLocked);
        }

        internal void InvalidateKeyboardSurface(bool immediate = false)
        {
            if (!immediate && this.ShouldDeferTimerDrivenRepaint())
                return;

            if (this.UsesLayeredOverlay())
            {
                this.EnsureLayeredOverlayForm();
                if (!this._layeredOverlay.Visible)
                {
                    this._keyboardSurface.Visible = false;
                    this._layeredOverlay.Show();
                    this.SyncLayeredOverlayBounds();
                }

                this.PresentLayeredOverlay();
            }
            else
                this._keyboardSurface?.Invalidate();
        }

        internal void ApplyLayeredOverlayPointerStyles(bool clickThrough)
        {
            if (this._layeredOverlay == null || !this._layeredOverlay.IsHandleCreated)
                return;

            var hwnd = this._layeredOverlay.Handle;
            if (hwnd == IntPtr.Zero)
                return;

            var ex = GetWindowLong(hwnd, GwlExstyle);
            if (clickThrough)
                SetWindowLong(hwnd, GwlExstyle, ex | WsExLayered | WsExTransparent);
            else
                SetWindowLong(hwnd, GwlExstyle, (ex | WsExLayered) & ~WsExTransparent);
        }

        internal void DisposeLayeredOverlay()
        {
            this._layeredBitmap?.Dispose();
            this._layeredBitmap = null;

            if (this._layeredOverlay != null)
            {
                this._layeredOverlay.Close();
                this._layeredOverlay.Dispose();
                this._layeredOverlay = null;
                this._layeredOverlayMouseWired = false;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (this.UsesLayeredOverlay())
                return;

            base.OnPaintBackground(e);
        }

        private void ClearMainFormLayeredStyles()
        {
            var formHwnd = this.Handle;
            if (formHwnd == IntPtr.Zero)
                return;

            var formEx = GetWindowLong(formHwnd, GwlExstyle);
            SetWindowLong(formHwnd, GwlExstyle, formEx & ~(WsExLayered | WsExTransparent));
        }

        private void EnsureLayeredOverlayForm()
        {
            if (this._layeredOverlay != null && !this._layeredOverlay.IsDisposed)
                return;

            this._layeredOverlay = new LayeredOverlayForm();
            this._layeredOverlay.HandleCreated += (_, _) =>
            {
                if (!this.ShouldDeferLayeredKeyboardRepaint())
                    this.PresentLayeredOverlay();
            };

            this._layeredOverlay.Owner = this;

            if (!this._layeredOverlayMouseWired)
            {
                this._layeredOverlay.MouseDown += this.LayeredOverlay_MouseDown;
                this._layeredOverlay.MouseMove += this.LayeredOverlay_MouseMove;
                this._layeredOverlay.MouseUp += this.LayeredOverlay_MouseUp;
                this._layeredOverlay.ContextMenuStrip = this.MainMenu;
                this._layeredOverlayMouseWired = true;
            }
        }

        private void LayeredOverlay_MouseDown(object sender, MouseEventArgs e)
        {
            this.MainForm_MouseDown(sender, e);
        }

        private void LayeredOverlay_MouseMove(object sender, MouseEventArgs e)
        {
            this.MainForm_MouseMove(sender, e);
        }

        private void LayeredOverlay_MouseUp(object sender, MouseEventArgs e)
        {
            this.MainForm_MouseUp(sender, e);

            if (e.Button != MouseButtons.Right || this.menuOpen)
                return;

            if (this._layeredOverlay == null || this._layeredOverlay.IsDisposed)
                return;

            this.MainMenu.Show(this._layeredOverlay, e.Location);
        }

        private void SyncLayeredOverlayBounds()
        {
            if (this._layeredOverlay == null)
                return;

            if (!this.Visible || GlobalSettings.CurrentDefinition == null)
                return;

            var definition = GlobalSettings.CurrentDefinition;
            var origin = this.PointToScreen(Point.Empty);
            var target = new Rectangle(origin, this.GetScaledKeyboardSize(definition));
            if (this._layeredOverlay.Bounds != target)
                this._layeredOverlay.Bounds = target;

            this._layeredOverlay.TopMost =
                (this.TopMost || this._overlayLocked) && !this._layeredOverlayModalMode;
        }

        private void EnsureOverlayLayeredStyle()
        {
            if (this._layeredOverlay == null || !this._layeredOverlay.IsHandleCreated)
                return;

            var hwnd = this._layeredOverlay.Handle;
            var ex = GetWindowLong(hwnd, GwlExstyle);
            if ((ex & WsExLayered) == 0)
                SetWindowLong(hwnd, GwlExstyle, ex | WsExLayered);
        }

        private void OnMainFormVisibleChanged()
        {
            if (!this.Visible)
            {
                this._layeredOverlay?.Hide();
                return;
            }

            if (this.UsesLayeredOverlay())
                this.ApplyOverlayTransparencyStyle();
        }

        private bool PresentLayeredOverlay()
        {
            if (this.ShouldDeferLayeredKeyboardRepaint())
                return false;

            if (!this.UsesLayeredOverlay())
                return false;

            if (this._layeredOverlay == null || !this._layeredOverlay.IsHandleCreated)
                return false;

            if (GlobalSettings.CurrentDefinition == null || GlobalSettings.CurrentStyle == null)
                return false;

            var definition = GlobalSettings.CurrentDefinition;
            this.SyncLayeredOverlayBounds();

            var scaledSize = this.GetScaledKeyboardSize(definition);
            var w = scaledSize.Width;
            var h = scaledSize.Height;

            if (this._layeredBitmap == null
                || this._layeredBitmap.Width != w
                || this._layeredBitmap.Height != h
                || this._layeredBitmap.PixelFormat != PixelFormat.Format32bppArgb)
            {
                this._layeredBitmap?.Dispose();
                this._layeredBitmap = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            }

            using (var g = Graphics.FromImage(this._layeredBitmap))
            {
                g.Clear(Color.Transparent);
                var scale = this.GetKeyboardScaleFactor();
                if (Math.Abs(scale - 1f) >= 0.001f)
                    g.ScaleTransform(scale, scale);
                this.DrawKeyboardContent(g);
            }

            LayeredWindowHelper.PremultiplyAlpha(this._layeredBitmap);

            this.EnsureOverlayLayeredStyle();
            this.SyncLayeredOverlayBounds();

            var screenOrigin = this._layeredOverlay.PointToScreen(Point.Empty);
            var presented = LayeredWindowHelper.TryUpdateLayeredWindow(
                this._layeredOverlay.Handle,
                this._layeredBitmap,
                screenOrigin);

            if (presented
                && this._layeredOverlayModalMode
                && this._activeAppModalDialog != null
                && !this._activeAppModalDialog.IsDisposed)
            {
                this.ScheduleEnsureModalDialogFocus(this._activeAppModalDialog);
            }

            return presented;
        }

        private void BringLayeredOverlayToFront()
        {
            if (this._layeredOverlay == null || !this._layeredOverlay.IsHandleCreated)
                return;

            SetWindowPos(
                this._layeredOverlay.Handle,
                this.TopMost || this._overlayLocked ? HwndTopmost : HwndNotopmost,
                0,
                0,
                0,
                0,
                SwpNomove | SwpNosize | SwpNoactivate | SwpShowwindow);
        }

        private void KeyboardSurface_Paint(object sender, PaintEventArgs e)
        {
            if (this.UsesLayeredOverlay())
                return;

            var surface = this._keyboardSurface;
            if (surface == null)
                return;

            if (GlobalSettings.CurrentDefinition == null || GlobalSettings.CurrentStyle == null)
            {
                e.Graphics.Clear(Color.Black);
                return;
            }

            var transparency = GlobalSettings.Settings?.OverlayTransparencyPercent ?? 0;
            using (var clearBrush = new SolidBrush(
                OverlayTransparency.Apply(GlobalSettings.CurrentStyle.BackgroundColor, transparency)))
            {
                e.Graphics.FillRectangle(clearBrush, surface.ClientRectangle);
            }

            if (!this.backBrushes.Any())
                return;

            var scale = this.GetKeyboardScaleFactor();
            if (Math.Abs(scale - 1f) >= 0.001f)
                e.Graphics.ScaleTransform(scale, scale);
            this.DrawKeyboardContent(e.Graphics);
        }

        private void DrawKeyboardContent(Graphics g)
        {
            var definition = GlobalSettings.CurrentDefinition;
            var style = GlobalSettings.CurrentStyle;
            var settings = GlobalSettings.Settings;
            if (definition == null || style == null || settings == null)
                return;

            var transparency = settings.OverlayTransparencyPercent;
            var bounds = new Rectangle(0, 0, definition.Width, definition.Height);
            var hideFrameAndFill = OverlayTransparency.HidesFrameAndFill(transparency);

            if (!hideFrameAndFill)
            {
                using (var bgBrush = new SolidBrush(
                    OverlayTransparency.Apply(style.BackgroundColor, transparency)))
                {
                    g.FillRectangle(bgBrush, bounds);
                }

                if (this.backBrushes.TryGetValue(KeyboardState.ShiftDown, out var capsBrushes)
                    && capsBrushes.TryGetValue(KeyboardState.CapsActive, out var backBrush))
                {
                    g.FillRectangle(backBrush, bounds);
                }
            }

            KeyboardState.CheckKeyHolds(settings.PressHold);
            var kbKeys = KeyboardState.PressedKeys;
            var mouseKeys = MouseState.PressedKeys.Select(k => (int)k).ToList();
            MouseState.CheckKeyHolds(settings.PressHold);
            MouseState.CheckScrollAndMovement();
            var scrollCounts = MouseState.ScrollCounts;
            var allDefs = definition.Elements;

            foreach (var def in allDefs)
                this.Render(g, def, allDefs, kbKeys, mouseKeys, scrollCounts, false);

            if (this.currentlyManipulating == null)
            {
                if (this.highlightedDefinition != this.selectedDefinition)
                    this.highlightedDefinition?.RenderHighlight(g);

                if (this.selectedDefinition != null)
                {
                    this.Render(g, this.selectedDefinition, allDefs, kbKeys, mouseKeys, scrollCounts, true);
                    this.selectedDefinition.RenderSelected(g);
                }
            }
            else
            {
                this.currentlyManipulating.Value.definition.RenderEditing(g);
            }
        }
    }
}
