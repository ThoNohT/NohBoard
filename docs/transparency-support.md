# 透明背景与无边框覆盖层（add-transparency-support 分支）

本文描述 `add-transparency-support` 分支新增的透明覆盖层能力：**F1 只切换背景透明**、**F2 只切换无边框窗口**，
两者互相独立，因此有四种组合：不透明有边框（普通窗口）、透明有边框、不透明无边框、透明无边框。

主窗口在两种透明组合下的渲染、鼠标交互完全一致，都是 **per-pixel alpha**（`UpdateLayeredWindow`）：
键帽 PNG 的 alpha 通道、半透明阴影和抗锯齿边缘都原样保留。区别只在于是否有额外的**原生边框窗口**：

| 组合 | 实现 |
| --- | --- |
| 有边框 + 透明 | 主窗口：无边框 + per-pixel alpha；**外面再套一个原生边框窗口**（`OverlayFrameForm`）提供标题栏和边框 |
| 无边框 + 透明 | 主窗口：无边框 + per-pixel alpha（与上面完全相同，只是没有边框窗口） |
| 无边框 + 不透明 | 普通 GDI+ 绘制，只是没有标题栏/边框 |
| 有边框 + 不透明 | 普通窗口 |

> 历史说明：早期版本为了在保留原生边框的同时做透明，用过 `Form.TransparencyKey` 颜色键方案。颜色键只能表达
> “透明/不透明”二值，且键控像素会被系统做成鼠标穿透、半透明键帽边缘会与底色混合出明显硬边（毛刺）。现在的
> 方案让主窗口继续走 per-pixel alpha，另用一个独立窗口提供原生边框，因此透明质量、鼠标点击/拖动/右键/编辑
> 行为与无边框模式**逐像素一致**（自动化验证中两种模式的同一区域截图逐像素差异为 0）。

## 1. 功能

| 功能 | 触发方式 | 说明 |
| --- | --- | --- |
| 透明模式 | `F1` / 右键菜单 **Toggle transparency (F1)** | 只切换背景透明，不再隐含无边框。透明时主窗口用 per-pixel alpha 渲染，样式背景色被忽略、桌面/游戏画面从透明像素透出；有边框模式下原生标题栏和边框由边框窗口提供。**仅在窗口有焦点时生效**，不是全局热键 |
| 无边框模式 | `F2` / 右键菜单 **Toggle borderless window (F2)** | 只切换标题栏/边框，不改变透明状态；与 F1 完全独立。**仅在窗口有焦点时生效** |
| 拖动窗口 | 无边框/透明状态下按住鼠标左键拖动客户区（非四角区域）；也可以直接拖动原生边框窗口的标题栏 | 记录 `Cursor.Position` 与 `Location` 的差值逐帧移动，两种透明模式都跟手；边框窗口与主窗口位置双向同步 |
| 缩放窗口 | 客户区四角手柄（所有模式都可用） | **保持布局宽高比**，缩放比例持久化到 `NohBoard.json` 的 `OverlayScale`；拖角时光标显示斜向双箭头 |
| 点击穿透 | 窗口组（主窗口或边框窗口）**没有焦点**时自动生效 | 鼠标事件穿透到下方窗口；窗口重新获得焦点（Alt+Tab、任务栏或点击原生标题栏）后恢复可点击、可拖动 |
| 整窗可点击 | 两种透明模式都自动启用 | 每帧铺一层 alpha=1 的近透明底色（肉眼几乎不可见），使原本 alpha=0 的透明区域也可点击、可拖动、可右键 |
| 状态记忆 | 关闭程序时 | `TransparentMode` / `BorderlessMode` / `OverlayScale` 写入 `NohBoard.json`，重启后恢复 |
| 普通窗口 | — | 不强制置顶（不使用 `TopMost`）；最小化时暂停渲染、状态继续更新，恢复后立即补画 |

F1/F2 是窗口内热键（`MainForm.OnKeyDown`，边框窗口获得焦点时由 `OverlayFrameForm` 转发）而不是全局低级钩子事件，
因为用户明确要求“快捷键不要全局”：这样只有 NohBoard 拥有焦点时才会切换模式，不会误伤正在运行的游戏或其它程序。

透明模式下背景色的处理规则：

1. 若当前样式的 `BackgroundColor.Alpha < 255`（样式显式配置了透明），启动后**自动进入透明模式**，不需要按 F1；
2. 若 `Alpha == 255`（旧样式），需要按 F1 才进入透明模式；F1 的透明是“强制全透明”，不写回样式文件；
3. 样式里的背景图片（`BackgroundImageFileName`）在透明模式下仍然绘制，并保留 PNG 自身的 alpha（两种透明模式一致）。

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

