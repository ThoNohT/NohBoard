# NohBoard · 初始化分析（init analysis）

本目录是对 [ThoNohT/NohBoard](https://github.com/ThoNohT/NohBoard) 的一次通读式分析，目的是在动手改代码之前建立完整的结构认知：架构、数据格式、编辑器交互、输入钩子、构建与维护建议。

## 0. 本次 fork / 下载记录

| 项 | 值 |
|---|---|
| 上游仓库 | https://github.com/ThoNohT/NohBoard （GPL-2.0，1298 stars，主语言 C#） |
| 我的 fork | https://github.com/gudqs7/NohBoard |
| 本地目录 | `D:\code\nohboard`（仓库根） |
| 分支 | `master`（HEAD `6d3a2a4 Create FUNDING.yml`，312 commits） |
| remotes | `origin` = 自己的 fork；`upstream` = 原作者 |
| 上游最近提交 | 2024-04-07 |
| 仓库规模 | 330 个 tracked 文件（`NohBoard/` 117、`keyboards/` 209、其余为根文件） |

Fork 与 clone 命令（可复现）：

```powershell
gh repo fork ThoNohT/NohBoard --clone=false
git clone https://github.com/gudqs7/NohBoard.git D:\code\nohboard
cd D:\code\nohboard
git remote add upstream https://github.com/ThoNohT/NohBoard.git
```

## 1. 项目一句话

一个 Windows 桌面小工具：**把键盘/鼠标的实时输入画在屏幕上**，方便录屏/OBS（支持 chroma key 抠绿底），布局（键盘键帽形状、文字、配色、背景图）完全由 JSON 数据驱动，程序内置所见即所得的编辑模式。

技术栈：C# / WinForms / GDI+ / .NET Core 3.1（`netcoreapp3.1`，`win-x64`）+ 低级键鼠钩子（`WH_KEYBOARD_LL` / `WH_MOUSE_LL`）+ Clipper 6（多边形几何）。

## 2. 目录导航

| 文档 | 内容 |
|---|---|
| [01-architecture.md](01-architecture.md) | 仓库/解决方案结构、启动链路、静态全局状态、一帧渲染生命周期、分层图、数据流、设计取舍、入口速查表 |
| [02-keyboard-model-and-format.md](02-keyboard-model-and-format.md) | 键盘/元素/样式模型、四类元素的语义、磁盘布局、JSON 格式（DataContractJsonSerializer）、旧版 `.kb` 兼容层、新增元素类型检查清单 |
| [03-editor.md](03-editor.md) | 编辑模式：撤销/重做快照机制、鼠标手势表、快捷键、元素/边界/属性/样式菜单、文件保存、已知粗糙点 |
| [04-input-hooking.md](04-input-hooking.md) | 钩子安装与回调路由、VK 与自定义伪键码、`StateBase` 状态模型、鼠标速度/滚轮、trap 拦截语义、权限限制、扩展点 |
| [05-settings-build-and-roadmap.md](05-settings-build-and-roadmap.md) | 设置字段与默认值、设置界面行为、构建与发布、分支与版本、P0–P2 维护建议、风险清单、验证清单 |

## 3. 构建与运行（本机已验证）

环境：Windows 11，.NET SDK 9.0.304 / 8.0.301（`netcoreapp3.1` 目标仍可构建）。

```powershell
cd D:\code\nohboard\NohBoard
dotnet build NohBoard.sln -c Debug
# 0 error，3 warning（NETSDK1138 netcoreapp3.1 EOL、NETSDK1201 RuntimeIdentifier 相关）
.\NohBoard\bin\Debug\netcoreapp3.1\win-x64\NohBoard.exe
```

注意：`keyboards/` 目录是运行时数据，必须和 exe 的相对位置保持 `keyboards/` 在可执行文件目录旁（发布时记得一并拷贝）。

## 4. 速览：关键结论

1. **数据模型是“不可变对象 + 整份快照撤销”**：任何编辑都返回新 `KeyboardDefinition`/`ElementDefinition`，撤销栈存整份 `(定义, 样式)`，简单但内存开销大、无上限。
2. **渲染靠预渲染背景位图**：4 张 `shift × caps` 组合的 `TextureBrush` 铺底，只有按下的元素逐帧重画；空闲时 `UpdateTimer` 几乎不刷新（`Updated` 标志驱动）。
3. **键是任意多边形**，支持组合键（`KeyCodes` 全部按下才算按下）、Shift/Caps 文案切换、Caps/Num/Scroll 的“状态伪键码”（1026/1027/1028）。
4. **输入钩子是全局副作用**：装的是低级钩子，可吞掉系统输入（trap），总开关是 ScrollLock（可改），需要管理员权限才能覆盖提权窗口。
5. **序列化用 `DataContractJsonSerializer`**（`__type` + `[KnownType]` 多态、字典变数组），改格式必须考虑向后兼容（`KeyboardVersion=2`）。
6. **没有 CI、没有测试、版本号靠 `gotri.exe` 构建期生成**；代码里 `Forms/` 与 `Controls/` 有重复控件实现，多处按元素类型硬编码分派。
7. **最值得先做的三件事**：升级 TFM、补 CI + 模型层测试、把 `GlobalSettings` 里的“当前文档 + 撤销”拆出去。

## 5. 待确认/后续问题

- [ ] 是否要在 fork 上直接做“升级到 .NET 8 + 现代化序列化”的重构，还是只做小修补？（影响 05 里 P1 的取舍）
- [ ] 目标用途是录屏美化、教学演示还是宏/反作弊辅助？（trap 语义、输入过滤策略不同）
- [ ] 是否需要保留对旧 `.kb` 布局与 `keyboards/` 既有 58 套布局的完全兼容？（决定序列化迁移方案）
- [ ] 是否要处理高 DPI / 多显示器？`dpi` 分支里已有未合并的探索。

## 6. 分析方法说明

- 以静态阅读 + 一次完整构建验证为主；未做运行时行为录制（除构建产物存在性验证）；
- 引用的 `文件:行号` 均基于 master `6d3a2a4`，行号会随后续提交漂移；
- 覆盖范围：`NohBoard.sln` 的三个项目（NohBoard / Hooking / clipper_library）与 `keyboards/` 数据；未逐行审计 `clipper_library/clipper.cs`（第三方 Clipper 6 源码，仅确认用途）。
