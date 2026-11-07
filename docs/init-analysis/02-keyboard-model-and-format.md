# 02 · 键盘数据模型与磁盘格式

> 核心文件：`NohBoard/NohBoard/Keyboard/**`、`NohBoard/NohBoard/Extra/FileHelper.cs`、`keyboards/**`

## 1. 模型总览

```
KeyboardDefinition                     (Keyboard/KeyboardDefinition.cs:33)
 ├─ Name / Category                   （不写入 JSON，由所在文件夹回填）
 ├─ Version : int                     （= Constants.KeyboardVersion = 2）
 ├─ Width / Height : int
 └─ Elements : List<ElementDefinition>
        └─ ElementDefinition (abstract, :30)
            ├─ Id : int
            ├─ StyleVersion / CurrentManipulation / PreviewManipulation
            ├─ GetBoundingBox / Inside / StartManipulating / Manipulate
            └─ KeyboardKeyDefinition / MouseKeyDefinition / MouseScrollDefinition / MouseSpeedIndicatorDefinition
                 └─ KeyDefinition (abstract, KeyDefinition.cs:36)
                      ├─ Boundaries : List<TPoint>   (任意多边形，顺时针/逆时针均可)
                      ├─ TextPosition : TPoint
                      ├─ KeyCodes : List<int>
                      └─ Text : string
```

要点：

- **元素不是矩形，而是任意多边形**。`KeyboardKeyDefinition`/`MouseKeyDefinition`/`MouseScrollDefinition` 都继承 `KeyDefinition`，用顶点列表 `Boundaries` 描述形状；`MouseSpeedIndicatorDefinition` 是唯一的非 `KeyDefinition` 元素（圆，`Location + Radius`）。
- **坐标是键盘画布坐标系**（左上角为原点，单位像素），最终窗口大小 = `Width × Height`。编辑模式下拖动窗口边框会改写定义尺寸（`MainForm.Editmode.cs:793`）。
- **Id 在定义内唯一**（`KeyboardDefinition.Load` 会校验，重复会抛异常），撤销、样式覆盖、剪贴板粘贴都依赖它。
- `KeyboardKeyDefinition` 额外有 `ShiftText`、`ChangeOnCaps`；鼠标键/滚轮键复用 `KeyCodes`（装 `MouseKeyCode` / `MouseScrollKeyCode` 的枚举值），所以鼠标键本质上是“键码为鼠标键码的 KeyDefinition”。

## 2. 元素类型逐个看

### 2.1 `KeyboardKeyDefinition`（`Keyboard/ElementDefinitions/KeyboardKeyDefinition.cs`）

| 字段 | 含义 |
|---|---|
| `Boundaries` | 多边形顶点（继承） |
| `KeyCodes` | 触发它的虚拟键码列表，**全部按下**才算按下（支持组合键，如 Shift+A） |
| `Text` / `ShiftText` | 普通文案 / Shift 文案（`ChangeOnCaps` 控制 Caps 是否也切到 ShiftText） |
| `TextPosition` | 文案中心点（绝对坐标） |

渲染 `Render(Graphics, pressed, shift, caps)`（`:86`）：取元素级 `KeyStyle` 或 `DefaultKeyStyle` → 选 `Loose`/`Pressed` 子样式 → `GetBackgroundBrush(subStyle, pressed)` 填充多边形 → `DrawString`（`SetClip` 限制在 `GetBoundingBox()` 内）→ 可选 `ShowOutline` 描边。

文本选择逻辑在 `GetText(shift, capsLock)`（`:359`）：

```
若 Capitalization == FollowKeys:        capitalize = ChangeOnCaps ? (caps ^ shift) : shift
否则（AlwaysCaps / AlwaysLower）:       caps 固定；
  preserveShift = (FollowShiftForCapsInsensitive && !ChangeOnCaps)
               || (FollowShiftForCapsSensitive && ChangeOnCaps)
  shift &= preserveShift
```

编辑模式的几何操作（`StartManipulating` 继承自 `KeyDefinition`）：顶点点中（±4px）→ 移动顶点；边附近（<4px）→ 移动边（正交投影）；按住 Alt → 只移动文案；否则整体平移；Ctrl 强制只平移。加点/删点分别通过右键菜单（要求至少保留 3 个点）。

### 2.2 `MouseKeyDefinition`（`MouseKeyDefinition.cs:63`）

单键码鼠标键（左/右/中/X1/X2，见 `Hooking/MouseKeyCode.cs`），渲染与键盘键基本一致，但没有 ShiftText。

