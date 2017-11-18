namespace TDS.server.extend {

	using GTANetworkAPI;
	using instance.lobby;

	internal static class EAPI {
		public static void TriggerClientEventForLobby ( this API api, Lobby lobby, string eventName, int teamID = -1,
														params object[] args ) {
			lobby.SendAllPlayerEvent ( eventName, teamID, args );
		}
	}

}
