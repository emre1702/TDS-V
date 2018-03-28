using GTANetworkAPI;

namespace TDS.server.manager.userpanel {

    partial class Userpanel : Script {

		public Userpanel ( ) { }
		
		public static void LoadAllDatas () {
			LoadReportsData ();
		}

		[ServerEvent(Event.PlayerDisconnected)]
		public static void OnPlayerDisconnected ( Client player, DisconnectionType type, string reason ) {
			// reports //
			if ( playersInReportMenu.Contains ( player ) ) {
				PlayerCloseReportsMenu ( player );
				PlayerCloseReport ( player );
			}
		}
    }
}
