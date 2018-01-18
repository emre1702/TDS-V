namespace TDS.server.extend {

	using instance.lobby;

	internal static class ENAPI {
		public static void TriggerClientEventForLobby ( this GTANetworkMethods.ClientEvent api, Lobby lobby, string eventName, int teamID = -1,
														params object[] args ) {
			lobby.SendAllPlayerEvent ( eventName, teamID, args );
		}
	}

}
