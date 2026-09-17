namespace ReadyOrNotModManager.App.Services;

/// <summary>
/// 内部逻辑值 → 中文显示文本。
/// 状态值、配置键、路径、主题 key 一律保持英文原样（代码里有硬比较），只在此处翻译“显示”。
/// </summary>
public static class UiText
{
    public static string Status(string? status)
    {
        var value = status?.Trim() ?? string.Empty;
        if (value.StartsWith("Connected", StringComparison.OrdinalIgnoreCase))
        {
            return "已连接：" + value["Connected:".Length..].Trim();
        }

        return value switch
        {
            "" => "未知",
            "Error" => "错误",
            "Missing" => "缺失",
            "Deployed" => "已部署",
            "Downloaded" => "已下载",
            "Imported archive" => "已导入压缩包",
            "Queued" => "排队中",
            "Requesting download link" => "正在获取下载链接",
            "Open Nexus page and import zip" => "打开 Nexus 页面并导入压缩包",
            "Downloading" => "下载中",
            "Opened in browser" => "已在浏览器中打开",
            "Failed - see errors" => "失败 - 见错误列表",
            "Loaded from profile" => "从配置档加载",
            "Missing archive" => "缺少压缩包",
            "Missing key" => "缺少密钥",
            "Missing zip" => "缺少 zip 文件",
            "Needs API key or browser import" => "需要 API 密钥或手动导入",
            "No install record" => "无安装记录",
            "Uninstalled" => "已卸载",
            "Not tested" => "未测试",
            "Detected" => "已检测到",
            "Not detected" => "未检测到",
            "Unknown" => "未知",
            _ => value
        };
    }
}
