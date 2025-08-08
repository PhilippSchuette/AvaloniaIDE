using AvaloniaIDE.Shell.State;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using System;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Hosting;

namespace AvaloniaIDE.Shell.UI;

internal sealed class ShellStartedState : ShellStateBase
{
    private readonly AppBuilder appBuilder;
    private readonly IHost host;

    public ShellStartedState(AppBuilder appBuilder, IHost host)
    {
        this.appBuilder = appBuilder;
        this.host = host;
    }

    protected override async Task OnTransitioningAsync()
    {
        Application app = appBuilder.Instance!;

        // var logger = host.Services.GetRequiredService<MsLogging.ILogger<Program>>();

        // logger.LogShellStarting(hostBuilder.Environment.ApplicationName);
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        await this.host.StartAsync(cancellationToken).ConfigureAwait(false);

        int result = 0;
        try
        {
             // logger.LogAvaloniaStarting();
             if (app.ApplicationLifetime is ClassicDesktopStyleApplicationLifetime lifetime)
             result = lifetime.Start();
             // logger.LogAvaloniaStopped();
         }
#pragma warning disable CA1031
        catch (Exception)
#pragma warning restore
         {
             // logger.LogAvaloniaStopped(ex);
             result = -1;
         }

        await host.StopAsync(cancellationToken).ConfigureAwait(false);

        // logger.LogShellStopped(hostBuilder.Environment.ApplicationName);

        // return result;
    }

    protected override IShellState GetNextState()
    {
        return new ApplicationStoppedState();
    }
}