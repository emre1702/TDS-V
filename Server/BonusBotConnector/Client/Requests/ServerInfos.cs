﻿using Grpc.Net.Client;
using System;
using System.Net;
using TDS_Server.Database.Entity.Bonusbot;
using static BonusBotConnector.Client.BonusBotConnectorClient;
using static BonusBotConnector.Client.RAGEServerStats;

namespace BonusBotConnector.Client.Requests
{
    public class ServerInfos
    {
        #region Private Fields

        private readonly RAGEServerStatsClient _client;

        private readonly string _ipAddress = "?";

        private readonly BonusbotSettings _settings;

        #endregion Private Fields

        #region Internal Constructors

        internal ServerInfos(GrpcChannel channel, BonusbotSettings settings)
        {
            _client = new RAGEServerStatsClient(channel);
            _settings = settings;

            _ipAddress = new WebClient().DownloadString("https://www.l2.io/ip");
        }

        #endregion Internal Constructors

        #region Public Events

        public event ErrorLogDelegate? Error;

        public event ErrorStringLogDelegate? ErrorString;

        #endregion Public Events

        #region Public Methods

        public async void Refresh(RAGEServerStatsRequest request)
        {
            try
            {
                request.GuildId = _settings.GuildId!.Value;
                request.ChannelId = _settings.ServerInfosChannelId!.Value;
                request.ServerAddress = _ipAddress;
                var result = await _client.SendAsync(request);

                if (string.IsNullOrEmpty(result.ErrorMessage))
                    return;
                ErrorString?.Invoke(result.ErrorMessage, result.ErrorStackTrace, result.ErrorType, true);
            }
            catch (Exception ex)
            {
                Error?.Invoke(ex);
            }
        }

        #endregion Public Methods
    }
}
