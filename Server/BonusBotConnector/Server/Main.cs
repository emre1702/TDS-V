using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Userpanel;

namespace BonusBotConnector_Server
{
    public class BonusBotConnectorServer
    {
        public BBCommandService CommandService { get; }

        public static void Main() { }

        public BonusBotConnectorServer(ILoggingHandler loggingHandler, IUserpanelHandler userpanelHandler)
        {
            var host = CreateHostBuilder(loggingHandler, userpanelHandler).Build();
            CommandService = host.Services.GetRequiredService<BBCommandService>();
            host.RunAsync();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(ILoggingHandler loggingHandler, IUserpanelHandler userpanelHandler) =>
            Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .UseUrls("http://localhost:5001")
                        .ConfigureServices(services => services
                            .AddSingleton(loggingHandler)
                            .AddSingleton(userpanelHandler)
                            .AddSingleton<BBCommandService>()
                            .AddSingleton<SupportRequestService>()
                        );
                });
    }
}
