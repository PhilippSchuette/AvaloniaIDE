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

        // basic loop should suffice for now.
        // we might need some more complex handling later on, based on specific interfaces implementied by states.
        while (!currentState.IsFinal)
            currentState = await currentState.TransitionAsync().ConfigureAwait(false);

        return 0;
    }
}