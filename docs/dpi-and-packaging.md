# DPI 感知与打包发布

本文档记录 `add-transparency-support` 分支上的 DPI 感知实现与本项目的打包/发布流程。透明与无边框功能见 [transparency-support.md](transparency-support.md)。

## 1. DPI 感知

### 1.1 目标

- 程序在高 DPI 显示器上按系统缩放比例渲染，而不是被 Windows 位图拉伸（发虚）。
- 键盘布局坐标保持“逻辑 96 DPI”：`keyboard.json` / `.style` 里的坐标与像素尺寸仍是原始设计值，旧的布局文件无需修改。
- 窗口被拖到不同缩放比例的显示器时能重新适配。

### 1.2 实现方式

启动时在创建任何窗口之前调用：

```csharp
// NohBoard/NohBoard/Program.cs
Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
```

`MainForm.Transparency.cs` 中的 `#region DPI`：

| 成员 | 作用 |
| --- | --- |
| `DpiScale => DebugDpiScaleOverride ?? (DeviceDpi / 96f)` | 逻辑坐标到设备像素的比例；DEBUG 构建可用环境变量 `NOHBOARD_DPI_SCALE` 覆盖，方便在 100% 机器上模拟 150%/200% |
| `ScaleToDevice(int)` / `ToLogical(Point)` | 逻辑 ↔ 设备像素换算 |
| `SetClientSizeToDefinition()` | 用 `定义尺寸 × DpiScale` 设置客户区 |
| `ApplyDpiScaling()` | DPI 变化时重设客户区、重建背景缓存并重绘 |
| `OnDpiChanged`（重写） | 响应窗口跨显示器移动 |

渲染侧：

- 预渲染背景位图（`ResetBackBrushes`）与透明模式离屏位图都按设备像素创建，绘制前 `g.ScaleTransform(DpiScale, DpiScale)`，元素仍用逻辑坐标绘制。
- 普通模式 `OnPaint` 用设备像素填背景位图，再缩放绘制元素；透明模式 `RenderLayeredFrame` 同理。
- 鼠标坐标在编辑模式、菜单命中、粘贴等入口统一经过 `ToLogical()`；窗口拖动、`ResizeEnd` 把客户区尺寸换算回逻辑尺寸写回定义。
- `AutoScaleMode = AutoScaleMode.None`：窗口尺寸完全由 DPI 代码管理，避免与 WinForms 自动缩放冲突。

### 1.3 实测（本机 200% 缩放，3840×2160）

测试样板：`D:\soft\e-game\NohBoard-ReWrite-v1.3.0\`（`NohBoard.json` + `keyboards/`，类别 `Input Overlay`，布局 `Input overlay layout` 逻辑尺寸 654×348，样式 `Input overlay style`）。

| 项目 | 结果 |
| --- | --- |
| 窗口客户区 | 1308×696 物理像素（654×348 × 2），文字/描边清晰无位图拉伸 |
| 布局与配置 | 正确加载测试样板的类别/布局/样式与元素图片 |
| F2 无边框后矩形 | 1308×696（去掉标题栏与边框，客户区位置不变） |
| 无边框拖动 | 拖动 120×60，窗口精确移动 120×60 |
| F1 透明 | 背景完全透出（洋红背板 86.1%），键帽描边/文字保留，黑色像素 0% |
| 再按 F1 | 恢复不透明黑底（黑色 86.9%） |
| 重启 | `TransparentMode` 持久化生效，无需再按 F1 |
| 样式 `Alpha: 0` | 不按 F1 也自动透明 |

## 2. 打包发布

### 2.1 版本号

版本号由 `gotri.exe` 在构建前根据 git 状态生成（`Version.cs` / `AssemblyInfo.cs`，均被 gitignore）：

- 有 tag（如 `v1.5.0`）时版本为 `v1.5.0`，没有 tag 时退化为 `v1.3.0.<提交数>~<hash>`；
- 工作树有未提交改动时会带 `*`（dirty）。

所以**发布前先提交并打 tag，再构建发布产物**。

### 2.2 构建发布包

目标框架已升级到 **net8.0-windows**。发布脚本 `packaging/build-release.ps1` 会一次生成两个包（推荐直接执行脚本）：

```powershell
powershell -ExecutionPolicy Bypass -File packaging\build-release.ps1 -Version v1.5.0
```

| 包 | exe 大小 | 是否依赖 .NET | 说明 |
| --- | --- | --- | --- |
| `NohBoard-v1.5.0-win-x64.zip` | **约 65 MB** | 否（自包含） | 单文件、已压缩；内置 .NET 8 桌面运行时（含 WPF 框架），所以体积大 |
| `NohBoard-v1.5.0-win-x64-lite.zip` | **约 1.6 MB**（启动器 0.33 MB + 核心 1.4 MB） | 需要 .NET 8 Desktop Runtime | 框架依赖单文件 + 运行时检测启动器；目标机需安装 Windows Desktop Runtime 8.x，缺失时启动器弹窗并引导下载（见 [lite-launcher.md](lite-launcher.md)） |

为什么旧版是 ~158 MB：`netcoreapp3.1` 的“自包含单文件”会把整个 .NET Core 3.1 桌面运行时（CoreCLR + BCL +
WinForms + WPF 桌面框架）原样打包，且 .NET Core 3.1 的单文件发布**不支持压缩**，所以 158 MB 是必然结果。
升级到 .NET 8 后开启 `EnableCompressionInSingleFile`（csproj 已配置），同样的自包含单文件降到约 65 MB；
如果目标机器已装 .NET 8 Desktop Runtime，则用 lite 版只有约 1.6 MB（其中 0.33 MB 是 .NET Framework 启动器）。

对应 publish 命令：

```powershell
# 自包含（压缩）单文件
dotnet publish NohBoard\NohBoard.csproj -c Release -r win-x64 --self-contained true

