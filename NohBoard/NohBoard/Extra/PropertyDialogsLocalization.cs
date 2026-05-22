/*
Copyright (C) 2017 by Eric Bataille <e.c.p.bataille@gmail.com>

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
    internal static class PropertyDialogsLocalization
    {
        public static string Cancel => UiTranslate.T("Cancel", "取消", "取消", "キャンセル");

        public static string Accept => UiTranslate.T("Accept", "確定", "确定", "OK");

        public static string Apply => UiTranslate.T("Apply", "套用", "应用", "適用");

        public static string Add => UiTranslate.T("Add", "新增", "添加", "追加");

        public static string Remove => UiTranslate.T("Remove", "移除", "移除", "削除");

        public static string Up => UiTranslate.T("Up", "上移", "上移", "上へ");

        public static string Down => UiTranslate.T("Down", "下移", "下移", "下へ");

        public static string Update => UiTranslate.T("Update", "更新", "更新", "更新");

        public static string Center => UiTranslate.T("Center", "置中", "居中", "中央");

        public static string Rectangle => UiTranslate.T("Rectangle", "矩形", "矩形", "矩形");

        public static string Detect => UiTranslate.T("Detect", "偵測", "检测", "検出");

        public static string Detecting => UiTranslate.T("Detecting...", "偵測中…", "检测中…", "検出中…");

        public static string KeyboardPropertiesTitle =>
            UiTranslate.T("Keyboard Properties", "鍵盤屬性", "键盘属性", "キーボードのプロパティ");

        public static string KeyboardKeyPropertiesTitle =>
            UiTranslate.T("Keyboard Key Properties", "鍵盤按鍵屬性", "键盘按键属性", "キーのプロパティ");

        public static string MouseKeyPropertiesTitle =>
            UiTranslate.T("Mouse Key Properties", "滑鼠按鍵屬性", "鼠标按键属性", "マウスボタンのプロパティ");

        public static string MouseScrollPropertiesTitle =>
            UiTranslate.T("Mouse Scroll Properties", "滑鼠捲動屬性", "鼠标滚动属性", "マウススクロールのプロパティ");

        public static string MouseElementFallbackTitle =>
            UiTranslate.T("Mouse Element Properties", "滑鼠元素屬性", "鼠标元素属性", "マウス要素のプロパティ");

        public static string MouseSpeedIndicatorPropertiesTitle =>
            UiTranslate.T(
                "Mouse Speed Indicator Properties",
                "滑鼠速度指示器屬性",
                "鼠标速度指示器属性",
                "マウス速度インジケーターのプロパティ");

        public static string RectangleBoundaryTitle =>
            UiTranslate.T("Rectangle", "矩形", "矩形", "矩形");

        public static string SizeLabel => UiTranslate.T("Size:", "大小：", "大小：", "サイズ：");

        public static string LocationLabel => UiTranslate.T("Location:", "位置：", "位置：", "位置：");

        public static string RadiusLabel => UiTranslate.T("Radius:", "半徑：", "半径：", "半径：");

        public static string TextLabel => UiTranslate.T("Text:", "文字：", "文字：", "テキスト：");

        public static string ShiftTextLabel => UiTranslate.T("Shift Text:", "Shift 文字：", "Shift 文字：", "Shift テキスト：");

        public static string TextPositionLabel =>
            UiTranslate.T("Text Position:", "文字位置：", "文字位置：", "テキスト位置：");

        public static string BoundariesLabel => UiTranslate.T("Boundaries:", "邊界：", "边界：", "境界：");

        public static string KeyCodesLabel => UiTranslate.T("Key codes:", "按鍵碼：", "键码：", "キーコード：");

        public static string KeyCodeLabel => UiTranslate.T("KeyCode:", "按鍵碼：", "键码：", "キーコード：");

        public static string PositionLabel =>
            UiTranslate.T("Position:", "位置：", "位置：", "位置：");

        public static string ChangeCapsCapitalization =>
            UiTranslate.T(
                "Change capitalization on Caps Lock key",
                "依 Caps Lock 鍵切換大小寫",
                "根据 Caps Lock 键切换大小写",
                "Caps Lock で大文字・小文字を切り替える");

        #region Style dialogs

        public static string StyleKeyboardStyleTitle =>
            UiTranslate.T("Keyboard Style", "鍵盤樣式", "键盘样式", "キーボードスタイル");

        public static string StyleKeyStyleTitle =>
            UiTranslate.T("Key Style", "按鍵樣式", "按键样式", "キースタイル");

        public static string StyleMouseSpeedIndicatorStyleTitle =>
            UiTranslate.T(
                "Mouse Speed Indicator Style",
                "滑鼠速度指示器樣式",
                "鼠标速度指示器样式",
                "マウス速度インジケーターのスタイル");

        public static string StyleBackgroundGroup =>
            UiTranslate.T("Background", "背景", "背景", "背景");

        public static string StyleImageLabel =>
            UiTranslate.T("Image:", "影像：", "图像：", "画像：");

        public static string StyleKeyboardLabel =>
            UiTranslate.T("Keyboard", "鍵盤", "键盘", "キーボード");

        public static string StylePressedKeysTitle =>
            UiTranslate.T("Pressed Keys", "按下鍵", "按下键", "押しているとき");

        public static string StyleLooseKeysTitle =>
            UiTranslate.T("Loose Keys", "鬆開鍵", "松开键", "離しているとき");

        public static string StyleMouseSpeedIndicatorPanelTitle =>
            UiTranslate.T("Mouse Speed Indicator", "滑鼠速度指示器", "鼠标速度指示器", "マウス速度インジケーター");

        public static string StylePressedShort =>
            UiTranslate.T("Pressed", "按下", "按下", "押下");

        public static string StyleLooseShort =>
            UiTranslate.T("Loose", "鬆開", "松开", "離し");

        public static string StyleMouseSpeedShort =>
            UiTranslate.T("Mouse Speed", "滑鼠速度", "鼠标速度", "マウス速度");

        public static string StyleOverwriteDefaultStyle =>
            UiTranslate.T("Overwrite default style", "覆寫預設樣式", "覆盖默认样式", "既定スタイルを上書き");

        public static string StyleBackgroundColorLabel =>
            UiTranslate.T("Background Color", "背景色彩", "背景色", "背景色");

        public static string StyleOutlineWidthWarning =>
            UiTranslate.T(
                "Setting a smaller outline for pressed\r\nthan loose keys will show the loose\r\noutline behind the pressed key.",
                "若按下時的外框較鬆開時細，\r\n鬆開狀態的外框會\r\n出現在按下鍵的後方。",
                "若按下时的描边比松开时细，\r\n松开状态的描边会\r\n显示在按下键的后方。",
                "押しているときの枠が離しているときより細いと、\r\n離しているときの枠が\r\n押したキーの後ろに見えます。");

        public static string StylePanelTextGroup =>
            UiTranslate.T("Text", "文字", "文字", "テキスト");

        public static string StylePanelOutlineGroup =>
            UiTranslate.T("Outline", "外框", "轮廓", "枠線");

        public static string StyleOutlineWidthLabel =>
            UiTranslate.T("Outline Width", "外框寬度", "轮廓宽度", "枠の幅");

        public static string StyleShowOutline =>
            UiTranslate.T("Show Outline", "顯示外框", "显示轮廓", "枠線を表示");

        public static string StyleTextColorLabel =>
            UiTranslate.T("Text Color", "文字色彩", "文字颜色", "テキスト色");

        public static string StyleOutlineColorLabel =>
            UiTranslate.T("Outline Color", "外框色彩", "轮廓颜色", "枠線の色");

        public static string StylePickFontLabel =>
            UiTranslate.T(
                "Double-click to pick a font.",
                "雙擊選擇字型",
                "双击选择字体",
                "フォントをダブルクリック");

        public static string StyleFontPreviewSample =>
            UiTranslate.T("Preview Aa", "預覽 Aa", "预览 Aa", "プレビュー Aa");

        public static string StylePickColorFallback =>
            UiTranslate.T("Pick a color.", "選擇色彩。", "选择颜色。", "色を選ぶ。");

        public static string StyleFontLinkLabel =>
            UiTranslate.T("Link:", "連結：", "链接：", "リンク：");

        public static string StyleMouseSpeedGeneralGroup =>
            UiTranslate.T("General", "一般", "常规", "一般");

        public static string StyleMouseSpeedColor1Low =>
            UiTranslate.T("Color 1 (low speed)", "色彩 1（低速）", "颜色 1（低速）", "色1（低速）");

        public static string StyleMouseSpeedColor2High =>
            UiTranslate.T("Color 2 (high speed)", "色彩 2（高速）", "颜色 2（高速）", "色2（高速）");

        #endregion Style dialogs
    }
}
