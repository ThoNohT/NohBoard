/*
 * NohBoard lite launcher.
 *
 * The lite release is a framework-dependent single-file build of NohBoard, so its apphost needs the .NET 8
 * Desktop Runtime before it can do anything at all (including showing an error message). When the runtime is
 * missing the apphost only prints an English error to a console window, which most users never see.
 *
 * This tiny launcher is compiled with the .NET Framework C# compiler that ships with Windows
 * (%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\csc.exe, C# 5) so that it can run on a machine without .NET 8:
 *   1. It looks for an installed .NET 8 Windows Desktop Runtime (x64).
 *   2. If the runtime is missing, it shows a Chinese/English dialog that explains what is missing, shows the
 *      official download URL, and offers to open the download page in the browser.
 *   3. If the runtime is present, it starts NohBoard.Core.exe from its own directory and exits immediately.
 *
 * Keep this file compatible with C# 5 (no string interpolation, no expression-bodied members, no null
 * conditional operator, ...) because it is compiled by the old framework compiler. Only the following
 * assemblies are used: mscorlib, System, System.Drawing, System.Windows.Forms, Microsoft.Win32.Registry.
 *
 * For testing the "runtime missing" path the launcher can be forced into that branch with either
 *   NohBoard.exe --simulate-missing-runtime
 * or
 *   set NOHBOARD_LITE_SIMULATE_MISSING_RUNTIME=1
 */

