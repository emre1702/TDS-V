using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity.Bonusbot;
using TDS_Server.Database.Entity.Userpanel;
using static BonusBotConnector.Client.BonusBotConnectorClient;
using static BonusBotConnector.Client.SupportRequest;

namespace BonusBotConnector.Client.Requests
{
    public class Support
    {
        private readonly SupportRequestClient _client;

        private readonly BonusbotSettings _settings;

        internal Support(GrpcChannel channel, BonusbotSettings settings)
        {
            _client = new SupportRequestClient(channel);
            _settings = settings;
        }

        public event ErrorLogDelegate? Error;

        public event ErrorStringLogDelegate? ErrorString;

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

                var result = await _client.AnswerAsync(request, deadline: _settings.GrpcDeadline);

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
                if (player.Entity.PlayerSettings.General.DiscordUserId == 0)
                    return;

                var request = new SupportRequestCreateRequest
                {
                    AtLeastAdminLevel = requestEntity.AtleastAdminLevel,
                    AuthorName = player.DisplayName,
                    GuildId = _settings.GuildId.Value,
                    SupportType = (int)requestEntity.Type,
                    Text = requestEntity.Messages.First().Text,
                    Title = requestEntity.Title,
                    UserId = player.Entity.PlayerSettings.General.DiscordUserId ?? 0
                };

                var result = await _client.CreateAsync(request, deadline: _settings.GrpcDeadline);

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

                var result = await _client.DeleteAsync(request, deadline: _settings.GrpcDeadline);

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

                var result = await _client.ToggleClosedAsync(request, deadline: _settings.GrpcDeadline);

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
