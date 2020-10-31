using BonusBotConnector.Client;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Userpanel;
using TDS_Server.Data.Models.Userpanel;
using TDS_Server.Database.Entity;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Extensions;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums.Challenge;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Userpanel
{
    public class UserpanelSettingsNormalHandler : DatabaseEntityWrapper, IUserpanelSettingsNormalHandler
    {
        private readonly BonusBotConnectorClient _bonusBotConnectorClient;
        private readonly ITDSPlayerHandler _tdsPlayerHandler;
        private readonly Dictionary<ulong, int> _playerIdWaitingForDiscordUserIdConfirm = new Dictionary<ulong, int>();

        public UserpanelSettingsNormalHandler(BonusBotConnectorClient bonusBotConnectorClient, TDSDbContext dbContext,
            ITDSPlayerHandler tdsPlayerHandler)
            : base(dbContext)
            => (_bonusBotConnectorClient, _tdsPlayerHandler)
            = (bonusBotConnectorClient, tdsPlayerHandler);

        public async Task<string> ConfirmDiscordUserId(ulong discordUserId)
        {
            if (!_playerIdWaitingForDiscordUserIdConfirm.TryGetValue(discordUserId, out int userId))
                return "This discord user id is not waiting for a confirmation.";

            try
            {
                var player = _tdsPlayerHandler.Get(userId);
                if (player is { })
                    await SaveDiscordUserId(player, discordUserId).ConfigureAwait(false);
                else
                    await SaveDiscordUserId(userId, discordUserId).ConfigureAwait(false);

                return "Discord Id confirmation was successful";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.GetBaseException().Message;
            }
        }

        public async Task<object?> SaveSettings(ITDSPlayer player, ArraySegment<object> args)
        {
            string json = (string)args[0];
            var obj = Serializer.FromBrowser<UserpanelSettingsNormalDataDto>(json);

            var newDiscordUserId = obj.General.DiscordUserId;
            obj.General.DiscordUserId = player.Entity!.PlayerSettings.DiscordUserId;
            obj.ThemeSettings.PlayerId = player.Id;
            await player.Database.ExecuteForDBAsync(async (dbContext) =>
            {
                dbContext.Entry(player.Entity.PlayerSettings).CurrentValues.SetValues(obj.General);
                dbContext.Entry(player.Entity.ThemeSettings).CurrentValues.SetValues(obj.ThemeSettings);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);

            player.Events.TriggerSettingsChanged();

            var generalSettingsJson = Serializer.ToBrowser(obj.General);
            NAPI.Task.RunSafe(() =>
            {
                player.TriggerEvent(ToClientEvent.SyncSettings, generalSettingsJson);

                if (newDiscordUserId != player.Entity.PlayerSettings.DiscordUserId && newDiscordUserId.HasValue)
                {
                    _bonusBotConnectorClient.PrivateChat?.SendMessage(string.Format(player.Language.DISCORD_IDENTITY_CHANGED_BONUSBOT_INFO, player.DisplayName), newDiscordUserId.Value, (reply) =>
                    {
                        if (string.IsNullOrEmpty(reply.ErrorMessage))
                            return;

                        player.SendChatMessage(string.Format(player.Language.DISCORD_IDENTITY_SAVE_FAILED, reply.ErrorMessage));
                    });
                }
            });

            return "";
        }

        private async Task SaveDiscordUserId(int userId, ulong discordUserId)
        {
            var settings = await ExecuteForDBAsync(async dbContext
                => await dbContext.PlayerSettings
                    .FirstOrDefaultAsync(s => s.PlayerId == userId)
                    .ConfigureAwait(false))
                .ConfigureAwait(false);
            if (settings is null)
                throw new Exception("Your player settings do not exist?!");
            settings.DiscordUserId = discordUserId;
            await ExecuteForDBAsync(async dbContext
                => await dbContext
                    .SaveChangesAsync()
                    .ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        private async Task SaveDiscordUserId(ITDSPlayer player, ulong discordUserId)
        {
            player.Entity!.PlayerSettings.DiscordUserId = discordUserId;
            await player.DatabaseHandler.SaveData(true).ConfigureAwait(false);
        }
    }
}
