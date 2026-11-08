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

namespace ThoNohT.NohBoard.Localization
{
    using System.Collections.Generic;

    /// <summary>
    /// The simplified Chinese translations. The keys are the exact English texts that are used in the designer
    /// files, in <see cref="Loc.T(string)"/> and in <see cref="Loc.Format(string, object[])"/>. An ampersand marks a
    /// keyboard mnemonic in a menu text, it can be moved to a suitable Chinese character or dropped.
    ///
    /// Keep the keys byte-for-byte identical to the English texts in the source code, including trailing spaces and
    /// the <c>\r\n</c> line breaks of multi-line texts. Missing translations are simply shown in English, so adding a
    /// new text to the user interface without a translation never breaks anything.
    /// </summary>
    public static partial class Loc
    {
        /// <summary>
        /// The simplified Chinese translations.
        /// </summary>
        private static readonly Dictionary<string, string> ChineseSimplifiedTranslations = new Dictionary<string, string>
        {
            // ------------------------------------------------------------------------------------------------
            // Main window / context menu (MainForm.Designer.cs, MainForm.cs, MainForm.Editmode.cs)
            // ------------------------------------------------------------------------------------------------
            { "&Settings", "设置(&S)" },
            { "&Load Keyboard", "加载键盘(&L)" },
            { "Start &Editing", "开始编辑(&E)" },
            { "&Update Text Position", "更新文本位置(&U)" },
            { "&Move", "移动(&M)" },
            { "Move to &top", "移到最上层(&T)" },
            { "Move &up", "上移一层(&U)" },
            { "Move &down", "下移一层(&D)" },
            { "Move to &bottom", "移到最下层(&B)" },
            { "&Add Element", "添加元素(&A)" },
            { "Add &Keyboard Key", "添加键盘按键(&K)" },
            { "Add &Mouse Key", "添加鼠标按键(&M)" },
            { "Add Mouse &Scroll", "添加鼠标滚轮(&S)" },
            { "Add Mouse Speed &Indicator", "添加鼠标速度指示器(&I)" },
            { "&Remove Element", "删除元素(&R)" },
            { "Add Boundary Point", "添加边界点" },
            { "Remove Boundary Point", "删除边界点" },
            { "Keyboard &Properties", "键盘属性(&K)" },
            { "Element &Properties", "元素属性(&E)" },
            { "Keyboard &Style", "键盘样式(&S)" },
            { "Element &Style", "元素样式(&L)" },
            { "Save &Definition", "保存定义(&D)" },
            { "&Save &To <DefinitionName>", "保存到(&S) <定义名>" },
            { "Save &As", "另存为(&A)" },
            { "Save St&yle", "保存样式(&Y)" },
            { "&Save To <StyleName>", "保存到(&S) <样式名>" },
            { "Save To &Global <StyleName>", "保存到全局(&G) <样式名>" },
            { "E&xit", "退出(&X)" },
            { "Update available", "有可用更新" },
            { "Generate crash log", "生成崩溃日志" },
            { "Toggle &transparency (F1)", "切换透明模式(&T) (F1)" },
            { "Toggle &borderless window (F2)", "切换无边框窗口(&B) (F2)" },
            { "Restart as &administrator", "以管理员身份重启(&A)" },

            // Main menu texts that are composed at runtime.
            { "Save &To '{0}/{1}'", "保存到(&S) '{0}/{1}'" },
            { "Save &To '{0}'", "保存到(&S) '{0}'" },
            { "Save To &Global '{0}'", "保存到全局(&G) '{0}'" },
            { "New version available: {0}.", "发现新版本：{0}。" },
            { "Stop Editing", "停止编辑" },
            { "Start Editing", "开始编辑" },

            // Messages of the main window.
            { "Discard changes", "放弃更改" },
            { "You have unsaved changes. Loading a new keyboard will undo them. Are you sure you want to load a new keyboard?",
              "有未保存的更改。加载新的键盘会撤销这些更改。确定要加载新键盘吗？" },
            { "Reverted keyboard change.", "已还原键盘更改。" },
            { "Failed to load the settings: {0}", "加载设置失败：{0}" },
            { "Failed to load settings", "加载设置失败" },
            { "There was an error loading the saved keyboard definition file:",
              "加载已保存的键盘定义文件时出错：" },
            { "Failed to load style {0}, loading default style.", "加载样式 {0} 失败，正在加载默认样式。" },
            { "Error loading style.", "加载样式出错。" },
            { "You have unsaved changes. If you exit now you will lose them. Are you sure you want to exit?",
              "有未保存的更改。现在退出将丢失这些更改。确定要退出吗？" },
            { "This will crash NohBoard in order to generate a log, are you sure you want to do this?",
              "这会故意让 NohBoard 崩溃以生成日志，确定要继续吗？" },

            // Edit mode messages.
            { "You cannot remove another boundary, there must be at least 3.",
              "无法再删除边界点，至少需要保留 3 个。" },
            { "Error removing boundary", "删除边界点出错" },
            { "Please load or save a style before editing element styles.",
              "请先加载或保存样式，然后再编辑元素样式。" },
            { "Please load or save a style before editing the keyboard style.",
              "请先加载或保存样式，然后再编辑键盘样式。" },

            // ------------------------------------------------------------------------------------------------
            // Settings window (SettingsForm.Designer.cs, SettingsForm.cs)
            // ------------------------------------------------------------------------------------------------
            { "Settings", "设置" },
            { "Trapping", "输入捕获" },
            { "Trap Keyboard", "捕获键盘" },
            { "Trap Mouse", "捕获鼠标" },
            { "Trap toggle key:", "捕获切换键：" },
            { "Trapping the mouse or keyboard prevents the respective device's input from reaching any other applications.",
              "捕获鼠标或键盘后，对应设备的输入将不会再传递给任何其他应用程序。" },
            { "Input", "输入" },
            { "Mouse sensitivity:", "鼠标灵敏度：" },
            { "Scroll hold time:", "滚轮保持时间：" },
            { "Show keypresses for at least", "按键至少显示" },
            { "ms", "毫秒" },
            { "Calculate mouse speed from center of screen", "以屏幕中心为基准计算鼠标速度" },
            { "Ok", "确定" },
            { "Cancel", "取消" },
            { "Capitalization of Keys", "按键大小写" },
            { "Follow Caps-Lock and Shift", "跟随 Caps Lock 和 Shift" },
            { "Show all buttons capitalized", "所有按键显示为大写" },
            { "Show all buttons lower-case", "所有按键显示为小写" },
            { "Still follow shift for:", "仍跟随 Shift 的按键：" },
            { "Caps Lock insensitive keys", "不受 Caps Lock 影响的按键" },
            { "Caps Lock sensitive keys", "受 Caps Lock 影响的按键" },
            { "General", "常规" },
            { "Window title:", "窗口标题：" },

            // Language selector (added to the settings window).
            { "Language", "语言" },
            { "Language:", "语言：" },
            { "Takes effect after restart", "重启后生效" },
            { "Auto (follow system)", "自动（跟随系统）" },
            { "English", "英文" },
            { "Simplified Chinese", "简体中文" },
            { "The language change takes effect after restarting NohBoard.",
              "语言更改将在重启 NohBoard 后生效。" },

            // Tooltips of the settings window.
            { "Double click to change the toggle key for the mouse/keyboard trap.",
              "双击可修改鼠标/键盘捕获的切换键。" },
            { "The sensitivity with which the mouse speed indicator reacts to mouse movement.",
              "鼠标速度指示器对鼠标移动的响应灵敏度。" },
            { "The time (in milliseconds) before a mouse scroll is released.",
              "鼠标滚轮状态释放前的保持时间（毫秒）。" },
            { "Check to let the keyboard be trapped by pressing the trap toggle key.",
              "勾选后，按下捕获切换键即可捕获键盘。" },
            { "Check to let the mouse be trapped by pressing the trap toggle key.",
              "勾选后，按下捕获切换键即可捕获鼠标。" },
            { "Show capitalized letter when shift or caps is pressed, \r\nbut normal letters when both or neither are pressed.",
              "按下 Shift 或 Caps Lock 时显示大写字母，\r\n两者同时按下或都不按下时显示普通字母。" },
            { "Always show capitalized letters.", "始终显示大写字母。" },
            { "Always show lower-case letters.", "始终显示小写字母。" },
            { "When Always show capitalized or lower-case letters is selected, checking this means the shift \r\nkey is still used to shift to the alternate text on the element, for all elements that are not \r\nsensitive to caps lock.",
              "当选择“始终显示大写/小写字母”时，勾选此项表示对于所有不受 Caps Lock 影响的元素，\r\n仍会使用 Shift 键切换到元素上的备用文本。" },
            { "When Always show capitalized or lower-case letters is selected, checking this means the shift \r\nkey is still used to shift to the alternate text on the element, for all elements that are \r\nsensitive to caps lock.",
              "当选择“始终显示大写/小写字母”时，勾选此项表示对于所有受 Caps Lock 影响的元素，\r\n仍会使用 Shift 键切换到元素上的备用文本。" },
            { "Some games keep resetting the cursor position to the center of the screen. This setting uses \r\nthis fact and compares the current mouse position to the center of the screen, rather than its \r\nlast position.",
              "有些游戏会不断把光标重置到屏幕中心。此设置利用这一特性，\r\n将当前鼠标位置与屏幕中心进行比较，而不是与上一次的位置进行比较。" },
            { "Fill in if you want a custom window title.\r\nIf left empty, the default window title of \"NohBoard + version number\" will be shown.",
              "如需自定义窗口标题，请在此填写。\r\n留空时将显示默认标题“NohBoard + 版本号”。" },
            { "TODO: Tooltip about holding presses.", "按键按下状态至少显示的时长（毫秒）。" },
            { "The language of the user interface.", "用户界面的显示语言。" },
            { "Changes take effect after NohBoard is restarted.", "更改将在重启 NohBoard 后生效。" },

            // ------------------------------------------------------------------------------------------------
            // Load keyboard window (LoadKeyboardForm.Designer.cs, LoadKeyboardForm.cs)
            // ------------------------------------------------------------------------------------------------
            { "Load Keyboard", "加载键盘" },
            { "Category:", "类别：" },
            { "Keyboard Definition:", "键盘定义：" },
            { "Keyboard Style:", "键盘样式：" },
            { "Legacy Keyboard Files|*.kb", "旧版键盘文件|*.kb" },
            { "Load Legacy kb file...", "加载旧版 .kb" },
            { "Close", "关闭" },
            { "Delete", "删除" },
            { "Restart", "重启" },
            { "The following fonts are defined in the chosen style but not present on this system.\r\nIf a link is provided, you may download it by double clicking the link:",
              "所选样式中定义的以下字体在此系统上不存在。\r\n如果提供了链接，可以双击该链接下载：" },
            { "After a new font has been installed, NohBoard needs to be restarted to recognize it.",
              "安装新字体后，需要重启 NohBoard 才能识别。" },
            { "global: {0}", "全局：{0}" },
            { "No download URL provided", "未提供下载链接" },
            { "Font", "字体" },
            { "Link", "链接" },
            { "Link:", "链接：" },
            { "Are you sure you want to delete {0}/{1}?", "确定要删除 {0}/{1} 吗？" },
            { "Delete definition", "删除定义" },
            { "Failed to load keyboard {0}: {1}", "加载键盘 {0} 失败：{1}" },

            // ------------------------------------------------------------------------------------------------
            // Save definition/style windows (SaveKeyboardAsForm, SaveStyleAsForm)
            // ------------------------------------------------------------------------------------------------
            { "Name:", "名称：" },
            { "Save", "保存" },
            { "Save Keyboard Definition", "保存键盘定义" },
            { "Save Keyboard Style", "保存键盘样式" },
            { "Save as global style", "保存为全局样式" },
            { "{0} cannot be used for a category name.", "类别名称不能使用 {0}。" },
            { "Invalid name", "名称无效" },
            { "Keyboard {0}/{1} already exists, do you want to overwrite it?",
              "键盘 {0}/{1} 已存在，是否覆盖？" },
            { "Style {0} already exists, do you want to overwrite it?",
              "样式 {0} 已存在，是否覆盖？" },
            { "Already exists", "已存在" },

            // ------------------------------------------------------------------------------------------------
            // Element property windows (Forms/Properties/**)
            // ------------------------------------------------------------------------------------------------
            { "Accept", "确定" },
            { "Apply", "应用" },
            { "Add", "添加" },
            { "Remove", "删除" },
            { "Up", "上移" },
            { "Down", "下移" },
            { "Update", "更新" },
            { "Detect", "检测" },
            { "Detecting...", "检测中..." },
            { "Center", "居中" },
            { "Rectangle", "矩形" },
            { "Size", "尺寸" },
            { "Size:", "尺寸：" },
            { "Position", "位置" },
            { "Location:", "位置：" },
            { "Radius:", "半径：" },
            { "Boundaries:", "边界：" },
            { "Text:", "文本：" },
            { "Text Position:", "文本位置：" },
            { "Shift Text:", "Shift 文本：" },
            { "Key codes:", "按键代码：" },
            { "KeyCode:", "按键代码：" },
            { "Change capitalization on Caps Lock key", "按下 Caps Lock 时切换大小写" },
            { "Keyboard Key Properties", "键盘按键属性" },
            { "Keyboard Properties", "键盘属性" },
            { "MouseElementPropertiesForm", "鼠标元素属性" },
            { "Mouse Key Properties", "鼠标按键属性" },
            { "Mouse Scroll Properties", "鼠标滚轮属性" },
            { "Mouse Speed Indicator Properties", "鼠标速度指示器属性" },

            // ------------------------------------------------------------------------------------------------
            // Style windows (Forms/Style/**) and the reusable style controls (Controls/**)
            // ------------------------------------------------------------------------------------------------
            { "Key Style", "按键样式" },
            { "Keyboard Style", "键盘样式" },
            { "Mouse Speed Indicator Style", "鼠标速度指示器样式" },
            { "Loose", "松开" },
            { "Pressed", "按下" },
            { "Loose Keys", "松开的按键" },
            { "Pressed Keys", "按下的按键" },
            { "Keyboard", "键盘" },
            { "Mouse Speed", "鼠标速度" },
            { "MouseSpeedIndicator", "鼠标速度指示器" },
            { "SubStyle", "子样式" },
            { "Background", "背景" },
            { "Image:", "图片：" },
            { "Background Color", "背景颜色" },
            { "Text", "文本" },
            { "Text Color", "文本颜色" },
            { "Outline", "描边" },
            { "Outline Color", "描边颜色" },
            { "Outline Width", "描边宽度" },
            { "Show Outline", "显示描边" },
            { "Color 1 (low speed)", "颜色 1（低速）" },
            { "Color 2 (high speed)", "颜色 2（高速）" },
            { "Overwrite default style", "覆盖默认样式" },
            { "Pick a color", "请选择颜色" },
            { "Pick a color.", "请选择颜色。" },
            { "Pick a Font", "请选择字体" },
            { "Pick a font.", "请选择字体。" },
            { "Setting a smaller outline for pressed\r\nthan loose keys will show the loose\r\noutline behind the pressed key.",
              "按下状态的描边比松开状态小时，\r\n松开状态的描边会显示在\r\n按下按键的后面。" },
            { "If a font is used that is not present on Windows systems by default, this field can be used \r\nto provide an URL to download the font from. The user can then click on the link, download the \r\nfont and be able to use the style with the correct font. Note that every link only needs to be \r\nprovided once, even if it is used in multiple key definitions.",
              "如果使用了 Windows 默认没有的字体，可以在此填入该字体的下载链接。\r\n用户随后可以点击链接下载字体，从而以正确的字体使用该样式。\r\n每个链接只需填写一次，即使它被多个按键定义使用。" },

            // ------------------------------------------------------------------------------------------------
            // Crash handler (CrashHandler.cs)
            // ------------------------------------------------------------------------------------------------
            { "NohBoard crashed. Exception message: {0}{1}A crash log was generated: {2}",
              "NohBoard 已崩溃。异常信息：{0}{1}已生成崩溃日志：{2}" },
            { "NohBoard has crashed", "NohBoard 已崩溃" },
        };
    }
}
