using System.Threading.Tasks;

namespace AvaloniaIDE.Shell.Abstractions;

public interface IShellState
{
    bool IsInitial { get; }

    bool IsFinal { get; }

    Task<IShellState> TransitionAsync();
}