### 3.1 per-pixel alpha（两种透明模式共用）

- 透明时给主窗口加 `WS_EX_LAYERED` 并用 `UpdateLayeredWindow(ULW_ALPHA, AC_SRC_ALPHA)` 呈现；
- 每一帧渲染到 `Bitmap(w, h, PixelFormat.Format32bppPArgb)`（预乘 alpha），再逐行拷贝到一个
  `CreateDIBSection` 创建的 32bpp top-down DIB 中交给 `UpdateLayeredWindow`；
- 整块窗口表面都由这张位图决定，所以透明时主窗口必须是无边框的（`FormBorderStyle.None`）；
- 键帽 PNG 的半透明像素、阴影、抗锯齿边缘都完整保留，桌面/游戏画面从透明像素透出，**没有颜色键硬边**。

### 3.2 原生边框窗口（有边框透明）

`NohBoard/NohBoard/Forms/OverlayFrameForm.cs` 是一个独立的工具窗口，只在
「透明且未按 F2 无边框」时显示：

- `FormBorderStyle.FixedSingle`，提供系统原生的标题栏、边框、最小化/关闭按钮；`ShowInTaskbar=false`
  （`WS_EX_TOOLWINDOW`，不出现在任务栏和 Alt+Tab），`ShowWithoutActivation=true`（显示时不抢焦点）；
- 客户区用 `TransparencyKey` 颜色键做成透明：`BackColor = TransparencyKey = RGB(1,0,0)`。**颜色键只作用于
  边框窗口空无一物的客户区**，让主窗口或桌面透出来；边框窗口本身不绘制任何键盘内容，因此不存在颜色键毛刺问题；
- 几何：`frame.ClientSize = main.Size`，再按实测的非客户区偏移把 frame 移到 `frame 客户区屏幕矩形 == main 窗口矩形`
  的位置（200% DPI 下精确对齐）；主窗口 `Move/Resize/DPI` 变化时同步 frame，拖动 frame 标题栏时反向同步主窗口；
- 最小化/关闭联动：frame 的最小化按钮最小化主窗口并隐藏自身，主窗口恢复时重新显示 frame；frame 的关闭按钮关闭主窗口；
  主窗口关闭时销毁 frame；
- 焦点：frame 获得焦点时，主窗口的透明区域仍然保持可点击（见 3.4），按 F1/F2 时 frame 把按键转发给主窗口；
- z 序：frame 显示后放在主窗口后面；用户点标题栏时 frame 会升到上面，但它的客户区是透明+点击穿透的，
  点击会落到主窗口上，视觉上仍然是一个整体。

### 3.3 渲染复用

原来的绘制逻辑从 `OnPaint` 抽成 `RenderScene(Graphics)`：

- 不透明模式：`OnPaint → Clear(背景色) → RenderScene(...)`；
- 透明模式（两种）：`RenderLayeredFrame → Clear(Transparent) → alpha=1 近透明底色（或样式 alpha） → 背景键帽图 → RenderScene(...) → UpdateLayeredWindow`；
- `UpdateTimer_Tick` 在透明模式下只在键盘/鼠标状态变化、编辑模式或拖动窗口时重绘，静止的覆盖层不占用渲染时间。

### 3.4 有焦点可点击、无焦点鼠标穿透

分层窗口（per-pixel alpha）对 alpha=0 的像素会做点击穿透，因此透明区域的点击原本会漏到桌面。为了满足
“透明时仍然可以点击拖动、但没有焦点时鼠标穿透”：

- 透明渲染时给整个窗口铺一层 **alpha=1 的近透明背景**（`Color.FromArgb(1, R, G, B)`，即约 0.4% 的着色，
  肉眼几乎不可见），这样所有像素 alpha>0，**整窗都可点击、可拖动**；
- `UpdateClickThrough()` 只在 **透明且整个窗口组都没有焦点**时给主窗口加 `WS_EX_TRANSPARENT`，鼠标事件全部穿透到下方窗口；
  窗口组 = 主窗口 + 边框窗口，因此点击原生标题栏后主窗口不会变成穿透状态；
- 窗口获得焦点（Alt+Tab、任务栏或点击标题栏）后，`OnActivated`/`OverlayFrameForm.OnActivated` 立即去掉该扩展样式，
  窗口恢复可点击、可拖动；
- `OnDeactivate` 以及 `UpdateTimer_Tick`（透明模式下每帧）都会调用 `UpdateClickThrough()`，保证焦点状态与
  扩展样式始终同步（避免 `Deactivate` 消息早于 WinForms 焦点状态更新导致的延迟）。

### 3.5 边框切换、位置补偿与最小化

