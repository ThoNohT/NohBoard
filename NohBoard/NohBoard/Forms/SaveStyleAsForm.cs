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
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using Extra;
    using Keyboard;

    /// <summary>
    /// The form used to save a style under a new name.
    /// </summary>
    public partial class SaveStyleAsForm : Form
    {
        /// <summary>
        /// The root path for the folder where styles should be saved.
        /// </summary>
        private string rootPath;

        /// <summary>
        /// The name of the currently selected style.
        /// </summary>
        private string SelectedStyle => this.StyleCombo.Text.SanitizeFilename();

        /// <summary>
        /// Indicates whether the style should be saved as a global style.
        /// </summary>
        private bool SelectedGlobal => this.chkGlobal.Checked;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveStyleAsForm" /> class.
        /// </summary>
        public SaveStyleAsForm()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes the form, decides whether or not global styles can be used and prefills the list of already
        /// known styles.
        /// </summary>
        private void SaveStyleAsForm_Load(object sender, System.EventArgs e)
        {
            // Determine whether we can save globally.
            this.chkGlobal.Enabled = GlobalSettings.CurrentStyle.IsGlobal;
            this.chkGlobal.Checked = GlobalSettings.Settings.LoadedGlobalStyle && GlobalSettings.CurrentStyle.IsGlobal;

            this.FillStyles();
        }

        /// <summary>
        /// Fills the list of known styles.
        /// </summary>
        private void FillStyles()
        {
            var sRoot = FileHelper.FromKbs().FullName;
            this.rootPath = this.chkGlobal.Checked
                ? Path.Combine(sRoot, Constants.GlobalStylesFolder)
                : Path.Combine(sRoot, GlobalSettings.CurrentDefinition.Category, GlobalSettings.CurrentDefinition.Name);
            this.StyleCombo.Items.Clear();

            var root = new DirectoryInfo(this.rootPath);

            // If there are no style files, no initialization is required.
            if (!root.Exists) return;

            this.StyleCombo.Items.AddRange(
                root.EnumerateFiles().Where(x => x.Extension == KeyboardStyle.StyleExtension)
                    .Select(x => (object)x.Name.Substring(0, x.Name.Length - KeyboardStyle.StyleExtension.Length))
                    .ToArray());

            this.StyleCombo.Text = GlobalSettings.Settings.LoadedStyle;
        }

        /// <summary>
        /// Handles the switch between global and definition specific styles. Re-fills the list of known styles.
        /// </summary>
        private void chkGlobal_CheckedChanged(object sender, System.EventArgs e)
        {
            this.FillStyles();
        }

        /// <summary>
        /// Saves the style to the chosen location and name.
        /// </summary>
        private void SaveButton_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.SelectedStyle))
                return;

            // Check if the name already exists.
            if (File.Exists(Path.Combine(this.rootPath, $"{this.SelectedStyle}{KeyboardStyle.StyleExtension}")))
            {
                var result = MessageBox.Show(
                    $"Style {this.SelectedStyle} already exists, do you want to overwrite it?",
                    "Already exists",
                    MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.No) return;
                if (result == DialogResult.Cancel)
                {
                    this.Close();
                    return;
                }
            }

            // Save the style.
            GlobalSettings.CurrentStyle.Name = this.SelectedStyle;
            GlobalSettings.CurrentStyle.Save(this.SelectedGlobal);
            GlobalSettings.Settings.LoadedStyle = this.SelectedStyle;
            GlobalSettings.Settings.LoadedGlobalStyle = this.SelectedGlobal;
            this.Close();
        }
    }
}
