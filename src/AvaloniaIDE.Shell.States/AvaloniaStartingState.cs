using AvaloniaIDE.Shell.Abstractions;
using AvaloniaIDE.Shell.UI;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

namespace AvaloniaIDE.Shell.States;

internal sealed class AvaloniaStartingState : ShellStateBase
{
    private readonly IHost host;
    private readonly Microsoft.Extensions.Logging.ILogger logger;

    private AppBuilder? appBuilder;

    public AvaloniaStartingState(
        IHost host,
        Microsoft.Extensions.Logging.ILogger<IShellState> logger)
    {
        this.host = host;
        this.logger = logger;
    }

    protected override Task OnTransitioningAsync()
    {
        using var lifetime = new ClassicDesktopStyleApplicationLifetime()
        {
            Args = Environment.GetCommandLineArgs(),
            ShutdownMode = Avalonia.Controls.ShutdownMode.OnMainWindowClose,
        };

        this.appBuilder = ShellUiBuilder.Create(logger)
            .SetupWithLifetime(lifetime);

        return Task.CompletedTask;
    }

    protected override IShellState GetNextState()
    {
        var logger =
            this.host.Services.GetRequiredService<Microsoft.Extensions.Logging.ILogger<ShellStartedState>>();
        var nextState = new ShellStartedState(this.appBuilder!, this.host, logger);

        return nextState;
    }
}