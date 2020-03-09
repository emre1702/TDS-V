using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server.Core.Instance.LobbyInstances.Lobby
{
    partial class Lobby
    {
        public void BanPlayer(TDSPlayer admin, TDSPlayer target, DateTime? endTime, string reason)
        {
            if (target.Player is null)
                return;
            if (Players.Contains(target))
                RemovePlayer(target);
            if (target.Entity is null)
                return;
            BanPlayer(admin, target.Entity, endTime, reason, target.Player.Serial);
            if (endTime.HasValue)
            {
                if (LobbyEntity.Type != TDS_Common.Enum.ELobbyType.MainMenu)
                    target.SendMessage(target.Language.TIMEBAN_LOBBY_YOU_INFO.Formatted(endTime.Value.Minute / 60, LobbyEntity.Name, admin.DisplayName, reason));
                else
                    target.SendMessage(target.Language.TIMEBAN_YOU_INFO.Formatted(endTime.Value.Minute / 60, admin.DisplayName, reason));
            }
            else
            {
                if (LobbyEntity.Type != TDS_Common.Enum.ELobbyType.MainMenu)
                    target.SendMessage(target.Language.PERMABAN_LOBBY_YOU_INFO.Formatted(LobbyEntity.Name, admin.DisplayName, reason));
                else
                    target.SendMessage(target.Language.PERMABAN_YOU_INFO.Formatted(admin.DisplayName, reason));
            }
        }

        public async void BanPlayer(TDSPlayer admin, Players target, DateTime? endTime, string reason, string? serial = null)
        {
            if (serial is null)
                serial = await ExecuteForDBAsync(async (dbContext) => await dbContext.LogRests.Where(l => l.Source == target.Id).Select(l => l.Serial).LastOrDefaultAsync());

            PlayerBans? ban = null;
            await ExecuteForDBAsync(async (dbContext) => 
            {
                ban = await dbContext.PlayerBans.FindAsync(target.Id, LobbyEntity.Id);
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
                        LobbyId = LobbyEntity.Id,
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
                if (LobbyEntity.IsOfficial && LobbyEntity.Type != TDS_Common.Enum.ELobbyType.MainMenu)
                    LangUtils.SendAllChatMessage(lang => lang.TIMEBAN_LOBBY_INFO.Formatted(target.Name, (endTime?.Minute ?? 0) / 60, LobbyEntity.Name, admin.DisplayName, reason));
                else if (LobbyEntity.Type == TDS_Common.Enum.ELobbyType.MainMenu)
                    SendAllPlayerLangMessage(lang => lang.TIMEBAN_INFO.Formatted(target.Name, (endTime?.Minute ?? 0) / 60, admin.DisplayName, reason));
                else
                    SendAllPlayerLangMessage(lang => lang.TIMEBAN_LOBBY_INFO.Formatted(target.Name, (endTime?.Minute ?? 0) / 60, LobbyEntity.Name, admin.DisplayName, reason));
            }
            else
            {
                if (LobbyEntity.IsOfficial && LobbyEntity.Type != TDS_Common.Enum.ELobbyType.MainMenu)
                    LangUtils.SendAllChatMessage(lang => lang.PERMABAN_LOBBY_INFO.Formatted(target.Name, LobbyEntity.Name, admin.DisplayName, reason));
                else if (LobbyEntity.Type == TDS_Common.Enum.ELobbyType.MainMenu)
                    SendAllPlayerLangMessage(lang => lang.PERMABAN_INFO.Formatted(target.Name, admin.DisplayName, reason));
                else
                    SendAllPlayerLangMessage(lang => lang.PERMABAN_LOBBY_INFO.Formatted(target.Name, LobbyEntity.Name, admin.DisplayName, reason));
            }

            if (IsOfficial && ban is { })
            {
                var embedFields = BonusBotConnector_Client.Helper.GetBanEmbedFields(ban);
                BonusBotConnector_Client.Requests.ChannelChat.SendBanInfo(ban, embedFields);
                BonusBotConnector_Client.Requests.PrivateChat.SendBanMessage(target.PlayerSettings.DiscordUserId, ban, embedFields);
            }
        }

        public void UnbanPlayer(TDSPlayer admin, TDSPlayer target, string reason)
        {
            if (target.Entity is null)
                return;
            UnbanPlayer(admin, target.Entity, reason);
            if (LobbyEntity.Type != TDS_Common.Enum.ELobbyType.MainMenu)
                target.SendMessage(target.Language.UNBAN_YOU_LOBBY_INFO.Formatted(LobbyEntity.Name, admin.DisplayName, reason));
        }

        public async void UnbanPlayer(TDSPlayer admin, Players target, string reason)
        {
            await ExecuteForDBAsync(async (dbContext) =>
            {
                PlayerBans? ban = await dbContext.PlayerBans.FindAsync(target.Id, LobbyEntity.Id);
                if (ban is null)
                {
                    if (admin.Player is { })
                        admin.SendMessage(admin.Language.PLAYER_ISNT_BANED);
                    return;
                }
                dbContext.PlayerBans.Remove(ban);
                await dbContext.SaveChangesAsync();
            });

            if (LobbyEntity.IsOfficial && LobbyEntity.Type != TDS_Common.Enum.ELobbyType.MainMenu)
                LangUtils.SendAllChatMessage(lang => lang.UNBAN_LOBBY_INFO.Formatted(target.Name, LobbyEntity.Name, admin.AdminLevelName, reason));
            else if (LobbyEntity.Type == TDS_Common.Enum.ELobbyType.MainMenu)
                LangUtils.SendAllChatMessage(lang => lang.UNBAN_INFO.Formatted(target.Name, admin.AdminLevelName, reason));
            else
                SendAllPlayerLangMessage(lang => lang.UNBAN_LOBBY_INFO.Formatted(target.Name, LobbyEntity.Name, admin.AdminLevelName, reason));
        }

        public async Task<bool> IsPlayerBaned(TDSPlayer character)
        {
            if (character.Entity is null)
                return false;
            PlayerBans? ban = await ExecuteForDBAsync(async (dbContext) => await dbContext.PlayerBans.FindAsync(character.Entity.Id, LobbyEntity.Id));
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
                character.SendMessage(Utils.GetReplaced(character.Language.GOT_LOBBY_BAN, duration, ban.Reason));
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
