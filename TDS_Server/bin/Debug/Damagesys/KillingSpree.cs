#warning Todo Killingspree after next efcore.bat
/*namespace TDS.Instance.Damagesys {

	using System;
	using System.Collections.Generic;
	using GTANetworkAPI;
	using extend;
    using TDS.server.instance.player;

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
		public Dictionary<Character, uint> PlayerSpree = new Dictionary<Character, uint> ();

		private void CheckKillingSpree ( Character character ) {
			if ( sSpreeReward.ContainsKey ( PlayerSpree[character] ) ) {
				Tuple<string, uint, uint> reward = sSpreeReward[PlayerSpree[character]];
				string rewardtyp = reward.Item1;
				if ( rewardtyp == "healtharmor" ) {
					uint bonus = reward.Item2;
                    character.Lobby.SendAllPlayerLangNotification ( "killing_spree_healtharmor", -1, character.Player.Name,
																			PlayerSpree[character].ToString (), bonus.ToString () );
                    character.Player.AddHPArmor ( bonus );
				}
			}
		}

		public void AddToKillingSpree ( Character character ) {
			if ( !PlayerSpree.ContainsKey ( character ) )
				PlayerSpree[character] = 0;
			PlayerSpree[character]++;
		}
	}

}*/
