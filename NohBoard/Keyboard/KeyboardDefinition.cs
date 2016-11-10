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

namespace ThoNohT.NohBoard.Keyboard
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using ElementDefinitions;
    using Extra;

    /// <summary>
    /// Represents a keyboard, can be serialized to a keyboard file.
    /// </summary>
    [DataContract(Name = "Keyboard", Namespace = "")]
    public class KeyboardDefinition
    {
        #region Properties

        /// <summary>
        /// A friendly name of the keyboard.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The category of the keyboard.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// The version of the keyboard.
        /// </summary>
        [DataMember]
        public int Version { get; set; }

        /// <summary>
        /// The width of the keyboard, in pixels.
        /// </summary>
        [DataMember]
        public int Width { get; set; }

        /// <summary>
        /// The height of the keyboard, in pixels.
        /// </summary>
        [DataMember]
        public int Height { get; set; }

        /// <summary>
        /// The list of elements defined in this keyboard.
        /// </summary>
        [DataMember]
        public List<ElementDefinition> Elements { get; set; }

        #endregion Properties

        #region Modification

        public KeyboardDefinition RemoveElement(ElementDefinition element)
        {
            var newElements = this.Elements.Where(e => e.Id != element.Id).ToList();
            if (newElements.Count != this.Elements.Count - 1)
                throw new Exception($"Keyboard contains no, or too many elements with id {element.Id}.");

            return new KeyboardDefinition
            {
                Category = this.Category,
                Elements = newElements,
                Width = this.Width,
                Height = this.Height,
                Name = this.Name,
                Version = this.Version
            };
        }

        public KeyboardDefinition AddElement(ElementDefinition element)
        {
            if (this.Elements.Any(e => e.Id == element.Id))
                throw new Exception($"Keyboard already contains an element with id {element.Id}.");
            var newElements = this.Elements.Union(element.Singleton()).ToList();

            return new KeyboardDefinition
            {
                Category = this.Category,
                Elements = newElements,
                Width = this.Width,
                Height = this.Height,
                Name = this.Name,
                Version = this.Version
            };
        }

        #endregion Modification

        /// <summary>
        /// Calculates the bounding box of all elements in the keyboard definition.
        /// </summary>
        /// <returns>The calculated bounding box.</returns>
        public Rectangle GetBoundingBox()
        {
            var minX = this.Elements.Select(x => x.GetBoundingBox().X).Min();
            var minY = this.Elements.Select(x => x.GetBoundingBox().Y).Min();

            return new Rectangle(
                new Point(minX, minY),
                new Size(
                    this.Elements.Select(x => x.GetBoundingBox().Right).Max() - minX,
                    this.Elements.Select(x => x.GetBoundingBox().Bottom).Max() - minY));
        }

        /// <summary>
        /// Saves this keyboard definition. The path is defined from the category and name of this keyboard.
        /// </summary>
        public void Save()
        {
            var filename = Path.Combine(
                Constants.ExePath,
                Constants.KeyboardsFolder,
                this.Category,
                this.Name,
                Constants.DefinitionFilename);

            FileHelper.EnsurePathExists(filename);
            FileHelper.Serialize(filename, this);
        }

        /// <summary>
        /// Loads a new keyboard definition.
        /// </summary>
        /// <param name="category">The category to load the keyboard from.</param>
        /// <param name="name">The name of the keyboard to load.</param>
        /// <returns>The loaded <see cref="KeyboardDefinition"/>.</returns>
        public static KeyboardDefinition Load(string category, string name)
        {
            var categoryPath = FileHelper.FromKbs(category);
            if (!categoryPath.Exists)
                throw new ArgumentException($"Category {category} does not exist.");

            var keyboardPath = Path.Combine(categoryPath.FullName, name);
            if (!Directory.Exists(keyboardPath))
                throw new ArgumentException($"Keyboard {name} does not exist.");

            var filePath = Path.Combine(keyboardPath, Constants.DefinitionFilename);
            if (!File.Exists(filePath))
                throw new Exception($"Keyboard definition file not found for {category}/{name}.");

            // Version check
            var readLines = File.ReadLines(filePath);
            var versionLine = readLines.SingleOrDefault(l => l.Contains("\"Version\": "));
            if (versionLine == null) throw new Exception("Keyboard does not contain version information.");
            int version;
            try
            {
                version = int.Parse(
                    versionLine.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries).Last().TrimEnd(','));
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to determine version info.", ex);
            }

            if (version < Constants.KeyboardVersion)
            {
                throw new Exception(
                    $"This version of NohBoard requires keyboards of version {Constants.KeyboardVersion}, " +
                    $"but this keyboard is of version {version}.");
            }

            var kbDef = FileHelper.Deserialize<KeyboardDefinition>(filePath);
            kbDef.Category = category;
            kbDef.Name = name;
            return kbDef;
        }
    }
}