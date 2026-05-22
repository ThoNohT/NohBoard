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
    using Keyboard;

    public partial class SaveStyleAsForm : Form
    {
        private string rootPath;

        private string SelectedStyle => this.StyleCombo.Text.SanitizeFilename();

        private bool SelectedGlobal => this.chkGlobal.Checked;

        public SaveStyleAsForm()
        {
            this.InitializeComponent();
        }

        private void SaveStyleAsForm_Load(object sender, System.EventArgs e)
        {
            this.ApplyLocalizedSaveStyleTexts();

            this.chkGlobal.Enabled = GlobalSettings.CurrentStyle.IsGlobal;
            this.chkGlobal.Checked = GlobalSettings.Settings.LoadedGlobalStyle && GlobalSettings.CurrentStyle.IsGlobal;

            this.FillStyles();
        }

        private void ApplyLocalizedSaveStyleTexts()
        {
            var L = UiTranslate.Lang;

            this.Text = UiTranslate.T(L, "Save Keyboard Style", "儲存鍵盤樣式", "保存键盘样式", "キーボードスタイルを保存");
            this.lblName.Text = UiTranslate.T(L, "Name:", "名稱：", "名称：", "名前：");
            this.SaveButton.Text = UiTranslate.T(L, "Save", "儲存", "保存", "保存");
            this.CancelButton2.Text = UiTranslate.T(L, "Cancel", "取消", "取消", "キャンセル");
            this.chkGlobal.Text = UiTranslate.T(L, "Save as global style", "儲存為全域樣式", "保存为全局样式", "グローバルスタイルとして保存");
        }

        private void FillStyles()
        {
            var sRoot = FileHelper.FromKbs().FullName;
            this.rootPath = this.chkGlobal.Checked
                ? Path.Combine(sRoot, Constants.GlobalStylesFolder)
                : Path.Combine(sRoot, GlobalSettings.CurrentDefinition.Category, GlobalSettings.CurrentDefinition.Name);
            this.StyleCombo.Items.Clear();

            var root = new DirectoryInfo(this.rootPath);

            if (!root.Exists) return;

            this.StyleCombo.Items.AddRange(
                root.EnumerateFiles().Where(x => x.Extension == KeyboardStyle.StyleExtension)
                    .Select(x => (object)x.Name.Substring(0, x.Name.Length - KeyboardStyle.StyleExtension.Length))
                    .ToArray());

            this.StyleCombo.Text = GlobalSettings.Settings.LoadedStyle;
        }

        private void chkGlobal_CheckedChanged(object sender, System.EventArgs e)
        {
            this.FillStyles();
        }

        private void SaveButton_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.SelectedStyle))
                return;

            if (File.Exists(Path.Combine(this.rootPath, $"{this.SelectedStyle}{KeyboardStyle.StyleExtension}")))
            {
                var result = AppModalUi.ShowMessageBox(
                    this,
                    string.Format(
                        UiTranslate.T(
                            "Style {0} already exists, do you want to overwrite it?",
                            "樣式 {0} 已存在，要覆寫嗎？",
                            "样式 {0} 已存在，要覆盖吗？",
                            "スタイル {0} は既にあります。上書きしますか？"),
                        this.SelectedStyle),
                    UiTranslate.T("Already exists", "已存在", "已存在", "既に存在します"),
                    MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.No) return;
                if (result == DialogResult.Cancel)
                {
                    this.Close();
                    return;
                }
            }

            GlobalSettings.CurrentStyle.Name = this.SelectedStyle;
            GlobalSettings.CurrentStyle.Save(this.SelectedGlobal);
            GlobalSettings.Settings.LoadedStyle = this.SelectedStyle;
            GlobalSettings.Settings.LoadedGlobalStyle = this.SelectedGlobal;
            this.Close();
        }
    }
}
