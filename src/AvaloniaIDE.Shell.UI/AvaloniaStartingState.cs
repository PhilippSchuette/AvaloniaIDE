using AvaloniaIDE.Shell.State;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

namespace AvaloniaIDE.Shell.UI;

public sealed class AvaloniaStartingState : IShellState
{
    private readonly IHost host;
    private readonly Microsoft.Extensions.Logging.ILogger logger;

    public AvaloniaStartingState(
        IHost host,
        Microsoft.Extensions.Logging.ILogger<AvaloniaStartingState> logger)
    {
        this.host = host;
        this.logger = logger;
    }

    public bool IsInitial => false;

    public bool IsFinal => false;

    public Task<IShellState> TransitionAsync()
    {
        using var lifetime = new ClassicDesktopStyleApplicationLifetime()
        {
            Args = Environment.GetCommandLineArgs(),
            ShutdownMode = Avalonia.Controls.ShutdownMode.OnMainWindowClose,
        };

        AppBuilder appBuilder = AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .SetupWithLifetime(lifetime);

        Avalonia.Logging.Logger.Sink = new LogSink(
            logger, [], Microsoft.Extensions.Logging.LogLevel.Warning
        );

        // TODO: return AvaloniaConfiguredState instead using activator utilities
        var nextState = new ApplicationStoppedState();

        return Task.FromResult<IShellState>(nextState);
    }
}