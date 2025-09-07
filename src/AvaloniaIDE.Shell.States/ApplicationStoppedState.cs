using System;
using System.Threading.Tasks;
using AvaloniaIDE.Shell.Abstractions;

namespace AvaloniaIDE.Shell.States;

internal sealed class ApplicationStoppedState : IShellState
{
    public bool IsInitial => false;

    public bool IsFinal => true;

    public Task<IShellState> TransitionAsync()
    {
        throw new NotSupportedException("Cannot transition from the final state.");
    }
}