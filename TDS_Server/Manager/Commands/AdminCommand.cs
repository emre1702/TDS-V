namespace TDS_Server.Manager.Commands
{

    using GTANetworkAPI;
    using TDS_Server.CustomAttribute;
    using TDS_Server.Default;
    using TDS_Server.Enum;
    using TDS_Server.Instance.Lobby;
    using TDS_Server.Instance.Player;
    using TDS_Server.Instance.Utility;
    using TDS_Server.Manager.Logs;
    using TDS_Server.Manager.Utility;

    class AdminCommand
    {

        //#region Chat
        [TDSCommand(DAdminCommand.AdminSay)]
        public static void AdminSay(TDSPlayer player, TDSCommandInfos cmdinfos, [TDSRemainingText] string text)
        {
            ChatManager.SendAdminMessage(player, text); 
        }

        [TDSCommand(DAdminCommand.AdminChat)]
        public static void AdminChat(TDSPlayer player, TDSCommandInfos cmdinfos, [TDSRemainingText] string text)
        {
            ChatManager.SendAdminChat(player, text);
        }

        [TDSCommand(DAdminCommand.NextMap)]
        public static void NextMap(TDSPlayer player, TDSCommandInfos cmdinfos)
        {
            if (!(player.CurrentLobby is Arena arena))
                return;
            if (!cmdinfos.AsLobbyOwner)
                AdminLogsManager.Log(ELogType.Next, player, asdonator: cmdinfos.AsDonator, asvip: cmdinfos.AsVIP);
            arena.CurrentRoundEndBecauseOfPlayer = player;
            arena.SetRoundStatus(ERoundStatus.RoundEnd, ERoundEndReason.Command);
        }

        [TDSCommand(DAdminCommand.LobbyKick)]
        public static async void LobbyKick(TDSPlayer player, TDSCommandInfos cmdinfos, TDSPlayer target, [TDSRemainingText] string reason)
        {
            if (player == target)
                return;
            if (target.CurrentLobby is null)
                return;
            if (!cmdinfos.AsLobbyOwner)
            {
                AdminLogsManager.Log(ELogType.Kick_Lobby, player, target, cmdinfos.AsDonator, cmdinfos.AsVIP, reason);
                LangUtils.SendAllChatMessage(lang => Utils.GetReplaced(lang.KICK_LOBBY_INFO, target.Client.Name, player.Client.Name, reason));
            }
            else
            {
                if (player.CurrentLobby != target.CurrentLobby)
                {
                    NAPI.Chat.SendChatMessageToPlayer(player.Client, player.Language.TARGET_NOT_IN_SAME_LOBBY);
                    return;
                }
                target.CurrentLobby.SendAllPlayerLangMessage(lang => Utils.GetReplaced(lang.KICK_LOBBY_INFO, target.Client.Name, player.Client.Name, reason));
            }
            target.CurrentLobby.RemovePlayer(target);
            await LobbyManager.GetLobby(0).AddPlayer(target, 0);
        }

        [TDSCommand(DAdminCommand.LobbyBan)]
        public static void LobbyBanPlayer(TDSPlayer player, TDSCommandInfos cmdinfos, TDSPlayer target, float hours, [TDSRemainingText] string reason)
        {
            if (player.CurrentLobby.Id == 0)
                return;
            if (!player.CurrentLobby.IsOfficial && !cmdinfos.AsLobbyOwner)
                return;
            /*if (hours == 0)
            {
                character.Lobby.UnBanPlayer(character, target, targetname, targetUID, reason);
            }
            else if (hours == -1)
            {
                character.Lobby.PermaBanPlayer(character, target, targetname, targetUID, reason);
            }
            else
            {
                character.Lobby.TimeBanPlayer(character, target, targetname, targetUID, hours, reason);
            }*/
        }



        /*private static readonly Dictionary<string, uint> neededLevels = new Dictionary<string, uint> {
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
