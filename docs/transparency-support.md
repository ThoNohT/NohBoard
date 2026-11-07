# 透明背景与无边框覆盖层（add-transparency-support 分支）

本文描述 `add-transparency-support` 分支新增的透明覆盖层能力：按 **F1 切换真正的背景透明**、按 **F2 切换无边框窗口**，
并让样式里显式配置为透明的背景真正透出桌面（而不是渲染成黑色）。

## 1. 功能

| 功能 | 触发方式 | 说明 |
| --- | --- | --- |
| 透明模式 | `F1` / 右键菜单 **Toggle transparency (F1)** | 使用分层窗口（per-pixel alpha）渲染，样式背景色被忽略，键帽 PNG 的透明通道原样保留。**仅在窗口有焦点时生效**，不是全局热键 |
| 无边框模式 | `F2` / 右键菜单 **Toggle borderless window (F2)** | 去掉标题栏与边框；透明模式隐含无边框。**仅在窗口有焦点时生效** |
| 拖动窗口 | 无边框状态下按住鼠标左键拖动 | 记录 `Cursor.Position` 与 `Location` 的差值，逐帧移动；窗口位移与光标位移一致 |
| 状态记忆 | 关闭程序时 | `TransparentMode` / `BorderlessMode` 写入 `NohBoard.json`，重启后恢复 |
| 普通窗口 | — | 不强制置顶（不使用 `TopMost`）；最小化时暂停渲染、状态继续更新，恢复后立即补画 |

F1/F2 改成窗口内热键（`MainForm.OnKeyDown`）而不是全局低级钩子事件，因为用户明确要求“快捷键不要全局”：
这样只有 NohBoard 拥有焦点时才会切换模式，不会误伤正在运行的游戏或其它程序。

透明模式下背景色的处理规则：

1. 若当前样式的 `BackgroundColor.Alpha < 255`（样式显式配置了透明），启动后**自动进入透明模式**，不需要按 F1；
2. 若 `Alpha == 255`（旧样式），需要按 F1 才进入透明模式；F1 的透明是“强制全透明”，不写回样式文件；
3. 样式里的背景图片（`BackgroundImageFileName`）在透明模式下仍然绘制，并保留 PNG 自身的 alpha。

## 2. 配置：给样式背景色加 Alpha

`.style` 文件格式是 `DataContractJsonSerializer` 生成的 JSON。本分支给 `SerializableColor` 增加了一个**可选**的
`Alpha` 字段（`0`–`255`，缺省 `255`，为 `255` 时不会写进文件，因此旧样式文件保持原格式不变）。

例如把 `keyboards/Input Overlay/Input overlay layout/Input overlay style.style` 的背景改成全透明：

```json
"BackgroundColor": {
  "Alpha": 0,
  "Blue": 0,
  "Green": 0,
  "Red": 0
},
```

保存后直接启动 NohBoard 即自动透明；也可以在程序里按 F1 临时切换（F1 的状态会记在 `NohBoard.json`，
但不会改动 `.style` 文件）。

> 注意：请用不带 BOM 的 UTF-8 保存 `.style` / `NohBoard.json`，`DataContractJsonSerializer` 不接受 BOM（否则会加载失败回退到默认样式）。

## 3. 实现原理

### 3.1 分层窗口（per-pixel alpha）

- 透明模式通过给窗口加 `WS_EX_LAYERED` 并用 `UpdateLayeredWindow(ULW_ALPHA, AC_SRC_ALPHA)` 呈现；
- 每一帧渲染到 `Bitmap(w, h, PixelFormat.Format32bppPArgb)`（预乘 alpha），再逐行拷贝到一个
  `CreateDIBSection` 创建的 32bpp top-down DIB 中交给 `UpdateLayeredWindow`；
- 不使用 `TransparencyKey` 色键：色键对边缘抗锯齿会产生黑边，且无法表达 PNG 的半透明像素。

### 3.2 渲染复用

原来的绘制逻辑从 `OnPaint` 抽成 `RenderScene(Graphics)`：

- 不透明模式：`OnPaint → Clear(背景色) → RenderScene(...)`；
- 透明模式：`RenderLayeredFrame → Clear(Transparent) → 可选背景色(带 alpha) → RenderScene(...) → UpdateLayeredWindow`；
- `UpdateTimer_Tick` 在透明模式下只在键盘/鼠标状态变化、编辑模式或拖动窗口时重绘，静止的覆盖层不占用渲染时间。

### 3.3 热键（仅窗口内生效）

F1/F2 由 `MainForm.OnKeyDown` 处理（`e.Handled = e.SuppressKeyPress = true`），不再注册全局低级钩子事件：

- 只有 NohBoard 窗口拥有键盘焦点时热键才生效，游戏/其它程序里的 F1/F2 不受影响（用户要求“不要全局”）；
- 旧的 `HookManager.KeyEvent` 事件已从键盘钩子回调中移除，钩子现在只负责把按键状态喂给 `KeyboardState`；
- 属性表单“检测键码”时不影响，因为热键走的是窗口消息而不是钩子。

### 3.4 无边框、位置补偿与最小化

切换 `FormBorderStyle` 会改变窗口边框，因此先记录 `RectangleToScreen(ClientRectangle)`（客户区屏幕矩形），
切换后再恢复 `ClientSize` 并用两次矩形差值补偿 `Location`，保证键位在屏幕上不跳动。

