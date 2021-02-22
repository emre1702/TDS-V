using BonusBotConnector.Server;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using TDS.Server.Data.Interfaces;

namespace BonusBotConnector_Server
{
    public class BonusBotConnectorServer
    {
        public BonusBotConnectorServer(ILoggingHandler loggingHandler)
        {
            CommandService = new(loggingHandler);
            SupportRequestService = new(loggingHandler);

            StartServer();
        }

        public BBCommandService CommandService { get; }
        public SupportRequestService SupportRequestService { get; }

        private void StartServer()
        {
            var server = new Server
            {
                Services =
                {
                    BBCommand.BindService(CommandService),
                    SupportRequest.BindService(SupportRequestService)
                },
                Ports = { new ServerPort("ragemp-server", 5001, ServerCredentials.Insecure) }
            };
            server.Start();
        }

        public static void Main()
        {
        }
    }
}