# 框架依赖单文件（小体积，需要 .NET 8 Desktop Runtime；框架依赖不支持单文件压缩）
dotnet publish NohBoard\NohBoard.csproj -c Release -r win-x64 --self-contained false `
  -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=false -o artifacts\publish-lite
```

说明：

- `keyboards/` 必须与 exe 同级（程序用 `ExePath/keyboards` 查找布局与样式）。
- 发布包里的 `NohBoard.json` 是默认配置（Input Overlay + 透明 + 无边框）；程序首次运行会在 exe 同目录保存用户配置。
- 进一步减小的可选方案：`PublishTrimmed`（WinForms 不支持裁剪，容易在运行时崩溃，不建议）；或者改用
  框架依赖版（约 1.6 MB）并让用户安装 .NET Desktop Runtime。
- 若不想升级 .NET 8，可退回 `netcoreapp3.1`，但自包含单文件会回到 ~158 MB（3.1 单文件不支持压缩）。

### 2.3 lite 启动器（.NET 8 运行时检测）

lite 包不是“一个 exe”，而是 `NohBoard.exe`（启动器）+ `NohBoard.Core.exe`（框架依赖单文件）两个文件：

- 启动器用 Windows 自带的 .NET Framework 4.8 编译（`packaging/lite-launcher/Launcher.cs` + csc），约 0.33 MB；
  它先检测 x64 .NET 8 Desktop Runtime（`dotnet\shared\Microsoft.WindowsDesktop.App\8.x`，目录优先、注册表兜底），
  缺失时弹中文/英文对话框说明缺少什么，并给出官方下载地址
  `https://dotnet.microsoft.com/zh-cn/download/dotnet/thank-you/runtime-desktop-8.0.33-windows-x64-installer`
  （「打开下载页面 / Open download page」按钮 / 「取消 / Cancel」按钮）；检测到则直接拉起同目录的
  `NohBoard.Core.exe` 并退出，不显示任何窗口。
- 发布包里的核心 exe 仍是 `requireAdministrator`，由启动器通过 ShellExecute 启动，UAC 行为与之前一致。
- 只构建 lite 包（迭代启动器时用，避免每次都打包 65 MB 的独立版）：
  `powershell -ExecutionPolicy Bypass -File packaging\build-release.ps1 -Version v-test -LiteOnly`

原理、检测顺序、测试方法和限制的完整说明见 [lite-launcher.md](lite-launcher.md)。

### 2.4 发布到 GitHub

```powershell
git tag v1.5.0
git push origin add-transparency-support --tags
gh release create v1.5.0 "dist\NohBoard-v1.5.0-win-x64.zip" `
  "dist\NohBoard-v1.5.0-win-x64-lite.zip" `
  --title "NohBoard v1.5.0" `
  --notes-file "dist\release-notes.md"
```

### 2.5 发布检查清单

- [ ] `dotnet build NohBoard.sln -c Debug` / `-c Release` 均 0 error
- [ ] 用 `D:\soft\e-game\NohBoard-ReWrite-v1.3.0\` 的配置与布局做一次人工冒烟（加载、F1、F2、拖动、重启记忆）
- [ ] 在 100% 与 200% 缩放下各验证一次窗口尺寸与清晰度
- [ ] 版本号无 `*`、无 `~hash` 后缀
- [ ] 独立版内 `NohBoard.exe` 与 `keyboards/` 同级；lite 版内同时有启动器 `NohBoard.exe` 和 `NohBoard.Core.exe`
- [ ] lite 版的 `README-FIRST.txt` 含 .NET 8 Desktop Runtime 下载地址
- [ ] lite 版启动器实测：`--simulate-missing-runtime` 能弹出中文引导弹窗（URL 原样、两个按钮可点），
      正常情况下能直接拉起 `NohBoard.Core.exe`（测试方法见 [lite-launcher.md](lite-launcher.md)）
