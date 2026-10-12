using PluggrClient.Services;

namespace PluggrClient
{
    public static class PluggrClientService
    {
        public static Services.PluggrClientService Service { get; private set; }

        public static void Initalise(uint port)
        {
            Task.Run(() =>
            {
                var builder = WebApplication.CreateBuilder();

                // Add services to the container.
                builder.Services.AddGrpc();

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                app.MapGrpcService<Services.PluggrClientService>();
                app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

                app.Run($"http://localhost:{port}/");
            });
        }
    }
}
