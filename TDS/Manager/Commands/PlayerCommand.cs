using GTANetworkAPI;

namespace TDS.Manager.Commands {

	class PlayerCommand : Script {

        /*#region Lobby
        [CommandAlias ( "leavelobby" )]
        [CommandAlias ( "lobbyleave" )]
        [CommandDescription ( "Leaves the lobby" )]
        [CommandGroup ( "user" )]
        [Command ( "leave" )]
		public static void Leave ( Client player ) {
            Character character = player.GetChar ();
			if ( character.Lobby != MainMenu.TheLobby ) {
                character.Lobby.RemovePlayerDerived ( character );
                MainMenu.Join ( character );
			}
		}

        [CommandDescription ( "Commits suicide" )]
        [CommandGroup ( "user" )]
        [CommandAlias ( "suicide" )]
        [Command ( "kill" )]
		public static void Kill ( Client player ) {
			Character character = player.GetChar ();
			if ( character.Lobby is FightLobby lobby ) {
				if ( character.Lifes > 0 ) {
					lobby.KillPlayer ( player, "commited_suicide" );
				}
			}
		}

        [CommandDescription ( "Join the gangwar lobby (only for open-world for map-creation). Use it in mainmenu." )]
        [CommandGroup ( "user" )]
        [CommandAlias ( "gwlobby" )]
        [CommandAlias ( "lobbygang" )]
        [CommandAlias ( "lobbygw" )]
        [Command ( "ganglobby" )]
		public static void JoinGangLobby ( Client player ) {
            Character character = player.GetChar();
            if ( character.Lobby == MainMenu.TheLobby ) {
				lobby.GangLobby.Join ( character );
			}
		}
        #endregion

        #region Chat
        [CommandDescription ( "Writes in global-chat" )]
        [CommandGroup ( "user" )]
        [CommandAlias ( "globalsay" )]
        [CommandAlias ( "global" )]
        [Command ( "globalchat" )]
		public static void GlobalChat ( Client player, [RemainingText] string text ) {
            Character character = player.GetChar ();
            if ( player.GetChar().LoggedIn )
			    Chat.SendGlobalMessage ( character, text );
		}

        [CommandDescription ( "Writes in team-chat" )]
        [CommandGroup ( "user" )]
        [CommandAlias ( "t" )]
        [CommandAlias ( "teamsay" )]
        [CommandAlias ( "team" )]
        [Command ( "teamchat" )]
		public static void TeamChat ( Client player, [RemainingText] string text ) {
            Character character = player.GetChar ();
            if ( character.LoggedIn )
                Chat.SendTeamChat ( character, text );
		}

        [CommandDescription ( "Writes a private-message to a player." )]
        [CommandGroup ( "user" )]
        [CommandAlias ( "message" )]
        [CommandAlias ( "pm" )]
        [Command ( "msg" )]
        public static void PrivateMessage ( Client player, Client target, [RemainingText] string text ) {
            Character character = player.GetChar ();
            if ( character.LoggedIn && player != target ) {
                Character targetcharacter = target.GetChar ();
                if ( target.GetChar ().LoggedIn )
                    Chat.SendPrivateMessage ( character, targetcharacter, text );
                else
                    player.SendLangNotification ( "target_not_logged_in" );
            }
        }
		#endregion

		#region Utility
        [CommandDescription ( "Outputs your position and rotation." )]
        [CommandGroup ( "user" )]
        [CommandAlias( "getpos" )]
        [CommandAlias ( "rot" )]
        [CommandAlias ( "getrot" )]
		[Command ( "pos", Description = "Gets your position and rotation", Group = "user" )]
		public void SendPlayerPosition ( Client sender ) {
			if ( NAPI.Player.IsPlayerInAnyVehicle ( sender ) ) {
				Vehicle veh = NAPI.Player.GetPlayerVehicle ( sender );
				Vector3 pos = NAPI.Entity.GetEntityPosition ( veh );
                NAPI.Chat.SendChatMessageToPlayer ( sender, "Vehicle X: " + pos.X + " Y: " + pos.Y + " Z: " + pos.Z );
				Vector3 rot = NAPI.Entity.GetEntityRotation ( veh );
                NAPI.Chat.SendChatMessageToPlayer ( sender, "Vehicle ROT RX: " + rot.X + " RY: " + rot.Y + " RZ: " + rot.Z );
			} else {
				Vector3 pos = NAPI.Entity.GetEntityPosition ( sender );
				NAPI.Chat.SendChatMessageToPlayer ( sender, "Player X: " + pos.X + " Y: " + pos.Y + " Z: " + pos.Z );
				Vector3 rot = NAPI.Entity.GetEntityRotation ( sender );
				NAPI.Chat.SendChatMessageToPlayer ( sender, "Player ROT RX: " + rot.X + " RY: " + rot.Y + " RZ: " + rot.Z );
			}
		}

        [CommandDescription ( "Checks if a map-name is already taken (needed to know for new maps)" )]
        [CommandGroup ( "user" )]
        [Command ( "checkmapname" )]
		public void CheckMapName ( Client player, string mapname ) {
			NAPI.Notification.SendNotificationToPlayer ( player, Map.DoesMapNameExist ( mapname ) ? "map-name already taken" : "map-name is available" );
		}
        #endregion

        #region Deathmatch
        [CommandDescription ( "Activates/deactivates the hitsound" )]
        [CommandGroup ( "user" )]
        [CommandAlias ( "hitglocke" )]
        [CommandAlias ( "togglehitsound" )]
        [Command ( "hitsound" )]
		public static void ToggleHitsound ( Client player, int activate = -1 ) {
			Character character = player.GetChar ();
			if ( activate == 1 || activate != 0 && !character.HitsoundOn ) {
				character.HitsoundOn = true;
				player.SendLangNotification ( "activated_hitsound" );
                NAPI.ClientEvent.TriggerClientEvent ( player, "onPlayerSettingChange", PlayerSetting.HITSOUND, true );
			} else {
				character.HitsoundOn = false;
				player.SendLangNotification ( "deactivated_hitsound" );
                NAPI.ClientEvent.TriggerClientEvent ( player, "onPlayerSettingChange", PlayerSetting.HITSOUND, false );
            }
		}
		#endregion

        [RemoteEvent ( "testit" )]
        public static void TestitEvent ( Client player, params object[] args ) {
            int number = Convert.ToInt32 ( args[0] );
            NetHandle handle = (NetHandle) args[1];
            switch ( number ) {
                case 0:
                    NAPI.Util.ConsoleOutput ( handle.GetType ().ToString() );
                    break;
            }
        }

        private static Vehicle lastveh;

        [Command ( "testit")]
        public static void Testit ( Client player, int number ) {
            switch ( number ) {
                case 1:
                    player.Vehicle.CustomPrimaryColor = new Color ( 60, 0, 0 );
                    player.Vehicle.CustomSecondaryColor = new Color ( 255, 0, 0 );

                    NAPI.Util.ConsoleOutput ( "\nTest 1 start:" );

                    Color color3 = player.Vehicle.CustomPrimaryColor;
                    NAPI.Util.ConsoleOutput ( color3.Red + " - " + color3.Green + " - " + color3.Blue );
                    color3 = player.Vehicle.CustomSecondaryColor;
                    NAPI.Util.ConsoleOutput ( color3.Red + " - " + color3.Green + " - " + color3.Blue );

                    color3 = NAPI.Vehicle.GetVehicleCustomPrimaryColor ( player.Vehicle );
                    NAPI.Util.ConsoleOutput ( color3.Red + " - " + color3.Green + " - " + color3.Blue );
                    color3 = NAPI.Vehicle.GetVehicleCustomSecondaryColor ( player.Vehicle );
                    NAPI.Util.ConsoleOutput ( color3.Red + " - " + color3.Green + " - " + color3.Blue );

                    break;

                case 2:

                    NAPI.Util.ConsoleOutput ( "\nTest 2 start:" );

                    Color color2 = player.Vehicle.CustomPrimaryColor;
                    NAPI.Util.ConsoleOutput ( color2.Red + " - " + color2.Green + " - " + color2.Blue );
                    color2 = player.Vehicle.CustomSecondaryColor;
                    NAPI.Util.ConsoleOutput ( color2.Red + " - " + color2.Green + " - " + color2.Blue );

                    color2 = player.Vehicle.CustomPrimaryColor;
                    NAPI.Util.ConsoleOutput ( color2.Red + " - " + color2.Green + " - " + color2.Blue );
                    color2 = player.Vehicle.CustomSecondaryColor;
                    NAPI.Util.ConsoleOutput ( color2.Red + " - " + color2.Green + " - " + color2.Blue );

                    color2 = NAPI.Vehicle.GetVehicleCustomPrimaryColor ( player.Vehicle );
                    NAPI.Util.ConsoleOutput ( color2.Red + " - " + color2.Green + " - " + color2.Blue );
                    color2 = NAPI.Vehicle.GetVehicleCustomSecondaryColor ( player.Vehicle );
                    NAPI.Util.ConsoleOutput ( color2.Red + " - " + color2.Green + " - " + color2.Blue );
                    break;

                case 3:
                    NAPI.Resource.StopResource ( "TDS-V" );
                    break;

                case 4:
                    lastveh = NAPI.Vehicle.CreateVehicle ( VehicleHash.T20, player.Position, player.Rotation.Z, 154, 0, "Hi", dimension: player.Dimension );
                    break;

                case 5:
                    NAPI.Entity.DeleteEntity ( lastveh );
                    lastveh = NAPI.Vehicle.CreateVehicle ( VehicleHash.T20, player.Position, player.Rotation.Z, 154, 0, "Hi", dimension: player.Dimension );
                    break;

                case 6:
                    NAPI.Util.ConsoleOutput ( lastveh.Health.ToString() + " - " + lastveh.Livery.ToString() );
                    break;

                case 7:
                    NAPI.Util.ConsoleOutput ( player.Vehicle.MaxOccupants.ToString() );
                    NAPI.Util.ConsoleOutput ( player.Vehicle.Occupants.Count.ToString () );
                    break;

                case 8:
                    player.TriggerEvent ( "testit", new bool[] { true, false, true } );
                    break;
                case 9:
                    player.TriggerEvent ( "testit", new string[] { "hi", "my" } );
                    break;
                case 10:
                    player.TriggerEvent ( "testit", new int[] { 0, 1, 2 } );
                    break;
                case 11:
                    player.TriggerEvent ( "testit", new uint[] { 0, 1, 2 } );
                    break;
                case 12:
                    player.TriggerEvent ( "testit", new Vector3[] { player.Position, new Vector3 ( 0, 0, 0 ) } );
                    break;
                case 13:
                    player.TriggerEvent ( "testit", new System.Collections.Generic.List<Vector3> { player.Position, new Vector3 ( 0, 0, 0 ) } );
                    break;
                case 14:
                    player.TriggerEvent ( "testit", new double[] { 0.1, 1.2, 2.3 } );
                    break;
                case 15:
                    player.TriggerEvent ( "testit", new float[] { 0.1f, 1.2f, 2.3f } );
                    break;

                case 16:
                    NAPI.Chat.SendChatMessageToPlayer ( player, "Hallo !{#FF0000}Bonus!" );
                    NAPI.Chat.SendChatMessageToPlayer ( player, "Hallo !{#FF0000}Bonus!", false );

                    NAPI.Chat.SendChatMessageToPlayer ( player, "Hallo "+"!{255 0 0 255}Bonus!" );
                    NAPI.Chat.SendChatMessageToPlayer ( player, "Hallo !{255 0 0 255}Bonus!", false );

                    string b = "Bonus";
                    NAPI.Chat.SendChatMessageToPlayer ( player, $"Hallo !{{#FF0000}}{b}!" );
                    NAPI.Chat.SendChatMessageToPlayer ( player, $"Hallo !{{#FF0000}}{b}!", false );

                    NAPI.Chat.SendChatMessageToPlayer ( player, $"Hallo !{{255 0 0}}{b}!" );
                    NAPI.Chat.SendChatMessageToPlayer ( player, $"Hallo !{{255 0 0}}{b}!", false );
                    break;
                    
            }
        }*/
    }

}
