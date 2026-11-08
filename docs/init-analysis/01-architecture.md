# 01 · 总体架构与运行时

> 适用版本：master `6d3a2a4`（2024-04-07），312 commits，GPL-2.0。
> 代码根目录：仓库根下的 `NohBoard/`（解决方案目录），下文路径均相对仓库根。

## 1. 仓库与解决方案结构

```
NohBoard/
├─ NohBoard.sln
├─ gotri.exe                      # 提交进仓库的版本号生成工具（构建期调用）
├─ NohBoard/                      # 主程序，WinForms，netcoreapp3.1，win-x64
│  ├─ NohBoard.csproj
│  ├─ Program.cs / Constants.cs / CrashHandler.cs / VersionInfo.cs
│  ├─ Version.cs.template  → (构建生成 Version.cs)
│  ├─ Properties/AssemblyInfo.cs.template → (构建生成 AssemblyInfo.cs)
│  ├─ Forms/                      # 主窗口 + 所有对话框（MainForm 拆成 MainForm.cs / MainForm.Editmode.cs）
│  ├─ Controls/                   # 自定义控件（VectorTextBox、样式面板、颜色/字体选择器）
│  ├─ Keyboard/                   # 键盘数据模型、元素定义、样式
│  ├─ Extra/                      # GlobalSettings、序列化、几何、缓存等基础设施
│  └─ Legacy/                     # NohBoard 0.x `.kb` 格式解析与键码/字符映射
├─ Hooking/                       # netstandard2.1：低级键鼠钩子与全局输入状态
└─ clipper_library/               # netstandard2.1：Clipper 6（多边形布尔运算）源码内嵌
```

三个项目的关系：`NohBoard` 引用 `Hooking` 与 `clipper_library`；后两者互相独立，不引用 WinForms。

## 2. 启动链路

`NohBoard/NohBoard/Program.cs:29-42`：

```csharp
[STAThread]
static void Main()
{
    CrashHandler.Protect(() =>
    {
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += ...;
        AppDomain.CurrentDomain.UnhandledException += ...;
        Application.Run(new MainForm());
    });
}
```

- `CrashHandler`（`NohBoard/NohBoard/CrashHandler.cs:29`）在未处理异常时写崩溃日志并提示用户（日志里带版本信息）。
- 主窗口 `MainForm` 是 `partial class`，分两个文件：`Forms/MainForm.cs`（720 行，主循环/加载/设置/菜单）与 `Forms/MainForm.Editmode.cs`（963 行，编辑模式）。设计器文件 `MainForm.Designer.cs` 定义了**两个 `System.Windows.Forms.Timer`**：
  - `UpdateTimer.Interval = 33`（`MainForm.Designer.cs:93`），默认 30 FPS，实际值由 `GlobalSettings.Settings.UpdateInterval` 覆盖（5–60000ms）。
  - `KeyCheckTimer.Interval = 1000`（`:389`），每秒校验一次，防止钩子丢事件导致“键卡住”。

## 3. 运行时全局状态（无 DI，全静态单例）

整个程序没有依赖注入、没有 MVVM，核心状态放在三个静态入口：

| 入口 | 位置 | 内容 |
|---|---|---|
| `GlobalSettings.Settings` | `Extra/GlobalSettings.cs:56` | 持久化设置（`NohBoard.json`），`Load()/Save()` |
| `GlobalSettings.CurrentDefinition` | `Extra/GlobalSettings.cs:61` | 当前 `KeyboardDefinition`（元素列表） |
| `GlobalSettings.CurrentStyle` | `Extra/GlobalSettings.cs:66` | 当前 `KeyboardStyle`（颜色/字体/图片） |
| `GlobalSettings.StyleDependencyCounter` | `Extra/GlobalSettings.cs:77` | 样式变更计数，用于让元素缓存的 Brush 失效 |
| `GlobalSettings.UnsavedDefinitionChanges` / `UnsavedStyleChanges` | `:45/:51` | 脏标记，关闭/切换键盘时弹确认 |
| `KeyboardState` / `MouseState` | `Hooking/KeyboardState.cs:28`、`Hooking/MouseState.cs:33` | 全局键鼠状态（继承 `StateBase<T>`），由钩子线程写入、UI 线程读取 |
| `HookManager` | `Hooking/Interop/HookManager.cs:34`（partial） | 低级钩子的安装/卸载与回调 |

