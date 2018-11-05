using System;
using System.Threading.Tasks;
using TDS.Entity;
using TDS.Instance.Player;

namespace TDS.Instance.Lobby
{

    public class LobbyBan
    {
        public sbyte Type;
        public string End;
        public uint EndSpan;
        public string ByAdmin;
        public string Reason;
    }

    partial class Lobby
    {
        private async Task<bool> IsPlayerBaned(Character character, TDSNewContext dbcontext)
        {
            Playerbans ban = await dbcontext.Playerbans.FindAsync(character.Entity.Id, this.entity.Id);
            if (ban == null)
                return false;

            // !ban.EndTimestamp.HasValue => permaban
            if (!ban.EndTimestamp.HasValue || ban.EndTimestamp.Value > DateTime.Now)
            {
#warning Todo: Add output
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
