using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using AvaloniaIDE.Shell.UI.ViewModels;

namespace AvaloniaIDE.Shell.UI;

#pragma warning disable CA1812
internal sealed class ViewLocator : IDataTemplate
#pragma warning restore
{

    public Control? Build(object? param)
    {
        if (param is null)
            return null;
        
        var name = param.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        var type = Type.GetType(name);

        if (type != null)
            return (Control)Activator.CreateInstance(type)!;
        
        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data) => data is ViewModelBase;
}