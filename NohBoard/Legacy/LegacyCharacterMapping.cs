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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents the mapping between special codes and their unicode characters as defined in legacy NohBoard.
    /// </summary>
    public static class LegacyCharacterMapping
    {
        /// <summary>
        /// A list containing characters that can be directly replaced.
        /// </summary>
        private static readonly Dictionary<string, string> constantReplacements = new Dictionary<string, string>
        {
            // Special characters
            { "%/o%", "ø" },
            { "%/O%", "Ø" },
            { "%ae%", "æ" },
            { "%AE%", "Æ" },
            { "%oe%", "œ" },
            { "%OE%", "Œ" },
            { "%,c%", "ç" },
            { "%,C%", "Ç" },
            { "%''%", "´" },
            { "%par%", "§" },
            { "%circle%", "°" },
            { "%mu%", "µ" },
            { "%sqr%", "²" },
            { "%gbp%", "£" },
            { "%ss%", "ß" },
            { "%oa%", "å" },
            { "%OA%", "Å" },
            { "%half%", "½" },
            { "%diaeresis%", "¨" },
            { "%currency%", "¤" },

            // Signs
            { "%up%", "↑" },
            { "%down%", "↓" },
            { "%left%", "←" },
            { "%right%", "→" },
            { "%return%", "↵" },
            { "%shift%", "⇑" },
            { "%lup%", "↖" },

            // Whitespace
            { "%20%", " " },
            { "%0%", "" }
        };

        /// <summary>
        /// Replaces the special codes in legacy keyboard files with their unicode characters.
        /// </summary>
        /// <param name="input">The input string to replace.</param>
        /// <returns>The string with all replacements performed.</returns>
        public static string Replace(string input)
        {
            // Perform the constantly defined replacements.
            input = constantReplacements
                .Aggregate(input, (current, replacement) => current.Replace(replacement.Key, replacement.Value));

            // Perform diacritics replacement.
            var letters = new[] { 'A', 'O', 'E', 'I', 'U', 'a', 'o', 'e', 'i', 'u' };
            var diaeresis = new[] { "Ä", "Ö", "Ë", "Ï", "Ü", "ä", "ö", "ë", "ï", "ü" };
            var tilde = new[] { "Ã", "Õ", "Ẽ", "Ï", "Ũ", "ã", "õ", "ẽ", "ï", "ũ" };
            var circumflex = new[] { "Â", "Ô", "Ê", "Î", "Ũ", "â", "ô", "ê", "î", "ũ" };
            var grave = new[] { "À", "Ò", "È", "Ì", "Ù", "à", "ò", "è", "ì", "ù" };
            var acute = new[] { "Á", "Ó", "É", "Í", "Ú", "á", "ó", "é", "í", "ú" };

            for (var i = 0; i < letters.Length; i++)
            {
                var letter = letters[i];
                input = Regex.Replace(input, $"%\"{letter}%", diaeresis[i]);
                input = Regex.Replace(input, $"%~{letter}%", tilde[i]);
                input = Regex.Replace(input, $"%\\^{letter}%", circumflex[i]);
                input = Regex.Replace(input, $"%'{letter}%", grave[i]);
                input = Regex.Replace(input, $"%{letter}'%", acute[i]);
            }

            return input;
        }
    }
}