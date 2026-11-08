# 03 · 编辑模式与交互流程

> 主要文件：`NohBoard/NohBoard/Forms/MainForm.Editmode.cs`（963 行）、`Forms/Properties/*.cs`、`Forms/Style/*.cs`、`Extra/GlobalSettings.ChangeTracking.cs`
> 前置阅读：01（渲染管线）、02（数据模型）

## 1. 编辑模式的本质

NohBoard 没有独立的编辑器窗口：编辑模式就是主窗口本身，切换方式为菜单 *Edit → Start/Stop Editing*（`MainForm.Editmode.cs:105`）。进入编辑后：

- `FormBorderStyle = Sizable`，允许拖边框改变画布大小；
- `ClientSize` 等于键盘定义的 `Width × Height`；
- 画面从“键帽按下的实时反馈”变成“元素轮廓 + 高亮 + 选中框”；
- 所有几何修改都**直接写回 `GlobalSettings.CurrentDefinition` 的新对象**，同时 `ResetBackBrushes()` 重建背景位图。

进入/退出编辑会清空选中与高亮状态。编辑模式下的窗口缩放会被解释成对键盘画布尺寸的修改（`MainForm_ResizeEnd`，`:793`，调用 `KeyboardDefinition.Resize(this.ClientSize)`），而不是普通的窗口拉伸。

## 2. 撤销/重做系统（`GlobalSettings.ChangeTracking.cs`）

撤销采用**整份快照**而不是命令模式：

```csharp
Stack<(KeyboardDefinition, KeyboardStyle, ChangeType)> undoHistory, redoHistory;
// CanUndo => undoHistory.Count > 1   （栈底是 InitUndoHistory 压入的初始布局）
```

| 操作 | 行为 |
|---|---|
| `InitUndoHistory()` | 清空两栈、清脏标记、压入 `(CurrentDefinition, CurrentStyle.Clone(), ChangeType.None)` |
| `UpdateDefinition(def, push)` | 替换 `CurrentDefinition`；`push=true` 时入栈 |
| `UpdateStyle(style, push)` | 替换 `CurrentStyle` |
| `UpdateBoth(def, style, push)` | 定义与样式一起替换（元素样式+定义同时变时用） |
| `PushUndoHistory` | **若新旧定义与样式都没变化则不压栈**（避免“只是点开表单”也产生一步撤销），压栈后清空 redo，并按 `ChangeType`（None/Definition/Style/Both）置脏标记 |
| `Undo()` / `Redo()` | 弹栈/回滚；回滚到栈底时清脏标记 |

`ChangeType` 决定 `UnsavedDefinitionChanges` / `UnsavedStyleChanges`，关闭窗口或切换键盘时据此弹“是否保存”确认。

## 3. 鼠标交互（`MainForm.Editmode.cs:138-300`）

| 手势 | 效果 |
|---|---|
| 左键点元素 | 选中（`selectedDefinition`），出现暗紫色 3px 选中框 |
| 左键拖动元素内部 | 整体平移（`Translate`） |
| 拖动多边形顶点（±4px） | 移动单个顶点（`MoveBoundary`） |
| 拖动边附近（<4px） | 沿该边法线方向移动边（`MoveEdge`，位移投影到正交向量） |
| `Alt` + 拖动 | 只移动文字（`MoveText`） |
| `Ctrl` + 拖动 | 强制只平移（`translateOnly`） |
| `Alt` + 点空白处拖动 | **整体平移整个键盘**（克隆定义后对所有元素 `Translate`，松开才提交） |
| 重叠元素上重复点击（未移动） | 在同位置元素间循环选择 |
| 右键/菜单 | 元素属性、样式、z 序、边界等 |

实现上的关键点（`:138-260`）：

1. `MouseDown` 先对**已选中元素**尝试 `StartManipulating`，失败再对光标下元素 `LastOrDefault`（最上层优先）。
2. 命中后先从 `Elements` 里 `RemoveElement`（`push=false`），拖动过程中把“正在操纵的元素”单独画在最上层，实现实时预览。
3. `MouseMove` 用**累积位移** `cumulManipulation` 调 `Manipulate`，因此一次拖动只产生一个 final 对象。
4. `MouseUp` 用 `AddElement(def, id)` 放回去，然后 `PushUndoHistory` 压一次栈；若本次 `cumul` 为 0（只是点击），则不压栈并执行“循环选择”。

