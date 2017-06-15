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
    using System.Collections.Generic;
    using System.Linq;
    using static Interop.Defines;
    using static Interop.FunctionImports;

    /// <summary>
    /// A class representing the current state of the keyboard. I.e. which buttons are pressed.
    /// </summary>
    public class KeyboardState
    {
        #region State

        /// <summary>
        /// A bag containing all currently pressed keys.
        /// </summary>
        private static readonly HashSet<int> pressedKeys = new HashSet<int>();

        /// <summary>
        /// A value indicating whether something has changed since the last check.
        /// </summary>
        private static bool updated;

        #endregion State

        /// <summary>
        /// A value indicating whether something has changed since the last check.
        /// Accessing this property will reset it to <c>false</c>.
        /// </summary>
        public static bool Updated
        {
            get
            {
                if (!updated) return false;

                updated = false;
                return true;
            }
        }

        /// <summary>
        /// Returns a list with all keys that are currently pressed.
        /// </summary>
        public static IReadOnlyList<int> PressedKeys
        {
            get { lock (pressedKeys) return pressedKeys.ToList().AsReadOnly(); }
        }

        /// <summary>
        /// Returns a value indicating whether any shift key is currently down.
        /// </summary>
        public static bool ShiftDown
        {
            get { lock (pressedKeys) return pressedKeys.Contains(VK_LSHIFT) || pressedKeys.Contains(VK_RSHIFT); }
        }

        /// <summary>
        /// Returns a value indicating whether any ctrl key is currently down.
        /// </summary>
        public static bool CtrlDown
        {
            get { lock (pressedKeys) return pressedKeys.Contains(VK_LCTRL) || pressedKeys.Contains(VK_RCTRL); }
        }

        /// <summary>
        /// Returns a value indicating whether any alt key is currently down.
        /// </summary>
        public static bool AltDown
        {
            get { lock (pressedKeys) return pressedKeys.Contains(VK_LALT) || pressedKeys.Contains(VK_RALT); }
        }

        /// <summary>
        /// Returns a value indicating whether caps lock is currently active.
        /// </summary>
        public static bool CapsActive => (GetKeyState(VK_CAPITAL) & 0x1) != 0;

        /// <summary>
        /// Checks the state of all keys and removes the ones that are no longer pressed from the list of pressed keys.
        /// </summary>
        public static void CheckKeys()
        {
            lock (pressedKeys)
            {
                foreach (var key in pressedKeys.Where(KeyIsUp).ToList())
                {
                    pressedKeys.Remove(key);
                    updated = true;
                }
            }
        }

        /// <summary>
        /// Adds the specified mouse keycode to the list of pressed keys.
        /// </summary>
        /// <param name="keyCode">The keycode to add.</param>
        public static void AddPressedElement(int keyCode)
        {
            lock (pressedKeys)
            {
                if (pressedKeys.Contains(keyCode)) return;

                pressedKeys.Add(keyCode);
                updated = true;
            }
        }

        /// <summary>
        /// Removes the specified keycode from the list of pressed keys.
        /// </summary>
        /// <param name="keyCode">The keycode to remove.</param>
        public static void RemovePressedElement(int keyCode)
        {
            lock (pressedKeys)
            {
                if (!pressedKeys.Contains(keyCode)) return;

                pressedKeys.Remove(keyCode);
                updated = true;
            }
        }

        /// <summary>
        /// Checks whether the specified key is up.
        /// </summary>
        /// <param name="keyCode">The keycode to check.</param>
        /// <returns><c>true</c> if it is up, <c>false</c> otherwise.</returns>
        private static bool KeyIsUp(int keyCode)
        {
            return GetKeyState(keyCode) >= 0;
        }
    }
}