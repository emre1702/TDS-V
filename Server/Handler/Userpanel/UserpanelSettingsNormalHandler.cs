using BonusBotConnector.Client;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.Userpanel;
using TDS.Server.Database.Entity;
using TDS.Server.Database.Entity.Player.Settings;
using TDS.Server.Database.Interfaces;
using TDS.Server.Handler.Entities;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Core;

namespace TDS.Server.Handler.Userpanel
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
                var player = _tdsPlayerHandler.GetPlayer(userId);
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
            var type = (UserpanelSettingsNormalType)Convert.ToInt32(args[0]);
            var json = (string)args[1];
            var setting = GetSetting(player, type);
            if (setting is null)
                return null;

            var obj = (IPlayerDataTable)Serializer.FromBrowser(setting.GetType(), json);
            obj.PlayerId = setting.PlayerId;

            if (obj is PlayerGeneralSettings generalSettings)
                HandleDiscordUserIdChange(player, (PlayerGeneralSettings)setting, generalSettings);

            await player.Database.ExecuteForDBAsync(async (dbContext) =>
            {
                dbContext.Entry(setting).CurrentValues.SetValues(obj);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);

            player.Events.TriggerSettingsChanged();

            return "";
        }

        private async Task SaveDiscordUserId(int userId, ulong discordUserId)
        {
            var player = await ExecuteForDBAsync(async dbContext
                => await dbContext.Players
                    .FirstOrDefaultAsync(s => s.Id == userId)
                    .ConfigureAwait(false))
                .ConfigureAwait(false);
            var settings = await ExecuteForDBAsync(async dbContext
                => await dbContext.PlayerSettings
                    .FirstOrDefaultAsync(s => s.PlayerId == userId)
                    .ConfigureAwait(false))
                .ConfigureAwait(false);
            if (settings is null)
                throw new Exception("Your player settings do not exist?!");

            player.DiscordUserId = discordUserId;
            settings.General.DiscordUserId = discordUserId;

            await ExecuteForDBAsync(async dbContext
                => await dbContext
                    .SaveChangesAsync()
                    .ConfigureAwait(false), dbContext => {
                        dbContext.Entry(player).State = EntityState.Detached;
                        dbContext.Entry(settings).State = EntityState.Detached;
                    })
                .ConfigureAwait(false);
        }

        private async Task SaveDiscordUserId(ITDSPlayer player, ulong discordUserId)
        {
            player.Entity!.PlayerSettings.General.DiscordUserId = discordUserId;
            player.Entity.DiscordUserId = discordUserId;
            await player.DatabaseHandler.SaveData(true).ConfigureAwait(false);
        }

        private void HandleDiscordUserIdChange(ITDSPlayer player, PlayerGeneralSettings originalSettings, PlayerGeneralSettings newSettings)
        {
            var newDiscordUserId = newSettings.DiscordUserId;
            if (newDiscordUserId == originalSettings.DiscordUserId || newDiscordUserId is null)
                return;

            // The player gets asked in Discord if he wants to confirm it.
            // This setting will be saved after the confirmation (in another method).
            newSettings.DiscordUserId = originalSettings.DiscordUserId;
            _playerIdWaitingForDiscordUserIdConfirm[newDiscordUserId.Value] = player.Id;

            _bonusBotConnectorClient.PrivateChat?.SendMessage(string.Format(player.Language.DISCORD_IDENTITY_CHANGED_BONUSBOT_INFO, player.DisplayName),
                newDiscordUserId.Value, (reply) =>
            {
                if (string.IsNullOrEmpty(reply.ErrorMessage))
                    return;

                NAPI.Task.RunSafe(() =>
                    player.SendChatMessage(string.Format(player.Language.DISCORD_IDENTITY_SAVE_FAILED, reply.ErrorMessage)));
            });
        }

        public object? LoadSettings(ITDSPlayer player, ref ArraySegment<object> args)
        {
            var type = (UserpanelSettingsNormalType)Convert.ToInt32(args[0]);
            var setting = GetSetting(player, type);
            if (setting is null)
                return null;

            return Serializer.ToBrowser(setting);
        }

        private IPlayerDataTable? GetSetting(ITDSPlayer player, UserpanelSettingsNormalType type)
            => type switch
            {
                UserpanelSettingsNormalType.Chat => player.Entity?.PlayerSettings.Chat,
                UserpanelSettingsNormalType.CooldownsAndDurations => player.Entity?.PlayerSettings.CooldownsAndDurations,
                UserpanelSettingsNormalType.FightEffect => player.Entity?.PlayerSettings.FightEffect,
                UserpanelSettingsNormalType.General => player.Entity?.PlayerSettings.General,
                UserpanelSettingsNormalType.Info => player.Entity?.PlayerSettings.Info,
                UserpanelSettingsNormalType.IngameColors => player.Entity?.PlayerSettings.IngameColors,
                UserpanelSettingsNormalType.Hud => player.Entity?.PlayerSettings.Hud,
                UserpanelSettingsNormalType.KillInfo => player.Entity?.KillInfoSettings,
                UserpanelSettingsNormalType.Scoreboard => player.Entity?.PlayerSettings.Scoreboard,
                UserpanelSettingsNormalType.Theme => player.Entity?.ThemeSettings,
                UserpanelSettingsNormalType.Voice => player.Entity?.PlayerSettings.Voice,
                _ => null
            };
    }
}
