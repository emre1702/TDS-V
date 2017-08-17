using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using System.Collections.Generic;
using GrandTheftMultiplayer.Shared.Math;
using GrandTheftMultiplayer.Shared;

namespace Manager {
	class AdminCommand : Script {
		private static Dictionary<string, int> neededLevels = new Dictionary<string, int> {
			{ "adminsay", 1 },
			{ "adminchat", 1 },
			{ "next", 1 },
			{ "lobbykick", 1 },
			{ "kick", 1 },
			{ "ban (time)", 2 },
			{ "ban (unban)", 2 },
			{ "ban (permanent)", 2 },
			{ "goto", 2 },
			{ "xyz", 2 },
			{ "cveh", 2 },
		};

		[Command ( "next", Alias = "endround", AddToHelpmanager = true, Description = "Ends the round.", Group = "supporter,lobby-owner" )]
		public void NextMap ( Client player ) {
			if ( player.IsAdminLevel ( neededLevels["next"], true ) ) {
				Class.Lobby lobby = player.GetChar ().lobby;
				if ( lobby.gotRounds ) {
					// LOG //
					Log.Admin ( "next", player, "0", lobby.name );
					/////////
					lobby.EndRoundEarlier ();
				}
			} else
				player.SendLangNotification ( "adminlvl_not_high_enough" );
		}

		[Command ( "kick", GreedyArg = true, Alias = "rkick", AddToHelpmanager = true, Description = "Kicks a player from the server.", Group = "supporter,VIP" )]
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

		[Command ( "lobbykick", GreedyArg = true, AddToHelpmanager = true, Description = "Kicks a player from the lobby.", Group = "supporter,lobby-owner,VIP" )]
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

		[Command ( "ban", GreedyArg = true, Alias = "tban,timeban,pban,permaban", AddToHelpmanager = true, Description = "Ban or unban a player. Use hours for types - 0 = unban, -1 = permaban, >0 = timeban.", Group = "administrator" )]
		public void BanPlayer ( Client player, string targetname, int hours, string reason ) {
			if ( Account.playerUIDs.ContainsKey ( targetname ) ) {
				if ( hours == -1 && player.IsAdminLevel ( neededLevels["ban (permanent)"] )
				|| hours == 0 && player.IsAdminLevel ( neededLevels["ban (unban)"] )
				|| hours > 0 && player.IsAdminLevel ( neededLevels["ban (time)"] ) ) {
					int targetadminlvl = 0;
					string targetaddress = "-";
					Client target = API.getPlayerFromName ( targetname );
					if ( target != null && target.GetChar ().loggedIn == true ) {
						Class.Character targetcharacter = target.GetChar ();
						targetadminlvl = targetcharacter.adminLvl;
						targetaddress = target.address;
					} else {
						if ( target != null )
							targetaddress = target.address;
						System.Data.DataTable targetdata = Database.ExecPreparedResult ( "SELECT adminlvl FROM player WHERE UID = {1}", new Dictionary<string, string> {
							{ "{1}", Account.playerUIDs[targetname].ToString() }
						} );
						targetadminlvl = System.Convert.ToInt32 ( targetdata.Rows[0]["adminlvl"] );
					}
					if ( targetadminlvl <= player.GetChar ().adminLvl ) {
						if ( hours == 0 ) {
							Database.ExecPrepared ( "DELETE FROM ban WHERE socialclubname = @socialclubname", new Dictionary<string, string> { { "@socialclubname", targetname } } );
							Language.SendMessageToAll ( "unban", targetname, player.name, reason );
							// LOG //
							Log.Admin ( "unban", player, Account.playerUIDs[targetname].ToString(), player.GetChar ().lobby.name );
							/////////
						} else if ( hours == -1 ) {
							Database.ExecPrepared ( "REPLACE INTO ban (socialclubname, address, type, startsec, startoptic, admin, reason) VALUES (@socialclubname, @address, @type, @startsec, @startoptic, @admin, @reason)",
								new Dictionary<string, string> {
								{ "@socialclubname", targetname },
								{ "@address", targetaddress },
								{ "@type", "permanent" },
								{ "@startsec", Utility.GetTimespan().ToString() },
								{ "@startoptic", Utility.GetTimestamp() },
								{ "@admin", player.name },
								{ "@reason", reason }
								}
							);
							Language.SendMessageToAll ( "permaban", targetname, player.name, reason );
							if ( target != null )
								target.kick ( target.GetLang ( "youpermaban", player.name, reason ) );
							// LOG //
							Log.Admin ( "permaban", player, Account.playerUIDs[targetname].ToString (), player.GetChar ().lobby.name );
							/////////
						} else {
							Database.ExecPrepared ( "REPLACE INTO ban (socialclubname, address, type, startsec, startoptic, endsec, endoptic, admin, reason) VALUES (@socialclubname, @address, @type, @startsec, @startoptic, @endsec, @endoptic, @admin, @reason)",
								new Dictionary<string, string> {
								{ "@socialclubname", targetname },
								{ "@address", targetaddress },
								{ "@type", "time" },
								{ "@startsec", Utility.GetTimespan().ToString() },
								{ "@startoptic", Utility.GetTimestamp() },
								{ "@endsec", Utility.GetTimespan(hours*3600).ToString() },
								{ "@endoptic", Utility.GetTimestamp ( hours*3600 ) },
								{ "@admin", player.name },
								{ "@reason", reason }
								}
							);
							Language.SendMessageToAll ( "timeban", targetname, hours.ToString (), player.name, reason );
							if ( target != null )
								target.kick ( target.GetLang ( "youtimeban", hours.ToString (), player.name, reason ) );
							// LOG //
							Log.Admin ( "timeban", player, Account.playerUIDs[targetname].ToString (), player.GetChar ().lobby.name );
							/////////
						}
					} else
						player.SendLangNotification ( "adminlvl_not_high_enough" );
				} else
					player.SendLangNotification ( "adminlvl_not_high_enough" );
			} else
				player.SendLangNotification ( "player_doesnt_exist" );
		}
		
