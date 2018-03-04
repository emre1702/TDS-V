using TDS.server.instance.player;
using TDS.server.manager.database;

namespace TDS.server.instance.lobby.ganglobby {

    partial class Gang {

        private void AddMember ( Character character ) {
            playerMemberOfGang[character.UID] = this;
            Database.Exec ( $"INSERT INTO gangmember (memberuid, ganguid) VALUES ({character.UID}, {uid});" ); 
            MemberCameOnline ( character );

            if ( character.Lobby is GangLobby lobby ) {
                lobby.SetPlayerTeam ( character, this );
            }
        }

        private void RemoveOnlineMember ( Character character ) {
            character.Gang = null;
            RemoveMember ( character.UID );

            if ( character.Lobby is GangLobby lobby ) {
                lobby.SetPlayerTeam ( character, 0 );    
            }
        }

        private void RemoveMember ( uint playeruid ) {
            playerMemberOfGang.Remove ( playeruid );
            Database.Exec ( $"DELETE FROM gangmember WHERE memberuid = {playeruid};" );
        }

        private void MemberCameOnline ( Character character ) {
            character.Gang = this;
        }

        public static void CheckPlayerGang ( Character character ) {
            uint uid = character.UID;
            if ( playerMemberOfGang.ContainsKey ( uid ) ) {
                playerMemberOfGang[uid].MemberCameOnline ( character );
            }
        }

    }
}