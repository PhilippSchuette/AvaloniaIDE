using System;
using System.Threading.Tasks;
using AvaloniaIDE.Shell.State;

namespace AvaloniaIDE.Shell;

internal sealed class Program
{
    [STAThread]
    public static async Task<int> Main(string[] args)
    {
        var initialState = new ShellStartingState();
        IShellState currentState = initialState;

        // next state is AvaloniaStartingState
        currentState = await currentState.TransitionAsync().ConfigureAwait(false);

        // Application app = appBuilder.Instance!;

        // var logger = host.Services.GetRequiredService<MsLogging.ILogger<Program>>();

        // logger.LogShellStarting(hostBuilder.Environment.ApplicationName);
        // using var cancellationTokenSource = new CancellationTokenSource();
        // var cancellationToken = cancellationTokenSource.Token;

        // TODO: move to transition on Avalonia configured state
        // await host.StartAsync(cancellationToken).ConfigureAwait(false);

        // TODO: move to transition on UI running state
        // int result = 0;

        // try
        // {
        //     logger.LogAvaloniaStarting();
        //     result = lifetime.Start();
        //     logger.LogAvaloniaStopped();
        // }
#pragma warning disable CA1031
        // catch (Exception ex)
#pragma warning restore
        // {
        //     logger.LogAvaloniaStopped(ex);
        //     result = -1;
        // }

        // TODO: move to shell stopping state
        // await host.StopAsync(cancellationToken).ConfigureAwait(false);

        // logger.LogShellStopped(hostBuilder.Environment.ApplicationName);

        // return result;
        
        return 0;
    }
}