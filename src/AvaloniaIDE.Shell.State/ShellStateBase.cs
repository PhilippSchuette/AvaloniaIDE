using System.Threading.Tasks;

namespace AvaloniaIDE.Shell.State;

public abstract class ShellStateBase : IShellState
{
    protected abstract Task OnTransitioningAsync();

    protected abstract IShellState GetNextState();

    public virtual bool IsInitial => false;

    public virtual bool IsFinal => false;

    public async Task<IShellState> TransitionAsync()
    {
        await this.OnTransitioningAsync().ConfigureAwait(false);

        return this.GetNextState();
    }
}