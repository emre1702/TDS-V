using System;
using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;

namespace Manager {
	static class Money {
		public static Dictionary<string, double> moneyForDict = new Dictionary<string, double> {
			{ "kill", 20 },
			{ "assist", 10 },
			{ "damage", 0.1 },
		};

		public static void GiveMoney ( this Client player, int money, Class.Character character = null ) {
			character = character ?? player.GetChar ();
			character.money += money;
			player.triggerEvent ( "onClientMoneyChange", character.money );
		}

	}
}