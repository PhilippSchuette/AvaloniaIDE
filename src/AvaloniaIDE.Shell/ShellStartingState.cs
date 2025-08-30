using AvaloniaIDE.Shell.State;
using AvaloniaIDE.Shell.Hosting;
using System.Threading.Tasks;
using System;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Serilog;

namespace AvaloniaIDE.Shell;

internal sealed class ShellStartingState : ShellStateBase
{
    private const string LogTemplate = """
        [{Timestamp:yyyy-MM-dd HH:mm:ss.fff}][{Level:u3}][{SourceContext}] {Message:lj}{NewLine}{Exception}
        """;

    private readonly WebApplicationBuilder builder;

    public ShellStartingState()
    {
        var args = Environment.GetCommandLineArgs();
        this.builder = WebApplication.CreateBuilder();
    }

    protected override Task OnTransitioningAsync()
    {
        this.PrepareHostConfiguration();
        this.PrepareHostEnvironment();
        this.PrepareHostLogging();

        return Task.CompletedTask;
    }

    protected override IShellState GetNextState()
    {
        return new HostingStartingState(this.builder);
    }

    public override bool IsInitial => true;

    private void PrepareHostEnvironment()
    {
        this.builder.Environment.ContentRootPath = Directory.GetCurrentDirectory();
    }

    private void PrepareHostConfiguration()
    {
        var basePath =
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "Configuration");
        this.builder.Configuration
            .AddJsonFile(
                Path.Combine(basePath, "shellsettings.json"))
            .AddJsonFile(
                Path.Combine(basePath, $"shellsettings.{builder.Environment.EnvironmentName}.json"));
    }

#pragma warning disable CA1305
    private void PrepareHostLogging()
    {
        builder.Services.AddSerilog((services, config) => config
            .ReadFrom.Configuration(builder.Configuration)
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