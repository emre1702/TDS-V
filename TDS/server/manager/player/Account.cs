namespace TDS.server.manager.player {

	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Threading.Tasks;
	using database;
	using extend;
	using GTANetworkAPI;
	using instance.player;
	using logs;
	using utility;

	class Account : Script {

		public static Dictionary<string, uint> PlayerUIDs = new Dictionary<string, uint> ();
		private static readonly Dictionary<string, bool> socialClubNameBanDict = new Dictionary<string, bool> ();
		private static readonly Dictionary<string, bool> addressBanDict = new Dictionary<string, bool> ();
		private static uint lastPlayerUID;

		public Account () {
			this.API.OnClientEventTrigger += this.OnClientEvent;
			this.API.OnPlayerDisconnected += this.OnPlayerDisconnected;
			this.API.OnPlayerConnect += OnPlayerBeginConnect;
			this.API.OnPlayerConnected += OnPlayerConnected;
			this.OnResourceStart ();
		}

		private static void SendWelcomeMessage ( Client player ) {
			/*player.sendChatMessage ( "~o~__________________________________________" );
			player.SendLangMessage ( "welcome_1" );
			player.SendLangMessage ( "welcome_2" );
			player.SendLangMessage ( "welcome_3" );
			player.SendLangMessage ( "welcome_4" );
			player.SendLangMessage ( "welcome_5" );
			player.sendChatMessage ( "~o~__________________________________________" );*/
			string msg = "~o~__________________________________________~w~";
			for ( int i = 1; i <= 6; i++ ) {
				msg += "~n~" + player.GetLang ( "welcome_" + i );
			}
			msg += "~n~~o~__________________________________________";
			player.SendChatMessage ( msg );
		}

		private static void OnPlayerConnected ( Client player ) {
			player.Position = new Vector3 ( 0, 0, 1000 ).Around ( 10 );
			player.Freeze ( true );
			player.Name = player.SocialClubName;
		}

		private async void OnClientEvent ( Client player, string eventName, params dynamic[] args ) {
			try {
				switch ( eventName ) {
					case "onPlayerJoin":
						this.API.TriggerClientEvent ( player, "startRegisterLogin", player.SocialClubName, PlayerUIDs.ContainsKey ( player.SocialClubName ) );
						break;

					case "onPlayerTryRegister":
						string registerpw = Utility.ConvertToSHA512 ( args[0] );
						lastPlayerUID++;
						PlayerUIDs[player.SocialClubName] = lastPlayerUID;
						await Register.RegisterPlayer ( player, lastPlayerUID, registerpw, args[1] ).ConfigureAwait ( false );
						break;

					case "onPlayerTryLogin":
						if ( PlayerUIDs.ContainsKey ( player.SocialClubName ) ) {
							string loginpw = Utility.ConvertToSHA512 ( args[0] );
							Login.LoginPlayer ( player, PlayerUIDs[player.SocialClubName], loginpw );
						} else
							player.SendLangNotification ( "account_doesnt_exist" );
						break;

					case "onPlayerLanguageChange":
						player.GetChar ().Language = args[0];
						break;

					case "onPlayerChatLoad":
						player.GetChar ().Language = args[0];
						SendWelcomeMessage ( player );
						break;
				}
			} catch ( Exception ex ) {
				Log.Error ( "Error in OnClientEvent AccountManager:" + ex.Message );
			}
		}

		public static void AddAccount ( string name, uint uid ) {
			PlayerUIDs[name] = uid;
		}

		private static async void OnPlayerBeginConnect ( Client player ) {
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
							return;
						}
						if ( Convert.ToInt32 ( row["endsec"] ) > Utility.GetTimespan () ) {
							player.Kick ( "You are banned until " + row["endoptic"] + " by " + row["admin"] + ". Reason: " + row["reason"] );
							return;
						}
						await Database.Exec ( "DELETE FROM ban WHERE id = " + row["id"] ).ConfigureAwait ( false );
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

		public static async Task SavePlayerData ( Client player ) {
			Character character = player.GetChar ();
			if ( character.LoggedIn ) {
				await Database.ExecPrepared ( "UPDATE player SET playtime = @PLAYTIME, money = @MONEY, kills = @KILLS, assists = @ASSISTS, deaths = @DEATHS, damage = @DAMAGE WHERE UID = @UID", new Dictionary<string, string> {
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
				} ).ConfigureAwait ( false );
				await Database.ExecPrepared ( "UPDATE playersetting SET hitsound = @HITSOUND WHERE UID = @UID", new Dictionary<string, string> {
					{
						"@HITSOUND", character.HitsoundOn ? "1" : "0"
					}, {
						"@UID", character.UID.ToString ()
					}
				} ).ConfigureAwait ( false );
			}
		}

		private async void OnPlayerDisconnected ( Client player, string reason ) {
			try {
				await SavePlayerData ( player ).ConfigureAwait ( false );
				uint adminlvl = player.GetChar ().AdminLvl;
				if ( adminlvl > 0 )
					Admin.SetOffline ( player, adminlvl );
				this.API.TriggerClientEventForAll ( "onClientPlayerQuit", player );
			} catch ( Exception ex ) {
				Log.Error ( "Error in OnPlayerDisconnected AccountManager:" + ex.Message );
			}
		}

		public static async Task PermaBanPlayer ( Client admin, Client target, string targetname, string targetaddress, string reason ) {
			await Database.ExecPrepared ( "REPLACE INTO ban (socialclubname, address, type, startsec, startoptic, admin, reason) VALUES (@socialclubname, @address, @type, @startsec, @startoptic, @admin, @reason)", new Dictionary<string, string> {
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
			} ).ConfigureAwait ( false );
			socialClubNameBanDict[targetname] = true;
			if ( targetaddress != "-" )
				addressBanDict[targetaddress] = true;
			Language.SendMessageToAll ( "permaban", targetname, admin.Name, reason );
			if ( target != null )
				target.Kick ( target.GetLang ( "youpermaban", admin.Name, reason ) );
			// LOG //
			Log.Admin ( "permaban", admin, PlayerUIDs[targetname].ToString (), admin.GetChar ().Lobby.Name );
			/////////
		}

		public static async Task TimeBanPlayer ( Client admin, Client target, string targetname, string targetaddress, string reason, int hours ) {
			await Database.ExecPrepared ( "REPLACE INTO ban (socialclubname, address, type, startsec, startoptic, endsec, endoptic, admin, reason) VALUES (@socialclubname, @address, @type, @startsec, @startoptic, @endsec, @endoptic, @admin, @reason)", new Dictionary<string, string> {
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
			} ).ConfigureAwait ( false );
			socialClubNameBanDict[targetname] = true;
			if ( targetaddress != "-" )
				addressBanDict[targetaddress] = true;
			Language.SendMessageToAll ( "timeban", targetname, hours.ToString (), admin.Name, reason );
			if ( target != null )
				target.Kick ( target.GetLang ( "youtimeban", hours.ToString (), admin.Name, reason ) );
			// LOG //
			Log.Admin ( "timeban", admin, PlayerUIDs[targetname].ToString (), admin.GetChar ().Lobby.Name );
			/////////
		}

		public static async Task UnBanPlayer ( Client admin, Client target, string targetname, string reason, Dictionary<string, string> queryparam ) {
			DataTable result = await Database.ExecPreparedResult ( "SELECT address FROM ban WHERE UID = {1}", queryparam ).ConfigureAwait ( false );
			string targetaddress = result.Rows[0]["address"].ToString ();
			await Database.ExecPrepared ( "DELETE FROM ban WHERE UID = {1}", queryparam ).ConfigureAwait ( false );
			socialClubNameBanDict.Remove ( targetname );
			if ( targetaddress != "-" )
				addressBanDict.Remove ( targetaddress );

			Language.SendMessageToAll ( "unban", targetname, admin.Name, reason );
			// LOG //
			Log.Admin ( "unban", admin, PlayerUIDs[targetname].ToString (), admin.GetChar ().Lobby.Name );
			/////////
		}

	}

}
