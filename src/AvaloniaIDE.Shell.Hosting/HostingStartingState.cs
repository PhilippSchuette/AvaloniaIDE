using AvaloniaIDE.Shell.State;
using AvaloniaIDE.Shell.UI;
using AvaloniaIDE.Shell.Hosting.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace AvaloniaIDE.Shell.Hosting;

public sealed class HostingStartingState : ShellStateBase
{
    private readonly WebApplicationBuilder builder;

    public HostingStartingState(WebApplicationBuilder builder)
    {
        this.builder = builder;
    }

    protected override Task OnTransitioningAsync()
    {
        this.builder.Services.AddGrpc();

        return Task.CompletedTask;
    }

    protected override IShellState GetNextState()
    {
        var app = this.builder.Build();
        Microsoft.Extensions.Logging.ILogger<IShellState> logger =
            app.Services.GetRequiredService<Microsoft.Extensions.Logging.ILogger<IShellState>>();

        // TODO: this needs to be moved, once dynamic modules have been implemented
        app.MapGrpcService<GreeterService>();

        return new AvaloniaStartingState(app, logger);
    }
}