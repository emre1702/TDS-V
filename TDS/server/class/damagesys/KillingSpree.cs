using System;
using System.Collections.Generic;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;

namespace Class {
	partial class Damagesys {

		private static Dictionary<int, Tuple<string, int, int>> spreeReward = new Dictionary<int, Tuple<string, int, int>> {
			{ 3, new Tuple<string, int, int> ( "healtharmor", 30, 0 ) },
			{ 5, new Tuple<string, int, int> ( "healtharmor", 50, 0 ) },
			{ 10, new Tuple<string, int, int> ( "healtharmor", 100, 0 ) },
			{ 15, new Tuple<string, int, int> ( "healtharmor", 100, 0 ) }
		};
		public Dictionary<Client, int> playerSpree = new Dictionary<Client, int> ();

		private void CheckKillingSpree ( Client player ) {
			if ( spreeReward.ContainsKey ( this.playerSpree[player] ) ) {
				Tuple<string, int, int> reward = spreeReward[this.playerSpree[player]];
				string rewardtyp = reward.Item1;
				if ( rewardtyp == "healtharmor" ) {
					int bonus = reward.Item2;
					player.GetChar ().lobby.SendAllPlayerLangNotification ( "killing_spree_healtharmor", -1, player.name, this.playerSpree[player].ToString (), bonus.ToString() );
					player.AddHPArmor ( bonus );
				}
				
			}
		}

		public void AddToKillingSpree ( Client player ) {
			if ( !this.playerSpree.ContainsKey ( player ) ) {
				this.playerSpree[player] = 0;
			}
			this.playerSpree[player]++;
		}

	}
}