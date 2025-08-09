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

        // next state is ShellStartedState
        currentState = await currentState.TransitionAsync().ConfigureAwait(false);

        // next state is ApplicationStoppedState
        currentState = await currentState.TransitionAsync().ConfigureAwait(false);
        
        return 0;
    }
}