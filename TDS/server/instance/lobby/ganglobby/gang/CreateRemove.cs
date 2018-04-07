using System.Collections.Generic;
using System.Linq;
using TDS.server.instance.player;
using TDS.server.manager.database;

namespace TDS.server.instance.lobby.ganglobby {

	partial class Gang {
		public static void CreateGang ( Character character, string name, string shortname ) {
			uint ganguid = gangByUID.Keys.Max () + 1;
			uint owneruid = character.UID;

			Gang gang = new Gang ( ganguid, name, shortname, owneruid );

			gangByUID[ganguid] = gang;

			gang.AddMember ( character );
			gang.membersRank[owneruid] = 2;

			Database.ExecPrepared ( $"INSERT INTO gang (uid, name, shortname, owneruid) VALUE ({ganguid}, @NAME@, @SHORTNAME@, {owneruid});", new Dictionary<string, string>
			{
				{ "@NAME@", name },
				{ "@SHORTNAME@", shortname }
			} );
		}

		public void Remove ( ) {
			Database.Exec ( $"DELETE FROM gang WHERE uid = {uid}" );
			Database.Exec ( $"DELETE FROM gangmember WHERE ganguid = {uid}" );

			gangByUID.Remove ( uid );

			SendAllPlayerLangMessage( "gang_removed" );

			// Remove online members //
			for ( int i = membersOnline.Count - 1; i >= 0; --i ) {
				RemoveOnlineMember ( membersOnline[i], false );
				playerMemberOfGang.Remove ( membersOnline[i].UID );
			}

			// Remove offline members
			foreach ( KeyValuePair<uint, Gang> entry in playerMemberOfGang.Where ( entry => entry.Value == this ).ToList () ) {
				playerMemberOfGang.Remove ( entry.Key );
			}
		}
	}
}