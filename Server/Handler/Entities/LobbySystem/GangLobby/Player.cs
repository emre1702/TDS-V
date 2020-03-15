using System.Threading.Tasks;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class GangLobby
    {
        public async override Task<bool> AddPlayer(TDSPlayer player, uint? teamindex)
        {
            if (!await base.AddPlayer(player, teamindex))
                return false;

            Workaround.FreezePlayer(player.Player!, false);

            return true;
        }
    }
}
