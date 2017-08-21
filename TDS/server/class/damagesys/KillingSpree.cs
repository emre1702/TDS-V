using System.Collections.Generic;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;

namespace Class {
	partial class Damagesys {

		private static Dictionary<int, int> spreeReward = new Dictionary<int, int> ();

		private void CheckKillingSpree ( Client player ) {
			if ( spreeReward.ContainsKey ( this.playerKills[player] ) ) {

			}
		}

	}
}