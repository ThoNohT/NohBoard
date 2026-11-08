# 04 · 输入钩子与键鼠状态

> 项目：`NohBoard/Hooking/`（netstandard2.1，独立程序集）
> 主要文件：`Interop/HookManager.cs`（263 行）、`Interop/HookManager.Public.cs`（138 行）、`Interop/Defines.cs`、`Interop/FunctionImports.cs`、`Interop/Structs.cs`、`KeyboardState.cs`、`MouseState.cs`、`StateBase.cs`、`CircleBuffer.cs`、`MouseKeyCode.cs`、`MouseScrollKeyCode.cs`

## 1. 为什么用低级钩子

NohBoard 需要**全局**捕获按键/鼠标，即使它自己不在前台。Windows 上可行方案是 `SetWindowsHookEx` + `WH_KEYBOARD_LL` / `WH_MOUSE_LL`：

```csharp
SetWindowsHookEx(WH_KEYBOARD_LL, proc, IntPtr.Zero, 0);  // 低级钩子，无需 DLL 注入
```

因为低级钩子在**安装钩子的线程**上被调用（通过该线程的消息循环），NohBoard 在 UI 线程安装钩子，回调就运行在 UI 线程，不存在跨线程调用 UI 的问题；`Hooking` 里的 `lock` 只是防御性的。

## 2. 钩子安装/卸载（`HookManager.Public.cs:29`）

```csharp
public static bool TrapMouse { get; set; }
public static bool TrapKeyboard { get; set; }
public static Func<int,bool> KeyboardInsert { get; set; }   // 返回 true = 吞掉该按键
public static int TrapToggleKeyCode = VK_SCROLL;            // Ctrl? 否：ScrollLock 切换拦截
public static int ScrollHold = 50;                          // 滚轮计数保持 ms
public static int PressHold = 0;                            // 按下后额外保持 ms
```

- `EnableMouseHook()` / `EnableKeyboardHook()`：`SetWindowsHookEx` 成功即记下 `IntPtr`；已在安装则直接返回（幂等）。
- `DisableMouseHook()` / `DisableKeyboardHook()`：`UnhookWindowsHookEx` 并把句柄置零。
- 失败抛 `Win32Exception(Marshal.GetLastWin32Error())`，调用方一般没有 catch —— 钩子装不上会直接崩（有 CrashHandler 兜底）。

钩子的启用是**按需的**：`MainForm.LoadKeyboard()` 只在定义里存在鼠标类元素时启用鼠标钩子、存在 `KeyboardKeyDefinition` 时启用键盘钩子。空键盘不会占用全局钩子。

## 3. 回调与事件路由

### 3.1 鼠标（`Interop/HookManager.cs:110`）

| 消息 | 处理 |
|---|---|
| `WM_MOUSEMOVE` | `MouseState.RegisterLocation(pt)`（速度采样） |
| `WM_LBUTTONDOWN/UP`、`WM_RBUTTONDOWN/UP`、`WM_MBUTTONDOWN/UP` | 映射到 `MouseKeyCode` 的按下/抬起 |
| `WM_XBUTTONDOWN/UP` | 解析 `HIWORD(MouseData)` 得到 X1/X2 |
| `WM_MOUSEWHEEL` | `HIWORD(MouseData)`：`120` = 上滚、`65416`（-120 的 WORD） = 下滚 |
| `WM_MOUSEHWHEEL` | 右/左滚 |

处理完：

```csharp
if (trapEnabled && TrapMouse) return 1;   // 吞掉事件，游戏/程序收不到
return CallNextHookEx(...);
```

### 3.2 键盘（`Interop/HookManager.cs:230`）

