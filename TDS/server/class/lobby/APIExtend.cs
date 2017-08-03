using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;

static partial class APIExtend {

	public static void TriggerClientEventForLobby ( this ServerAPI api, Class.Lobby lobby, string eventName, int teamID = -1, params object[] args ) {
		lobby.SendAllPlayerEvent ( eventName, teamID, args );
	}

}