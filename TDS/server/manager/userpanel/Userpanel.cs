using GTANetworkAPI;
using TDS.server.extend;
using TDS.server.instance.player;

namespace TDS.server.manager.userpanel {

    partial class Userpanel : Script {

		public Userpanel ( ) { }
		
		public static void LoadAllDatas () {
			LoadReportsData ();
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
