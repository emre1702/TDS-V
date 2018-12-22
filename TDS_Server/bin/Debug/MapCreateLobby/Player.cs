using GTANetworkAPI;
using System.Threading.Tasks;
using TDS.Default;
using TDS.Instance.Player;

namespace TDS.Instance.Lobby
{

    partial class MapCreateLobby
    {

        public override async Task<bool> AddPlayer(Character character, uint teamid)
        {
            if (!await base.AddPlayer(character, teamid))
                return false;

            NAPI.ClientEvent.TriggerClientEvent(character.Player, DCustomEvents.ClientPlayerJoinMapCreatorLobby);
            character.Player.Freeze(false);

            return true;
        }
    }
}
