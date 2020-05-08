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
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using Extra;
    using Keyboard;
    using Keyboard.ElementDefinitions;

    /// <summary>
    /// Parses a legacy (NohBoard 0.x keyboard file).
    /// </summary>
    public class LegacyKbFileParser
    {
        /// <summary>
        /// Parses an old keyboard file.
        /// </summary>
        /// <param name="fileName">The filename of the legacy keyboard file.</param>
        /// <returns>The parsed keyboard definition.</returns>
        public static KeyboardDefinition Parse(string fileName)
        {
            var file = new FileInfo(fileName);

            if (!file.Exists)
                throw new LegacyFileParseException($"{fileName} does not exist.");

            if (file.Extension != ".kb")
                throw new LegacyFileParseException($"{fileName} does not have the '.kb' extension.");

            var definition = new KeyboardDefinition
            {
                Name = file.Name.Substring(0, file.Name.Length - file.Extension.Length),
                Version = Constants.KeyboardVersion,
                Elements = new List<ElementDefinition>()
            };

            var reader = new StreamReader(fileName);
            var index = 0;
            var elementId = 0;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var elements = line.Split(' ');
                index++;

                try
                {
                    switch (elements[0])
                    {
                        case "key":
                            var (keyType, keyCode) = LegacyKeyCodeMapping.Map(int.Parse(elements[1]));
                            switch (keyType)
                            {
                                case KeyType.Keyboard:
                                    AddKeyboardKey(elementId++, elements, definition, keyCode);
                                    break;

                                case KeyType.Mouse:
                                    AddMouseKey(elementId++, elements, definition, keyCode);
                                    break;

                                case KeyType.MouseScroll:
                                    AddMouseScroll(elementId++, elements, definition, keyCode);
                                    break;

                                case KeyType.MouseMovement:
                                    AddMouseMovement(elementId++, elements, definition);
                                    break;

                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            break;
                        case "width":
                            if (elements.Length < 2)
                                throw new LegacyFileParseException(
                                    "A keyboard property line should have at least 2 elements.");

                            definition.Width = ParseInt(elements[1], "Width");
                            break;
                        case "height":
                            if (elements.Length < 2)
                                throw new LegacyFileParseException(
                                    "A keyboard property line should have at least 2 elements.");

                            definition.Height = ParseInt(elements[1], "Height");
                            break;
                        case "category":
                            if (elements.Length < 2)
                                throw new LegacyFileParseException(
                                    "A keyboard property line should have at least 2 elements.");

                            definition.Category = elements[1];
                            break;
                    }
                }
                catch (Exception ex)
                {
                    throw new LegacyFileParseException($"Unable to parse key file line {index}: '{line}'", ex);
                }
            }

            // Fix the size of the keyboard and the position of the keys. Use the standard of 9 pixels on all edges.
            var bb = definition.GetBoundingBox();
            definition.Elements = definition.Elements.Select(x => x.Translate(9 - bb.Left, 9 - bb.Top)).ToList();
            var bb2 = definition.GetBoundingBox();
            definition.Width = bb2.Right + 9;
            definition.Height = bb2.Bottom + 9;

            return definition;
        }

        #region Type adders

        /// <summary>
        /// Adds a keyboard key definition.
        /// </summary>
        /// <param name="id">The identifier of the key.</param>
        /// <param name="elements">The kb file elements describing the key.</param>
        /// <param name="definition">The keyboard definition to add the key to.</param>
        /// <param name="keyCode">The keycode to add.</param>
        private static void AddKeyboardKey(int id, string[] elements, KeyboardDefinition definition, int keyCode)
        {
            if (elements.Length < 10)
                throw new LegacyFileParseException("A key definition line should have at least 10 elements.");

            // Parse the individual elements of the line.
            var posX = ParseInt(elements[2], "X-Position");
            var posY = ParseInt(elements[3], "Y-Position");
            var width = ParseInt(elements[4], "Width");
            var height = ParseInt(elements[5], "Height");
            var normalText = LegacyCharacterMapping.Replace(elements[6]);
            var shiftText = LegacyCharacterMapping.Replace(elements[7]);
            var changeOnCaps = ParseBool(elements[8], "ChangeOnCaps");

            // Construct the new key definition.
            var newKeyDefinition = new KeyboardKeyDefinition(
                id: id,
                boundaries: new List<TPoint>
                {
                    new TPoint(posX, posY),
                    new TPoint(posX + width, posY),
                    new TPoint(posX + width, posY + height),
                    new TPoint(posX, posY + height)
                },
                keyCodes: keyCode.Singleton(),
                normalText: normalText,
                shiftText: shiftText,
                changeOnCaps: changeOnCaps);

            AddOrOverlap(definition, keyCode, newKeyDefinition);
        }

        /// <summary>
        /// Adds a mouse key definition.
        /// </summary>
        /// <param name="id">The identifier of the key.</param>
        /// <param name="elements">The kb file elements describing the key.</param>
        /// <param name="definition">The keyboard definition to add the key to.</param>
        /// <param name="keyCode">The keycode to add.</param>
        private static void AddMouseKey(int id, string[] elements, KeyboardDefinition definition, int keyCode)
        {
            if (elements.Length < 10)
                throw new LegacyFileParseException("A key definition line should have at least 10 elements.");

            // Parse the individual elements of the line.
            var posX = ParseInt(elements[2], "X-Position");
            var posY = ParseInt(elements[3], "Y-Position");
            var width = ParseInt(elements[4], "Width");
            var height = ParseInt(elements[5], "Height");
            var normalText = LegacyCharacterMapping.Replace(elements[6]);

            // Construct the new key definition.
            var newKeyDefinition = new MouseKeyDefinition(
                id: id,
                boundaries: new List<TPoint>
                {
                    new TPoint(posX, posY),
                    new TPoint(posX + width, posY),
                    new TPoint(posX + width, posY + height),
                    new TPoint(posX, posY + height)
                },
                keyCode: keyCode,
                text: normalText);

            AddOrOverlap(definition, keyCode, newKeyDefinition);
        }

        /// <summary>
        /// Adds a mouse scroll definition.
        /// </summary>
        /// <param name="id">The identifier of the key.</param>
        /// <param name="elements">The kb file elements describing the key.</param>
        /// <param name="definition">The keyboard definition to add the key to.</param>
        /// <param name="keyCode">The keycode to add.</param>
        private static void AddMouseScroll(int id, string[] elements, KeyboardDefinition definition, int keyCode)
        {
            if (elements.Length < 10)
                throw new LegacyFileParseException("A key definition line should have at least 10 elements.");

            // Parse the individual elements of the line.
            var posX = ParseInt(elements[2], "X-Position");
            var posY = ParseInt(elements[3], "Y-Position");
            var width = ParseInt(elements[4], "Width");
            var height = ParseInt(elements[5], "Height");
            var normalText = LegacyCharacterMapping.Replace(elements[6]);

            // Construct the new key definition.
            var newKeyDefinition = new MouseScrollDefinition(
                id: id,
                boundaries: new List<TPoint>
                {
                    new TPoint(posX, posY),
                    new TPoint(posX + width, posY),
                    new TPoint(posX + width, posY + height),
                    new TPoint(posX, posY + height)
                },
                keyCode: keyCode,
                text: normalText);

            AddOrOverlap(definition, keyCode, newKeyDefinition);
        }

        /// <summary>
        /// Adds a mouse movement definition.
        /// </summary>
        /// <param name="id">The identifier of the key.</param>
        /// <param name="elements">The kb file elements describing the key.</param>
        /// <param name="definition">The keyboard definition to add the key to.</param>
        private static void AddMouseMovement(int id, string[] elements, KeyboardDefinition definition)
        {
            if (elements.Length < 5)
                throw new LegacyFileParseException("A mouse speed indicator line should have at least 5 elements.");

            // Parse the individual elements of the line.
            var posX = ParseInt(elements[2], "X-Position");
            var posY = ParseInt(elements[3], "Y-Position");
            var width = ParseInt(elements[4], "Width");
            var center = new Point(posX + width / 2, posY + width / 2);

            definition.Elements.Add(new MouseSpeedIndicatorDefinition(id, center, width / 2));
        }

        #endregion Type adders

        /// <summary>
        /// Checks if the key overlaps with any other keys of the same type and keycode. If so, it removes those
        /// keys from the keyboar definition and creates a new one that unifies them. Then the new unified
        /// key is added to the keyboard definition,
        /// </summary>
        /// <typeparam name="TKey">The key type to overlap.</typeparam>
        /// <param name="keyboard">The keyboard definition.</param>
        /// <param name="keyCode">The keycode of the key.</param>
        /// <param name="newKey">The key definition to add or overlap.</param>
        private static void AddOrOverlap<TKey>(KeyboardDefinition keyboard, int keyCode, TKey newKey)
            where TKey : KeyDefinition
        {
            var overlappingEqualKeys = keyboard.Elements.OfType<TKey>()
                // Legacy files don't support multiple keycodes per key.
                .Where(x => x.KeyCodes.Count == 1 && x.KeyCodes.Single() == keyCode)
                .Where(x => x.BordersWith(newKey))
                .ToList();
            foreach (var key in overlappingEqualKeys)
                keyboard.Elements.Remove(key);

            keyboard.Elements.Add(newKey.UnionWith(overlappingEqualKeys.ConvertAll<KeyDefinition>(x => x)));
        }

        #region Parsing

        /// <summary>
        /// Parses an int.
        /// </summary>
        /// <param name="input">The string to parse.</param>
        /// <param name="paramName">The name of the parameter to parse.</param>
        /// <returns>The parsed int.</returns>
        private static int ParseInt(string input, string paramName)
        {
            int result;

            if (!int.TryParse(input, out result))
                throw new ArgumentException($"Unable to parse an integer value from {input}", paramName);

            return result;
        }

        /// <summary>
        /// Parses a bool.
        /// </summary>
        /// <param name="input">The string to parse.</param>
        /// <param name="paramName">The name of the parameter to parse.</param>
        /// <returns>The parsed bool</returns>
        private static bool ParseBool(string input, string paramName)
        {
            if (input == "1")
                return true;
            if (input == "0")
                return false;

            throw new ArgumentException($"Unable to parse a boolean value from {input}.", paramName);
        }

        #endregion Parsing
    }
}