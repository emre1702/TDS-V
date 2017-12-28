namespace TDS.server.manager.player {

	using System;
	using System.Collections.Generic;
	using System.Data;
	using database;
	using extend;
	using GTANetworkAPI;
	using instance.player;
	using lobby;
	using logs;
	using utility;

	static class Login {

		public static async void LoginPlayer ( Client player, uint uid, string password = "" ) {
			try {
                NAPI.Util.ConsoleOutput ( "onPlayerTryLogin 3" );
                uint adminlvl = 0;
				uint donatorLvl = 0;
				uint playtime = 0;
				uint money = 0;
				uint kills = 0;
				uint assists = 0;
				uint deaths = 0;
				uint damage = 0;
				bool isvip = false;
				bool hitsoundon = true;

				if ( password != "" ) {
                    NAPI.Util.ConsoleOutput ( "onPlayerTryLogin 4" );
                    DataTable result = await Database.ExecPreparedResult ( "SELECT * FROM player, playersetting WHERE player.UID = @UID AND player.UID = playersetting.UID", new Dictionary<string, string> {
						{
							"@UID", uid.ToString ()
						}
					} ).ConfigureAwait ( false );
					if ( result.Rows.Count > 0 ) {
						DataRow row = result.Rows[0];
						if ( Utility.ConvertToSHA512 ( password ) == row["password"].ToString () ) {
							player.Name = row["name"].ToString ();
							adminlvl = Convert.ToUInt16 ( row["adminlvl"] );
							donatorLvl = Convert.ToUInt16 ( row["donatorlvl"] );
							playtime = Convert.ToUInt32 ( row["playtime"] );
							money = Convert.ToUInt32 ( row["money"] );
							kills = Convert.ToUInt32 ( row["kills"] );
							assists = Convert.ToUInt32 ( row["assists"] );
							deaths = Convert.ToUInt32 ( row["deaths"] );
							damage = Convert.ToUInt32 ( row["damage"] );
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
				//player.Team = 1; // Damage canceln lassen;
				Character character = player.GetChar ();

				character.UID = uid;
				character.AdminLvl = adminlvl;
				character.DonatorLvl = donatorLvl;
				character.Playtime = playtime;
				character.Kills = kills;
				character.Assists = assists;
				character.Deaths = deaths;
				character.Damage = damage;
				character.IsVIP = isvip;
				character.HitsoundOn = hitsoundon;

				character.LoggedIn = true;

				player.GiveMoney ( money, character );
                NAPI.Util.ConsoleOutput ( "onPlayerTryLogin 5" );

                if ( adminlvl > 0 )
					Admin.SetOnline ( player, adminlvl );

				API.Shared.TriggerClientEvent ( player, "registerLoginSuccessful" );

				MainMenu.Join ( player );
			} catch ( Exception ex ) {
				Log.Error ( "Error in LoginPlayer:" + ex.Message );
			}
		}
	}

}
