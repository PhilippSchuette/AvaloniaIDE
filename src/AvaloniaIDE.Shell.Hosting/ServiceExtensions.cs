using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using AvaloniaIDE.Shell.Hosting.Services;

namespace AvaloniaIDE.Shell.Hosting;

public static class ServiceExtensions
{
    public static void ConfigureGrpc(this IServiceCollection services)
    {
        services.AddGrpc();
    }

    public static void MapGrpcServices(this WebApplication app)
    {
        // TODO: this needs to be moved, once dynamic modules have been implemented
        app.MapGrpcService<GreeterService>();
    }
}