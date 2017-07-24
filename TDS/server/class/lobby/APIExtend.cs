using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;

static partial class APIExtend {

	#pragma warning disable IDE1006 // Benennungsstile
	public static void triggerClientEventForLobby ( this API api, Class.Lobby lobby, string eventName, int teamID = -1, params object[] args ) {
		lobby.SendAllPlayerEvent ( eventName, teamID, args );
	}

	#pragma warning restore IDE1006 // Benennungsstile
}