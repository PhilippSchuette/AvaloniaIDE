using System;
using Microsoft.Extensions.Logging;

namespace AvaloniaIDE.Shell;

internal static partial class Logs
{
    [LoggerMessage(EventId = 0, Level = LogLevel.Information, Message = "--- AvaloniaIDE shell starting ---")]
    public static partial void LogShellStart(this ILogger logger);

    [LoggerMessage(EventId = 0, Level = LogLevel.Information, Message = "--- AvaloniaIDE shell stopped ---")]
    public static partial void LogShellStopped(this ILogger logger);

    [LoggerMessage(EventId = 0, Level = LogLevel.Information, Message = "Regular UI shutdown")]
    public static partial void LogAvaloniaStopped(this ILogger logger);

    [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "Unexpected UI shutdown")]
    public static partial void LogAvaloniaStopped(this ILogger logger, Exception ex);
}