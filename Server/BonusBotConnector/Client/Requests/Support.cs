using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity.Bonusbot;
using TDS_Server.Database.Entity.Userpanel;
using static BonusBotConnector.Client.BonusBotConnectorClient;
using static BonusBotConnector.Client.SupportRequest;

namespace BonusBotConnector.Client.Requests
{
    public class Support
    {
        #region Private Fields

        private readonly SupportRequestClient _client;

        private readonly BonusbotSettings _settings;

        #endregion Private Fields

        #region Internal Constructors

        internal Support(GrpcChannel channel, BonusbotSettings settings)
        {
            _client = new SupportRequestClient(channel);
            _settings = settings;
        }

        #endregion Internal Constructors

        #region Public Events

        public event ErrorLogDelegate? Error;

        public event ErrorStringLogDelegate? ErrorString;

        #endregion Public Events

        #region Public Methods

        public async void Answer(ITDSPlayer player, SupportRequestMessages messageEntity)
        {
            try
            {
                if (_settings.GuildId is null)
                    return;

                var request = new SupportRequestAnswerRequest
                {
                    AuthorName = player.DisplayName,
                    GuildId = _settings.GuildId.Value,
                    SupportRequestId = messageEntity.RequestId,
                    Text = messageEntity.Text
                };

                var result = await _client.AnswerAsync(request);

                if (string.IsNullOrEmpty(result.ErrorMessage))
                    return;
                ErrorString?.Invoke(result.ErrorMessage, result.ErrorStackTrace, result.ErrorType, true);
            }
            catch (Exception ex)
            {
                Error?.Invoke(ex);
            }
        }

        public async void Create(ITDSPlayer player, SupportRequests requestEntity)
        {
            try
            {
                if (_settings.GuildId is null)
                    return;
                if (player.Entity is null)
                    return;
                if (player.Entity.PlayerSettings is null)
                    return;
                if (player.Entity.PlayerSettings.DiscordUserId == 0)
                    return;

                var request = new SupportRequestCreateRequest
                {
                    AtLeastAdminLevel = requestEntity.AtleastAdminLevel,
                    AuthorName = player.DisplayName,
                    GuildId = _settings.GuildId.Value,
                    SupportType = (int)requestEntity.Type,
                    Text = requestEntity.Messages.First().Text,
                    Title = requestEntity.Title,
                    UserId = player.Entity.PlayerSettings.DiscordUserId
                };

                var result = await _client.CreateAsync(request);

                if (string.IsNullOrEmpty(result.ErrorMessage))
                    return;
                ErrorString?.Invoke(result.ErrorMessage, result.ErrorStackTrace, result.ErrorType, true);
            }
            catch (Exception ex)
            {
                Error?.Invoke(ex);
            }
        }

        public async void Delete(IEnumerable<int> requestIds)
        {
            try
            {
                if (_settings.GuildId is null)
                    return;

                var request = new SupportRequestDeleteRequest
                {
                    GuildId = _settings.GuildId.Value
                };
                request.SupportRequestIds.AddRange(requestIds);

                var result = await _client.DeleteAsync(request);

                if (string.IsNullOrEmpty(result.ErrorMessage))
                    return;
                ErrorString?.Invoke(result.ErrorMessage, result.ErrorStackTrace, result.ErrorType, true);
            }
            catch (Exception ex)
            {
                Error?.Invoke(ex);
            }
        }

        public async void ToggleClosed(ITDSPlayer player, int id, bool closed)
        {
            try
            {
                if (_settings.GuildId is null)
                    return;

                var request = new SupportRequestToggleClosedRequest
                {
                    GuildId = _settings.GuildId.Value,
                    SupportRequestId = id,
                    Closed = closed,
                    RequesterName = player.DisplayName
                };

                var result = await _client.ToggleClosedAsync(request);

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
