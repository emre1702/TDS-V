using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class GangLobby
    {
        public async override Task<bool> AddPlayer(ITDSPlayer player, uint? teamindex)
        {
            if (!await base.AddPlayer(player, teamindex))
                return false;

            player.ModPlayer?.Freeze(false);

            return true;
        }
    }
}
