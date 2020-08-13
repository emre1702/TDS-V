using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Utility;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Entity.LobbySystem.BaseSystem
{
    partial class Lobby
    {
        #region Public Methods

        public async Task<PlayerBans?> BanPlayer(ITDSPlayer admin, ITDSPlayer target, TimeSpan? length, string reason)
        {
            if (target.ModPlayer is null)
                return null;
            if (Players.ContainsKey(target.Id))
                await RemovePlayer(target);
            if (target.Entity is null)
                return null;
            var ban = await BanPlayer(admin, target.Entity, length, reason, target.ModPlayer.Serial);
            if (ban is null)
                return null;

            ModAPI.Thread.QueueIntoMainThread(() =>
            {
                if (length.HasValue)
                {
                    double hours = length.Value.TotalMinutes / 60;
                    if (Entity.Type != TDS_Shared.Data.Enums.LobbyType.MainMenu)
                        target.SendMessage(string.Format(target.Language.TIMEBAN_LOBBY_YOU_INFO, hours, Entity.Name, admin.DisplayName, reason));
                    else
                        target.SendMessage(string.Format(target.Language.TIMEBAN_YOU_INFO, hours, admin.DisplayName, reason));
                }
                else
                {
                    if (Entity.Type != TDS_Shared.Data.Enums.LobbyType.MainMenu)
                        target.SendMessage(string.Format(target.Language.PERMABAN_LOBBY_YOU_INFO, Entity.Name, admin.DisplayName, reason));
                    else
                        target.SendMessage(string.Format(target.Language.PERMABAN_YOU_INFO, admin.DisplayName, reason));
                }
            });
            return ban;
        }

        public async Task<PlayerBans?> BanPlayer(ITDSPlayer admin, Players target, TimeSpan? length, string reason, string? serial = null)
        {
            int targetId = target.Id;
            if (serial is null)
                serial = await ExecuteForDBAsync(async (dbContext) => await dbContext.LogRests.Where(l => l.Source == targetId).OrderBy(l => l.Id).Select(l => l.Serial).LastOrDefaultAsync());

            PlayerBans? ban = null;
            await ExecuteForDBAsync(async (dbContext) =>
            {
                ban = await dbContext.PlayerBans.FindAsync(target.Id, Entity.Id);
                if (ban != null)
                {
                    ban.AdminId = admin.Entity?.Id ?? -1;
                    ban.Serial = serial;
                    ban.StartTimestamp = DateTime.UtcNow;
                    ban.EndTimestamp = DateTime.UtcNow + length;
                    ban.Reason = reason;
                }
                else
                {
                    ban = new PlayerBans()
                    {
                        PlayerId = target.Id,
                        LobbyId = Entity.Id,
                        Serial = serial,
                        AdminId = admin.Entity?.Id ?? -1,
                        EndTimestamp = DateTime.UtcNow + length,
                        Reason = reason
                    };
                    dbContext.PlayerBans.Add(ban);
                }
                await dbContext.SaveChangesAsync();
            });

            ModAPI.Thread.QueueIntoMainThread(() =>
            {
                if (length.HasValue)
                {
                    if (Entity.IsOfficial && Entity.Type != TDS_Shared.Data.Enums.LobbyType.MainMenu)
                        LangHelper.SendAllChatMessage(lang => string.Format(lang.TIMEBAN_LOBBY_INFO, target.Name, length.Value.TotalMinutes / 60, Entity.Name, admin.DisplayName, reason));
                    else if (Entity.Type == TDS_Shared.Data.Enums.LobbyType.MainMenu)
                        SendAllPlayerLangMessage(lang => string.Format(lang.TIMEBAN_INFO, target.Name, length.Value.TotalMinutes / 60, admin.DisplayName, reason));
                    else
                        SendAllPlayerLangMessage(lang => string.Format(lang.TIMEBAN_LOBBY_INFO, target.Name, length.Value.TotalMinutes / 60, Entity.Name, admin.DisplayName, reason));
                }
                else
                {
                    if (Entity.IsOfficial && Entity.Type != TDS_Shared.Data.Enums.LobbyType.MainMenu)
                        LangHelper.SendAllChatMessage(lang => string.Format(lang.PERMABAN_LOBBY_INFO, target.Name, Entity.Name, admin.DisplayName, reason));
                    else if (Entity.Type == TDS_Shared.Data.Enums.LobbyType.MainMenu)
                        SendAllPlayerLangMessage(lang => string.Format(lang.PERMABAN_INFO, target.Name, admin.DisplayName, reason));
                    else
                        SendAllPlayerLangMessage(lang => string.Format(lang.PERMABAN_LOBBY_INFO, target.Name, Entity.Name, admin.DisplayName, reason));
                }

                if (IsOfficial && ban is { })
                {
                    if (ban.LobbyId == LobbiesHandler.MainMenu.Id)
                        BansHandler.AddServerBan(ban);
                    var embedFields = BonusBotConnectorClient.Helper?.GetBanEmbedFields(ban);
                    if (embedFields is { })
                    {
                        BonusBotConnectorClient.ChannelChat?.SendBanInfo(ban, embedFields);
                        if (target.PlayerSettings.DiscordUserId.HasValue)
                            BonusBotConnectorClient.PrivateChat?.SendBanMessage(target.PlayerSettings.DiscordUserId.Value, ban, embedFields);
                    }
                }
            });

            return ban;
        }

        public async Task<bool> IsPlayerBaned(ITDSPlayer character)
        {
            if (character.Entity is null)
                return false;
            PlayerBans? ban = await ExecuteForDBAsync(async (dbContext) => await dbContext.PlayerBans.FindAsync(character.Entity.Id, Entity.Id));
            if (ban is null)
                return false;

            // !ban.EndTimestamp.HasValue => permaban
            if (!ban.EndTimestamp.HasValue || ban.EndTimestamp.Value > DateTime.UtcNow)
            {
                string duration = "-";
                if (ban.EndTimestamp.HasValue)
                {
                    duration = DateTime.UtcNow.DurationTo(ban.EndTimestamp.Value);
                }
                ModAPI.Thread.QueueIntoMainThread(() => character.SendMessage(string.Format(character.Language.GOT_LOBBY_BAN, duration, ban.Reason)));
                return true;
            }
            else if (ban.EndTimestamp.HasValue)
            {
                await ExecuteForDBAsync(async (dbContext) =>
                {
                    dbContext.Remove(ban);
                    await dbContext.SaveChangesAsync();
                });
            }
            return false;
        }

        public void UnbanPlayer(ITDSPlayer admin, ITDSPlayer target, string reason)
        {
            if (target.Entity is null)
                return;
            UnbanPlayer(admin, target.Entity, reason);
            if (Entity.Type != TDS_Shared.Data.Enums.LobbyType.MainMenu)
                target.SendMessage(string.Format(target.Language.UNBAN_YOU_LOBBY_INFO, Entity.Name, admin.DisplayName, reason));
        }

        public async void UnbanPlayer(ITDSPlayer admin, Players target, string reason)
        {
            await ExecuteForDBAsync(async (dbContext) =>
            {
                PlayerBans? ban = await dbContext.PlayerBans.FindAsync(target.Id, Entity.Id);
                if (ban is null)
                {
                    ModAPI.Thread.QueueIntoMainThread(() => admin.SendMessage(admin.Language.PLAYER_ISNT_BANED));
                    return;
                }
                dbContext.PlayerBans.Remove(ban);
                await dbContext.SaveChangesAsync();

                if (ban.LobbyId == LobbiesHandler.MainMenu.Id)
                    ModAPI.Thread.QueueIntoMainThread(() => BansHandler.RemoveServerBanByPlayerId(ban));
            });

            ModAPI.Thread.QueueIntoMainThread(() =>
            {
                if (Entity.IsOfficial && Entity.Type != TDS_Shared.Data.Enums.LobbyType.MainMenu)
                    LangHelper.SendAllChatMessage(lang => string.Format(lang.UNBAN_LOBBY_INFO, target.Name, Entity.Name, admin.AdminLevelName, reason));
                else if (Entity.Type == TDS_Shared.Data.Enums.LobbyType.MainMenu)
                    LangHelper.SendAllChatMessage(lang => string.Format(lang.UNBAN_INFO, target.Name, admin.AdminLevelName, reason));
                else
                    SendAllPlayerLangMessage(lang => string.Format(lang.UNBAN_LOBBY_INFO, target.Name, Entity.Name, admin.AdminLevelName, reason));
            });
        }

        #endregion Public Methods
    }
}
