/*
Copyright (C) 2018 by Eric Bataille <e.c.p.bataille@gmail.com>

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

namespace ThoNohT.NohBoard
{
    using System;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using ThoNohT.NohBoard.Extra;

    /// <summary>
    /// Helper class for handling crashes.
    /// </summary>
    internal static class CrashHandler
    {
        /// <summary>
        /// Indicates whether a crash was handled, and shutting down should not be prevented.
        /// </summary>
        public static bool Crashed = false;

        /// <summary>
        /// Protects an action, showing an error message if it has failed, and writes a log file.
        /// </summary>
        /// <param name="action">The action to protect.</param>
        public static void Protect(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Handles an exception.
        /// </summary>
        public static void HandleException(Exception ex)
        {
            var logFile = GetLogFile();

            File.WriteAllText(logFile.FullName, $"{ShowException(ex)}{CollectState()}");

            MessageBox.Show($"NohBoard crashed. Exception message: {ex.Message}{Environment.NewLine}" +
                $"A crash log was generated: {logFile.FullName}", "NohBoard has crashed");
            Crashed = true;
            Application.Exit();
        }

        /// <summary>
        /// Collects the current state of the program.
        /// </summary>
        /// <returns>The state of the program.</returns>
        private static string CollectState()
        {
            var sb = new StringBuilder();

            sb.AppendLine();
            sb.AddLine("Current settings:");
            sb.AddLine(FileHelper.Serialize(GlobalSettings.Settings));

            if (!(GlobalSettings.CurrentDefinition is null))
            {
                sb.AppendLine();
                sb.AddLine("Current definition:");
                sb.AddLine(FileHelper.Serialize(GlobalSettings.CurrentDefinition));
            }

            if (!(GlobalSettings.CurrentStyle is null))
            {
                sb.AppendLine();
                sb.AddLine("Current style:");
                sb.AddLine(FileHelper.Serialize(GlobalSettings.CurrentStyle));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns a string describing the exception.
        /// </summary>
        private static string ShowException(Exception ex, int depth = 0)
        {
            var sb = new StringBuilder();

            sb.AddLine(ex.GetType().FullName);
            sb.AddLine($"Message: '{ex.Message}'.");
            sb.AddLine("Stack trace:");
            sb.AddLine(ex.StackTrace);

            if (!(ex.InnerException is null) && (depth < 10))
            {
                sb.AppendLine();
                sb.AddLine("Inner exception:");
                sb.Append(ShowException(ex.InnerException, depth + 1));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Adds a line to the provided string builder, after prepending it with a timestamp.
        /// </summary>
        private static void AddLine(this StringBuilder sb, string line)
        {
            sb.AppendLine(DateTimeFormat(line));
        }

        /// <summary>
        /// Formats a string with date and time prepended.
        /// </summary>
        private static string DateTimeFormat(string input)
        {
            return $"[{DateTime.Now:yyyy-MM-dd hh:mm:ss}] {input}";
        }

        /// <summary>
        /// Returns a unique filename to store the log in.
        /// </summary>
        private static FileInfo GetLogFile()
        {
            var counter = 1;
            var fileName = $"{DateTime.Now:yyyyMMdd-hhmmss.fffffff}{counter}.log";

            while (File.Exists(Path.Combine("logs", fileName))) {
                counter += 1;
                fileName = $"A{counter}";
            }

            var file = new FileInfo(Path.Combine("logs", fileName));

            if (!file.Directory.Exists) file.Directory.Create();

            return file;
        }
    }
}
