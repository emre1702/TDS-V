using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Userpanel;

namespace BonusBotConnector_Server
{
    public class BonusBotConnectorServer
    {
        #region Public Constructors

        public BonusBotConnectorServer(ILoggingHandler loggingHandler, IUserpanelHandler userpanelHandler)
        {
            var host = CreateHostBuilder(loggingHandler, userpanelHandler).Build();
            CommandService = host.Services.GetRequiredService<BBCommandService>();
            host.RunAsync();
        }

        #endregion Public Constructors

        #region Public Properties

        public BBCommandService CommandService { get; }

        #endregion Public Properties

        #region Public Methods

        // Additional configuration is required to successfully run gRPC on macOS. For instructions
        // on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
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

        public static void Main()
        {
        }

        #endregion Public Methods
    }
}
