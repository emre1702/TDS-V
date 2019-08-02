using GTANetworkAPI;
using TDS_Common.Default;
using TDS_Common.Enum;
using TDS_Server.CustomAttribute;
using TDS_Server.Default;
using TDS_Server.Instance.Lobby;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Sync;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server.Manager.Commands
{
    internal class PlayerCommand : Script
    {
        [TDSCommand(DPlayerCommand.LobbyLeave)]
        public static async void LobbyLeave(TDSPlayer player)
        {
            if (player.CurrentLobby == null)
                return;
            if (player.CurrentLobby.LobbyEntity.Type == ELobbyType.MainMenu)
            {
                if (CustomLobbyMenuSync.IsPlayerInCustomLobbyMenu(player))
                {
                    CustomLobbyMenuSync.RemovePlayer(player);
                    NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.LeaveCustomLobbyMenu);
                }
                return;
            }

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

        [TDSCommand(DPlayerCommand.OpenPrivateChat)]
        public static void OpenPrivateChat(TDSPlayer player, TDSPlayer target)
        {
            // Am I blocked?
            if (player.BlockingPlayerIds.Contains(target.Entity?.Id ?? 0))
            {
                NAPI.Chat.SendChatMessageToPlayer(player.Client, string.Format(player.Language.YOU_GOT_BLOCKED_BY, target.Client.Name));
                return;
            }

            // Am I already in chat?
            if (player.InPrivateChatWith != null)
            {
                player.Client.SendNotification(player.Language.ALREADY_IN_PRIVATE_CHAT_WITH.Formatted(player.InPrivateChatWith.Client.Name));
                return;
            }

            // Did I already send a request?
            if (player.SentPrivateChatRequestTo == target)
                return;

            // Withdraw my old request
            if (player.SentPrivateChatRequestTo != null)
            {
                TDSPlayer oldTargett = player.SentPrivateChatRequestTo;
                oldTargett.Client.SendNotification(oldTargett.Language.PRIVATE_CHAT_REQUEST_CLOSED_REQUESTER.Formatted(player.Client.Name));
                player.SentPrivateChatRequestTo = null;
            }

            // Is Target already in a private chat?
            if (target.InPrivateChatWith != null)
            {
                player.Client.SendNotification(player.Language.TARGET_ALREADY_IN_PRIVATE_CHAT);
                return;
            }

            // Send request
            if (target.SentPrivateChatRequestTo != player)
            {
                player.Client.SendNotification(player.Language.PRIVATE_CHAT_REQUEST_SENT_TO.Formatted(target.Client.Name));
                target.Client.SendNotification(target.Language.PRIVATE_CHAT_REQUEST_RECEIVED_FROM.Formatted(player.Client.Name));
                player.SentPrivateChatRequestTo = target;
            }
            // Accept request
            else
            {
                player.Client.SendNotification(player.Language.PRIVATE_CHAT_OPENED_WITH.Formatted(target.Client.Name));
                target.Client.SendNotification(target.Language.PRIVATE_CHAT_OPENED_WITH.Formatted(player.Client.Name));
                target.SentPrivateChatRequestTo = null;
                player.InPrivateChatWith = target;
                target.InPrivateChatWith = player;
            }
        }

        [TDSCommand(DPlayerCommand.ClosePrivateChat)]
        public static void ClosePrivateChat(TDSPlayer player)
        {
            if (player.InPrivateChatWith == null && player.SentPrivateChatRequestTo == null)
            {
                player.Client.SendChatMessage(player.Language.NOT_IN_PRIVATE_CHAT);
                return;
            }
            player.ClosePrivateChat(false);
        }

        [TDSCommand(DPlayerCommand.PrivateChat)]
        public static void PrivateChat(TDSPlayer player, [TDSRemainingText] string message)
        {
            if (player.InPrivateChatWith == null)
            {
                player.Client.SendChatMessage(player.Language.NOT_IN_PRIVATE_CHAT);
                return;
            }
            string colorStr = "!{155|35|133}";
            player.InPrivateChatWith.Client.SendChatMessage($"{colorStr}[{player.Client.Name}: {message}]");
        }

        [TDSCommand(DPlayerCommand.PrivateMessage)]
        public static void PrivateMessage(TDSPlayer player, TDSPlayer target, [TDSRemainingText] string message)
        {
            if (player == target)
                return;
            if (player.BlockingPlayerIds.Contains(target.Entity?.Id ?? 0))
            {
                NAPI.Chat.SendChatMessageToPlayer(player.Client, string.Format(player.Language.YOU_GOT_BLOCKED_BY, target.Client.Name));
                return;
            }
            ChatManager.SendPrivateMessage(player, target, message);
            NAPI.Chat.SendChatMessageToPlayer(player.Client, player.Language.PRIVATE_MESSAGE_SENT);
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

        [TDSCommand(DPlayerCommand.UserId)]
        public static void OutputUserId(TDSPlayer player)
        {
            NAPI.Chat.SendChatMessageToPlayer(player.Client, "User id: " + (player.Entity?.Id.ToString() ?? "?"));
        }

        [TDSCommand(DPlayerCommand.BlockUser)]
        public static async void BlockUser(TDSPlayer player, TDSPlayer target)
        {
            if (player.Entity == null || target.Entity == null)
                return;

            var relation = await player.DbContext.PlayerRelations.FindAsync(player.Entity.Id, target.Entity.Id);
            if (relation != null && relation.Relation == EPlayerRelation.Block)
            {
                //NAPI.Chat.SendChatMessageToPlayer(player.Client, string.Format(player.Language.TARGET_ALREADY_BLOCKED, target.Client.Name));
                UnblockUser(player, target);
                return;
            }

            string msg;
            if (relation != null && relation.Relation == EPlayerRelation.Friend)
            {
                msg = string.Format(player.Language.TARGET_REMOVED_FRIEND_ADDED_BLOCK, target.Client.Name);
                player.PlayerRelationsPlayer.Find(r => r.PlayerId == player.Entity?.Id && r.TargetId == target.Entity?.Id).Relation = EPlayerRelation.Block;
                target.PlayerRelationsTarget.Find(r => r.PlayerId == player.Entity?.Id && r.TargetId == target.Entity?.Id).Relation = EPlayerRelation.Block;
            }
            else
            {
                relation = new PlayerRelations { PlayerId = player.Entity.Id, TargetId = target.Entity.Id };
                player.DbContext.PlayerRelations.Add(relation);
                msg = string.Format(player.Language.TARGET_ADDED_BLOCK, target.Client.Name);
                player.PlayerRelationsPlayer.Add(relation);
                target.PlayerRelationsTarget.Add(relation);
            }

            if (player.InPrivateChatWith == target)
                player.ClosePrivateChat(false);
            target.Client.DisableVoiceTo(player.Client);

            relation.Relation = EPlayerRelation.Block;
            await player.DbContext.SaveChangesAsync();
            NAPI.Chat.SendChatMessageToPlayer(player.Client, msg);
            NAPI.Chat.SendChatMessageToPlayer(target.Client, string.Format(target.Language.YOU_GOT_BLOCKED_BY, player.Client.Name));
        }

        [TDSCommand(DPlayerCommand.UnblockUser)]
        public static async void UnblockUser(TDSPlayer player, TDSPlayer target)
        {
            if (player.Entity == null || target.Entity == null)
                return;

            var relation = await player.DbContext.PlayerRelations.FindAsync(player.Entity.Id, target.Entity.Id);
            if (relation == null || relation.Relation != EPlayerRelation.Block)
            {
                NAPI.Chat.SendChatMessageToPlayer(player.Client, string.Format(player.Language.TARGET_NOT_BLOCKED, target.Client.Name));
                return;
            }

            if (target.Team == player.Team) 
                target.Client.EnableVoiceTo(player.Client);

            player.DbContext.PlayerRelations.Remove(relation);
            await player.DbContext.SaveChangesAsync();
            player.PlayerRelationsPlayer.RemoveAll(r => r.PlayerId == player.Entity?.Id && r.TargetId == target.Entity?.Id);
            target.PlayerRelationsTarget.RemoveAll(r => r.PlayerId == player.Entity?.Id && r.TargetId == target.Entity?.Id);
            NAPI.Chat.SendChatMessageToPlayer(player.Client, string.Format(player.Language.YOU_UNBLOCKED, target.Client.Name));
            NAPI.Chat.SendChatMessageToPlayer(target.Client, string.Format(target.Language.YOU_GOT_BLOCKED_BY, player.Client.Name));
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