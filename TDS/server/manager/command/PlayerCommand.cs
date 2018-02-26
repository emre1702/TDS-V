namespace TDS.server.manager.command {

	using extend;
	using GTANetworkAPI;
	using instance.lobby;
	using instance.player;
	using lobby;
	using map;
    using System;
    using utility;

	class PlayerCommand : Script {

		#region Lobby
		[Command ( "leave", Alias = "leavelobby,lobbyleave", Description = "Leaves the lobby", Group = "user" )]
		public static void Leave ( Client player ) {
			Lobby lobby = player.GetChar ().Lobby;
			if ( lobby != MainMenu.TheLobby ) {
                lobby.RemovePlayerDerived ( player );
                MainMenu.Join ( player );
			}
		}

		[Command ( "kill", Alias = "suicide", Description = "Commits suicide", Group = "user" )]
		public static void Kill ( Client player ) {
			Character character = player.GetChar ();
			if ( character.Lobby is FightLobby lobby ) {
				if ( character.Lifes > 0 ) {
					lobby.KillPlayer ( player, "commited_suicide" );
				}
			}
		}

		[Command ( "ganglobby", Alias = "gwlobby,lobbygang,lobbygw", Description = "Join the gangwar lobby (only for open-world for map-creation). Use it in mainmenu.", Group = "user" )]
		public static void JoinGangLobby ( Client player ) {
			if ( player.GetChar ().Lobby == MainMenu.TheLobby ) {
				lobby.GangLobby.Join ( player );
			}
		}
		#endregion

		#region Chat
		[Command ( "globalchat", Alias = "globalsay,global", Description = "Writes in global-chat", GreedyArg = true, Group = "user" )]
		public static void GlobalChat ( Client player, string text ) {
            if ( player.GetChar().LoggedIn )
			    Chat.SendGlobalMessage ( player, text );
		}

		[Command ( "teamchat", Alias = "t,teamsay,team", Description = "Writes in team-chat", GreedyArg = true, Group = "user" )]
		public static void TeamChat ( Client player, string text ) {
            if ( player.GetChar ().LoggedIn )
                Chat.SendTeamChat ( player, text );
		}

        [Command ( "msg", Alias = "message,pm", Description = "Writes a private-message to a player", GreedyArg = true, Group = "user" )]
        public static void PrivateMessage ( Client player, Client target, string text ) {
            if ( player.GetChar ().LoggedIn && player != target ) {
                if ( target.GetChar ().LoggedIn )
                    Chat.SendPrivateMessage ( player, target, text );
                else
                    player.SendLangNotification ( "target_not_logged_in" );
            }
        }
		#endregion

		#region Utility
		[Command ( "pos", Alias = "getpos,rot,getrot", Description = "Gets your position and rotation", Group = "user" )]
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

		[Command ( "checkmapname", Description = "Checks if a map-name is already taken (needed to know for new maps)", Group = "user" )]
		public void CheckMapName ( Client player, string mapname ) {
			NAPI.Notification.SendNotificationToPlayer ( player, Map.DoesMapNameExist ( mapname ) ? "map-name already taken" : "map-name is available" );
		}
		#endregion

		#region Deathmatch
		[Command ( "hitsound", Alias = "hitglocke,togglehitsound", Description = "Activates/deactivates the hitsound", Group = "user" )]
		public static void ToggleHitsound ( Client player, int activate = -1 ) {
			Character character = player.GetChar ();
			if ( activate == 1 || activate != 0 && !character.HitsoundOn ) {
				character.HitsoundOn = true;
				player.SendLangNotification ( "activated_hitsound" );
			} else {
				character.HitsoundOn = false;
				player.SendLangNotification ( "deactivated_hitsound" );
			}
		}
		#endregion

        [RemoteEvent ( "testit")]
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
        }
    }

}
