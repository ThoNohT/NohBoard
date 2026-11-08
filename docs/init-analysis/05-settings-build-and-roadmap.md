# 05 · 设置、构建发布与维护建议

> 文件：`Extra/GlobalSettings.cs`、`Extra/GlobalSettings.ChangeTracking.cs`、`Forms/SettingsForm.cs`、`NohBoard/NohBoard.csproj`、`NohBoard/gotri.exe`、`.github/`

## 1. 设置模型

设置是单个 `[DataContract] partial class GlobalSettings`（`Extra/GlobalSettings.cs:33`），**持久化到 exe 同目录的 `NohBoard.json`**（`Constants.SettingsFilename`；`Constants.ExePath` 基于 `Process.GetCurrentProcess().MainModule.FileName`）。不是 `%APPDATA%`、没有注册表。

### 1.1 字段与默认值

| 字段 | 默认 | 说明 |
|---|---|---|
| `WindowTitle` | `""` | 自定义标题；空则自动生成 `NohBoard {Version}` |
| `MouseSensitivity` | `50` | 鼠标速度指示器灵敏度（0–100，乘到速度上） |
| `ScrollHold` | `50` | 滚轮计数保持毫秒数（`HookManager.ScrollHold`） |
| `PressHold` | `0` | 松开后键帽保持按下的毫秒数（`HookManager.PressHold`） |
| `MouseFromCenter` | `false` | 速度相对屏幕中心而非绝对移动 |
| `UpdateInterval` | `33` | 重绘间隔 ms；`get => updateInterval ?? 33`，`set` 钳制到 **[5, 60000]** |
| `TrapKeyboard` / `TrapMouse` | `false` | 是否吞掉键/鼠事件（总开关仍是 ScrollLock） |
| `TrapToggleKeyCode` | `VK_SCROLL`(0x91) | 切换拦截的键 |
| `Capitalization` | `FollowKeys` | `CapitalizationMethod`：FollowKeys / AlwaysCaps / AlwaysLower |
| `FollowShiftForCapsInsensitive` | `false` | 大小写不敏感布局下是否跟随 Shift |
| `FollowShiftForCapsSensitive` | `false` | 大小写敏感布局下是否跟随 Shift |
| `LoadedCategory` / `LoadedKeyboard` / `LoadedStyle` | `null` | 上次加载的键盘与样式 |
| `LoadedGlobalStyle` | `false` | 上次样式是否为全局样式 |
| `X` / `Y` | `25` | 窗口位置（屏幕坐标） |
| `UpdateTextPosition` | `true` | 编辑模式下是否随拖动自动调整文字位置 |

其它静态成员：`CurrentDefinition`、`CurrentStyle`、`StyleDependencyCounter`、`UnsavedDefinitionChanges`、`UnsavedStyleChanges`、`Errors`。

`Load()` 的健壮性：文件不存在 → 返回 `true` + 默认值；反序列化异常 → `Errors = ex.Message`，返回 `false`，主窗口弹 `Failed to load the settings: {Errors}` 但继续用默认值运行。

### 1.2 设置界面（`Forms/SettingsForm.cs`）

- 数值/勾选框直接绑 `GlobalSettings.Settings`，OK 时写回并 `Save()`；
- **切换拦截键**：`txtToggleKey` 双击进入捕获模式，此时先 `Disable` 两个钩子，`KeyUp` 时把 `e.KeyCode` 存起来再 `Enable`，避免捕获到自己的开关；
- **大小写模式联动**：`rdbFollowKeystate` / `rdbAlwaysCaps` / `rdbAlwaysLower` 三选一，后两个勾选框只在 FollowKeys 且对应敏感模式下可用；
- `chkMouseFromCenter` 勾选时调用 `MouseState.SetMouseFromCenter(true, Screen.AllScreens 的中心列表)`；
- 所有 ToolTip 文案集中在 `SetToolTips()`，其中 `PressHold` 的 tooltip 还是占位 `TODO: Tooltip about holding presses.`（可直接补）。

## 2. 构建

### 2.1 项目配置（`NohBoard/NohBoard/NohBoard.csproj`）

