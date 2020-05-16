using Grpc.Net.Client;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity.Bonusbot;
using TDS_Server.Database.Entity.Userpanel;
using static BonusBotConnector.Client.BonusBotConnectorClient;
using static BonusBotConnector.Client.SupportRequest;

namespace BonusBotConnector.Client.Requests
{
    public class Support
    {
        public event ErrorLogDelegate? Error;
        public event ErrorStringLogDelegate? ErrorString;

        private readonly SupportRequestClient _client;
        private readonly BonusbotSettings _settings;

        internal Support(GrpcChannel channel, BonusbotSettings settings)
        {
            _client = new SupportRequestClient(channel);
            _settings = settings;
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
                ErrorString?.Invoke(result.ErrorMessage, result.ErrorStackTrace, true);
            }
            catch (Exception ex)
            {
                Error?.Invoke(ex);
            }
        }

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
                ErrorString?.Invoke(result.ErrorMessage, result.ErrorStackTrace, true);
            }
            catch (Exception ex)
            {
                Error?.Invoke(ex);
            }
        }
    }
}
