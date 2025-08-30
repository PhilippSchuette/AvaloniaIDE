using AvaloniaIDE.Shell.State;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using System;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace AvaloniaIDE.Shell.UI;

internal sealed class ShellStartedState : ShellStateBase
{
    private readonly AppBuilder appBuilder;
    private readonly IHost host;
    private readonly ILogger logger;

    public ShellStartedState(AppBuilder appBuilder, IHost host, ILogger<ShellStartedState> logger)
    {
        this.appBuilder = appBuilder;
        this.host = host;
        this.logger = logger;
    }

    protected override async Task OnTransitioningAsync()
    {
        Application app = appBuilder.Instance!;

        var configuration = this.host.Services.GetRequiredService<IConfiguration>();
        var applicationName = configuration["Environment:ApplicationName"]!;
        logger.LogShellStarting(applicationName);

        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        await this.host.StartAsync(cancellationToken).ConfigureAwait(false);

        int result = 0;
        try
        {
             logger.LogAvaloniaStarting();
             if (app.ApplicationLifetime is ClassicDesktopStyleApplicationLifetime lifetime)
                result = lifetime.Start();
             logger.LogAvaloniaStopped();
         }
#pragma warning disable CA1031
        catch (Exception ex)
#pragma warning restore
         {
             logger.LogAvaloniaStopped(ex);
             result = -1;
         }

        await host.StopAsync(cancellationToken).ConfigureAwait(false);

        logger.LogShellStopped(applicationName);

        // TODO: how can we propagate the result code?
        // return result;
    }

    protected override IShellState GetNextState()
    {
        return new ApplicationStoppedState();
    }
}