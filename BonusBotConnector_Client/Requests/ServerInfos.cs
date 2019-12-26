using Grpc.Net.Client;
using System;
using System.Net;
using static BonusBotConnector_Client.Main;
using static BonusBotConnector_Client.RAGEServerStats;

namespace BonusBotConnector_Client.Requests
{
    public static class ServerInfos
    {
        private static RAGEServerStatsClient? _client;
        private static BonusBotErrorLoggerDelegate? _errorLogger;
        private static string _ipAddress = "?";

        internal static void Init(GrpcChannel channel, BonusBotErrorLoggerDelegate errorLogger)
        {
            _client = new RAGEServerStatsClient(channel);
            _errorLogger = errorLogger;

            _ipAddress = new WebClient().DownloadString("https://www.l2.io/ip");
        }

        public static async void Refresh(RAGEServerStatsRequest request)
        {
            if (_client is null)
                return;
            if (Settings is null)
                return;
            if (Settings.GuildId is null)
                return;
            if (Settings.ServerInfosChannelId is null)
                return;
            try
            {
                request.GuildId = Settings.GuildId.Value;
                request.ChannelId = Settings.ServerInfosChannelId.Value;
                request.ServerAddress = _ipAddress;
                var result = await _client.SendAsync(request);
              
            }
            catch (Exception ex)
            {
                _errorLogger?.Invoke(ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace);
            }
        }
    }
}
