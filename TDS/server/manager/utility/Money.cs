namespace TDS.server.manager.utility {

	using System.Collections.Generic;
	using extend;
	using GTANetworkAPI;
	using instance.player;

	static class Money {

		public static Dictionary<string, double> MoneyForDict = new Dictionary<string, double> {
			{
				"kill", 20
			}, {
				"assist", 10
			}, {
				"damage", 0.1
			}
		};

		public static void GiveMoney ( this Client player, uint money, Character character = null ) {
			character = character ?? player.GetChar ();
			character.Money += money;
			NAPI.ClientEvent.TriggerClientEvent ( player, "onClientMoneyChange", character.Money );
		}

	}

}
