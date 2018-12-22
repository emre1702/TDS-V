using GTANetworkAPI;
using System.Collections.Generic;
using TDS.server.extend;
using TDS.server.instance.player;

namespace TDS.server.manager.userpanel {

	partial class Userpanel : Script {

		private static Dictionary<string, uint> neededAdminlvls = new Dictionary<string, uint> {
			{ "removeReport", 2 },
			{ "removeSuggestion", 2 }
	};

		public Userpanel ( ) { }
		
		public static void LoadAllDatas () {
			LoadReportsData ();
			LoadSuggestionsData();
		}

		[ServerEvent(Event.PlayerDisconnected)]
		public static void OnPlayerDisconnected ( Client player, DisconnectionType type, string reason ) {
			// reports //
			Character character = player.GetChar();
			if ( playersInReportMenu.Contains (character) ) {
				ClientCloseReportsMenu (player);
				ClientCloseReport(player);
			}
		}
    }
}
