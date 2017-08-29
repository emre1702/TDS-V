using GrandTheftMultiplayer.Server.Elements;

namespace Manager {

	static partial class FightInfo {

		public static void DeathInfoSync ( this Class.Lobby lobby, Client player, int team, Client killer, int weapon ) {
			lobby.SendAllPlayerEvent ( "onClientPlayerDeath", -1, player, team, killer, weapon );
		}

	}
}