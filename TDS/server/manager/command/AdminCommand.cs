namespace TDS.server.manager.command {

	using System;
	using System.Collections.Generic;
	using System.Data;
	using database;
	using extend;
	using GTANetworkAPI;
	using instance.lobby;
	using instance.player;
	using lobby;
	using logs;
	using player;
	using utility;

	class AdminCommand : Script {
		private static readonly Dictionary<string, uint> neededLevels = new Dictionary<string, uint> {
			{
				"adminsay", 1
			}, {
				"adminchat", 1
			}, {
				"next", 1
			}, {
				"lobbykick", 1
			}, {
				"kick", 1
			}, {
				"ban (time)", 2
			}, {
				"ban (unban)", 2
			}, {
				"ban (permanent)", 2
			}, {
				"goto", 2
			}, {
				"xyz", 2
			}, {
				"cveh", 2
			}, {
				"testskin", 2
			}
		};

		#region Lobby
		[Command ( "next", Alias = "endround", Description = "Ends the round.", Group = "supporter,lobby-owner" )]
		public static void NextMap ( Client player ) {
			if ( player.IsAdminLevel ( neededLevels["next"], true ) ) {
				if ( player.GetChar ().Lobby is instance.lobby.Arena lobby ) {
					// LOG //
					Log.Admin ( "next", player, "0", lobby.Name );
					/////////
					lobby.EndRoundEarlier ();
				}
			} else
				player.SendLangNotification ( "adminlvl_not_high_enough" );
		}

		[Command ( "lobbykick", GreedyArg = true, Description = "Kicks a player from the lobby.", Group = "supporter,lobby-owner,VIP" )]
		public static void LobbyKickPlayer ( Client player, Client target, string reason ) {
			if ( player != target ) {
				if ( player.IsAdminLevel ( neededLevels["lobbykick"], true, true ) ) {
					// LOG //
					if ( player.GetChar ().IsLobbyOwner )
						Log.LobbyOwner ( "lobbykick", player, target, player.GetChar ().Lobby.Name );
					else if ( player.GetChar ().AdminLvl >= neededLevels["kick"] )
						Log.Admin ( "lobbykick", player, target, player.GetChar ().Lobby.Name );
					else
						Log.VIP ( "lobbykick", player, target, player.GetChar ().Lobby.Name );
					/////////
					ServerLanguage.SendMessageToAll ( "lobbykick", target.Name, player.Name, reason );
					target.GetChar ().Lobby.RemovePlayer ( target );
				}
			}
		}
		#endregion

		#region Kick
		[Command ( "kick", GreedyArg = true, Alias = "rkick", Description = "Kicks a player from the server.", Group = "supporter,VIP" )]
		public static void KickPlayer ( Client player, Client target, string reason ) {
			if ( player != target ) {
				if ( player.IsAdminLevel ( neededLevels["kick"], false, true ) ) {
					// LOG //
					if ( player.GetChar ().AdminLvl >= neededLevels["kick"] )
						Log.Admin ( "kick", player, target, player.GetChar ().Lobby.Name );
					else
						Log.VIP ( "kick", player, target, player.GetChar ().Lobby.Name );
                    /////////
                    ServerLanguage.SendMessageToAll ( "kick", target.Name, player.Name, reason );
					target.Kick ( target.GetLang ( "youkick", player.Name, reason ) );
				}
			}
		}
		#endregion

		#region Ban
		[Command ( "ban", GreedyArg = true, Alias = "tban,timeban,pban,permaban", Description = "Ban or unban a player. Use hours for types - 0 = unban, -1 = permaban, >0 = timeban.", Group = "administrator" )]
		public async void BanPlayer ( Client player, string targetname, int hours, string reason ) {
			try {
				if ( Account.PlayerUIDs.ContainsKey ( targetname ) ) {
					if ( hours == -1 && player.IsAdminLevel ( neededLevels["ban (permanent)"] ) || hours == 0 && player.IsAdminLevel ( neededLevels["ban (unban)"] ) || hours > 0 && player.IsAdminLevel ( neededLevels["ban (time)"] ) ) {
						uint targetadminlvl;
						string targetaddress = "-";
						uint targetUID = Account.PlayerUIDs[targetname];
						Dictionary<string, string> queryparam = new Dictionary<string, string> {
							{
								"{1}", targetUID.ToString ()
							}
						};
						Client target = API.GetPlayerFromName ( targetname );
						if ( target != null && target.GetChar ().LoggedIn ) {
							Character targetcharacter = target.GetChar ();
							targetadminlvl = targetcharacter.AdminLvl;
							targetaddress = target.Address;
						} else {
							if ( target != null )
								targetaddress = target.Address;
							DataTable targetdata = await Database.ExecPreparedResult ( "SELECT adminlvl FROM player WHERE UID = {1}", queryparam ).ConfigureAwait ( false );
							targetadminlvl = Convert.ToUInt16 ( targetdata.Rows[0]["adminlvl"] );
						}
						if ( targetadminlvl <= player.GetChar ().AdminLvl ) {
							if ( hours == 0 ) {
								await Account.UnBanPlayer ( player, target, targetname, reason, queryparam ).ConfigureAwait ( false );
							} else if ( hours == -1 ) {
								await Account.PermaBanPlayer ( player, target, targetname, targetaddress, reason ).ConfigureAwait ( false );
							} else {
								await Account.TimeBanPlayer ( player, target, targetname, targetaddress, reason, hours ).ConfigureAwait ( false );
							}
						} else
							player.SendLangNotification ( "adminlvl_not_high_enough" );
					} else
						player.SendLangNotification ( "adminlvl_not_high_enough" );
				} else
					player.SendLangNotification ( "player_doesnt_exist" );
			} catch ( Exception ex ) {
				Log.Error ( "Error in BanPlayer AdminCommand:" + ex.Message );
			}
		}
		#endregion

		#region Utility 
		[Command ( "goto", Alias = "gotoplayer,warpto", Description = "Warps to another player.", Group = "Administrator,lobby-owner" )]
		public void GotoPlayer ( Client player, Client target ) {
			if ( player.IsAdminLevel ( neededLevels["goto"], true ) || player.GetChar ().Lobby == GangLobby.TheLobby ) {
				Vector3 playerpos = API.GetEntityPosition ( target );
				if ( player.IsInVehicle ) {
					API.SetEntityPosition ( player.Vehicle, new Vector3 ( playerpos.X + 1, playerpos.Y + 1, playerpos.Z + 1 ) );
				} else if ( target.IsInVehicle ) {
					List<Client> usersInCar = target.Vehicle.Occupants;
					if ( usersInCar.Count < API.GetVehicleMaxOccupants ( (VehicleHash) ( target.Vehicle.Model ) ) ) {
						Dictionary<int, bool> occupiedseats = new Dictionary<int, bool> ();
						foreach ( Client occupant in usersInCar ) {
							occupiedseats[occupant.VehicleSeat] = true;
						}
						for ( int i = 0; i < API.GetVehicleMaxOccupants ( (VehicleHash) ( target.Vehicle.Model ) ); i++ ) {
							if ( !occupiedseats.ContainsKey ( i ) ) {
								API.SetPlayerIntoVehicle ( player, target.Vehicle, i );
								return;
							}
						}
					}
					API.SetEntityPosition ( player, new Vector3 ( playerpos.X + 1, playerpos.Y + 1, playerpos.Z + 1 ) );
				} else {
					API.SetEntityPosition ( player, new Vector3 ( playerpos.X + 1, playerpos.Y, playerpos.Z ) );
				}
			} else
				player.SendLangNotification ( "adminlvl_not_high_enough" );
		}

		[Command ( "xyz", Alias = "gotoxyz,gotopos", Description = "Warps to a point.", Group = "Administrator,lobby-owner" )]
		public void GotoXYZ ( Client player, float x, float y, float z ) {
			if ( player.IsAdminLevel ( neededLevels["xyz"], true ) || player.GetChar ().Lobby == GangLobby.TheLobby ) {
				API.SetEntityPosition ( player, new Vector3 ( x, y, z ) );
			}
		}

		[Command ( "cveh", Alias = "createvehicle", Description = "Creates a vehicle.", Group = "Administrator,lobby-owner" )]
		public void SpawnCarCommand ( Client player, string name ) {
			if ( player.IsAdminLevel ( neededLevels["cveh"], true ) || player.GetChar ().Lobby == GangLobby.TheLobby ) {
				VehicleHash model = API.VehicleNameToModel ( name );

				Vector3 rot = API.GetEntityRotation ( player.Handle );
				Vehicle veh = API.CreateVehicle ( model, player.Position, rot.Z, 0, 0 );

				API.SetPlayerIntoVehicle ( player, veh, -1 );
			}
		}

		[Command ( "testskin" )]
		public static void TestSkin ( Client player, PedHash hash ) {
			if ( player.IsAdminLevel ( neededLevels["testskin"] ) ) {
				player.SetSkin ( hash );
			}
		}
		#endregion

		#region Chat
		[Command ( "adminsay", Alias = "o,ochat,osay", Description = "Global-say for admins (for announcements).", Group = "Supporter", GreedyArg = true )]
		public static void AdminSay ( Client player, string text ) {
			if ( player.IsAdminLevel ( neededLevels["adminsay"] ) ) {
				Chat.Instance.SendAdminMessage ( player, text );
			}
		}

		[Command ( "adminchat", Alias = "a,achat,asay", Description = "Chat only for admins.", Group = "Supporter", GreedyArg = true )]
		public static void AdminChat ( Client player, string text ) {
			if ( player.IsAdminLevel ( neededLevels["adminchat"] ) ) {
				Chat.Instance.SendAdminChat ( player, text );
			}
		}
		#endregion

	}

}
