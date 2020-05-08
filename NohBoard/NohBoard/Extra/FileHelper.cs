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
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml;

    /// <summary>
    /// A class containing helper methods for interacting with the file system.
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// Ensures that a provided path exists. If it does not exist, it any any missing parent directories are
        /// created.
        /// </summary>
        /// <param name="path">The path that should be ensured of existence.</param>
        public static void EnsurePathExists(string path)
        {
            var file = new FileInfo(path);
            var dir = file.Directory;

            if (dir.Exists) return;

            EnsurePathExists(dir.Parent.FullName);
            dir.Create();
        }

        /// <summary>
        /// Deserializes a filename into a <typeparamref name="T"/> instance.
        /// </summary>
        /// <typeparam name="T">The type to deserialize into.</typeparam>
        /// <param name="filename">The filename containing the data to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        public static T Deserialize<T>(string filename) where T: class
        {
            if (!File.Exists(filename)) return null;

            using (var fileStream = new FileStream(filename, FileMode.Open))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                using (var reader = JsonReaderWriterFactory.CreateJsonReader(
                    fileStream,
                    Encoding.UTF8,
                    XmlDictionaryReaderQuotas.Max,
                    dictionaryReader => { }))
                {
                    return (T)serializer.ReadObject(reader);
                }
            }
        }

        /// <summary>
        /// Serializes <paramref name="obj"/> into a string and stores it in the specified filename.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="filename">The filename to write the serialized object to.</param>
        /// <param name="obj">The object to serialize.</param>
        public static void Serialize<T>(string filename, T obj)
        {
            using (var fileStream = new FileStream(filename, FileMode.Create))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                using (var writer = JsonReaderWriterFactory.CreateJsonWriter(fileStream, Encoding.UTF8, true, true))
                {
                    serializer.WriteObject(writer, obj);
                }
            }
        }

        /// <summary>
        /// Serializes <paramref name="obj"/> into a string and returns the string.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>The serialized string/</returns>
        public static string Serialize<T>(T obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                using (var writer = JsonReaderWriterFactory.CreateJsonWriter(memoryStream, Encoding.UTF8, true, true))
                {
                    serializer.WriteObject(writer, obj);
                }

                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }

        /// <summary>
        /// Ensures that a filename does not contain illegal characters or points to sub or parent folders.
        /// </summary>
        /// <param name="filename">The filename to sanitize.</param>
        /// <returns>The sanitized filename.</returns>
        public static string SanitizeFilename(this string filename)
        {
            return filename == null ? null : Regex.Replace(filename, @"[^\w\d\.\-_~]", "");
        }

        /// <summary>
        /// Returns a <see cref="DirectoryInfo"/> for the path to the specified parts, from the keyboards folder.
        /// </summary>
        /// <param name="parts">The parts that make up the path from the keyboards folder.</param>
        /// <returns>The specified <see cref="DirectoryInfo"/>.</returns>
        public static DirectoryInfo FromKbs(params string[] parts)
        {
            var array = new List<string> { Constants.ExePath, Constants.KeyboardsFolder };
            array.AddRange(parts);
            return new DirectoryInfo(Path.Combine(array.ToArray()));
        }

        /// <summary>
        /// Checks whether an image exists for the currently loaded style.
        /// </summary>
        /// <param name="fileName">The filename of the image.</param>
        /// <returns>A value indicating whether the image exists.</returns>
        public static bool StyleImageExists(string fileName)
        {
            return File.Exists(GetStyleImagePath(fileName));
        }

        /// <summary>
        /// Gets the path to the image with the provided filename for the currently loaded style.
        /// </summary>
        /// <param name="filename">The filename of the image to get the path to.</param>
        /// <returns>The path to the provided image in the current style.</returns>
        /// <remarks>Images are not relative to styles, only relative to category. A recommended image filename is
        /// keyboard_style_imageName.png, for global styles these are directly in the global/images folder,
        /// for non global styles, these are located in the category/images folder.</remarks>
        public static string GetStyleImagePath(string filename)
        {
            var s = GlobalSettings.Settings;
            return Path.Combine(
                s.LoadedGlobalStyle
                    ? FromKbs(Constants.GlobalStylesFolder, Constants.ImagesFolder).FullName
                    : FromKbs(s.LoadedCategory, Constants.ImagesFolder).FullName,
                filename);
        }
    }
}
 