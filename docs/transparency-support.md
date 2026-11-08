# 透明背景与无边框覆盖层（add-transparency-support 分支）

本文描述 `add-transparency-support` 分支新增的透明覆盖层能力：**F1 只切换背景透明**、**F2 只切换无边框窗口**，
两者互相独立，因此有四种组合：不透明有边框（普通窗口）、透明有边框、不透明无边框、透明无边框。

透明有两种不同的实现，取决于窗口是否保留原生边框：

| 组合 | 透明实现 |
| --- | --- |
| 有边框 + 透明 | **颜色键透明**（`Form.TransparencyKey` / `LWA_COLORKEY`）：客户区背景填近黑色 key，系统把 key 色变成透明；标题栏和边框由系统原生绘制，保持不变 |
| 无边框 + 透明 | **per-pixel alpha**（`UpdateLayeredWindow`，沿用原实现）：键帽 PNG 的 alpha 通道原样保留，质量最好 |
| 无边框 + 不透明 | 普通 GDI+ 绘制，只是没有标题栏/边框 |
| 有边框 + 不透明 | 普通窗口 |

> 为什么有边框时不能用 per-pixel alpha：`UpdateLayeredWindow` 的逐像素 alpha 会**替换整个窗口表面**，
> 连同系统绘制的标题栏/边框一起替换掉，所以「per-pixel 透明 + 原生边框」在 Windows 上不可行。
> 颜色键透明则由 DWM 在合成时按颜色做键控，非客户区不受影响，因此能保留原生标题栏和边框。

## 1. 功能

| 功能 | 触发方式 | 说明 |
| --- | --- | --- |
| 透明模式 | `F1` / 右键菜单 **Toggle transparency (F1)** | 只切换透明，不再隐含无边框。有边框时用颜色键透明（样式背景色被忽略、客户区背景透出桌面，标题栏/边框保留）；无边框时用 per-pixel alpha。**仅在窗口有焦点时生效**，不是全局热键 |
| 无边框模式 | `F2` / 右键菜单 **Toggle borderless window (F2)** | 只切换标题栏/边框，不改变透明状态；与 F1 完全独立。**仅在窗口有焦点时生效** |
| 拖动窗口 | 无边框状态下按住鼠标左键拖动（非四角区域）；有边框透明模式用原生标题栏拖动 | 记录 `Cursor.Position` 与 `Location` 的差值，逐帧移动；窗口位移与光标位移一致 |
| 缩放窗口 | 普通窗口与无边框窗口的客户区四角手柄 | **保持布局宽高比**，缩放比例持久化到 `NohBoard.json` 的 `OverlayScale`；拖角时光标显示斜向双箭头。**颜色键透明模式（有边框透明）下四角手柄不可用**，见已知限制 |
| 点击穿透 | 仅「无边框 + 透明（per-pixel）」且窗口**没有焦点**时自动生效 | 鼠标事件穿透到下方窗口；窗口获得焦点（Alt+Tab/任务栏）后恢复可点击、可拖动 |
| 整窗可点击 | 仅 per-pixel 透明渲染 | 每帧铺一层 alpha=1 的近透明底色（肉眼几乎不可见），使原本 alpha=0 的透明区域也可点击、可拖动 |
| 颜色键模式的鼠标行为 | 有边框 + 透明 | 不加 `WS_EX_TRANSPARENT`（否则标题栏都点不到）；客户区中被键控成透明的背景由系统按分层窗口规则穿透，键帽、标题栏和边框仍可点击 |
| 状态记忆 | 关闭程序时 | `TransparentMode` / `BorderlessMode` / `OverlayScale` 写入 `NohBoard.json`，重启后按新规则恢复 |
| 普通窗口 | — | 不强制置顶（不使用 `TopMost`）；最小化时暂停渲染、状态继续更新，恢复后立即补画 |

F1/F2 是窗口内热键（`MainForm.OnKeyDown`）而不是全局低级钩子事件，因为用户明确要求“快捷键不要全局”：
这样只有 NohBoard 拥有焦点时才会切换模式，不会误伤正在运行的游戏或其它程序。

