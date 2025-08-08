using System;
using System.Threading.Tasks;

namespace AvaloniaIDE.Shell.State;

public sealed class ApplicationStoppedState : IShellState
{
    public bool IsInitial => false;

    public bool IsFinal => true;

    public Task<IShellState> TransitionAsync()
    {
        throw new NotSupportedException("Cannot transition from the final state.");
    }
}