`GlobalSettings` 自身是 `[DataContract]`（`Extra/GlobalSettings.cs:33`），`Save()` 直接 `FileHelper.Serialize(Constants.SettingsFilename, Settings)`，即**设置文件与 exe 同目录**（`Constants.SettingsFilename => "NohBoard.json"`，`Constants.cs:68`；`Constants.ExePath` 见 `Constants.cs:73`）。

## 4. 一帧的生命周期（渲染管线）

```
键盘/鼠标钩子线程                        UI 线程（消息循环）
 键按下/抬起 ──┐
 鼠标移动   ──┼──> KeyboardState / MouseState (lock 保护的静态字典)
 滚轮/速度   ──┘         │  Updated = true
                          ▼
              UpdateTimer_Tick (33ms 默认)
              if (KeyboardState.Updated || MouseState.Updated) Refresh();
                          ▼
                     OnPaint
```

关键实现：

1. **背景预渲染**（`MainForm.cs:204-253`，`ResetBackBrushes()`）：以 `shift ∈ {false,true} × caps ∈ {false,true}` 组合建 4 张 `Bitmap`，先把样式背景图（`BackgroundImageFileName`）画上去，再把所有键以 `pressed = false` 画一遍，包成 `TextureBrush` 存入 `backBrushes[shift][caps]`。因此**只有按下的键需要逐帧重绘**。
2. **OnPaint**（`MainForm.cs:589`）：`Clear(BackgroundColor)` → 未加载定义则返回 → 用对应 shift/caps 的 `TextureBrush` 铺满窗口 → 取 `KeyboardState.PressedKeys`（`CheckKeyHolds(PressHold)` 之后）与 `MouseState.PressedKeys`、`ScrollCounts`、`AverageSpeed` → 逐个元素 `Render(..., alwaysRender: false)`。编辑模式下再叠加高亮/选中/正在操纵元素的绘制。
3. **元素级渲染条件**（`MainForm.cs:646` 的 `Render`）：
   - `KeyboardKeyDefinition`：`KeyCodes` 非空且**全部**包含于当前按下集合才算按下（支持组合键）；如果该键已被某个多键组合元素覆盖，则单键不再重复绘制。
   - `MouseKeyDefinition`：`KeyCodes.Single()` 是否按下。
   - `MouseScrollDefinition`：`scrollCounts[keycode] > 0`。
   - `MouseSpeedIndicatorDefinition`：使用 `MouseState.AverageSpeed`。
4. **更新节流**：`UpdateTimer_Tick`（`MainForm.cs:690`）只在状态真的变化时 `Refresh()`；`StateBase.Updated` 读取后自动复位（`Hooking/StateBase.cs`），所以空闲时几乎不产生绘制。`KeyCheckTimer_Tick`（`:699`）每秒调用 `CheckKeys(PressHold)` 清理“按住不放”的残留。

### 分层小结

```
┌──────────────────────────── UI (WinForms) ────────────────────────────┐
│ MainForm / LoadKeyboardForm / SettingsForm / *PropertiesForm / 样式表单 │
│ Controls: VectorTextBox, KeySubStylePanel, ColorChooser, FontChooser   │
└───────────────┬───────────────────────────────┬───────────────────────┘
                │ 编辑命令                        │ 读取输入状态
┌───────────────▼───────────────┐   ┌───────────▼───────────────────────┐
│ 模型层 Keyboard/              │   │ Hooking（netstandard2.1）         │
│ KeyboardDefinition / Element- │   │ HookManager → KeyboardState /     │
│ Definition 子类 / Style 子类  │   │ MouseState（StateBase<T>）         │
│ Clipper 几何（点内判定/合并） │   │ WH_KEYBOARD_LL / WH_MOUSE_LL      │
└───────────────┬───────────────┘   └───────────┬───────────────────────┘
                │ 序列化/文件                     │ SetWindowsHookEx / GetKeyState
┌───────────────▼───────────────┐   ┌───────────▼───────────────────────┐
│ Extra：FileHelper(DataContract│   │ Win32 (user32.dll)                │
│ JsonSerializer)、ImageCache、 │   └───────────────────────────────────┘
│ Geom、TPoint/TRectangle       │
└───────────────────────────────┘
```

