namespace TDS.server.instance.lobby {

	using System;
	using System.Collections.Generic;
	using extend;
	using GTANetworkAPI;
	using manager.utility;

	partial class Lobby {

		public void DeathInfoSync ( Client player, uint team, Client killer, int weapon ) {
			Dictionary<string, string> killstr = new Dictionary<string, string> ();
			if ( killer != null ) {
				string weaponname = Enum.GetName ( typeof ( WeaponHash ), weapon );
				killstr["english"] = Language.GetLang ( "english", "deathinfo_killed", killer.Name, player.Name, weaponname );
				killstr["german"] = Language.GetLang ( "german", "deathinfo_killed", killer.Name, player.Name, weaponname );
			} else {
				killstr["english"] = Language.GetLang ( "english", "deathinfo_died", player.Name );
				killstr["german"] = Language.GetLang ( "german", "deathinfo_died", player.Name );
			}

			this.FuncIterateAllPlayers ( ( target, teamID ) => {
				string language = target.GetChar ( ).Language;
				target.TriggerEvent ( "onClientPlayerDeath", player, team, killstr[language] );
			} );
		}

		private void PlayerAmountInFightSync ( List<uint> amountinteam ) {
			this.SendAllPlayerEvent ( "onClientPlayerAmountInFightSync", -1, amountinteam, false );
		}
	}

}
