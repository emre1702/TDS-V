using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using System.Data;
using System.Collections.Generic;
using System.Security.Cryptography;
using System;
using GrandTheftMultiplayer.Shared.Math;
using GrandTheftMultiplayer.Server.Constant;

namespace Manager {
	static class Login {

		public static void LoginPlayer ( Client player, int uid, string password = "" ) {
			int adminlvl = 0;
			int donatorLvl = 0;
			int playtime = 0;
			int kills = 0;
			int assists = 0;
			int deaths = 0;
			int damage = 0;
			bool isvip = false;
			bool hitsoundon = true;
			
			if ( password != "" ) {
				DataTable result = Database.ExecPreparedResult ( "SELECT * FROM player, playersetting WHERE player.UID = @UID AND player.UID = playersetting.UID", new Dictionary<string, string> { { "@UID", uid.ToString () } } );
				if ( result.Rows.Count > 0 ) {
					DataRow row = result.Rows[0];
					if ( Utility.ConvertToSHA512 ( password ) == row["password"].ToString () ) {
						player.name = row["name"].ToString();
						adminlvl = Convert.ToInt32 ( row["adminlvl"] );
						donatorLvl = Convert.ToInt32 ( row["donatorlvl"] );
						playtime = Convert.ToInt32 ( row["playtime"] );
						kills = Convert.ToInt32 ( row["kills"] );
						assists = Convert.ToInt32 ( row["assists"] );
						deaths = Convert.ToInt32 ( row["deaths"] );
						damage = Convert.ToInt32 ( row["damage"] );
						isvip = row["isvip"].ToString () == "1";
						hitsoundon = row["hitsound"].ToString () == "1";
					} else {
						player.SendLangMessage ( "wrong_password" );
						return;
					}
				} else {
					player.SendLangMessage ( "account_doesnt_exist" );
					return;
				}

			}
			player.team = 1;  // Damage canceln lassen;
			Class.Character character = player.GetChar ();

			character.uID = uid;
			character.adminLvl = adminlvl;
			character.donatorLvl = donatorLvl;
			character.playtime = playtime;
			character.kills = kills;
			character.assists = assists;
			character.deaths = deaths;
			character.damage = damage;
			character.isVIP = isvip;
			character.hitsoundOn = hitsoundon;

			character.loggedIn = true;

			API.shared.triggerClientEvent ( player, "registerLoginSuccessful" );

			MainMenu.Join ( player );
		}
	}
}
