/*
Copyright (C) 2016 by Eric Bataille <e.c.p.bataille@gmail.com>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 2 of the License, or
(at your option) any later version.
*/

namespace ThoNohT.NohBoard.Forms
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Ask DWM to prefer an iconic (application icon) preview in the task switcher instead of a live window thumbnail.
    /// Helps layered / <c>UpdateLayeredWindow</c> surfaces where live capture is often blank.
    /// </summary>
    public partial class MainForm
    {
        private const int DwmwaForceIconicRepresentation = 7;

        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(
            IntPtr hwnd,
            int dwAttribute,
            ref int pvAttribute,
            int cbAttribute);

        /// <summary>
        /// Sets <see cref="DwmwaForceIconicRepresentation"/> on the main HWND so Alt+Tab / task overview use the exe-style icon bitmap when the compositor cannot thumbnail layered content reliably.
        /// </summary>
        private void ApplyTaskSwitcherIconicPreviewPreference()
        {
            if (!this.IsHandleCreated || this.Handle == IntPtr.Zero)
                return;

            try
            {
                var enabled = 1;
                DwmSetWindowAttribute(
                    this.Handle,
                    DwmwaForceIconicRepresentation,
                    ref enabled,
                    Marshal.SizeOf(typeof(int)));
            }
            catch
            {
                // DWM unavailable or attribute unsupported — ignore.
            }
        }
    }
}
