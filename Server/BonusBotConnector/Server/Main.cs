using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TDS_Server.Data.Interfaces;

namespace BonusBotConnector_Server
{
    public class BonusBotConnectorServer
    {
        public BBCommandService CommandService { get; }

        public BonusBotConnectorServer(ILoggingHandler loggingHandler)
        {
            var host = CreateHostBuilder(loggingHandler).Build();
            CommandService = host.Services.GetRequiredService<BBCommandService>();
            host.RunAsync();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(ILoggingHandler loggingHandler) =>
            Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .UseUrls("http://localhost:5001")
                        .ConfigureServices(services => services.AddSingleton(loggingHandler));
                });
    }
}
