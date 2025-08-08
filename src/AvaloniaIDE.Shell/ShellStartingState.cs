using AvaloniaIDE.Shell.State;
using AvaloniaIDE.Shell.UI;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AvaloniaIDE.Shell;

internal sealed class ShellStartingState : ShellStateBase
{
    private const string LogTemplate = """
        [{Timestamp:yyyy-MM-dd HH:mm:ss.fff}][{Level:u3}][{SourceContext}] {Message:lj}{NewLine}{Exception}
        """;

    private readonly HostApplicationBuilder hostBuilder;

    public ShellStartingState()
    {
        var args = Environment.GetCommandLineArgs();
        this.hostBuilder = Host.CreateApplicationBuilder(args);
    }

    protected override Task OnTransitioningAsync()
    {
        this.PrepareHostEnvironment();
        this.PrepareHostConfiguration();
        this.PrepareHostLogging();

        return Task.CompletedTask;
    }

    protected override IShellState GetNextState()
    {

        IHost host = this.hostBuilder.Build();
        Microsoft.Extensions.Logging.ILogger<AvaloniaStartingState> logger =
            host.Services.GetRequiredService<Microsoft.Extensions.Logging.ILogger<AvaloniaStartingState>>();

        return new AvaloniaStartingState(host, logger);
    }

    public override bool IsInitial => true;

    private void PrepareHostEnvironment()
    {
        this.hostBuilder.Environment.ContentRootPath = Directory.GetCurrentDirectory();
    }

    private void PrepareHostConfiguration()
    {
        this.hostBuilder.Configuration
            .AddJsonFile("shellsettings.json")
            .AddJsonFile($"shellsettings.{hostBuilder.Environment.EnvironmentName}.json");
    }

#pragma warning disable CA1305
    private void PrepareHostLogging()
    {
        hostBuilder.Services.AddSerilog((services, config) => config
            .ReadFrom.Configuration(hostBuilder.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.File(
                GetLogFilePath(),
                fileSizeLimitBytes: 128 * 1024 * 1024,
                rollOnFileSizeLimit: true,
                retainedFileCountLimit: 2,
                outputTemplate: LogTemplate
            )
        );
    }
#pragma warning restore

    private static string GetLogFilePath()
    {
        string basePath = Environment.GetFolderPath(
            RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ?
                Environment.SpecialFolder.ApplicationData
                : Environment.SpecialFolder.LocalApplicationData,
            Environment.SpecialFolderOption.None
        );
        return Path.Combine(basePath, "avalonia-ide", "shell.logs");
    }
}