using TDS.server.instance.lobby.ganglobby;
using TDS.server.instance.player;

namespace TDS.Instance.Lobby.GangLobby {
    partial class GangLobby {

        public override bool AddPlayer ( Character character, bool spectator = false ) {
			if ( !base.AddPlayer ( character, spectator ) )
				return false;

            character.Player.Freeze ( false );

            if ( character.Gang != null ) {
                SetPlayerTeam ( character, character.Gang );
            }

			return true;
        }
    }
}