```xml
<Project Sdk="Microsoft.NET.Sdk.WindowsDesktop">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>netcoreapp3.1</TargetFramework>
    <UseWindowsForms>true</UseWindowsForms>
    <RootNamespace>ThoNohT.NohBoard</RootNamespace>
    <StartupObject>ThoNohT.NohBoard.Program</StartupObject>
    <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
    <PublishReadyToRun>true</PublishReadyToRun>
    <PublishSingleFile>true</PublishSingleFile>
    <RuntimeIdentifier>win-x64</RuntimeIdentifier>
  </PropertyGroup>
  ...
</Project>
```

- `GenerateAssemblyInfo=false` 是因为 `AssemblyInfo.cs` 由构建期工具生成；
- 自定义 target `GenerateVersionedFiles`（`BeforeTargets="BeforeBuild"`）调用 `$(SolutionDir)gotri.exe` 两次，从 `Version.cs.template` / `Properties/AssemblyInfo.cs.template` 生成带版本号的文件，然后重新 glob 收集 `**/*.cs`（排除 `obj/`、`bin/`）；
- 依赖：`ProjectReference` 指向 `clipper_library` 与 `Hooking`；无 NuGet 包依赖（`packages.config`/`PackageReference` 都没有）。

### 2.2 本机实测（2026-11，Windows 11 + .NET SDK 9.0.304 / 8.0.301）

```powershell
cd D:\code\nohboard\NohBoard
dotnet build NohBoard.sln -c Debug
```

结果：**0 error，3 warnings**，产物在 `NohBoard/NohBoard/bin/Debug/netcoreapp3.1/win-x64/`。警告值得记录：

- `NETSDK1138`：`netcoreapp3.1` 已结束支持（EOL），建议升级 TFM；
- `NETSDK1201`：指定 `RuntimeIdentifier` 后 `dotnet build` 也会按 self-contained 处理，生成 `win-x64` 子目录；
- 第三条同为 SDK 针对 `RuntimeIdentifier` / 单文件发布的提示。

构建时会生成 `Version.cs` 与 `Properties/AssemblyInfo.cs`（都在 `.gitignore` 里），因此 `git status` 保持干净——**不要把这两个生成文件提交**。

### 2.3 发布

```powershell
cd D:\code\nohboard\NohBoard
dotnet publish NohBoard/NohBoard.csproj -c Release -r win-x64
```

（`PublishSingleFile` + `PublishReadyToRun` 已配好；发布目录里需要同时带上 `keyboards/`，否则程序起来是空白画布且设置里没有可选键盘。）

## 3. 分支与版本

| 分支 | 说明 |
|---|---|
| `master` | 当前主线（netcoreapp3.1 + WinForms），最后一次提交 2024-04-07（`Create FUNDING.yml`），共 312 commits |
| `Legacy` | 旧版（0.x 系列，对应 `.kb` 格式的历史实现） |
| `Legacy-Framework` | 旧版 .NET Framework 实现 |
| `dpi` | DPI 缩放相关实验分支 |
| `inputhistory` | 输入历史相关实验分支 |

- 没有 CI（`.github/` 下只有 `FUNDING.yml`），没有自动化测试项目；
- 版本号由 `gotri.exe` 在构建时生成，`VersionInfo.cs` 读取并展示；
- 发布走 GitHub Releases（手工），运行时通过 gist 上的 `nohboard_version.json` 检查新版本。

## 4. 维护与二次开发建议（按性价比排序）

### P0：能立刻降低风险

1. **升级 TFM 到 `net8.0-windows`**：改 csproj 的 `TargetFramework`/SDK，删掉 `UseWindowsForms`（新版 SDK 不需要 `WindowsDesktop`），验证 `Graphics`/`Font`/`DataContractJsonSerializer` 在 .NET 8 上的行为（后者仍可用但已不推荐）。顺带解决 NETSDK1138。
2. **补 CI**：GitHub Actions `windows-latest` + `dotnet build`，至少挡住“提交了编译不过的代码”。
3. **加最小测试**：`KeyboardDefinition.Load/Save` 往返、`KeyboardKeyDefinition` 几何（`Inside`/`BordersWith`/`UnionWith`）、`LegacyKbFileParser.Parse` 的样例文件——这些是纯逻辑，不依赖 WinForms 即可测。
4. **去掉 `dotnet build` 的 RuntimeIdentifier 副作用**：把 `RuntimeIdentifier` 移到发布配置（`Condition="'$(Configuration)'=='Release'"`），Debug 构建更快。

