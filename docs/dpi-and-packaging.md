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

- 有 tag（如 `v1.4.0`）时版本为 `v1.4.0`，没有 tag 时退化为 `v1.3.0.<提交数>~<hash>`；
- 工作树有未提交改动时会带 `*`（dirty）。

所以**发布前先提交并打 tag，再构建发布产物**。

### 2.2 构建发布包

```powershell
# 在仓库根目录
cd NohBoard
dotnet publish NohBoard\NohBoard.csproj -c Release -r win-x64 --self-contained true
```

产物：`NohBoard/NohBoard/bin/Release/netcoreapp3.1/win-x64/publish/NohBoard.exe`（单文件，含 .NET 运行时，目标机无需安装 .NET）。

组装发布目录：

```powershell
$dist = "dist\NohBoard-v1.4.0-win-x64"
New-Item -ItemType Directory -Path $dist -Force | Out-Null
Copy-Item "NohBoard\NohBoard\bin\Release\netcoreapp3.1\win-x64\publish\NohBoard.exe" $dist
Copy-Item "keyboards" $dist -Recurse
Copy-Item "README.md","LICENSE","docs" $dist -Recurse
Compress-Archive -Path $dist -DestinationPath "dist\NohBoard-v1.4.0-win-x64.zip" -Force
```

说明：

- `keyboards/` 必须与 exe 同级（程序用 `ExePath/keyboards` 查找布局与样式）。
- 用户配置 `NohBoard.json` 生成在 exe 同目录，首次运行后出现，不应打进发布包。
- 若只发布框架依赖版：去掉 `--self-contained true`，目标机需要 .NET Core 3.1 运行时（或设置 `DOTNET_ROLL_FORWARD=Major` 用更高版本运行时）。

### 2.3 发布到 GitHub

```powershell
git tag v1.4.0
git push origin add-transparency-support --tags
gh release create v1.4.0 "dist\NohBoard-v1.4.0-win-x64.zip" `
  --title "NohBoard v1.4.0 (transparency + DPI)" `
  --notes-file "dist\release-notes.md"
```

### 2.4 发布检查清单

- [ ] `dotnet build NohBoard.sln -c Debug` / `-c Release` 均 0 error
- [ ] 用 `D:\soft\e-game\NohBoard-ReWrite-v1.3.0\` 的配置与布局做一次人工冒烟（加载、F1、F2、拖动、重启记忆）
- [ ] 在 100% 与 200% 缩放下各验证一次窗口尺寸与清晰度
- [ ] 版本号无 `*`、无 `~hash` 后缀
- [ ] 发布包内 `NohBoard.exe` 与 `keyboards/` 同级，且不含 `NohBoard.json`
