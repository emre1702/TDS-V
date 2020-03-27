using BonusBotConnector.Client;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Player;
using TDS_Shared.Data.Enums.Challenge;
using TDS_Shared.Default;
using TDS_Shared.Manager.Utility;

namespace TDS_Server.Handler.Userpanel
{
    public class UserpanelSettingsNormalHandler : DatabaseEntityWrapper
    {
        private Dictionary<ulong, int> _playerIdWaitingForDiscordUserIdConfirm = new Dictionary<ulong, int>();

        private readonly Serializer _serializer;
        private readonly BonusBotConnectorClient _bonusBotConnectorClient;
        private readonly TDSPlayerHandler _tdsPlayerHandler;

        public UserpanelSettingsNormalHandler(Serializer serializer, BonusBotConnectorClient bonusBotConnectorClient, TDSDbContext dbContext, 
            ILoggingHandler loggingHandler, TDSPlayerHandler tdsPlayerHandler)
            : base(dbContext, loggingHandler)
            => (_serializer, _bonusBotConnectorClient, _tdsPlayerHandler) = (serializer, bonusBotConnectorClient, tdsPlayerHandler);

        public async void SaveSettings(ITDSPlayer player, string json)
        {
            var obj = _serializer.FromBrowser<PlayerSettings>(json);

            var newDiscordUserId = obj.DiscordUserId;
            obj.DiscordUserId = player.Entity!.PlayerSettings.DiscordUserId;
            await player.ExecuteForDBAsync(async (dbContext) =>
            {
                dbContext.Entry(player.Entity.PlayerSettings).CurrentValues.SetValues(obj);
                await dbContext.SaveChangesAsync();
            });

            player.LoadTimezone();
            player.AddToChallenge(ChallengeType.ChangeSettings);

            player.SendEvent(ToClientEvent.SyncSettings, json);

            if (newDiscordUserId != player.Entity.PlayerSettings.DiscordUserId)
            {
                _bonusBotConnectorClient.PrivateChat?.SendMessage(string.Format(player.Language.DISCORD_IDENTITY_CHANGED_BONUSBOT_INFO, player.DisplayName), newDiscordUserId, (reply) =>
                {
                    if (string.IsNullOrEmpty(reply.ErrorMessage))
                        return;

                    player.SendMessage(string.Format(player.Language.DISCORD_IDENTITY_SAVE_FAILED, reply.ErrorMessage));
                });
            }
        }

        public string ConfirmDiscordUserId(ulong discordUserId)
        {
            if (!_playerIdWaitingForDiscordUserIdConfirm.TryGetValue(discordUserId, out int userId))
                return "This discord user id is not waiting for a confirmation.";

            try
            {
                var player = _tdsPlayerHandler.GetIfExists(userId);
                if (player is { })
                    SaveDiscordUserId(player, discordUserId);
                else 
                    SaveDiscordUserId(userId, discordUserId);

                return "Discord Id confirmation was successful";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.GetBaseException().Message;
            }
        }

        private async void SaveDiscordUserId(int userId, ulong discordUserId)
        {
            var settings = await ExecuteForDBAsync(async dbContext 
                => await dbContext.PlayerSettings.FirstOrDefaultAsync(s => s.PlayerId == userId));
            if (settings is null)
                throw new Exception("Your player settings do not exist?!");
            settings.DiscordUserId = discordUserId;
            await ExecuteForDBAsync(async dbContext
                => await dbContext.SaveChangesAsync());
        }

        private async void SaveDiscordUserId(ITDSPlayer player, ulong discordUserId)
        {
            player.Entity!.PlayerSettings.DiscordUserId = discordUserId;
            await player.SaveData(true);
        }
    }
}
