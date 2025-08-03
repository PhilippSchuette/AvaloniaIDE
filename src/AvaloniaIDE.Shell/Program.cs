using Avalonia;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Avalonia.Controls.ApplicationLifetimes;

namespace AvaloniaIDE.Shell;

sealed class Program
{
    [STAThread]
    public static async Task<int> Main(string[] args)
    {
        IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args);

        ClassicDesktopStyleApplicationLifetime lifetime = new ClassicDesktopStyleApplicationLifetime()
        {
            Args = args,
            ShutdownMode = Avalonia.Controls.ShutdownMode.OnMainWindowClose,
        };

        AppBuilder appBuilder = AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .SetupWithLifetime(lifetime);

        IHost host = hostBuilder.Build();
        Application app = appBuilder.Instance!;

        await host.StartAsync();
        int result = lifetime.Start();

        await host.StopAsync();
        await host.WaitForShutdownAsync();

        return result;
    }
}
