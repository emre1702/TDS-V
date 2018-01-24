namespace TDS.server.instance.damagesys {

	using System;
	using System.Collections.Generic;
	using GTANetworkAPI;
	using extend;

	partial class Damagesys {

		private static readonly Dictionary<uint, Tuple<string, uint, uint>> sSpreeReward =
			new Dictionary<uint, Tuple<string, uint, uint>> {
				{
					3, new Tuple<string, uint, uint> ( "healtharmor", 30, 0 )
				}, {
					5, new Tuple<string, uint, uint> ( "healtharmor", 50, 0 )
				}, {
					10, new Tuple<string, uint, uint> ( "healtharmor", 100, 0 )
				}, {
					15, new Tuple<string, uint, uint> ( "healtharmor", 100, 0 )
				}
			};
		public Dictionary<Client, uint> PlayerSpree = new Dictionary<Client, uint> ();

		private void CheckKillingSpree ( Client player ) {
			if ( sSpreeReward.ContainsKey ( PlayerSpree[player] ) ) {
				Tuple<string, uint, uint> reward = sSpreeReward[PlayerSpree[player]];
				string rewardtyp = reward.Item1;
				if ( rewardtyp == "healtharmor" ) {
					uint bonus = reward.Item2;
					player.GetChar ().Lobby.SendAllPlayerLangNotification ( "killing_spree_healtharmor", -1, player.Name,
																			PlayerSpree[player].ToString (), bonus.ToString () );
					player.AddHPArmor ( bonus );
				}
			}
		}

		public void AddToKillingSpree ( Client player ) {
			if ( !PlayerSpree.ContainsKey ( player ) )
				PlayerSpree[player] = 0;
			PlayerSpree[player]++;
		}
	}

}
