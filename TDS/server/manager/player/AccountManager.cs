using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using System.Collections.Generic;
using System.Data;
using System;

namespace Manager {

	class Account {
		private static Random rnd = new Random ();

		public static Dictionary<string, int> playerUIDs = new Dictionary<string, int> ();
		private static Dictionary<string, bool> socialClubNameBanDict = new Dictionary<string, bool> ();
		private static Dictionary<string, bool> addressBanDict = new Dictionary<string, bool> ();
		private static int lastPlayerUID = 0;

		public static void AccountOnStart ( API api ) {
			api.onClientEventTrigger += OnClientEvent;
			api.onPlayerBeginConnect += OnPlayerBeginConnect;
			api.onPlayerDisconnected += OnPlayerDisconnected;
			OnResourceStart ();
		}

		private static void SendWelcomeMessage ( Client player ) {
			player.sendChatMessage ( "~o~_______________________________________" );
			player.SendLangMessage ( "welcome_1" );
			player.SendLangMessage ( "welcome_2" );
			player.SendLangMessage ( "welcome_3" );
			player.SendLangMessage ( "welcome_4" );
			player.SendLangMessage ( "welcome_5" );
			player.sendChatMessage ( "~o~_______________________________________" );
		}

		public static void OnClientEvent ( Client player, string eventName, params dynamic[] args ) {
			switch ( eventName ) {

				case "onPlayerJoin":
					player.GetChar ().language = args[0];
					player.position = new GrandTheftMultiplayer.Shared.Math.Vector3 ( rnd.Next ( -10, 10 ), rnd.Next ( -10, 10 ), 1000 );
					player.freeze ( true );
					player.name = player.socialClubName;
					SendWelcomeMessage ( player );
					API.shared.triggerClientEvent ( player, "startRegisterLogin", player.socialClubName, playerUIDs.ContainsKey ( player.socialClubName ) );
					break;

				case "onPlayerTryRegister":
					string registerpw = Manager.Utility.ConvertToSHA512 ( args[0] );
					lastPlayerUID++;
					playerUIDs[player.socialClubName] = lastPlayerUID;
					Register.RegisterPlayer ( player, lastPlayerUID, registerpw, args[1] );
					break;

				case "onPlayerTryLogin":
					string loginpw = Manager.Utility.ConvertToSHA512 ( args[0] );
					Login.LoginPlayer ( player, playerUIDs[player.socialClubName], loginpw );
					break;

				case "onPlayerLanguageChange":
					player.GetChar ().language = args[0];
					break;

			}
		}

		public static void AddAccount ( string name, int uid ) {
			playerUIDs[name] = uid;
		}

		public void BanPlayer ( Client player ) {
			socialClubNameBanDict[player.socialClubName] = true;
			addressBanDict[player.address] = true;
		}

		private static void OnPlayerBeginConnect ( Client player, CancelEventArgs e ) {
			player.name = player.socialClubName;
			if ( socialClubNameBanDict.ContainsKey ( player.socialClubName ) || addressBanDict.ContainsKey ( player.address ) ) {
				DataTable result = Database.ExecPreparedResult ( "SELECT * FROM ban WHERE socialclubname = @SCN OR address = @address",
											new Dictionary<string, string> { { "@scn", player.socialClubName }, { "@address", player.address } } );
				if ( result.Rows.Count > 0 ) {
					DataRow row = result.Rows[0];
					if ( row["type"].ToString () == "permanent" || Convert.ToInt32 ( row["endsec"] ) > Utility.GetTimespan () ) {
						e.Cancel = true;
						return;
					} else
						Database.Exec ( "DELETE FROM ban WHERE id = " + row["id"].ToString () );
				}
				socialClubNameBanDict.Remove ( player.socialClubName );
				addressBanDict.Remove ( player.address );
			}
		}

		private static void OnResourceStart ( ) {
			DataTable result = Database.ExecResult ( "SELECT UID, name FROM player" );
			foreach ( DataRow row in result.Rows ) {
				playerUIDs[row["name"].ToString ()] = Convert.ToInt32 ( row["UID"] );
			}
			DataTable maxuidresult = Database.ExecResult ( "SELECT Max(UID) AS MaxUID FROM player" );
			lastPlayerUID = Convert.ToInt32 ( maxuidresult.Rows[0]["MaxUID"] );
		}

		public static void SavePlayerData ( Client player ) {
			Class.Character character = player.GetChar ();
			if ( character.loggedIn ) {
				Database.ExecPrepared ( "UPDATE player SET playtime = @PLAYTIME, kills = @KILLS, assists = @ASSISTS, deaths = @DEATHS, damage = @DAMAGE WHERE UID = @UID",
					new Dictionary<string, string> {
						{ "@PLAYTIME", character.playtime.ToString() },
						{ "@KILLS", character.kills.ToString() },
						{ "@ASSISTS", character.assists.ToString() },
						{ "@DEATHS", character.deaths.ToString() },
						{ "@DAMAGE", character.damage.ToString() },

						{ "@UID", character.uID.ToString() }
					}
				);
			}
		}

		private static void OnPlayerDisconnected ( Client player, string reason ) {
			SavePlayerData ( player );
			API.shared.triggerClientEventForAll ( "onClientPlayerQuit", player );
		}

	}
}