`UpdateWindowBorder` 的规则：**透明时主窗口必须无边框**（per-pixel alpha 的要求），否则按 `borderlessMode`
和编辑模式决定 `FormBorderStyle`：

- 切换 `FormBorderStyle` 会改变窗口边框，因此先记录 `RectangleToScreen(ClientRectangle)`（客户区屏幕矩形），
  切换后再恢复 `ClientSize` 并用两次矩形差值补偿 `Location`，保证键位在屏幕上不跳动；
- 编辑模式会把主窗口设为 `Sizable`，但透明时仍强制为无边框；切换编辑模式后会重新应用一次 overlay 模式，
  保证边框窗口的显隐和几何正确；
- 窗口**不强制置顶**（`TopMost` 保持用户/系统默认值）：透明模式下窗口仍是普通窗口，可以被其它窗口覆盖，也可以最小化。
  最小化时 `RenderLayeredFrame` 与 `UpdateTimer_Tick` 直接返回，边框窗口隐藏；恢复后立即补画并重新显示边框窗口。

### 3.6 保持比例的窗口缩放

窗口没有使用系统的可缩放边框（`FixedSingle` / `FormBorderStyle.None`），因此在客户区四个角各留 18px 的
“缩放手柄”（`HitTestResizeCorner` / `TryStartWindowResize` / `ContinueWindowResize`）：

- **普通窗口、透明窗口、无边框窗口都生效**（per-pixel 渲染保证透明背景也可点击，不存在颜色键模式那种抓不到手柄的问题）；
- 鼠标移到角上时显示 `SizeNWSE` / `SizeNESW` 光标；
- 按住左键拖动只使用水平位移，按同一个比例同时改变宽高，**布局宽高比不变**（`OverlayScale` × 定义尺寸 × DPI）；
- 拖动左/上角时同步补偿窗口 `Location`，被拖角的对角保持不动；边框窗口跟随主窗口几何；
- 缩放范围限制在 0.25×–8×；松开后保存到 `NohBoard.json` 的 `OverlayScale`，下次启动恢复。

### 3.7 本地化（i18n）相关

菜单文本在 `InitializeOverlayFeatures`（构造函数阶段）里先使用英文原文 `Toggle &transparency (F1)` /
`Toggle &borderless window (F2)` / `Restart as &administrator` 作为本地化 key；此时 `Loc.Initialize` 还没按设置里的语言
初始化，所以不能在构造函数里调用 `Loc.T`（否则会先按系统语言翻译死，之后切不回英文）。翻译由 `MainForm_Load` 中
`Loc.Initialize(settings.Language)` 之后的 `Loc.Apply(this.MainMenu)` 统一完成。

## 4. 改动的文件

| 文件 | 改动 |
| --- | --- |
| `NohBoard/NohBoard/Forms/MainForm.Transparency.cs` | F1（透明）与 F2（无边框）解耦；透明时主窗口统一 ULW per-pixel；有边框透明时创建/同步/销毁 `OverlayFrameForm`；窗口组焦点判定与鼠标穿透；F1/F2 热键处理方法 `HandleOverlayHotkey`；边框切换、四角缩放、拖动窗口、状态持久化、最小化联动、管理员重启菜单 |
| `NohBoard/NohBoard/Forms/OverlayFrameForm.cs` | 新增：原生标题栏/边框工具窗口，客户区颜色键透明，几何/标题/图标与主窗口双向同步，最小化/关闭联动，焦点与 F1/F2 转发 |
| `NohBoard/NohBoard/Forms/MainForm.cs` | `OnPaint` 拆分出 `RenderScene`；`OnKeyDown` 局部热键改为调用 `HandleOverlayHotkey`（边框窗口也走同一逻辑）；`OnHandleCreated/OnHandleDestroyed/WndProc` 接 Raw Input；`UpdateTimer_Tick` 透明/最小化分支；启动时恢复状态；关闭时保存状态 |
| `NohBoard/NohBoard/Forms/MainForm.Editmode.cs` | 编辑模式切换后重新应用 overlay 模式（透明时保持无边框、边框窗口跟随）；编辑模式之外支持拖动透明/无边框窗口 |
| `NohBoard/Hooking/RawInputManager.cs` | 新增：注册 Raw Input（键盘/鼠标）、`WM_INPUT` 解析并桥接到 `KeyboardState` / `MouseState` |
| `NohBoard/Hooking/MouseState.cs` | 新增 `MouseFromCenter` 与 `RegisterRawMovement`（原始相对位移算速度） |
| `NohBoard/NohBoard/Extra/SerializableColor.cs` | 新增可选 `Alpha`（`DataMember`，`EmitDefaultValue=false`），`IsTransparent` |
| `NohBoard/NohBoard/Extra/GlobalSettings.cs` | 新增 `TransparentMode` / `BorderlessMode` / `OverlayScale` 持久化设置，以及 i18n 的 `Language` |

