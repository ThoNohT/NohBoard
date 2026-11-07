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
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Extra;
    using Hooking;
    using Hooking.Interop;

    /// <summary>
    /// Adds overlay behaviour to the main form: a per-pixel transparent window (toggled with F1) and a borderless
    /// window (toggled with F2). The transparent window is implemented with a layered window that is painted through
    /// <c>UpdateLayeredWindow</c>, so that the alpha channel of the keyboard images and of the configured background
    /// color is preserved. This makes it possible to use NohBoard as an overlay on top of other applications.
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

        #region DPI

        /// <summary>
        /// The factor with which the logical (96 DPI) layout coordinates have to be multiplied to get device pixels.
        /// In debug builds this can be overridden with the <c>NOHBOARD_DPI_SCALE</c> environment variable, which makes
        /// it possible to test the scaled rendering on a machine with a different scaling factor.
        /// </summary>
        private float DpiScale => DebugDpiScaleOverride ?? (this.DeviceDpi / 96f);

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
        /// The hotkeys that are currently held down, used to ignore the auto repeat of the keyboard hook.
        /// </summary>
        private readonly HashSet<int> overlayHotkeysDown = new HashSet<int>();

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
        /// Whether the form was top most before it was made borderless.
        /// </summary>
        private bool originalTopMost;

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

        #endregion Fields

        #region Initialization and cleanup

        /// <summary>
        /// Sets up the overlay features: the global hotkeys and the corresponding menu items.
        /// </summary>
        private void InitializeOverlayFeatures()
        {
            // All layout coordinates are managed by the application itself, so the automatic WinForms scaling is
            // switched off. The DPI dependent scaling is applied in <see cref="ApplyDpiScaling"/>.
            this.AutoScaleMode = AutoScaleMode.None;

            HookManager.KeyEvent += this.HookManager_KeyEvent;

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
            }

            HookManager.KeyEvent -= this.HookManager_KeyEvent;

            this.layeredModeActive = false;
            this.DisableLayeredWindow();
            this.ReleaseLayeredBuffer();
        }

        #endregion Initialization and cleanup

        #region Hotkeys and modes

        /// <summary>
        /// Handles the global key events from the keyboard hook.
        /// </summary>
        /// <param name="keyCode">The virtual key code of the key.</param>
        /// <param name="down">Whether the key was pressed (<c>true</c>) or released (<c>false</c>).</param>
        private void HookManager_KeyEvent(int keyCode, bool down)
        {
            if (keyCode != VkF1 && keyCode != VkF2) return;

            // While a property dialog is detecting key codes, it owns the keyboard.
            if (HookManager.KeyboardInsert != null) return;

            if (!down)
            {
                this.overlayHotkeysDown.Remove(keyCode);
                return;
            }

            // Ignore the auto repeat of a key that is already held down.
            if (!this.overlayHotkeysDown.Add(keyCode)) return;

            // Toggle from the message loop: the hook procedure must return as fast as possible.
            if (this.IsHandleCreated && !this.IsDisposed)
            {
                try
                {
                    this.BeginInvoke((Action)(() =>
                    {
                        if (keyCode == VkF1) this.ToggleTransparency();
                        else this.ToggleBorderless();
                    }));
                }
                catch (InvalidOperationException)
                {
                    // The form is closing, the hotkey can be ignored.
                }
            }
        }

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

            this.UpdateWindowBorder();

            if (this.IsTransparencyActive)
            {
                this.EnableLayeredWindow();
                this.RenderLayeredFrame();
            }
            else
            {
                this.layeredModeActive = false;
                this.DisableLayeredWindow();
                this.Refresh();
            }
        }

        #endregion Hotkeys and modes

        #region Window border

        /// <summary>
        /// Applies the window border style and top most state that follow from the current overlay modes. The client
        /// area is kept at exactly the same position on the screen.
        /// </summary>
        private void UpdateWindowBorder()
        {
            var wantBorderless = this.transparentMode || this.borderlessMode;
            if (wantBorderless == this.borderlessApplied) return;

            var clientOnScreen = this.RectangleToScreen(this.ClientRectangle);

            if (wantBorderless)
            {
                this.originalBorderStyle = this.FormBorderStyle;
                this.originalTopMost = this.TopMost;

                this.FormBorderStyle = FormBorderStyle.None;
                this.TopMost = true;
            }
            else
            {
                this.FormBorderStyle = this.originalBorderStyle;
                this.TopMost = this.originalTopMost;
            }

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

        #region Layered window

        /// <summary>
        /// Makes the window layered, so that it can be painted through <c>UpdateLayeredWindow</c>.
        /// </summary>
        private void EnableLayeredWindow()
        {
            if (!this.IsHandleCreated) return;

            var exStyle = GetWindowLong(this.Handle, GwlExStyle);
            if ((exStyle & WsExLayered) == 0)
            {
                SetWindowLong(this.Handle, GwlExStyle, exStyle | WsExLayered);
            }

            this.layeredModeActive = true;
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
        }

        /// <summary>
        /// Renders the current keyboard scene onto the layered window. The background is left transparent, unless the
        /// current style defines a background color with an alpha smaller than 255.
        /// </summary>
        private void RenderLayeredFrame()
        {
            if (!this.IsHandleCreated || !this.layeredModeActive) return;

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

                // Paint the configured background color only when it is (partially) translucent. A fully transparent
                // background is not painted at all, and pressing F1 forces the background to be fully transparent.
                var backgroundAlpha = this.GetLayeredBackgroundAlpha();
                if (backgroundAlpha > 0)
                {
                    var backgroundColor = GlobalSettings.CurrentStyle.BackgroundColor;
                    using (var backgroundBrush = new SolidBrush(Color.FromArgb(
                        backgroundAlpha, backgroundColor.Red, backgroundColor.Green, backgroundColor.Blue)))
                    {
                        g.FillRectangle(backgroundBrush, 0, 0, width, height);
                    }
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
