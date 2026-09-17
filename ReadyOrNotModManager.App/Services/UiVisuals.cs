using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using MahApps.Metro.IconPacks;

namespace ReadyOrNotModManager.App.Services;

public enum VisualTone
{
    Neutral,
    Info,
    Success,
    Warning,
    Danger
}

public enum DashboardStatusKind
{
    Game,
    Nexus,
    Update
}

public sealed record QueueStatusVisual(string Label, VisualTone Tone, PackIconMaterialKind Icon)
{
    public static QueueStatusVisual FromStatus(string? status)
    {
        var value = status?.Trim() ?? string.Empty;
        if (value.Contains("fail", StringComparison.OrdinalIgnoreCase) || value.Contains("error", StringComparison.OrdinalIgnoreCase))
        {
            return new QueueStatusVisual(UiText.Status("Error"), VisualTone.Danger, PackIconMaterialKind.AlertCircleOutline);
        }

        if (value.Contains("missing", StringComparison.OrdinalIgnoreCase))
        {
            return new QueueStatusVisual(UiText.Status("Missing"), VisualTone.Warning, PackIconMaterialKind.AlertOutline);
        }

        if (value.Equals("Deployed", StringComparison.OrdinalIgnoreCase))
        {
            return new QueueStatusVisual(UiText.Status("Deployed"), VisualTone.Success, PackIconMaterialKind.CheckCircleOutline);
        }

        if (value.Equals("Downloaded", StringComparison.OrdinalIgnoreCase) || value.Equals("Imported archive", StringComparison.OrdinalIgnoreCase))
        {
            return new QueueStatusVisual(UiText.Status(value), VisualTone.Info, PackIconMaterialKind.ArchiveCheckOutline);
        }

        if (value.Equals("Queued", StringComparison.OrdinalIgnoreCase) || value.Contains("requesting", StringComparison.OrdinalIgnoreCase) || value.Contains("open nexus", StringComparison.OrdinalIgnoreCase))
        {
            return new QueueStatusVisual(UiText.Status(value.Length == 0 ? "Queued" : value), VisualTone.Warning, PackIconMaterialKind.ClockOutline);
        }

        return new QueueStatusVisual(UiText.Status(value.Length == 0 ? "Unknown" : value), VisualTone.Neutral, PackIconMaterialKind.CircleOutline);
    }
}

public sealed record RecentActivityVisual(DateTimeOffset TimestampUtc, string TimeText, string Text, VisualTone Tone, PackIconMaterialKind Icon)
{
    public static RecentActivityVisual FromActivity(RecentActivityItem item)
    {
        var text = item.Text;
        var lower = text.ToLowerInvariant();
        var tone = VisualTone.Neutral;
        var icon = PackIconMaterialKind.CircleOutline;

        if (lower.Contains("failed") || lower.Contains("error") || lower.Contains("失败") || lower.Contains("错误"))
        {
            tone = VisualTone.Danger;
            icon = PackIconMaterialKind.AlertCircleOutline;
        }
        else if (lower.Contains("deploy") || lower.Contains("部署"))
        {
            tone = VisualTone.Success;
            icon = PackIconMaterialKind.CheckCircleOutline;
        }
        else if (lower.Contains("download") || lower.Contains("import") || lower.Contains("added")
                 || lower.Contains("下载") || lower.Contains("导入") || lower.Contains("添加"))
        {
            tone = VisualTone.Info;
            icon = PackIconMaterialKind.DownloadCircleOutline;
        }
        else if (lower.Contains("profile") || lower.Contains("modpack") || lower.Contains("uninstall") || lower.Contains("delete")
                 || lower.Contains("配置档") || lower.Contains("整合包") || lower.Contains("卸载") || lower.Contains("删除"))
        {
            tone = VisualTone.Warning;
            icon = PackIconMaterialKind.FolderMultipleOutline;
        }

        return new RecentActivityVisual(item.TimestampUtc, item.TimestampUtc.ToLocalTime().ToString("HH:mm", CultureInfo.InvariantCulture), text, tone, icon);
    }
}