透明模式下背景色的处理规则：

1. 若当前样式的 `BackgroundColor.Alpha < 255`（样式显式配置了透明），启动后**自动进入透明模式**，不需要按 F1；
2. 若 `Alpha == 255`（旧样式），需要按 F1 才进入透明模式；F1 的透明是“强制全透明”，不写回样式文件；
3. 样式里的背景图片（`BackgroundImageFileName`）在透明模式下仍然绘制：per-pixel 模式保留 PNG 自身的 alpha；
   颜色键模式中 PNG 的透明像素会显示为 key 色并被键控掉，半透明像素与近黑底色混合成深色描边。

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

### 3.1 两种透明实现

#### 3.1.1 per-pixel alpha（无边框透明）

- 给窗口加 `WS_EX_LAYERED` 并用 `UpdateLayeredWindow(ULW_ALPHA, AC_SRC_ALPHA)` 呈现；
- 每一帧渲染到 `Bitmap(w, h, PixelFormat.Format32bppPArgb)`（预乘 alpha），再逐行拷贝到一个
  `CreateDIBSection` 创建的 32bpp top-down DIB 中交给 `UpdateLayeredWindow`；
- 整块窗口表面都由这张位图决定，所以只能用于无边框窗口；PNG 的半透明像素能完整表达。

#### 3.1.2 颜色键透明（有边框透明）

- 有边框透明时**不使用** `UpdateLayeredWindow`，而是设置 `Form.TransparencyKey`，WinForms 会把窗口变成
  带 `LWA_COLORKEY` 的分层窗口，并用系统 `SetLayeredWindowAttributes` 设置颜色键；
- 客户区背景统一填 `ColorKeyTransparencyColor = Color.FromArgb(255, 1, 0, 0)`（RGB 近黑，不是纯黑：
  纯黑会把黑色文字也一起键控掉；RGB 1,0,0 与黑色文字只差 1/255，黑色文字仍然可见）；
- 键帽仍按正常 GDI+ 绘制在客户区上，半透明像素与近黑底色混合成深色描边（视觉可接受）；
- 标题栏、边框由系统原生绘制，不参与客户区绘制，因此保持原样；
- **模式切换顺序**很重要：调用过 `SetLayeredWindowAttributes` 之后 `UpdateLayeredWindow` 会失败，必须先清除
  `WS_EX_LAYERED` 样式位再重新设置（`DisableLayeredWindow` / `EnableLayeredWindow` 已做，`EnableLayeredWindow`
  在样式位已存在时会先清除再设置一次，确保颜色键属性被重置）；切换到颜色键模式时也先移除 ULW 的分层样式，
  再由 WinForms 重新设置带颜色键的分层窗口。
- 菜单文本在 `InitializeOverlayFeatures`（构造函数阶段）里先使用英文原文 `Toggle &transparency (F1)` /
  `Toggle &borderless window (F2)` / `Restart as &administrator` 作为本地化 key；此时 `Loc.Initialize` 还没按设置里的语言
  初始化，所以不能在构造函数里调用 `Loc.T`（否则会先按系统语言翻译死，之后切不回英文）。翻译由 `MainForm_Load` 中
  `Loc.Initialize(settings.Language)` 之后的 `Loc.Apply(this.MainMenu)` 统一完成。

### 3.2 渲染复用

原来的绘制逻辑从 `OnPaint` 抽成 `RenderScene(Graphics)`：

- 不透明模式：`OnPaint → Clear(背景色) → RenderScene(...)`；
- per-pixel 透明模式：`RenderLayeredFrame → Clear(Transparent) → alpha=1 近透明底色（或样式 alpha） → 背景键帽图 → RenderScene(...) → UpdateLayeredWindow`；
- 颜色键透明模式：走普通 `OnPaint`，`Clear(ClientBackgroundColor)` 时 `ClientBackgroundColor` 返回 key 色；
- `UpdateTimer_Tick` 在 per-pixel 透明模式下只在键盘/鼠标状态变化、编辑模式或拖动窗口时重绘，静止的覆盖层不占用渲染时间；
  颜色键模式下与普通窗口一样按状态 `Refresh()`。

