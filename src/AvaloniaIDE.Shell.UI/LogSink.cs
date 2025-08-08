using System.Collections.ObjectModel;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace AvaloniaIDE.Shell.UI;

internal sealed class LogSink : Avalonia.Logging.ILogSink
{
    private readonly ILogger logger;
    private readonly ReadOnlySet<string>? areas;
    private readonly LogLevel minLevel;

    public LogSink(
        ILogger logger,
        string[]? areas,
        LogLevel minLevel)
    {
        this.logger = logger;
        this.areas = areas?.Length > 0
            ? new ReadOnlySet<string>(new HashSet<string>(areas))
            : null;
        this.minLevel = minLevel;
    }

    public bool IsEnabled(Avalonia.Logging.LogEventLevel level, string area)
    {
        var convertedLevel = Convert(level);

        return convertedLevel >= this.minLevel
            && this.logger.IsEnabled(convertedLevel)
            && (this.areas?.Contains(area) ?? true);
    }

    public void Log(
        Avalonia.Logging.LogEventLevel level,
        string area,
        object? source,
        string messageTemplate) => this.Log(level, area, source, messageTemplate, []);

    public void Log(
        Avalonia.Logging.LogEventLevel level,
        string area,
        object? source,
        string messageTemplate, 
        params object?[] propertyValues)
    {
        if (this.IsEnabled(level, area))
        {
            EventId eventId = new EventId(0, $"Area[{area}]#Source[{source}]");
#pragma warning disable CA1848,CA2254
            this.logger.Log(Convert(level), eventId, messageTemplate, propertyValues);
#pragma warning restore
        }
    }

    private static LogLevel Convert(Avalonia.Logging.LogEventLevel level)
    {
        return level switch
        {
            Avalonia.Logging.LogEventLevel.Verbose => LogLevel.Trace,
            Avalonia.Logging.LogEventLevel.Debug => LogLevel.Debug,
            Avalonia.Logging.LogEventLevel.Information => LogLevel.Information,
            Avalonia.Logging.LogEventLevel.Warning => LogLevel.Warning,
            Avalonia.Logging.LogEventLevel.Error => LogLevel.Error,
            Avalonia.Logging.LogEventLevel.Fatal => LogLevel.Critical,
            _ => LogLevel.None
        };
    }
}