> Raw Input 与输入捕获的细节见 [input-capture-and-hotkeys.md](input-capture-and-hotkeys.md)。

## 5. 测试与验证

测试样板：`D:\soft\e-game\NohBoard-ReWrite-v1.3.0\`（`NohBoard.json` + `keyboards/`，类别 `Input Overlay`，
布局 `Input overlay layout`，样式 `Input overlay style`）。测试环境里在 NohBoard 窗口后面放一个全屏纯洋红色
（magenta）窗口，按窗口区域统计像素占比：不透明时背景是黑色，透明生效时背景像素应变成洋红色。
本机为 Windows 11、200% 缩放（物理 3840×2160），截图由 DPI aware 的 helper 完成。

`artifacts/lead-frame-test.ps1` 自动化验证（26/26 断言通过）：

| 断言 | 结果 |
| --- | --- |
| 透明主窗口无边框且 `WS_EX_LAYERED` 设置 | 是（1308×696，无边框） |
| 边框窗口存在、是 tool window、客户区屏幕矩形 == 主窗口矩形 | 是（误差 ≤1px，200% DPI） |
| 客户区背景透明 | 洋红 86.1%、黑色 0% |
| 原生标题栏可见、未被透明化 | 标题栏条带洋红 1.8% |
| **有边框透明 vs 无边框透明 渲染逐像素对比** | **meanDiff=0，max=0，0/56898 采样点不同** |
| 点击透明客户区后焦点保持在窗口组内 | 是（前景为主窗口，未落到桌面/背景窗口） |
| 在客户区按住左键拖动 | 窗口移动 (160,80)，与注入位移完全一致 |
| 拖动后边框窗口跟随 | 客户区屏幕矩形与主窗口矩形仍精确对齐 |
| 右键点击透明客户区 | 上下文菜单弹出（338×428） |
| 拖动原生标题栏 | 主窗口移动 (-180,60)，边框窗口保持对齐 |
| F2 切无边框 | 边框窗口隐藏，主窗口无边框 |
| F1 关闭 | 边框窗口隐藏，原生边框恢复（标题栏高 71px） |
| 最小化/恢复 | 边框窗口随主窗口隐藏/恢复且保持对齐 |
| 程序退出 | 无残留窗口/进程 |

其他历史验证（`artifacts/test-input-features.ps1`，全部断言通过）：无边框拖动跟手、保持比例缩放、
无焦点鼠标穿透、alpha=1 底垫使整窗可点击、后台按键/最小化恢复、非全局热键等。

自动化脚本见 `artifacts/lead-frame-test.ps1`（本地测试用，未提交；DPI/打包见 [dpi-and-packaging.md](dpi-and-packaging.md)）。

## 6. 已知限制

- 仅 Windows；`UpdateLayeredWindow` 在部分远程桌面/虚拟机/旧显卡驱动上可能表现为黑底（系统限制）。
- **有边框透明是“主窗口 + 独立边框窗口”两个窗口**：两者会同步移动/缩放/最小化，视觉上是一个整体；
  边框窗口是工具窗口，不出现在任务栏和 Alt+Tab。OBS 采集时请采集**主窗口**（它和无边框模式一样是 per-pixel，
  可保留透明）；只采集边框窗口本身时画面里只有标题栏/边框。
- 无边框透明窗口没有标题栏；移动窗口需要在**非编辑模式**下按住左键拖动客户区（避开四角缩放手柄）。
- **F1/F2 只在 NohBoard 拥有焦点时生效**（有意设计，不再作为全局热键）；透明窗口在无焦点时客户区鼠标穿透，
  需要用 Alt+Tab、任务栏或点击原生标题栏重新聚焦。
- 窗口不置顶，是普通窗口；需要压在游戏上方时请使用游戏/直播软件的置顶设置，或把窗口放在合适位置。
- 最小化期间不渲染（但输入状态仍在更新），恢复后立即补画；任务栏里仍可看到普通窗口。
- F1 是强制全透明，会忽略样式背景色的 alpha；若希望透明程度由配置决定，请使用样式 `Alpha` 字段。透明渲染会额外铺一层
  alpha=1 的近透明底色，肉眼几乎不可见，目的是让整窗可点击（分层窗口对 alpha=0 像素会穿透）。
- 缩放统一使用客户区四角手柄，不依赖系统边框；手柄区域 18px，与拖动窗口的区域互不冲突。
- 覆盖 UAC 提权窗口需要 NohBoard 本身以管理员运行；发布版 exe 的 manifest 已要求管理员权限（双击即弹 UAC）。
