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

namespace ThoNohT.NohBoard.Hooking
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using ThoNohT.NohBoard.Hooking.Interop;

    /// <summary>
    /// Feeds the keyboard and mouse state from the raw input API. The low level hooks that are used by default are not
    /// always able to see all input: input that is sent to an elevated application is not passed to hooks of a non
    /// elevated process, and some games only expose their input through raw input. The raw input API with the
    /// <c>RIDEV_INPUTSINK</c> flag receives the input regardless of the focus of the window, so the overlay keeps
    /// showing the correct keys while another application is in the foreground.
    /// </summary>
    /// <remarks>
    /// The raw input events are translated into the same <see cref="KeyboardState" /> and <see cref="MouseState" />
    /// calls as the low level hooks. Both sources are kept active at the same time: the calls are idempotent, so a
    /// key that is reported by both sources is still only pressed once.
    /// </remarks>
    public static class RawInputManager
    {
        #region Constants

        /// <summary>
        /// The message that is sent for raw input.
        /// </summary>
        private const int WmInput = 0x00FF;

        /// <summary>
        /// The command to get the raw input data.
        /// </summary>
        private const uint RidInput = 0x10000003;

        /// <summary>
        /// The raw input device flag that makes the window receive the input even when it is not in the foreground.
        /// </summary>
        private const uint RidevInputSink = 0x00000100;

        /// <summary>
        /// The raw input type for mouse input.
        /// </summary>
        private const uint RiTypeMouse = 0;

        /// <summary>
        /// The raw input type for keyboard input.
        /// </summary>
        private const uint RiTypeKeyboard = 1;

        /// <summary>
        /// The keyboard flag that indicates a key release.
        /// </summary>
        private const ushort RiKeyBreak = 0x01;

        /// <summary>
        /// The keyboard flag that indicates an E0 prefixed scan code.
        /// </summary>
        private const ushort RiKeyE0 = 0x02;

        /// <summary>
        /// The keyboard flag that indicates an E1 prefixed scan code.
        /// </summary>
        private const ushort RiKeyE1 = 0x04;

        /// <summary>
        /// The mouse button flags for the five mouse buttons.
        /// </summary>
        private const ushort RiMouseLeftButtonDown = 0x0001;
        private const ushort RiMouseLeftButtonUp = 0x0002;
        private const ushort RiMouseRightButtonDown = 0x0004;
        private const ushort RiMouseRightButtonUp = 0x0008;
        private const ushort RiMouseMiddleButtonDown = 0x0010;
        private const ushort RiMouseMiddleButtonUp = 0x0020;
        private const ushort RiMouseButton4Down = 0x0040;
        private const ushort RiMouseButton4Up = 0x0080;
        private const ushort RiMouseButton5Down = 0x0100;
        private const ushort RiMouseButton5Up = 0x0200;

        /// <summary>
        /// The mouse flags for vertical and horizontal wheel movement.
        /// </summary>
        private const ushort RiMouseWheel = 0x0400;
        private const ushort RiMouseHWheel = 0x0800;

        /// <summary>
        /// The virtual key code of the return key.
        /// </summary>
        private const ushort VkReturn = 0x0D;

        /// <summary>
        /// The virtual key code of the pause key.
        /// </summary>
        private const ushort VkPause = 0x13;

        /// <summary>
        /// The NohBoard key code for the enter key on the numeric keypad.
        /// </summary>
        private const int NumpadEnterKeyCode = 1025;

        /// <summary>
        /// The translation mode that translates a scan code to an extended virtual key code. This returns the left
        /// and right variants of the modifier keys.
        /// </summary>
        private const uint MapvkVscToVkEx = 3;

        #endregion Constants

        #region Fields

        /// <summary>
        /// The window that receives the raw input, or <see cref="IntPtr.Zero" /> when raw input is not active.
        /// </summary>
        private static IntPtr targetWindow = IntPtr.Zero;

        #endregion Fields

        #region Registration

        /// <summary>
        /// Gets a value indicating whether the raw input devices are registered.
        /// </summary>
        public static bool IsActive => targetWindow != IntPtr.Zero;

        /// <summary>
        /// Registers the keyboard and mouse raw input devices for the specified window.
        /// </summary>
        /// <param name="windowHandle">The window that should receive the raw input messages.</param>
        /// <returns><c>true</c> if the registration succeeded, <c>false</c> otherwise.</returns>
        public static bool Initialize(IntPtr windowHandle)
        {
            if (windowHandle == IntPtr.Zero) return false;

            var devices = new[]
            {
                new RAWINPUTDEVICE
                {
                    UsagePage = 0x01,
                    Usage = 0x06,
                    Flags = RidevInputSink,
                    Target = windowHandle,
                },
                new RAWINPUTDEVICE
                {
                    UsagePage = 0x01,
                    Usage = 0x02,
                    Flags = RidevInputSink,
                    Target = windowHandle,
                },
            };

            if (!RegisterRawInputDevices(
                devices,
                (uint)devices.Length,
                (uint)Marshal.SizeOf(typeof(RAWINPUTDEVICE))))
            {
                targetWindow = IntPtr.Zero;
                return false;
            }

            targetWindow = windowHandle;
            return true;
        }

        /// <summary>
        /// Unregisters the raw input devices.
        /// </summary>
        public static void Uninitialize()
        {
            if (targetWindow == IntPtr.Zero) return;

            var devices = new[]
            {
                new RAWINPUTDEVICE { UsagePage = 0x01, Usage = 0x06, Flags = 0, Target = IntPtr.Zero },
                new RAWINPUTDEVICE { UsagePage = 0x01, Usage = 0x02, Flags = 0, Target = IntPtr.Zero },
            };

            RegisterRawInputDevices(
                devices,
                (uint)devices.Length,
                (uint)Marshal.SizeOf(typeof(RAWINPUTDEVICE)));

            targetWindow = IntPtr.Zero;
        }

        #endregion Registration

        #region Message handling

        /// <summary>
        /// Processes a window message. When the message is a raw input message, the input is fed into the keyboard and
        /// mouse state and <c>true</c> is returned.
        /// </summary>
        /// <param name="message">The window message.</param>
        /// <param name="lParam">The lParam of the window message.</param>
        /// <returns><c>true</c> if the message was a raw input message, <c>false</c> otherwise.</returns>
        public static bool ProcessMessage(int message, IntPtr lParam)
        {
            if (message != WmInput) return false;

            try
            {
                var headerSize = (uint)Marshal.SizeOf(typeof(RAWINPUTHEADER));
                uint dataSize = 0;

                if (GetRawInputData(lParam, RidInput, IntPtr.Zero, ref dataSize, headerSize) != 0 || dataSize == 0)
                    return true;

                var buffer = Marshal.AllocHGlobal((int)dataSize);
                try
                {
                    if (GetRawInputData(lParam, RidInput, buffer, ref dataSize, headerSize) != dataSize)
                        return true;

                    var header = Marshal.PtrToStructure<RAWINPUTHEADER>(buffer);
                    var dataPointer = IntPtr.Add(buffer, Marshal.SizeOf(typeof(RAWINPUTHEADER)));

                    if (header.Type == RiTypeKeyboard)
                    {
                        ProcessKeyboard(Marshal.PtrToStructure<RAWKEYBOARD>(dataPointer));
                    }
                    else if (header.Type == RiTypeMouse)
                    {
                        ProcessMouse(Marshal.PtrToStructure<RAWMOUSE>(dataPointer));
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(buffer);
                }
            }
            catch
            {
                // Raw input should never be able to crash the overlay, ignore malformed messages.
            }

            return true;
        }

        /// <summary>
        /// Processes a raw keyboard event.
        /// </summary>
        /// <param name="keyboard">The raw keyboard data.</param>
        private static void ProcessKeyboard(RAWKEYBOARD keyboard)
        {
            var keyCode = TranslateKeyCode(keyboard);
            if (keyCode == 0) return;

            if ((keyboard.Flags & RiKeyBreak) != 0)
            {
                KeyboardState.RemovePressedElement(keyCode, HookManager.PressHold);
            }
            else
            {
                KeyboardState.AddPressedElement(keyCode, HookManager.PressHold);
            }
        }

        /// <summary>
        /// Translates the raw keyboard data to a NohBoard key code.
        /// </summary>
        /// <param name="keyboard">The raw keyboard data.</param>
        /// <returns>The NohBoard key code.</returns>
        private static int TranslateKeyCode(RAWKEYBOARD keyboard)
        {
            var extended = (keyboard.Flags & RiKeyE0) != 0;
            var enhancedE1 = (keyboard.Flags & RiKeyE1) != 0;

            // The enter key on the numeric keypad is reported as an extended return key.
            if (extended && keyboard.VKey == VkReturn) return NumpadEnterKeyCode;

            // The pause key is reported with an E1 prefix, and has no regular virtual key translation.
            if (enhancedE1 && keyboard.VKey == VkPause) return VkPause;

            // Translate the scan code to an extended virtual key code, so that the left and right variants of the
            // modifier keys are reported separately.
            var scanCode = (uint)(keyboard.MakeCode | (extended ? 0x100 : 0x000));
            var translated = MapVirtualKey(scanCode, MapvkVscToVkEx);
            if (translated != 0) return (int)translated;

            return keyboard.VKey;
        }

        /// <summary>
        /// Processes a raw mouse event.
        /// </summary>
        /// <param name="mouse">The raw mouse data.</param>
        private static void ProcessMouse(RAWMOUSE mouse)
        {
            var flags = mouse.ButtonFlags;

            if ((flags & RiMouseLeftButtonDown) != 0) AddMouseKey(MouseKeyCode.LeftButton);
            else if ((flags & RiMouseLeftButtonUp) != 0) RemoveMouseKey(MouseKeyCode.LeftButton);

            if ((flags & RiMouseRightButtonDown) != 0) AddMouseKey(MouseKeyCode.RightButton);
            else if ((flags & RiMouseRightButtonUp) != 0) RemoveMouseKey(MouseKeyCode.RightButton);

            if ((flags & RiMouseMiddleButtonDown) != 0) AddMouseKey(MouseKeyCode.MiddleButton);
            else if ((flags & RiMouseMiddleButtonUp) != 0) RemoveMouseKey(MouseKeyCode.MiddleButton);

            if ((flags & RiMouseButton4Down) != 0) AddMouseKey(MouseKeyCode.X1Button);
            else if ((flags & RiMouseButton4Up) != 0) RemoveMouseKey(MouseKeyCode.X1Button);

            if ((flags & RiMouseButton5Down) != 0) AddMouseKey(MouseKeyCode.X2Button);
            else if ((flags & RiMouseButton5Up) != 0) RemoveMouseKey(MouseKeyCode.X2Button);

            if ((flags & RiMouseWheel) != 0)
            {
                var delta = (short)mouse.ButtonData;
                MouseState.AddScrollDirection(delta > 0 ? MouseScrollKeyCode.ScrollUp : MouseScrollKeyCode.ScrollDown);
            }
            else if ((flags & RiMouseHWheel) != 0)
            {
                var delta = (short)mouse.ButtonData;
                MouseState.AddScrollDirection(delta > 0 ? MouseScrollKeyCode.ScrollRight : MouseScrollKeyCode.ScrollLeft);
            }

            var dx = mouse.LastX;
            var dy = mouse.LastY;
            if (dx == 0 && dy == 0) return;

            if (MouseState.MouseFromCenter)
            {
                // When the speed is measured from the center of the screen, the cursor position is needed to
                // determine the distance to that center.
                if (GetCursorPos(out var cursorPosition))
                {
                    MouseState.RegisterLocation(cursorPosition, Environment.TickCount);
                }
            }
            else
            {
                // Games that capture the mouse keep the cursor in place. The raw relative movement is the only
                // reliable source for the mouse speed in that case.
                MouseState.RegisterRawMovement(dx, dy, Environment.TickCount);
            }
        }

        /// <summary>
        /// Registers a pressed mouse key.
        /// </summary>
        /// <param name="key">The mouse key.</param>
        private static void AddMouseKey(MouseKeyCode key)
        {
            MouseState.AddPressedElement(key);
        }

        /// <summary>
        /// Registers a released mouse key.
        /// </summary>
        /// <param name="key">The mouse key.</param>
        private static void RemoveMouseKey(MouseKeyCode key)
        {
            MouseState.RemovePressedElement(key, HookManager.PressHold);
        }

        #endregion Message handling

        #region Interop

        /// <summary>
        /// Defines the raw input devices.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct RAWINPUTDEVICE
        {
            public ushort UsagePage;
            public ushort Usage;
            public uint Flags;
            public IntPtr Target;
        }

        /// <summary>
        /// Defines the header of the raw input data.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct RAWINPUTHEADER
        {
            public uint Type;
            public uint Size;
            public IntPtr Device;
            public IntPtr WParam;
        }

        /// <summary>
        /// Defines the raw keyboard data.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct RAWKEYBOARD
        {
            public ushort MakeCode;
            public ushort Flags;
            public ushort Reserved;
            public ushort VKey;
            public uint Message;
            public uint ExtraInformation;
        }

        /// <summary>
        /// Defines the raw mouse data.
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        private struct RAWMOUSE
        {
            [FieldOffset(0)]
            public ushort Flags;

            [FieldOffset(4)]
            public ushort ButtonFlags;

            [FieldOffset(6)]
            public ushort ButtonData;

            [FieldOffset(8)]
            public uint RawButtons;

            [FieldOffset(12)]
            public int LastX;

            [FieldOffset(16)]
            public int LastY;

            [FieldOffset(20)]
            public uint ExtraInformation;
        }

        /// <summary>
        /// Registers the raw input devices.
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RegisterRawInputDevices(
            RAWINPUTDEVICE[] devices,
            uint deviceCount,
            uint deviceSize);

        /// <summary>
        /// Gets the raw input data.
        /// </summary>
        [DllImport("user32.dll")]
        private static extern uint GetRawInputData(
            IntPtr rawInputHandle,
            uint command,
            IntPtr data,
            ref uint dataSize,
            uint headerSize);

        /// <summary>
        /// Translates a scan code to a virtual key code.
        /// </summary>
        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint code, uint mapType);

        /// <summary>
        /// Gets the current cursor position.
        /// </summary>
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out Point point);

        #endregion Interop
    }
}
