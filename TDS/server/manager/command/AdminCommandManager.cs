using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using System.Collections.Generic;
using GrandTheftMultiplayer.Shared.Math;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Server.Constant;
using System.Data;
using System;
using System.Threading.Tasks;

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
			{ "testskin", 2 }
		};

		[Command ( "next", Alias = "endround", AddToHelpmanager = true, Description = "Ends the round.", Group = "supporter,lobby-owner" )]
		public static void NextMap ( Client player ) {
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
		public static void KickPlayer ( Client player, Client target, string reason ) {
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
		public static void LobbyKickPlayer ( Client player, Client target, string reason ) {
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
		public static async void BanPlayer ( Client player, string targetname, int hours, string reason ) {
			if ( Account.playerUIDs.ContainsKey ( targetname ) ) {
				if ( hours == -1 && player.IsAdminLevel ( neededLevels["ban (permanent)"] )
				|| hours == 0 && player.IsAdminLevel ( neededLevels["ban (unban)"] )
				|| hours > 0 && player.IsAdminLevel ( neededLevels["ban (time)"] ) ) {
					int targetadminlvl = 0;
					string targetaddress = "-";
					int targetUID = Account.playerUIDs[targetname];
					Dictionary<string, string> queryparam = new Dictionary<string, string> { { "{1}", targetUID.ToString () } };
					Client target = API.shared.getPlayerFromName ( targetname );
					if ( target != null && target.GetChar ().loggedIn == true ) {
						Class.Character targetcharacter = target.GetChar ();
						targetadminlvl = targetcharacter.adminLvl;
						targetaddress = target.address;
					} else {
						if ( target != null )
							targetaddress = target.address;
						DataTable targetdata = await Database.ExecPreparedResult ( "SELECT adminlvl FROM player WHERE UID = {1}", queryparam );
						targetadminlvl = Convert.ToInt32 ( targetdata.Rows[0]["adminlvl"] );
					}
					if ( targetadminlvl <= player.GetChar ().adminLvl ) {
						if ( hours == 0 ) {
							await Account.UnBanPlayer ( player, target, targetname, targetaddress, reason, queryparam );
						} else if ( hours == -1 ) {
							await Account.PermaBanPlayer ( player, target, targetname, targetaddress, reason );
						} else {
							await Account.TimeBanPlayer ( player, target, targetname, targetaddress, reason, hours );
						}
					} else
						player.SendLangNotification ( "adminlvl_not_high_enough" );
				} else
					player.SendLangNotification ( "adminlvl_not_high_enough" );
			} else
				player.SendLangNotification ( "player_doesnt_exist" );
		}
		
		[Command ( "goto", AddToHelpmanager = true, Alias = "gotoplayer,warpto", Description = "Warps to another player.", Group = "Administrator,lobby-owner" )]
		public static void GotoPlayer ( Client player, Client target ) {
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
		public static void GotoXYZ ( Client player, float x, float y, float z ) {
			if ( player.IsAdminLevel ( neededLevels["xyz"], true ) || player.GetChar().lobby == GangLobby.lobby ) {
				API.shared.setEntityPosition ( player, new Vector3 ( x, y, z ) );
			}
		}

		[Command ( "cveh", AddToHelpmanager = true, Alias = "createvehicle", Description = "Creates a vehicle.", Group = "Administrator,lobby-owner" )]
		public static void SpawnCarCommand ( Client player, string name ) {
			if ( player.IsAdminLevel ( neededLevels["cveh"], true ) || player.GetChar ().lobby == GangLobby.lobby ) {
				VehicleHash model = API.shared.vehicleNameToModel ( name );

				Vector3 rot = API.shared.getEntityRotation ( player.handle );
				Vehicle veh = API.shared.createVehicle ( model, player.position, new Vector3 ( 0, 0, rot.Z ), 0, 0 );

				API.shared.setPlayerIntoVehicle ( player, veh, -1 );
			}
		}

		[Command ( "adminsay", AddToHelpmanager = true, Alias = "o,ochat,osay", Description = "Global-say for admins (for announcements).", Group = "Supporter", GreedyArg = true )]
		public static void AdminSay ( Client player, string text ) {
			if ( player.IsAdminLevel ( neededLevels["adminsay"] ) ) {
				Chat.SendAdminMessage ( player, text );
			}
		}

		[Command ( "adminchat", AddToHelpmanager = true, Alias = "a,achat,asay", Description = "Chat only for admins.", Group = "Supporter", GreedyArg = true )]
		public static void AdminChat ( Client player, string text ) {
			if ( player.IsAdminLevel ( neededLevels["adminchat"] ) ) {
				Chat.SendAdminChat ( player, text );
			}
		}

		[Command ("testskin")]
		public static void TestSkin ( Client player, PedHash hash ) {
			if ( player.IsAdminLevel ( neededLevels["testskin"] ) ) {
				player.setSkin ( hash );
			}
		}
	}
}
