using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity.Bonusbot;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Database.Entity.Userpanel;
using static BonusBotConnector.Client.BonusBotConnectorClient;
using static BonusBotConnector.Client.MessageToChannel;

namespace BonusBotConnector.Client.Requests
{
    public class ChannelChat
    {
        public event ErrorLogDelegate? Error;
        public event ErrorStringLogDelegate? ErrorString;

        private readonly MessageToChannelClient _client;
        private readonly BonusbotSettings _settings;

        public ChannelChat(GrpcChannel channel, BonusbotSettings settings)
        {
            _client = new MessageToChannelClient(channel);
            _settings = settings;
        }

        public void SendAdminApplication(Applications application)
        {
            var request = new EmbedToChannelRequest
            {
                Author = $"{application.Player.Name} ({application.Player.SCName})",
                Title = "A new application was sent.",
                ColorR = 0,
                ColorG = 0,
                ColorB = 120
            };
            request.Fields.Add(new EmbedField { Name = "Play hours:", Value = (application.Player.PlayerStats.PlayTime / 60f).ToString() });
            request.Fields.Add(new EmbedField { Name = "Created:", Value = application.CreateTime.ToString() });
            SendRequest(request, _settings.AdminApplicationsChannelId);
        }

        public void SendSupportRequest(string info)
        {
            SendRequest(info, _settings.SupportRequestsChannelId);
        }

        public void SendError(string msg)
        {
            SendRequest(msg, _settings.ErrorLogsChannelId, false);
        }

        public void SendActionInfo(string info)
        {
            SendRequest(info, _settings.ActionsInfoChannelId);
        }

        public void SendBanInfo(PlayerBans ban, List<EmbedField> fields)
        {
            if (_settings.BansInfoChannelId is null)
                return;

            try
            {
                var embed = new EmbedToChannelRequest
                {
                    Author = $"{ban.Player.Name} ({ban.Player.SCName})",
                    Title = "Someone got banned.",
                    ColorR = 130,
                    ColorG = 0,
                    ColorB = 0
                };

                embed.Fields.AddRange(fields);
                SendRequest(embed, _settings.BansInfoChannelId);

            }
            catch (Exception ex)
            {
                Error?.Invoke(ex, true);
            }
        }

        private async void SendRequest(string text, ulong? channelId, bool logToBonusBotOnError = true)
        {
            if (channelId is null)
                return;
            try
            {
                var result = await _client.SendAsync(new MessageToChannelRequest { GuildId = _settings.GuildId!.Value, ChannelId = channelId.Value, Text = text });
                HandleResult(result);
            }
            catch (Exception ex)
            {
                Error?.Invoke(ex, logToBonusBotOnError);
            }
        }

        private async void SendRequest(EmbedToChannelRequest request, ulong? channelId, bool logToBonusBotOnError = true)
        {
            if (channelId is null)
                return;
            try
            {
                request.GuildId = _settings.GuildId!.Value;
                request.ChannelId = channelId.Value;
                var result = await _client.SendEmbedAsync(request);
                HandleResult(result);
            }
            catch (Exception ex)
            {
                Error?.Invoke(ex, logToBonusBotOnError);
            }
        }

        private void HandleResult(MessageToChannelRequestReply result)
        {
            if (string.IsNullOrEmpty(result.ErrorMessage))
                return;
            ErrorString?.Invoke(result.ErrorMessage, result.ErrorStackTrace, true);
        }
    }
}
