using System;
using System.Collections.Generic;
using GrandTheftMultiplayer.Server.Elements;

namespace Manager {
	static class Money {
		public static Dictionary<string, double> moneyForDict = new Dictionary<string, double> {
			{ "kill", 20 },
			{ "assist", 10 },
			{ "damage", 0.1 },
		};

		public static void GivePoints ( this Client player, double points, Class.Character character = null ) {
			character = character ?? player.GetChar ();
			character.money += (int) points;
		}

	}
}