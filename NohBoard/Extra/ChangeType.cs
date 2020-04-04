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


namespace ThoNohT.NohBoard.Extra
{
    using System;

    /// <summary>
    /// The types of changes to a keyboard.
    /// </summary>
    [Flags]
    public enum ChangeType
    {
        /// <summary>
        /// No change. This can be used for the initial entry.
        /// </summary>
        None = 0,

        /// <summary>
        /// A change to the keyboard definition.
        /// </summary>
        Definition = 1,

        /// <summary>
        /// A change to the keyboard style.
        /// </summary>
        Style = 2,

        /// <summary>
        /// A change to both the keyboard definition and style.
        /// </summary>
        Both = 3
    }
}
