using AvaloniaIDE.Shell.Abstractions;
using AvaloniaIDE.Shell.Hosting;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace AvaloniaIDE.Shell.States;

internal sealed class HostingStartingState : ShellStateBase
{
    private readonly WebApplicationBuilder builder;

    public HostingStartingState(WebApplicationBuilder builder)
    {
        this.builder = builder;
    }

    protected override Task OnTransitioningAsync()
    {
        this.builder.Services.ConfigureGrpc();

        return Task.CompletedTask;
    }

    protected override IShellState GetNextState()
    {
        var app = this.builder.Build();
        Microsoft.Extensions.Logging.ILogger<IShellState> logger =
            app.Services.GetRequiredService<Microsoft.Extensions.Logging.ILogger<IShellState>>();

        app.MapGrpcServices();

        return new AvaloniaStartingState(app, logger);
    }
}