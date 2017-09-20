using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using System.Collections.Generic;
using System.Data;
using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Manager {

	class Account {
		private static Random rnd = new Random ();

		public static ConcurrentDictionary<string, int> playerUIDs = new ConcurrentDictionary<string, int> ();
		private static Dictionary<string, bool> socialClubNameBanDict = new Dictionary<string, bool> ();
		private static Dictionary<string, bool> addressBanDict = new Dictionary<string, bool> ();
		private static int lastPlayerUID = 0;

		public static void AccountOnStart ( API api ) {
			api.onClientEventTrigger += OnClientEvent;
			api.onPlayerBeginConnect += OnPlayerBeginConnect;
			api.onPlayerDisconnected += OnPlayerDisconnected;
			api.onPlayerConnected += OnPlayerConnected;
			OnResourceStart ();
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
			for ( int i = 1; i <= 5; i++ ) {
				msg += "~n~" + Language.GetLang ( player, "welcome_" + i );
			}
			msg += "~n~~o~__________________________________________";
			player.sendChatMessage ( msg );
		}

		private static void OnPlayerConnected ( Client player ) {
			player.position = new GrandTheftMultiplayer.Shared.Math.Vector3 ( rnd.Next ( -10, 10 ), rnd.Next ( -10, 10 ), 1000 );
			player.freeze ( true );
			player.name = player.socialClubName;
		}

		private static async void OnClientEvent ( Client player, string eventName, params dynamic[] args ) {
			switch ( eventName ) {

				case "onPlayerJoin":
					API.shared.triggerClientEvent ( player, "startRegisterLogin", player.socialClubName, playerUIDs.ContainsKey ( player.socialClubName ) );
					break;

				case "onPlayerTryRegister":
					string registerpw = Manager.Utility.ConvertToSHA512 ( args[0] );
					lastPlayerUID++;
					playerUIDs[player.socialClubName] = lastPlayerUID;
					await Register.RegisterPlayer ( player, lastPlayerUID, registerpw, args[1] ).ConfigureAwait ( false );
					break;

				case "onPlayerTryLogin":
					if ( playerUIDs.ContainsKey ( player.socialClubName ) ) {
						string loginpw = Manager.Utility.ConvertToSHA512 ( args[0] );
						Login.LoginPlayer ( player, playerUIDs[player.socialClubName], loginpw );
					} else
						player.SendLangNotification ( "account_doesnt_exist" );
					break;

				case "onPlayerLanguageChange":
					player.GetChar ().language = args[0];
					break;

				case "onPlayerChatLoad":
					player.GetChar ().language = args[0];
					SendWelcomeMessage ( player );
					break;

			}
		}

		public static void AddAccount ( string name, int uid ) {
			playerUIDs[name] = uid;
		}

		private static async void OnPlayerBeginConnect ( Client player, CancelEventArgs e ) {
			player.name = player.socialClubName;
			if ( socialClubNameBanDict.ContainsKey ( player.socialClubName ) || addressBanDict.ContainsKey ( player.address ) ) {
				DataTable result = await Database.ExecPreparedResult ( "SELECT * FROM ban WHERE socialclubname = @SCN OR address = @address",
											new Dictionary<string, string> { { "@scn", player.socialClubName }, { "@address", player.address } } );
				if ( result.Rows.Count > 0 ) {
					DataRow row = result.Rows[0];
					if ( row["type"].ToString () == "permanent" || Convert.ToInt32 ( row["endsec"] ) > Utility.GetTimespan () ) {
						e.Cancel = true;
						return;
					} else
						await Database.Exec ( "DELETE FROM ban WHERE id = " + row["id"].ToString () ).ConfigureAwait ( false );
				}
				socialClubNameBanDict.Remove ( player.socialClubName );
				addressBanDict.Remove ( player.address );
			}
		}

		private static async void OnResourceStart ( ) {
			DataTable result = await Database.ExecResult ( "SELECT UID, name FROM player" );
			foreach ( DataRow row in result.Rows ) {
				playerUIDs[row["name"].ToString ()] = Convert.ToInt32 ( row["UID"] );
			}
			DataTable maxuidresult = await Database.ExecResult ( "SELECT Max(UID) AS MaxUID FROM player" );
			lastPlayerUID = Convert.ToInt32 ( maxuidresult.Rows[0]["MaxUID"] );
		}

		public static async Task SavePlayerData ( Client player ) {
			Class.Character character = player.GetChar ();
			if ( character.loggedIn ) {
				await Database.ExecPrepared ( "UPDATE player SET playtime = @PLAYTIME, money = @MONEY, kills = @KILLS, assists = @ASSISTS, deaths = @DEATHS, damage = @DAMAGE WHERE UID = @UID",
					new Dictionary<string, string> {
						{ "@PLAYTIME", character.playtime.ToString() },
						{ "@MONEY", character.money.ToString() },
						{ "@KILLS", character.kills.ToString() },
						{ "@ASSISTS", character.assists.ToString() },
						{ "@DEATHS", character.deaths.ToString() },
						{ "@DAMAGE", character.damage.ToString() },

						{ "@UID", character.uID.ToString() }
					}
				).ConfigureAwait ( false );
				await Database.ExecPrepared ( "UPDATE playersetting SET hitsound = @HITSOUND WHERE UID = @UID",
					new Dictionary<string, string> {
						{ "@HITSOUND", character.hitsoundOn ? "1" : "0" },

						{ "@UID", character.uID.ToString() }
					}
				).ConfigureAwait ( false );
			}
		}

		private static async void OnPlayerDisconnected ( Client player, string reason ) {
			await SavePlayerData ( player ).ConfigureAwait ( false );
			int adminlvl = player.GetChar ().adminLvl;
			if ( adminlvl > 0 )
				Admin.SetOffline ( player, adminlvl );
			API.shared.triggerClientEventForAll ( "onClientPlayerQuit", player );
		}

		public static async Task PermaBanPlayer ( Client admin, Client target, string targetname, string targetaddress, string reason ) {
			await Database.ExecPrepared ( "REPLACE INTO ban (socialclubname, address, type, startsec, startoptic, admin, reason) VALUES (@socialclubname, @address, @type, @startsec, @startoptic, @admin, @reason)",
								new Dictionary<string, string> {
								{ "@socialclubname", targetname },
								{ "@address", targetaddress },
								{ "@type", "permanent" },
								{ "@startsec", Utility.GetTimespan().ToString() },
								{ "@startoptic", Utility.GetTimestamp() },
								{ "@admin", admin.name },
								{ "@reason", reason }
								}
			).ConfigureAwait ( false );
			socialClubNameBanDict[targetname] = true;
			if ( targetaddress != "-" )
				addressBanDict[targetaddress] = true;
			Language.SendMessageToAll ( "permaban", targetname, admin.name, reason );
			if ( target != null )
				target.kick ( target.GetLang ( "youpermaban", admin.name, reason ) );
			// LOG //
			Log.Admin ( "permaban", admin, Account.playerUIDs[targetname].ToString (), admin.GetChar ().lobby.name );
			/////////
		}

		public static async Task TimeBanPlayer ( Client admin, Client target, string targetname, string targetaddress, string reason, int hours ) {
			await Database.ExecPrepared ( "REPLACE INTO ban (socialclubname, address, type, startsec, startoptic, endsec, endoptic, admin, reason) VALUES (@socialclubname, @address, @type, @startsec, @startoptic, @endsec, @endoptic, @admin, @reason)",
								new Dictionary<string, string> {
								{ "@socialclubname", targetname },
								{ "@address", targetaddress },
								{ "@type", "time" },
								{ "@startsec", Utility.GetTimespan().ToString() },
								{ "@startoptic", Utility.GetTimestamp() },
								{ "@endsec", Utility.GetTimespan(hours*3600).ToString() },
								{ "@endoptic", Utility.GetTimestamp ( hours*3600 ) },
								{ "@admin", admin.name },
								{ "@reason", reason }
								}
			).ConfigureAwait ( false );
			socialClubNameBanDict[targetname] = true;
			if ( targetaddress != "-" )
				addressBanDict[targetaddress] = true;
			Language.SendMessageToAll ( "timeban", targetname, hours.ToString (), admin.name, reason );
			if ( target != null )
				target.kick ( target.GetLang ( "youtimeban", hours.ToString (), admin.name, reason ) );
			// LOG //
			Log.Admin ( "timeban", admin, Account.playerUIDs[targetname].ToString (), admin.GetChar ().lobby.name );
			/////////
		}

		public static async Task UnBanPlayer ( Client admin, Client target, string targetname, string targetaddress, string reason, Dictionary<string, string> queryparam ) {
			DataTable result = await Database.ExecPreparedResult ( "SELECT address FROM ban WHERE UID = {1}", queryparam );
			targetaddress = result.Rows[0]["address"].ToString ();
			await Database.ExecPrepared ( "DELETE FROM ban WHERE UID = {1}", queryparam ).ConfigureAwait ( false );
			socialClubNameBanDict.Remove ( targetname );
			if ( targetaddress != "-" )
				addressBanDict.Remove ( targetaddress );

			Language.SendMessageToAll ( "unban", targetname, admin.name, reason );
			// LOG //
			Log.Admin ( "unban", admin, Account.playerUIDs[targetname].ToString (), admin.GetChar ().lobby.name );
			/////////
		}

	}
}
