using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using TDS_Server_DB.Entity.Player;
using static BonusBotConnector_Client.Main;
using static BonusBotConnector_Client.MessageToUser;

namespace BonusBotConnector_Client.Requests
{
    public static class PrivateChat
    {
        private static MessageToUserClient? _client;
        private static BonusBotErrorLoggerDelegate? _errorLogger;

        internal static void Init(GrpcChannel channel, BonusBotErrorLoggerDelegate errorLogger)
        {
            _client = new MessageToUserClient(channel);
            _errorLogger = errorLogger;
        }

        public static void SendBanMessage(ulong userId, PlayerBans ban, List<EmbedField> fields)
        {
            if (Settings!.SendPrivateMessageOnBan != true)
                return;
            if (userId == 0)
                return;

            try
            {
                var embed = new EmbedToUserRequest
                {
                    UserId = userId,
                    Author = $"{ban.Admin.Name} ({ban.Admin.SCName})",
                    Title = "You got banned",
                    ColorR = 130,
                    ColorG = 0,
                    ColorB = 0
                };

                embed.Fields.Add(new EmbedField { Name = "You are:", Value = $"{ban.Player.Name} ({ban.Player.SCName})" });
                embed.Fields.AddRange(fields);

                SendRequest(embed);
                
            }
            catch (Exception ex)
            {
                _errorLogger?.Invoke(ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace, true);
            }
        }

        public static void SendOfflineMessage(string author, string text, ulong userId)
        {
            if (Settings!.SendPrivateMessageOnOfflineMessage != true)
                return;
            SendRequest($"You got an offline message from '{author}':{Environment.NewLine}{text}", userId);
        }

        public static void SendMessage(string text, ulong userId, Action<MessageToUserRequestReply> replyHandler)
        {
            SendRequest(text, userId, replyHandler);
        }

        private static async void SendRequest(string text, ulong userId, bool logToBonusBotOnError = true)
        {
            if (_client is null)
                return;
            if (Settings is null)
                return;
            if (Settings.GuildId is null)
                return;
            if (userId == 0)
                return;
            try
            {
                var result = await _client.SendAsync(new MessageToUserRequest { GuildId = Settings.GuildId.Value, UserId = userId, Text = text });
                HandleResult(result);
            }
            catch (Exception ex)
            {
                _errorLogger?.Invoke(ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace, logToBonusBotOnError);
            }
        }

        private static async void SendRequest(EmbedToUserRequest request, bool logToBonusBotOnError = true)
        {
            if (_client is null)
                return;
            if (Settings is null)
                return;
            if (Settings.GuildId is null)
                return;
            try
            {
                request.GuildId = Settings.GuildId.Value;
                var result = await _client.SendEmbedAsync(request);
                HandleResult(result);
            }
            catch (Exception ex)
            {
                _errorLogger?.Invoke(ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace, logToBonusBotOnError);
            }
        }

        private static async void SendRequest(string text, ulong userId, Action<MessageToUserRequestReply> replyHandler, bool logToBonusBotOnError = true)
        {
            if (_client is null)
                return;
            if (Settings is null)
                return;
            if (Settings.GuildId is null)
                return;
            if (userId == 0)
                return;
            try
            {
                var result = await _client.SendAsync(new MessageToUserRequest { GuildId = Settings.GuildId.Value, UserId = userId, Text = text });
                replyHandler(result);
            }
            catch (Exception ex)
            {
                _errorLogger?.Invoke(ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace, logToBonusBotOnError);
            }
        }

        private static void HandleResult(MessageToUserRequestReply result)
        {
            if (string.IsNullOrEmpty(result.ErrorMessage))
                return;
            _errorLogger?.Invoke(result.ErrorMessage, Environment.StackTrace, true);
        }
    }
}
