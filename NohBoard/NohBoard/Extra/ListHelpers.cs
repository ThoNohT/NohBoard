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
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Contains helper methods for lists.
    /// </summary>
    public static class ListHelpers
    {
        /// <summary>
        /// Returns <c>true</c> if <paramref name="src"/> contains all elements in <paramref name="toContain"/>.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="src">The source list.</param>
        /// <param name="toContain">The list of elements that should be contained.</param>
        /// <returns><c>true</c> or <c>false</c> depending on whether all elements are contained.</returns>
        public static bool ContainsAll<T>(this IEnumerable<T> src, IEnumerable<T> toContain)
        {
            return toContain.All(src.Contains);
        }

        /// <summary>
        /// Returns a list with <paramref name="element"/> in it.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="element">The element to return as a singleton list.</param>
        /// <returns>The list.</returns>
        public static List<T> Singleton<T>(this T element)
        {
            return new List<T> { element };
        }

        /// <summary>
        /// Converts an enumerable to a set.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="enumerable">The enumerable to convert.</param>
        /// <returns>The converted set.</returns>
        public static HashSet<T> ToSet<T>(this IEnumerable<T> enumerable)
        {
            return new HashSet<T>(enumerable);
        }
    }
}