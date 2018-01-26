﻿namespace TDS.server.manager.player {

	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Threading.Tasks;
	using database;
	using extend;
    using GTANetworkAPI;
    using GTANetworkInternals;
    using instance.player;
	using logs;
    using TDS.server.enums;
    using TDS.server.instance.utility;
    using utility;

	class Account : Script {

		public static Dictionary<string, uint> PlayerUIDs = new Dictionary<string, uint> ();
		private static readonly Dictionary<string, bool> socialClubNameBanDict = new Dictionary<string, bool> ();
		private static readonly Dictionary<string, bool> addressBanDict = new Dictionary<string, bool> ();
		private static uint lastPlayerUID;

		public Account () {
            Event.OnPlayerDisconnected += OnPlayerDisconnected;
            Event.OnPlayerConnect += OnPlayerBeginConnect;
            Event.OnPlayerConnected += OnPlayerConnected;
            Event.OnResourceStart += OnResourceStart;
        }

		private static void SendWelcomeMessage ( Client player ) {
            player.SendChatMessage ( "~o~__________________________________________~w~" );
			for ( int i = 1; i <= 6; i++ ) {
                player.SendChatMessage ( "~n~" + player.GetLang ( "welcome_" + i ) );
			}
            player.SendChatMessage ( "~n~~o~__________________________________________" ) ;
		}

        private static void OnPlayerConnected ( Client player, CancelEventArgs cancel ) {
            player.Position = new Vector3 ( 0, 0, 1000 ).Around ( 10 );
            player.Freeze ( true );
            player.Name = player.SocialClubName;
            NAPI.Util.ConsoleOutput ( "Already playerUID in: " + ( PlayerUIDs.ContainsKey ( player.SocialClubName ) ? 1 : 0 ) );
            NAPI.ClientEvent.TriggerClientEvent ( player, "startRegisterLogin", player.SocialClubName, PlayerUIDs.ContainsKey ( player.SocialClubName ) ? 1 : 0 );
        }

        [RemoteEvent ( "onPlayerTryRegister" )]
        private void OnPlayerTryRegisterEvent ( Client player, string password, string email ) {
            if ( PlayerUIDs.ContainsKey ( player.SocialClubName ) )
                return;
            string registerpw = Utility.ConvertToSHA512 ( password );
            lastPlayerUID++;
            PlayerUIDs[player.SocialClubName] = lastPlayerUID;
            Register.RegisterPlayer ( player, lastPlayerUID, registerpw, email );
        }

        [RemoteEvent ( "onPlayerTryLogin" )]
        private void OnPlayerTryLoginEvent ( Client player, string password ) {
            if ( PlayerUIDs.ContainsKey ( player.SocialClubName ) ) {
                string loginpw = Utility.ConvertToSHA512 ( password );
                Login.LoginPlayer ( player, PlayerUIDs[player.SocialClubName], loginpw );
            } else
                player.SendLangNotification ( "account_doesnt_exist" );
        }

        [RemoteEvent ( "onPlayerLanguageChange" )]
        private void OnPlayerLanguageChangeEvent ( Client player, string language ) {
            player.GetChar ().Language = (Language) Enum.Parse ( typeof ( Language ), language );
        }

        [RemoteEvent ( "onPlayerChatLoad" )]
        private void OnPlayerChatLoadEvent ( Client player, string language ) {
            player.GetChar ().Language = (Language) Enum.Parse ( typeof ( Language ), language );
            SendWelcomeMessage ( player );
        }

		public static void AddAccount ( string name, uint uid ) {
			PlayerUIDs[name] = uid;
		}

        [System.Diagnostics.CodeAnalysis.SuppressMessage ( "Await.Warning", "CS4014:Await.Warning" )]
        private static async void OnPlayerBeginConnect ( Client player, CancelEventArgs cancel ) {
			try {
				player.Name = player.SocialClubName;
				if ( socialClubNameBanDict.ContainsKey ( player.SocialClubName ) || addressBanDict.ContainsKey ( player.Address ) ) {
					DataTable result = await Database.ExecPreparedResult ( "SELECT * FROM ban WHERE socialclubname = @SCN OR address = @address", new Dictionary<string, string> {
						{
							"@scn", player.SocialClubName
						}, {
							"@address", player.Address
						}
					} ).ConfigureAwait ( false );
					if ( result.Rows.Count > 0 ) {
						DataRow row = result.Rows[0];
						if ( row["type"].ToString () == "permanent" ) {
							player.Kick ( "You are permanently banned by " + row["admin"] + ". Reason: " + row["reason"] );
                            cancel.Cancel = true;
							return;
						}
						if ( Convert.ToInt32 ( row["endsec"] ) > Utility.GetTimespan () ) {
							player.Kick ( "You are banned until " + row["endoptic"] + " by " + row["admin"] + ". Reason: " + row["reason"] );
                            cancel.Cancel = true;
                            return;
						}
                        Database.Exec ( "DELETE FROM ban WHERE id = " + row["id"] );
					}
					socialClubNameBanDict.Remove ( player.SocialClubName );
					addressBanDict.Remove ( player.Address );
				}
			} catch ( Exception ex ) {
				Log.Error ( "Error in OnPlayerBeginConnect AccountManager:" + ex.Message );
			}
		}

		private async void OnResourceStart () {
			try {
				DataTable result = await Database.ExecResult ( "SELECT UID, name FROM player" ).ConfigureAwait ( false );
				foreach ( DataRow row in result.Rows ) {
					PlayerUIDs[row["name"].ToString ()] = Convert.ToUInt16 ( row["UID"] );
				}
				DataTable maxuidresult = await Database.ExecResult ( "SELECT Max(UID) AS MaxUID FROM player" ).ConfigureAwait ( false );
				lastPlayerUID = Convert.ToUInt16 ( maxuidresult.Rows[0]["MaxUID"] );
			} catch ( Exception ex ) {
				Log.Error ( "Error in OnResourceStart AccountManager:" + ex.Message );
			}
		}

		public static void SavePlayerData ( Client player ) {
			Character character = player.GetChar ();
			if ( character.LoggedIn ) {
                Database.ExecPrepared ( "UPDATE player SET playtime = @PLAYTIME, money = @MONEY, kills = @KILLS, assists = @ASSISTS, deaths = @DEATHS, damage = @DAMAGE WHERE UID = @UID", new Dictionary<string, string> {
                    {
                        "@PLAYTIME", character.Playtime.ToString ()
                    }, {
                        "@MONEY", character.Money.ToString ()
                    }, {
                        "@KILLS", character.Kills.ToString ()
                    }, {
                        "@ASSISTS", character.Assists.ToString ()
                    }, {
                        "@DEATHS", character.Deaths.ToString ()
                    }, {
                        "@DAMAGE", character.Damage.ToString ()
                    }, {
                        "@UID", character.UID.ToString ()
                    }
                } );
				Database.ExecPrepared ( "UPDATE playersetting SET hitsound = @HITSOUND WHERE UID = @UID", new Dictionary<string, string> {
					{
						"@HITSOUND", character.HitsoundOn ? "1" : "0"
					}, {
						"@UID", character.UID.ToString ()
					}
				} );
			}
		}

		private void OnPlayerDisconnected ( Client player, byte type, string reason ) {
			try {
				SavePlayerData ( player );
				uint adminlvl = player.GetChar ().AdminLvl;
				if ( adminlvl > 0 )
					Admin.SetOffline ( player, adminlvl );
				//NAPI.ClientEvent.TriggerClientEventForAll ( "onClientPlayerQuit", player.Value );   //TODO NOT USED RIGHT NOW
			} catch ( Exception ex ) {
				Log.Error ( "Error in OnPlayerDisconnected AccountManager:" + ex.Message );
			}
		}

		public static void PermaBanPlayer ( Client admin, Client target, string targetname, string targetaddress, string reason ) {
			Database.ExecPrepared ( "REPLACE INTO ban (socialclubname, address, type, startsec, startoptic, admin, reason) VALUES (@socialclubname, @address, @type, @startsec, @startoptic, @admin, @reason)", new Dictionary<string, string> {
				{
					"@socialclubname", targetname
				}, {
					"@address", targetaddress
				}, {
					"@type", "permanent"
				}, {
					"@startsec", Utility.GetTimespan ().ToString ()
				}, {
					"@startoptic", Utility.GetTimestamp ()
				}, {
					"@admin", admin.Name
				}, {
					"@reason", reason
				}
			} );
			socialClubNameBanDict[targetname] = true;
			if ( targetaddress != "-" )
				addressBanDict[targetaddress] = true;
            ServerLanguage.SendMessageToAll ( "permaban", targetname, admin.Name, reason );
			if ( target != null )
				target.Kick ( target.GetLang ( "youpermaban", admin.Name, reason ) );
			// LOG //
			Log.Admin ( "permaban", admin, PlayerUIDs[targetname].ToString (), admin.GetChar ().Lobby.Name );
			/////////
		}

		public static void TimeBanPlayer ( Client admin, Client target, string targetname, string targetaddress, string reason, int hours ) {
			Database.ExecPrepared ( "REPLACE INTO ban (socialclubname, address, type, startsec, startoptic, endsec, endoptic, admin, reason) VALUES (@socialclubname, @address, @type, @startsec, @startoptic, @endsec, @endoptic, @admin, @reason)", new Dictionary<string, string> {
				{
					"@socialclubname", targetname
				}, {
					"@address", targetaddress
				}, {
					"@type", "time"
				}, {
					"@startsec", Utility.GetTimespan ().ToString ()
				}, {
					"@startoptic", Utility.GetTimestamp ()
				}, {
					"@endsec", Utility.GetTimespan ( hours * 3600 ).ToString ()
				}, {
					"@endoptic", Utility.GetTimestamp ( hours * 3600 )
				}, {
					"@admin", admin.Name
				}, {
					"@reason", reason
				}
			} );
			socialClubNameBanDict[targetname] = true;
			if ( targetaddress != "-" )
				addressBanDict[targetaddress] = true;
            ServerLanguage.SendMessageToAll ( "timeban", targetname, hours.ToString (), admin.Name, reason );
			if ( target != null )
				target.Kick ( target.GetLang ( "youtimeban", hours.ToString (), admin.Name, reason ) );
			// LOG //
			Log.Admin ( "timeban", admin, PlayerUIDs[targetname].ToString (), admin.GetChar ().Lobby.Name );
			/////////
		}

		public static async Task UnBanPlayer ( Client admin, Client target, string targetname, string reason, Dictionary<string, string> queryparam ) {
			DataTable result = await Database.ExecPreparedResult ( "SELECT address FROM ban WHERE UID = {1}", queryparam ).ConfigureAwait ( false );
			string targetaddress = result.Rows[0]["address"].ToString ();
			Database.ExecPrepared ( "DELETE FROM ban WHERE UID = {1}", queryparam );
			socialClubNameBanDict.Remove ( targetname );
			if ( targetaddress != "-" )
				addressBanDict.Remove ( targetaddress );

            ServerLanguage.SendMessageToAll ( "unban", targetname, admin.Name, reason );
			// LOG //
			Log.Admin ( "unban", admin, PlayerUIDs[targetname].ToString (), admin.GetChar ().Lobby.Name );
			/////////
		}

	}

}