### 3.3 热键（仅窗口内生效）

F1/F2 由 `MainForm.OnKeyDown` 处理（`e.Handled = e.SuppressKeyPress = true`），不再注册全局低级钩子事件：

- 只有 NohBoard 窗口拥有键盘焦点时热键才生效，游戏/其它程序里的 F1/F2 不受影响（用户要求“不要全局”）；
- 旧的 `HookManager.KeyEvent` 事件已从键盘钩子回调中移除，钩子现在只负责把按键状态喂给 `KeyboardState`；
- 属性表单“检测键码”时不影响，因为热键走的是窗口消息而不是钩子。

### 3.4 无边框、位置补偿与最小化

`UpdateWindowBorder` 只在 `borderlessMode` 为真时去掉边框（不再由透明模式隐含）：

- 切换 `FormBorderStyle` 会改变窗口边框，因此先记录 `RectangleToScreen(ClientRectangle)`（客户区屏幕矩形），
  切换后再恢复 `ClientSize` 并用两次矩形差值补偿 `Location`，保证键位在屏幕上不跳动；
- 编辑模式会把 `FormBorderStyle` 设为 `Sizable`，离开编辑模式恢复 `FixedSingle`，`originalBorderStyle` 记录切换前的样式；
- 窗口**不再强制置顶**（`TopMost` 保持用户/系统默认值）：透明模式下窗口仍是普通窗口，可以被其它窗口覆盖，也可以最小化。
  最小化时 `RenderLayeredFrame` 与 `UpdateTimer_Tick` 直接返回；键盘/鼠标状态照常更新，恢复窗口后下一帧立即补画。

### 3.5 保持比例的窗口缩放（普通窗口与无边框窗口都可以）

窗口没有使用系统的可缩放边框（`FixedSingle` / `FormBorderStyle.None`），因此在客户区四个角各留 18px 的
“缩放手柄”（`HitTestResizeCorner` / `TryStartWindowResize` / `ContinueWindowResize`）：

- **普通窗口和不透明/无边框窗口都生效**，无需先按 F2；
- **颜色键透明模式（有边框透明）下不生效**：该模式中客户区背景已被键控成透明、鼠标会穿透到下方窗口，四角手柄无法抓取；
  此时 `HitTestResizeCorner` 直接返回 -1，不显示误导性的缩放光标；需要缩放时可按 F2 切到无边框模式，或使用原生窗口边框；
- 鼠标移到角上时显示 `SizeNWSE` / `SizeNESW` 光标；
- 按住左键拖动只使用水平位移，按同一个比例同时改变宽高，**布局宽高比不变**（`OverlayScale` × 定义尺寸 × DPI）；
- 拖动左/上角时同步补偿窗口 `Location`，被拖角的对角保持不动；
- 缩放范围限制在 0.25×–8×；松开后保存到 `NohBoard.json` 的 `OverlayScale`，下次启动恢复。

### 3.6 有焦点可点击、无焦点鼠标穿透（仅 per-pixel 无边框模式）

分层窗口（per-pixel alpha）对 alpha=0 的像素会做点击穿透，因此透明区域的点击原本会漏到桌面。为了满足
“透明时仍然可以点击拖动、但没有焦点时鼠标穿透”：

- 透明渲染时给整个窗口铺一层 **alpha=1 的近透明背景**（`Color.FromArgb(1, R, G, B)`，即约 0.4% 的着色，
  肉眼几乎不可见），这样所有像素 alpha>0，**整窗都可点击、可拖动**；
- `UpdateClickThrough()` 只在 **per-pixel 分层模式且没有焦点**时给窗口加上 `WS_EX_TRANSPARENT`，鼠标事件全部穿透到下方窗口；
- 窗口获得焦点（Alt+Tab 或任务栏选中）后，`OnActivated` 立即去掉该扩展样式，窗口恢复可点击、可拖动；
- 颜色键透明模式**不会**加 `WS_EX_TRANSPARENT`：该模式必须保留标题栏和键帽的点击能力，透明背景本身由系统按颜色键穿透；
- `OnDeactivate` 以及 `UpdateTimer_Tick`（per-pixel 模式下每帧）都会调用 `UpdateClickThrough()`，保证焦点状态与
  扩展样式始终同步（避免 `Deactivate` 消息早于 WinForms 焦点状态更新导致的延迟）。

