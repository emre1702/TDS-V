using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TDS.Server.Data.Interfaces;
namespace BonusBotConnector_Server
{
    public class BonusBotConnectorServer
    {

        public BonusBotConnectorServer(ILoggingHandler loggingHandler)
        {
            var host = CreateHostBuilder(loggingHandler).Build();
            CommandService = host.Services.GetRequiredService<BBCommandService>();
            SupportRequestService = host.Services.GetRequiredService<SupportRequestService>();
            host.RunAsync();
        }

        public BBCommandService CommandService { get; }
        public SupportRequestService SupportRequestService { get; }

        public static IHostBuilder CreateHostBuilder(ILoggingHandler loggingHandler) =>
            Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .UseUrls("http://localhost:5001")
                        .ConfigureServices(services => services
                            .AddSingleton(loggingHandler)
                            .AddSingleton<BBCommandService>()
                            .AddSingleton<SupportRequestService>()
                        );
                });

        public static void Main()
        {
        }

    }
}