```csharp
int code = info.VirtualKeyCode;
if ((info.Flags & LLKHF_EXTENDED) != 0 && code == VK_RETURN) code = 1025; // 小键盘回车
switch (wParam) {
  case WM_KEYDOWN: case WM_SYSKEYDOWN:
      KeyboardState.AddPressedElement(code, PressHold); break;
  case WM_KEYUP: case WM_SYSKEYUP:
      KeyboardState.RemovePressedElement(code);
      if (code == TrapToggleKeyCode) trapEnabled = !trapEnabled; break;   // ScrollLock 切换拦截
}
if ((KeyboardInsert?.Invoke(code) ?? false) || (trapEnabled && TrapKeyboard)) return 1;
```

注意：`KeyboardInsert` 是编辑属性表单的“按键捕获”用的一次性回调，返回 true 表示“这个键被我吃了”，表单拿到键码后应复位为 null。

### 3.3 键码体系

| 键码 | 含义 |
|---|---|
| Windows `VK_*` | 普通按键（`Defines.cs` 内定义了常用常量） |
| `1025` | 小键盘 Enter（与主 Enter `VK_RETURN` 区分） |
| `1026` / `1027` / `1028` | NohBoard 自定义的伪键码，代表 CapsLock / NumLock / ScrollLock 的**激活状态**（`KeyboardState.StateKeys`） |

伪键码的意义：布局里可以把 CapsLock 键绑定到 1026，这样视觉上只反映“大写锁定是否激活”，而不是物理按键的按下瞬间。

键码 → 显示文本的映射在键盘 JSON 的 `Text`/`ShiftText` 里手写，代码里没有“VK → 名字”的通用表。

## 4. 状态模型（`StateBase.cs:28`）

```csharp
protected struct KeyPress { long startTime; bool removed; }
protected static readonly Dictionary<T, KeyPress> pressedKeys;
protected static bool updated;               // 读取时自动置 false
protected static Stopwatch keyHoldStopwatch;
```

- `AddPressedElement(key, hold)`：写入 `startTime`；若 `hold > 0` 则立即标记 `removed = true`，表示“松手后还要再显示 hold 毫秒”。
- `RemovePressedElement(key)`：把 `removed = true`；`hold == 0` 时直接删除。
- `CheckKeyHolds(hold)`：每帧/每秒清理 `removed && now >= startTime + hold` 的键。
- `PressedKeys`：`lock` 下返回快照（UI 渲染用）。
- `Updated`：读取后自动复位，是“按需重绘”的闸门。
- `EnsureStopwatchRunning` / `TryStopStopwatch`：有键按下才跑表，空闲时停表省电。

`PressHold` 的效果是**视觉保持**（按下后松开，仍然显示按下状态若干毫秒），不是真的延迟事件。

### 4.1 键盘状态（`KeyboardState.cs:28`）

- `ShiftDown` / `CtrlDown` / `AltDown`：判断左右键（`VK_LSHIFT`/`VK_RSHIFT` 等）；
- `CapsActive => (GetKeyState(VK_CAPITAL) & 0x1) != 0`：直接问系统，不维护自己的状态；
- `CheckKeys(hold)`：对字典里的每个键用 `KeyIsUp` 校验（物理键已抬起但钩子丢了 KEYUP 时清掉），每秒由 `KeyCheckTimer` 调一次，用于**防卡键**；
- `AddPressedElement` 覆写：若键码是状态键，调 `TryToggleStateKey` 切换伪键码 1026/1027/1028 的按下状态；
- `KeyIsUp` 对状态键取反（状态键的“按下”= 锁定激活，而不是键盘物理按下）。

### 4.2 鼠标状态（`MouseState.cs:33`）

```csharp
updateInterval = 33;          // ms，速度采样最小间隔
mouseSmooth   = 5;            // 速度滑动窗口样本数
mouseFromCenter = false;      // 相对屏幕中心（鼠标速度指示器用）
```

