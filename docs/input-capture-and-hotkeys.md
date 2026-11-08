# 输入捕获与热键（add-transparency-support 分支）

本文说明本分支对输入捕获与窗口行为的调整，对应需求：

1. F1/F2 **不再作为全局热键**；
2. 按键捕获要更可靠（包括 NohBoard 不在前台/最小化时，以及游戏、UAC 提权窗口里的按键）；
3. 窗口不再强制置顶，作为普通窗口运行，最小化时也正常工作。

## 1. 双输入源：低级钩子 + Raw Input

NohBoard 原来只有一条输入路径：`WH_KEYBOARD_LL` / `WH_MOUSE_LL` 低级钩子（`Hooking/Interop/HookManager.cs`）。
问题是低级钩子在一些场景下收不到输入，典型如：

- 游戏使用 Raw Input 且处于独占全屏/高权限进程；
- 目标程序以管理员权限运行（UIPI 会拦截发给低完整性进程的输入消息）；
- 某些反作弊/安全软件会屏蔽低级钩子。

本分支新增 `NohBoard/Hooking/RawInputManager.cs`，用 `RegisterRawInputDevices` 注册键盘（UsagePage 0x01 / Usage 0x06）
与鼠标（Usage 0x02），标志 `RIDEV_INPUTSINK`，然后把 `WM_INPUT` 解析为 NohBoard 的键码：

| 输入 | 处理 |
| --- | --- |
| 键盘 | `MapVirtualKey(MakeCode \| (E0 ? 0x100 : 0), MAPVK_VSC_TO_VK_EX)` 得到左右修饰键的区分；E0+VK_RETURN → 伪键码 1025（小键盘回车）；E1 → VK_PAUSE |
| 鼠标按键 | `usButtonFlags` 的 DOWN/UP 位映射到 `MouseKeyCode`（左/右/中/X1/X2） |
| 滚轮 | `RI_MOUSE_WHEEL` 的 `usButtonData` 有符号值（±120）→ ScrollUp/ScrollDown；`RI_MOUSE_HWHEEL` → ScrollRight/ScrollLeft |
| 鼠标移动 | **必须用 `lLastX` / `lLastY` 相对位移**（游戏中指针锁定/隐藏时光标坐标不变），交给 `MouseState.RegisterRawMovement` 计算速度 |

两条路径最终都调用同一套状态接口（`KeyboardState.AddPressedElement/RemovePressedElement`、
`MouseState.AddPressedElement/RemovePressedElement/AddScrollDirection`、`MouseState.RegisterLocation`），
这些调用是幂等的，重复喂入不会产生错误状态。

## 2. 窗口与热键行为

- **F1/F2 只在窗口有焦点时生效**：在 `MainForm.OnKeyDown` 中处理，`e.Handled = e.SuppressKeyPress = true`；
  旧实现里的 `HookManager.KeyEvent` 全局事件已移除。
- **不置顶**：`FormBorderStyle` 切换逻辑不再设置 `TopMost`，窗口保持普通 z 序；透明模式下也一样。
- **最小化**：`UpdateTimer_Tick` 与 `RenderLayeredFrame` 在最小化时直接返回，不做无效渲染；输入状态仍持续更新，
  恢复窗口后立即补画（不会出现“最小化一段时间后画面停留在旧状态”）。
- **管理员重启**：菜单里新增 **Restart as administrator**（仅在当前非管理员时显示）。它不是默认行为，
  也不会修改 manifest；需要覆盖 UAC 提权窗口时手动点一次即可（会以当前 exe 路径 `Verb=runas` 重启）。

## 3. 为什么保留两条输入路径

Raw Input 与低级钩子各有盲区：

- Raw Input 收不到 `SendInput` 注入的按键（实测：注册成功后注入按键不产生 `WM_INPUT`，而钩子能收到），
  所以自动化测试/宏软件场景仍依赖钩子；
- 低级钩子受 UIPI/反作弊限制，且看不到游戏内的原始鼠标位移。

两者并存后，真实硬件输入通常会被两条路径同时收到（幂等，无害），某一源失效时另一源仍能工作。
自动化测试脚本（`artifacts/test-input-features.ps1`）因此使用低级钩子/`PostMessage` 路径验证功能，
真实游戏输入则由 Raw Input 兜底。

## 4. 验证

`artifacts/test-input-features.ps1` 在 200% DPI、`Input Overlay` 布局下自动化验证并全部通过：

| 断言 | 结果 |
| --- | --- |
| 窗口非置顶 | ✅ `WS_EX_TOPMOST` 未设置 |
| 无边框模式生效（1308×696，无标题栏） | ✅ |
| 拖动窗口跟手（窗口位移 = 光标位移 611×305） | ✅ |
| F1 透明（背景黑色占比 0%，洋红 86.1%） | ✅ |
| 后台按键可见（A 按下像素差 3385，松开后 0） | ✅ |
| 最小化期间按键在恢复后正确显示（像素差 3435） | ✅ |
| 焦点在其它窗口时按 F1，NohBoard 保持透明（非全局热键） | ✅ |
| 再按 F1 恢复不透明（黑色 86.9%） | ✅ |

## 5. 已知限制

- 提权（UAC）目标窗口：普通权限的 NohBoard 无法可靠获取其输入（Windows UIPI 限制），需 **Restart as administrator**；
- 独占全屏游戏：Raw Input 也受游戏自身捕获策略影响，通常需要以窗口化/无边框全屏运行游戏；
- 远程桌面/虚拟机环境下的输入与分层窗口行为可能受限；
- 热键仅在窗口聚焦时可用，全屏游戏时需 Alt+Tab 切回；
- `MouseState.MouseFromCenter` 模式在 Raw Input 路径下使用光标位置而非相对位移计算速度（该模式语义就是“相对屏幕中心”）。

## 6. 改动文件

见 [transparency-support.md §4](transparency-support.md) 的改动表；输入相关新增/修改主要集中在：
`NohBoard/Hooking/RawInputManager.cs`、`NohBoard/Hooking/MouseState.cs`、`NohBoard/NohBoard/Forms/MainForm.cs`、
`NohBoard/NohBoard/Forms/MainForm.Transparency.cs`。
