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
