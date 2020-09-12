using BonusBotConnector.Client.Requests;
using Grpc.Net.Client;
using System;
using System.Linq;
using TDS_Server.Database.Entity;

namespace BonusBotConnector.Client
{
    public class BonusBotConnectorClient
    {
        public BonusBotConnectorClient(TDSDbContext dbContext)
        {
            if (System.Diagnostics.Debugger.IsAttached)
                return;

            var settings = dbContext.BonusbotSettings.FirstOrDefault();

            if (settings is null)
                return;
            if (settings.GuildId is null)
                return;

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var channel = GrpcChannel.ForAddress("http://localhost:5000");

            Helper = new Helper();
            ChannelChat = new ChannelChat(channel, settings);

            if (settings.ServerInfosChannelId is { })
                ServerInfos = new ServerInfos(channel, settings);
            PrivateChat = new PrivateChat(channel, settings);
            Support = new Support(channel, settings);
        }

        //public delegate void BonusBotErrorLoggerDelegate(string info, string stackTrace, bool logToBonusBot = true);
        public delegate void ErrorLogDelegate(Exception ex, bool logToBonusBot = true);

        public delegate void ErrorStringLogDelegate(string message, string stackTrace, string exceptionType, bool logToBonusBot = true);

        public ChannelChat? ChannelChat { get; }
        public Helper? Helper { get; }
        public PrivateChat? PrivateChat { get; }
        public ServerInfos? ServerInfos { get; }
        public Support? Support { get; }
    }
}
