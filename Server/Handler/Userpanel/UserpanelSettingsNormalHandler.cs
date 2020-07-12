using BonusBotConnector.Client;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.Userpanel;
using TDS_Server.Data.Models.Userpanel;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Player;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums.Challenge;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Userpanel
{
    public class UserpanelSettingsNormalHandler : DatabaseEntityWrapper, IUserpanelSettingsNormalHandler
    {
        #region Private Fields

        private readonly BonusBotConnectorClient _bonusBotConnectorClient;
        private readonly IModAPI _modAPI;
        private readonly Serializer _serializer;
        private readonly TDSPlayerHandler _tdsPlayerHandler;
        private Dictionary<ulong, int> _playerIdWaitingForDiscordUserIdConfirm = new Dictionary<ulong, int>();

        #endregion Private Fields

        #region Public Constructors

        public UserpanelSettingsNormalHandler(Serializer serializer, BonusBotConnectorClient bonusBotConnectorClient, TDSDbContext dbContext,
            ILoggingHandler loggingHandler, TDSPlayerHandler tdsPlayerHandler, IModAPI modAPI)
            : base(dbContext, loggingHandler)
            => (_modAPI, _serializer, _bonusBotConnectorClient, _tdsPlayerHandler)
            = (modAPI, serializer, bonusBotConnectorClient, tdsPlayerHandler);

        #endregion Public Constructors

        #region Public Methods

        public async Task<string> ConfirmDiscordUserId(ulong discordUserId)
        {
            if (!_playerIdWaitingForDiscordUserIdConfirm.TryGetValue(discordUserId, out int userId))
                return "This discord user id is not waiting for a confirmation.";

            try
            {
                var player = _tdsPlayerHandler.GetIfExists(userId);
                if (player is { })
                    await SaveDiscordUserId(player, discordUserId);
                else
                    await SaveDiscordUserId(userId, discordUserId);

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
            var obj = _serializer.FromBrowser<UserpanelSettingsNormalDataDto>(json);

            var newDiscordUserId = obj.General.DiscordUserId;
            obj.General.DiscordUserId = player.Entity!.PlayerSettings.DiscordUserId;
            await player.ExecuteForDBAsync(async (dbContext) =>
            {
                dbContext.Entry(player.Entity.PlayerSettings).CurrentValues.SetValues(obj.General);
                dbContext.Entry(player.Entity.ThemeSettings).CurrentValues.SetValues(obj.ThemeSettings);
                await dbContext.SaveChangesAsync();
            });

            _modAPI.Thread.QueueIntoMainThread(() =>
            {
                player.LoadTimezone();
                player.AddToChallenge(ChallengeType.ChangeSettings);

                player.SendEvent(ToClientEvent.SyncSettings, _serializer.ToBrowser(obj.General));

                if (newDiscordUserId != player.Entity.PlayerSettings.DiscordUserId && newDiscordUserId.HasValue)
                {
                    _bonusBotConnectorClient.PrivateChat?.SendMessage(string.Format(player.Language.DISCORD_IDENTITY_CHANGED_BONUSBOT_INFO, player.DisplayName), newDiscordUserId.Value, (reply) =>
                    {
                        if (string.IsNullOrEmpty(reply.ErrorMessage))
                            return;

                        player.SendMessage(string.Format(player.Language.DISCORD_IDENTITY_SAVE_FAILED, reply.ErrorMessage));
                    });
                }
            });

            return null;
        }

        #endregion Public Methods

        #region Private Methods

        private async Task SaveDiscordUserId(int userId, ulong discordUserId)
        {
            var settings = await ExecuteForDBAsync(async dbContext
                => await dbContext.PlayerSettings.FirstOrDefaultAsync(s => s.PlayerId == userId));
            if (settings is null)
                throw new Exception("Your player settings do not exist?!");
            settings.DiscordUserId = discordUserId;
            await ExecuteForDBAsync(async dbContext
                => await dbContext.SaveChangesAsync());
        }

        private async Task SaveDiscordUserId(ITDSPlayer player, ulong discordUserId)
        {
            player.Entity!.PlayerSettings.DiscordUserId = discordUserId;
            await player.SaveData(true);
        }

        #endregion Private Methods
    }
}
