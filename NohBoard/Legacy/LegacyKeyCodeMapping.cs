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

namespace ThoNohT.NohBoard.Legacy
{
    using System;
    using ThoNohT.NohBoard.Hooking;

    /// <summary>
    /// Represents the mapping between legacy keycodes that were invented for mouse buttons and keyboard buttons.
    /// </summary>
    public static class LegacyKeyCodeMapping
    {
        /// <summary>
        /// Maps the specified key code to a key type and a key code for this type.
        /// </summary>
        /// <param name="keyCode">The key code to map.</param>
        /// <returns>The mapped key type and code.</returns>
        public static (KeyType, int) Map(int keyCode)
        {
            // 1025 will still be mapped to the Enter key on the numpad. There is no other suitable key-code for this.
            if (keyCode <= 1025)
                return (KeyType.Keyboard, keyCode);

            // Mouse buttons.
            if (keyCode == 1026)
                return (KeyType.Mouse, (int)MouseKeyCode.LeftButton);
            if (keyCode == 1027)
                return (KeyType.Mouse, (int)MouseKeyCode.RightButton);

            // Mouse movement.
            if (keyCode == 1028)
                return (KeyType.MouseMovement, 0);

            // Mouse scrolling.
            if (keyCode == 1029)
                return (KeyType.MouseScroll, (int)MouseScrollKeyCode.ScrollUp);
            if (keyCode == 1030)
                return (KeyType.MouseScroll, (int)MouseScrollKeyCode.ScrollDown);
            if (keyCode == 1031)
                return (KeyType.MouseScroll, (int)MouseScrollKeyCode.ScrollRight);
            if (keyCode == 1032)
                return (KeyType.MouseScroll, (int)MouseScrollKeyCode.ScrollLeft);

            // Morem ouse buttons.
            if (keyCode == 1033)
                return (KeyType.Mouse, (int)MouseKeyCode.MiddleButton);
            if (keyCode == 1034)
                return (KeyType.Mouse, (int)MouseKeyCode.X1Button);
            if (keyCode == 1035)
                return (KeyType.Mouse, (int)MouseKeyCode.X2Button);

            throw new ArgumentOutOfRangeException(nameof(keyCode));
        }
    }
}
