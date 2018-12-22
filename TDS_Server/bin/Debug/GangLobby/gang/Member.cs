using System.Collections.Generic;
using TDS.server.instance.player;
using TDS.server.manager.database;

namespace TDS.Instance.Lobby.GangLobby.gang {

    partial class Gang {

		private List<Character> membersOnline = new List<Character>();
		private Dictionary<uint, uint> membersRank = new Dictionary<uint, uint>();

		private void AddMember ( Character character ) {
            playerMemberOfGang[character.UID] = this;
            Database.Exec ( $"INSERT INTO gangmember (memberuid, ganguid) VALUES ({character.UID}, {uid});" );
            membersRank[uid] = 1;
			AddPlayerVehicle ( character.UID );


			MemberCameOnline ( character );

            if ( character.Lobby is GangLobby lobby ) {
                lobby.SetPlayerTeam ( character, this );
            }
        }

        private void RemoveOnlineMember ( Character character, bool saveindb = true ) {
            character.Gang = null;
            membersOnline.Remove ( character );
            RemoveMember ( character.UID, saveindb );

            if ( character.Lobby is GangLobby lobby ) {
                lobby.SetPlayerTeam ( character, 0 );    
            }
        }

        private void RemoveMember ( uint playeruid, bool saveindb = true ) {
            playerMemberOfGang.Remove ( playeruid );
			if ( saveindb ) {
				Database.Exec ( $"DELETE FROM gangmember WHERE memberuid = {playeruid};" );
				RemovePlayerVehicle ( playeruid );
			}
		}

        private void MemberCameOnline ( Character character ) {
            character.Gang = this;
            character.GangRank = membersRank[character.UID];
            membersOnline.Add ( character );
        }

        public static void CheckPlayerGang ( Character character ) {
            uint uid = character.UID;
            if ( playerMemberOfGang.ContainsKey ( uid ) ) {
                playerMemberOfGang[uid].MemberCameOnline ( character );
            }
        }

        private void MemberWentOffline ( Character character )
        {
            membersOnline.Remove ( character );
        }

    }
}