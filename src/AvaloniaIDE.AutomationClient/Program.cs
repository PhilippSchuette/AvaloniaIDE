using System;
using Grpc.Net.Client;
using AvaloniaIDE.AutomationClient;

#pragma warning disable CA1303

// The port number must match the port of the gRPC server.
// Also, you need a trusted development certificate for ASP.NET for this to work
using var channel = GrpcChannel.ForAddress("https://localhost:7043");
var client = new Greeter.GreeterClient(channel);

var reply = await client.SayHelloAsync(
    new HelloRequest { Name = "GreeterClient" });

Console.WriteLine("Greeting: " + reply.Message);
Console.WriteLine("Press any key to exit...");
Console.ReadKey();