using GTANetworkAPI;
using System.Threading.Tasks;
using TDS_Common.Default;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.Lobby
{
    partial class MapCreateLobby
    {
        public override async Task<bool> AddPlayer(TDSPlayer player, uint? teamindex)
        {
            if (!await base.AddPlayer(player, 0))
                return false;

            player.Client.Invincible = true;
            player.Client.Position = new Vector3(-365.425, -131.809, 37.873);
            Workaround.FreezePlayer(player.Client, false);

            NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.JoinMapCreatorLobby);

            return true;
        }
    }
}
