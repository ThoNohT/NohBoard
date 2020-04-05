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
    public class KeyboardState : StateBase<int>
    {
        /// <summary>
        /// A dictionary mapping key codes to the key codes of the state keys they update.
        /// </summary>
        private static Dictionary<int, int> StateKeys = new Dictionary<int, int>
        {
            { VK_CAPITAL, 1026 },
            { VK_NUMLOCK, 1027 },
            { VK_SCROLL, 1028 }
        };

        /// <summary>
        /// Initializes the state of state keys.
        /// </summary>
        static KeyboardState()
        {
            foreach (var key in StateKeys)
            {
                // Note that during this initialization, hold is not relevant as we are only adding keys that are
                // currently active. So passing 0 is not an issue.
                if (CheckStateKey(key.Key)) AddPressedElement(key.Value, 0);
            }
        }

        /// <summary>
        /// Returns a value indicating whether any shift key is currently down.
        /// </summary>
        public static bool ShiftDown
        {
            get { lock (pressedKeys) return pressedKeys.ContainsKey(VK_LSHIFT) || pressedKeys.ContainsKey(VK_RSHIFT); }
        }

        /// <summary>
        /// Returns a value indicating whether any ctrl key is currently down.
        /// </summary>
        public static bool CtrlDown
        {
            get { lock (pressedKeys) return pressedKeys.ContainsKey(VK_LCTRL) || pressedKeys.ContainsKey(VK_RCTRL); }
        }

        /// <summary>
        /// Returns a value indicating whether any alt key is currently down.
        /// </summary>
        public static bool AltDown
        {
            get { lock (pressedKeys) return pressedKeys.ContainsKey(VK_LALT) || pressedKeys.ContainsKey(VK_RALT); }
        }

        /// <summary>
        /// Returns a value indicating whether caps lock is currently active.
        /// </summary>
        public static bool CapsActive => CheckStateKey(VK_CAPITAL);

        /// <summary>
        /// Checks the state of all keys and removes the ones that are no longer pressed from the list of pressed keys.
        /// </summary>
        /// <param name="hold">The minimum time to hold keys.</param>
        public static void CheckKeys(int hold)
        {
            return;
            lock (pressedKeys)
            {
                if (!pressedKeys.Any()) return;

                var time = keyHoldStopwatch.ElapsedMilliseconds;

                foreach (var key in pressedKeys.Where(t => KeyIsUp(t.Key)).Select(t => t.Key).ToList())
                {
                    var pressed = pressedKeys[key];

                    if (pressed.startTime + hold < time)
                    {
                        pressedKeys.Remove(key);
                    }
                    else
                    {
                        pressed.removed = true;
                        pressedKeys[key] = pressed;
                    }

                    // Always update to keep checking whether to remove the key on the next render cycle.
                    updated = true;
                }

                TryStopStopwatch();
            }
        }

        /// <summary>
        /// Adds the specified mouse keycode to the list of pressed keys.
        /// </summary>
        /// <param name="keyCode">The keycode to add.</param>
        /// <param name="hold">The minimum time to hold keys.</param>
        public static void AddPressedElement(int keyCode, int hold)
        {
            lock (pressedKeys)
            {
                EnsureStopwatchRunning();

                var time = keyHoldStopwatch.ElapsedMilliseconds;

                TryToggleStateKey(keyCode, hold);

                if (pressedKeys.TryGetValue(keyCode, out var pressed))
                {
                    pressed.startTime = time;
                    pressed.removed = false;
                    pressedKeys[keyCode] = pressed;
                }
                else
                {
                    pressedKeys.Add(
                        keyCode,
                        new KeyPress
                        {
                            startTime = keyHoldStopwatch.ElapsedMilliseconds,
                            removed = false
                        });

                    updated = true;
                }

            }
        }

        /// <summary>
        /// Attempts to toggle a key that can have a state. If the key is valid for having a state, the current state
        /// is looked up and removed or added from the list of pressed keys.
        /// </summary>
        /// <param name="keyCode">The key code of the key to check.</param>
        /// <param name="hold">The minimum time to hold keys.</param>
        private static void TryToggleStateKey(int keyCode, int hold)
        {
            if (!StateKeys.TryGetValue(keyCode, out var stateKey)) return;

            // The state at this moment is that before the switch.
            if (!CheckStateKey(keyCode))
            {
                AddPressedElement(stateKey, hold);
            }
            else
            {
                RemovePressedElement(stateKey, hold);
            }
        }

        /// <summary>
        /// Removes the specified keycode from the list of pressed keys.
        /// </summary>
        /// <param name="keyCode">The keycode to remove.</param>
        /// <param name="hold">The minimum time to hold keys.</param>
        public static void RemovePressedElement(int keyCode, int hold)
        {
            lock (pressedKeys)
            {
                if (!pressedKeys.ContainsKey(keyCode)) return;

                var time = keyHoldStopwatch.ElapsedMilliseconds;

                var pressed = pressedKeys[keyCode];

                if (pressed.startTime + hold < time)
                {
                    pressedKeys.Remove(keyCode);
                }
                else
                {
                    pressed.removed = true;
                    pressedKeys[keyCode] = pressed;
                }

                // Always update to keep checking whether to remove the key on the next render cycle.
                updated = true;
                TryStopStopwatch();
            }
        }

        /// <summary>
        /// Checks whether the state key is active.
        /// </summary>
        /// <param name="keyCode">The key code of the key to check the state of.</param>
        /// <returns>A value indicating whether its state is active.</returns>
        private static bool CheckStateKey(int keyCode)
        {
            return (GetKeyState(keyCode) & 0x1) != 0;
        }

        /// <summary>
        /// Checks whether the specified key is up.
        /// </summary>
        /// <param name="keyCode">The keycode to check.</param>
        /// <returns><c>true</c> if it is up, <c>false</c> otherwise.</returns>
        private static bool KeyIsUp(int keyCode)
        {
            if (StateKeys.Values.Contains(keyCode))
            {
                var actualCode = StateKeys.Single(k => k.Value == keyCode).Key;
                return !CheckStateKey(actualCode);
            }

            return GetKeyState(keyCode) >= 0;
        }
    }
}