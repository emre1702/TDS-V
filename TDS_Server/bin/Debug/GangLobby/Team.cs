using System.Collections.Generic;
using TDS.server.instance.lobby.ganglobby;
using TDS.server.instance.player;

namespace TDS.Instance.Lobby.GangLobby {

    partial class GangLobby {

        private Dictionary<Gang, int> gangTeamID = new Dictionary<Gang, int> ();

        public void SetPlayerTeam ( Character character, Gang gang ) {
            SetPlayerTeam ( character, GetGangTeamID ( gang ) );
        }

        private int GetGangTeamID ( Gang gang ) {
            if ( gangTeamID.ContainsKey ( gang ) )
                return gangTeamID[gang];
            int id = Teams.Count;
            
            return id;
        }
    }
}
