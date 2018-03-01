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
    using TDS.server.manager.map;
    using utility;

	static class Login {

		public static async void LoginPlayer ( Client player, uint uid, string password = "" ) {
			try {
				if ( password != "" ) {
                    DataTable result = await Database.ExecResult ( $"SELECT * FROM player, playerarenastats WHERE player.uid = {uid} AND player.uid = playersettings.uid AND player.uid = playerarenastats.uid" ).ConfigureAwait ( false );
					if ( result.Rows.Count > 0 ) {
						DataRow row = result.Rows[0];
						if ( Utility.ConvertToSHA512 ( password ) == row["password"].ToString () ) {
                            player.Team = 1;
                            Character character = player.GetChar ();

                            character.UID = uid;
                            player.Name = row["name"].ToString ();
                            character.AdminLvl = Convert.ToUInt32 ( row["adminlvl"] );
                            character.DonatorLvl = Convert.ToUInt32 ( row["donatorlvl"] );
                            character.Playtime = Convert.ToUInt32 ( row["playtime"] );
                            character.ArenaStats = new LobbyDeathmatchStats {
                                Kills = Convert.ToUInt32 ( row["arenakills"] ),
                                Assists = Convert.ToUInt32 ( row["arenaassists"] ),
                                Deaths = Convert.ToUInt32 ( row["arenadeaths"] ),
                                Damage = Convert.ToUInt32 ( row["arenadamage"] ),
                                TotalKills = Convert.ToUInt32 ( row["arenatotalkills"] ),
                                TotalAssists = Convert.ToUInt32 ( row["arenatotalassists"] ),
                                TotalDeaths = Convert.ToUInt32 ( row["arenatotaldeaths"] ),
                                TotalDamage = Convert.ToUInt32 ( row["arenatotaldamage"] )
                            };
                            character.CurrentStats = character.ArenaStats;
                            character.IsVIP = row["isvip"].ToString () == "1";

                            character.LoggedIn = true;

                            player.GiveMoney ( Convert.ToUInt32 ( row["money"] ), character );

                            if ( character.AdminLvl > 0 )
                                Admin.SetOnline ( player, character.AdminLvl );

                            Map.SendPlayerHisRatings ( player );

                            NAPI.ClientEvent.TriggerClientEvent ( player, "registerLoginSuccessful" );

                            MainMenu.Join ( player );
						} else {
							player.SendLangNotification ( "wrong_password" );
							return;
						}
					} else {
						player.SendLangNotification ( "account_doesnt_exist" );
						return;
					}
				}
			} catch ( Exception ex ) {
				Log.Error ( ex.ToString() );
			}
		}
	}

}