- **速度**：`RegisterLocation(location, time)` 用 `GetSpeed` 算 `dx/dt`（每秒像素），样本进 `CircleBuffer<SizeF>`（预填 5 个 0）；`AverageSpeed` 取平均。`CheckScrollAndMovement` 若窗口内全零会补 `(0,0)` 样本，让指针“渐回 0”而不是突变。
- **滚轮**：`scrollCounts[4]` + `scrollTimers[4]`：每收到一次滚轮，计数 +1 并重置计时器；`CheckScrollAndMovement` 清掉超过 `ScrollHold` 的计数；`ScrollCounts` 返回快照。
- **多显示器**：`ScreenCenters` 由 `MainForm`/`SettingsForm` 传入 `Screen.AllScreens` 的中心点；`mouseFromCenter` 时速度 = 相对最近屏幕中心的位置，而不是 `WM_MOUSEMOVE` 的绝对坐标。
- 鼠标键本身走 `StateBase<MouseKeyCode>` 的按下集合，与键盘同一套 `KeyPress` 模型。

## 5. 拦截（trap）语义

`TrapKeyboard` / `TrapMouse` 打开时，NohBoard 把事件 `return 1` 吞掉，等于**屏蔽**该输入（NohBoard 常被用来做“只显示在屏幕上、不让系统收到”的演示）。`trapEnabled` 默认 false，由 **ScrollLock 键**切换（`TrapToggleKeyCode` 可在设置里改成任意键）。设置界面里“Trap keyboard/mouse”的勾选框只是把 `TrapMouse/TrapKeyboard` 写进 `HookManager`，真正的开关仍是 ScrollLock。

注意副作用：低级钩子吞事件会影响**整个系统**（包括当前游戏），不是只影响 NohBoard 窗口。

## 6. 平台与权限限制

- 仅 Windows；`WH_*_LL` 自 XP 起可用。
- **UIPI**：非管理员进程的钩子收不到发往更高完整性级别（管理员）窗口的输入；NohBoard 想覆盖管理员权限的游戏需要自己也以管理员运行。
- **无注入过滤**：代码没有检查 `LLKHF_INJECTED` / `LLMHF_INJECTED`，因此脚本/宏注入的输入也会被显示（一般无害，但如果做反作弊/宏检测相关需求需自行过滤）。
- 不处理 Raw Input、不做键盘布局（`GetKeyboardLayout`）转换：中文/日文输入法状态下 `VK` 仍是物理键，`Text` 由布局文件作者手写。
- `WH_KEYBOARD_LL` 对 UAC 安全桌面（Ctrl+Alt+Del）不可见。

## 7. 扩展点

| 需求 | 改动位置 |
|---|---|
| 支持新鼠标键（如更多 XButton） | `MouseKeyCode` 枚举 + `HookManager` 消息映射 + 布局 JSON 的键码 |
| 支持组合键“全部按下” | 已支持：`KeyboardKeyDefinition.KeyCodes` 是列表，渲染时要求全部在按下集合中 |
| 增加“按下时长”精确统计 | `StateBase.KeyPress` 已存 `startTime`，可在回调/渲染读取 |
| 过滤注入输入 | `HookManager` 检查 `LLKHF_INJECTED`（键盘）/`LLMHF_INJECTED`（鼠标） |
| 换用 Raw Input / 更底层方案 | 需替换整个 `Hooking` 项目，`KeyboardState`/`MouseState` 的 API 面较小，替换成本可控 |
| DPI / 多屏坐标 | `MouseState.ScreenCenters` 与布局坐标都按像素；高 DPI 下窗口缩放会导致布局与光标错位（见 `dpi` 分支） |

## 8. 快速验证方法

1. 运行程序，打开任意键盘布局；
2. 按 ScrollLock → 键鼠输入被吞（系统无响应），再按恢复；
3. 设置里勾选 “Trap mouse/keyboard” 后，观察到 ScrollLock 仍然是总开关；
4. 编辑模式 → 元素属性 → “Detect key code”，按键能被捕获且不影响系统其它窗口（`KeyboardInsert` 返回 true）；
5. 把 `PressHold` 调到 200ms，观察松开后键帽保持高亮的时长。