窗口**不再强制置顶**（`TopMost` 保持用户/系统默认值，普通窗口即普通 z 序）：透明模式下窗口仍是普通窗口，
可以被其它窗口覆盖，也可以最小化。最小化时 `RenderLayeredFrame` 与 `UpdateTimer_Tick` 直接返回，不产生无效的
`UpdateLayeredWindow` 调用；键盘/鼠标状态照常更新，恢复窗口后下一帧立即补画。

## 4. 改动的文件

| 文件 | 改动 |
| --- | --- |
| `NohBoard/NohBoard/Forms/MainForm.Transparency.cs` | 新增：分层窗口渲染、F1/F2 切换与菜单、无边框切换、拖动窗口、状态持久化、最小化早退、管理员重启菜单 |
| `NohBoard/NohBoard/Forms/MainForm.cs` | `OnPaint` 拆分出 `RenderScene`；`OnKeyDown` 局部热键；`OnHandleCreated/OnHandleDestroyed/WndProc` 接 Raw Input；`UpdateTimer_Tick` 透明/最小化分支；启动时恢复状态；关闭时保存状态 |
| `NohBoard/NohBoard/Forms/MainForm.Editmode.cs` | 编辑模式之外支持拖动无边框窗口 |
| `NohBoard/Hooking/RawInputManager.cs` | 新增：注册 Raw Input（键盘/鼠标）、`WM_INPUT` 解析并桥接到 `KeyboardState` / `MouseState` |
| `NohBoard/Hooking/MouseState.cs` | 新增 `MouseFromCenter` 与 `RegisterRawMovement`（原始相对位移算速度） |
| `NohBoard/NohBoard/Extra/SerializableColor.cs` | 新增可选 `Alpha`（`DataMember`，`EmitDefaultValue=false`），`IsTransparent` |
| `NohBoard/NohBoard/Extra/GlobalSettings.cs` | 新增 `TransparentMode` / `BorderlessMode` 两个持久化设置 |

> Raw Input 与输入捕获的细节见 [input-capture-and-hotkeys.md](input-capture-and-hotkeys.md)。

## 5. 测试与验证

测试样板：`D:\soft\e-game\NohBoard-ReWrite-v1.3.0\`（`NohBoard.json` + `keyboards/`，类别 `Input Overlay`，
布局 `Input overlay layout`，样式 `Input overlay style`）。测试环境里在 NohBoard 窗口后面放一个全屏纯洋红色
（magenta）窗口，按窗口区域统计像素占比，透明生效时背景像素应变成洋红色。

实测结果（本机 Windows 11，200% 缩放下布局客户区 1308×696，`DOTNET_ROLL_FORWARD=Major`）：

| 步骤 | 窗口矩形 | 黑色像素占比 | 洋红像素占比 |
| --- | --- | --- | --- |
| 初始（普通窗口，非置顶） | 1334×774（含边框） | 76.9% | 2.4% |
| F2 无边框 | 1308×696 | 76.9% | — |
| F1 透明 | 1308×696 | **0%** | **86.1%** |
| 聚焦别的窗口后按 F1（验证非全局） | 1308×696 | **0%** | **86.1%** |
| 再按 F1（关透明） | 1308×696 | 86.9% | 0% |
| 重启（状态记忆） | 1308×696 | **0%** | **86.1%** |
| 样式 `Alpha: 0`（无需 F1） | 1308×696 | **0%** | **86.1%** |

其他验证（`artifacts/test-input-features.ps1` 自动化，全部断言通过）：

- 窗口与无边框模式都不置顶（`GetWindowLong(GWL_EXSTYLE) & WS_EX_TOPMOST == 0`）；
- 无边框下拖动：注入光标位移 611×305，窗口 `Location` 同步变化 611×305（完全跟手）；
- 后台按键：NohBoard 不聚焦时按 A，窗口出现按键渲染（像素差 3385），松开后恢复（像素差 0）；
- 最小化：最小化期间按 S，恢复后 S 的按下状态正确显示（像素差 3435），且恢复后不处于最小化；
- 非全局热键：把焦点切到另一个小窗口后按 F1，NohBoard 保持透明（洋红 86.1%），说明 F1 没有被全局拦截。

自动化脚本见 `artifacts/test-input-features.ps1`（本地测试用，未提交；DPI/打包见 [dpi-and-packaging.md](dpi-and-packaging.md)）。

## 6. 已知限制

- 仅 Windows；`UpdateLayeredWindow` 在部分远程桌面/虚拟机/旧显卡驱动上可能表现为黑底（系统限制）。
- 透明窗口没有标题栏，移动窗口需要在**非编辑模式**下按住左键拖动；编辑模式下按键/元素编辑优先。
- **F1/F2 只在 NohBoard 拥有焦点时生效**（有意设计，不再作为全局热键）；全屏游戏时请先用 Alt+Tab 切回。
- 窗口不置顶，是普通窗口；需要压在游戏上方时请使用游戏/直播软件的置顶设置，或把窗口放在合适位置。
- 最小化期间不渲染（但输入状态仍在更新），恢复后立即补画；任务栏里仍可看到普通窗口。
- F1 是强制全透明，会忽略样式背景色的 alpha；若希望透明程度由配置决定，请使用样式 `Alpha` 字段。
- 透明模式下窗口不可调整大小（`FormBorderStyle.None`）；如需修改布局尺寸，请先退出透明/无边框模式或使用编辑模式。
- OBS 采集分层窗口需要较新的 OBS 版本（窗口采集勾选“允许透明”/使用 Windows 10 1903+ 的捕获方式），否则可能采到黑底。
- 覆盖 UAC 提权窗口需要 NohBoard 本身以管理员运行；菜单里的 **Restart as administrator** 会给当前 exe 提权重启（不影响配置文件）。
