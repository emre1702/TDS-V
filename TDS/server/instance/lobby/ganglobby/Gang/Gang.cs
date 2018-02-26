using GTANetworkAPI;
using System.Collections.Generic;
using TDS.server.extend;
using TDS.server.instance.player;

namespace TDS.server.instance.lobby.ganglobby {

    partial class Gang {
        private static Dictionary<uint, Gang> playerMemberOfGang = new Dictionary<uint, Gang> ();

        private uint uid;
        private string name;
        private string shortname;

        private List<Client> membersOnline = new List<Client> ();

        public Gang ( uint uid, string name, string shortname ) {
            this.uid = uid;
            this.name = name;
            this.shortname = shortname;
        }

        private void MemberCameOnline ( Client player ) {
            Character character = player.GetChar ();
            character.Gang = uid;
        }

        public static void CheckPlayerGang ( Client player ) {
            uint uid = player.GetChar ().UID;
            if ( playerMemberOfGang.ContainsKey ( uid ) ) {
                playerMemberOfGang[uid].MemberCameOnline ( player );
            }
        }
    }
}