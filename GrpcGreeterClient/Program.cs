using Grpc.Net.Client;
using GrpcGreeterClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

var loggerFactory = LoggerFactory.Create(logging =>
{
    logging.SetMinimumLevel(LogLevel.Trace);
    logging.AddSimpleConsole(options =>
    {
        options.TimestampFormat = "HH:mm:ss.fff ";
        options.ColorBehavior = LoggerColorBehavior.Enabled;
    });
});

var handler = new SocketsHttpHandler
{
    PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
    KeepAlivePingDelay = TimeSpan.FromSeconds(60),
    KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
    EnableMultipleHttp2Connections = true
};

// The port number must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress("https://KS:40125", //TODO Replace KS with your hostname
    new GrpcChannelOptions
    {
        HttpHandler = handler,
        LoggerFactory = loggerFactory
    });

var client = new Greeter.GreeterClient(channel);

while (true)
{
    var reply = await client.SayHelloAsync(
        new HelloRequest { Name = "GreeterClient" });
    Console.WriteLine("Greeting: " + reply.Message);
    Console.WriteLine("Press any key to exit...\n");
    Console.ReadKey();
}