### 2.3 `MouseScrollDefinition`（`MouseScrollDefinition.cs:61`）

`Render(Graphics, int scrollCount)`：显示滚轮次数（滚轮上下/左右，键码见 `Hooking/MouseScrollKeyCode.cs`）。滚动计数由 `MouseState` 按 `ScrollHold` 毫秒衰减。

### 2.4 `MouseSpeedIndicatorDefinition`（`MouseSpeedIndicatorDefinition.cs:82`）

以 `Location` 为圆心、`Radius` 为半径的指示器：外圈颜色固定；指针长度 = `min(Radius, sensitivity * speed * Radius)`，方向为速度向量角度；指针用 `FillPie` + `LinearGradientBrush` 从内色渐变到外色，并在外缘画一个小圆点。无速度时只画内圈。编辑模式下拖外缘（距圆周 <16px）为缩放，其余为移动（`:246-279`）。

## 3. 样式模型

```
KeyboardStyle                     (KeyboardKeyboardStyle.cs:33)
 ├─ BackgroundColor : SerializableColor        (默认 0,0,100)
 ├─ BackgroundImageFileName : string           (相对 keyboards/…/images)
 ├─ DefaultKeyStyle : KeyStyle
 ├─ DefaultMouseSpeedIndicatorStyle : MouseSpeedIndicatorStyle
 └─ ElementStyles : Dictionary<int, ElementStyle>   (元素 Id → 覆盖样式)

KeyStyle : ElementStyle          (Styles/KeyStyle.cs:27)
 ├─ Loose   : KeySubStyle        (默认背景 100,100,100)
 └─ Pressed : KeySubStyle        (默认背景白)

KeySubStyle                      (Styles/KeySubStyle.cs:29)
 ├─ Background / Text / Outline : SerializableColor
 ├─ ShowOutline : bool, OutlineWidth : int = 1
 ├─ Font : SerializableFont     (可跨机器序列化，含字体缺失检测)
 └─ BackgroundImageFileName      (有图时用 TextureBrush 拉伸到包围盒)

MouseSpeedIndicatorStyle : ElementStyle  (Styles/MouseSpeedIndicatorStyle.cs:29)
 ├─ InnerColor (100,100,100) / OuterColor (255,255,255) / OutlineWidth (1)
```

- 覆盖规则：`GlobalSettings.CurrentStyle.TryGetElementStyle<T>(id) ?? DefaultKeyStyle/DefaultMouseSpeedIndicatorStyle`（见 `KeyboardKeyDefinition.cs:88-91`、`MouseSpeedIndicatorDefinition.cs:84`）。
- 样式与定义**分文件存储**：定义在 `keyboard.json`，样式在若干 `.style` 文件里；`KeyboardStyle.IsGlobal => !ElementStyles.Any()`（`KeyboardStyle.cs:54`）——全局样式不允许带元素级覆盖。
- 字体缺失检测：`SerializableFont` 带下载 URL（`Extra/SerializableFont.cs`），`LoadKeyboardForm.ToggleFontsPanel` 列出缺失字体并提供双击打开下载页、重启按钮。
- 图片路径：`FileHelper.GetStyleImagePath` 按是否全局样式选 `keyboards/global/images` 或 `keyboards/<Category>/images`；`ImageCache` 避免重复加载。

## 4. 磁盘布局与 JSON 格式

```
keyboards/
├─ global/                       # 全局样式
│  ├─ default.style
│  └─ images/…
├─ <Category>/                   # 例如 Normal、GamesLegacy、quake、TheCore、wheels…
│  ├─ images/                    # 该分类共享图片
│  └─ <Keyboard>/
│     ├─ keyboard.json           # 定义（Constants.DefinitionFilename）
│     └─ <Style>.style           # 该键盘专属样式（可多个）
```

仓库自带：58 个 `keyboard.json`、21 个 `.style`、130 个 `.png`（`keyboards/` 共 209 个文件）；9 个分类目录（`global` 之外：BurningFish、GamesLegacy、HaleyHalcyon、joao7yt、Normal、quake、TheCore、wheels）。

### 4.1 序列化实现

**不是 Newtonsoft，而是 `System.Runtime.Serialization.Json.DataContractJsonSerializer`**（`Extra/FileHelper.cs:54-108`）：

```csharp
new DataContractJsonSerializer(typeof(T))
JsonReaderWriterFactory.CreateJsonReader(stream, Encoding.UTF8, XmlDictionaryReaderQuotas.Max, null)
JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8, true /*indent*/, true)
```

