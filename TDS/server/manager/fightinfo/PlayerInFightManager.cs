using System.Collections.Generic;
using GrandTheftMultiplayer.Server.Elements;

namespace Manager {

	static partial class FightInfo {

		public static void PlayerAmountInFightSync ( this Class.Lobby lobby, List<int> amountinteam ) {
			lobby.SendAllPlayerEvent ( "onClientPlayerAmountInFightSync", -1, amountinteam, false );
		}

		public static void PlayerAmountInFightSync ( this Client player, List<int> amountinteam, List<int> amountaliveinteam ) {
			player.triggerEvent ( "onClientPlayerAmountInFightSync", amountinteam, true, amountaliveinteam );
		}

	}
}