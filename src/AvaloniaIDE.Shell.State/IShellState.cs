using System.Threading.Tasks;

namespace AvaloniaIDE.Shell.State;

public interface IShellState
{
    bool IsInitial { get; }

    bool IsFinal { get; }

    Task<IShellState> TransitionAsync();
}