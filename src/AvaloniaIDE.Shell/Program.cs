using System;
using System.Threading.Tasks;

using AvaloniaIDE.Shell.State;

namespace AvaloniaIDE.Shell;

internal static class Program
{
    [STAThread]
    public static async Task<int> Main()
    {
        var initialState = new ShellStartingState();
        IShellState currentState = initialState;

        // A basic loop should suffice for now.
        // We might need some more complex handling later on,
        // based on specific interfaces implemented by states.
        while (!currentState.IsFinal)
            currentState = await currentState.TransitionAsync().ConfigureAwait(false);

        return 0;
    }
}