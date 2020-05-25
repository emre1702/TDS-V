using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using TDS_Server.Database.Entity.Bonusbot;
using TDS_Server.Database.Entity.Player;
using static BonusBotConnector.Client.BonusBotConnectorClient;
using static BonusBotConnector.Client.MessageToUser;

namespace BonusBotConnector.Client.Requests
{
    public class PrivateChat
    {
        #region Private Fields

        private readonly MessageToUserClient _client;

        private readonly BonusbotSettings _settings;

        #endregion Private Fields

        #region Internal Constructors

        internal PrivateChat(GrpcChannel channel, BonusbotSettings settings)
        {
            _client = new MessageToUserClient(channel);
            _settings = settings;
        }

        #endregion Internal Constructors

        #region Public Events

        public event ErrorLogDelegate? Error;

        public event ErrorStringLogDelegate? ErrorString;

        #endregion Public Events

        #region Public Methods

        public void SendBanMessage(ulong userId, PlayerBans ban, List<EmbedField> fields)
        {
            if (_settings.SendPrivateMessageOnBan != true)
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
                Error?.Invoke(ex);
            }
        }

        public void SendMessage(string text, ulong userId, Action<MessageToUserRequestReply> replyHandler)
        {
            SendRequest(text, userId, replyHandler);
        }

        public void SendOfflineMessage(string author, string text, ulong userId)
        {
            if (_settings.SendPrivateMessageOnOfflineMessage != true)
                return;
            SendRequest($"You got an offline message from '{author}':{Environment.NewLine}{text}", userId);
        }

        #endregion Public Methods

        #region Private Methods

        private void HandleResult(MessageToUserRequestReply result)
        {
            if (string.IsNullOrEmpty(result.ErrorMessage))
                return;
            ErrorString?.Invoke(result.ErrorMessage, result.ErrorStackTrace, result.ErrorType, true);
        }

        private async void SendRequest(string text, ulong userId, bool logToBonusBotOnError = true)
        {
            if (userId == 0)
                return;
            try
            {
                var result = await _client.SendAsync(new MessageToUserRequest { GuildId = _settings.GuildId!.Value, UserId = userId, Text = text });
                HandleResult(result);
            }
            catch (Exception ex)
            {
                Error?.Invoke(ex, logToBonusBotOnError);
            }
        }

        private async void SendRequest(EmbedToUserRequest request, bool logToBonusBotOnError = true)
        {
            try
            {
                request.GuildId = _settings.GuildId!.Value;
                var result = await _client.SendEmbedAsync(request);
                HandleResult(result);
            }
            catch (Exception ex)
            {
                Error?.Invoke(ex, logToBonusBotOnError);
            }
        }

        private async void SendRequest(string text, ulong userId, Action<MessageToUserRequestReply> replyHandler, bool logToBonusBotOnError = true)
        {
            if (userId == 0)
                return;
            try
            {
                var result = await _client.SendAsync(new MessageToUserRequest { GuildId = _settings.GuildId!.Value, UserId = userId, Text = text });
                replyHandler(result);
            }
            catch (Exception ex)
            {
                Error?.Invoke(ex, logToBonusBotOnError);
            }
        }

        #endregion Private Methods
    }
}
