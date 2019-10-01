using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server.Instance.Lobby
{
    partial class Lobby
    {
        public void BanPlayer(TDSPlayer admin, TDSPlayer target, DateTime? endTime, string reason)
        {
            if (Players.Contains(target))
                RemovePlayer(target);
            if (target.Entity is null)
                return;
            BanPlayer(admin, target.Entity, endTime, reason, target.Client.Serial);
            if (endTime.HasValue)
            {
                if (LobbyEntity.Id != 0)
                    NAPI.Chat.SendChatMessageToPlayer(target.Client, target.Language.TIMEBAN_LOBBY_YOU_INFO.Formatted(endTime.Value.Minute / 60, LobbyEntity.Name, admin.AdminLevelName, reason));
                else
                    NAPI.Chat.SendChatMessageToPlayer(target.Client, target.Language.TIMEBAN_YOU_INFO.Formatted(endTime.Value.Minute / 60, admin.AdminLevelName, reason));
            }
            else
            {
                if (LobbyEntity.Id != 0)
                    NAPI.Chat.SendChatMessageToPlayer(target.Client, target.Language.PERMABAN_LOBBY_YOU_INFO.Formatted(LobbyEntity.Name, admin.AdminLevelName, reason));
                else
                    NAPI.Chat.SendChatMessageToPlayer(target.Client, target.Language.PERMABAN_YOU_INFO.Formatted(admin.AdminLevelName, reason));
            }
        }

        public async void BanPlayer(TDSPlayer admin, Players target, DateTime? endTime, string reason, string? serial = null)
        {
            if (serial is null)
                serial = await DbContext.LogRests.Where(l => l.Source == target.Id).Select(l => l.Serial).LastOrDefaultAsync();

            PlayerBans? ban = await DbContext.PlayerBans.FindAsync(target.Id, LobbyEntity.Id);
            if (ban != null)
            {
                ban.AdminId = admin.Entity?.Id ?? 0;
                ban.Serial = serial;
                ban.StartTimestamp = DateTime.Now;
                ban.EndTimestamp = endTime;
                ban.Reason = reason;
            }
            else
            {
                ban = new PlayerBans()
                {
                    PlayerId = target.Id,
                    LobbyId = LobbyEntity.Id,
                    Serial = serial,
                    AdminId = admin.Entity?.Id ?? 0,
                    EndTimestamp = endTime,
                    Reason = reason
                };
                await DbContext.PlayerBans.AddAsync(ban);
            }
            await DbContext.SaveChangesAsync();

            if (endTime.HasValue)
            {
                if (LobbyEntity.IsOfficial && LobbyEntity.Id != 0)
                    LangUtils.SendAllChatMessage(lang => lang.TIMEBAN_LOBBY_INFO.Formatted(target.Name, (endTime?.Minute ?? 0) / 60, LobbyEntity.Name, admin.AdminLevelName, reason));
                else if (LobbyEntity.Id == 0)
                    SendAllPlayerLangMessage(lang => lang.TIMEBAN_INFO.Formatted(target.Name, (endTime?.Minute ?? 0) / 60, admin.AdminLevelName, reason));
                else
                    SendAllPlayerLangMessage(lang => lang.TIMEBAN_LOBBY_INFO.Formatted(target.Name, (endTime?.Minute ?? 0) / 60, LobbyEntity.Name, admin.AdminLevelName, reason));
            }
            else
            {
                if (LobbyEntity.IsOfficial && LobbyEntity.Id != 0)
                    LangUtils.SendAllChatMessage(lang => lang.PERMABAN_LOBBY_INFO.Formatted(target.Name, LobbyEntity.Name, admin.AdminLevelName, reason));
                else if (LobbyEntity.Id == 0)
                    SendAllPlayerLangMessage(lang => lang.PERMABAN_INFO.Formatted(target.Name, admin.AdminLevelName, reason));
                else
                    SendAllPlayerLangMessage(lang => lang.PERMABAN_LOBBY_INFO.Formatted(target.Name, LobbyEntity.Name, admin.AdminLevelName, reason));
            }
        }

        public void UnbanPlayer(TDSPlayer admin, TDSPlayer target, string reason)
        {
            if (target.Entity is null)
                return;
            UnbanPlayer(admin, target.Entity, reason);
            if (LobbyEntity.Id != 0)
                NAPI.Chat.SendChatMessageToPlayer(target.Client, target.Language.UNBAN_YOU_LOBBY_INFO.Formatted(LobbyEntity.Name, admin.AdminLevelName, reason));
        }

        public async void UnbanPlayer(TDSPlayer admin, Players target, string reason)
        {
            PlayerBans? ban = await DbContext.PlayerBans.FindAsync(target.Id, LobbyEntity.Id);
            if (ban is null)
            {
                NAPI.Chat.SendChatMessageToPlayer(admin.Client, admin.Language.PLAYER_ISNT_BANED);
                return;
            }
            DbContext.PlayerBans.Remove(ban);
            await DbContext.SaveChangesAsync();

            if (LobbyEntity.IsOfficial && LobbyEntity.Id != 0)
                LangUtils.SendAllChatMessage(lang => lang.UNBAN_LOBBY_INFO.Formatted(target.Name, LobbyEntity.Name, admin.AdminLevelName, reason));
            else if (LobbyEntity.Id == 0)
                LangUtils.SendAllChatMessage(lang => lang.UNBAN_INFO.Formatted(target.Name, admin.AdminLevelName, reason));
            else
                SendAllPlayerLangMessage(lang => lang.UNBAN_LOBBY_INFO.Formatted(target.Name, LobbyEntity.Name, admin.AdminLevelName, reason));
        }

        private async Task<bool> IsPlayerBaned(TDSPlayer character)
        {
            if (character.Entity is null)
                return false;
            PlayerBans? ban = await DbContext.PlayerBans.FindAsync(character.Entity.Id, LobbyEntity.Id);
            if (ban is null)
                return false;

            // !ban.EndTimestamp.HasValue => permaban
            if (!ban.EndTimestamp.HasValue || ban.EndTimestamp.Value > DateTime.Now)
            {
                string duration = "-";
                if (ban.EndTimestamp.HasValue)
                {
                    duration = DateTime.Now.DurationTo(ban.EndTimestamp.Value);
                }
                NAPI.Chat.SendChatMessageToPlayer(character.Client, Utils.GetReplaced(character.Language.GOT_LOBBY_BAN, duration, ban.Reason));
                return true;
            }
            else if (ban.EndTimestamp.HasValue)
            {
                DbContext.Remove(ban);
                await DbContext.SaveChangesAsync();
            }
            return false;
        }
    }
}