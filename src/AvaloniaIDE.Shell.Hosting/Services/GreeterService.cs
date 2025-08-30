using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AvaloniaIDE.Shell.Hosting.Services;

#pragma warning disable CA1812
internal sealed class GreeterService : Greeter.GreeterBase
#pragma warning restore
{
    private readonly ILogger<GreeterService> _logger;

    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }
}