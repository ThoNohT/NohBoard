# Lite 版启动器：.NET 8 运行时检测与下载引导

本文档记录 lite 版（框架依赖、单文件）的启动器实现：`packaging/lite-launcher/Launcher.cs`。
打包流程与 DPI 相关内容见 [dpi-and-packaging.md](dpi-and-packaging.md)。

## 1. 要解决的问题

lite 包用「框架依赖 + 单文件」发布，`NohBoard.exe` 是 .NET apphost，必须先找到 .NET 8 Desktop Runtime 才能加载程序集。
目标机器没有运行时的时候，apphost 只会弹出一闪而过的控制台窗口并输出英文错误（`You must install .NET Desktop Runtime...`），
普通用户既看不懂也不知道去哪里装。

因此 lite 包改为「启动器 + 真正的程序」两个 exe：

| 文件 | 大小 | 技术 | 作用 |
| --- | --- | --- | --- |
| `NohBoard.exe` | 约 0.33 MB | .NET Framework 4.8（Windows 自带），C# 5 | 启动器：检测运行时、弹窗引导、拉起 `NohBoard.Core.exe` |
| `NohBoard.Core.exe` | 约 1.4 MB | .NET 8（框架依赖单文件） | 真正的 NohBoard，需要 .NET 8 Desktop Runtime |

启动器用 Windows 自带的旧版 C# 编译器（`%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\csc.exe`）编译，不依赖任何
NuGet 包和 .NET SDK；因为 .NET Framework 4.8 在 Windows 10/11 上是系统组件，即使没装 .NET 8 也能运行。

## 2. 启动器逻辑

```
双击 NohBoard.exe
  ├─ 检测到 .NET 8 Desktop Runtime（x64）
  │    └─ ShellExecute 启动同目录的 NohBoard.Core.exe（工作目录 = 启动器目录），启动器立即退出
  └─ 没检测到（或测试开关强制）
       └─ 弹出中文/英文对话框：说明缺什么 + 官方下载 URL + 两个按钮
             ├─ “打开下载页面 / Open download page” → 用默认浏览器打开 URL，然后退出
             └─ “取消 / Cancel”（或 ESC）→ 直接退出
```

### 2.1 运行时检测（`RuntimeLocator`）

只认 **8.x 的 x64 Windows Desktop Runtime**，原因：

- lite 发布时 `-r win-x64`，所以需要 x64 运行时；
- `net8.0-windows` 框架依赖程序的默认 roll-forward 策略是 `Minor`，只会使用 8.0.x 的补丁/次要版本，
  装了 .NET 9/10 也**不能**运行，所以检测必须按 major == 8 过滤。

检测顺序：

1. 环境变量 `DOTNET_ROOT_X64`、`DOTNET_ROOT` 指向的自定义运行时根目录；
2. `%ProgramW6432%\dotnet\shared\Microsoft.WindowsDesktop.App\<版本>` 与
   `%ProgramFiles%\dotnet\shared\Microsoft.WindowsDesktop.App\<版本>`：枚举子目录，取 major == 8 的最高版本；
3. 注册表兜底（部分安装器从 32 位视图写入，所以两个位置都查，值为版本号）：
   - `HKLM\SOFTWARE\dotnet\Setup\InstalledVersions\x64\sharedfx\Microsoft.WindowsDesktop.App`
   - `HKLM\SOFTWARE\WOW6432Node\dotnet\Setup\InstalledVersions\x64\sharedfx\Microsoft.WindowsDesktop.App`

目录检测优先，因为它是运行时真实存在的位置；注册表只在目录缺失时兜底。

### 2.2 弹窗

弹窗没有用 `MessageBox`，因为 `MessageBox` 不支持自定义按钮文字，而用户要求按钮显示
「打开下载页面 / Open download page」和「取消 / Cancel」。启动器里用了一个等价的模态 WinForms 对话框
（`MissingRuntimeDialog`）：

- 标题：`需要 .NET 8 桌面运行时 / .NET 8 Desktop Runtime required`
- 正文：中文为主、英文附后，说明 lite 版缺什么、去哪里下载、装完后重新运行 `NohBoard.exe`
- 下载地址单独放在只读文本框里，内容与下面这一行完全一致，可选中复制：
  `https://dotnet.microsoft.com/zh-cn/download/dotnet/thank-you/runtime-desktop-8.0.33-windows-x64-installer`
- 按钮：「打开下载页面 / Open download page」（点击后用默认浏览器打开上面 URL）和「取消 / Cancel」
- 按 ESC 等价于「取消」；关闭弹窗后启动器退出码为 0

### 2.3 启动真正的程序

- `Process.Start` 使用 `UseShellExecute = true`。发布版 `NohBoard.Core.exe` 的清单是 `requireAdministrator`
  （NohBoard 要读取提权窗口的输入），`UseShellExecute = false` 的 `CreateProcess` 会因为
  `ERROR_ELEVATION_REQUIRED` 直接失败；ShellExecute 会正常弹出 UAC 提示。
- 用户在 UAC 里点「否」时（`Win32Exception` 错误码 1223）启动器静默退出，不再弹错误框。
- 启动器本身是 `asInvoker`：机器缺运行时时不需要管理员权限就能看到提示；真正的程序启动时才弹 UAC。
- `NohBoard.Core.exe` 缺失或启动失败时会有一个中文+英文的错误弹窗，退出码 1。

退出码约定：正常拉起 `NohBoard.Core.exe` 为 0；缺运行时弹窗被「取消」或点「打开下载页面」后退出为 0；
找不到核心 exe、启动失败、或用户在 UAC 中取消为 1。

## 3. 测试用的强制缺失开关

