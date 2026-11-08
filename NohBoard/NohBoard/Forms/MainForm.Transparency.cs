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
    using System.Drawing.Imaging;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Security.Principal;
    using System.Windows.Forms;
    using Extra;
    using Hooking;
    using Hooking.Interop;

    /// <summary>
    /// Adds overlay behaviour to the main form: a transparent background (toggled with F1) and a borderless window
    /// (toggled with F2). These are independent modes, which gives four combinations:
    /// <list type="bullet">
    /// <item>opaque with the native border (the normal window),</item>
    /// <item>transparent with the native border,</item>
    /// <item>opaque without a border,</item>
    /// <item>transparent without a border.</item>
    /// </list>
    /// Both transparent variants render the main window itself in exactly the same way: the window is borderless and
    /// painted through <c>UpdateLayeredWindow</c> with per-pixel alpha, so the alpha channel of the keyboard images and
    /// of the configured background color is preserved, and the mouse interaction is identical in both variants. When
    /// the window should keep its native border, a separate frame window (<see cref="OverlayFrameForm" />) is placed
    /// around the main window to supply the native title bar and border. This makes it possible to use NohBoard as an
    /// overlay on top of other applications with or without a native frame.
    /// </summary>
    public partial class MainForm
    {
        #region Constants

        /// <summary>
        /// The virtual key code of the F1 key.
        /// </summary>
        private const int VkF1 = 0x70;

        /// <summary>
        /// The virtual key code of the F2 key.
        /// </summary>
        private const int VkF2 = 0x71;

        /// <summary>
        /// The index of the extended window style.
        /// </summary>
        private const int GwlExStyle = -20;

        /// <summary>
        /// The extended window style that makes a window layered.
        /// </summary>
        private const int WsExLayered = 0x00080000;

        /// <summary>
        /// The extended window style that makes mouse input pass through the window.
        /// </summary>
        private const int WsExTransparent = 0x00000020;

        /// <summary>
        /// The size of the corner area that can be dragged to resize the window.
        /// </summary>
        private const int ResizeGripSize = 18;

        /// <summary>
        /// The flag for <c>UpdateLayeredWindow</c> that uses the alpha channel of the source bitmap.
        /// </summary>
        private const int UlwAlpha = 0x02;

        /// <summary>
        /// The blend operation used for a normal source-over blend.
        /// </summary>
        private const byte AcSrcOver = 0x00;

        /// <summary>
        /// Indicates that the source bitmap contains premultiplied alpha.
        /// </summary>
        private const byte AcSrcAlpha = 0x01;

        /// <summary>
        /// The DIB color usage flag.
        /// </summary>
        private const uint DibRgbColors = 0;

        /// <summary>
        /// The uncompressed bitmap compression flag.
        /// </summary>
        private const uint BiRgb = 0;

        #endregion Constants

        /// <summary>
        /// Gets the color with which the client area background is filled in the normal (non layered) painting mode.
        /// </summary>
        private Color ClientBackgroundColor => GlobalSettings.CurrentStyle.BackgroundColor;

        #region DPI

        /// <summary>
        /// The factor with which the logical (96 DPI) layout coordinates have to be multiplied to get device pixels.
        /// This includes the DPI scale of the current monitor and the user scale of the overlay. In debug builds this
        /// can be overridden with the <c>NOHBOARD_DPI_SCALE</c> environment variable, which makes it possible to test
        /// the scaled rendering on a machine with a different scaling factor.
        /// </summary>
        private float DpiScale => DebugDpiScaleOverride ?? ((this.DeviceDpi / 96f) * this.overlayScale);

#if DEBUG
        /// <summary>
        /// An optional DPI scale override for debug builds, read from the environment.
        /// </summary>
        private static readonly float? DebugDpiScaleOverride = ParseDebugDpiScale();

        /// <summary>
        /// Parses the <c>NOHBOARD_DPI_SCALE</c> environment variable.
        /// </summary>
        /// <returns>The parsed scale, or <c>null</c> when it is not set or invalid.</returns>
        private static float? ParseDebugDpiScale()
        {
            var value = Environment.GetEnvironmentVariable("NOHBOARD_DPI_SCALE");
            return float.TryParse(
                value,
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture,
                out var scale) && scale > 0
                ? scale
                : (float?)null;
        }
#else
        /// <summary>
        /// No DPI scale override exists in release builds.
        /// </summary>
        private static readonly float? DebugDpiScaleOverride = null;
#endif

        /// <summary>
        /// Converts a logical length into device pixels.
        /// </summary>
        /// <param name="value">The logical length.</param>
        /// <returns>The length in device pixels.</returns>
        private int ScaleToDevice(int value)
        {
            return (int)Math.Round(value * this.DpiScale);
        }

        /// <summary>
        /// Converts a point in device pixels into the logical coordinates of the keyboard definition.
        /// </summary>
        /// <param name="point">The point in device pixels, relative to the client area.</param>
        /// <returns>The point in logical coordinates.</returns>
        private Point ToLogical(Point point)
        {
            return new Point(
                (int)Math.Round(point.X / this.DpiScale),
                (int)Math.Round(point.Y / this.DpiScale));
        }

        /// <summary>
        /// Sizes the client area so that the keyboard definition is displayed at the system scaling factor.
        /// </summary>
        private void SetClientSizeToDefinition()
        {
            var definition = GlobalSettings.CurrentDefinition;
            if (definition == null) return;

            this.ClientSize = new Size(this.ScaleToDevice(definition.Width), this.ScaleToDevice(definition.Height));
        }

        /// <summary>
        /// Re-applies all sizes that depend on the DPI of the monitor the window is on. This is called when the window
        /// is moved to a monitor with a different scaling factor.
        /// </summary>
        private void ApplyDpiScaling()
        {
            if (!this.IsHandleCreated) return;

            this.SetClientSizeToDefinition();
            this.ResetBackBrushes();

            if (this.layeredModeActive)
            {
                this.RenderLayeredFrame();
            }
            else
            {
                this.Refresh();
            }

            // The non-client area of the frame window changes with the DPI, so its geometry has to be recalculated.
            this.SyncOverlayFrameToMain();
        }

        /// <summary>
        /// Re-applies the DPI dependent sizes when the window is moved to a monitor with a different scaling factor.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            base.OnDpiChanged(e);

            this.ApplyDpiScaling();
        }

        #endregion DPI

        #region Fields

        /// <summary>
        /// The menu item that toggles the transparency.
        /// </summary>
        private ToolStripMenuItem mnuToggleTransparency;

        /// <summary>
        /// The menu item that toggles the borderless window.
        /// </summary>
        private ToolStripMenuItem mnuToggleBorderless;

        /// <summary>
        /// Whether the user requested a transparent window (F1).
        /// </summary>
        private bool transparentMode;

        /// <summary>
        /// Whether the user requested a borderless window (F2).
        /// </summary>
        private bool borderlessMode;

        /// <summary>
        /// Whether the borderless window mode is currently applied to the form.
        /// </summary>
        private bool borderlessApplied;

        /// <summary>
        /// Whether the window is currently painted as a layered window.
        /// </summary>
        private bool layeredModeActive;

        /// <summary>
        /// The border style of the form before it was made borderless.
        /// </summary>
        private FormBorderStyle originalBorderStyle = FormBorderStyle.FixedSingle;

        /// <summary>
        /// The off screen bitmap that is used to compose the layered window.
        /// </summary>
        private Bitmap layeredBitmap;

        /// <summary>
        /// The size of the current layered bitmap.
        /// </summary>
        private Size layeredBitmapSize;

        /// <summary>
        /// Whether the window is currently being dragged.
        /// </summary>
        private bool movingWindow;

        /// <summary>
        /// The cursor position at the start of dragging the window.
        /// </summary>
        private Point windowDragCursorStart;

        /// <summary>
        /// The window location at the start of dragging the window.
        /// </summary>
        private Point windowDragLocationStart;

        /// <summary>
        /// The user scale of the overlay, changed by dragging a corner of the borderless window.
        /// </summary>
        private float overlayScale = 1f;

        /// <summary>
        /// Whether the window is currently being resized by dragging a corner.
        /// </summary>
        private bool resizingWindow;

        /// <summary>
        /// The corner that is currently being dragged to resize the window: 0 = bottom right, 1 = bottom left,
        /// 2 = top right, 3 = top left.
        /// </summary>
        private int resizeCorner;

        /// <summary>
        /// The cursor position at the start of resizing the window.
        /// </summary>
        private Point resizeStartCursor;

        /// <summary>
        /// The client size at the start of resizing the window.
        /// </summary>
        private Size resizeStartClient;

        /// <summary>
        /// The window location at the start of resizing the window.
        /// </summary>
        private Point resizeStartLocation;

        /// <summary>
        /// Whether the resize cursor is currently shown.
        /// </summary>
        private bool resizeCursorActive;

        /// <summary>
        /// The native frame window that supplies the title bar and the border when the window is transparent but should
        /// keep its native border. The main window itself is borderless in that mode.
        /// </summary>
        private OverlayFrameForm overlayFrame;

        /// <summary>
        /// Guards against re-entrancy while the geometry of the main window and of the frame window is synchronized:
        /// moving one window moves the other one, which would otherwise trigger an endless loop.
        /// </summary>
        private bool syncingOverlayGeometry;

        /// <summary>
        /// Indicates whether the overlay modes are currently being re-applied. During a refresh the frame window is
        /// synchronized once at the end instead of on every intermediate resize event.
        /// </summary>
        private bool refreshingOverlayMode;

        #endregion Fields

        #region Initialization and cleanup

        /// <summary>
        /// Sets up the overlay features: the corresponding menu items. The hotkeys themselves are handled by the
        /// window key events in <see cref="OnKeyDown(KeyEventArgs)"/>.
        /// </summary>
        private void InitializeOverlayFeatures()
        {
            // All layout coordinates are managed by the application itself, so the automatic WinForms scaling is
            // switched off. The DPI dependent scaling is applied in <see cref="ApplyDpiScaling"/>.
            this.AutoScaleMode = AutoScaleMode.None;

            // The English texts are used as localization keys: this runs in the constructor, before
            // Loc.Initialize() has been called with the configured language, so the translation is applied later by
            // Loc.Apply(this.MainMenu) in MainForm_Load. Translating here would translate against the system
            // language and make it impossible to switch back to English afterwards.
            this.mnuToggleTransparency = new ToolStripMenuItem("Toggle &transparency (F1)");
            this.mnuToggleTransparency.Click += (s, e) =>
            {
                this.menuOpen = false;
                this.ToggleTransparency();
            };

            this.mnuToggleBorderless = new ToolStripMenuItem("Toggle &borderless window (F2)");
            this.mnuToggleBorderless.Click += (s, e) =>
            {
                this.menuOpen = false;
                this.ToggleBorderless();
            };

            var insertIndex = this.MainMenu.Items.IndexOf(this.mnuExit);
            if (insertIndex < 0) insertIndex = this.MainMenu.Items.Count;

            this.MainMenu.Items.Insert(insertIndex, this.mnuToggleTransparency);
            this.MainMenu.Items.Insert(insertIndex + 1, this.mnuToggleBorderless);
            this.MainMenu.Items.Insert(insertIndex + 2, new ToolStripSeparator());

            // When NohBoard is not running elevated, it is not able to see input that is sent to elevated
            // applications. Offer to restart as administrator so the overlay keeps working in that case as well.
            if (!IsElevated)
            {
                var mnuRestartElevated = new ToolStripMenuItem("Restart as &administrator");
                mnuRestartElevated.Click += (s, e) =>
                {
                    this.menuOpen = false;
                    this.RestartElevated();
                };

                this.MainMenu.Items.Insert(insertIndex + 3, mnuRestartElevated);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current process is running elevated (as administrator).
        /// </summary>
        private static bool IsElevated
        {
            get
            {
                using (var identity = WindowsIdentity.GetCurrent())
                {
                    return new WindowsPrincipal(identity).IsInRole(WindowsBuiltInRole.Administrator);
                }
            }
        }

        /// <summary>
        /// Restarts NohBoard with administrator privileges, so that input that is sent to elevated applications can
        /// be seen as well. Does nothing when the process is already elevated.
        /// </summary>
        private void RestartElevated()
        {
            if (IsElevated) return;

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = Application.ExecutablePath,
                    UseShellExecute = true,
                    Verb = "runas",
                });
            }
            catch
            {
                // The user cancelled the elevation prompt; keep running as is.
                return;
            }

            this.Close();
        }

        /// <summary>
        /// Restores the overlay modes that were stored in the global settings, so that a transparent or borderless
        /// window stays that way after a restart.
        /// </summary>
        private void RestoreOverlayState()
        {
            if (GlobalSettings.Settings == null) return;

            this.transparentMode = GlobalSettings.Settings.TransparentMode;
            this.borderlessMode = GlobalSettings.Settings.BorderlessMode;
            this.overlayScale = GlobalSettings.Settings.OverlayScale;

            // Apply the persisted user scale before the window mode is applied.
            this.SetClientSizeToDefinition();
            this.ResetBackBrushes();
            this.UpdateOverlayMode();
        }

        /// <summary>
        /// Releases all resources used by the overlay features.
        /// </summary>
        private void CleanupOverlayFeatures()
        {
            if (GlobalSettings.Settings != null)
            {
                GlobalSettings.Settings.TransparentMode = this.transparentMode;
                GlobalSettings.Settings.BorderlessMode = this.borderlessMode;
                GlobalSettings.Settings.OverlayScale = this.overlayScale;
            }

            this.layeredModeActive = false;
            this.DisableLayeredWindow();
            this.ReleaseLayeredBuffer();
            this.DisposeOverlayFrame();
        }

        #endregion Initialization and cleanup

        #region Modes

        /// <summary>
        /// Whether the window should be painted with a transparent background. This is the case when the user pressed
        /// F1, or when the background color of the current style is not fully opaque.
        /// </summary>
        private bool IsTransparencyActive =>
            this.transparentMode
            || (GlobalSettings.CurrentStyle?.BackgroundColor?.IsTransparent ?? false);

        /// <summary>
        /// Toggles the transparent window mode.
        /// </summary>
        private void ToggleTransparency()
        {
            this.transparentMode = !this.transparentMode;
            this.UpdateOverlayMode();
        }

        /// <summary>
        /// Toggles the borderless window mode.
        /// </summary>
        private void ToggleBorderless()
        {
            this.borderlessMode = !this.borderlessMode;
            this.UpdateOverlayMode();
        }

        /// <summary>
        /// Applies the current overlay modes and updates the menu checkmarks.
        /// </summary>
        private void UpdateOverlayMode()
        {
            if (this.mnuToggleTransparency != null)
            {
                this.mnuToggleTransparency.Checked = this.transparentMode;
                this.mnuToggleBorderless.Checked = this.borderlessMode;
            }

            this.RefreshOverlayMode();
        }

        /// <summary>
        /// Applies the window border and the painting mode that follow from the current overlay modes.
        /// </summary>
        private void RefreshOverlayMode()
        {
            if (!this.IsHandleCreated) return;

            this.refreshingOverlayMode = true;
            try
            {
                this.UpdateWindowBorder();

                if (this.IsTransparencyActive)
                {
                    // Both transparent variants paint the main window itself through UpdateLayeredWindow with
                    // per-pixel alpha; the native border of the bordered variant is supplied by a separate frame
                    // window.
                    this.layeredModeActive = false;
                    this.DisableLayeredWindow();
                    this.EnableLayeredWindow();
                    this.RenderLayeredFrame();
                }
                else
                {
                    // Opaque window: plain GDI+ painting, with a native border unless the borderless mode is active.
                    this.layeredModeActive = false;
                    this.DisableLayeredWindow();
                    this.Refresh();
                }

                this.UpdateOverlayFrameVisibility();
                this.UpdateClickThrough();
            }
            finally
            {
                this.refreshingOverlayMode = false;
            }
        }

        #endregion Hotkeys and modes

        #region Window border

        /// <summary>
        /// Applies the window border style that follows from the current overlay modes. The client area is kept at
        /// exactly the same position on the screen. The window is never made top most: NohBoard stays an ordinary
        /// window, so other applications can be brought to the foreground on top of it.
        /// </summary>
        private void UpdateWindowBorder()
        {
            // A transparent window always has to be borderless: the per-pixel alpha surface replaces the whole window,
            // so there is no non-client area to draw. The native border of the bordered transparent variant is
            // supplied by the separate frame window instead. F2 still controls the border of an opaque window.
            var wantBorderless = this.IsTransparencyActive || this.borderlessMode;
            var desiredStyle = wantBorderless
                ? FormBorderStyle.None
                : (this.mnuToggleEditMode.Checked ? FormBorderStyle.Sizable : this.originalBorderStyle);

            if (this.FormBorderStyle == desiredStyle)
            {
                this.borderlessApplied = wantBorderless;
                return;
            }

            var clientOnScreen = this.RectangleToScreen(this.ClientRectangle);

            // Remember the normal bordered style. The resizable border of the edit mode is managed by the edit mode
            // itself, so it is not remembered as the original style.
            if (this.FormBorderStyle != FormBorderStyle.None
                && this.FormBorderStyle != FormBorderStyle.Sizable)
            {
                this.originalBorderStyle = this.FormBorderStyle;
            }

            this.FormBorderStyle = desiredStyle;
            this.borderlessApplied = wantBorderless;

            // Keep the client area at exactly the same place on the screen.
            this.ClientSize = clientOnScreen.Size;
            var newClientOnScreen = this.RectangleToScreen(this.ClientRectangle);
            this.Location = new Point(
                this.Location.X + clientOnScreen.X - newClientOnScreen.X,
                this.Location.Y + clientOnScreen.Y - newClientOnScreen.Y);
        }

        #endregion Window border

        #region Window dragging

        /// <summary>
        /// Starts dragging the window if it is borderless and not in edit mode.
        /// </summary>
        /// <param name="e">The mouse event arguments.</param>
        /// <returns><c>true</c> if the event was handled by dragging the window, <c>false</c> otherwise.</returns>
        private bool StartWindowDrag(MouseEventArgs e)
        {
            if (this.mnuToggleEditMode.Checked || e.Button != MouseButtons.Left || !this.borderlessApplied)
                return false;

            this.movingWindow = true;
            this.windowDragCursorStart = Cursor.Position;
            this.windowDragLocationStart = this.Location;
            this.Capture = true;

            return true;
        }

        /// <summary>
        /// Continues dragging the window, if it is currently being dragged.
        /// </summary>
        /// <returns><c>true</c> if the event was handled by dragging the window, <c>false</c> otherwise.</returns>
        private bool ContinueWindowDrag()
        {
            if (!this.movingWindow) return false;

            var delta = new Size(
                Cursor.Position.X - this.windowDragCursorStart.X,
                Cursor.Position.Y - this.windowDragCursorStart.Y);

            this.Location = new Point(
                this.windowDragLocationStart.X + delta.Width,
                this.windowDragLocationStart.Y + delta.Height);

            // The layered window has to be repainted at its new position.
            if (this.layeredModeActive) this.RenderLayeredFrame();

            return true;
        }

        /// <summary>
        /// Ends dragging the window, if it is currently being dragged.
        /// </summary>
        private void EndWindowDrag()
        {
            if (!this.movingWindow) return;

            this.movingWindow = false;
            this.Capture = false;
        }

        #endregion Window dragging

        #region Window resizing and click-through

        /// <summary>
        /// Starts resizing the window when the left mouse button is pressed in one of the corners of a borderless
        /// window. The aspect ratio of the keyboard layout is preserved while resizing.
        /// </summary>
        /// <param name="e">The mouse event arguments.</param>
        /// <returns><c>true</c> if the event was handled by resizing the window, <c>false</c> otherwise.</returns>
        private bool TryStartWindowResize(MouseEventArgs e)
        {
            if (this.mnuToggleEditMode.Checked || e.Button != MouseButtons.Left)
                return false;

            var corner = this.HitTestResizeCorner(e.Location);
            if (corner < 0) return false;

            this.resizingWindow = true;
            this.resizeCorner = corner;
            this.resizeStartCursor = Cursor.Position;
            this.resizeStartClient = this.ClientSize;
            this.resizeStartLocation = this.Location;
            this.Capture = true;
            this.SetResizeCursor(corner);

            return true;
        }

        /// <summary>
        /// Continues resizing the window, if it is currently being resized. Both dimensions are scaled by the same
        /// factor, keeping the aspect ratio of the keyboard layout intact.
        /// </summary>
        /// <returns><c>true</c> if the event was handled by resizing the window, <c>false</c> otherwise.</returns>
        private bool ContinueWindowResize()
        {
            if (!this.resizingWindow) return false;

            var definition = GlobalSettings.CurrentDefinition;
            if (definition == null) return true;

            var dpiFactor = this.DeviceDpi / 96f;
            var deltaX = Cursor.Position.X - this.resizeStartCursor.X;

            // Only the horizontal delta is used; the same scale is applied to both dimensions, so the aspect ratio of
            // the keyboard layout does not change.
            var fromLeft = this.resizeCorner == 1 || this.resizeCorner == 3;
            var newWidth = (float)(fromLeft
                ? this.resizeStartClient.Width - deltaX
                : this.resizeStartClient.Width + deltaX);

            var minimumWidth = definition.Width * dpiFactor * 0.25f;
            var maximumWidth = definition.Width * dpiFactor * 8f;
            newWidth = Math.Max(minimumWidth, Math.Min(maximumWidth, newWidth));

            this.overlayScale = newWidth / (definition.Width * dpiFactor);

            var newClientWidth = this.ScaleToDevice(definition.Width);
            var newClientHeight = this.ScaleToDevice(definition.Height);

            this.ClientSize = new Size(newClientWidth, newClientHeight);

            // Keep the opposite corner of the window at the same position while resizing.
            var location = this.resizeStartLocation;
            if (fromLeft) location.X -= newClientWidth - this.resizeStartClient.Width;
            if (this.resizeCorner == 2 || this.resizeCorner == 3)
                location.Y -= newClientHeight - this.resizeStartClient.Height;
            this.Location = location;

            this.ResetBackBrushes();
            if (this.layeredModeActive) this.RenderLayeredFrame();
            else this.Refresh();

            return true;
        }

        /// <summary>
        /// Ends resizing the window, if it is currently being resized, and persists the new scale.
        /// </summary>
        private void EndWindowResize()
        {
            if (!this.resizingWindow) return;

            this.resizingWindow = false;
            this.Capture = false;
            this.SetResizeCursor(-1);

            if (GlobalSettings.Settings != null)
            {
                GlobalSettings.Settings.OverlayScale = this.overlayScale;
            }
        }

        /// <summary>
        /// Returns the corner that can be dragged to resize the window at the specified point, or -1.
        /// </summary>
        /// <param name="location">The point relative to the client area.</param>
        /// <returns>0 = bottom right, 1 = bottom left, 2 = top right, 3 = top left, -1 = no corner.</returns>
        private int HitTestResizeCorner(Point location)
        {
            if (this.mnuToggleEditMode.Checked) return -1;

            var width = this.ClientSize.Width;
            var height = this.ClientSize.Height;
            var left = location.X <= ResizeGripSize;
            var right = location.X >= width - ResizeGripSize;
            var top = location.Y <= ResizeGripSize;
            var bottom = location.Y >= height - ResizeGripSize;

            if (right && bottom) return 0;
            if (left && bottom) return 1;
            if (right && top) return 2;
            if (left && top) return 3;

            return -1;
        }

        /// <summary>
        /// Shows or resets the resize cursor for the given corner.
        /// </summary>
        /// <param name="corner">The corner, or -1 to reset the cursor.</param>
        private void SetResizeCursor(int corner)
        {
            if (corner < 0)
            {
                if (!this.resizeCursorActive) return;

                this.Cursor = Cursors.Default;
                this.resizeCursorActive = false;
                return;
            }

            this.Cursor = corner == 0 || corner == 3 ? Cursors.SizeNWSE : Cursors.SizeNESW;
            this.resizeCursorActive = true;
        }

        /// <summary>
        /// Updates the cursor when hovering over one of the resize corners.
        /// </summary>
        /// <param name="e">The mouse event arguments.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!this.resizingWindow && !this.movingWindow)
            {
                this.SetResizeCursor(this.HitTestResizeCorner(e.Location));
            }

            base.OnMouseMove(e);
        }

        /// <summary>
        /// Makes a focused transparent window receive mouse input, and an unfocused one click-through. When the whole
        /// window group (main window and, in the bordered transparent mode, its frame window) has no focus, the mouse
        /// events are passed to the window below it. The frame window counts as part of the group, so clicking the
        /// native title bar of the frame keeps the main window clickable.
        /// </summary>
        private void UpdateClickThrough()
        {
            if (!this.IsHandleCreated) return;

            var exStyle = GetWindowLong(this.Handle, GwlExStyle);
            var wantTransparent = this.layeredModeActive && !this.WindowGroupContainsFocus;
            var newStyle = wantTransparent
                ? exStyle | WsExTransparent
                : exStyle & ~WsExTransparent;

            if (newStyle != exStyle)
            {
                SetWindowLong(this.Handle, GwlExStyle, newStyle);
            }
        }

        /// <summary>
        /// Re-applies the click-through state when the window gains the focus.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            this.UpdateClickThrough();
        }

        /// <summary>
        /// Makes the window click-through when it loses the focus and is transparent.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);

            this.UpdateClickThrough();
        }

        #endregion Window resizing and click-through

        #region Overlay frame

        /// <summary>
        /// Gets a value indicating whether the window group (the main window and its native frame window) currently
        /// contains the keyboard focus. The frame window counts as part of the group, so clicking the native title bar
        /// keeps the main window clickable instead of making it click-through.
        /// </summary>
        private bool WindowGroupContainsFocus =>
            this.ContainsFocus || (this.overlayFrame != null && this.overlayFrame.ContainsFocus);

        /// <summary>
        /// Handles the overlay hotkeys (F1 and F2). This is called both from the main window and from the native frame
        /// window, so the hotkeys also work while the frame has the focus.
        /// </summary>
        /// <param name="e">The key event arguments.</param>
        /// <returns><c>true</c> if the key was handled, <c>false</c> otherwise.</returns>
        internal bool HandleOverlayHotkey(KeyEventArgs e)
        {
            if (e == null) return false;

            switch (e.KeyCode)
            {
                case Keys.F1:
                    this.ToggleTransparency();
                    return true;

                case Keys.F2:
                    this.ToggleBorderless();
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Re-evaluates the click-through state when the focus of the frame window changed.
        /// </summary>
        internal void NotifyOverlayFrameFocusChanged()
        {
            this.UpdateClickThrough();
        }

        /// <summary>
        /// Keeps the main window at the client area of the frame window while the user drags the frame by its native
        /// title bar.
        /// </summary>
        internal void NotifyOverlayFrameGeometryChanged()
        {
            if (this.overlayFrame == null || !this.overlayFrame.Visible) return;
            if (this.syncingOverlayGeometry) return;
            if (!this.IsTransparencyActive || this.borderlessMode) return;
            if (this.WindowState != FormWindowState.Normal) return;

            this.syncingOverlayGeometry = true;
            try
            {
                // In this mode the main window is borderless, so its bounds are exactly its client area on the screen.
                var clientOnScreen = this.overlayFrame.ClientScreenRectangle;
                if (this.PointToScreen(Point.Empty) != clientOnScreen.Location)
                {
                    this.Location = clientOnScreen.Location;
                }
            }
            finally
            {
                this.syncingOverlayGeometry = false;
            }
        }

        /// <summary>
        /// Re-aligns the frame window when it was moved to a monitor with a different scaling factor.
        /// </summary>
        internal void NotifyOverlayFrameDpiChanged()
        {
            this.SyncOverlayFrameToMain();
        }

        /// <summary>
        /// Shows or hides the native frame window, depending on the current overlay modes and the window state.
        /// </summary>
        private void UpdateOverlayFrameVisibility()
        {
            var wantFrame = this.IsTransparencyActive
                && !this.borderlessMode
                && this.IsHandleCreated
                && this.Visible
                && this.WindowState != FormWindowState.Minimized;

            if (wantFrame)
            {
                this.ShowOverlayFrame();
            }
            else
            {
                this.HideOverlayFrame();
            }
        }

        /// <summary>
        /// Shows the native frame window without activating it and aligns it with the main window.
        /// </summary>
        private void ShowOverlayFrame()
        {
            if (!this.IsHandleCreated || !this.Visible) return;
            if (this.WindowState == FormWindowState.Minimized) return;

            if (this.overlayFrame == null)
            {
                this.overlayFrame = new OverlayFrameForm(this);
            }

            this.overlayFrame.SyncTitleAndIcon();

            if (!this.overlayFrame.Visible)
            {
                this.overlayFrame.ApplyGeometry(this.ClientSize, this.PointToScreen(Point.Empty));
                this.overlayFrame.ShowWithoutActivating();
                this.overlayFrame.PlaceBehind(this.Handle);
            }

            this.SyncOverlayFrameToMain();
        }

        /// <summary>
        /// Hides the native frame window. When the frame had the focus, the focus is given back to the main window.
        /// </summary>
        private void HideOverlayFrame()
        {
            if (this.overlayFrame == null) return;
            if (!this.overlayFrame.Visible) return;

            var frameHadFocus = this.overlayFrame.ContainsFocus;
            this.overlayFrame.HideFrame();

            if (frameHadFocus && !this.ContainsFocus && this.Visible)
            {
                this.Activate();
            }
        }

        /// <summary>
        /// Disposes the native frame window.
        /// </summary>
        private void DisposeOverlayFrame()
        {
            var frame = this.overlayFrame;
            if (frame == null) return;

            this.overlayFrame = null;
            this.syncingOverlayGeometry = true;
            try
            {
                frame.DisposeFrame();
            }
            finally
            {
                this.syncingOverlayGeometry = false;
            }
        }

        /// <summary>
        /// Sizes and moves the native frame window so that its client area exactly matches the client area of the main
        /// window. The non-client area of the frame is measured every time, so the alignment also stays correct when
        /// the DPI of the monitor changes.
        /// </summary>
        private void SyncOverlayFrameToMain()
        {
            if (this.overlayFrame == null || !this.overlayFrame.Visible) return;
            if (this.syncingOverlayGeometry) return;
            if (!this.IsTransparencyActive || this.borderlessMode) return;
            if (this.WindowState != FormWindowState.Normal) return;

            this.syncingOverlayGeometry = true;
            try
            {
                this.overlayFrame.SyncTitleAndIcon();
                this.overlayFrame.ApplyGeometry(this.ClientSize, this.PointToScreen(Point.Empty));
            }
            finally
            {
                this.syncingOverlayGeometry = false;
            }
        }

        /// <summary>
        /// Keeps the native frame window aligned when the main window is moved.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnMove(EventArgs e)
        {
            base.OnMove(e);

            this.SyncOverlayFrameToMain();
        }

        /// <summary>
        /// Hides the frame window while the main window is minimized and shows it again when the main window is
        /// restored. Also keeps the frame aligned when the main window is resized.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (this.WindowState == FormWindowState.Minimized)
            {
                this.HideOverlayFrame();
                return;
            }

            // During a mode refresh the final visibility and geometry are applied at the end of the refresh.
            if (this.refreshingOverlayMode) return;

            this.UpdateOverlayFrameVisibility();
        }

        /// <summary>
        /// Shows the frame window once the main window is displayed.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            this.UpdateOverlayFrameVisibility();
        }

        /// <summary>
        /// Keeps the title of the frame window in sync with the title of the main window.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            this.overlayFrame?.SyncTitleAndIcon();
        }

        #endregion Overlay frame

        #region Layered window

        /// <summary>
        /// Makes the window layered, so that it can be painted through <c>UpdateLayeredWindow</c>.
        /// </summary>
        private void EnableLayeredWindow()
        {
            if (!this.IsHandleCreated) return;

            var exStyle = GetWindowLong(this.Handle, GwlExStyle);

            // Remove a possibly existing layered style first and set it again afterwards. This resets any layered
            // window attributes that might have been set for the window and makes sure UpdateLayeredWindow is accepted.
            if ((exStyle & WsExLayered) != 0)
            {
                SetWindowLong(this.Handle, GwlExStyle, exStyle & ~WsExLayered);
            }

            SetWindowLong(this.Handle, GwlExStyle, exStyle | WsExLayered);

            this.layeredModeActive = true;
            this.UpdateClickThrough();
        }

        /// <summary>
        /// Removes the layered window style, so that the window is painted by GDI+ again.
        /// </summary>
        private void DisableLayeredWindow()
        {
            if (!this.IsHandleCreated) return;

            var exStyle = GetWindowLong(this.Handle, GwlExStyle);
            if ((exStyle & WsExLayered) != 0)
            {
                SetWindowLong(this.Handle, GwlExStyle, exStyle & ~WsExLayered);
            }

            this.UpdateClickThrough();
        }

        /// <summary>
        /// Renders the current keyboard scene onto the layered window. The background is left transparent, unless the
        /// current style defines a background color with an alpha smaller than 255.
        /// </summary>
        private void RenderLayeredFrame()
        {
            if (!this.IsHandleCreated || !this.layeredModeActive) return;

            // A minimized window has no visible surface to paint; painting it would also place the layered bitmap at
            // invalid screen coordinates.
            if (this.WindowState == FormWindowState.Minimized) return;

            var definition = GlobalSettings.CurrentDefinition;
            if (definition == null || definition.Width <= 0 || definition.Height <= 0) return;

            var width = this.ScaleToDevice(definition.Width);
            var height = this.ScaleToDevice(definition.Height);
            if (width <= 0 || height <= 0) return;

            this.EnsureLayeredBuffer(width, height);
            if (this.layeredBitmap == null) return;

            using (var g = Graphics.FromImage(this.layeredBitmap))
            {
                g.Clear(Color.Transparent);

                // Fill the whole window with an almost invisible background layer. Pixels with an alpha of 0 are
                // click-through for layered windows, so a single alpha step keeps the entire window hit-testable
                // (clickable and draggable) while remaining visually transparent.
                var backgroundAlpha = this.GetLayeredBackgroundAlpha();
                var fillAlpha = backgroundAlpha == 0 ? (byte)1 : backgroundAlpha;
                var backgroundColor = GlobalSettings.CurrentStyle.BackgroundColor;
                using (var backgroundBrush = new SolidBrush(Color.FromArgb(
                    fillAlpha, backgroundColor.Red, backgroundColor.Green, backgroundColor.Blue)))
                {
                    g.FillRectangle(backgroundBrush, 0, 0, width, height);
                }

                // Draw the pre-rendered loose key artwork. The back brush bitmaps have a transparent background, so
                // all fully transparent parts of the layout stay transparent in the layered window as well.
                var shiftDown = KeyboardState.ShiftDown;
                var capsActive = KeyboardState.CapsActive;
                if (this.backBrushes.ContainsKey(shiftDown) && this.backBrushes[shiftDown].ContainsKey(capsActive))
                {
                    g.FillRectangle(this.backBrushes[shiftDown][capsActive], 0, 0, width, height);
                }

                // The element definitions are in logical (96 DPI) coordinates, scale them to the device pixels of the
                // monitor the window is currently on.
                g.ScaleTransform(this.DpiScale, this.DpiScale);
                this.RenderScene(g);
            }

            this.UpdateLayeredWindowFromBitmap();
        }

        /// <summary>
        /// Gets the alpha value with which the style background color should be painted onto the layered window.
        /// </summary>
        /// <returns>The alpha value, or 0 when the background should not be painted at all.</returns>
        private byte GetLayeredBackgroundAlpha()
        {
            var backgroundColor = GlobalSettings.CurrentStyle?.BackgroundColor;
            if (backgroundColor == null) return 0;

            // Pressing F1 forces the background to be fully transparent, even if the style defines an opaque color.
            if (this.transparentMode && backgroundColor.Alpha == byte.MaxValue) return 0;

            return backgroundColor.Alpha;
        }

        /// <summary>
        /// Renders the entire keyboard scene onto the specified surface, including the edit mode overlays. The surface
        /// is expected to be scaled to logical (96 DPI) coordinates already; the background back-brush is painted by
        /// the caller in device pixels.
        /// </summary>
        /// <param name="g">The surface to render on.</param>
        private void RenderScene(Graphics g)
        {
            var definition = GlobalSettings.CurrentDefinition;
            if (definition == null || this.backBrushes.Count == 0) return;

            // Render all keys.
            KeyboardState.CheckKeyHolds(GlobalSettings.Settings.PressHold);
            var kbKeys = KeyboardState.PressedKeys;
            var mouseKeys = MouseState.PressedKeys.Select(k => (int)k).ToList();
            MouseState.CheckKeyHolds(GlobalSettings.Settings.PressHold);
            MouseState.CheckScrollAndMovement();
            var scrollCounts = MouseState.ScrollCounts;
            var allDefs = definition.Elements;
            foreach (var def in allDefs)
            {
                this.Render(g, def, allDefs, kbKeys, mouseKeys, scrollCounts, false);
            }

            // Draw the element being manipulated.
            if (this.currentlyManipulating == null)
            {
                if (this.highlightedDefinition != this.selectedDefinition)
                    // Draw highlighted only if it is not also selected.
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

        /// <summary>
        /// Makes sure the off screen bitmap used for the layered window has the correct size.
        /// </summary>
        /// <param name="width">The required width.</param>
        /// <param name="height">The required height.</param>
        private void EnsureLayeredBuffer(int width, int height)
        {
            if (this.layeredBitmap != null
                && this.layeredBitmapSize.Width == width
                && this.layeredBitmapSize.Height == height)
            {
                return;
            }

            this.ReleaseLayeredBuffer();

            // Premultiplied alpha is required by UpdateLayeredWindow with AC_SRC_ALPHA.
            this.layeredBitmap = new Bitmap(width, height, PixelFormat.Format32bppPArgb);
            this.layeredBitmapSize = new Size(width, height);
        }

        /// <summary>
        /// Releases the off screen bitmap used for the layered window.
        /// </summary>
        private void ReleaseLayeredBuffer()
        {
            this.layeredBitmap?.Dispose();
            this.layeredBitmap = null;
            this.layeredBitmapSize = Size.Empty;
        }

        /// <summary>
        /// Copies the current layered bitmap to a device independent bitmap and hands it to <c>UpdateLayeredWindow</c>.
        /// </summary>
        private void UpdateLayeredWindowFromBitmap()
        {
            if (!this.IsHandleCreated || this.layeredBitmap == null) return;

            var width = this.layeredBitmap.Width;
            var height = this.layeredBitmap.Height;

            var screenDc = GetDC(IntPtr.Zero);
            if (screenDc == IntPtr.Zero) return;

            var memoryDc = IntPtr.Zero;
            var dib = IntPtr.Zero;
            var previousObject = IntPtr.Zero;

            try
            {
                memoryDc = CreateCompatibleDC(screenDc);

                var info = new BitmapInfo
                {
                    Header = new BitmapInfoHeader
                    {
                        Size = (uint)Marshal.SizeOf(typeof(BitmapInfoHeader)),
                        Width = width,
                        Height = -height, // Negative height means a top-down bitmap.
                        Planes = 1,
                        BitCount = 32,
                        Compression = BiRgb
                    },
                    Colors = new uint[1]
                };

                dib = CreateDIBSection(screenDc, ref info, DibRgbColors, out var dibBits, IntPtr.Zero, 0);
                if (dib == IntPtr.Zero) return;

                previousObject = SelectObject(memoryDc, dib);

                var data = this.layeredBitmap.LockBits(
                    new Rectangle(0, 0, width, height),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format32bppPArgb);
                try
                {
                    var rowBytes = width * 4;
                    for (var y = 0; y < height; y++)
                    {
                        CopyMemory(
                            IntPtr.Add(dibBits, y * rowBytes),
                            IntPtr.Add(data.Scan0, y * data.Stride),
                            (uint)rowBytes);
                    }
                }
                finally
                {
                    this.layeredBitmap.UnlockBits(data);
                }

                var destination = this.PointToScreen(Point.Empty);
                var size = new Size(width, height);
                var source = new Point(0, 0);
                var blend = new BlendFunction
                {
                    BlendOp = AcSrcOver,
                    BlendFlags = 0,
                    SourceConstantAlpha = 255,
                    AlphaFormat = AcSrcAlpha
                };

                var updated = UpdateLayeredWindow(
                    this.Handle, screenDc, ref destination, ref size, memoryDc, ref source, 0, ref blend, UlwAlpha);
            }
            finally
            {
                if (previousObject != IntPtr.Zero) SelectObject(memoryDc, previousObject);
                if (dib != IntPtr.Zero) DeleteObject(dib);
                if (memoryDc != IntPtr.Zero) DeleteDC(memoryDc);
                ReleaseDC(IntPtr.Zero, screenDc);
            }
        }

        #endregion Layered window

        #region Interop

        [StructLayout(LayoutKind.Sequential)]
        private struct BitmapInfoHeader
        {
            public uint Size;
            public int Width;
            public int Height;
            public ushort Planes;
            public ushort BitCount;
            public uint Compression;
            public uint SizeImage;
            public int XPelsPerMeter;
            public int YPelsPerMeter;
            public uint ClrUsed;
            public uint ClrImportant;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct BitmapInfo
        {
            public BitmapInfoHeader Header;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public uint[] Colors;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct BlendFunction
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDc);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UpdateLayeredWindow(
            IntPtr hWnd,
            IntPtr hdcDst,
            ref Point pptDst,
            ref Size psize,
            IntPtr hdcSrc,
            ref Point pprSrc,
            int crKey,
            ref BlendFunction pblend,
            int dwFlags);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern IntPtr CreateCompatibleDC(IntPtr hDc);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteDC(IntPtr hDc);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern IntPtr SelectObject(IntPtr hDc, IntPtr hObject);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern IntPtr CreateDIBSection(
            IntPtr hDc,
            ref BitmapInfo pbmi,
            uint usage,
            out IntPtr ppvBits,
            IntPtr hSection,
            uint offset);

        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = false)]
        private static extern void CopyMemory(IntPtr destination, IntPtr source, uint length);

        #endregion Interop
    }
}
