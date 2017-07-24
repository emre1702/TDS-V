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
			{ "ban (permanent)", 3 }
		};

		[Command ( "next" )]
		public void NextMap ( Client player ) {
			if ( player.IsAdminLevel ( neededLevels["next"], true ) ) {
				Class.Lobby lobby = player.GetChar ().lobby;
				if ( lobby.gotRounds ) {
					lobby.EndRoundEarlier ();
				}
			} else
				player.SendLangNotification ( "adminlvl_not_high_enough" );
		}

		[Command ( "kick", GreedyArg = true )]
		public void KickPlayer ( Client player, Client target, string reason ) {
			if ( player != target ) {
				if ( player.IsAdminLevel ( neededLevels["kick"] ) ) {
					Language.SendMessageToAll ( "kick", target.name, player.name, reason );
					target.kick ( target.GetLang ( "youkick", player.name, reason ) );
				}
			}
		}

		[Command ( "lobbykick", GreedyArg = true )]
		public void LobbyKickPlayer ( Client player, Client target, string reason ) {
			if ( player != target ) {
				if ( player.IsAdminLevel ( neededLevels["lobbykick"], true ) ) {
					Language.SendMessageToAll ( "lobbykick", target.name, player.name, reason );
					target.GetChar ().lobby.RemovePlayer ( target );
				}
			}
		}

		[Command ( "ban", GreedyArg = true )]
		public void BanPlayer ( Client player, Client target, int hours, string reason ) {
			if ( player != target ) {
				if ( hours == -1 && player.IsAdminLevel ( neededLevels["ban (permanent)"] )
				|| hours == 0 && player.IsAdminLevel ( neededLevels["ban (unban)"] )
				|| hours > 0 && player.IsAdminLevel ( neededLevels["ban (time)"] ) ) {
					if ( hours == 0 ) {
						Database.ExecPrepared ( "DELETE FROM ban WHERE socialclubname = @socialclubname", new Dictionary<string, string> { { "@socialclubname", target.socialClubName } } );
						Language.SendMessageToAll ( "unban", target.name, player.name, reason );
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
					}
				} else
					player.SendLangNotification ( "adminlvl_not_high_enough" );
			}
		}
	}
}
