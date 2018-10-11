﻿/*
Copyright (C) 2018 by Eric Bataille <e.c.p.bataille@gmail.com>

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
    using Extra;
    using Hooking;
    using Hooking.Interop;
    using Keyboard;
    using Keyboard.ElementDefinitions;
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

            // Reset all edit mode related fields, as we should be no longer in edit mode.
            this.undoHistory.Clear();
            this.undoHistoryStyle.Clear();
            this.redoHistory.Clear();
            this.redoHistoryStyle.Clear();

            if (this.mnuToggleEditMode.Checked)
            {
                this.mnuToggleEditMode.Checked = false;
                this.mnuToggleEditMode_Click(null, null);
            }

            this.currentlyManipulating = null;
            this.highlightedDefinition = null;
            this.selectedDefinition = null;

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
            this.menuOpen = false;

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
            this.menuOpen = false;

            GlobalSettings.CurrentDefinition.Save();
            GlobalSettings.Settings.LoadedCategory = GlobalSettings.CurrentDefinition.Category;
            GlobalSettings.Settings.LoadedKeyboard = GlobalSettings.CurrentDefinition.Name;
        }

        /// <summary>
        /// Opens a form the save the current definition under a custom name.
        /// </summary>
        private void mnuSaveDefinitionAs_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

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
        /// Handles the moving of the form. Stores the current position for future use.
        /// </summary>
        private void MainForm_Move(object sender, EventArgs e)
        {
            if (GlobalSettings.Settings != null && this.WindowState == FormWindowState.Normal)
            {
                GlobalSettings.Settings.X = this.Location.X;
                GlobalSettings.Settings.Y = this.Location.Y;
            }
        }

        /// <summary>
        /// Handles the closing of the form. Hooks are disabled and the settings are saved before closing.
        /// </summary>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            HookManager.DisableMouseHook();
            HookManager.DisableKeyboardHook();

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
            this.menuOpen = false;

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
            this.menuOpen = true;

            this.mnuSaveDefinition.Enabled = GlobalSettings.CurrentDefinition != null;
            if (GlobalSettings.CurrentDefinition != null)
            {
                this.mnuSaveDefinitionAsName.Text =
                    $"Save &To '{GlobalSettings.CurrentDefinition.Category}/{GlobalSettings.CurrentDefinition.Name}'";

                var mousePos = this.PointToClient(Cursor.Position);
                this.elementUnderCursor =
                    GlobalSettings.CurrentDefinition.Elements.FirstOrDefault(x => x.Inside(mousePos));

                // Set the highlighted definition only if we're in edit mode, and there is not an already selected definition.
                if (this.mnuToggleEditMode.Checked && this.selectedDefinition == null)
                {
                    this.highlightedDefinition = this.elementUnderCursor;
                    this.highlightedDefinition?.StartManipulating(mousePos, false);
                }

                var relevantElement = this.selectedDefinition ?? this.elementUnderCursor;
                this.mnuEditElementStyle.Enabled = relevantElement != null;
                this.mnuElementProperties.Enabled = relevantElement != null;
            }

            // Only allow editing of properties/styles in edit mode.
            this.mnuKeyboardProperties.Visible = this.mnuToggleEditMode.Checked;
            this.mnuUpdateTextPosition.Visible = this.mnuToggleEditMode.Checked;
            this.mnuElementProperties.Visible = this.mnuToggleEditMode.Checked;
            this.mnuEditKeyboardStyle.Visible = this.mnuToggleEditMode.Checked;
            this.mnuEditElementStyle.Visible = this.mnuToggleEditMode.Checked;
            this.MainMenuSep1.Visible = this.mnuToggleEditMode.Checked;

            this.mnuSaveStyleToName.Text = $"Save &To '{GlobalSettings.CurrentStyle.Name}'";
            this.mnuSaveStyleToName.Visible = !GlobalSettings.Settings.LoadedGlobalStyle;
            this.mnuSaveToGlobalStyleName.Text = $"Save To &Global '{GlobalSettings.CurrentStyle.Name}'";
            this.mnuSaveToGlobalStyleName.Enabled = GlobalSettings.CurrentStyle.IsGlobal;
            this.mnuSaveToGlobalStyleName.Visible = GlobalSettings.Settings.LoadedGlobalStyle;

            this.mnuToggleEditMode.Enabled = GlobalSettings.CurrentDefinition != null;

            if (this.latestVersion != null && !this.mnuUpdate.Visible)
            {
                this.mnuUpdate.Text = $"New version available: {this.latestVersion.Format()}.";
                this.mnuUpdate.Visible = true;
                this.mnuUpdate.Click += (s, ea) => { Process.Start("https://github.com/ThoNohT/NohBoard/releases"); };
            }

            this.mnuMoveElement.Visible = this.relevantDefinition != null;

            var highlightedSomething = this.mnuToggleEditMode.Checked && this.relevantDefinition != null;

            // Edit mode related menu items.
            this.mnuAddBoundaryPoint.Visible = highlightedSomething &&
                this.relevantDefinition.RelevantManipulation.Type == ElementManipulationType.MoveEdge;

            this.mnuRemoveBoundaryPoint.Visible = highlightedSomething &&
                this.relevantDefinition.RelevantManipulation.Type == ElementManipulationType.MoveBoundary;

            this.mnuRemoveElement.Visible = highlightedSomething;
            this.mnuAddElement.Visible = this.mnuToggleEditMode.Checked && this.relevantDefinition == null;
        }

        /// <summary>
        /// Handles setting the menu open variable to false when the form loses focus.
        /// </summary>
        private void MainForm_Deactivate(object sender, EventArgs e)
        {
            // Deactivating the form also closes the menu.
            this.menuOpen = false;
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
                Render(e.Graphics, def, allDefs, kbKeys, mouseKeys, scrollCounts, false);
            }

            // Draw the element being manipulated
            if (this.currentlyManipulating == null)
            {
                if (this.highlightedDefinition != this.selectedDefinition)
                    // Draw highlighted only if it is not also selected.
                    this.highlightedDefinition?.RenderHighlight(e.Graphics);

                if (this.selectedDefinition != null)
                {
                    Render(e.Graphics, this.selectedDefinition, allDefs, kbKeys, mouseKeys, scrollCounts, true);
                    this.selectedDefinition.RenderSelected(e.Graphics);
                }
            }
            else
            {
                this.currentlyManipulating.Item2.RenderEditing(e.Graphics);
            }

            base.OnPaint(e);
        }

        /// <summary>
        /// Renders a single element definition.
        /// </summary>
        /// <param name="g">The GDI+ surface to render on.</param>
        /// <param name="def">The element definition to render.</param>
        /// <param name="allDefs">The list of all element definition.</param>
        /// <param name="kbKeys">The list of keyboard keys that are pressed.</param>
        /// <param name="mouseKeys">The list of mouse keys that are pressed.</param>
        /// <param name="scrollCounts">The list of scroll counts.</param>
        /// <param name="scrollCounts">If <c>true</c>, the key will always render, regardless of whether it is
        /// different from the background.</param>
        private void Render(
            Graphics g,
            ElementDefinition def,
            List<ElementDefinition> allDefs,
            IReadOnlyList<int> kbKeys,
            List<int> mouseKeys,
            IReadOnlyList<int> scrollCounts,
            bool alwaysRender)
        {
            if (def is KeyboardKeyDefinition kkDef)
            {
                var pressed = true;
                if (!kkDef.KeyCodes.Any() || !kkDef.KeyCodes.All(kbKeys.Contains)) pressed = false;

                if (kkDef.KeyCodes.Count == 1
                    && allDefs.OfType<KeyboardKeyDefinition>()
                        .Any(d => d.KeyCodes.Count > 1
                        && d.KeyCodes.All(kbKeys.Contains)
                        && d.KeyCodes.ContainsAll(kkDef.KeyCodes))) pressed = false;

                if (!pressed && !alwaysRender) return;

                kkDef.Render(g, pressed, KeyboardState.ShiftDown, KeyboardState.CapsActive);
            }
            if (def is MouseKeyDefinition mkDef)
            {
                var pressed = mouseKeys.Contains(mkDef.KeyCodes.Single());
                if (pressed || alwaysRender)
                    mkDef.Render(g, pressed, KeyboardState.ShiftDown, KeyboardState.CapsActive);
            }
            if (def is MouseScrollDefinition msDef)
            {
                var scrollCount = scrollCounts[msDef.KeyCodes.Single()];
                if (scrollCount > 0 || alwaysRender) msDef.Render(g, scrollCount);
            }
            if (def is MouseSpeedIndicatorDefinition)
            {
                ((MouseSpeedIndicatorDefinition)def).Render(g, MouseState.AverageSpeed);
            }
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
    }
}