## 4. 改动的文件

| 文件 | 改动 |
| --- | --- |
| `NohBoard/NohBoard/Forms/MainForm.Transparency.cs` | F1（透明）与 F2（无边框）解耦；有边框透明用颜色键（`TransparencyKey` + 近黑 key 色），无边框透明用 ULW per-pixel；模式切换时先清除再设置 `WS_EX_LAYERED`；颜色键模式禁用四角缩放命中、不加 `WS_EX_TRANSPARENT`；菜单文本走 `Loc.T`；无边框切换、四角缩放、拖动窗口、状态持久化、最小化早退、管理员重启菜单 |
| `NohBoard/NohBoard/Forms/MainForm.cs` | `OnPaint` 拆分出 `RenderScene` 并改用 `ClientBackgroundColor`；`OnKeyDown` 局部热键；`OnHandleCreated/OnHandleDestroyed/WndProc` 接 Raw Input；`UpdateTimer_Tick` 透明/最小化分支；启动时恢复状态；关闭时保存状态 |
| `NohBoard/NohBoard/Forms/MainForm.Editmode.cs` | 编辑模式之外支持拖动无边框窗口 |
| `NohBoard/Hooking/RawInputManager.cs` | 新增：注册 Raw Input（键盘/鼠标）、`WM_INPUT` 解析并桥接到 `KeyboardState` / `MouseState` |
| `NohBoard/Hooking/MouseState.cs` | 新增 `MouseFromCenter` 与 `RegisterRawMovement`（原始相对位移算速度） |
| `NohBoard/NohBoard/Extra/SerializableColor.cs` | 新增可选 `Alpha`（`DataMember`，`EmitDefaultValue=false`），`IsTransparent` |
| `NohBoard/NohBoard/Extra/GlobalSettings.cs` | 新增 `TransparentMode` / `BorderlessMode` / `OverlayScale` 持久化设置 |

> Raw Input 与输入捕获的细节见 [input-capture-and-hotkeys.md](input-capture-and-hotkeys.md)。

## 5. 测试与验证