## 4. 键盘交互（`MainForm.Editmode.cs:343-490`）

| 快捷键 | 效果 |
|---|---|
| 方向键 | 选中元素平移 1px（`Manipulate(new Size(±1,0))`；先移除再加回以保持 z 序，`push=true`） |
| `Esc` / `Enter` | 取消选中（Esc 也用于关闭菜单） |
| `Ctrl+Z` / `Ctrl+Shift+Z` | 撤销 / 重做；成功后同步 `ClientSize` 并 `ResetBackBrushes()` |
| `Ctrl+C` | 复制选中元素到内部剪贴板（`clipboard`） |
| `Ctrl+V` | 在鼠标位置粘贴：先保证落在 `validArea` 内，按剪贴板包围盒中心与光标的差平移，`SetId(GetNextId())` 后加入；若原元素有元素级样式，同时把样式复制到新 Id（`UpdateBoth`） |

## 5. 元素管理菜单

| 菜单 | 位置 | 行为 |
|---|---|---|
| Add Keyboard Key | `:505` | 40×40 方框（`Constants.DefaultElementSize`），空 `KeyCodes`，文本空，`ChangeOnCaps=true` |
| Add Mouse Key | `:532` | 默认键码 `MouseKeyCode.LeftButton` |
| Add Mouse Scroll | `:557` | 默认键码 `MouseScrollKeyCode.ScrollDown` |
| Add Mouse Speed Indicator | `:582` | 半径 20（`DefaultElementSize / 2`），圆心为当前鼠标位置 |
| Remove Element | `:596` | 移除元素并 `RemoveElementStyle(id)`（有元素级样式时用 `UpdateBoth`） |
| Move to Top / Up / Down / Bottom | `:288-340` | 通过 `MoveElementDown(def, int.MaxValue / 1 / -1 / int.MinValue)` 改列表顺序；列表顺序即绘制顺序（后面的盖前面的） |

四个 Add 菜单都走私有 `AddElement(:492)`，它在**非编辑态或当前有高亮元素**时直接 return（`:494-495`），避免误加。

## 6. 边界编辑

- 加边界点：`mnuAddBoundaryPoint`（`:633`）把 `currentManipulationPoint` 插入到最近边的下一个位置（`AddBoundary`）；
- 删边界点：`mnuRemoveBoundaryPoint`（`:655`）要求至少保留 3 个点，否则 MessageBox：`You cannot remove another boundary, there must be at least 3.`；
- 两个菜单只在元素当前操纵类型是 `MoveEdge` / `MoveBoundary` 时出现在右键菜单（`MainMenu_Opening` 里判断 `RelevantManipulation.Type`）；
- 矩形快捷入口：属性表单里的 “Rectangle” 按钮用 `TRectangle.FromPointList` 打开 `RectangleBoundaryForm`，确认后把多边形重写成 4 个角点（`Forms/Properties/KeyboardKeyPropertiesForm.cs`）。

## 7. 属性表单

`mnuElementProperties`（`:690`）按元素类型分派：

| 元素 | 表单 |
|---|---|
| `KeyboardKeyDefinition` | `Forms/Properties/KeyboardKeyPropertiesForm.cs` |
| `MouseKeyDefinition` / `MouseScrollDefinition` | `Forms/Properties/MouseElementPropertiesForm.cs` |
| `MouseSpeedIndicatorDefinition` | `Forms/Properties/MouseSpeedPropertiesForm.cs` |

共同模式：

- 表单持有一份 `initialDefinition`（原对象）和一份 `currentDefinition = (T)initialDefinition.Clone()`；
- 每次控件改动 → `Modify(...)` 生成新元素 → 触发 `DefinitionChanged` 回调；主窗体回调里 `RemoveElement` + `AddElement(index)`（`push=false`），实现**实时预览**；
- 点 OK → `DefinitionSaved`（主窗体 `UpdateDefinition(..., push=true)`）；点 Cancel → `DefinitionChanged(initialDefinition)` 回滚到原状。

