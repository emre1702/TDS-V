using Grpc.Net.Client;
using System;
using System.Net;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity.Bonusbot;
using static BonusBotConnector.Client.RAGEServerStats;

namespace BonusBotConnector.Client.Requests
{
    public class ServerInfos
    {
        private readonly RAGEServerStatsClient _client;
        private readonly ILoggingHandler _loggingHandler;
        private readonly BonusbotSettings _settings;
        private readonly Helper _helper;
        private readonly string _ipAddress = "?";

        internal ServerInfos(GrpcChannel channel, ILoggingHandler loggingHandler, Helper helper, BonusbotSettings settings)
        {
            _client = new RAGEServerStatsClient(channel);
            _loggingHandler = loggingHandler;
            _settings = settings;
            _helper = helper;

            _ipAddress = new WebClient().DownloadString("https://www.l2.io/ip");
        }

        public async void Refresh(RAGEServerStatsRequest request)
        {
            try
            {
                request.GuildId = _settings.GuildId!.Value;
                request.ChannelId = _settings.ServerInfosChannelId!.Value;
                request.ServerAddress = _ipAddress;
                var result = await _client.SendAsync(request);

            }
            catch (Exception ex)
            {
                _loggingHandler.LogErrorFromBonusBot(ex);
            }
        }
    }
}
