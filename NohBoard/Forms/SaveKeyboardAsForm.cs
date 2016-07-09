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
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using Extra;

    /// <summary>
    /// The form used to save a keyboard under a new name.
    /// </summary>
    public partial class SaveKeyboardAsForm : Form
    {
        /// <summary>
        /// The name of the currently selected category.
        /// </summary>
        private string SelectedCategory => this.CategoryCombo.Text.SanitizeFilename();

        /// <summary>
        /// The name of the currently selected defintion.
        /// </summary>
        private string SelectedDefinition => this.DefinitionCombo.Text.SanitizeFilename();

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveKeyboardAsForm" /> class.
        /// </summary>
        public SaveKeyboardAsForm()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes the form, pre-filling the list of categories and definitions in the current category.
        /// </summary>
        private void SaveKeyboardAsForm_Load(object sender, EventArgs e)
        {
            var root = FileHelper.FromKbs();

            // If there are no keyboard files, no initialization is required.
            if (!root.Exists) return;

            this.CategoryCombo.Items.AddRange(root.EnumerateDirectories().Select(x => (object)x.Name).ToArray());

            this.CategoryCombo.Text = GlobalSettings.Settings.LoadedCategory;
            this.DefinitionCombo.Text = GlobalSettings.Settings.LoadedKeyboard;
        }

        /// <summary>
        /// Populates the list of keyboards for the specified category.
        /// </summary>
        /// <param name="category">The category to load the keyboards from.</param>
        private void PopulateKeyboards(string category)
        {
            var root = FileHelper.FromKbs(category);
            if (!root.Exists) return;

            this.DefinitionCombo.Items.Clear();
            this.DefinitionCombo.Items.AddRange(root.EnumerateDirectories().Select(x => (object)x.Name).ToArray());
        }

        /// <summary>
        /// Handles the change of the category combo selection change, re-populates keyboards.
        /// </summary>
        private void CategoryCombo_TextChanged(object sender, EventArgs e)
        {
            this.PopulateKeyboards(this.SelectedCategory);
        }

        /// <summary>
        /// Saves the keyboard definition in the specified location.
        /// </summary>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.SelectedCategory) || string.IsNullOrWhiteSpace(this.SelectedDefinition))
                return;

            // Check for a reserved name.
            if (string.Equals(
                this.SelectedCategory,
                Constants.GlobalStylesFolder,
                StringComparison.InvariantCultureIgnoreCase))
            {
                var result = MessageBox.Show(
                    $"{Constants.GlobalStylesFolder} cannot be used for a category name.",
                    "Invalid name",
                    MessageBoxButtons.OKCancel);

                if (result == DialogResult.OK)
                    return;

                this.Close();
                return;
            }

            // Check if the name already exists.
            if (FileHelper.FromKbs(this.SelectedCategory, this.SelectedDefinition).Exists)
            {
                var result = MessageBox.Show(
                    $"Keyboard {this.SelectedCategory}/{this.SelectedDefinition} already exists, " +
                    "do you want to overwrite it?",
                    "Already exists",
                    MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.No) return;
                if (result == DialogResult.Cancel)
                {
                    this.Close();
                    return;
                }
            }

            // Save the definition.
            GlobalSettings.CurrentDefinition.Category = this.SelectedCategory;
            GlobalSettings.CurrentDefinition.Name = this.SelectedDefinition;
            GlobalSettings.CurrentDefinition.Save();
            GlobalSettings.Settings.LoadedCategory = this.SelectedCategory;
            GlobalSettings.Settings.LoadedKeyboard = this.SelectedDefinition;
            this.Close();
        }
    }
}
