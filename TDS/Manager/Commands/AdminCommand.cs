namespace TDS.Manager.Commands
{

    using GTANetworkAPI;
    using TDS.CustomAttribute;
    using TDS.Instance.Player;
    using TDS.Instance.Utility;
    using TDS.Manager.Logs;
    using TDS.Manager.Utility;

    class AdminCommand : Script
    {

        //#region Chat
        [TDSCommand("adminsay")]
        public static void AdminSay(TDSPlayer character, TDSCommandInfos cmdinfos, [TDSRemainingText] string text)
        {
            ChatManager.SendAdminMessage(character, text); 
        }

        [TDSCommand("adminchat")]
        public static void AdminChat(TDSPlayer character, TDSCommandInfos cmdinfos, [TDSRemainingText] string text)
        {
            ChatManager.SendAdminChat(character, text);
        }

        /*[TDSCommand("next")]
         public static void NextMap(Character character, TDSCommandInfos cmdinfos)
         {
             //if (character.CurrentLobby is IRound roundlobby)  // Todo: Activate after implementing
             //{
                 // LOG //
                 Admin.Log(Enum.ELogType.Next, character, "Lobby: " + roundlobby.Id,   (AdminLogType.NEXT, character.UID, 0, lobby.Name);
                 if (!character.CurrentLobby.IsPlayerLobbyOwner(character)) 
                 /////////
                 currentRoundEndBecauseOfPlayer = character;
                 roundlobby.EndRoundEarlier(RoundEndReason.COMMAND, player.Name);

             //}
         }*/

        /*private static readonly Dictionary<string, uint> neededLevels = new Dictionary<string, uint> {
			{ "next", 1 },
			{ "lobbykick", 1 },
			{ "kick", 1 },
			{ "ban (time)", 1 },
			{ "ban (unban)", 2 },
			{ "ban (permanent)", 3 },
			{ "lobbyban (time)", 1 },
			{ "lobbyban (unban)", 2 },
			{ "lobbyban (permanent)", 2 },
			{ "mute (time)", 1 },
			{ "mute (unmute)", 1 },
			{ "mute (permaunmute)", 2 },
			{ "mute (permanent)", 2 },
			{ "goto", 2 },
			{ "xyz", 2 },
			{ "cveh", 2 },
			{ "testskin", 2 },
			{ "testweapon", 2 },
			{ "object", 2 }
		};

		#region Lobby

		[CommandDescription( "Kicks a player from the lobby." )]
		[CommandGroup( "supporter/lobby-owner/VIP" )]
		[Command( "lobbykick" )]
		public static void LobbyKickPlayer ( Client player, Client target, [RemainingText] string reason ) {
			if ( player != target ) {
				Character character = player.GetChar();
				if ( character.IsAdminLevel( neededLevels["lobbykick"], true, true ) ) {
					// LOG //
					Character targetcharacter = target.GetChar();
					if ( character.IsLobbyOwner ) {
						if ( character.Lobby == targetcharacter.Lobby )
							Log.LobbyOwner( "lobbykick", player, target, ref character.Lobby.Name );
						else
							player.SendLangNotification( "target_not_in_same_lobby" );
					} else if ( character.AdminLvl >= neededLevels["kick"] )
						AdminLog.Log( AdminLogType.LOBBYKICK, character.UID, targetcharacter.UID, character.Lobby.Name );
					else
						Log.VIP( "lobbykick", player, target, character.Lobby.Name );
					/////////
					ServerLanguage.SendMessageToAll( "lobbykick", target.Name, player.Name, reason );
					targetcharacter.Lobby.RemovePlayerDerived( targetcharacter );
					MainMenu.Join( targetcharacter );
				}
			}
		}

		[CommandDescription( "Bans or unbans a player from the lobby. Can be used for breaking a lobby-rule or if a lobby-owner doesn't want a player in his lobby." )]
		[CommandGroup( "supporter/lobby-owner" )]
		[Command( "lobbyban" )]
		public static void LobbyBanPlayer ( Client player, string targetname, int hours, [RemainingText] string reason ) {
			try {
				if ( Account.PlayerUIDs.ContainsKey( targetname ) ) {
					Character character = player.GetChar();
					if ( hours == -1 && character.IsAdminLevel( neededLevels["lobbyban (permanent)"], true ) || hours == 0 && character.IsAdminLevel( neededLevels["lobbyban (unban)"], true ) || hours > 0 && character.IsAdminLevel( neededLevels["lobbyban (time)"], true ) ) {
						if ( reason.Length > 3 ) {
							if ( character.Lobby.BanAllowed ) {
								uint targetUID = Account.PlayerUIDs[targetname];
								Client target = NAPI.Player.GetPlayerFromName( targetname );
								if ( hours == 0 ) {
									character.Lobby.UnBanPlayer( character, target, targetname, targetUID, reason );
								} else if ( hours == -1 ) {
									character.Lobby.PermaBanPlayer( character, target, targetname, targetUID, reason );
								} else {
									character.Lobby.TimeBanPlayer( character, target, targetname, targetUID, hours, reason );
								}
							} else
								character.SendLangNotification( "not_possible_in_this_lobby" );
						} else
							character.SendLangNotification( "reason_missing" );
					} else
						character.SendLangNotification( "adminlvl_not_high_enough" );
				} else
					player.SendLangNotification( "player_doesnt_exist" );
			} catch ( Exception ex ) {
				Log.Error( ex.ToString() );
			}
		}
		#endregion

		#region Kick
		[CommandDescription( "Kicks a player from the server." )]
		[CommandGroup( "supporter/VIP" )]
		[CommandAlias( "rkick" )]
		[Command( "kick" )]
		public static void KickPlayer ( Client player, Client target, [RemainingText] string reason ) {
			if ( player != target ) {
				Character character = player.GetChar();
				if ( character.IsAdminLevel( neededLevels["kick"], false, true ) ) {
					Character targetcharacter = target.GetChar();
					if ( character.AdminLvl > targetcharacter.AdminLvl ) {
						// LOG //
						if ( character.AdminLvl >= neededLevels["kick"] )
							AdminLog.Log( AdminLogType.KICK, character.UID, targetcharacter.UID, character.Lobby.Name + ": " + reason );
						else
							Log.VIP( "kick", player, target, character.Lobby.Name );
						/////////
						ServerLanguage.SendMessageToAll( "kick", target.Name, player.Name, reason );
						target.Kick( target.GetLang( "youkick", player.Name, reason ) );
					}
				}
			}
		}
		#endregion

		#region Ban
		[CommandDescription( "Ban or unban a player. Use hours for types - 0 = unban, -1 = permaban, >0 = timeban." )]
		[CommandGroup( "administrator" )]
		[CommandAlias( "tban" )]
		[CommandAlias( "timeban" )]
		[CommandAlias( "pban" )]
		[CommandAlias( "permaban" )]
		[Command( "ban" )]
		public async void BanPlayer ( Client player, string targetname, int hours, [RemainingText] string reason ) {
			try {
				if ( Account.PlayerUIDs.ContainsKey( targetname ) ) {
					Character character = player.GetChar();
					if ( hours == -1 && character.IsAdminLevel( neededLevels["ban (permanent)"] ) || hours == 0 && character.IsAdminLevel( neededLevels["ban (unban)"] ) || hours > 0 && character.IsAdminLevel( neededLevels["ban (time)"] ) ) {
						if ( reason.Length > 3 ) {
							uint targetadminlvl = 0;
							string targetaddress = "-";
							uint targetUID = Account.PlayerUIDs[targetname];
							Client target = NAPI.Player.GetPlayerFromName( targetname );
							if ( target != null && target.GetChar().LoggedIn ) {
								Character targetcharacter = target.GetChar();
								targetadminlvl = targetcharacter.AdminLvl;
								targetaddress = target.Address;
							} else {
								DataTable targetdata = await Database.ExecResult( $"SELECT adminlvl FROM player WHERE uid = {targetUID}" ).ConfigureAwait( false );
								targetadminlvl = Convert.ToUInt16( targetdata.Rows[0]["adminlvl"] );
								if ( target != null )
									targetaddress = target.Address;
							}
							if ( targetadminlvl <= character.AdminLvl ) {
								if ( hours == 0 ) {
#pragma warning disable CS4014 // Da dieser Aufruf nicht abgewartet wird, wird die Ausführung der aktuellen Methode fortgesetzt, bevor der Aufruf abgeschlossen ist
									Account.UnBanPlayer( character, target, targetname, reason, targetUID );
#pragma warning restore CS4014 // Da dieser Aufruf nicht abgewartet wird, wird die Ausführung der aktuellen Methode fortgesetzt, bevor der Aufruf abgeschlossen ist
								} else if ( hours == -1 ) {
									Account.PermaBanPlayer( character, target, targetname, targetaddress, reason, targetUID);
								} else {
									Account.TimeBanPlayer( character, target, targetname, targetaddress, reason, hours, targetUID);
								}
							} else
								character.SendLangNotification( "adminlvl_not_high_enough" );
						} else
							character.SendLangNotification( "reason_missing" );
					} else
						character.SendLangNotification( "adminlvl_not_high_enough" );
				} else
					player.SendLangNotification( "player_doesnt_exist" );
			} catch ( Exception ex ) {
				Log.Error( ex.ToString() );
			}
		}
		#endregion

		#region mute
		[CommandDescription( "Mute or unmute a player. Use minutes for types - 0 = unmute, -1 = permamute, >0 = timemute." )]
		[CommandAlias( "pmute" )]
		[CommandAlias( "permamute" )]
		[CommandAlias( "tmute" )]
		[CommandAlias( "timemute" )]
		[Command( "mute" )]
		public async void MutePlayer ( Client player, string targetname, int minutes, [RemainingText] string reason ) {
			try {
				if ( Account.PlayerUIDs.ContainsKey( targetname ) ) {
					Character character = player.GetChar();
					if ( minutes == -1 && character.IsAdminLevel( neededLevels["mute (permanent)"] ) || minutes == 0 && character.IsAdminLevel( neededLevels["mute (unmute)"] ) || minutes > 0 && character.IsAdminLevel( neededLevels["mute (time)"] ) ) {
						uint targetadminlvl = 0;
						int mutetime = 0;
						uint targetUID = Account.PlayerUIDs[targetname];
						Client target = NAPI.Player.GetPlayerFromName( targetname );
						if ( target != null && target.GetChar().LoggedIn ) {
							Character targetcharacter = target.GetChar();
							targetadminlvl = targetcharacter.AdminLvl;
							mutetime = targetcharacter.MuteTime;
						} else {
							DataTable targetdata = await Database.ExecResult( $"SELECT adminlvl, mutetime FROM player WHERE uid = {targetUID}" ).ConfigureAwait( false );
							targetadminlvl = Convert.ToUInt16( targetdata.Rows[0]["adminlvl"] );
							mutetime = Convert.ToInt32( targetdata.Rows[0]["mutetime"] );
						}
						if ( targetadminlvl <= character.AdminLvl ) {
							if ( minutes == 0 ) {
								switch ( mutetime ) {
									case 0:
										character.SendLangNotification( "player_not_muted" );
										break;
									case -1:
										if ( character.IsAdminLevel( neededLevels["mute (permaunmute)"] ) )
											Account.ChangePlayerMuteTime( character, target, targetUID, minutes, reason );
										else
											character.SendLangNotification( "adminlvl_not_high_enough" );
										break;
									default:
										Account.ChangePlayerMuteTime( character, target, targetUID, minutes, reason );
										break;
								}
							} else if ( minutes == -1 ) {
								if ( mutetime != -1 )
									Account.ChangePlayerMuteTime( character, target, targetUID, minutes, reason );
								else
									character.SendLangNotification( "player_already_muted" );
							} else {
								switch ( mutetime ) {
									case -1:
										character.SendLangNotification( "player_already_muted" );
										break;
									case 0:
										Account.ChangePlayerMuteTime( character, target, targetUID, minutes, reason );
										break;
									default:
										character.SendLangNotification( "player_already_muted" );
										break;
								}

							}
						} else
							character.SendLangNotification( "adminlvl_not_high_enough" );
					} else
						character.SendLangNotification( "adminlvl_not_high_enough" );
				} else
					player.SendLangNotification( "player_doesnt_exist" );
			} catch ( Exception ex ) {
				Log.Error( ex.ToString() );
			}
		}
		#endregion

		#region Utility 
		[CommandDescription( "Warps to another player." )]
		[CommandGroup( "administrator/lobby-owner" )]
		[CommandAlias( "gotoplayer" )]
		[CommandAlias( "warpto" )]
		[Command( "goto" )]
		public void GotoPlayer ( Client player, Client target ) {
			if ( player.IsAdminLevel( neededLevels["goto"], true ) ) {
				if ( target.GetChar().Lobby == player.GetChar().Lobby ) {
					Vector3 playerpos = NAPI.Entity.GetEntityPosition( target );
					if ( player.IsInVehicle ) {
						NAPI.Entity.SetEntityPosition( player.Vehicle, new Vector3( playerpos.X + 1, playerpos.Y + 1, playerpos.Z + 1 ) );
					} else if ( target.IsInVehicle ) {
						List<Client> usersInCar = target.Vehicle.Occupants;
						if ( usersInCar.Count < NAPI.Vehicle.GetVehicleMaxOccupants( (VehicleHash)(target.Vehicle.Model) ) ) {
							Dictionary<int, bool> occupiedseats = new Dictionary<int, bool>();
							foreach ( Client occupant in usersInCar ) {
								occupiedseats[occupant.VehicleSeat] = true;
							}
							for ( int i = 0; i < NAPI.Vehicle.GetVehicleMaxOccupants( (VehicleHash)(target.Vehicle.Model) ); i++ ) {
								if ( !occupiedseats.ContainsKey( i ) ) {
									NAPI.Player.SetPlayerIntoVehicle( player, target.Vehicle, i );
									return;
								}
							}
						}
						NAPI.Entity.SetEntityPosition( player, new Vector3( playerpos.X + 1, playerpos.Y + 1, playerpos.Z + 1 ) );
					} else {
						NAPI.Entity.SetEntityPosition( player, new Vector3( playerpos.X + 1, playerpos.Y, playerpos.Z ) );
					}
				} else
					player.SendLangNotification( "target_not_in_same_lobby" );
			} else
				player.SendLangNotification( "adminlvl_not_high_enough" );
		}

		[CommandDescription( "Warps to a point." )]
		[CommandGroup( "administrator/lobby-owner" )]
		[CommandAlias( "gotoxyz" )]
		[CommandAlias( "gotopos" )]
		[Command( "xyz" )]
		public void GotoXYZ ( Client player, float x, float y, float z ) {
			if ( player.IsAdminLevel( neededLevels["xyz"], true ) ) {
				NAPI.Entity.SetEntityPosition( player, new Vector3( x, y, z ) );
			}
		}

		[CommandDescription( "Creates a vehicle." )]
		[CommandGroup( "administrator/lobby-owner" )]
		[CommandAlias( "createvehicle" )]
		[Command( "cveh" )]
		public void SpawnCarCommand ( Client player, string name ) {
			if ( player.IsAdminLevel( neededLevels["cveh"], true ) ) {
				VehicleHash model = NAPI.Util.VehicleNameToModel( name );

				Vector3 rot = player.Rotation;
				Vehicle veh = NAPI.Vehicle.CreateVehicle( model, player.Position, rot.Z, 0, 0, numberPlate: player.Name, dimension: player.Dimension );

				NAPI.Player.SetPlayerIntoVehicle( player, veh, -1 );
			}
		}

		[Command( "testskin" )]
		public static void TestSkin ( Client player, PedHash hash ) {
			if ( player.IsAdminLevel( neededLevels["testskin"] ) ) {
				player.SetSkin( hash );
			}
		}

		[Command( "testweapon" )]
		public static void TestWeapon ( Client player, string name ) {
			if ( player.IsAdminLevel( neededLevels["testweapon"] ) ) {
				NAPI.Player.GivePlayerWeapon( player, NAPI.Util.WeaponNameToModel( name ), 1000 );
			}
		}
		#endregion


		
		#endregion

		#region RCON
		[Command( "rcon" )]
		public static void AddRCONRights ( Client player ) {
			if ( player.IsRcon ) {
				Character character = player.GetChar();
				character.UID = 0;
				character.AdminLvl = 4;
				character.Login();
				character.ArenaStats = new LobbyDeathmatchStats();
				character.CurrentStats = character.ArenaStats;
				character.Lobby = lobby.Arena.TheLobby;
			}
		}
		#endregion*/

    }

}