		[Command ( "goto", AddToHelpmanager = true, Alias = "gotoplayer,warpto", Description = "Warps to another player.", Group = "Administrator,lobby-owner" )]
		public void GotoPlayer ( Client player, Client target ) {
			if ( player.IsAdminLevel ( neededLevels["goto"], true ) || player.GetChar ().lobby == GangLobby.lobby ) {
				Vector3 playerpos = API.shared.getEntityPosition ( target );
				if ( player.isInVehicle ) {
					API.shared.setEntityPosition ( player.vehicle, new Vector3 ( playerpos.X + 1, playerpos.Y + 1, playerpos.Z + 1 ) );
				} else if ( target.isInVehicle ) {
					Client[] usersInCar = target.vehicle.occupants;
					if ( usersInCar.Length < API.shared.getVehicleMaxOccupants ( (VehicleHash) ( target.vehicle.model ) ) ) {
						Dictionary<int, bool> occupiedseats = new Dictionary<int, bool> ();
						foreach ( Client occupant in usersInCar ) {
							occupiedseats[occupant.vehicleSeat] = true;
						}
						for ( int i = 0; i < API.shared.getVehicleMaxOccupants ( (VehicleHash) ( target.vehicle.model ) ); i++ ) {
							if ( !occupiedseats.ContainsKey ( i ) ) {
								API.shared.setPlayerIntoVehicle ( player, target.vehicle, i );
								return;
							}
						}
					}
					API.shared.setEntityPosition ( player, new Vector3 ( playerpos.X + 1, playerpos.Y + 1, playerpos.Z + 1 ) );
				} else {
					API.shared.setEntityPosition ( player, new Vector3 ( playerpos.X + 1, playerpos.Y, playerpos.Z ) );

				}
			} else
				player.SendLangNotification( "adminlvl_not_high_enough" );
		}

		[Command ( "xyz", AddToHelpmanager = true, Alias = "gotoxyz,gotopos", Description = "Warps to a point.", Group = "Administrator,lobby-owner" )]
		public void GotoXYZ ( Client player, float x, float y, float z ) {
			if ( player.IsAdminLevel ( neededLevels["xyz"], true ) || player.GetChar().lobby == GangLobby.lobby ) {
				API.shared.setEntityPosition ( player, new Vector3 ( x, y, z ) );
			}
		}

		[Command ( "cveh", AddToHelpmanager = true, Alias = "createvehicle", Description = "Creates a vehicle.", Group = "Administrator,lobby-owner" )]
		public void SpawnCarCommand ( Client player, string name ) {
			if ( player.IsAdminLevel ( neededLevels["cveh"], true ) || player.GetChar ().lobby == GangLobby.lobby ) {
				VehicleHash model = API.shared.vehicleNameToModel ( name );

				Vector3 rot = API.shared.getEntityRotation ( player.handle );
				Vehicle veh = API.shared.createVehicle ( model, player.position, new Vector3 ( 0, 0, rot.Z ), 0, 0 );

				API.shared.setPlayerIntoVehicle ( player, veh, -1 );
			}
		}

		[Command ( "adminsay", AddToHelpmanager = true, Alias = "o,ochat,osay", Description = "Global-say for admins (for announcements).", Group = "Supporter", GreedyArg = true )]
		public void AdminSay ( Client player, string text ) {
			if ( player.IsAdminLevel ( neededLevels["adminsay"] ) ) {
				Chat.SendAdminMessage ( player, text );
			}
		}

		[Command ( "adminchat", AddToHelpmanager = true, Alias = "a,achat,asay", Description = "Chat only for admins.", Group = "Supporter", GreedyArg = true )]
		public void AdminChat ( Client player, string text ) {
			if ( player.IsAdminLevel ( neededLevels["adminchat"] ) ) {
				Chat.SendAdminChat ( player, text );
			}
		}
	}
}
