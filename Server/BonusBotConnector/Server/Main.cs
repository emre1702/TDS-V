using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Runtime.InteropServices;
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
                        .ConfigureServices(services => services
                            .AddSingleton(loggingHandler)
                            .AddSingleton<BBCommandService>()
                            .AddSingleton<SupportRequestService>()
                        )
                        .ConfigureKestrel(options =>
                        {
                            options.Listen(IPAddress.Loopback, 5001, listenOptions =>
                            {
                                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                                    listenOptions.UseHttps("/home/localhost.pfx", "grpc");
                            });
                        }); ;
                });

        public static void Main()
        {
        }
    }
}