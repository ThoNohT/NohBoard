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
    using System.Runtime.Serialization;

    /// <summary>
    /// Lists the possible types of keys.
    /// </summary>
    [DataContract]
    public enum KeyType
    {
        /// <summary>
        /// A key on a keyboard.
        /// </summary>
        Keyboard,

        /// <summary>
        /// A key on a mouse.
        /// </summary>
        Mouse,

        /// <summary>
        /// A scroller on a mouse.
        /// </summary>
        MouseScroll,

        /// <summary>
        /// A mouse movement indicator.
        /// </summary>
        MouseMovement
    }
}