### P1：结构性改进（改动面较大，建议先写测试再动）

5. **替换序列化层**：`DataContractJsonSerializer` 的多态（`__type`/`KnownType`）与 `Dictionary` 数组化格式都很难维护。`System.Text.Json` + 自定义 `JsonConverter` 或 `[JsonDerivedType]`（.NET 7+）可读性更好，但**必须写迁移脚本/兼容读取**，否则所有已发布布局失效（`KeyboardVersion` 就是为这种迁移预留的）。
6. **拆掉静态全局状态**：`GlobalSettings` 同时是设置存储、当前文档、撤销历史宿主，测试与多窗口都不可能。最小改动是把 `CurrentDefinition`/`CurrentStyle`/undo 拆成 `KeyboardDocument` 类，`MainForm` 持有一个实例。
7. **渲染解耦**：`OnPaint` 现在直接读 `KeyboardState`/`MouseState` 静态类。抽一个 `IInputSource` 接口，可以让渲染在无钩子环境（测试/回放）下运行。
8. **合并 `Forms/` 与 `Controls/` 下重复的 `ColorChooser`/`FontChooser`**，或明确只保留一套。

### P2：产品/体验

9. **DPI 感知**：`app.manifest` 设置 `PerMonitorV2`，处理 `ClientSize` 与 `DeviceDpi`；`dpi` 分支已有探索可参考。当前在高 DPI 下布局会整体缩放模糊，鼠标坐标与画布像素可能错位。
10. **多显示器窗口位置**：`X/Y` 只存一个点，换屏/改分辨率后窗口可能跑到屏幕外；保存时做 `Screen.AllScreens` 可见性校验。
11. **撤销步数上限**（如 100 步）与“大键盘”内存保护。
12. **本地化**：所有 UI 字符串硬编码英文，可用 `.resx` 起步。
13. **编辑便利性**：网格/吸附、对齐辅助线、批量选择、多选拖动——目前只能单选。
14. **输入过滤**：按需过滤 `LLKHF_INJECTED` / `LLMHF_INJECTED`（见 04 文档）。

## 5. 改动前的风险清单

| 风险 | 说明 |
|---|---|
| 布局格式向后兼容 | 用户布局是数据资产；改 JSON 结构必须升 `Constants.KeyboardVersion` 并提供迁移 |
| 硬编码类型分派 | `MainForm`/`LoadKeyboardForm`/`MainMenu_Opening` 里到处是 `is KeyboardKeyDefinition`、`switch (element)`；新增元素类型漏一处就静默失效 |
| 静态状态时序 | `ResetBackBrushes()` 必须在样式/尺寸/元素变化后调用，否则画面与模型不一致 |
| 钩子是全局副作用 | trap 开着时用户可能“键盘失灵”，调试时先确认 ScrollLock 状态 |
| 单文件发布路径 | `Constants.ExePath` 在 `PublishSingleFile` 下取的是单文件 exe 所在目录，`keyboards/` 必须放它旁边；不要改成 `AppContext.BaseDirectory` 之前先验证两种发布方式 |
| 无测试/无 CI | 任何重构都建议先补 3–5 个针对模型层的测试再动手 |

## 6. 本地验证清单（改完代码跑一遍）

```powershell
cd D:\code\nohboard\NohBoard
dotnet build NohBoard.sln -c Debug          # 0 error
.\NohBoard\bin\Debug\netcoreapp3.1\win-x64\NohBoard.exe
```

1. 启动后恢复上次窗口位置与键盘；
2. 按几个键，键帽高亮正确、松开后消失（`PressHold` 生效）；
3. 切换键盘/样式（Ctrl 菜单或 Load 菜单）不崩、缺失字体有提示；
4. 进入编辑模式：拖动键、拖顶点、Alt 拖文字、加/删元素、撤销/重做；
5. 保存定义/样式后重启，改动还在；
6. 按 ScrollLock 能切换拦截（注意会吞掉系统键盘输入，再按一次恢复）。
