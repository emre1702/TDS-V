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
using TDS_Shared.Core;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.ModAPI;

namespace TDS_Server.Handler.Userpanel
{
    public class UserpanelSettingsNormalHandler : DatabaseEntityWrapper
    {
        private Dictionary<ulong, int> _playerIdWaitingForDiscordUserIdConfirm = new Dictionary<ulong, int>();

        private readonly IModAPI _modAPI;
        private readonly Serializer _serializer;
        private readonly BonusBotConnectorClient _bonusBotConnectorClient;
        private readonly TDSPlayerHandler _tdsPlayerHandler;

        public UserpanelSettingsNormalHandler(Serializer serializer, BonusBotConnectorClient bonusBotConnectorClient, TDSDbContext dbContext, 
            ILoggingHandler loggingHandler, TDSPlayerHandler tdsPlayerHandler, IModAPI modAPI)
            : base(dbContext, loggingHandler)
            => (_modAPI, _serializer, _bonusBotConnectorClient, _tdsPlayerHandler) 
            = (modAPI, serializer, bonusBotConnectorClient, tdsPlayerHandler);

        public async Task<object?> SaveSettings(ITDSPlayer player, object[] args)
        {
            string json = (string)args[0];
            var obj = _serializer.FromBrowser<PlayerSettings>(json);

            var newDiscordUserId = obj.DiscordUserId;
            obj.DiscordUserId = player.Entity!.PlayerSettings.DiscordUserId;
            await player.ExecuteForDBAsync(async (dbContext) =>
            {
                dbContext.Entry(player.Entity.PlayerSettings).CurrentValues.SetValues(obj);
                await dbContext.SaveChangesAsync();
            });

            _modAPI.Thread.RunInMainThread(() =>
            {
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
            });
            
            return null;
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
