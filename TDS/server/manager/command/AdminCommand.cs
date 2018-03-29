﻿namespace TDS.server.manager.command {

	using System;
	using System.Collections.Generic;
	using System.Data;
	using database;
	using extend;
	using GTANetworkAPI;
	using instance.player;
	using lobby;
	using logs;
	using player;
	using TDS.server.enums;
	using TDS.server.instance.lobby;
	using TDS.server.instance.lobby.interfaces;
	using utility;

	class AdminCommand : Script {
		private static readonly Dictionary<string, uint> neededLevels = new Dictionary<string, uint> {
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
			{ "testskin", 2 },
			{ "testweapon", 2 },
			{ "object", 2 }
		};

		#region Lobby
		[CommandDescription ( "Ends the round." )]
		[CommandGroup ( "supporter/lobby-owner" )]
		[CommandAlias ( "endround" )]
		[Command ( "next" )]
		public static void NextMap ( Client player ) {
			Character character = player.GetChar ();
			if ( character.IsAdminLevel ( neededLevels["next"], true ) ) {
				Lobby lobby = player.GetChar ().Lobby;
				if ( lobby is IRound roundlobby ) {
					// LOG //
					AdminLog.Log ( AdminLogType.NEXT, character.UID, 0, lobby.Name );
					/////////
					roundlobby.EndRoundEarlier ( RoundEndReason.COMMAND, player.Name );
				}
			} else
				player.SendLangNotification ( "adminlvl_not_high_enough" );
		}

		[CommandDescription ( "Kicks a player from the lobby." )]
		[CommandGroup ( "supporter/lobby-owner/VIP" )]
		[Command ( "lobbykick" )]
		public static void LobbyKickPlayer ( Client player, Client target, [RemainingText] string reason ) {
			if ( player != target ) {
				Character character = player.GetChar ();
				if ( character.IsAdminLevel ( neededLevels["lobbykick"], true, true ) ) {
					// LOG //
					Character targetcharacter = target.GetChar ();
					if ( character.IsLobbyOwner ) {
						if ( character.Lobby == targetcharacter.Lobby )
							Log.LobbyOwner ( "lobbykick", player, target, ref character.Lobby.Name );
						else
							player.SendLangNotification ( "target_not_in_same_lobby" );
					} else if ( character.AdminLvl >= neededLevels["kick"] )
						AdminLog.Log ( AdminLogType.LOBBYKICK, character.UID, targetcharacter.UID, character.Lobby.Name );
					else
						Log.VIP ( "lobbykick", player, target, character.Lobby.Name );
					/////////
					ServerLanguage.SendMessageToAll ( "lobbykick", target.Name, player.Name, reason );
					targetcharacter.Lobby.RemovePlayerDerived ( targetcharacter );
					MainMenu.Join ( targetcharacter );
				}
			}
		}
		#endregion

		#region Kick
		[CommandDescription ( "Kicks a player from the server." )]
		[CommandGroup ( "supporter/VIP" )]
		[CommandAlias ( "rkick" )]
		[Command ( "kick" )]
		public static void KickPlayer ( Client player, Client target, [RemainingText] string reason ) {
			if ( player != target ) {
				Character character = player.GetChar ();
				if ( character.IsAdminLevel ( neededLevels["kick"], false, true ) ) {
					Character targetcharacter = target.GetChar ();
					if ( character.AdminLvl > targetcharacter.AdminLvl ) {
						// LOG //
						if ( character.AdminLvl >= neededLevels["kick"] )
							AdminLog.Log ( AdminLogType.KICK, character.UID, targetcharacter.UID, character.Lobby.Name + ": " + reason );
						else
							Log.VIP ( "kick", player, target, character.Lobby.Name );
						/////////
						ServerLanguage.SendMessageToAll ( "kick", target.Name, player.Name, reason );
						target.Kick ( target.GetLang ( "youkick", player.Name, reason ) );
					}
				}
			}
		}
		#endregion

		#region Ban
		[CommandDescription ( "Ban or unban a player. Use hours for types - 0 = unban, -1 = permaban, >0 = timeban." )]
		[CommandGroup ( "administrator" )]
		[CommandAlias ( "tban" )]
		[CommandAlias ( "timeban" )]
		[CommandAlias ( "pban" )]
		[CommandAlias ( "permaban" )]
		[Command ( "ban" )]
		public async void BanPlayer ( Client player, string targetname, int hours, [RemainingText] string reason ) {
			try {
				if ( Account.PlayerUIDs.ContainsKey ( targetname ) ) {
					Character character = player.GetChar ();
					if ( hours == -1 && character.IsAdminLevel ( neededLevels["ban (permanent)"] ) || hours == 0 && character.IsAdminLevel ( neededLevels["ban (unban)"] ) || hours > 0 && character.IsAdminLevel ( neededLevels["ban (time)"] ) ) {
						uint targetadminlvl = 0;
						string targetaddress = "-";
						uint targetUID = Account.PlayerUIDs[targetname];
						Client target = NAPI.Player.GetPlayerFromName ( targetname );
						Character targetcharacter;
						if ( target != null ) {
							targetcharacter = target.GetChar ();
							if ( target != null && targetcharacter.LoggedIn ) {
								targetadminlvl = targetcharacter.AdminLvl;
								targetaddress = target.Address;
							} else {
								if ( target != null )
									targetaddress = target.Address;
								DataTable targetdata = await Database.ExecResult ( $"SELECT adminlvl FROM player WHERE uid = {targetUID}" ).ConfigureAwait ( false );
								targetadminlvl = Convert.ToUInt16 ( targetdata.Rows[0]["adminlvl"] );
							}
						}
						if ( targetadminlvl <= player.GetChar ().AdminLvl ) {
							if ( hours == 0 ) {
								await Account.UnBanPlayer ( character, target, targetname, reason, targetUID ).ConfigureAwait ( false );
							} else if ( hours == -1 ) {
								Account.PermaBanPlayer ( character, target, targetname, targetaddress, reason );
							} else {
								Account.TimeBanPlayer ( character, target, targetname, targetaddress, reason, hours );
							}
						} else
							player.SendLangNotification ( "adminlvl_not_high_enough" );
					} else
						player.SendLangNotification ( "adminlvl_not_high_enough" );
				} else
					player.SendLangNotification ( "player_doesnt_exist" );
			} catch ( Exception ex ) {
				Log.Error ( ex.ToString () );
			}
		}
		#endregion

		#region Utility 
		[CommandDescription ( "Warps to another player." )]
		[CommandGroup ( "administrator/lobby-owner" )]
		[CommandAlias ( "gotoplayer" )]
		[CommandAlias ( "warpto" )]
		[Command ( "goto" )]
		public void GotoPlayer ( Client player, Client target ) {
			if ( player.IsAdminLevel ( neededLevels["goto"], true ) ) {
				if ( target.GetChar ().Lobby == player.GetChar ().Lobby ) {
					Vector3 playerpos = NAPI.Entity.GetEntityPosition ( target );
					if ( player.IsInVehicle ) {
						NAPI.Entity.SetEntityPosition ( player.Vehicle, new Vector3 ( playerpos.X + 1, playerpos.Y + 1, playerpos.Z + 1 ) );
					} else if ( target.IsInVehicle ) {
						List<Client> usersInCar = target.Vehicle.Occupants;
						if ( usersInCar.Count < NAPI.Vehicle.GetVehicleMaxOccupants ( (VehicleHash) ( target.Vehicle.Model ) ) ) {
							Dictionary<int, bool> occupiedseats = new Dictionary<int, bool> ();
							foreach ( Client occupant in usersInCar ) {
								occupiedseats[occupant.VehicleSeat] = true;
							}
							for ( int i = 0; i < NAPI.Vehicle.GetVehicleMaxOccupants ( (VehicleHash) ( target.Vehicle.Model ) ); i++ ) {
								if ( !occupiedseats.ContainsKey ( i ) ) {
									NAPI.Player.SetPlayerIntoVehicle ( player, target.Vehicle, i );
									return;
								}
							}
						}
						NAPI.Entity.SetEntityPosition ( player, new Vector3 ( playerpos.X + 1, playerpos.Y + 1, playerpos.Z + 1 ) );
					} else {
						NAPI.Entity.SetEntityPosition ( player, new Vector3 ( playerpos.X + 1, playerpos.Y, playerpos.Z ) );
					}
				} else
					player.SendLangNotification ( "target_not_in_same_lobby" );
			} else
				player.SendLangNotification ( "adminlvl_not_high_enough" );
		}

		[CommandDescription ( "Warps to a point." )]
		[CommandGroup ( "administrator/lobby-owner" )]
		[CommandAlias ( "gotoxyz" )]
		[CommandAlias ( "gotopos" )]
		[Command ( "xyz" )]
		public void GotoXYZ ( Client player, float x, float y, float z ) {
			if ( player.IsAdminLevel ( neededLevels["xyz"], true ) ) {
				NAPI.Entity.SetEntityPosition ( player, new Vector3 ( x, y, z ) );
			}
		}

		[CommandDescription ( "Creates a vehicle." )]
		[CommandGroup ( "administrator/lobby-owner" )]
		[CommandAlias ( "createvehicle" )]
		[Command ( "cveh" )]
		public void SpawnCarCommand ( Client player, string name ) {
			if ( player.IsAdminLevel ( neededLevels["cveh"], true ) ) {
				VehicleHash model = NAPI.Util.VehicleNameToModel ( name );

				Vector3 rot = player.Rotation;
				Vehicle veh = NAPI.Vehicle.CreateVehicle ( model, player.Position, rot.Z, 0, 0, numberPlate: player.Name, dimension: player.Dimension );

				NAPI.Player.SetPlayerIntoVehicle ( player, veh, -1 );
			}
		}

		[Command ( "testskin" )]
		public static void TestSkin ( Client player, PedHash hash ) {
			if ( player.IsAdminLevel ( neededLevels["testskin"] ) ) {
				player.SetSkin ( hash );
			}
		}

		[Command ( "testweapon" )]
		public static void TestWeapon ( Client player, string name ) {
			if ( player.IsAdminLevel ( neededLevels["testweapon"] ) ) {
				NAPI.Player.GivePlayerWeapon ( player, NAPI.Util.WeaponNameToModel ( name ), 1000 );
			}
		}
		#endregion

		#region Chat
		[CommandDescription ( "Global-say for admins (for announcements)." )]
		[CommandGroup ( "supporter" )]
		[CommandAlias ( "o" )]
		[CommandAlias ( "ochat" )]
		[CommandAlias ( "osay" )]
		[Command ( "adminsay" )]
		public static void AdminSay ( Client player, [RemainingText] string text ) {
			if ( player.IsAdminLevel ( neededLevels["adminsay"] ) ) {
				Chat.SendAdminMessage ( player.GetChar (), text );
			}
		}

		[CommandDescription ( "Chat only for admins." )]
		[CommandGroup ( "supporter" )]
		[CommandAlias ( "a" )]
		[CommandAlias ( "achat" )]
		[CommandAlias ( "asay" )]
		[Command ( "adminchat" )]
		public static void AdminChat ( Client player, [RemainingText] string text ) {
			if ( player.IsAdminLevel ( neededLevels["adminchat"] ) ) {
				Chat.SendAdminChat ( player.GetChar (), text );
			}
		}
		#endregion

		#region RCON
		[Command ( "rcon" )]
		public static void AddRCONRights ( Client player ) {
			if ( player.IsRcon ) {
				Character character = player.GetChar ();
				character.UID = 0;
				character.AdminLvl = 4;
				character.Login ();
				character.ArenaStats = new LobbyDeathmatchStats ();
				character.CurrentStats = character.ArenaStats;
				character.Lobby = lobby.Arena.TheLobby;
			}
		}
		#endregion

	}

}
