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

namespace ThoNohT.NohBoard.Hooking.Interop
{
    /// <summary>
    /// Contains the defines for different keys.
    /// </summary>
    public static class Defines
    {
        #region Hooks

        //values from Winuser.h in Microsoft SDK.
        /// <summary>
        /// Windows NT/2000/XP: Installs a hook procedure that monitors low-level mouse input events.
        /// </summary>
        public const int WH_MOUSE_LL = 14;

        /// <summary>
        /// Windows NT/2000/XP: Installs a hook procedure that monitors low-level keyboard  input events.
        /// </summary>
        public const int WH_KEYBOARD_LL = 13;

        #endregion Hooks

        #region Mouse messages

        /// <summary>
        /// The WM_MOUSEMOVE message is posted to a window when the cursor moves.
        /// </summary>
        public const int WM_MOUSEMOVE = 0x200;

        /// <summary>
        /// The WM_LBUTTONDOWN message is posted when the user presses the left mouse button
        /// </summary>
        public const int WM_LBUTTONDOWN = 0x201;

        /// <summary>
        /// The WM_RBUTTONDOWN message is posted when the user presses the right mouse button
        /// </summary>
        public const int WM_RBUTTONDOWN = 0x204;

        /// <summary>
        /// The WM_MBUTTONDOWN message is posted when the user presses the middle mouse button
        /// </summary>
        public const int WM_MBUTTONDOWN = 0x207;

        /// <summary>
        /// The WM_LBUTTONUP message is posted when the user releases the left mouse button
        /// </summary>
        public const int WM_LBUTTONUP = 0x202;

        /// <summary>
        /// The WM_RBUTTONUP message is posted when the user releases the right mouse button
        /// </summary>
        public const int WM_RBUTTONUP = 0x205;

        /// <summary>
        /// The WM_MBUTTONUP message is posted when the user releases the middle mouse button
        /// </summary>
        public const int WM_MBUTTONUP = 0x208;

        /// <summary>
        /// The WM_MOUSEWHEEL message is posted when the user scrolls the mouse wheel vertically.
        /// </summary>
        public const int WM_MOUSEWHEEL = 0x020A;

        /// <summary>
        /// THE WM_MOUSEHWHEEL message is posted when the user scrolls the mouse wheel horizontally.
        /// </summary>
        public const int WM_MOUSEHWHEEL = 0x020E;

        /// <summary>
        /// The WM_XBUTTONDOWN message is posted when the user presses any X-button.
        /// </summary>
        public const int WM_XBUTTONDOWN = 0x020B;

        /// <summary>
        /// The WM_XBUTTONDOWN message is posted when the user releases any X-button.
        /// </summary>
        public const int WM_XBUTTONUP = 0x020C;

        #endregion Mouse messages

        #region Keyboard messages

        /// <summary>
        /// The WM_KEYDOWN message is posted to the window with the keyboard focus when a nonsystem
        /// key is pressed. A nonsystem key is a key that is pressed when the ALT key is not pressed.
        /// </summary>
        public const int WM_KEYDOWN = 0x100;

        /// <summary>
        /// The WM_KEYUP message is posted to the window with the keyboard focus when a nonsystem
        /// key is released. A nonsystem key is a key that is pressed when the ALT key is not pressed,
        /// or a keyboard key that is pressed when a window has the keyboard focus.
        /// </summary>
        public const int WM_KEYUP = 0x101;

        /// <summary>
        /// The WM_SYSKEYDOWN message is posted to the window with the keyboard focus when the user
        /// presses the F10 key (which activates the menu bar) or holds down the ALT key and then
        /// presses another key. It also occurs when no window currently has the keyboard focus;
        /// in this case, the WM_SYSKEYDOWN message is sent to the active window. The window that
        /// receives the message can distinguish between these two contexts by checking the context
        /// code in the lParam parameter.
        /// </summary>
        public const int WM_SYSKEYDOWN = 0x104;

        /// <summary>
        /// The WM_SYSKEYUP message is posted to the window with the keyboard focus when the user
        /// releases a key that was pressed while the ALT key was held down. It also occurs when no
        /// window currently has the keyboard focus; in this case, the WM_SYSKEYUP message is sent
        /// to the active window. The window that receives the message can distinguish between
        /// these two contexts by checking the context code in the lParam parameter.
        /// </summary>
        public const int WM_SYSKEYUP = 0x105;

        #endregion Keyboard messages

        #region Key codes

        #region Mouse

        /// <summary>
        /// The left mouse button.
        /// </summary>
        public const byte VK_LBUTTON = 0x1;

        /// <summary>
        /// The right mouse button.
        /// </summary>
        public const byte VK_RBUTTON = 0x2;

        /// <summary>
        /// The middle mouse button.
        /// </summary>
        public const byte VK_MBUTTON = 0x04;

        /// <summary>
        /// The first X-button.
        /// </summary>
        public const byte VK_XBUTTON1 = 0x05;

        /// <summary>
        /// The second X-button.
        /// </summary>
        public const byte VK_XBUTTON2 = 0x06;

        /// <summary>
        /// The first X-button in an X-button message.
        /// </summary>
        public const byte XBUTTON1 = 0x1;

        /// <summary>
        /// The second X-button in an X-button message.
        /// </summary>
        public const byte XBUTTON2 = 0x2;

        #endregion Mouse

        #region Keyboard

        /// <summary>
        /// The shift key.
        /// </summary>
        public const byte VK_SHIFT = 0x10;

        /// <summary>
        /// The caps lock key.
        /// </summary>
        public const byte VK_CAPITAL = 0x14;

        /// <summary>
        /// The num-lock key.
        /// </summary>
        public const byte VK_NUMLOCK = 0x90;

        /// <summary>
        /// The scroll-lock key.
        /// </summary>
        public const byte VK_SCROLL = 0x91;

        /// <summary>
        /// The enter key.
        /// </summary>
        public const byte VK_RETURN = 0XD;

        /// <summary>
        /// The left shift key.
        /// </summary>
        public const byte VK_LSHIFT = 0XA0;

        /// <summary>
        /// The right shift key.
        /// </summary>
        public const byte VK_RSHIFT = 0XA1;

        /// <summary>
        /// The left control key.
        /// </summary>
        public const byte VK_LCTRL = 0XA2;

        /// <summary>
        /// The right control key.
        /// </summary>
        public const byte VK_RCTRL = 0XA3;

        /// <summary>
        /// The left alt key.
        /// </summary>
        public const byte VK_LALT = 0XA4;

        /// <summary>
        /// The right alt key.
        /// </summary>
        public const byte VK_RALT = 0XA5;

        #endregion Keyboard

        #endregion Key codes

        #region Flags

        /// <summary>
        /// Test the extended-key flag.
        /// </summary>
        public const int LLKHF_EXTENDED = KF_EXTENDED >> 8;

        /// <summary>
        /// The extended-key flag.
        /// </summary>
        public const int KF_EXTENDED = 0x0100;

        #endregion Flags
    }
}