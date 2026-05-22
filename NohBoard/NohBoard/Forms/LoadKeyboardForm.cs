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
    using ThoNohT.NohBoard.Hooking.Interop;
    using ThoNohT.NohBoard.Keyboard;
    using ThoNohT.NohBoard.Legacy;

    public partial class LoadKeyboardForm : Form
    {
        #region Fields

        private string SelectedCategory => (string)this.CategoryCombo.SelectedItem;

        private string SelectedDefinition => (string)this.DefinitionsList.SelectedItem;

        private StyleInfo SelectedStyle => (StyleInfo)this.StyleList.SelectedItem;

        private bool loaded = false;

        private bool styleListProgrammaticChange;

        private bool fontsGridApplyingColumnWidths;

        private const int LoadKeyboardClientHeight = 335;

        private const int BottomRowY = 305;

        private List<StyleInfo> globalStyles;

        private class StyleInfo
        {
            public bool Global { get; set; }

            public string Name { get; set; }

            public override string ToString()
            {
                return this.Global ? $"global: {this.Name}" : this.Name;
            }
        }

        private class MissingFont
        {
            public string Name { get; set; }

            public string Link { get; set; }
        }

        #endregion Fields

        #region Events

        public event Action<KeyboardDefinition, KeyboardStyle, bool> DefinitionChanged;

        #endregion Events

        #region Constructors

        public LoadKeyboardForm()
        {
            this.InitializeComponent();
            this.fontsGrid.Resize += this.fontsGrid_ColumnWidthsResize;
            this.ApplyBottomRowLayout();
        }

        #endregion Constructors

        public void ToggleFontsPanel(List<SerializableFont> missingFonts)
        {
            if (!missingFonts.Any())
            {
                this.ClientSize = new System.Drawing.Size(385, LoadKeyboardClientHeight);
                this.fontsGrid.Enabled = false;
                this.btnRestart.Enabled = false;
                this.lblMissingFonts.Visible = false;
                this.fontsGridPanel.Visible = false;
                this.lblRestart.Visible = false;
                this.btnRestart.Visible = false;
            }
            else
            {
                this.ClientSize = new System.Drawing.Size(990, LoadKeyboardClientHeight);
                this.fontsGrid.Enabled = true;
                this.btnRestart.Enabled = true;
                this.lblMissingFonts.Visible = true;
                this.fontsGridPanel.Visible = true;
                this.lblRestart.Visible = true;
                this.btnRestart.Visible = true;

                var L = UiTranslate.Lang;
                var gridData = missingFonts
                    .Select(f => new MissingFont
                    {
                        Name = f.FontFamily,
                        Link = f.DownloadUrl ?? UiTranslate.T(L, "No download URL provided", "未提供下載連結", "未提供下载链接", "ダウンロード URL がありません")
                    })
                    .ToList();
                this.fontsGrid.DataSource = gridData;
                this.ApplyFontsGridColumnWidthsAfterBind();
                this.ApplyFontsGridColumnHeaders();

                this.fontsGrid.Update();

            }

            this.ApplyBottomRowLayout();
        }

        private void ApplyBottomRowLayout()
        {
            this.CloseButton.Location = new System.Drawing.Point(this.CloseButton.Location.X, BottomRowY);
            this.lblRestart.Location = new System.Drawing.Point(this.lblRestart.Location.X, BottomRowY);
            this.btnRestart.Location = new System.Drawing.Point(this.btnRestart.Location.X, BottomRowY);
        }

        private void ApplyFontsGridColumnWidthsAfterBind()
        {
            if (this.fontsGrid.Columns.Count < 2 || this.fontsGridApplyingColumnWidths)
                return;

            this.fontsGridApplyingColumnWidths = true;
            try
            {
                const int nameColumnWidth = 140;
                this.fontsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                this.fontsGrid.Columns[0].MinimumWidth = 80;
                this.fontsGrid.Columns[0].Width = nameColumnWidth;

                this.fontsGrid.Columns[1].MinimumWidth = 120;
                var displayWidth = this.fontsGrid.DisplayRectangle.Width;
                var linkWidth = displayWidth - nameColumnWidth - 3;
                if (linkWidth < this.fontsGrid.Columns[1].MinimumWidth)
                    linkWidth = this.fontsGrid.Columns[1].MinimumWidth;
                this.fontsGrid.Columns[1].Width = linkWidth;
            }
            finally
            {
                this.fontsGridApplyingColumnWidths = false;
            }
        }

        private void fontsGrid_ColumnWidthsResize(object sender, EventArgs e)
        {
            this.ApplyFontsGridColumnWidthsAfterBind();
        }

        private void ApplyFontsGridColumnHeaders()
        {
            if (this.fontsGrid.Columns.Count < 2)
                return;

            var L = UiTranslate.Lang;
            this.fontsGrid.Columns[0].HeaderText = UiTranslate.T(L, "Name", "名稱", "名称", "名前");
            this.fontsGrid.Columns[1].HeaderText = UiTranslate.T(L, "Link", "連結", "链接", "リンク");
        }

        private void LoadLegacyButton_Click(object sender, System.EventArgs e)
        {
            var L = UiTranslate.Lang;
            var dialog = new OpenFileDialog
            {
                Filter = UiTranslate.T(L, "Legacy Keyboard Files|*.kb", "舊版鍵盤檔|*.kb", "旧版键盘文件|*.kb", "レガシーキーボード|*.kb")
            };
            var result = AppModalUi.ShowCommonDialog(dialog, this);

            if (result == DialogResult.Cancel) return;

            this.DefinitionChanged?.Invoke(LegacyKbFileParser.Parse(dialog.FileName), null, false);
            this.Close();
        }

        private void LoadKeyboardForm_Load(object sender, System.EventArgs e)
        {
            this.ApplyLocalizedLoadKeyboardTexts();

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
            this.ApplyPreviewSelection();
        }

        private void CategoryCombo_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.PopulateKeyboards();

            if (this.loaded)
                this.ApplyPreviewSelection();
        }

        private void CloseButton_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void mnuDeleteDefinition_Click(object sender, System.EventArgs e)
        {
            if (this.SelectedDefinition == null) return;

            var L = UiTranslate.Lang;
            var result = AppModalUi.ShowMessageBox(
                this,
                string.Format(
                    UiTranslate.T(
                        L,
                        "Are you sure you want to delete {0}/{1}?",
                        "確定要刪除 {0}/{1} 嗎？",
                        "确定要删除 {0}/{1} 吗？",
                        "{0}/{1} を削除しますか？"),
                    this.SelectedCategory,
                    this.SelectedDefinition),
                UiTranslate.T(L, "Delete definition", "刪除定義", "删除定义", "定義の削除"),
                MessageBoxButtons.YesNo);

            if (result != DialogResult.Yes)
                return;

            FileHelper.FromKbs(this.SelectedCategory, this.SelectedDefinition).Delete(true);
            this.LoadKeyboardForm_Load(null, null);
        }

        private void DefinitionsList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.LoadStyles();
                this.ApplyPreviewSelection();
            }
            catch (Exception ex)
            {
                this.ShowPreviewLoadError(ex, this.SelectedDefinition);
            }
        }

        private void StyleList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (this.styleListProgrammaticChange)
                return;

            try
            {
                this.ApplyPreviewSelection();
            }
            catch (Exception ex)
            {
                this.ShowPreviewLoadError(ex, this.SelectedStyle?.Name ?? this.SelectedDefinition);
            }
        }

        private void ApplyPreviewSelection()
        {
            if (!this.loaded || this.SelectedDefinition == null)
                return;

            if (this.StyleList.Items.Count == 0)
            {
                this.DefinitionChanged?.Invoke(
                    KeyboardDefinition.Load(this.SelectedCategory, this.SelectedDefinition, null),
                    null,
                    false);
                return;
            }

            if (this.SelectedStyle == null)
                return;

            this.DefinitionChanged?.Invoke(
                KeyboardDefinition.Load(
                    this.SelectedCategory,
                    this.SelectedDefinition,
                    this.SelectedStyle.Name),
                KeyboardStyle.Load(this.SelectedStyle.Name, this.SelectedStyle.Global),
                this.SelectedStyle.Global);
        }

        private void ShowPreviewLoadError(Exception ex, string itemName)
        {
            var L = UiTranslate.Lang;
            AppModalUi.ShowMessageBox(
                this,
                string.Format(
                    UiTranslate.T(
                        L,
                        "Failed to load keyboard {0}: {1}",
                        "載入鍵盤 {0} 失敗：{1}",
                        "加载键盘 {0} 失败：{1}",
                        "キーボード {0} の読み込みに失敗しました：{1}"),
                    itemName,
                    ex.Message),
                UiTranslate.T(L, "Error", "錯誤", "错误", "エラー"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private void fontsGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            var value = this.fontsGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            var linkText = value as string ?? value?.ToString();

            if (string.IsNullOrWhiteSpace(linkText))
                return;

            linkText = linkText.Trim();
            if (!Uri.TryCreate(linkText, UriKind.Absolute, out var uri))
                return;
            if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
                return;

            Process.Start(new ProcessStartInfo { FileName = uri.AbsoluteUri, UseShellExecute = true });
        }

        #region Helpers

        private void ApplyLocalizedLoadKeyboardTexts()
        {
            var L = UiTranslate.Lang;

            this.Text = UiTranslate.T(L, "Load Keyboard", "載入鍵盤", "加载键盘", "キーボードを読み込む");
            this.lblCategory.Text = UiTranslate.T(L, "Category:", "分類：", "类别：", "カテゴリ：");
            this.lblKeyboardDefinition.Text = UiTranslate.T(L, "Keyboard Definition:", "鍵盤定義：", "键盘定义：", "キーボード定義：");
            this.lblKeyboardStyle.Text = UiTranslate.T(L, "Keyboard Style:", "鍵盤樣式：", "键盘样式：", "キーボードスタイル：");
            this.LoadLegacyButton.Text = UiTranslate.T(L, "Load Legacy kb file...", "載入舊版鍵盤檔...", "加载旧版键盘文件...", "レガシー .kb ファイルを読み込む...");
            this.CloseButton.Text = UiTranslate.T(L, "Close", "關閉", "关闭", "閉じる");
            this.lblMissingFonts.Text = UiTranslate.T(
                L,
                "The following fonts are defined in the chosen style but not present on this system.\r\nIf a link is provided, you may download it by double clicking the link:",
                "下列字型在所選樣式中有使用，但此電腦未安裝。\r\n若有連結，請連按兩下連結以下載：",
                "下列字体在所选样式中有使用，但此电脑未安装。\r\n若有链接，请双击链接以下载：",
                "選択したスタイルで使われている次のフォントが、この PC にありません。\r\nリンクがある場合は、リンクをダブルクリックしてダウンロードできます：");
            this.lblRestart.Text = UiTranslate.T(
                L,
                "After a new font has been installed, bm-nohboard needs to be restarted to recognize it.",
                "安裝新字型後，需要重新啟動 bm-nohboard 才會辨識。",
                "安装新字体后，需要重新启动 bm-nohboard 才会识别。",
                "新しいフォントをインストールした後、bm-nohboard を再起動すると認識されます。");
            this.btnRestart.Text = UiTranslate.T(L, "Restart", "重新啟動", "重新启动", "再起動");
            this.mnuDeleteDefinition.Text = UiTranslate.T(L, "Delete", "刪除", "删除", "削除");
        }

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

            if (this.DefinitionsList.Items.Count > 0 && this.loaded)
                this.DefinitionsList.SelectedIndex = 0;

            this.LoadStyles();
        }

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

            this.styleListProgrammaticChange = true;
            try
            {
                this.StyleList.Items.Clear();
                this.StyleList.Items.AddRange(this.globalStyles.Cast<object>().ToArray());
                this.StyleList.Items.AddRange(specificStyles.Cast<object>().ToArray());

                if (this.StyleList.Items.Count == 0)
                    return;

                this.StyleList.SelectedIndex = 0;

                var loadedStyle = GlobalSettings.Settings.LoadedStyle;
                if (loadedStyle != null
                    && GlobalSettings.Settings.LoadedCategory == this.SelectedCategory
                    && GlobalSettings.Settings.LoadedKeyboard == this.SelectedDefinition)
                {
                    var styleIndex = this.FindStyleListIndex(loadedStyle);
                    if (styleIndex != -1)
                        this.StyleList.SelectedIndex = styleIndex;
                }
            }
            finally
            {
                this.styleListProgrammaticChange = false;
            }
        }

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

        private void btnRestart_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}