namespace NohBoard.LiteLauncher
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using Microsoft.Win32;

    internal static class Program
    {
        /// <summary>The official .NET 8 Desktop Runtime (x64) download page shown to the user.</summary>
        internal const string DownloadUrl =
            "https://dotnet.microsoft.com/zh-cn/download/dotnet/thank-you/runtime-desktop-8.0.33-windows-x64-installer";

        /// <summary>File name of the real framework-dependent NohBoard executable shipped next to the launcher.</summary>
        internal const string CoreExecutableName = "NohBoard.Core.exe";

        private const int RequiredMajorVersion = 8;
        private const string SimulateMissingRuntimeArgument = "--simulate-missing-runtime";
        private const string SimulateMissingRuntimeEnvironmentVariable = "NOHBOARD_LITE_SIMULATE_MISSING_RUNTIME";

        [STAThread]
        private static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string runtimeVersion = RuntimeLocator.FindWindowsDesktopRuntime(RequiredMajorVersion);
            bool simulateMissingRuntime = IsSimulationRequested(args);

            if (simulateMissingRuntime || runtimeVersion == null)
            {
                using (MissingRuntimeDialog dialog = new MissingRuntimeDialog())
                {
                    dialog.ShowDialog();
                }

                return 0;
            }

            return LaunchCoreProcess();
        }

        /// <summary>
        /// Returns true when the "runtime missing" path should be forced. Used by automated tests only.
        /// </summary>
        private static bool IsSimulationRequested(string[] args)
        {
            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (string.Equals(args[i], SimulateMissingRuntimeArgument, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            string value = Environment.GetEnvironmentVariable(SimulateMissingRuntimeEnvironmentVariable);
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            return string.Equals(value, "1", StringComparison.OrdinalIgnoreCase)
                || string.Equals(value, "true", StringComparison.OrdinalIgnoreCase)
                || string.Equals(value, "yes", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Starts the real NohBoard executable next to the launcher and exits. ShellExecute is used on purpose:
        /// the release build of NohBoard.Core.exe requests administrator rights, so starting it must go through
        /// the shell instead of CreateProcess (otherwise the launch fails with ERROR_ELEVATION_REQUIRED).
        /// </summary>
        private static int LaunchCoreProcess()
        {
            string launcherDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (string.IsNullOrEmpty(launcherDirectory))
            {
                launcherDirectory = Environment.CurrentDirectory;
            }

            string corePath = Path.Combine(launcherDirectory, CoreExecutableName);
            if (!File.Exists(corePath))
            {
                ShowError(
                    string.Format(
                        "启动器所在目录中找不到 {0}。\r\n\r\n" +
                        "The launcher could not find {0} in its own directory.\r\n\r\n" +
                        "目录 / Directory: {1}",
                        CoreExecutableName,
                        launcherDirectory),
                    "NohBoard 启动失败 / Failed to start NohBoard");
                return 1;
            }

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = corePath;
            startInfo.WorkingDirectory = launcherDirectory;
            startInfo.UseShellExecute = true;

            try
            {
                Process.Start(startInfo);
                return 0;
            }
            catch (Win32Exception ex)
            {
                // ERROR_CANCELLED (1223): the user declined the UAC elevation prompt, so there is no error to report.
                if (ex.NativeErrorCode == 1223)
                {
                    return 1;
                }

                ShowLaunchError(ex);
                return 1;
            }
            catch (Exception ex)
            {
                ShowLaunchError(ex);
                return 1;
            }
        }

        private static void ShowLaunchError(Exception ex)
        {
            ShowError(
                string.Format(
                    "无法启动 NohBoard：{0}\r\n\r\nFailed to start NohBoard: {0}",
                    ex.Message),
                "NohBoard 启动失败 / Failed to start NohBoard");
        }

        private static void ShowError(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Locates an installed .NET Windows Desktop Runtime. The lite build targets win-x64 and .NET 8 framework
    /// dependent apps roll forward inside the 8.x major version only, so only an 8.x x64 desktop runtime counts.
    /// </summary>
    internal static class RuntimeLocator
    {
        private const string WindowsDesktopAppName = "Microsoft.WindowsDesktop.App";

        /// <summary>
        /// Returns the highest installed version of the given major version, or null when no such runtime exists.
        /// </summary>
        internal static string FindWindowsDesktopRuntime(int majorVersion)
        {
            // 1. The shared framework directory is the authoritative source: <root>\dotnet\shared\Microsoft.WindowsDesktop.App.
            string[] runtimeRoots = new string[]
            {
                Environment.GetEnvironmentVariable("DOTNET_ROOT_X64"),
                Environment.GetEnvironmentVariable("DOTNET_ROOT"),
                Environment.GetEnvironmentVariable("ProgramW6432"),
                Environment.GetEnvironmentVariable("ProgramFiles"),
            };

            for (int i = 0; i < runtimeRoots.Length; i++)
            {
                string root = runtimeRoots[i];
                if (string.IsNullOrEmpty(root))
                {
                    continue;
                }

                string sharedFrameworkDirectory = Path.Combine(root, "dotnet", "shared", WindowsDesktopAppName);
                string version = FindVersionInDirectory(sharedFrameworkDirectory, majorVersion);
                if (version != null)
                {
                    return version;
                }
            }

            // 2. Fall back to the registry. The .NET installers register the x64 shared frameworks here (some
            //    installers write through the 32-bit registry view, hence the WOW6432Node variant).
            string[] registryKeys = new string[]
            {
                @"SOFTWARE\dotnet\Setup\InstalledVersions\x64\sharedfx\" + WindowsDesktopAppName,
                @"SOFTWARE\WOW6432Node\dotnet\Setup\InstalledVersions\x64\sharedfx\" + WindowsDesktopAppName,
            };

            for (int i = 0; i < registryKeys.Length; i++)
            {
                string version = FindVersionInRegistry(Registry.LocalMachine, registryKeys[i], majorVersion);
                if (version != null)
                {
                    return version;
                }
            }

            return null;
        }

        private static string FindVersionInDirectory(string frameworkDirectory, int majorVersion)
        {
            if (string.IsNullOrEmpty(frameworkDirectory) || !Directory.Exists(frameworkDirectory))
            {
                return null;
            }

            string[] directories;
            try
            {
                directories = Directory.GetDirectories(frameworkDirectory);
            }
            catch (Exception)
            {
                return null;
            }

            string bestVersionText = null;
            Version bestVersion = null;
            for (int i = 0; i < directories.Length; i++)
            {
                string versionText = Path.GetFileName(directories[i]);
                Version version;
                if (!TryParseVersion(versionText, out version) || version.Major != majorVersion)
                {
                    continue;
                }

                if (bestVersion == null || version.CompareTo(bestVersion) > 0)
                {
                    bestVersion = version;
                    bestVersionText = versionText;
                }
            }

            return bestVersionText;
        }

        private static string FindVersionInRegistry(RegistryKey hive, string subKeyPath, int majorVersion)
        {
            try
            {
                using (RegistryKey key = hive.OpenSubKey(subKeyPath))
                {
                    if (key == null)
                    {
                        return null;
                    }

                    string[] valueNames = key.GetValueNames();
                    string bestVersionText = null;
                    Version bestVersion = null;
                    for (int i = 0; i < valueNames.Length; i++)
                    {
                        string versionText = valueNames[i];
                        Version version;
                        if (!TryParseVersion(versionText, out version) || version.Major != majorVersion)
                        {
                            continue;
                        }

                        if (bestVersion == null || version.CompareTo(bestVersion) > 0)
                        {
                            bestVersion = version;
                            bestVersionText = versionText;
                        }
                    }

                    return bestVersionText;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static bool TryParseVersion(string text, out Version version)
        {
            version = null;
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            try
            {
                Version parsed;
                if (!Version.TryParse(text, out parsed))
                {
                    return false;
                }

                version = parsed;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    /// <summary>
    /// The dialog shown when no .NET 8 Desktop Runtime is installed. A plain MessageBox cannot use the custom
    /// button captions the users asked for ("打开下载页面 / Open download page" and "取消 / Cancel"), so this is a
    /// small message-box style Form instead: same modal behaviour, no owner window, ESC cancels.
    /// </summary>
    internal sealed class MissingRuntimeDialog : Form
    {
        private const string DialogTitle = "需要 .NET 8 桌面运行时 / .NET 8 Desktop Runtime required";

        private const string ChineseMessage =
            "NohBoard 精简版（lite）需要目标电脑安装 Microsoft .NET 8 桌面运行时（Windows Desktop Runtime 8.x，x64）。\r\n" +
            "当前电脑未检测到可用的 .NET 8 桌面运行时，因此无法启动 NohBoard。\r\n" +
            "请下载并安装 .NET 8 桌面运行时（x64），安装完成后重新运行 NohBoard.exe。";

        private const string EnglishMessage =
            "NohBoard lite requires the Microsoft .NET 8 Desktop Runtime (Windows Desktop Runtime 8.x, x64).\r\n" +
            "No compatible .NET 8 Desktop Runtime was found on this computer, so NohBoard cannot start.\r\n" +
            "Download and install the runtime, then run NohBoard.exe again.";

        private const string UrlCaption = "官方下载地址 / Official download URL:";

        private const string HintMessage =
            "点击“打开下载页面”会在浏览器中打开官方下载页；也可以复制上面的链接手动下载。\r\n" +
            "Click \"Open download page\" to open the official page in your browser, or copy the URL above.";

        private const int FormWidth = 780;
        private const int MarginSize = 20;
        private const int ButtonHeight = 30;

        public MissingRuntimeDialog()
        {
            this.Font = SystemFonts.MessageBoxFont;
            this.Text = DialogTitle;
            this.AutoScaleMode = AutoScaleMode.None;
            this.ClientSize = new Size(FormWidth, 10);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = true;

            try
            {
                this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            }
            catch (Exception)
            {
                // Not worth failing over: the dialog works fine with the default icon.
            }

            int contentWidth = FormWidth - 2 * MarginSize;
            int y = MarginSize;

            Label chineseLabel = CreateMessageLabel(ChineseMessage, this.Font, MarginSize, y, contentWidth);
            this.Controls.Add(chineseLabel);
            y += chineseLabel.Height + 10;

            Label englishLabel = CreateMessageLabel(EnglishMessage, this.Font, MarginSize, y, contentWidth);
            this.Controls.Add(englishLabel);
            y += englishLabel.Height + 14;

            Label urlCaptionLabel = new Label();
            urlCaptionLabel.AutoSize = true;
            urlCaptionLabel.Text = UrlCaption;
            urlCaptionLabel.Location = new Point(MarginSize, y);
            this.Controls.Add(urlCaptionLabel);
            y += urlCaptionLabel.Height + 4;

            TextBox urlBox = new TextBox();
            urlBox.ReadOnly = true;
            urlBox.Text = Program.DownloadUrl;
            urlBox.Location = new Point(MarginSize, y);
            urlBox.Size = new Size(contentWidth, urlBox.PreferredHeight);
            this.Controls.Add(urlBox);
            y += urlBox.PreferredHeight + 14;

            Label hintLabel = CreateMessageLabel(HintMessage, this.Font, MarginSize, y, contentWidth);
            this.Controls.Add(hintLabel);
            y += hintLabel.Height + 14;

            Button openButton = new Button();
            openButton.Text = "打开下载页面 / Open download page";
            openButton.Size = new Size(250, ButtonHeight);
            openButton.Location = new Point(FormWidth - MarginSize - 110 - 10 - 250, y);
            openButton.Click += this.OpenDownloadPage;
            this.Controls.Add(openButton);

            Button cancelButton = new Button();
            cancelButton.Text = "取消 / Cancel";
            cancelButton.Size = new Size(110, ButtonHeight);
            cancelButton.Location = new Point(FormWidth - MarginSize - 110, y);
            cancelButton.Click += this.CloseDialog;
            this.Controls.Add(cancelButton);
            this.CancelButton = cancelButton;

            this.ClientSize = new Size(FormWidth, y + ButtonHeight + MarginSize);
        }

        private static Label CreateMessageLabel(string text, Font font, int x, int y, int width)
        {
            Label label = new Label();
            label.AutoSize = false;
            label.Font = font;
            label.Text = text;
            label.Location = new Point(x, y);
            label.Size = new Size(width, MeasureTextHeight(text, font, width));
            return label;
        }

        private static int MeasureTextHeight(string text, Font font, int width)
        {
            Size proposedSize = new Size(width, int.MaxValue);
            Size measured = TextRenderer.MeasureText(text, font, proposedSize, TextFormatFlags.WordBreak);
            return measured.Height + 4;
        }

        private void OpenDownloadPage(object sender, EventArgs e)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(Program.DownloadUrl);
                startInfo.UseShellExecute = true;
                Process.Start(startInfo);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    this,
                    "无法打开下载页面，请手动复制上面的链接在浏览器中访问。\r\n\r\n" +
                    "Could not open the download page. Please copy the URL above into your browser.\r\n\r\n" +
                    ex.Message,
                    DialogTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void CloseDialog(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
