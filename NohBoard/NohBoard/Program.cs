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

namespace ThoNohT.NohBoard
{
    using System;
    using System.Windows.Forms;
    using Forms;

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CrashHandler.Protect(() =>
            {
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.ThreadException += (s, e) => CrashHandler.HandleException(e.Exception);
                AppDomain.CurrentDomain.UnhandledException += (s, e) => CrashHandler.HandleException((Exception)e.ExceptionObject);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            });
        }
    }
}