仅用于自动化/人工测试，正常打包不会用到：

```powershell
# 命令行参数
.\NohBoard.exe --simulate-missing-runtime

# 或环境变量（值 1/true/yes）
$env:NOHBOARD_LITE_SIMULATE_MISSING_RUNTIME = '1'
.\NohBoard.exe
```

两种方式都会跳过检测、直接走「缺少运行时」分支。注意：在本机已装 .NET 8 的情况下只有这个开关能测到弹窗分支；
真实缺运行时的机器上启动器会自然进入该分支。

## 4. 打包流程变化

`packaging/build-release.ps1` 的 lite 部分现在是：

1. `dotnet publish ... --self-contained false -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=false`
   得到框架依赖的 `NohBoard.exe`；
2. 把它改名为 `NohBoard.Core.exe`；
3. 用 Windows 自带的 csc 编译启动器，输出为 `NohBoard.exe`：
   ```powershell
   csc.exe /nologo /target:winexe /platform:anycpu /optimize+ `
       /win32icon:<仓库>\NohBoard\NohBoard\NohBoard2.ico `
       /out:<publish 目录>\NohBoard.exe <仓库>\packaging\lite-launcher\Launcher.cs
   ```
4. `New-Package` 支持复制多个 exe：独立版仍是单个 `NohBoard.exe`，lite 版是 `NohBoard.exe` + `NohBoard.Core.exe`。

找不到 csc 时脚本会报错并给出尝试过的路径。只构建 lite 包（迭代启动器时用，不打包 65 MB 的独立版）：

```powershell
powershell -ExecutionPolicy Bypass -File packaging\build-release.ps1 -Version v-test -LiteOnly
```

lite 包最终结构：

```
NohBoard-vX.Y.Z-win-x64-lite/
├─ NohBoard.exe            # 启动器（约 0.33 MB，.NET Framework）
├─ NohBoard.Core.exe       # 真正的 NohBoard（约 1.4 MB，框架依赖单文件）
├─ NohBoard.json           # 默认配置
├─ README-FIRST.txt        # 含运行时下载地址与独立版说明
├─ README.md
├─ LICENSE
├─ docs/
└─ keyboards/
```

## 5. 测试方法

在仓库根目录准备一个测试目录（不要放进 `dist/`）：

```powershell
$root = (Get-Location).Path
$src = Join-Path $root 'artifacts\publish-lite-test'
$dst = Join-Path $root 'artifacts\lite-test'
dotnet publish NohBoard\NohBoard\NohBoard.csproj -c Release -r win-x64 --self-contained false `
    -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=false `
    -p:SolutionDir="$root\NohBoard\" -p:ApplicationManifest="$root\artifacts\manifest-asinvoker.manifest" `
    -o $src
Rename-Item (Join-Path $src 'NohBoard.exe') 'NohBoard.Core.exe'
& 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe' /nologo /target:winexe /platform:anycpu /optimize+ `
    /win32icon:"$root\NohBoard\NohBoard\NohBoard2.ico" /out:"$src\NohBoard.exe" "$root\packaging\lite-launcher\Launcher.cs"
New-Item -ItemType Directory $dst -Force | Out-Null
Copy-Item (Join-Path $src 'NohBoard.exe'), (Join-Path $src 'NohBoard.Core.exe') $dst
Copy-Item keyboards $dst -Recurse
Copy-Item packaging\NohBoard.json (Join-Path $dst 'NohBoard.json')
```

> 测试构建用 `artifacts/manifest-asinvoker.manifest` 覆盖清单，避免测试时弹 UAC；发布包里的核心 exe 仍是
> `requireAdministrator`。启动器的行为与清单无关，所以不影响结论。

1. **正常路径**：运行 `artifacts\lite-test\NohBoard.exe`，预期启动器进程立即退出、`NohBoard.Core.exe`
   进程出现并显示 NohBoard 窗口；随后结束该进程。
2. **缺失路径**：运行 `NohBoard.exe --simulate-missing-runtime`（或环境变量方式），预期出现中文弹窗；
   本机是 200% 缩放，截图前要调用 `SetProcessDPIAware`（参考 `artifacts/ui-test-helpers.ps1` 的
   `Save-Screenshot` / `Save-WindowCrop`，测试脚本见 `artifacts/test-lite-launcher.ps1`）。
   可用 UI Automation 校验：标题、中文文案、URL 原文、两个按钮都存在；然后 Invoke「取消 / Cancel」，
   检查启动器已退出。
3. 测试结束确认没有残留的 `NohBoard.exe` / `NohBoard.Core.exe` 进程。

## 6. 注意事项与限制

- 启动器是 DPI-unaware 的（csc 默认清单），在 200% 缩放下由 Windows 位图放大，弹窗文字略软但可读；
  对一次性提示框来说可以接受，也避免 .NET Framework 4.8 下 PerMonitorV2 的额外复杂度。
- 启动器没有版本资源（`FileVersion` 显示 n/a），发布版本信息以 `NohBoard.Core.exe` 为准；
  启动器 exe 的体积（约 0.33 MB）主要来自 333 KB 的 `NohBoard2.ico`。
- 只支持 win-x64：如果以后加 ARM64 或 x86 lite 包，需要同时调整 `RuntimeLocator` 的检测根目录。
- 如果项目目标框架升级到 `net9.0-windows`/`net10.0-windows`，必须同步修改 `Launcher.cs` 里的
  `RequiredMajorVersion`（现在是 8，与 8.0.33 下载链接、lite 包语义一致）。
- 启动器的「打开下载页面」只打开官方页面，不下载/安装任何东西；安装动作始终由用户完成。