因此 JSON 有以下“DataContract 风味”：

- 属性名 PascalCase，`null` 字段也写出（如 `"BackgroundImageFileName": null`）；
- 多态元素靠 `__type` 字段 + `[KnownType]` 还原（`KeyboardKey`、`MouseKey`、`MouseScroll`、`MouseSpeedIndicator` 等注册在 `ElementDefinition`/`KeyDefinition` 上）；
- `Dictionary<int, ElementStyle>` 序列化为数组 `[{"Key":..,"Value":..}]`（`default.style` 的 `"ElementStyles": [ ]` 即空字典）；
- 颜色为 `{Red, Green, Blue}` 对象（`SerializableColor`），字体为 `{FontFamily, Size, Style}` 对象。

> 注意：`KeyboardDefinition.Load()` 用了一个很粗糙的版本探测——逐行读文件，`line.Contains("\"Version\": ")` 后 `Split(':')` 取整数（`KeyboardDefinition.cs:273` 附近），要求 `>= 2`，然后才反序列化。改 JSON 缩进/结构时要小心这段。

### 4.2 一个键的 JSON 形状（节选 `keyboards/Normal/us_intl/keyboard.json`）

```json
{
  "Elements": [
    {
      "__type": "KeyboardKey",
      "Id": 0,
      "Boundaries": [ {"X":581,"Y":100}, {"X":671,"Y":100}, {"X":671,"Y":143}, {"X":581,"Y":143} ],
      "KeyCodes": [ 8 ],
      "Text": "Backspc",
      "TextPosition": { "X": 626, "Y": 121 },
      "ChangeOnCaps": false,
      "ShiftText": "Backspc"
    }
  ]
}
```

（完整文件 3127 行，包含整个键盘的所有键；`__type` 之外的字段顺序无关。）

## 5. 旧版格式（NohBoard 0.x `.kb`）

`Legacy/LegacyKbFileParser.cs` 是**兼容层**，不是主格式：

- 行式文本，`key <键码> <x> <y> <w> <h> <normalText> <shiftText> <changeOnCaps> …`；另有 `width`、`height`、`category` 指令（`:93-114`）。
- 键码通过 `LegacyKeyCodeMapping.Map(int)` 映射成 `Keyboard`/`Mouse`/`MouseScroll`/`MouseMovement` 四类（`:70`）；字符通过 `LegacyCharacterMapping.Replace` 还原特殊字符（`:151`）。
- `.kb` 只能描述矩形键；解析后统一平移，保证左上留 9px 边距，并重算 `Width/Height`（`:122-128`）。
- 同一键码且 `BordersWith` 的相邻矩形会被 Clipper 合并成一个多边形（`AddOrOverlap`，`:274-286`）。
- 入口在 `Forms/LoadKeyboardForm.cs:162`（“Load legacy”按钮，文件对话框过滤 `*.kb`），解析结果直接注入主窗口，不写入 `keyboards/`。

## 6. 修改键盘定义的方式（面向二次开发）

`KeyboardDefinition` 的编辑 API 都是**函数式**的（返回新对象，不改自身），这是撤销系统的基础：

| 方法 | 作用 |
|---|---|
| `AddElement(def, index?)` / `RemoveElement(def)` | 元素增删（只改列表） |
| `MoveElementDown(def, n)` | z 序调整（负值下移） |
| `Resize(Size)` | 改画布尺寸（编辑模式拖窗口触发） |
| `Clone()` | 深拷贝（撤销/整体平移时用） |
| `IsChanged(other)` | 与另一份定义比较（撤销去重用） |
| `GetNextId()` | 生成新元素 Id |
| `GetBoundingBox()` | 所有元素包围盒 |

`ElementDefinition` 侧对应 `Translate`、`Manipulate`、`Modify...`、`SetId`、`Clone`、`IsChanged`；`KeyboardKeyDefinition.Modify(boundaries/keyCodes/text/shiftText/changeOnCaps/textPosition)` 是属性表单唯一的写入口。

**序列化契约**：新增元素类型时，必须：
1. 加 `[DataContract(Name="...")]`（名字即 JSON 里的 `__type`）；
2. 在 `ElementDefinition`（或其基类）的 `[KnownType(typeof(新类型))]` 中登记；
3. 在 `MainForm.Render`/`ResetBackBrushes`/`LoadKeyboard`（钩子选择）、`MainMenu_Opening`、属性/样式表单分派处补齐分支——这些地方都是**按类型硬编码的 if/switch**，编译器不会提醒。
