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
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using Extra;
    using ThoNohT.NohBoard.Keyboard;
    using ThoNohT.NohBoard.Legacy;

    /// <summary>
    /// The form that is used to load keyboards and styles.
    /// </summary>
    public partial class LoadKeyboardForm : Form
    {
        #region Fields

        /// <summary>
        /// The name of the currently selected category.
        /// </summary>
        private string SelectedCategory => (string)this.CategoryCombo.SelectedItem;

        /// <summary>
        /// The name of the currently selected definition.
        /// </summary>
        private string SelectedDefinition => (string)this.DefinitionsList.SelectedItem;

        /// <summary>
        /// Information about the currently selected style.
        /// </summary>
        private StyleInfo SelectedStyle => (StyleInfo)this.StyleList.SelectedItem;

        /// <summary>
        /// A value indicating whether the form has completed loading.
        /// </summary>
        private bool loaded = false;

        /// <summary>
        /// The list of global styles, this is constant regardless of the loaded styles for specific keyboards.
        /// </summary>
        private List<StyleInfo> globalStyles;

        /// <summary>
        /// A helper class with information about the currently selected style.
        /// </summary>
        private class StyleInfo
        {
            /// <summary>
            /// Indicates whether the style is global.
            /// </summary>
            public bool Global { get; set; }

            /// <summary>
            /// The name of the style.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Returns a string that represents the current object.
            /// </summary>
            /// <returns>A string that represents the current object.</returns>
            public override string ToString()
            {
                return this.Global ? $"global: {this.Name}" : this.Name;
            }
        }

        /// <summary>
        /// A helper class for filling the missing fonts table.
        /// </summary>
        private class MissingFont
        {
            /// <summary>
            /// The name of the font.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// The link to download the font.
            /// </summary>
            public string Link { get; set; }
        }

        #endregion Fields

        #region Events

        /// <summary>
        /// The event that is invoked when the style has been changed. Only invoked when the style is changed through
        /// the user interface, not when it is changed programmatically.
        /// </summary>
        public event Action<KeyboardDefinition, KeyboardStyle, bool> DefinitionChanged;

        #endregion Events

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadKeyboardForm" /> class.
        /// </summary>
        public LoadKeyboardForm()
        {
            this.InitializeComponent();
        }

        #endregion Constructors

        /// <summary>
        /// Shows or hides the font download panel depending on wether there are missing fonts in the currently loaded
        /// style.
        /// </summary>
        /// <param name="missingFonts"></param>
        public void ToggleFontsPanel(List<SerializableFont> missingFonts)
        {
            if (!missingFonts.Any())
            {
                // Hide the panel.
                this.Width = 422;
                this.fontsGrid.Enabled = false;
                this.btnRestart.Enabled = false;
            }
            else
            {
                this.Width = 1025;
                this.fontsGrid.Enabled = true;
                this.btnRestart.Enabled = true;

                var gridData = missingFonts
                    .Select(f => new MissingFont { Name = f.FontFamily, Link = f.DownloadUrl ?? "No download URL provided" })
                    .ToList();
                this.fontsGrid.DataSource = gridData;
                this.fontsGrid.Columns[0].Width = 140;
                this.fontsGrid.Columns[1].Width = 435;

                this.fontsGrid.Update();

            }
        }

        /// <summary>
        /// Loads a legacy keyboard definition. This immediately closes the form and loads it in the main form.
        /// </summary>
        private void LoadLegacyButton_Click(object sender, System.EventArgs e)
        {
            var dialog = new OpenFileDialog { Filter = "Legacy Keyboard Files|*.kb" };
            var result = dialog.ShowDialog();

            if (result == DialogResult.Cancel) return;

            this.DefinitionChanged?.Invoke(LegacyKbFileParser.Parse(dialog.FileName), null, false);
            this.Close();
        }

        /// <summary>
        /// Loads the keyboard form, filling the controls with the found categories, definitions and styles.
        /// </summary>
        private void LoadKeyboardForm_Load(object sender, System.EventArgs e)
        {
            // Load the list of global styles
            var globalStylesRoot = FileHelper.FromKbs(Constants.GlobalStylesFolder);

            if (!globalStylesRoot.Exists)
                globalStylesRoot.Create();

            this.globalStyles = globalStylesRoot.EnumerateFiles()
                .Where(x => x.Extension == KeyboardStyle.StyleExtension)
                .Select(
                    x => new StyleInfo
                    {
                        Global = true,
                        Name = x.Name.Substring(0, x.Name.Length - KeyboardStyle.StyleExtension.Length)
                    })
                .ToList();

            var root = FileHelper.FromKbs();

            // If there are no keyboard files, no initialization is required.
            if (!root.Exists) return;

            this.CategoryCombo.Items.Clear();
            this.CategoryCombo.Items.AddRange(
                root.EnumerateDirectories()
                    .Select(x => (object)x.Name).Where(x => (string)x != Constants.GlobalStylesFolder).ToArray());

            if (GlobalSettings.Settings.LoadedCategory != null)
            {
                this.CategoryCombo.SelectedItem = GlobalSettings.Settings.LoadedCategory;
                this.PopulateKeyboards();

                if (GlobalSettings.Settings.LoadedKeyboard != null)
                    this.DefinitionsList.SelectedItem = GlobalSettings.Settings.LoadedKeyboard;
            }
            this.loaded = true;
        }

        /// <summary>
        /// Handles a selection change in the category combo, re-populates the list of keyboards and styles.
        /// </summary>
        private void CategoryCombo_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.PopulateKeyboards();
        }

        /// <summary>
        /// Closes the form.
        /// </summary>
        private void CloseButton_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Deletes the selected keyboard definition and all of its styles. Re-initializes the form to clear the
        /// style from the lists.
        /// </summary>
        private void mnuDeleteDefinition_Click(object sender, System.EventArgs e)
        {
            if (this.SelectedDefinition == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete {this.SelectedCategory}/{this.SelectedDefinition}?",
                "Delete definition",
                MessageBoxButtons.YesNo);

            if (result != DialogResult.Yes)
                return;

            FileHelper.FromKbs(this.SelectedCategory, this.SelectedDefinition).Delete(true);
            this.LoadKeyboardForm_Load(null, null);
        }

        /// <summary>
        /// Handles a selection change in the definition list, re-populates the styles list for this definition.
        /// Loads the definition in the main form.
        /// </summary>
        private void DefinitionsList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                var kbDef = KeyboardDefinition.Load(this.SelectedCategory, this.SelectedDefinition);

                this.LoadStyles();
                if (this.StyleList.Items.Count == 0) this.DefinitionChanged?.Invoke(kbDef, null, false);
            } catch (Exception ex)
            {
                MessageBox.Show($"Failed to load keyboard {this.SelectedDefinition}: {ex.Message}");
                return;
            }
        }

        /// <summary>
        /// Handles a selection change in the styles list, loading that style in the main form.
        /// </summary>
        private void StyleList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.DefinitionChanged?.Invoke(
                    KeyboardDefinition.Load(this.SelectedCategory, this.SelectedDefinition),
                    KeyboardStyle.Load(this.SelectedStyle.Name, this.SelectedStyle.Global),
                    this.SelectedStyle.Global);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load keyboard {this.SelectedDefinition}: {ex.Message}");
                return;
            }
        }

        /// <summary>
        /// Handles a double click on the link cell. Will open the browser if it contains an URL.
        /// </summary>
        private void fontsGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var linkText = (string)this.fontsGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

            if (string.IsNullOrWhiteSpace(linkText)) return;
            if (!Uri.IsWellFormedUriString(linkText, UriKind.Absolute)) return;

            Process.Start(linkText);
        }

        #region Helpers

        /// <summary>
        /// Populates the list of keyboards, for the currently selected category.
        /// </summary>
        private void PopulateKeyboards()
        {
            var root = FileHelper.FromKbs(this.SelectedCategory);
            if (!root.Exists) return;

            this.DefinitionsList.Items.Clear();
            this.DefinitionsList.Items.AddRange(
                root.EnumerateDirectories()
                    .Where(x => File.Exists(Path.Combine(x.FullName, Constants.DefinitionFilename)))
                    .Select(x => (object)x.Name)
                    .ToArray());

            // If the form is still loading, don't set the selected index.
            if (this.DefinitionsList.Items.Count > 0 && this.loaded)
                this.DefinitionsList.SelectedIndex = 0;

            this.LoadStyles();
        }

        /// <summary>
        /// Populates the list of styles for the currently selected keyboard definition.
        /// </summary>
        private void LoadStyles()
        {
            if (this.SelectedDefinition == null)
                return;

            var specificStylesRoot = FileHelper.FromKbs(this.SelectedCategory, this.SelectedDefinition);

            var specificStyles = specificStylesRoot.EnumerateFiles()
                .Where(x => x.Extension == KeyboardStyle.StyleExtension)
                .Select(
                    x => new StyleInfo
                    {
                        Global = false,
                        Name = x.Name.Substring(0, x.Name.Length - KeyboardStyle.StyleExtension.Length)
                    })
                .ToList();

            this.StyleList.Items.Clear();
            this.StyleList.Items.AddRange(this.globalStyles.Cast<object>().ToArray());
            this.StyleList.Items.AddRange(specificStyles.Cast<object>().ToArray());

            // Try to retain the style.
            var loadedStyle = GlobalSettings.Settings.LoadedStyle;
            if (this.StyleList.Items.Count > 0) this.StyleList.SelectedIndex = 0;
            if (loadedStyle != null)
            {
                var styleIndex = this.FindStyleListIndex(loadedStyle);
                if (styleIndex != -1)
                {
                    this.StyleList.SelectedIndex = styleIndex;
                }
            }
        }

        /// <summary>
        /// Returns the index of an item in StyleList that has a given name.
        /// </summary>
        private int FindStyleListIndex(string styleName)
        {
            for (var i = 0; i < this.StyleList.Items.Count; i++)
            {
                var item = (StyleInfo)this.StyleList.Items[i];
                if (item.Name == styleName)
                {
                    return i;
                }
            }

            return -1;
        }

        #endregion Helpers

        /// <summary>
        /// Restarts the application.
        /// </summary>
        private void btnRestart_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}
