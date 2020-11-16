using Grpc.Net.Client;
using System;
using System.Net;
using TDS.Server.Database.Entity.Bonusbot;
using static BonusBotConnector.Client.BonusBotConnectorClient;
using static BonusBotConnector.Client.RAGEServerStats;

namespace BonusBotConnector.Client.Requests
{
    public class ServerInfos
    {
        private readonly RAGEServerStatsClient _client;

        private readonly string _ipAddress = "?";

        private readonly BonusbotSettings _settings;

        internal ServerInfos(GrpcChannel channel, BonusbotSettings settings)
        {
            _client = new RAGEServerStatsClient(channel);
            _settings = settings;

            using var webClient = new WebClient();
            _ipAddress = webClient.DownloadString("https://www.l2.io/ip");
        }

        public event ErrorLogDelegate? Error;

        public event ErrorStringLogDelegate? ErrorString;

        public async void Refresh(RAGEServerStatsRequest request)
        {
            try
            {
                request.GuildId = _settings.GuildId!.Value;
                request.ChannelId = _settings.ServerInfosChannelId!.Value;
                request.ServerAddress = _ipAddress;
                var result = await _client.SendAsync(request, deadline: _settings.GrpcDeadline);

                if (string.IsNullOrEmpty(result.ErrorMessage))
                    return;
                ErrorString?.Invoke(result.ErrorMessage, result.ErrorStackTrace, result.ErrorType, true);
            }
            catch (Exception ex)
            {
                Error?.Invoke(ex);
            }
        }
    }
}