测试样板：`D:\soft\e-game\NohBoard-ReWrite-v1.3.0\`（`NohBoard.json` + `keyboards/`，类别 `Input Overlay`，
布局 `Input overlay layout`，样式 `Input overlay style`）。测试环境里在 NohBoard 窗口后面放一个全屏纯洋红色
（magenta）窗口，按窗口区域统计像素占比：不透明时背景是黑色，透明生效时背景像素应变成洋红色。
本机为 Windows 11、200% 缩放（物理 3840×2160），截图由 DPI aware 的 helper 完成。

`artifacts/test-f1-border.ps1` 自动化验证四种组合（12:00 一轮，44/44 断言通过）：

| 步骤 | 窗口矩形 | 客户区 | 边框/标题栏高度 | layered | 客户区黑色像素 | 客户区洋红像素 |
| --- | --- | --- | --- | --- | --- | --- |
| A 不透明 + 有边框 | 1334×767 | 1308×696 | 71px | 否 | 77.8% | 2.5% |
| B 按 F1：透明 + 有边框 | **1334×767（不变）** | 1308×696（不变） | **71px（标题栏仍在）** | 是（颜色键） | **1.4%** | **85.5%** |
| A2 再按 F1：不透明 + 有边框 | 1334×767 | 1308×696 | 71px | 否 | 77.8% | 2.5% |
| D F1+F2：透明 + 无边框 | 1308×696（=客户区） | 1308×696 | 0 | 是（per-pixel） | **0%** | **86.1%** |
| B3 再按 F2：透明 + 有边框 | 1334×767 | 1308×696 | 71px | 是（颜色键） | — | 85.5% |
| C F2：不透明 + 无边框 | 1308×696（=客户区） | 1308×696 | 0 | 否 | 86.9% | 0% |
| 关闭时持久化 | — | — | — | — | — | `TransparentMode=true` / `BorderlessMode=false` |

其他断言与验证：

- B（有边框透明）时**窗口矩形/客户区尺寸与不透明状态完全相同**，`borderHeight=71` 说明标题栏仍在；
  标题栏条带（窗口顶部、客户区上方）洋红像素占比 **0%**，说明标题栏是系统绘制、没有被透明化；
- B 状态 `WS_EX_LAYERED` 已设置、`WS_EX_TRANSPARENT` 未设置（标题栏可点击）；
- D（无边框透明）窗口矩形等于客户区（无标题栏），客户区洋红 86.1%、黑色 0%，键帽像素仍在（Other 13.9%）；
- 颜色键模式截图 `artifacts/f1-test/desktop-02-transparent-bordered-crop.png` 可直接看到：原生标题栏
  “NohBoard v1.5.0”、原生边框、洋红背景透出、白色键帽与黑色文字均可见；
- 无边框 per-pixel 透明截图 `desktop-04-transparent-borderless-crop.png`：无标题栏、洋红背景、键帽半透明边缘正常；
- 其他历史验证（`artifacts/test-input-features.ps1`，全部断言通过）：无边框拖动跟手、保持比例缩放、
  无焦点鼠标穿透、alpha=1 底垫使整窗可点击、后台按键/最小化恢复、非全局热键等。

自动化脚本见 `artifacts/test-f1-border.ps1`（本地测试用，未提交；DPI/打包见 [dpi-and-packaging.md](dpi-and-packaging.md)）。

## 6. 已知限制

- 仅 Windows；`UpdateLayeredWindow` 在部分远程桌面/虚拟机/旧显卡驱动上可能表现为黑底（系统限制）。
- **有边框透明（颜色键模式）是二值透明**：颜色键只能做“全透明/不透明”，无法表达半透明像素。样式背景色会被忽略；
  键帽 PNG 的半透明边缘/阴影会与近黑底色（RGB 1,0,0）混合成深色描边，视觉上可接受但不是逐像素精确。
- **颜色键模式下四角缩放手柄不可用**：客户区背景被键控成透明后鼠标会穿透到下方窗口，抓不到手柄；需要缩放时可先按 F2
  切到无边框模式（per-pixel），或使用原生窗口边框/标题栏。移动窗口可直接拖动原生标题栏。
- 无边框透明窗口没有标题栏；移动窗口需要在**非编辑模式**下按住左键拖动（避开四角缩放手柄）。
- **F1/F2 只在 NohBoard 拥有焦点时生效**（有意设计，不再作为全局热键）；per-pixel 无边框透明模式下无焦点时窗口还会鼠标穿透，
  需要用 Alt+Tab 或任务栏重新聚焦。
- 窗口不置顶，是普通窗口；需要压在游戏上方时请使用游戏/直播软件的置顶设置，或把窗口放在合适位置。
- 最小化期间不渲染（但输入状态仍在更新），恢复后立即补画；任务栏里仍可看到普通窗口。
- F1 是强制全透明，会忽略样式背景色的 alpha；若希望透明程度由配置决定，请使用样式 `Alpha` 字段。per-pixel 透明渲染会额外铺一层
  alpha=1 的近透明底色，肉眼几乎不可见，目的是让整窗可点击（分层窗口对 alpha=0 像素会穿透）。
- 缩放统一使用客户区四角手柄（普通窗口与无边框窗口均可，颜色键透明模式除外），不依赖系统边框；手柄区域 18px，与拖动窗口的区域互不冲突。
- OBS 采集：per-pixel 分层窗口需要较新的 OBS 版本（窗口采集勾选“允许透明”/使用 Windows 10 1903+ 的捕获方式），否则可能采到黑底；
  颜色键模式按普通窗口采集即可（但注意背景会被键控成透明）。
- 覆盖 UAC 提权窗口需要 NohBoard 本身以管理员运行；发布版 exe 的 manifest 已要求管理员权限（双击即弹 UAC）。
