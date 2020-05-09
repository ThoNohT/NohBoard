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
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    /// <summary>
    /// Contains constants used throughout the application.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The current keyboard file version.
        /// </summary>
        public const int KeyboardVersion = 2;

        /// <summary>
        /// The subfolder in the NohBoard executable folder that contains keyboard definitions.
        /// </summary>
        public const string KeyboardsFolder = "keyboards";

        /// <summary>
        /// The subfolder in the keyboard definitions folder that contains global styles.
        /// </summary>
        public const string GlobalStylesFolder = "global";

        /// <summary>
        /// The filename of the main keyboard definition file for a keyboard.
        /// </summary>
        public const string DefinitionFilename = "keyboard.json";

        /// <summary>
        /// The name of the folder containing images for styles.
        /// </summary>
        public const string ImagesFolder = "images";

        /// <summary>
        /// The default size in pixels for a new element.
        /// </summary>
        public const int DefaultElementSize = 40;

        /// <summary>
        /// A GDI+ graphics context.
        /// </summary>
        public static Graphics G => Graphics.FromHwndInternal(new Form().Handle);

        /// <summary>
        /// The filename of the settings file.
        /// </summary>
        public static string SettingsFilename => "NohBoard.json";

        /// <summary>
        /// Returns the path this executable is running in.
        /// </summary>
        public static string ExePath => Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

        /// <summary>
        /// The brush to use for the background of highlighted elements.
        /// </summary>
        public static Brush HighlightBrush = new SolidBrush(Color.FromArgb(80, 0, 180, 255));

        /// <summary>
        /// The color to use for the outline for a selected element.
        /// </summary>
        public static Color SelectedColor = Color.DarkMagenta;

        /// <summary>
        /// The color to use for special manipulation type indications for a selected element.
        /// </summary>
        public static Color SelectedColorSpecial = Color.OrangeRed;
    }
}