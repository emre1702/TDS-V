using BonusBotConnector_Client.Requests;
using Grpc.Net.Client;
using System;
using System.Linq;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Bonusbot;

namespace BonusBotConnector_Client
{
    public static class Main
    {
        public static BonusbotSettings? Settings { get; private set; }
        // public static GrpcChannel? Channel { get; private set; }

        public delegate void BonusBotErrorLoggerDelegate(string info, string stackTrace, bool logToBonusBot = true);

        public static void Init(TDSDbContext dbContext, BonusBotErrorLoggerDelegate errorLogger)
        {
            if (System.Diagnostics.Debugger.IsAttached)
                return;

            Settings = dbContext.BonusbotSettings.FirstOrDefault();

            if (Settings.GuildId is null)
                return;

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var channel = GrpcChannel.ForAddress("http://localhost:5000");

            ChannelChat.Init(channel, errorLogger);
            ServerInfos.Init(channel, errorLogger);
            PrivateChat.Init(channel, errorLogger);
        }
    }
}
