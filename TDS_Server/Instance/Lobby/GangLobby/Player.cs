using System.Threading.Tasks;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.Lobby
{
    partial class GangLobby
    {
        public async override Task<bool> AddPlayer(TDSPlayer character, uint? teamindex)
        {
            //teamindex = character.Gang.Index;
            if (!await base.AddPlayer(character, teamindex))
                return false;

            Workaround.FreezePlayer(character.Client, false);

            return true;
        }

        /*public override bool AddPlayer ( Character character, bool spectator = false ) {
			if ( !base.AddPlayer ( character, spectator ) )
				return false;

            Workaround.FreezePlayer(player.Client, false);

            if ( character.Gang != null ) {
                SetPlayerTeam ( character, character.Gang );
            }

			return true;
        }*/
    }
}