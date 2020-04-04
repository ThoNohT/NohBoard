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
    using Extra;
    using Styles;

    /// <summary>
    /// The style for a keyboard.
    /// </summary>
    [DataContract(Name = "KeyboardStyle", Namespace = "")]
    public class KeyboardStyle
    {
        /// <summary>
        /// The filename to save this style as.
        /// </summary>
        private string FileName => this.Name + StyleExtension;

        /// <summary>
        /// The extension used for styles.
        /// </summary>
        public const string StyleExtension = ".style";

        /// <summary>
        /// The name of this style.
        /// </summary>
        public string Name { get; set; } = "unnamed";

        /// <summary>
        /// Indicates whether this style is global. When a style is global, it can be used for any
        /// keyboard definition. A style is global when it has no styles for specific elements.
        /// </summary>
        public bool IsGlobal => !this.ElementStyles.Any();

        #region The keyboard itself

        /// <summary>
        /// The background color of the keyboard.
        /// </summary>
        [DataMember]
        public SerializableColor BackgroundColor { get; set; } = Color.FromArgb(0, 0, 100);

        /// <summary>
        /// The filename of the background image, relative to the style's images folder.
        /// </summary>
        [DataMember]
        public string BackgroundImageFileName { get; set; }

        #endregion The keyboard itself

        #region Defaults for elements

        /// <summary>
        /// The default style for all key definitions; KeyboardKeyDefinition, MouseKeyDefinition and
        /// MouseScrollDefinition.
        /// </summary>
        [DataMember]
        public KeyStyle DefaultKeyStyle { get; set; } = new KeyStyle();

        /// <summary>
        /// The default style for MouseSpeedIndicatorDefinition elements.
        /// </summary>
        [DataMember]
        public MouseSpeedIndicatorStyle DefaultMouseSpeedIndicatorStyle { get; set; } = new MouseSpeedIndicatorStyle();

        #endregion Defaults for elements

        /// <summary>
        /// A dictionary of styles for specific elements. The key in this dictionary represents the identifier of the
        /// element the style applies to.
        /// </summary>
        [DataMember]
        public Dictionary<int, ElementStyle> ElementStyles { get; set; } = new Dictionary<int, ElementStyle>();

        /// <summary>
        /// Returns a clone of this keyboard style.
        /// </summary>
        /// <returns>The cloned keyboard style.</returns>
        public KeyboardStyle Clone()
        {
            return new KeyboardStyle
            {
                Name = this.Name,
                BackgroundColor = this.BackgroundColor,
                BackgroundImageFileName = this.BackgroundImageFileName,
                DefaultKeyStyle = (KeyStyle) this.DefaultKeyStyle.Clone(),
                DefaultMouseSpeedIndicatorStyle =
                    (MouseSpeedIndicatorStyle) this.DefaultMouseSpeedIndicatorStyle.Clone(),
                ElementStyles = this.ElementStyles.Select(s => (s.Key, s.Value.Clone()))
                    .ToDictionary(s => s.Item1, s => s.Item2)
            };
        }


        /// <summary>
        /// Returns a value indicating whether the specified element has a custom style.
        /// </summary>
        /// <param name="elementId">The identifier of the element to check for a style.</param>
        /// <returns>True if there is a style for the element, false otherwise.</returns>
        public bool ElementIsStyled(int elementId)
        {
            return this.ElementStyles.ContainsKey(elementId);
        }

        /// <summary>
        /// Sets the style for the specified element.
        /// </summary>
        /// <param name="index">The identifier of the element to set the style for.</param>
        /// <param name="style">The style to set.</param>
        /// <returns>A new version of this <see cref="KeyboardStyle"/> with the element style set.</returns>
        public KeyboardStyle SetElementStyle(int id, ElementStyle style)
        {
            var result = this.Clone();
            if (this.ElementStyles.TryGetValue(id, out _))
            {
                result.ElementStyles[id] = style.Clone();
                return result;
            }
            else
            {
                result.ElementStyles.Add(id, style.Clone());
                return result;
            }
        }

        /// <summary>
        /// Removes the element style for the specified element.
        /// </summary>
        /// <param name="id">The identifier of the element to set the style for.</param>
        /// <returns>A new version of this <see cref="KeyboardStyle"/> with the element style removed.</returns>
        public KeyboardStyle RemoveElementStyle(int id)
        {
            var result = this.Clone();
            result.ElementStyles.Remove(id);
            return result;
        }

        /// <summary>
        /// Checks whether the style has changes relative to the specified other style.
        /// </summary>
        /// <param name="other">The style to compare against.</param>
        /// <returns>True if the style has changes, false otherwise.</returns>
        public bool IsChanged(KeyboardStyle other)
        {
            if (this.BackgroundColor.IsChanged(other.BackgroundColor)) return true;
            if (this.BackgroundImageFileName != other.BackgroundImageFileName) return true;
            if (this.BackgroundColor != other.BackgroundColor) return true;
            if (this.DefaultKeyStyle.IsChanged(other.DefaultKeyStyle)) return true;
            if (this.DefaultMouseSpeedIndicatorStyle.IsChanged(other.DefaultMouseSpeedIndicatorStyle)) return true;

            if (!this.ElementStyles.Keys.ToSet().SetEquals(other.ElementStyles.Keys)) return true;

            // the same element ids are present, now compare each.
            return this.ElementStyles.Any(e => e.Value.IsChanged(other.ElementStyles[e.Key]));
        }

        /// <summary>
        /// Saves this keyboard style.
        /// </summary>
        /// <param name="global">Indicates whether to save it as a global style or as a definition specific
        /// style.</param>
        public void Save(bool global)
        {
            if (global && !this.IsGlobal)
                throw new InvalidOperationException("Cannot save a non-global style globally.");

            var cDef = GlobalSettings.CurrentDefinition;
            var filename = global
                ? FileHelper.FromKbs(Constants.GlobalStylesFolder, this.FileName).FullName
                : FileHelper.FromKbs(cDef.Category, cDef.Name, this.FileName).FullName;

            FileHelper.EnsurePathExists(filename);
            FileHelper.Serialize(filename, this);

            // The style was saved, so there are now no unsaved style changes.
            GlobalSettings.UnsavedStyleChanges = false;
        }

        /// <summary>
        /// Loads a keyboard style for the currently loaded definition.
        /// </summary>
        /// <param name="name">The name of the style to load.</param>
        /// <param name="global">Indicates whether to load a global style or not.</param>
        /// <returns>The loaded style.</returns>
        public static KeyboardStyle Load(string name, bool global)
        {
            var cDef = GlobalSettings.CurrentDefinition;
            var filePath = global
                ? FileHelper.FromKbs(Constants.GlobalStylesFolder, $"{name}{StyleExtension}").FullName
                : FileHelper.FromKbs(cDef.Category, cDef.Name, $"{name}{StyleExtension}").FullName;

            var currentPath = global ? Constants.GlobalStylesFolder : $"{cDef.Category}/{cDef.Name}";

            if (!File.Exists(filePath))
                throw new Exception($"Style file not found for {currentPath}/{name}.");

            var kbStyle = FileHelper.Deserialize<KeyboardStyle>(filePath);

            kbStyle.Name = name;
            return kbStyle;
        }

        /// <summary>
        /// Tries to get the element style for the key with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the key.</param>
        /// <returns>The retrieved identifier, or null if it is not defined.</returns>
        public T TryGetElementStyle<T>(int id) where T: ElementStyle
        {
            var success = this.ElementStyles.TryGetValue(id, out var style);

            // Not found, return null.
            if (!success) return null;

            // Return the style if it is the right style, null otherwise.
            return style as T;
        }
    }
}