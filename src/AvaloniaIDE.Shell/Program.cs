using Avalonia;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Avalonia.Controls.ApplicationLifetimes;
using Serilog;

namespace AvaloniaIDE.Shell;

internal sealed class Program
{
    [STAThread]
    public static async Task<int> Main(string[] args)
    {
        HostApplicationBuilder hostBuilder = Host.CreateApplicationBuilder(args);

        hostBuilder.Configuration
            .AddJsonFile("shellsettings.json");

        hostBuilder.Environment.EnvironmentName = "Development";

        string basePath = Environment.GetFolderPath(
            RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ?
                Environment.SpecialFolder.ApplicationData
                : Environment.SpecialFolder.LocalApplicationData,
            Environment.SpecialFolderOption.None
        );
        string logFilePath = Path.Combine(basePath, "avalonia-ide", "shell.logs");
            
#pragma warning disable CA1305
        hostBuilder.Services.AddSerilog((services, config) => config
            .ReadFrom.Configuration(hostBuilder.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.File(
                logFilePath,
                fileSizeLimitBytes: 128 * 1024 * 1024,
                rollOnFileSizeLimit: true,
                retainedFileCountLimit: 2
            )
        );
#pragma warning restore

        using var lifetime = new ClassicDesktopStyleApplicationLifetime()
        {
            Args = args,
            ShutdownMode = Avalonia.Controls.ShutdownMode.OnMainWindowClose,
        };

        AppBuilder appBuilder = AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace(Avalonia.Logging.LogEventLevel.Verbose)
            .SetupWithLifetime(lifetime);

        // TODO: use a custom log sink
        // Avalonia.Logging.Logger.Sink = null;

        IHost host = hostBuilder.Build();
        Application app = appBuilder.Instance!;

        await host.StartAsync().ConfigureAwait(false);

        int result = 0;
        try
        {
            result = lifetime.Start();
        }
#pragma warning disable CA1031
        catch (Exception)
#pragma warning restore
        {
            // TODO: add logging
            result = -1;
        }

        await host.StopAsync().ConfigureAwait(false);
        await host.WaitForShutdownAsync().ConfigureAwait(false);

        return result;
    }
}
