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

namespace ThoNohT.NohBoard
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Contains version information.
    /// </summary>
    [DataContract(Name = "VersionInfo", Namespace = "")]
    public class VersionInfo
    {
        /// <summary>
        /// Gets the major version.
        /// </summary>
        [DataMember]
        public int Major { get; set; }

        /// <summary>
        /// Gets the minor version.
        /// </summary>
        [DataMember]
        public int Minor { get; set; }

        /// <summary>
        /// Gets the patch version.
        /// </summary>
        [DataMember]
        public int Patch { get; set; }

        /// <summary>
        /// Returns a formatted string for this verion.
        /// </summary>
        public string Format()
        {
            return $"v{this.Major}.{this.Minor}.{this.Patch}";
        }
    }
}