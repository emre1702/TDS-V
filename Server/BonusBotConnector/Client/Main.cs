using BonusBotConnector.Client.Requests;
using Grpc.Net.Client;
using System;
using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Bonusbot;

namespace BonusBotConnector.Client
{
    public class BonusBotConnectorClient
    {
        public ChannelChat? ChannelChat { get; }
        public ServerInfos? ServerInfos { get; }
        public PrivateChat? PrivateChat { get; }

        public delegate void BonusBotErrorLoggerDelegate(string info, string stackTrace, bool logToBonusBot = true);

        public BonusBotConnectorClient(ILoggingHandler loggingHandler, TDSDbContext dbContext)
        {
            if (System.Diagnostics.Debugger.IsAttached)
                return;

            var settings = dbContext.BonusbotSettings.FirstOrDefault();

            if (settings.GuildId is null)
                return;

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var channel = GrpcChannel.ForAddress("http://localhost:5000");

            var helper = new Helper();
            ChannelChat = new ChannelChat(channel, loggingHandler, helper, settings);

            if (settings.ServerInfosChannelId is { })
                ServerInfos = new ServerInfos(channel, loggingHandler, helper, settings);
            PrivateChat = new PrivateChat(channel, loggingHandler, helper, settings);
        }
    }
}