## 5. 数据流：从磁盘到屏幕

```
keyboards/<Category>/<Name>/keyboard.json   ──KeyboardDefinition.Load()──┐
keyboards/global/<Style>.style        ┐                                  ├─> GlobalSettings.Current*
keyboards/<Category>/<Name>/<Style>.style ┘──KeyboardStyle.Load()────────┘
NohBoard.json  ──GlobalSettings.Load()──> Settings（含 LoadedCategory/Keyboard/Style）
                                                          │
                        MainForm_Load → LoadKeyboard() → ResetBackBrushes() → 显示
```

`MainForm_Load`（`MainForm.cs:351-421`）的顺序值得注意：先恢复窗口位置/标题，异步启动版本检查（GitHub gist），再按上次的 `LoadedCategory/LoadedKeyboard` 载入定义，然后按 `LoadedStyle/LoadedGlobalStyle` 载入样式并 `LoadKeyboard()` + `ResetBackBrushes()`；只有存在非键盘元素才启用鼠标钩子，只有存在 `KeyboardKeyDefinition` 才启用键盘钩子。

## 6. 值得注意的设计取舍

- **静态状态 + 全局可变对象**：极简但耦合度高；`MainForm.Editmode.cs` 的每个操作都要手动 `UpdateDefinition(...)` 并 `ResetBackBrushes()`，漏一处就会出现“改动已生效但画面没刷”的问题。
- **不可变式模型 + 快照式撤销**：所有修改方法（`Translate`/`Manipulate`/`Modify`/`AddElement`/`RemoveElement`）都返回新对象，撤销历史直接存整份 `(KeyboardDefinition, KeyboardStyle)` 快照（`GlobalSettings.ChangeTracking.cs`）。实现简单，但大键盘 + 长历史时内存与克隆开销明显。
- **预渲染背景位图**：按下/抬起只重绘变化元素，是最主要的性能手段；代价是任何样式/布局/尺寸变化都要重建 4 张位图（`StyleDependencyCounter` 就是为此服务的）。
- **hook 线程与 UI 线程直接共享静态字典**：只有 `lock`，没有消息队列或 `ConcurrentDictionary`；`Updated` 标志的读写也不是原子语义（bool 读写在 x86/x64 上实际安全，但设计上依赖这一点）。
- **版本检查**走硬编码 gist URL（`MainForm.cs:83` 附近），与上游仓库解耦。

## 7. 入口速查表

| 想改什么 | 起点 |
|---|---|
| 启动流程、崩溃处理 | `NohBoard/NohBoard/Program.cs`、`CrashHandler.cs` |
| 主窗口/菜单/窗口状态 | `Forms/MainForm.cs`、`Forms/MainForm.Designer.cs` |
| 编辑模式交互 | `Forms/MainForm.Editmode.cs` |
| 绘制逻辑 | `Forms/MainForm.cs:589-760`（`OnPaint`/`Render`） |
| 键盘数据模型 | `Keyboard/KeyboardDefinition.cs`、`Keyboard/ElementDefinitions/*.cs` |
| 样式模型 | `Keyboard/KeyboardStyle.cs`、`Keyboard/Styles/*.cs` |
| 键鼠钩子 | `Hooking/Interop/HookManager*.cs`、`Hooking/*State.cs` |
| 设置与持久化 | `Extra/GlobalSettings*.cs`、`Extra/FileHelper.cs` |
| 键盘加载 UI | `Forms/LoadKeyboardForm.cs` |
