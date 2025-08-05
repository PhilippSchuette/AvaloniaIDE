using Avalonia;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Avalonia.Controls.ApplicationLifetimes;
using Serilog;

using MsLogging = Microsoft.Extensions.Logging;

namespace AvaloniaIDE.Shell;

internal sealed class Program
{
    private const string LogTemplate = """
        [{Timestamp:yyyy-MM-dd HH:mm:ss.fff}][{Level:u3}][{SourceContext}] {Message:lj}{NewLine}{Exception}
        """;

    [STAThread]
    public static async Task<int> Main(string[] args)
    {
        HostApplicationBuilder hostBuilder = Host.CreateApplicationBuilder(args);

        hostBuilder.Environment.ContentRootPath = Directory.GetCurrentDirectory();

        hostBuilder.Configuration
            //.AddJsonFile("shellsettings.json");
            .AddJsonFile($"shellsettings.{hostBuilder.Environment.EnvironmentName}.json");

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
                retainedFileCountLimit: 2,
                outputTemplate: LogTemplate
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

        var logger = host.Services.GetRequiredService<MsLogging.ILogger<Program>>();

        logger.LogShellStart();
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        await host.StartAsync(cancellationToken).ConfigureAwait(false);

        int result = 0;

        try
        {
            result = lifetime.Start();
            logger.LogAvaloniaStopped();
        }
#pragma warning disable CA1031
        catch (Exception ex)
#pragma warning restore
        {
            logger.LogAvaloniaStopped(ex);
            result = -1;
        }

        await host.StopAsync(cancellationToken).ConfigureAwait(false);

        logger.LogShellStopped();

        return result;
    }
}
