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
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// A base class for keyboard and mouse state.
    /// </summary>
    /// <typeparam name="T">The type of the key being tracked.</typeparam>
    public class StateBase<T>
    {
        /// <summary>
        /// Contains information about a recorded keypress.
        /// </summary>
        protected struct KeyPress
        {
            /// <summary>
            /// The time in milliseconds when the press was detected.
            /// </summary>
            public long startTime { get; set; }

            /// <summary>
            /// A value indicating whether the press should be removed, once the hold time is elapsed.
            /// </summary>
            public bool removed { get; set; }
        }

        /// <summary>
        /// A dictionary containing all currently pressed keys.
        /// </summary>
        protected static readonly Dictionary<T, KeyPress> pressedKeys = new Dictionary<T, KeyPress>();

        /// <summary>
        /// A value indicating whether something has changed since the last check.
        /// </summary>
        protected static bool updated;

        /// <summary>
        /// A stopwatch for measuring how long to hold key presses.
        /// </summary>
        protected static Stopwatch keyHoldStopwatch;

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
        public static IReadOnlyList<T> PressedKeys
        {
            get { lock (pressedKeys) return pressedKeys.Keys.ToList().AsReadOnly(); }
        }

        /// <summary>
        /// Checks all key holds, and any holds that are exceeded are removed from the pressed keys list.
        /// </summary>
        /// <param name="hold">The minimum time to hold keys.</param>
        public static void CheckKeyHolds(int hold)
        {
            lock (pressedKeys)
            {
                if (!pressedKeys.Where(t => t.Value.removed).Any()) return;

                // There are removed keys, so we do want to check again soon to see if we need to take them out of the
                // pressed keys list.
                updated = true;

                var time = keyHoldStopwatch.ElapsedMilliseconds;

                foreach (var key in pressedKeys
                    .Where(t => t.Value.removed)
                    .Where(t => t.Value.startTime + hold < time)
                    .Select(t => t.Key).ToList())
                {
                    pressedKeys.Remove(key);
                }

                TryStopStopwatch();
            }
        }

        /// <summary>
        /// Starts a new key hold stopwatch if one is not already runing.
        /// </summary>
        protected static void EnsureStopwatchRunning()
        {
            if (keyHoldStopwatch is null || !keyHoldStopwatch.IsRunning)
                keyHoldStopwatch = Stopwatch.StartNew();
        }

        /// <summary>
        /// Checks if there are any pressed keys left. If not, stops the key hold stopwatch.
        /// </summary>
        protected static void TryStopStopwatch()
        {
            if (!pressedKeys.Any())
            {
                keyHoldStopwatch.Stop();
            }
        }
    }
}