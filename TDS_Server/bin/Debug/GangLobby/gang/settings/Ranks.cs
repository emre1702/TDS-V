using System.Collections.Generic;

namespace TDS.Instance.Lobby.GangLobby.gang.settings {

	partial class Gang {

		private List<string> rankNames = new List<string> { "Member", "Leader" };

		public void ChangeRanks ( string[] ranks ) {
			if ( ranks.Length != rankNames.Count ) {
				ResetRights ( (uint) ranks.Length );
			}

			rankNames = new List<string> ( ranks );
		}

	}
}