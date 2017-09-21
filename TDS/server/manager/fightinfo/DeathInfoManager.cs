using System;
using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;

namespace Manager {

	static partial class FightInfo {

		public static void DeathInfoSync ( this Class.Lobby lobby, Client player, int team, Client killer, int weapon ) {
			Dictionary<string, string> killstr = new Dictionary<string, string> ();
			if ( killer != null ) {
				string weaponname = Enum.GetName ( typeof ( WeaponHash ), weapon );
				killstr["english"] = Language.GetLang ( "english", "deathinfo_killed", killer.name, player.name, weaponname );
				killstr["german"] = Language.GetLang ( "german", "deathinfo_killed", killer.name, player.name, weaponname );
			} else {
				killstr["english"] = Language.GetLang ( "english", "deathinfo_died", player.name );
				killstr["german"] = Language.GetLang ( "german", "deathinfo_died", player.name );
			}

			lobby.FuncIterateAllPlayers ( ( target, teamID, thislobby ) => {
				string language = target.GetChar ().language;
				target.triggerEvent ( "onClientPlayerDeath", player, team, killstr[language] );
			} );
		}

	}
}