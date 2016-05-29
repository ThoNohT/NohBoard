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

    /// <summary>
    /// Represents an exception that occurred during the parsing of a legacy kb file.
    /// </summary>
    public class LegacyFileParseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LegacyFileParseException" /> class.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        public LegacyFileParseException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LegacyFileParseException" /> class.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        /// <param name="innerException">The inner exception.</param>
        public LegacyFileParseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}