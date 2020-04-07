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
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using Hooking;
    using Keyboard;
    using static Hooking.Interop.Defines;

    /// <summary>
    /// Contains global settings for NohBoard.
    /// </summary>
    [DataContract(Name = "GlobalSettings", Namespace = "")]
    public partial class GlobalSettings
    {
        /// <summary>
        /// Field for <see cref="UpdateInterval"/>.
        /// </summary>
        private int? updateInterval;

        /// <summary>
        /// Indicates whether there were changes made to the definition since the last save or load action.
        /// Changes are tracked when undo history is pushed, and reset when a keyboard is loaded or saved.
        /// </summary>
        public static bool UnsavedDefinitionChanges { get; set; }

        /// <summary>
        /// Indicates whether there were changes made to the style since the last save or load action.
        /// Changes are tracked when undo history is pushed, and reset when a style is loaded or saved.
        /// </summary>
        public static bool UnsavedStyleChanges { get; set; }

        /// <summary>
        /// Retrieves the global settings.
        /// </summary>
        public static GlobalSettings Settings { get; private set; }

        /// <summary>
        /// The currently loaded keyboard definition.
        /// </summary>
        public static KeyboardDefinition CurrentDefinition { get; private set; }

        /// <summary>
        /// The currently loaded keyboard style.
        /// </summary>
        public static KeyboardStyle CurrentStyle { get; private set; } = new KeyboardStyle();

        /// <summary>
        /// Any errors while loading settings.
        /// </summary>
        public static string Errors { get; set; }

        /// <summary>
        /// A dependency counter for the current style. This tells render functions to grab new brushes if it has
        /// changed.
        /// </summary>
        public static int StyleDependencyCounter { get; set; } = 0;

        #region General

        /// <summary>
        /// The window title. If empty, NohBoard with the version number will be shown.
        /// </summary>
        [DataMember]
        public string WindowTitle { get; set; } = "";

        #endregion General

        #region Input

        /// <summary>
        /// The mouse sensitivity.
        /// </summary>
        [DataMember]
        public int MouseSensitivity { get; set; } = 50;

        /// <summary>
        /// The time in millisecond to hold the scroll counter before resetting it.
        /// </summary>
        [DataMember]
        public int ScrollHold { get; set; } = 50;

        /// <summary>
        /// Whether the mouse speed is determined from the center of the screen, or from its previous position.
        /// This can be used for games that keep resetting the cursor position to the center.
        /// </summary>
        [DataMember]
        public bool MouseFromCenter { get; set; }

        /// <summary>
        /// The time in milliseconds for which to at least show key presses. Presses are still shown as long as they are
        /// actually pressed. But if they are pressed for shorter than this duration, they will be held active longer.
        /// </summary>
        [DataMember]
        public int PressHold { get; set; }

        /// <summary>
        /// The time in milliseconds between which the keyboard is updated and rendered again.
        /// Minimum: 5ms (200fps), maximum: 60s, default: 33ms (30fps).
        /// </summary>
        [DataMember]
        public int UpdateInterval
        {
            get => this.updateInterval ?? 33;
            set => this.updateInterval = Math.Max(5, Math.Min(60000, value));
        }

        #endregion Input

        #region Trapping

        /// <summary>
        /// Indicates whether to trap the keyboard input.
        /// </summary>
        [DataMember]
        public bool TrapKeyboard { get; set; }

        /// <summary>
        /// Indicates whether to trap the mouse input.
        /// </summary>
        [DataMember]
        public bool TrapMouse { get; set; }

        /// <summary>
        /// The key code of the key that  toggles the traps.
        /// </summary>
        [DataMember]
        public int TrapToggleKeyCode { get; set; } = VK_SCROLL;

        #endregion Trapping

        #region Capitalization

        /// <summary>
        /// The method used for capitalizing keys that have different shift-text than normal text.
        /// </summary>
        [DataMember]
        public CapitalizationMethod Capitalization { get; set; } = CapitalizationMethod.FollowKeys;

        /// <summary>
        /// If <see cref="Capitalization"/> is not <see cref="CapitalizationMethod.FollowKeys"/>, then setting this
        /// property to <c>true</c> means that the keys that are insensitive to Caps Lock will still follow the shift
        /// key.
        /// </summary>
        [DataMember]
        public bool FollowShiftForCapsInsensitive { get; set; }

        /// <summary>
        /// If <see cref="Capitalization"/> is not <see cref="CapitalizationMethod.FollowKeys"/>, then setting this
        /// property to <c>true</c> means that the keys that are sensitive to Caps Lock will still follow the shift
        /// key.
        /// </summary>
        [DataMember]
        public bool FollowShiftForCapsSensitive { get; set; }

        #endregion Capitalization

        #region State

        /// <summary>
        /// The category of the currently loaded keyboard file.
        /// </summary>
        [DataMember]
        public string LoadedCategory { get; set; }

        /// <summary>
        /// The name of the currently loaded keyboard file.
        /// </summary>
        [DataMember]
        public string LoadedKeyboard { get; set; }

        /// <summary>
        /// The name of the style that is currently loaded.
        /// </summary>
        [DataMember]
        public string LoadedStyle { get; set; }

        /// <summary>
        /// Indicates whether the currently loaded style is loaded from the global folder or the
        /// keyboard definition folder.
        /// </summary>
        [DataMember]
        public bool LoadedGlobalStyle { get; set; }

        /// <summary>
        /// The X position of the NohBoard window.
        /// </summary>
        [DataMember]
        public int X { get; set; } = 25;

        /// <summary>
        /// The Y position of the NohBoard window.
        /// </summary>
        [DataMember]
        public int Y { get; set; } = 25;

        #endregion State

        #region Editing

        /// <summary>
        /// Whether to update the text position of an element when updating boundaries or edges.
        /// </summary>
        [DataMember]
        public bool UpdateTextPosition = true;

        #endregion Editing

        #region Methods

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public static void Save()
        {
            FileHelper.Serialize(Constants.SettingsFilename, Settings);
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public static bool Load()
        {
            // Only load if the file exists.
            if (!File.Exists(Constants.SettingsFilename))
            {
                Settings = new GlobalSettings();
                return true;
            }

            try
            {
                Settings = FileHelper.Deserialize<GlobalSettings>(Constants.SettingsFilename);
                MouseState.SetMouseFromCenter(Settings.MouseFromCenter);
                return true;
            }
            catch (Exception ex)
            {
                Errors = ex.Message;
                Settings = new GlobalSettings();
                return false;
            }
        }

        #endregion Methods
    }
}