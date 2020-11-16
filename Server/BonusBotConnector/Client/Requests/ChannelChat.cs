using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Database.Entity.Bonusbot;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Database.Entity.Userpanel;
using static BonusBotConnector.Client.BonusBotConnectorClient;
using static BonusBotConnector.Client.MessageToChannel;

namespace BonusBotConnector.Client.Requests
{
    public class ChannelChat
    {
        private readonly MessageToChannelClient _client;

        private readonly BonusbotSettings _settings;

        public ChannelChat(GrpcChannel channel, BonusbotSettings settings)
        {
            _client = new MessageToChannelClient(channel);
            _settings = settings;
        }

        public event ErrorLogDelegate? Error;

        public event ErrorStringLogDelegate? ErrorString;

        public void SendActionStartInfo(IGangGamemode gamemode)
        {
            if (_settings.ActionsInfoChannelId is null)
                return;

            try
            {
                var embed = new EmbedToChannelRequest
                {
                    Author = "",
                    Title = "A gang action was started!",
                    ColorR = 150,
                    ColorG = 150,
                    ColorB = 150
                };
                embed.Fields.Add(new EmbedField { Name = "Attacker", Value = gamemode.AttackerGang?.Entity.Name ?? "?" });
                embed.Fields.Add(new EmbedField { Name = "Owner", Value = gamemode.OwnerGang?.Entity.Name ?? "?" });
                embed.Fields.Add(new EmbedField { Name = "Type", Value = gamemode.Type.ToString() });
                embed.Fields.Add(new EmbedField { Name = "Area name", Value = gamemode.AreaName });

                SendRequest(embed, _settings.ActionsInfoChannelId);
            }
            catch (Exception ex)
            {
                Error?.Invoke(ex, true);
            }
        }

        public void SendAdminApplication(Applications application, ITDSPlayer player)
        {
            if (player.Entity is null)
                return;
            var request = new EmbedToChannelRequest
            {
                Author = $"{player.Entity.Name} ({player.Entity.SCName})",
                Title = "A new application was sent.",
                ColorR = 0,
                ColorG = 0,
                ColorB = 120
            };
            request.Fields.Add(new EmbedField { Name = "Play hours:", Value = (player.Entity.PlayerStats.PlayTime / 60f).ToString() });
            request.Fields.Add(new EmbedField { Name = "Created:", Value = application.CreateTime.ToString() });
            SendRequest(request, _settings.AdminApplicationsChannelId);
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

        public void SendError(string msg)
        {
            SendRequest(msg, _settings.ErrorLogsChannelId, false);
        }

        public void SendSupportRequest(string info)
        {
            SendRequest(info, _settings.SupportRequestsChannelId);
        }

        private void HandleResult(MessageToChannelRequestReply result)
        {
            if (string.IsNullOrEmpty(result.ErrorMessage))
                return;
            ErrorString?.Invoke(result.ErrorMessage, result.ErrorStackTrace, result.ErrorType, true);
        }

        private async void SendRequest(string text, ulong? channelId, bool logToBonusBotOnError = true)
        {
            if (channelId is null)
                return;
            try
            {
                var result = await _client.SendAsync(new MessageToChannelRequest { GuildId = _settings.GuildId!.Value, ChannelId = channelId.Value, Text = text }, deadline: _settings.GrpcDeadline);
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
                var result = await _client.SendEmbedAsync(request, deadline: _settings.GrpcDeadline);
                HandleResult(result);
            }
            catch (Exception ex)
            {
                Error?.Invoke(ex, logToBonusBotOnError);
            }
        }
    }
}
