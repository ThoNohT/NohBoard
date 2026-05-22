/*
Copyright (C) 2017 by Eric Bataille <e.c.p.bataille@gmail.com>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 2 of the License, or
(at your option) any later version.
*/

namespace ThoNohT.NohBoard.Forms
{
    using System;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using ThoNohT.NohBoard.Extra;

    /// <summary>
    /// Shared placement rules for modal dialogs opened from the app.
    /// </summary>
    internal static class FormPlacement
    {
        public const int DefaultWindowOffsetX = 100;
        public const int DefaultWindowOffsetY = 100;

        /// <summary>Horizontal gap between keyboard right edge and dialog (pixels).</summary>
        public const int BesideMainKeyboardGapPixels = 12;

        /// <summary>
        /// Top-left of the main window on the primary screen (offset 100, 100), matching other B.M. tools.
        /// </summary>
        public static Point GetDefaultMainWindowLocation()
        {
            var screen = Screen.PrimaryScreen ?? Screen.AllScreens.FirstOrDefault();
            if (screen == null)
                return new Point(DefaultWindowOffsetX, DefaultWindowOffsetY);

            return new Point(
                screen.Bounds.Left + DefaultWindowOffsetX,
                screen.Bounds.Top + DefaultWindowOffsetY);
        }

        /// <summary>
        /// When the main form is hidden (tray), <see cref="Control.PointToScreen"/> is unreliable.
        /// Temporarily anchor the Form to the shared default location so dialog placement stays consistent.
        /// </summary>
        public static void PrepareMainWindowAnchorBeforeDialog(MainForm main)
        {
            if (main == null || main.IsDisposed)
                return;

            if (main.Visible)
                return;

            main.StartPosition = FormStartPosition.Manual;
            main.Location = ClampTopLeftToWorkingArea(
                GetDefaultMainWindowLocation(),
                main.Size);
        }

        public static Point ClampTopLeftToWorkingArea(Point topLeft, Size formSize)
        {
            var w = formSize.Width > 0 ? formSize.Width : 400;
            var h = formSize.Height > 0 ? formSize.Height : 300;

            Rectangle wa;
            try
            {
                wa = Screen.FromPoint(topLeft).WorkingArea;
            }
            catch (ArgumentException)
            {
                wa = Screen.PrimaryScreen?.WorkingArea ?? SystemInformation.VirtualScreen;
            }

            var x = topLeft.X;
            var y = topLeft.Y;

            if (x + w > wa.Right)
                x = wa.Right - w;
            if (y + h > wa.Bottom)
                y = wa.Bottom - h;
            if (x < wa.Left)
                x = wa.Left;
            if (y < wa.Top)
                y = wa.Top;

            return new Point(x, y);
        }

        /// <summary>
        /// Moves the main overlay to the default screen position (primary screen + fixed offset).
        /// </summary>
        public static void MoveMainFormToDefaultPosition(MainForm form)
        {
            if (form == null || form.IsDisposed)
                return;

            var location = GetDefaultMainWindowLocation();
            form.StartPosition = FormStartPosition.Manual;
            form.Location = location;
        }

        /// <summary>
        /// Brings the form to the foreground (tray restore).
        /// </summary>
        public static void FocusMainForm(Form form)
        {
            if (form == null || form.IsDisposed)
                return;

            form.Activate();
            form.BringToFront();

            if (!form.Visible)
                return;

            var hwnd = form.Handle;
            if (hwnd == IntPtr.Zero)
                return;

            BringWindowToTop(hwnd);
            SetForegroundWindow(hwnd);
        }

        /// <summary>
        /// Resolves scaled keyboard width in pixels (matches main overlay client width).
        /// </summary>
        public static int GetScaledKeyboardWidthPixels(MainForm main)
        {
            var keyboardWidth = GlobalSettings.CurrentDefinition?.Width ?? 0;
            var scale = Math.Max(25, Math.Min(300, GlobalSettings.Settings?.KeyboardScalePercent ?? 100)) / 100f;
            keyboardWidth = (int)Math.Round(keyboardWidth * scale);
            if (keyboardWidth <= 0 && main != null)
                keyboardWidth = Math.Max(main.ClientSize.Width, 200);

            return Math.Max(keyboardWidth, 1);
        }

        /// <summary>
        /// Places the dialog to the right of the keyboard (client left + scaled width + gap).
        /// Vertically aligns with the <strong>main window top</strong> (same screen Y as the caption), not the client
        /// area below the title bar, so it looks level with the main UI.
        /// </summary>
        public static void AlignBesideMainKeyboard(MainForm main, Form dialog, int gapPixels = BesideMainKeyboardGapPixels)
        {
            if (main == null || dialog == null)
                return;

            PrepareMainWindowAnchorBeforeDialog(main);

            var keyboardWidth = GetScaledKeyboardWidthPixels(main);
            var clientOrigin = main.PointToScreen(Point.Empty);
            var target = new Point(clientOrigin.X + keyboardWidth + gapPixels, main.Top);

            dialog.StartPosition = FormStartPosition.Manual;
            dialog.Location = target;

            if (!main.IsHandleCreated)
                return;

            try
            {
                var wa = Screen.FromControl(main).WorkingArea;
                var w = dialog.Width > 0 ? dialog.Width : 400;
                var h = dialog.Height > 0 ? dialog.Height : 300;

                if (target.X + w > wa.Right)
                    target.X = Math.Max(wa.Left + gapPixels, wa.Right - w - gapPixels);
                if (target.X < wa.Left)
                    target.X = wa.Left + gapPixels;

                if (target.Y + h > wa.Bottom)
                    target.Y = Math.Max(wa.Top + gapPixels, wa.Bottom - h - gapPixels);
                if (target.Y < wa.Top)
                    target.Y = wa.Top + gapPixels;

                dialog.Location = target;
            }
            catch (ArgumentException)
            {
                // Screen.FromControl failed; keep unclamped position.
            }
        }

        /// <summary>
        /// Same as <see cref="AlignDialogBesideMainKeyboard"/> (kept for existing call sites).
        /// </summary>
        public static void AlignTopLeftWithMainWindow(Form dialog)
        {
            AlignDialogBesideMainKeyboard(dialog);
        }

        /// <summary>
        /// Places settings to the right of the keyboard (same as <see cref="AlignBesideMainKeyboard"/>).
        /// </summary>
        public static void AlignSettingsBesideOverlay(MainForm main, Form dialog)
        {
            AlignBesideMainKeyboard(main, dialog);
        }

        /// <summary>
        /// Positions a modal dialog to the right of the main keyboard UI.
        /// </summary>
        public static void AlignModalDialog(MainForm main, Form dialog)
        {
            if (main == null || dialog == null)
                return;

            AlignBesideMainKeyboard(main, dialog);
        }

        /// <summary>
        /// Positions a dialog using the main form if it exists; otherwise centers on screen.
        /// </summary>
        public static void AlignDialogBesideMainKeyboard(Form dialog)
        {
            if (dialog == null)
                return;

            var main = Application.OpenForms.OfType<MainForm>().FirstOrDefault(f => !f.IsDisposed);
            if (main != null)
                AlignBesideMainKeyboard(main, dialog);
            else
                dialog.StartPosition = FormStartPosition.CenterScreen;
        }

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool BringWindowToTop(IntPtr hWnd);
    }
}
