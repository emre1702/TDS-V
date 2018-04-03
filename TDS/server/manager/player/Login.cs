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
    using TDS.server.instance.lobby.ganglobby;
    using TDS.server.manager.map;
    using utility;

	static class Login {

		public static async void LoginPlayer ( Character character, uint uid, string password = null ) {
			try {
                Client player = character.Player;

                if ( password != null ) {
                    DataTable result = await Database.ExecResult ( $"SELECT * FROM player, playerarenastats WHERE player.uid = {uid} AND player.uid = playerarenastats.uid" ).ConfigureAwait ( false );
                    if ( result.Rows.Count > 0 ) {
						DataRow row = result.Rows[0];
						if ( Utility.ConvertToSHA512 ( password ) == row["password"].ToString () ) {

                            player.Name = row["name"].ToString ();
                            character.AdminLvl = Convert.ToUInt32 ( row["adminlvl"] );
                            character.DonatorLvl = Convert.ToUInt32 ( row["donatorlvl"] );
                            character.Playtime = Convert.ToUInt32 ( row["playtime"] );
							character.MuteTime = Convert.ToInt32(row["mutetime"]);
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

                            character.GiveMoney ( Convert.ToInt16 ( row["money"] ) );

                            if ( character.AdminLvl > 0 )
                                Admin.SetOnline ( character );

                            Map.SendPlayerHisRatings ( character );
                            Gang.CheckPlayerGang ( character );

							Log.Login ( player );

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
                character.UID = uid;
                character.Login();

                NAPI.ClientEvent.TriggerClientEvent ( player, "registerLoginSuccessful", character.AdminLvl );

                MainMenu.Join ( character );
            } catch ( Exception ex ) {
				Log.Error ( ex.ToString() );
			}
		}
	}

}
