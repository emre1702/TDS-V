using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Database.Entity.Userpanel;
using static BonusBotConnector_Client.Main;
using static BonusBotConnector_Client.MessageToChannel;

namespace BonusBotConnector.Client.Requests
{
    public static class ChannelChat
    {
        private static MessageToChannelClient? _client;
        private static BonusBotErrorLoggerDelegate? _errorLogger;

        internal static void Init(GrpcChannel channel, BonusBotErrorLoggerDelegate errorLogger)
        {
            _client = new MessageToChannelClient(channel);
            _errorLogger = errorLogger;
        }

        public static void SendAdminApplication(Applications application)
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
            SendRequest(request, Settings?.AdminApplicationsChannelId);
        }

        public static void SendSupportRequest(string info)
        {
            SendRequest(info, Settings?.SupportRequestsChannelId);
        }

        public static void SendError(string msg)
        {
            SendRequest(msg, Settings?.ErrorLogsChannelId, false);
        }

        public static void SendActionInfo(string info)
        {
            SendRequest(info, Settings?.ActionsInfoChannelId);
        }

        public static void SendBanInfo(PlayerBans ban, List<EmbedField> fields)
        {
            if (Settings?.BansInfoChannelId is null)
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
                SendRequest(embed, Settings.BansInfoChannelId);

            }
            catch (Exception ex)
            {
                _errorLogger?.Invoke(ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace, true);
            }
        }

        private static async void SendRequest(string text, ulong? channelId, bool logToBonusBotOnError = true) 
        {
            if (_client is null)
                return;
            if (Settings?.GuildId is null)
                return;
            if (channelId is null)
                return;
            try
            {
                var result = await _client.SendAsync(new MessageToChannelRequest { GuildId = Settings.GuildId.Value, ChannelId = channelId.Value, Text = text });
                HandleResult(result);
            }
            catch (Exception ex)
            {
                _errorLogger?.Invoke(ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace, logToBonusBotOnError);
            }
        }

        private static async void SendRequest(EmbedToChannelRequest request, ulong? channelId, bool logToBonusBotOnError = true)
        {
            if (_client is null)
                return;
            if (Settings is null)
                return;
            if (Settings.GuildId is null)
                return;
            if (channelId is null)
                return;
            try
            {
                request.GuildId = Settings.GuildId.Value;
                request.ChannelId = channelId.Value;
                var result = await _client.SendEmbedAsync(request);
                HandleResult(result);
            }
            catch (Exception ex)
            {
                _errorLogger?.Invoke(ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace, logToBonusBotOnError);
            }
        }

        private static void HandleResult(MessageToChannelRequestReply result)
        {
            if (string.IsNullOrEmpty(result.ErrorMessage))
                return;
            _errorLogger?.Invoke(result.ErrorMessage, Environment.StackTrace, true);
        }
    }
}
