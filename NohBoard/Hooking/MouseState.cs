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
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using Interop;
    using static Interop.Defines;
    using static Interop.FunctionImports;

    /// <summary>
    /// A class representing the current state of the mouse. I.e. which buttons are pressed, what is the current
    /// location, movement and scroll state.
    /// </summary>
    public class MouseState : StateBase<MouseKeyCode>
    {
        #region Configuration

        /// <summary>
        /// An interval of 33 ms means updating 30 times per second at max.
        /// </summary>
        private static int updateInterval = 33;

        /// <summary>
        /// The number of samples to use for mouse speed smoothing.
        /// </summary>
        private static int mouseSmooth = 5;

        /// <summary>
        /// Whether to check the mouse speed from the center.
        /// </summary>
        private static bool mouseFromCenter = false;

        #endregion Configuration

        #region State

        /// <summary>
        /// The center of the screen to compare against.
        /// </summary>
        private static List<(Rectangle screen, Point center)> ScreenCenters = new List<(Rectangle, Point)>();

        /// <summary>
        /// The history of recorded mouse speeds.
        /// </summary>
        private static readonly CircleBuffer<SizeF> speedHistory = new CircleBuffer<SizeF>(mouseSmooth, default(SizeF));

        /// <summary>
        /// The last captured location.
        /// </summary>
        private static Point lastLocation;

        /// <summary>
        /// The time of the last location update.
        /// </summary>
        private static int? lastLocationUpdate;

        /// <summary>
        /// Timers used to determine how long to keep a scroll direction as pressed.
        /// </summary>
        private static readonly long[] scrollTimers = new long[4];

        /// <summary>
        /// The counters for the different scroll directions.
        /// </summary>
        private static readonly int[] scrollCounts = new int[4];

        /// <summary>
        /// A stopwatch used for comparing the scroll timers.
        /// </summary>
        private static Stopwatch scrollStopwatch;

        #endregion State

        #region Properties

        /// <summary>
        /// Returns a list of scroll counts.
        /// </summary>
        public static IReadOnlyList<int> ScrollCounts
        {
            get { lock (scrollCounts) return scrollCounts.ToList().AsReadOnly(); }
        }

        /// <summary>
        /// Returns the average mouse speed currently recorded.
        /// </summary>
        public static SizeF AverageSpeed
        {
            get
            {
                var sum = speedHistory.Aggregate((acc, elem) => acc + elem);
                return new SizeF(sum.Width / speedHistory.Size, sum.Height / speedHistory.Size);
            }
        }

        #endregion Properties

        /// <summary>
        /// Sets a value indicating whether to calculate the mouse speed from the center or not.
        /// </summary>
        /// <param name="activate">True if distance should be caculated relative to the screen center,
        /// false if distance should be calculated relative to the previous mouse position.</param>
        /// <remarks>This method also recalculates the screen centers. So if a screen is added or removed while NohBoard
        /// is running, calling this method with <c>true</c> will re-calculate the new screen locations.</remarks>
        /// <param name="screenCenters">The screen centers to calculate for.</param>
        public static void SetMouseFromCenter(bool activate, List<(Rectangle screen, Point center)> screenCenters)
        {
            mouseFromCenter = activate;

            // Determine the screens and their centers.
            if (activate) ScreenCenters = screenCenters;
        }

        /// <summary>
        /// Checks the state of all keys and removes the ones that are no longer pressed from the list of pressed keys.
        /// </summary>
        /// <param name="hold">The minimum time to hold keys.</param>
        public static void CheckKeys(int hold)
        {
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
        /// Checks the scroll keys and movement.
        /// Call this method to remove the scroll buttons from state after they have been added. Also adds zero movement
        /// items to the history to return the movement counter to 0 gradually.
        /// </summary>
        public static void CheckScrollAndMovement()
        {
            if (!speedHistory.All(x => x.IsEmpty))
            {
                speedHistory.Add(new SizeF(0, 0));
                updated = true;
            }

            if (scrollStopwatch == null) return;

            lock (scrollCounts)
            {
                var stillActiveKeys = 0;
                for (var i = 0; i < 4; i++)
                {
                    if (scrollTimers[i] < scrollStopwatch.ElapsedMilliseconds)
                    {
                        scrollCounts[i] = 0;
                        updated = true;
                    }
                    else
                    {
                        stillActiveKeys++;
                    }
                }

                // If there are no active scroll keys, disable the stopwatch, so we can start anew when the next scroll
                // is done.
                if (stillActiveKeys == 0)
                {
                    scrollStopwatch.Stop();
                    scrollStopwatch = null;
                }
            }
        }

        /// <summary>
        /// Adds the specified scroll direction to the list of scroll keys. Resets its timer if it is already
        /// pressed.
        /// </summary>
        /// <param name="keyCode">The keycode of the scroll direction.</param>
        public static void AddScrollDirection(MouseScrollKeyCode keyCode)
        {
            lock (scrollCounts)
            {
                if (scrollStopwatch == null) scrollStopwatch = Stopwatch.StartNew();

                scrollCounts[(int)keyCode] += 1;
                scrollTimers[(int)keyCode] = scrollStopwatch.ElapsedMilliseconds + HookManager.ScrollHold;
                updated = true;
            }
        }

        /// <summary>
        /// Adds the specified mouse keycode to the list of pressed keys.
        /// </summary>
        /// <param name="keyCode">The keycode to add.</param>
        public static void AddPressedElement(MouseKeyCode keyCode)
        {
            lock (pressedKeys)
            {
                EnsureStopwatchRunning();

                var time = keyHoldStopwatch.ElapsedMilliseconds;

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
        /// Removes the specified keycode from the list of pressed keys.
        /// </summary>
        /// <param name="keyCode">The keycode to remove.</param>
        /// <param name="hold">The minimum time to hold keys.</param>
        public static void RemovePressedElement(MouseKeyCode keyCode, int hold)
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
        /// Registers a new location for the mouse.
        /// </summary>
        /// <param name="location">The new location.</param>
        /// <param name="time">The time at which the location was registered.</param>
        public static void RegisterLocation(Point location, int time)
        {
            if (lastLocationUpdate == null)
            {
                // After the first capture, we can't determine any speed yet. Initialize all variables here.
                lastLocation = mouseFromCenter ? GetScreenCenterForPoint(location) ?? location : location;
                lastLocationUpdate = time;
                return;
            }

            // We have prior information now, so let's cuddle some bears and hunt a duck.

            // Don't do it too quick, if the system don't think time has passed the duck'll divide by zero
            // 30 ducks per bear should be sufficient.
            if (time - lastLocationUpdate.Value < updateInterval) return;

            speedHistory.Add(GetSpeed(location, lastLocation, time - lastLocationUpdate.Value));

            // Update the stored values
            lastLocationUpdate = time;
            lastLocation = mouseFromCenter ? GetScreenCenterForPoint(location) ?? location : location;

            updated = true;
        }

        /// <summary>
        /// Gets the coordinates of the center of the screen that contains <paramref name="point"/>.
        /// </summary>
        /// <param name="point">The point to get the screen center for.</param>
        /// <returns>The retrieved screen center.</returns>
        /// <remarks>The screen centers are pre-calculated whenever <see cref="SetMouseFromCenter"/> is
        /// set to <c>true</c>.</remarks>
        private static Point? GetScreenCenterForPoint(Point point)
        {
            Func<Rectangle, Point, bool> contains =
                (r, p) => p.X >= r.Left && p.X <= r.Right && p.Y >= r.Top && p.Y <= r.Bottom;

            var result = ScreenCenters.Where(t => contains(t.screen, point))
                .Select(t => (Point?)t.center).FirstOrDefault();
            // For some reason a point can be in multiple screens at the same time? Probably while dragging the window
            // around over to another window. To bypass this, we just get the first one, and it'll have to do. There is
            // no way to prioritize any of the screens.

            return result;
        }

        /// <summary>
        /// Calculates the speed given two points and a time difference.
        /// </summary>
        /// <param name="target">The target point</param>
        /// <param name="source">The source point.</param>
        /// <param name="dt">The time difference.</param>
        /// <returns>The speed.</returns>
        private static SizeF GetSpeed(Point target, Point source, int dt)
        {
            var dx = target.X - source.X;
            var dy = target.Y - source.Y;

            return new SizeF((float)dx / dt, (float)dy / dt);
        }

        /// <summary>
        /// Checks whether the specified key is up.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns><c>true</c> if it is up, <c>false</c> otherwise.</returns>
        private static bool KeyIsUp(MouseKeyCode key)
        {
            switch (key)
            {
                case MouseKeyCode.LeftButton:
                    return GetKeyState(VK_LBUTTON) >= 0;

                case MouseKeyCode.RightButton:
                    return GetKeyState(VK_RBUTTON) >= 0;

                case MouseKeyCode.MiddleButton:
                    return GetKeyState(VK_MBUTTON) >= 0;

                case MouseKeyCode.X1Button:
                    return GetKeyState(VK_XBUTTON1) >= 0;

                case MouseKeyCode.X2Button:
                    return GetKeyState(VK_XBUTTON2) >= 0;
            }

            return false;
        }
    }
}