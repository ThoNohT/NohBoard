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

namespace ThoNohT.NohBoard.Extra
{
    /// <summary>
    /// UI strings by <see cref="UiLanguageCode"/> (settings UI language).
    /// </summary>
    public static class UiTranslate
    {
        /// <summary>
        /// Current UI language from settings (normalized).
        /// </summary>
        public static string Lang =>
            UiLanguageCode.Normalize(GlobalSettings.Settings?.UiLanguage ?? UiLanguageCode.EnUs);

        /// <summary>
        /// Pick localized text using explicit language code.
        /// </summary>
        public static string T(string lang, string en, string zhTw, string zhCn, string ja) =>
            lang switch
            {
                UiLanguageCode.ZhTw => zhTw,
                UiLanguageCode.ZhCn => zhCn,
                UiLanguageCode.JaJp => ja,
                _ => en,
            };

        /// <summary>
        /// Pick localized text using current settings language.
        /// </summary>
        public static string T(string en, string zhTw, string zhCn, string ja) => T(Lang, en, zhTw, zhCn, ja);

        /// <summary>
        /// Cycle button label for the settings language picker.
        /// </summary>
        public static string LanguageDisplayName(string lang) =>
            lang switch
            {
                UiLanguageCode.ZhTw => "繁體中文",
                UiLanguageCode.ZhCn => "简体中文",
                UiLanguageCode.JaJp => "日本語",
                _ => "English",
            };
    }
}