`KeyboardKeyPropertiesForm` 的细节：

- 键码列表 `lstKeyCodes` 支持增删；`btnDetectKeyCode` 进入“按键捕获”模式：临时设置 `HookManager.KeyboardInsert = code => { udKeyCode.Value = code; return true; }`，捕获到即拦截该按键，关闭表单时把 `KeyboardInsert` 置回 `null`；
- 边界列表 `lstBoundaries` 支持增/改/删/上移/下移，直接写回 `Boundaries`；
- 文本 / Shift 文本 / `ChangeOnCaps` / `TextPosition`（`btnCenterText` 取包围盒中心）。

`mnuKeyboardProperties`（`:763`）打开 `KeyboardPropertiesForm` 改名称/分类/尺寸，尺寸变化后主窗体把 `ClientSize` 同步为定义大小。

## 8. 样式编辑

样式入口都要求**已经加载或保存过一个样式**（`mnuEditElementStyle` 会检查 `GlobalSettings.CurrentStyle` 是否来自真实文件，否则提示 `Please load or save a style before editing element styles.`）：

| 菜单 | 表单 | 作用 |
|---|---|---|
| Edit Element Style | `Forms/Style/KeyStyleForm.cs` 或 `MouseSpeedStyleForm.cs` | 编辑选中元素的元素级样式；若元素没有元素级样式，可“Remove element style”回退到默认 |
| Edit Keyboard Style | `Forms/Style/KeyboardStyleForm.cs` | 编辑背景色/背景图/默认键样式/默认速度指示器样式 |
| Save Style To Name / Save To Global Style / Save Style As | `SaveStyleAsForm` 等 | 写回 `.style` 文件；全局样式（`IsGlobal`，无元素级覆盖）存到 `keyboards/global/` |

`KeySubStylePanel` / `MouseSpeedStylePanel`（`Controls/`）是可复用的颜色+字体编辑面板，内部用 `ColorChooser` / `FontChooser`。

## 9. 文件保存与删除

- 定义保存：`KeyboardDefinition.Save()` 写 `keyboards/<Category>/<Name>/keyboard.json`；名称/分类由键盘属性表单决定，保存时改名会移动到新目录（由 `SaveKeyboardAsForm` 处理）。
- 删除键盘：`LoadKeyboardForm` 的删除按钮会 `FileHelper.FromKbs(category, name).Delete(true)` 整个目录（带确认框）。
- 旧格式导入：`LoadKeyboardForm` 的 “Load legacy” 按钮解析 `.kb` 后只注入内存，不落盘；要持久化需再 Save。
- 未保存保护：`MainForm_FormClosing` 在 `UnsavedDefinitionChanges || UnsavedStyleChanges` 时询问；`CrashHandler.Crashed` 时跳过（避免崩溃循环）。

## 10. 已知粗糙点（改代码前值得知道）

1. **编辑操作是“修改即预览、松开才入栈”**，如果中途异常/取消，模型可能停在预览态；`MouseUp` 之外没有兜底恢复路径。
2. **样式失效靠 `StyleDependencyCounter`**：任何样式改动都要记得调用 `ResetBackBrushes()`；`KeySubStyle.GetBackgroundBrush` 里自己判断 `StyleVersion`，两套机制并存。
3. **元素 Id 与样式字典的同步全靠手工**：删除元素不删样式会留下孤儿样式（`RemoveElement` 菜单替你做了，但 `MainForm.Editmode.cs` 里其它路径不一定）。
4. **`Forms/` 与 `Controls/` 下各有一套 `ColorChooser` / `FontChooser`**，历史遗留重复实现，改一处别以为改了两处。
5. **没有撤销步数上限**：大键盘 + 长时间编辑会让内存持续增长（每步是整份定义快照）。
6. **字符串全部硬编码英文**，没有本地化资源。
