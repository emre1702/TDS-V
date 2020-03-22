using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Utility;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Entities.Player;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class Lobby
    {
        public void BanPlayer(ITDSPlayer admin, ITDSPlayer target, DateTime? endTime, string reason)
        {
            if (target.ModPlayer is null)
                return;
            if (Players.Contains(target))
                RemovePlayer(target);
            if (target.Entity is null)
                return;
            BanPlayer(admin, target.Entity, endTime, reason, target.ModPlayer.Serial);
            if (endTime.HasValue)
            {
                if (Entity.Type != TDS_Shared.Data.Enums.LobbyType.MainMenu)
                    target.SendMessage(string.Format(target.Language.TIMEBAN_LOBBY_YOU_INFO, endTime.Value.Minute / 60, Entity.Name, admin.DisplayName, reason));
                else
                    target.SendMessage(string.Format(target.Language.TIMEBAN_YOU_INFO, endTime.Value.Minute / 60, admin.DisplayName, reason));
            }
            else
            {
                if (Entity.Type != TDS_Shared.Data.Enums.LobbyType.MainMenu)
                    target.SendMessage(string.Format(target.Language.PERMABAN_LOBBY_YOU_INFO, Entity.Name, admin.DisplayName, reason));
                else
                    target.SendMessage(string.Format(target.Language.PERMABAN_YOU_INFO, admin.DisplayName, reason));
            }
        }

        public async void BanPlayer(ITDSPlayer admin, Players target, DateTime? endTime, string reason, string? serial = null)
        {
            if (serial is null)
                serial = await ExecuteForDBAsync(async (dbContext) => await dbContext.LogRests.Where(l => l.Source == target.Id).Select(l => l.Serial).LastOrDefaultAsync());

            PlayerBans? ban = null;
            await ExecuteForDBAsync(async (dbContext) =>
            {
                ban = await dbContext.PlayerBans.FindAsync(target.Id, Entity.Id);
                if (ban != null)
                {
                    ban.AdminId = admin.Entity?.Id ?? 0;
                    ban.Serial = serial;
                    ban.StartTimestamp = DateTime.UtcNow;
                    ban.EndTimestamp = endTime;
                    ban.Reason = reason;
                }
                else
                {
                    ban = new PlayerBans()
                    {
                        PlayerId = target.Id,
                        LobbyId = Entity.Id,
                        Serial = serial,
                        AdminId = admin.Entity?.Id ?? 0,
                        EndTimestamp = endTime,
                        Reason = reason
                    };
                    dbContext.PlayerBans.Add(ban);
                }
                await dbContext.SaveChangesAsync();
            });

            if (endTime.HasValue)
            {
                if (Entity.IsOfficial && Entity.Type != TDS_Shared.Data.Enums.LobbyType.MainMenu)
                    LangHelper.SendAllChatMessage(lang => string.Format(lang.TIMEBAN_LOBBY_INFO, target.Name, (endTime?.Minute ?? 0) / 60, Entity.Name, admin.DisplayName, reason));
                else if (Entity.Type == TDS_Shared.Data.Enums.LobbyType.MainMenu)
                    SendAllPlayerLangMessage(lang => string.Format(lang.TIMEBAN_INFO, target.Name, (endTime?.Minute ?? 0) / 60, admin.DisplayName, reason));
                else
                    SendAllPlayerLangMessage(lang => string.Format(lang.TIMEBAN_LOBBY_INFO, target.Name, (endTime?.Minute ?? 0) / 60, Entity.Name, admin.DisplayName, reason));
            }
            else
            {
                if (Entity.IsOfficial && Entity.Type != TDS_Shared.Data.Enums.LobbyType.MainMenu)
                    LangHelper.SendAllChatMessage(lang => lang.PERMABAN_LOBBY_INFO.Formatted(target.Name, Entity.Name, admin.DisplayName, reason));
                else if (Entity.Type == TDS_Shared.Data.Enums.LobbyType.MainMenu)
                    SendAllPlayerLangMessage(lang => string.Format(lang.PERMABAN_INFO, target.Name, admin.DisplayName, reason));
                else
                    SendAllPlayerLangMessage(lang => string.Format(lang.PERMABAN_LOBBY_INFO, target.Name, Entity.Name, admin.DisplayName, reason));
            }

            if (IsOfficial && ban is { })
            {
                var embedFields = BonusBotConnectorClient.Helper?.GetBanEmbedFields(ban);
                if (embedFields is null)
                    return;
                BonusBotConnectorClient.ChannelChat?.SendBanInfo(ban, embedFields);
                BonusBotConnectorClient.PrivateChat?.SendBanMessage(target.PlayerSettings.DiscordUserId, ban, embedFields);
            }
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
                    if (admin.ModPlayer is { })
                        admin.SendMessage(admin.Language.PLAYER_ISNT_BANED);
                    return;
                }
                dbContext.PlayerBans.Remove(ban);
                await dbContext.SaveChangesAsync();
            });

            if (Entity.IsOfficial && Entity.Type != TDS_Shared.Data.Enums.LobbyType.MainMenu)
                LangHelper.SendAllChatMessage(lang => string.Format(lang.UNBAN_LOBBY_INFO, target.Name, Entity.Name, admin.AdminLevelName, reason));
            else if (Entity.Type == TDS_Shared.Data.Enums.LobbyType.MainMenu)
                LangHelper.SendAllChatMessage(lang => string.Format(lang.UNBAN_INFO, target.Name, admin.AdminLevelName, reason));
            else
                SendAllPlayerLangMessage(lang => string.Format(lang.UNBAN_LOBBY_INFO, target.Name, Entity.Name, admin.AdminLevelName, reason));
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
                character.SendMessage(string.Format(character.Language.GOT_LOBBY_BAN, duration, ban.Reason));
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
    }
}