public sealed record DashboardStatusVisual(DashboardStatusKind Kind, string Status, string HelperText, VisualTone Tone, PackIconMaterialKind Icon)
{
    public static DashboardStatusVisual FromStatus(DashboardStatusKind kind, string status)
    {
        var normalized = status.Trim();
        return kind switch
        {
            DashboardStatusKind.Game when normalized.Equals("Detected", StringComparison.OrdinalIgnoreCase) || normalized.Equals("已检测到", StringComparison.OrdinalIgnoreCase) =>
                new DashboardStatusVisual(kind, UiText.Status(status), "已定位 ReadyOrNot\\Content\\Paks", VisualTone.Success, PackIconMaterialKind.GamepadVariantOutline),
            DashboardStatusKind.Game =>
                new DashboardStatusVisual(kind, UiText.Status(status), "选择或自动检测游戏目录", VisualTone.Danger, PackIconMaterialKind.GamepadVariantOutline),
            DashboardStatusKind.Nexus when normalized.StartsWith("Connected", StringComparison.OrdinalIgnoreCase) || normalized.StartsWith("已连接", StringComparison.OrdinalIgnoreCase) =>
                new DashboardStatusVisual(kind, UiText.Status(status), "该账号的 API 密钥已验证", VisualTone.Success, PackIconMaterialKind.AccountCheckOutline),
            DashboardStatusKind.Nexus when normalized.Equals("Not tested", StringComparison.OrdinalIgnoreCase) || normalized.Equals("未测试", StringComparison.OrdinalIgnoreCase) =>
                new DashboardStatusVisual(kind, UiText.Status(status), "在设置中运行连接测试", VisualTone.Warning, PackIconMaterialKind.AccountQuestionOutline),
            DashboardStatusKind.Nexus =>
                new DashboardStatusVisual(kind, UiText.Status(status), "Nexus API 访问需要处理", VisualTone.Danger, PackIconMaterialKind.AccountCancelOutline),
            DashboardStatusKind.Update when normalized.Contains("up to date", StringComparison.OrdinalIgnoreCase) || normalized.Contains("已是最新", StringComparison.OrdinalIgnoreCase) =>
                new DashboardStatusVisual(kind, UiText.Status(status), "GitHub 最新版本与本版本一致", VisualTone.Success, PackIconMaterialKind.Update),
            DashboardStatusKind.Update when normalized.Contains("available", StringComparison.OrdinalIgnoreCase) || normalized.Contains("可用更新", StringComparison.OrdinalIgnoreCase) =>
                new DashboardStatusVisual(kind, UiText.Status(status), "GitHub 上有更新的版本", VisualTone.Warning, PackIconMaterialKind.Update),
            DashboardStatusKind.Update when normalized.Contains("unable to check", StringComparison.OrdinalIgnoreCase) || normalized.Contains("无法检查", StringComparison.OrdinalIgnoreCase) =>
                new DashboardStatusVisual(kind, UiText.Status(status), "点击「应用版本」重试 GitHub 版本检查", VisualTone.Warning, PackIconMaterialKind.Update),
            DashboardStatusKind.Update =>
                new DashboardStatusVisual(kind, UiText.Status(status), "尚未完成版本检查", VisualTone.Neutral, PackIconMaterialKind.Update),
            _ => new DashboardStatusVisual(kind, UiText.Status(status), "状态不可用", VisualTone.Neutral, PackIconMaterialKind.HelpCircleOutline)
        };
    }
}

public sealed class QueueStatusLabelConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return QueueStatusVisual.FromStatus(value as string).Label;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

public sealed class QueueStatusIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return QueueStatusVisual.FromStatus(value as string).Icon;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

public sealed class QueueStatusBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ToneBrushes.ForTone(QueueStatusVisual.FromStatus(value as string).Tone);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

public sealed class VisualToneBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ToneBrushes.ForTone(value is VisualTone tone ? tone : VisualTone.Neutral);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

internal static class ToneBrushes
{
    public static System.Windows.Media.Brush ForTone(VisualTone tone)
    {
        var color = tone switch
        {
            VisualTone.Info => System.Windows.Media.Color.FromRgb(0x58, 0xA6, 0xFF),
            VisualTone.Success => System.Windows.Media.Color.FromRgb(0x7E, 0xD9, 0x92),
            VisualTone.Warning => System.Windows.Media.Color.FromRgb(0xD6, 0xA8, 0x4F),
            VisualTone.Danger => System.Windows.Media.Color.FromRgb(0xE0, 0x6C, 0x75),
            _ => System.Windows.Media.Color.FromRgb(0x8B, 0x98, 0xA5)
        };
        return new SolidColorBrush(color);
    }
}
