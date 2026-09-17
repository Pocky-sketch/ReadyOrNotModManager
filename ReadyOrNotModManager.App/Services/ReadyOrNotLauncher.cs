using System.IO;

namespace ReadyOrNotModManager.App.Services;

public sealed record ReadyOrNotLaunchTarget(
    bool CanLaunch,
    string Target,
    string WorkingDirectory,
    bool UseShellExecute,
    string Message);

public static class ReadyOrNotLauncher
{
    public const string SteamLaunchUri = "steam://rungameid/1144200";

    private static readonly string[] ExecutableCandidates =
    [
        Path.Combine("ReadyOrNot", "Binaries", "Win64", "ReadyOrNotSteam-Win64-Shipping.exe"),
        Path.Combine("ReadyOrNot", "Binaries", "Win64", "ReadyOrNot-Win64-Shipping.exe"),
        Path.Combine("ReadyOrNot", "Binaries", "Win64", "ReadyOrNot.exe"),
        "ReadyOrNot.exe"
    ];

    public static ReadyOrNotLaunchTarget Resolve(string installDirectory, bool preferSteam)
    {
        if (preferSteam)
        {
            return new ReadyOrNotLaunchTarget(true, SteamLaunchUri, string.Empty, true, "正在通过 Steam 启动 Ready or Not。");
        }

        var executablePath = FindDirectExecutable(installDirectory);
        if (!string.IsNullOrWhiteSpace(executablePath))
        {
            return new ReadyOrNotLaunchTarget(
                true,
                executablePath,
                Path.GetDirectoryName(executablePath) ?? installDirectory,
                true,
                "正在启动 Ready or Not。");
        }

        return new ReadyOrNotLaunchTarget(
            false,
            string.Empty,
            string.Empty,
            true,
            "未找到 Ready or Not 可执行文件，请在设置中检查游戏安装目录。");
    }

    public static string? FindDirectExecutable(string installDirectory)
    {
        if (string.IsNullOrWhiteSpace(installDirectory))
        {
            return null;
        }

        foreach (var candidate in ExecutableCandidates)
        {
            var path = Path.Combine(installDirectory, candidate);
            if (File.Exists(path))
            {
                return path;
            }
        }

        return null;
    }
}
