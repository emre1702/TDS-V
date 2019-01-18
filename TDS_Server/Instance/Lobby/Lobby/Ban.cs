using GTANetworkAPI;
using System;
using System.Threading.Tasks;
using TDS_Server.Entity;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.Lobby
{
    partial class Lobby
    {
        public async void BanPlayer(TDSPlayer admin, TDSPlayer target, DateTime? endTime, string reason)
        {
            using (var dbcontext = new TDSNewContext())
            {
                Playerbans ban = await dbcontext.Playerbans.FindAsync(target.Entity.Id, LobbyEntity.Id);
                if (ban != null)
                {
                    ban.Admin = admin.Entity.Id;
                    ban.StartTimestamp = DateTime.Now;
                    ban.EndTimestamp = endTime;
                    ban.Reason = reason;
                }
                else
                {
                    ban = new Playerbans()
                    {
                        Id = target.Entity.Id,
                        ForLobby = LobbyEntity.Id,
                        Admin = admin.Entity.Id,
                        EndTimestamp = endTime,
                        Reason = reason
                    };
                    await dbcontext.Playerbans.AddAsync(ban);
                }
                await dbcontext.SaveChangesAsync();
            }
            if (endTime.HasValue)
                SendAllPlayerLangMessage(lang => lang.TIMEBAN_LOBBY_INFO.Formatted(target.Client.Name, LobbyEntity.Name, admin.AdminLevelName, reason));
            else
                SendAllPlayerLangMessage(lang => lang.PERMABAN_LOBBY_INFO.Formatted(target.Client.Name, LobbyEntity.Name, admin.AdminLevelName, reason));
            if (Players.Contains(target))
                RemovePlayer(target);
        }

        public async void UnbanPlayer(TDSPlayer admin, TDSPlayer target, string reason)
        {
            using (var dbcontext = new TDSNewContext())
            {
                Playerbans ban = await dbcontext.Playerbans.FindAsync(target.Entity.Id, LobbyEntity.Id);
                if (ban == null)
                {
                    NAPI.Chat.SendChatMessageToPlayer(admin.Client, admin.Language.PLAYER_ISNT_BANED);
                    return;
                }
                dbcontext.Playerbans.Remove(ban);
                await dbcontext.SaveChangesAsync();
            }
            SendAllPlayerLangMessage(lang => lang.UNBAN_LOBBY_INFO.Formatted(target.Client.Name, LobbyEntity.Name, admin.AdminLevelName, reason));
            NAPI.Chat.SendChatMessageToPlayer(target.Client, target.Language.UNBAN_YOU_LOBBY_INFO.Formatted(LobbyEntity.Name, admin.AdminLevelName, reason));
        }

        private async Task<bool> IsPlayerBaned(TDSPlayer character, TDSNewContext dbcontext)
        {
            Playerbans ban = await dbcontext.Playerbans.FindAsync(character.Entity.Id, LobbyEntity.Id);
            if (ban == null)
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
                dbcontext.Remove(ban);
                await dbcontext.SaveChangesAsync();
            }
            return false;
        }
    }
}
