using BonusBotConnector.Client.Requests;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Linq;
using TDS.Server.Database.Entity;
using TDS.Server.Database.Entity.Player;

namespace BonusBotConnector.Client
{
    public class BonusBotConnectorClient
    {
        public event ErrorLogDelegate? Error;

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
            var channel = GrpcChannel.ForAddress("http://bonusbot:5000", channelOptions: new GrpcChannelOptions
            {
                Credentials = ChannelCredentials.Insecure
            });

            var actionHandler = new ActionHandler(ex => Error?.Invoke(ex, true));
            Helper = new Helper();
            ChannelChat = new ChannelChat(channel, settings, actionHandler);

            if (settings.ServerInfosChannelId is { })
                ServerInfos = new ServerInfos(channel, settings, actionHandler);
            PrivateChat = new PrivateChat(channel, settings, actionHandler);
            Support = new Support(channel, settings, actionHandler);
        }

        //public delegate void BonusBotErrorLoggerDelegate(string info, string stackTrace, bool logToBonusBot = true);
        public delegate void ErrorLogDelegate(Exception ex, bool logToBonusBot = true);

        public delegate void ErrorStringLogDelegate(string message, string stackTrace, string exceptionType, bool logToBonusBot = true);

        public ChannelChat? ChannelChat { get; }
        public Helper? Helper { get; }
        public PrivateChat? PrivateChat { get; }
        public ServerInfos? ServerInfos { get; }
        public Support? Support { get; }

        public void OnNewOfficialBan(PlayerBans ban, ulong? playerDiscordUserId)
        {
            var embedFields = Helper?.GetBanEmbedFields(ban);
            if (embedFields is { })
            {
                ChannelChat?.SendBanInfo(ban, embedFields);
                if (playerDiscordUserId.HasValue)
                    PrivateChat?.SendBanMessage(playerDiscordUserId.Value, ban, embedFields);
            }
        }
    }
}