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

        /// <summary>
        /// Removes an element from this keyboard definition.
        /// </summary>
        /// <param name="element">The element to remove.</param>
        /// <returns>A new version of this <see cref="KeyboardDefinition"/> without the element that was
        /// removed.</returns>
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

        /// <summary>
        /// Moves <paramref name="element"/> down by a distance of <paramref name="diff"/>.
        /// <paramref name="diff"/> is clamped by the list boundaries.
        /// </summary>
        /// <param name="element">The element to move.</param>
        /// <param name="diff">The distance to move the element down.</param>
        /// <returns>A new version of this <see cref="KeyboardDefinition"/> with the element moved.</returns>
        public KeyboardDefinition MoveElementDown(ElementDefinition element, int diff)
        {
            if (!this.Elements.Contains(element)) throw new Exception("Attempting to move a non-existent element.");
            var index = this.Elements.IndexOf(element);

            // Clamp diff values.
            if (diff == int.MaxValue) diff = int.MaxValue - index;
            if (index + diff < 0) diff = -index;
            if (index + diff >= this.Elements.Count) diff = this.Elements.Count - index - 1;

            var newPosition = index + diff;
            var oldElements = this.Elements.Except(element.Singleton()).ToList();

            var newElements = Enumerable.Range(0, this.Elements.Count)
                .Select(i => i < newPosition ? oldElements[i] : (i == newPosition ? element : oldElements[i - 1]))
                .ToList();

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

        /// <summary>
        /// Adds an element to this keyboard definition.
        /// </summary>
        /// <param name="element">The element to add.</param>
        /// <param name="index">An optional index to add it at. If <c>null</c>, the element is added at the end.</param>
        /// <returns>A new version of this <see cref="KeyboardDefinition"/> with the element added.</returns>
        public KeyboardDefinition AddElement(ElementDefinition element, int? index = null)
        {
            if (this.Elements.Any(e => e.Id == element.Id))
                throw new Exception($"Keyboard already contains an element with id {element.Id}.");

            List<ElementDefinition> newElements;
            if (index == null)
            {
                newElements = this.Elements.Union(element.Singleton()).ToList();
            }
            else
            {
                newElements = Enumerable.Range(0, this.Elements.Count + 1)
                    .Select(i => i < index ? this.Elements[i] : (i == index ? element : this.Elements[i - 1]))
                    .ToList();
            }

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

        /// <summary>
        /// Resizes this keyboard definition.
        /// </summary>
        /// <param name="newSize">The new size.</param>
        /// <returns>A new version of this <see cref="KeyboardDefinition"/> with the new size.</returns>
        public KeyboardDefinition Resize(Size newSize)
        {
            return new KeyboardDefinition
            {
                Category = this.Category,
                Elements = this.Elements.Select(x => x.Clone()).ToList(),
                Width = newSize.Width,
                Height = newSize.Height,
                Name = this.Name,
                Version = this.Version
            };
        }

        /// <summary>
        /// Returns a clone of this keyboard definition.
        /// </summary>
        /// <returns>The cloned keyboard definition.</returns>
        public KeyboardDefinition Clone()
        {
            return new KeyboardDefinition
            {
                Category = this.Category,
                Elements = this.Elements.Select(x => x.Clone()).ToList(),
                Width = this.Width,
                Height = this.Height,
                Name = this.Name,
                Version = this.Version
            };
        }

        /// <summary>
        /// Checks whether the definition has changes relative to the specified other definition.
        /// </summary>
        /// <param name="other">The definition to compare against.</param>
        /// <returns>True if the definition has changes, false otherwise.</returns>
        public bool IsChanged(KeyboardDefinition other)
        {
            if (this.Category != other.Category) return true;
            if (this.Width != other.Width) return true;
            if (this.Height != other.Height) return true;

            if (!this.Elements.Select(e => e.Id).ToSet().SetEquals(other.Elements.Select(e => e.Id)))
                return true;

            var otherElements = other.Elements.ToDictionary(e => e.Id);

            // The same element ids are present, now compare each.
            return this.Elements.Any(e => e.IsChanged(otherElements[e.Id]));
        }

        /// <summary>
        /// Returns the next identifier for an alement definition to be used in this keyboard.
        /// </summary>
        public int GetNextId()
        {
            return this.Elements.Select(e => e.Id).DefaultIfEmpty(0).Max() + 1;
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

            // The definition was saved, so there are now no unsaved definition changes.
            GlobalSettings.UnsavedDefinitionChanges = false;
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

            // Check that there are not duplicate elements.
            var elementIds = kbDef.Elements.Select(e => e.Id);
            if (elementIds.Count() != elementIds.Distinct().Count())
                throw new Exception("Not all element ids are unique in this keyboard definition.");

            kbDef.Category = category;
            kbDef.Name = name;
            return kbDef;
        }
    }
}