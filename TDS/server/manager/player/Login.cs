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
                uint adminlvl = 0;
				uint donatorLvl = 0;
				uint playtime = 0;
				uint money = 0;
				uint kills = 0;
				uint assists = 0;
				uint deaths = 0;
				uint damage = 0;
                uint totalkills = 0;
                uint totalassists = 0;
                uint totaldeaths = 0;
                uint totaldamage = 0;
				bool isvip = false;
				bool hitsoundon = true;

				if ( password != "" ) {
                    DataTable result = await Database.ExecPreparedResult ( "SELECT * FROM player, playerarenastats, playersetting WHERE player.uid = @uid AND player.uid = playersetting.uid AND player.uid = playerarenastats.uid", new Dictionary<string, string> {
						{
                            "@uid", uid.ToString ()
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
							kills = Convert.ToUInt32 ( row["currentkills"] );
							assists = Convert.ToUInt32 ( row["currentassists"] );
							deaths = Convert.ToUInt32 ( row["currentdeaths"] );
							damage = Convert.ToUInt32 ( row["currentdamage"] );
                            totalkills = Convert.ToUInt32 ( row["totalkills"] );
                            totalassists = Convert.ToUInt32 ( row["totalassists"] );
                            totaldeaths = Convert.ToUInt32 ( row["totaldeaths"] );
                            totaldamage = Convert.ToUInt32 ( row["totaldamage"] );
                            isvip = row["isvip"].ToString () == "1";
							hitsoundon = row["hitsound"].ToString () == "1";
						} else {
							player.SendLangNotification ( "wrong_password" );
							return;
						}
					} else {
						player.SendLangNotification ( "account_doesnt_exist" );
						return;
					}
				}
				player.Team = 1;
				Character character = player.GetChar ();

				character.UID = uid;
				character.AdminLvl = (ushort) adminlvl;
				character.DonatorLvl = (ushort) donatorLvl;
				character.Playtime = playtime;
				character.Kills = kills;
				character.Assists = assists;
				character.Deaths = deaths;
				character.Damage = damage;
                character.TotalKills = totalkills;
                character.TotalAssists = totalassists;
                character.TotalDeaths = totaldeaths;
                character.TotalDamage = totaldamage;
                character.IsVIP = isvip;
				character.HitsoundOn = hitsoundon;

				character.LoggedIn = true;

				player.GiveMoney ( money, character );

                if ( adminlvl > 0 )
					Admin.SetOnline ( player, adminlvl );

				API.Shared.TriggerClientEvent ( player, "registerLoginSuccessful" );

				MainMenu.Join ( player );
			} catch ( Exception ex ) {
				Log.Error ( ex.StackTrace );
			}
		}
	}

}
