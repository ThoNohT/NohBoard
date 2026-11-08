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
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows.Forms;

    /// <summary>
    /// The language that is used to display the user interface.
    /// </summary>
    public enum UiLanguage
    {
        /// <summary>
        /// Follow the language of the operating system (default).
        /// </summary>
        Auto = 0,

        /// <summary>
        /// English.
        /// </summary>
        English = 1,

        /// <summary>
        /// Simplified Chinese.
        /// </summary>
        ChineseSimplified = 2,
    }

    /// <summary>
    /// Provides the user interface translations.
    ///
    /// The English texts used in the WinForms designer files are the translation keys. <see cref="Apply(Control)"/>
    /// walks a control tree (including context menus and tool strips) and replaces every known English text with its
    /// translation. Texts that are composed at runtime have to be translated at the place where they are assigned, with
    /// <see cref="T(string)"/> or <see cref="Format(string, object[])"/>.
    /// </summary>
    public static partial class Loc
    {
        /// <summary>
        /// The language that is currently used to display the user interface.
        /// </summary>
        private static UiLanguage effectiveLanguage;

        /// <summary>
        /// Indicates whether <see cref="Initialize(UiLanguage)"/> has been called.
        /// </summary>
        private static bool initialized;

        /// <summary>
        /// The translation table for the currently used language, or <c>null</c> for English (the source language).
        /// </summary>
        private static Dictionary<string, string> translations;

        /// <summary>
        /// Gets the language that is currently used to display the user interface.
        /// </summary>
        public static UiLanguage EffectiveLanguage
        {
            get
            {
                EnsureInitialized();
                return effectiveLanguage;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user interface is displayed in Chinese.
        /// </summary>
        public static bool IsChinese => EffectiveLanguage == UiLanguage.ChineseSimplified;

        /// <summary>
        /// Initializes the localization with the language configured by the user. This is normally called once at
        /// startup, after the global settings have been loaded. The environment variable <c>NOHBOARD_UI_LANGUAGE</c>
        /// (<c>en</c>, <c>zh</c> or <c>auto</c>) overrides the configured language, which is useful for testing.
        /// </summary>
        /// <param name="preference">The language configured by the user.</param>
        public static void Initialize(UiLanguage preference)
        {
            var environment = Environment.GetEnvironmentVariable("NOHBOARD_UI_LANGUAGE");
            if (!string.IsNullOrWhiteSpace(environment))
            {
                switch (environment.Trim().ToLowerInvariant())
                {
                    case "en":
                    case "en-us":
                    case "english":
                        preference = UiLanguage.English;
                        break;

                    case "zh":
                    case "zh-cn":
                    case "zh-hans":
                    case "chinese":
                        preference = UiLanguage.ChineseSimplified;
                        break;

                    case "auto":
                    case "system":
                        preference = UiLanguage.Auto;
                        break;
                }
            }

            effectiveLanguage = preference == UiLanguage.Auto
                ? GetSystemLanguage()
                : preference;

            translations = effectiveLanguage == UiLanguage.ChineseSimplified
                ? ChineseSimplifiedTranslations
                : null;

            initialized = true;
        }

        /// <summary>
        /// Returns the language that follows from the language of the operating system.
        /// </summary>
        /// <returns>The language to use.</returns>
        private static UiLanguage GetSystemLanguage()
        {
            var language = CultureInfo.CurrentUICulture;
            if (language.TwoLetterISOLanguageName == "zh")
            {
                // Traditional Chinese is not translated yet, fall back to English for it.
                if (language.Name.IndexOf("Hant", StringComparison.OrdinalIgnoreCase) >= 0
                    || language.Name.IndexOf("TW", StringComparison.OrdinalIgnoreCase) >= 0
                    || language.Name.IndexOf("HK", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return UiLanguage.English;
                }

                return UiLanguage.ChineseSimplified;
            }

            return language.TwoLetterISOLanguageName == "en"
                ? UiLanguage.English
                : UiLanguage.English;
        }

        /// <summary>
        /// Makes sure the translations are available, even when <see cref="Initialize(UiLanguage)"/> was not called
        /// yet.
        /// </summary>
        private static void EnsureInitialized()
        {
            if (!initialized) Initialize(UiLanguage.Auto);
        }

        /// <summary>
        /// Translates one of the English user interface texts to the current language. When no translation is known,
        /// the English text is returned unchanged, so missing translations never break the user interface.
        /// </summary>
        /// <param name="english">The English text.</param>
        /// <returns>The text in the current language.</returns>
        public static string T(string english)
        {
            if (string.IsNullOrEmpty(english)) return english;

            EnsureInitialized();
            if (translations != null && translations.TryGetValue(english, out var translated))
            {
                return translated;
            }

            return english;
        }

        /// <summary>
        /// Translates an English user interface format string and formats it with the given arguments. The argument
        /// values themselves are not translated.
        /// </summary>
        /// <param name="english">The English format string.</param>
        /// <param name="args">The arguments for the format string.</param>
        /// <returns>The formatted text in the current language.</returns>
        public static string Format(string english, params object[] args)
        {
            try
            {
                return string.Format(CultureInfo.CurrentCulture, T(english), args);
            }
            catch (FormatException)
            {
                // A wrong format string should never crash the application.
                return T(english);
            }
        }

        /// <summary>
        /// Translates the texts of the given control and all of its child controls. The texts configured in the
        /// designer files are used as keys; unknown texts are left unchanged.
        /// </summary>
        /// <param name="root">The control to translate.</param>
        public static void Apply(Control root)
        {
            if (root == null) return;

            ApplyControl(root);
        }

        /// <summary>
        /// Translates the texts of all items in the given tool strip item collection, including the items of
        /// drop-down menus.
        /// </summary>
        /// <param name="items">The items to translate.</param>
        public static void Apply(ToolStripItemCollection items)
        {
            if (items == null) return;

            foreach (ToolStripItem item in items)
            {
                if (!string.IsNullOrEmpty(item.Text))
                {
                    item.Text = T(item.Text);
                }

                if (item is ToolStripDropDownItem dropDown)
                {
                    Apply(dropDown.DropDownItems);
                }
            }
        }

        /// <summary>
        /// Translates the text of a single control and recursively applies the translations to its children.
        /// </summary>
        /// <param name="control">The control to translate.</param>
        private static void ApplyControl(Control control)
        {
            if (!string.IsNullOrEmpty(control.Text))
            {
                var translated = T(control.Text);
                if (!ReferenceEquals(translated, control.Text) && translated != control.Text)
                {
                    control.Text = translated;
                }
            }

            if (control is ToolStrip toolStrip)
            {
                Apply(toolStrip.Items);
            }

            if (control.ContextMenuStrip != null)
            {
                Apply(control.ContextMenuStrip.Items);
            }

            foreach (Control child in control.Controls)
            {
                ApplyControl(child);
            }
        }
    }
}
