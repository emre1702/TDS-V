using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using System.Collections.Generic;

namespace Manager {
	class Admin : Script {
		private static Dictionary<string, int> neededLevels = new Dictionary<string, int> {
			{ "next", 1 },
			{ "lobbykick", 1 },
			{ "kick", 1 },
			{ "ban (time)", 2 },
			{ "ban (unban)", 2 },
			{ "ban (permanent)", 2 }
		};

		[Command ( "next", Alias = "endround", AddToHelpmanager = true, Description = "Ends the round. Requirement: Supporter" )]
		public void NextMap ( Client player ) {
			if ( player.IsAdminLevel ( neededLevels["next"], true ) ) {
				Class.Lobby lobby = player.GetChar ().lobby;
				if ( lobby.gotRounds ) {
					// LOG //
					Log.Admin ( "next", player, null, lobby.name );
					/////////
					lobby.EndRoundEarlier ();
				}
			} else
				player.SendLangNotification ( "adminlvl_not_high_enough" );
		}

		[Command ( "kick", GreedyArg = true, Alias = "rkick", AddToHelpmanager = true, Description = "Kicks a player from the server. Requirement: Supporter or VIP" )]
		public void KickPlayer ( Client player, Client target, string reason ) {
			if ( player != target ) {
				if ( player.IsAdminLevel ( neededLevels["kick"], false, true ) ) {
					// LOG //
					if ( player.GetChar().adminLvl >= neededLevels["kick"] )
						Log.Admin ( "kick", player, target, player.GetChar ().lobby.name );
					else 
						Log.VIP ( "kick", player, target, player.GetChar ().lobby.name );
					/////////
					Language.SendMessageToAll ( "kick", target.name, player.name, reason );
					target.kick ( target.GetLang ( "youkick", player.name, reason ) );
				}
			}
		}

		[Command ( "lobbykick", GreedyArg = true, AddToHelpmanager = true, Description = "Kicks a player from the lobby. Requirement: Supporter, lobby-owner or VIP" )]
		public void LobbyKickPlayer ( Client player, Client target, string reason ) {
			if ( player != target ) {
				if ( player.IsAdminLevel ( neededLevels["lobbykick"], true, true ) ) {
					// LOG //
					if ( player.GetChar().isLobbyOwner )
						Log.LobbyOwner ( "lobbykick", player, target, player.GetChar ().lobby.name );
					else if ( player.GetChar ().adminLvl >= neededLevels["kick"] )
						Log.Admin ( "lobbykick", player, target, player.GetChar ().lobby.name );
					else 
						Log.VIP ( "lobbykick", player, target, player.GetChar ().lobby.name );
					/////////
					Language.SendMessageToAll ( "lobbykick", target.name, player.name, reason );
					target.GetChar ().lobby.RemovePlayer ( target );
				}
			}
		}

		[Command ( "ban", GreedyArg = true, Alias = "tban,timeban,pban,permaban", AddToHelpmanager = true, Description = "Ban or unban a player. Use hours for types - 0 = unban, -1 = permaban, >0 = timeban. Requirement: Administrator" )]
		public void BanPlayer ( Client player, Client target, int hours, string reason ) {
			if ( player != target ) {
				if ( hours == -1 && player.IsAdminLevel ( neededLevels["ban (permanent)"] )
				|| hours == 0 && player.IsAdminLevel ( neededLevels["ban (unban)"] )
				|| hours > 0 && player.IsAdminLevel ( neededLevels["ban (time)"] ) ) {
					if ( hours == 0 ) {
						Database.ExecPrepared ( "DELETE FROM ban WHERE socialclubname = @socialclubname", new Dictionary<string, string> { { "@socialclubname", target.socialClubName } } );
						Language.SendMessageToAll ( "unban", target.name, player.name, reason );
						// LOG //
						Log.Admin ( "unban", player, target, player.GetChar ().lobby.name );
						/////////
					} else if ( hours == -1 ) {
						Database.ExecPrepared ( "REPLACE INTO ban (socialclubname, address, type, startsec, startoptic, admin, reason) VALUES (@socialclubname, @address, @type, @startsec, @startoptic, @admin, @reason)",
							new Dictionary<string, string> {
								{ "@socialclubname", target.socialClubName },
								{ "@address", target.address },
								{ "@type", "permanent" },
								{ "@startsec", Utility.GetTimespan().ToString() },
								{ "@startoptic", Utility.GetTimestamp() },
								{ "@admin", player.name },
								{ "@reason", reason }
							}
						);
						Language.SendMessageToAll ( "permaban", target.name, player.name, reason );
						target.kick ( target.GetLang ( "youpermaban", player.name, reason ) );
						// LOG //
						Log.Admin ( "permaban", player, target, player.GetChar ().lobby.name );
						/////////
					} else {
						Database.ExecPrepared ( "REPLACE INTO ban (socialclubname, address, type, startsec, startoptic, endsec, endoptic, admin, reason) VALUES (@socialclubname, @address, @type, @startsec, @startoptic, @endsec, @endoptic, @admin, @reason)",
							new Dictionary<string, string> {
								{ "@socialclubname", target.socialClubName },
								{ "@address", target.address },
								{ "@type", "time" },
								{ "@startsec", Utility.GetTimespan().ToString() },
								{ "@startoptic", Utility.GetTimestamp() },
								{ "@endsec", Utility.GetTimespan(hours*3600).ToString() },
								{ "@endoptic", Utility.GetTimestamp ( hours*3600 ) },
								{ "@admin", player.name },
								{ "@reason", reason }
							}
						);
						Language.SendMessageToAll ( "timeban", target.name, hours.ToString (), player.name, reason );
						target.kick ( target.GetLang ( "youtimeban", hours.ToString (), player.name, reason ) );
						// LOG //
						Log.Admin ( "timeban", player, target, player.GetChar ().lobby.name );
						/////////
					}
				} else
					player.SendLangNotification ( "adminlvl_not_high_enough" );
			}
		}
	}
}
