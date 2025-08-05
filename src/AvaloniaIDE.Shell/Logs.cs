using System;
using Microsoft.Extensions.Logging;

namespace AvaloniaIDE.Shell;

internal static partial class Logs
{
    [LoggerMessage(
        EventId = 0, Level = LogLevel.Information, Message = "--- {ApplicationName} shell starting ---")]
    public static partial void LogShellStarting(this ILogger logger, string applicationName);

    [LoggerMessage(
        EventId = 0, Level = LogLevel.Information, Message = "--- {ApplicationName} shell stopped ---")]
    public static partial void LogShellStopped(this ILogger logger, string applicationName);

    [LoggerMessage(
        EventId = 0, Level = LogLevel.Information, Message = "Avalonia UI starting")]
    public static partial void LogAvaloniaStarting(this ILogger logger);

    [LoggerMessage(
        EventId = 0, Level = LogLevel.Information, Message = "Avalonia UI stopped")]
    public static partial void LogAvaloniaStopped(this ILogger logger);

    [LoggerMessage(
        EventId = 0, Level = LogLevel.Error, Message = "Avalonia UI stopped with exception")]
    public static partial void LogAvaloniaStopped(this ILogger logger, Exception ex);
}