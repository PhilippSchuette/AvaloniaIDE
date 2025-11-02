using Microsoft.Extensions.Logging;
using Avalonia;
using AvaloniaIDE.Shell.UI.Logging;

namespace AvaloniaIDE.Shell.UI;

public static class ShellUiBuilder
{
    public static AppBuilder Create(ILogger logger)
    {
        var appBuilder = AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont();

        Avalonia.Logging.Logger.Sink = new LogSink(
            logger, [], Microsoft.Extensions.Logging.LogLevel.Warning
        );

        return appBuilder;
    }
}