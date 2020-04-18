using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class GangLobby
    {
        public async override Task<bool> AddPlayer(ITDSPlayer player, uint? teamindex)
        {
            var team = player.Gang.GangLobbyTeam;
            
            if (!await base.AddPlayer(player, null))
                return false;
            team.AddPlayer(player);

            player.ModPlayer?.Freeze(false);
            player.ModPlayer?.SetInvincible(true);



            return true;
        }
    }
}
