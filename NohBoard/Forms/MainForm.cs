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

namespace ThoNohT.NohBoard.Forms
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Net.Http;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Xml;
    using Extra;
    using Hooking;
    using Hooking.Interop;
    using Keyboard;
    using Keyboard.ElementDefinitions;
    using Keyboard.Styles;
    using Version = NohBoard.Version;

    /// <summary>
    /// The main form.
    /// </summary>
    public partial class MainForm : Form
    {
        #region Fields

        /// <summary>
        /// The back-brushes used for efficient drawing.
        /// </summary>
        private readonly Dictionary<bool, Dictionary<bool, Brush>> backBrushes =
            new Dictionary<bool, Dictionary<bool, Brush>>();

        /// <summary>
        /// The element currently under the cursor.
        /// </summary>
        private ElementDefinition elementUnderCursor = null;

        /// <summary>
        /// The latest version, if it was retrieved from the update site.
        /// </summary>
        private VersionInfo latestVersion = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm" /> class.
        /// </summary>
        public MainForm()
        {
            this.InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        #endregion Constructors

        #region Version check

        /// <summary>
        /// Attempts to retrieve the latest version from the update site.
        /// </summary>
        public Task GetLatestVersion()
        {
            return new Task(
                () =>
                {
                    var updateUrl =
                        "https://gist.githubusercontent.com/ThoNohT/3181561f8148fb6b865f88714e975154/raw/nohboard_version.json";

                    Func<string, VersionInfo> downloadVersionInfo = url =>
                    {
                        var serializer = new DataContractJsonSerializer(typeof(VersionInfo));
                        using (var client = new HttpClient())
                        using (var reader = JsonReaderWriterFactory.CreateJsonReader(
                            client.GetStreamAsync(updateUrl).Result,
                            Encoding.UTF8,
                            XmlDictionaryReaderQuotas.Max,
                            dictionaryReader => { }))
                        {
                            return (VersionInfo)serializer.ReadObject(reader);
                        }
                    };

                    var versionInfo = downloadVersionInfo(updateUrl);

                    if ((versionInfo.Major > Version.Major) ||
                        (versionInfo.Major == Version.Major && versionInfo.Minor > Version.Minor) ||
                        (versionInfo.Major == Version.Major && versionInfo.Minor == Version.Minor
                         && versionInfo.Patch > Version.Patch))
                    {
                        this.latestVersion = versionInfo;
                    }
                });
        }

        #endregion Version check

        #region Keyboard loading and saving

        /// <summary>
        /// Loads the keyboard currently defined in the settings.
        /// </summary>
        private void LoadKeyboard()
        {
            if (GlobalSettings.CurrentDefinition == null) return;

            this.ClientSize = new Size(GlobalSettings.CurrentDefinition.Width, GlobalSettings.CurrentDefinition.Height);

            this.ResetBackBrushes();
        }

        /// <summary>
        /// Redraws the back-brushes. These back-brushes are drawn for all possible states of the keys when none of them
        /// are pressed. This prevents having to render each of these keys every time. However, every time anything
        /// about the definition or style changes, the back-brushes have to be re-rendered.
        /// </summary>
        private void ResetBackBrushes()
        {
            GlobalSettings.StyleDependencyCounter++;

            foreach (var brush in this.backBrushes)
            {
                foreach (var b in brush.Value)
                    b.Value.Dispose();

                brush.Value.Clear();
            }
            this.backBrushes.Clear();

            // Fill the back-brushes.
            foreach (var shift in new[] { false, true })
            {
                this.backBrushes.Add(shift, new Dictionary<bool, Brush>());

                foreach (var caps in new[] { false, true })
                {
                    var bmp = new Bitmap(
                        GlobalSettings.CurrentDefinition.Width,
                        GlobalSettings.CurrentDefinition.Height);
                    var g = Graphics.FromImage(bmp);

                    // Render the background image if set.
                    var cs = GlobalSettings.CurrentStyle;
                    if (cs.BackgroundImageFileName != null && FileHelper.StyleImageExists(cs.BackgroundImageFileName))
                    {
                        g.DrawImage(ImageCache.Get(cs.BackgroundImageFileName), this.ClientRectangle);
                    }

                    // Render the individual keys.
                    foreach (var def in GlobalSettings.CurrentDefinition.Elements)
                    {
                        if (def is KeyboardKeyDefinition) ((KeyboardKeyDefinition)def).Render(g, false, shift, caps);

                        if (def is MouseKeyDefinition) ((MouseKeyDefinition)def).Render(g, false, shift, caps);

                        if (def is MouseScrollDefinition) ((MouseScrollDefinition)def).Render(g, 0);

                        // No need to render mouse speed indicators in backbrush.
                    }

                    this.backBrushes[shift].Add(caps, new TextureBrush(bmp));
                }
            }
        }

        /// <summary>
        /// Opens the load keyboard form.
        /// </summary>
        private void mnuLoadKeyboard_Click(object sender, EventArgs e)
        {
            using (var manageForm = new LoadKeyboardForm())
            {
                manageForm.DefinitionChanged += (kbDef, kbStyle, globalStyle) =>
                {
                    var backupDef = GlobalSettings.CurrentDefinition;
                    var backupStyle = GlobalSettings.CurrentStyle;

                    var backupCat = GlobalSettings.Settings.LoadedCategory;
                    var backupKb = GlobalSettings.Settings.LoadedKeyboard;
                    var backupKbStyle = GlobalSettings.Settings.LoadedStyle;
                    var backupkbGlobalStyle = GlobalSettings.Settings.LoadedGlobalStyle;
                    
                    GlobalSettings.CurrentDefinition = kbDef;
                    GlobalSettings.CurrentStyle = kbStyle ?? new KeyboardStyle();

                    GlobalSettings.Settings.LoadedCategory = kbDef.Category;
                    GlobalSettings.Settings.LoadedKeyboard = kbDef.Name;
                    GlobalSettings.Settings.LoadedStyle = kbStyle?.Name;
                    GlobalSettings.Settings.LoadedGlobalStyle = globalStyle;

                    try
                    {
                        this.LoadKeyboard();
                    }
                    catch (Exception ex)
                    {
                        GlobalSettings.CurrentDefinition = backupDef;
                        GlobalSettings.CurrentStyle = backupStyle;

                        GlobalSettings.Settings.LoadedCategory = backupCat;
                        GlobalSettings.Settings.LoadedKeyboard = backupKb;
                        GlobalSettings.Settings.LoadedStyle = backupKbStyle;
                        GlobalSettings.Settings.LoadedGlobalStyle = backupkbGlobalStyle;

                        this.LoadKeyboard();

                        MessageBox.Show(ex.Message + Environment.NewLine + "Reverted keyboard change.");
                    }
                };

                manageForm.ShowDialog(this);
            }
        }

        /// <summary>
        /// Saves the current definition under its default name.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuSaveDefinitionAsName_Click(object sender, EventArgs e)
        {
            GlobalSettings.CurrentDefinition.Save();
            GlobalSettings.Settings.LoadedCategory = GlobalSettings.CurrentDefinition.Category;
            GlobalSettings.Settings.LoadedKeyboard = GlobalSettings.CurrentDefinition.Name;
        }

        /// <summary>
        /// Opens a form the save the current definition under a custom name.
        /// </summary>
        private void mnuSaveDefinitionAs_Click(object sender, EventArgs e)
        {
            using (var saveForm = new SaveKeyboardAsForm())
            {
                saveForm.ShowDialog(this);
            }
        }

        #endregion Keyboard loading and saving

        #region Settings

        /// <summary>
        /// Handles the loading of the form, all settings are read, hooks are created and the keyboard is initialized.
        /// </summary>
        private void MainForm_Load(object sender, EventArgs e)
        {
            // Load the settings
            if (!GlobalSettings.Load())
            {
                MessageBox.Show(
                    this,
                    $"Failed to load the settings: {GlobalSettings.Errors}",
                    "Failed to load settings");
            }

            this.Location = new Point(GlobalSettings.Settings.X, GlobalSettings.Settings.Y);
            this.Text = $"NohBoard {Version.Get}";

            this.GetLatestVersion().Start();

            // Load a definition if possible.
            if (GlobalSettings.Settings.LoadedKeyboard != null && GlobalSettings.Settings.LoadedCategory != null)
            {
                try
                {
                    GlobalSettings.CurrentDefinition = KeyboardDefinition
                        .Load(GlobalSettings.Settings.LoadedCategory, GlobalSettings.Settings.LoadedKeyboard);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "There was an error loading the saved keyboard definition file:" +
                        $"{Environment.NewLine}{ex.Message}");
                    GlobalSettings.Settings.LoadedCategory = null;
                    GlobalSettings.Settings.LoadedKeyboard = null;
                }
            }

            // Load a style if possible.
            if (GlobalSettings.CurrentDefinition != null && GlobalSettings.Settings.LoadedStyle != null)
            {
                try
                {
                    GlobalSettings.CurrentStyle = KeyboardStyle.Load(
                        GlobalSettings.Settings.LoadedStyle,
                        GlobalSettings.Settings.LoadedGlobalStyle);
                    this.ResetBackBrushes();
                }
                catch
                {
                    GlobalSettings.Settings.LoadedStyle = null;
                    MessageBox.Show(
                        $"Failed to load style {GlobalSettings.Settings.LoadedStyle}, loading default style.",
                        "Error loading style.");
                }
            }

            HookManager.EnableMouseHook();
            HookManager.EnableKeyboardHook();

            this.UpdateTimer.Enabled = true;
            this.KeyCheckTimer.Enabled = true;

            this.Activate();
            this.ApplySettings();
        }

        /// <summary>
        /// Handles the closing of the form. Hooks are disabled and the current position is stored before closing.
        /// </summary>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            HookManager.DisableMouseHook();
            HookManager.DisableKeyboardHook();

            GlobalSettings.Settings.X = this.Location.X;
            GlobalSettings.Settings.Y = this.Location.Y;

            GlobalSettings.Save();
        }

        /// <summary>
        /// Applies the currently stored settings.
        /// </summary>
        private void ApplySettings()
        {
            HookManager.TrapKeyboard = GlobalSettings.Settings.TrapKeyboard;
            HookManager.TrapMouse = GlobalSettings.Settings.TrapMouse;
            HookManager.TrapToggleKeyCode = GlobalSettings.Settings.TrapToggleKeyCode;
            HookManager.ScrollHold = GlobalSettings.Settings.ScrollHold;
            this.LoadKeyboard();
        }

        /// <summary>
        /// Opens the settings form.
        /// </summary>
        private void mnuSettings_Click(object sender, EventArgs e)
        {
            using (var settingsForm = new SettingsForm())
            {
                var result = settingsForm.ShowDialog(this);

                if (result == DialogResult.Cancel)
                    return;

                // Re-initialize with the new settings.
                this.ApplySettings();
            }
        }

        /// <summary>
        /// Populates the main menu. Elements are visible based on which definitions and styles are loaded, and whether
        /// actions on a specifically pointed element are possible.
        /// </summary>
        private void MainMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.mnuSaveDefinition.Enabled = GlobalSettings.CurrentDefinition != null;
            if (GlobalSettings.CurrentDefinition != null)
            {
                this.mnuSaveDefinitionAsName.Text =
                    $"Save &To '{GlobalSettings.CurrentDefinition.Category}/{GlobalSettings.CurrentDefinition.Name}'";

                var mousePos = this.PointToClient(Cursor.Position);
                this.elementUnderCursor =
                    GlobalSettings.CurrentDefinition.Elements.FirstOrDefault(x => x.Inside(mousePos));
                this.mnuEditElementStyle.Enabled = this.elementUnderCursor != null;
            }

            this.mnuEditKeyboardStyle.Enabled = GlobalSettings.CurrentDefinition != null;

            this.mnuSaveStyleToName.Text = $"Save &To '{GlobalSettings.CurrentStyle.Name}'";
            this.mnuSaveStyleToName.Visible = !GlobalSettings.Settings.LoadedGlobalStyle;
            this.mnuSaveToGlobalStyleName.Text = $"Save To &Global '{GlobalSettings.CurrentStyle.Name}'";
            this.mnuSaveToGlobalStyleName.Enabled = GlobalSettings.CurrentStyle.IsGlobal;
            this.mnuSaveToGlobalStyleName.Visible = GlobalSettings.Settings.LoadedGlobalStyle;

            this.mnuToggleEditMode.Enabled = GlobalSettings.CurrentDefinition != null;

            if (this.latestVersion != null && !this.mnuUpdate.Visible)
            {
                // TODO: Change to visibility based on new version, current strategy will add an item every time the menu is opened.
                this.mnuUpdate.Text = $"New version available: {this.latestVersion.Format()}.";
                this.mnuUpdate.Visible = true;
                this.mnuUpdate.Click += (s, ea) => { Process.Start("https://github.com/ThoNohT/NohBoard/releases"); };
            }
        }

        /// <summary>
        /// Exits the application.
        /// </summary>
        private void mnuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion Settings

        #region Rendering

        /// <summary>
        /// Paints the keyboard on the screen.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(GlobalSettings.CurrentStyle.BackgroundColor);

            if (GlobalSettings.CurrentDefinition == null || !this.backBrushes.Any())
                return;

            // Fill the appropriate back brush.
            e.Graphics.FillRectangle(
                this.backBrushes[KeyboardState.ShiftDown][KeyboardState.CapsActive],
                new Rectangle(0, 0, GlobalSettings.CurrentDefinition.Width, GlobalSettings.CurrentDefinition.Height));

            // Render all keys.
            var kbKeys = KeyboardState.PressedKeys;
            var mouseKeys = MouseState.PressedKeys.Select(k => (int)k).ToList();
            MouseState.CheckScrollAndMovement();
            var scrollCounts = MouseState.ScrollCounts;
            var allDefs = GlobalSettings.CurrentDefinition.Elements;
            foreach (var def in allDefs)
            {
                if (def is KeyboardKeyDefinition)
                {
                    var kkDef = (KeyboardKeyDefinition)def;
                    if (!kkDef.KeyCodes.All(kbKeys.Contains)) continue;

                    if (kkDef.KeyCodes.Count == 1
                        && allDefs.OfType<KeyboardKeyDefinition>().Any(
                            d => d.KeyCodes.Count > 1
                                 && d.KeyCodes.All(kbKeys.Contains)
                                 && d.KeyCodes.ContainsAll(kkDef.KeyCodes))) continue;

                    kkDef.Render(e.Graphics, true, KeyboardState.ShiftDown, KeyboardState.CapsActive);
                }
                if (def is MouseKeyDefinition)
                {
                    var mkDef = (MouseKeyDefinition)def;
                    if (mouseKeys.Contains(mkDef.KeyCodes.Single()))
                        mkDef.Render(e.Graphics, true, KeyboardState.ShiftDown, KeyboardState.CapsActive);
                }
                if (def is MouseScrollDefinition)
                {
                    var msDef = (MouseScrollDefinition)def;
                    var scrollCount = scrollCounts[msDef.KeyCodes.Single()];
                    if (scrollCount > 0) msDef.Render(e.Graphics, scrollCount);
                }
                if (def is MouseSpeedIndicatorDefinition)
                {
                    ((MouseSpeedIndicatorDefinition)def).Render(e.Graphics, MouseState.AverageSpeed);
                }
            }

            // Draw the element being manipulated
            this.currentlyManipulating?.RenderEditing(e.Graphics);
            this.HighlightedDefinition?.RenderHighlight(e.Graphics, this.currentManipulationPoint);

            base.OnPaint(e);
        }

        /// <summary>
        /// Forces an update if any of the key or mouse states have changed.
        /// </summary>
        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (KeyboardState.Updated || MouseState.Updated)
                this.Refresh();
        }

        /// <summary>
        /// Periodically checks that no keys got stuck.
        /// </summary>
        private void KeyCheckTimer_Tick(object sender, EventArgs e)
        {
            MouseState.CheckKeys();
            KeyboardState.CheckKeys();
        }

        #endregion Rendering

        #region Styles

        /// <summary>
        /// Opens the edit element style form for the element currently under the cursor.
        /// </summary>
        private void mnuEditElementStyle_Click(object sender, EventArgs e)
        {
            if (GlobalSettings.Settings.LoadedStyle == null)
            {
                MessageBox.Show("Please load or save a style before editing element styles.");
                return;
            }
            
            // Sanity check, don't try anything if there's no selected element.
            if (this.elementUnderCursor == null) return;
            var id = this.elementUnderCursor.Id;

            if (this.elementUnderCursor is KeyDefinition)
            {
                using (var styleForm = new KeyStyleForm(
                    GlobalSettings.CurrentStyle.TryGetElementStyle<KeyStyle>(id),
                    GlobalSettings.CurrentStyle.DefaultKeyStyle))
                {
                    styleForm.StyleChanged += style =>
                    {
                        if (style.Loose == null && style.Pressed == null
                            && GlobalSettings.CurrentStyle.ElementStyles.ContainsKey(id))
                            GlobalSettings.CurrentStyle.ElementStyles.Remove(id);

                        if (!GlobalSettings.CurrentStyle.ElementStyles.ContainsKey(id))
                            GlobalSettings.CurrentStyle.ElementStyles.Add(id, style);
                        else
                            GlobalSettings.CurrentStyle.ElementStyles[id] = style;

                        this.ResetBackBrushes();
                    };

                    styleForm.ShowDialog(this);
                }
            }

            if (this.elementUnderCursor is MouseSpeedIndicatorDefinition)
            {
                using (var styleForm = new MouseSpeedStyleForm(
                        GlobalSettings.CurrentStyle.TryGetElementStyle<MouseSpeedIndicatorStyle>(id),
                    GlobalSettings.CurrentStyle.DefaultMouseSpeedIndicatorStyle))
                {
                    styleForm.StyleChanged += style =>
                    {
                        if (style == null && GlobalSettings.CurrentStyle.ElementStyles.ContainsKey(id))
                            GlobalSettings.CurrentStyle.ElementStyles.Remove(id);

                        if (!GlobalSettings.CurrentStyle.ElementStyles.ContainsKey(id))
                            GlobalSettings.CurrentStyle.ElementStyles.Add(id, style);
                        else
                            GlobalSettings.CurrentStyle.ElementStyles[id] = style;

                        this.ResetBackBrushes();
                    };

                    styleForm.ShowDialog(this);
                }
            }
        }

        /// <summary>
        /// Opens the edit keyboard style form.
        /// </summary>
        private void mnuEditKeyboardStyle_Click(object sender, EventArgs e)
        {
            if (GlobalSettings.Settings.LoadedStyle == null)
            {
                MessageBox.Show("Please load or save a style before editing the keyboard style.");
                return;
            }

            using (var styleForm = new KeyboardStyleForm(GlobalSettings.CurrentStyle))
            {
                styleForm.StyleChanged += style =>
                {
                    GlobalSettings.CurrentStyle = style;
                    this.ResetBackBrushes();
                };
                styleForm.ShowDialog(this);
            }
        }

        /// <summary>
        /// Saves the current style to its default name.
        /// </summary>
        private void mnuSaveStyleToName_Click(object sender, EventArgs e)
        {
            GlobalSettings.CurrentStyle.Save(false);
            GlobalSettings.Settings.LoadedStyle = GlobalSettings.CurrentStyle.Name;
            GlobalSettings.Settings.LoadedGlobalStyle = false;
        }

        /// <summary>
        /// Saves the current style as a global style to its default name.
        /// </summary>
        private void mnuSaveToGlobalStyleName_Click(object sender, EventArgs e)
        {
            GlobalSettings.CurrentStyle.Save(true);
            GlobalSettings.Settings.LoadedStyle = GlobalSettings.CurrentStyle.Name;
            GlobalSettings.Settings.LoadedGlobalStyle = true;
        }

        /// <summary>
        /// Opens the save style as form to save the style under a custom name.
        /// </summary>
        private void mnuSaveStyleAs_Click(object sender, EventArgs e)
        {
            using (var saveForm = new SaveStyleAsForm())
            {
                saveForm.ShowDialog(this);
                saveForm.Dispose();
            }
        }

        #endregion Styles
    }
}