using GTANetworkAPI;
using System;
using System.Diagnostics;
using TDS_Server.CustomAttribute;
using TDS_Server.Default;
using TDS_Server.Instance.Dto;
using TDS_Server.Instance.Lobby;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Commands
{
    internal class PlayerCommand : Script
    {
        [TDSCommand(DPlayerCommand.LobbyLeave)]
        public static async void LobbyLeave(TDSPlayer player)
        {
            if (player.CurrentLobby == null || player.CurrentLobby.Id == 0)
                return;

            player.CurrentLobby.RemovePlayer(player);
            await LobbyManager.MainMenu.AddPlayer(player, 0);
        }

        [TDSCommand(DPlayerCommand.Suicide)]
        public static void Suicide(TDSPlayer player)
        {
            if (!(player.CurrentLobby is FightLobby fightLobby))
                return;
            if (player.Lifes == 0)
                return;
            fightLobby.KillPlayer(player.Client, player.Language.COMMITED_SUICIDE);
        }

        [TDSCommand(DPlayerCommand.GlobalChat)]
        public static void GlobalChat(TDSPlayer player, [TDSRemainingText] string message)
        {
            ChatManager.SendGlobalMessage(player, message);
        }

        [TDSCommand(DPlayerCommand.TeamChat)]
        public static void TeamChat(TDSPlayer player, [TDSRemainingText] string message)
        {
            ChatManager.SendTeamChat(player, message);
        }

        [TDSCommand(DPlayerCommand.PrivateChat)]
        public static void PrivateChat(TDSPlayer player, TDSPlayer target, [TDSRemainingText] string message)
        {
            if (player == target)
                return;
            ChatManager.SendPrivateMessage(player, target, message);
        }

        [TDSCommand(DPlayerCommand.Position)]
        public static void OutputCurrentPosition(TDSPlayer player)
        {
            if (player.Client.IsInVehicle)
            {
                Vector3 pos = player.Client.Vehicle.Position;
                NAPI.Chat.SendChatMessageToPlayer(player.Client, "Vehicle X: " + pos.X + " Y: " + pos.Y + " Z: " + pos.Z);
                Vector3 rot = player.Client.Vehicle.Rotation;
                NAPI.Chat.SendChatMessageToPlayer(player.Client, "Vehicle ROT RX: " + rot.X + " RY: " + rot.Y + " RZ: " + rot.Z);
            }
            else
            {
                Vector3 pos = player.Client.Position;
                NAPI.Chat.SendChatMessageToPlayer(player.Client, "Player X: " + pos.X + " Y: " + pos.Y + " Z: " + pos.Z);
                Vector3 rot = player.Client.Rotation;
                NAPI.Chat.SendChatMessageToPlayer(player.Client, "Player ROT RX: " + rot.X + " RY: " + rot.Y + " RZ: " + rot.Z);
            }
        }

        [TDSCommand("test1")]
        public static void TestCmd1(TDSPlayer player, TDSCommandInfos commandInfos, string a)
        {
            Console.WriteLine(a);
        }

        [TDSCommand("test2")]
        public static void TestCmd2(TDSPlayer player, string a)
        {
            Console.WriteLine(a);
        }

        [TDSCommand("test3")]
        public static void TestCmd3(TDSPlayer player, TDSCommandInfos commandInfos, DateTime? dateTime)
        {
            Console.WriteLine(dateTime.ToString());
        }

        [TDSCommand("test4", 2)]
        public static void TestCmd4(TDSPlayer player, TDSCommandInfos commandInfos, DateTime dateTime)
        {
            Console.WriteLine(dateTime.ToString());
        }

        [TDSCommand("test4", 1)]
        public static void TestCmd4(TDSPlayer player, TDSCommandInfos commandInfos, string a)
        {
            Console.WriteLine(a);
        }

        [TDSCommand("test5", 3)]
        public static void TestCmd5(TDSPlayer player, TDSCommandInfos commandInfos, Players target)
        {
            Console.WriteLine("Players: " + target.Name);
        }

        [TDSCommand("test5", 2)]
        public static void TestCmd5(TDSPlayer player, TDSCommandInfos commandInfos, TDSPlayer target)
        {
            Console.WriteLine("TDSPlayer: " + target.Client.Name);
        }

        [TDSCommand("test5", 1)]
        public static void TestCmd5(TDSPlayer player, TDSCommandInfos commandInfos, Client target)
        {
            Console.WriteLine("Client: " + target.Name);
        }

        [TDSCommand("test6", 3)]
        public static void TestCmd6(TDSPlayer player, TDSCommandInfos commandInfos, Players target)
        {
            Console.WriteLine("Players: " + target.Name);
        }

        [TDSCommand("test6", 2)]
        public static void TestCmd6(TDSPlayer player, TDSCommandInfos commandInfos, TDSPlayer target)
        {
            Console.WriteLine("TDSPlayer: " + target.Client.Name);
        }

        [TDSCommand("test6", 1)]
        public static void TestCmd6(TDSPlayer player, TDSCommandInfos commandInfos, Client target)
        {
            Console.WriteLine("Client: " + target.Name);
        }

        [TDSCommand("test6", 0)]
        public static void TestCmd6(TDSPlayer player, TDSCommandInfos commandInfos, string target)
        {
            Console.WriteLine("String: " + target);
        }

        /*#region Lobby
        [CommandAlias ( "leavelobby" )]
        [CommandAlias ( "lobbyleave" )]
        [CommandDescription ( "Leaves the lobby" )]
        [CommandGroup ( "user" )]
        [Command ( "leave" )]
        [CommandDescription ( "Commits suicide" )]
        [CommandGroup ( "user" )]
        [CommandAlias ( "suicide" )]
        [Command ( "kill" )]
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

        [CommandDescription ( "Writes in global-chat" )]
        [CommandGroup ( "user" )]
        [CommandAlias ( "globalsay" )]
        [CommandAlias ( "global" )]
        [Command ( "globalchat" )]
        [CommandDescription ( "Writes in team-chat" )]
        [CommandGroup ( "user" )]
        [CommandAlias ( "t" )]
        [CommandAlias ( "teamsay" )]
        [CommandAlias ( "team" )]
        [Command ( "teamchat" )]
        [CommandDescription ( "Writes a private-message to a player." )]
        [CommandGroup ( "user" )]
        [CommandAlias ( "message" )]
        [CommandAlias ( "pm" )]
        [Command ( "msg" )]

		#region Utility

        [CommandDescription ( "Outputs your position and rotation." )]
        [CommandGroup ( "user" )]
        [CommandAlias( "getpos" )]
        [CommandAlias ( "rot" )]
        [CommandAlias ( "getrot" )]
		[Command ( "pos", Description = "Gets your position and rotation", Group = "user" )]
        [CommandDescription ( "Checks if a map-name is already taken (needed to know for new maps)" )]
        [CommandGroup ( "user" )]
        [Command ( "checkmapname" )]
		public void CheckMapName ( Client player, string mapname ) {
			NAPI.Notification.SendNotificationToPlayer ( player, Map.DoesMapNameExist ( mapname ) ? "map-name already taken" : "map-name is available" );
		}

        #endregion Utility

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

		#endregion Deathmatch

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