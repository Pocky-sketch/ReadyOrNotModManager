# Ready Or Not Mod Manager — 简体中文汉化

**汉化：Pocky-sketch** · 基于上游 `jdharleyjones/ReadyOrNotModManager` **v1.3.10** 源码

本目录是汉化的**全部可复现材料**：脚本 + 翻译表。上游发布新版本时，重复下面三步即可重新出汉化版：

```bash
# 1. 取对应版本的源码
curl -sL -o src.tar.gz https://codeload.github.com/jdharleyjones/ReadyOrNotModManager/tar.gz/refs/tags/vX.Y.Z
tar xzf src.tar.gz

# 2. 注入汉化（把本目录放进源码根目录，或改脚本里的字符串表路径）
python localization/localize.py .

# 3. 编译单文件自包含版
dotnet publish ReadyOrNotModManager.App/ReadyOrNotModManager.App.csproj \
  -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ../publish
```

## 翻译边界（重要）

| 类别 | 处理 |
|---|---|
| 界面静态文案（XAML 的 `Text/Content/Header/ToolTip/Title`） | 全部译为中文 |
| 代码里的提示、状态栏、对话框文案 | 全部译为中文 |
| **文件路径** | **保留原样**（`Downloads\ReadyOrNotMods`、`Documents\ReadyOrNotModpacks`、`ReadyOrNot\Content\Paks`、`steamapps\common` 等） |
| **`settings.json` 字段名** | **保留原样**（`DownloadDirectory` / `ReadyOrNotDirectory` / `ApiKey` / `ThemeName` …），否则旧配置读不出来 |
| **内部状态值** | **保留英文**（`Deployed` / `Queued` / `Downloaded` / `Missing archive` / `Not tested` …）。代码里有 `is "Deployed"` 这类硬比较，改动会破坏筛选与卸载逻辑。中文显示由 `Services/UiText.cs` 在**渲染层**映射 |
| **主题内部 key** | **保留原样**（`tactical` / `dark` / `red` / `claude` / `codex` / `purple` / `hacker` / `light`），仅换语言 |
| 品牌 / 专名 | 保留：Nexus、Ready or Not、Vortex、`.pak`/`.zip`/`.7z`、作者与社媒账号 |

## 文件

- `localize.py` — 汉化注入脚本（按上表规则做定向替换，可重复执行）
- `strings.zh.xaml.json` — XAML 界面文案对照表
- `strings.zh.cs.json` — C# 展示文案对照表
- `../ReadyOrNotModManager.App/Services/UiText.cs` — 内部状态值 → 中文显示的映射层

## 许可

上游仓库**未附带任何许可证**（no license ⇒ 默认保留所有权利）。因此本汉化：
自用不构成问题；**若公开分发衍生版本，请先取得原作者授权**。
本目录只包含汉化脚本与翻译表，不包含上游源码本身。
