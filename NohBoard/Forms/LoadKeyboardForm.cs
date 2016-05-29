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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using ThoNohT.NohBoard.Extra;
    using ThoNohT.NohBoard.Keyboard;
    using ThoNohT.NohBoard.Legacy;

    public partial class LoadKeyboardForm : Form
    {
        private class StyleInfo
        {
            public bool Global { get; set; }

            public string Name { get; set; }

            public override string ToString()
            {
                return this.Global ? $"global: {this.Name}" : this.Name;
            }
        }

        /// <summary>
        /// The calling parent form.
        /// </summary>
        private readonly MainForm parent;

        /// <summary>
        /// The currently selected definition. This definition is only used when selecting a legacy kb file.
        /// </summary>
        public KeyboardDefinition LoadedLegacyDefinition { get; private set; }

        private string SelectedCategory => (string)this.CategoryCombo.SelectedItem;

        private string SelectedDefinition => (string)this.DefinitionsList.SelectedItem;

        private StyleInfo SelectedStyle => (StyleInfo)this.StyleList.SelectedItem;

        private bool loaded = false;

        private List<StyleInfo> globalStyles;

        public LoadKeyboardForm(MainForm parent)
        {
            this.parent = parent;
            this.InitializeComponent();
        }

        private void LoadLegacyButton_Click(object sender, System.EventArgs e)
        {
            var dialog = new OpenFileDialog { Filter = "Legacy Keyboard Files|*.kb" };

            var result = dialog.ShowDialog();

            if (result == DialogResult.Cancel)
                return;

            this.LoadedLegacyDefinition = LegacyKbFileParser.Parse(dialog.FileName);
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void LoadKeyboardForm_Load(object sender, System.EventArgs e)
        {
            // Load the list of global styles
            var globalStylesRoot = new DirectoryInfo(
                Path.Combine(Constants.ExePath, Constants.KeyboardsFolder, Constants.GlobalStylesFolder));

            this.globalStyles = globalStylesRoot.EnumerateFiles()
                .Where(x => x.Extension == KeyboardStyle.styleExtension)
                .Select(
                    x => new StyleInfo
                    {
                        Global = true,
                        Name = x.Name.Substring(0, x.Name.Length - KeyboardStyle.styleExtension.Length)
                    })
                .ToList();

            var root = new DirectoryInfo(Path.Combine(Constants.ExePath, Constants.KeyboardsFolder));
            
            // If there are no keyboard files, no initialization is required.
            if (!root.Exists) return;

            this.CategoryCombo.Items.Clear();
            this.CategoryCombo.Items.AddRange(
                root.EnumerateDirectories()
                    .Select(x => (object)x.Name).Where(x => (string)x != Constants.GlobalStylesFolder).ToArray());

            if (GlobalSettings.Settings.LoadedCategory != null)
            {
                this.CategoryCombo.SelectedItem = GlobalSettings.Settings.LoadedCategory;
                this.PopulateKeyboards(this.SelectedCategory);

                if (GlobalSettings.Settings.LoadedKeyboard != null)
                    this.DefinitionsList.SelectedItem = GlobalSettings.Settings.LoadedKeyboard;
            }
            this.loaded = true;
        }

        private void PopulateKeyboards(string category)
        {
            var root = new DirectoryInfo(Path.Combine(Constants.ExePath, Constants.KeyboardsFolder, category));
            if (!root.Exists) return;

            this.DefinitionsList.Items.Clear();
            this.DefinitionsList.Items.AddRange(
                root.EnumerateDirectories()
                    .Where(x => File.Exists(Path.Combine(x.FullName, Constants.DefinitionFilename)))
                    .Select(x => (object)x.Name)
                    .ToArray());

            if (this.DefinitionsList.Items.Count > 0 && this.loaded)
                this.DefinitionsList.SelectedIndex = 0;

            this.LoadStyles();
        }

        private void LoadStyles()
        {
            if (this.SelectedDefinition == null)
                return;

            var specificStylesRoot = new DirectoryInfo(
                Path.Combine(
                    Constants.ExePath,
                    Constants.KeyboardsFolder,
                    this.SelectedCategory,
                    this.SelectedDefinition));

            var specificStyles = specificStylesRoot.EnumerateFiles()
                .Where(x => x.Extension == KeyboardStyle.styleExtension)
                .Select(
                    x => new StyleInfo
                    {
                        Global = false,
                        Name = x.Name.Substring(0, x.Name.Length - KeyboardStyle.styleExtension.Length)
                    })
                .ToList();

            this.StyleList.Items.Clear();
            this.StyleList.Items.AddRange(this.globalStyles.Cast<object>().ToArray());
            this.StyleList.Items.AddRange(specificStyles.Cast<object>().ToArray());
        }

        private void CategoryCombo_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.PopulateKeyboards((string)this.CategoryCombo.SelectedItem);
        }

        private void CloseButton_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void mnuDeleteDefinition_Click(object sender, System.EventArgs e)
        {
            if (this.SelectedDefinition == null)
                return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete {this.SelectedCategory}/{this.SelectedDefinition}?",
                "Delete definition",
                MessageBoxButtons.YesNo);

            if (result != DialogResult.Yes)
                return;

            new DirectoryInfo(
                Path.Combine(
                    Constants.ExePath,
                    Constants.KeyboardsFolder,
                    this.SelectedCategory,
                    this.SelectedDefinition)).Delete(true);
            this.LoadKeyboardForm_Load(null, null);
        }

        private void DefinitionsList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.LoadStyles();
            if (this.StyleList.Items.Count > 0) this.StyleList.SelectedIndex = 0;
        }

        private void StyleList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var kbDef = KeyboardDefinition.Load(this.SelectedCategory, this.SelectedDefinition);
            var kbStyle = KeyboardStyle.Load(this.SelectedStyle.Name, this.SelectedStyle.Global);

            kbDef.Category = this.SelectedCategory;
            kbDef.Name = this.SelectedDefinition;

            kbStyle.Name = this.SelectedStyle.Name;

            GlobalSettings.Settings.LoadedCategory = kbDef.Category;
            GlobalSettings.Settings.LoadedKeyboard = kbDef.Name;
            GlobalSettings.Settings.LoadedStyle = this.SelectedStyle.Name;
            GlobalSettings.Settings.LoadedGlobalStyle = this.SelectedStyle.Global;


            this.parent.LoadNewKeyboard(kbDef, kbStyle);
        }
    }
}
