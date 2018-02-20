using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TDS.server.enums;
using TDS.server.extend;
using TDS.server.manager.utility;

namespace TDS.server.instance.lobby {

	partial class FightLobby {

		public void DeathInfoSync ( Client player, int team, Client killer, uint weapon ) {
			Dictionary<Language, string> killstr = new Dictionary<Language, string> ();
			if ( killer != null ) {
				string weaponname = Enum.GetName ( typeof ( WeaponHash ), weapon );
				killstr[Language.ENGLISH] = ServerLanguage.GetLang ( Language.ENGLISH, "deathinfo_killed", killer.Name, player.Name, weaponname );
				killstr[Language.GERMAN] = ServerLanguage.GetLang ( Language.GERMAN, "deathinfo_killed", killer.Name, player.Name, weaponname );
			} else {
				killstr[Language.ENGLISH] = ServerLanguage.GetLang ( Language.ENGLISH, "deathinfo_died", player.Name );
				killstr[Language.GERMAN] = ServerLanguage.GetLang ( Language.GERMAN, "deathinfo_died", player.Name );
			}

			FuncIterateAllPlayers ( ( target, teamID ) => {
                Language language = target.GetChar ( ).Language;
				target.TriggerEvent ( "onClientPlayerDeath", player.Value, team, killstr[language] );
			} );
		}

		public void PlayerAmountInFightSync ( List<uint> amountinteam ) {
			SendAllPlayerEvent ( "onClientPlayerAmountInFightSync", -1, JsonConvert.SerializeObject ( amountinteam ), false );
		}
	}

}
