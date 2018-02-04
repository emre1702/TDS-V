namespace TDS.server.manager.player {

	using System;
	using System.Collections.Generic;
	using System.Data;
    using System.Text;
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
            StringBuilder builder = new StringBuilder ();
            builder.Append ( "#o#__________________________________________#w#" );
			for ( int i = 1; i <= 6; i++ ) {
                builder.Append ( "#n#"+player.GetLang ( "welcome_" + i ) );
			}
            builder.Append ( "#n##o#__________________________________________" );
            player.SendChatMessage ( builder.ToString () );
		}

        private static void OnPlayerConnected ( Client player, CancelEventArgs cancel ) {
            player.Position = new Vector3 ( 0, 0, 1000 ).Around ( 10 );
            player.Freeze ( true );
            player.Name = player.SocialClubName;
            NAPI.ClientEvent.TriggerClientEvent ( player, "startRegisterLogin", player.SocialClubName, PlayerUIDs.ContainsKey ( player.SocialClubName ) );
        }

        [RemoteEvent ( "onPlayerTryRegister" )]
        public void OnPlayerTryRegisterEvent ( Client player, params object[] args ) {
            if ( PlayerUIDs.ContainsKey ( player.SocialClubName ) )
                return;
            string registerpw = Utility.ConvertToSHA512 ( (string)args[0] );
            PlayerUIDs[player.SocialClubName] = ++lastPlayerUID;
            Register.RegisterPlayer ( player, lastPlayerUID, registerpw, (string)args[1] );
        }

        [RemoteEvent ( "onPlayerChatLoad" )]
        public void OnPlayerChatLoadEvent ( Client player, params object[] args ) {
            player.GetChar ().Language = (Language) Enum.Parse ( typeof ( Language ), (string) args[0] );
            SendWelcomeMessage ( player );
        }

        [RemoteEvent ( "onPlayerTryLogin" )]
        public void OnPlayerTryLoginEvent ( Client player, params object[] args ) {
            if ( PlayerUIDs.ContainsKey ( player.SocialClubName ) ) {
                string loginpw = Utility.ConvertToSHA512 ( (string) args[0] );
                Login.LoginPlayer ( player, PlayerUIDs[player.SocialClubName], loginpw );
            } else
                player.SendLangNotification ( "account_doesnt_exist" );
        }

        [RemoteEvent ( "onPlayerLanguageChange" )]
        public void OnPlayerLanguageChangeEvent ( Client player, params object[] args ) {
            player.GetChar ().Language = (Language) Enum.Parse ( typeof ( Language ), (string) args[0] );
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
				Log.Error ( ex.StackTrace );
			}
		}

		private async void OnResourceStart () {
			try {
				DataTable result = await Database.ExecResult ( "SELECT uid, name FROM player" ).ConfigureAwait ( false );
				foreach ( DataRow row in result.Rows ) {
					PlayerUIDs[row["name"].ToString ()] = Convert.ToUInt16 ( row["uid"] );
				}
				DataTable maxuidresult = await Database.ExecResult ( "SELECT Max(uid) AS Maxuid FROM player" ).ConfigureAwait ( false );
				lastPlayerUID = Convert.ToUInt16 ( maxuidresult.Rows[0]["Maxuid"] );
			} catch ( Exception ex ) {
				Log.Error ( ex.StackTrace );
                lastPlayerUID = 0;

            }
		}

		public static void SavePlayerData ( Client player ) {
			Character character = player.GetChar ();
			if ( character.LoggedIn ) {
                Database.ExecPrepared ( "UPDATE player SET playtime = @PLAYTIME, money = @MONEY WHERE uid = @UID", new Dictionary<string, string> {
                    {
                        "@PLAYTIME", character.Playtime.ToString ()
                    }, {
                        "@MONEY", character.Money.ToString ()
                    }, {
                        "@UID", character.UID.ToString ()
                    }
                } );
                Database.ExecPrepared ( "UPDATE playerarenastats SET currentkills = @CURRENTKILLS, currentassists = @CURRENTASSISTS, currentdeaths = @CURRENTDEATHS, currentdamage = @CURRENTDAMAGE, " +
                    "totalkills = @TOTALKILLS, totalassists = @TOTALASSISTS, totaldeaths = @TOTALDEATHS, totaldamage = @TOTALDAMAGE WHERE uid = @UID", new Dictionary<string, string> {
                        { "@CURRENTKILLS", character.Kills.ToString() },
                        { "@CURRENTASSISTS", character.Assists.ToString() },
                        { "@CURRENTDEATHS", character.Deaths.ToString() },
                        { "@CURRENTDAMAGE", character.Damage.ToString() },
                        { "@TOTALKILLS", character.TotalKills.ToString() },
                        { "@TOTALASSISTS", character.TotalAssists.ToString() },
                        { "@TOTALDEATHS", character.TotalDeaths.ToString() },
                        { "@TOTALDAMAGE", character.TotalDamage.ToString() },
                        { "@UID", character.UID.ToString() }
                    } );
				Database.ExecPrepared ( "UPDATE playersetting SET hitsound = @HITSOUND WHERE uid = @UID", new Dictionary<string, string> {
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
				Log.Error ( ex.StackTrace );
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
			DataTable result = await Database.ExecPreparedResult ( "SELECT address FROM ban WHERE uid = {1}", queryparam ).ConfigureAwait ( false );
			string targetaddress = result.Rows[0]["address"].ToString ();
			Database.ExecPrepared ( "DELETE FROM ban WHERE uid = {1}", queryparam );
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
