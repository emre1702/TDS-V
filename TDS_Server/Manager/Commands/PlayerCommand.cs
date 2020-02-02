using GTANetworkAPI;
using System;
using TDS_Common.Default;
using TDS_Common.Enum;
using TDS_Server.CustomAttribute;
using TDS_Server.Default;
using TDS_Server.Enums;
using TDS_Server.Instance.LobbyInstances;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Instance.Utility;
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
            if (player.CurrentLobby is null)
                return;
            if (player.CurrentLobby.LobbyEntity.Type == ELobbyType.MainMenu)
            {
                if (CustomLobbyMenuSync.IsPlayerInCustomLobbyMenu(player))
                {
                    CustomLobbyMenuSync.RemovePlayer(player);
                    NAPI.ClientEvent.TriggerClientEvent(player.Player, DToClientEvent.LeaveCustomLobbyMenu);
                }
                return;
            }

            player.CurrentLobby.RemovePlayer(player);
            await LobbyManager.MainMenu.AddPlayer(player, 0);
        }

        [TDSCommand(DPlayerCommand.Suicide)]
        public static void Suicide(TDSPlayer player)
        {
            if (player.Player is null)
                return;
            if (!(player.CurrentLobby is FightLobby fightLobby))
                return;
            if (player.Lifes == 0)
                return;

            string animName = "PILL";
            float animTime = 0.536f;
            switch ((EWeaponHash)player.Player.CurrentWeapon)
            {
                // Pistols //
                case EWeaponHash.Pistol:
                case EWeaponHash.CombatPistol:
                case EWeaponHash.APPistol:
                case EWeaponHash.Pistol50:
                case EWeaponHash.HeavyRevolver:
                case EWeaponHash.SNSPistol:
                case EWeaponHash.HeavyPistol:
                case EWeaponHash.DoubleActionRevolver:
                case EWeaponHash.HeavyRevolverMk2:
                case EWeaponHash.SNSPistolMk2:
                case EWeaponHash.PistolMk2:
                case EWeaponHash.VintagePistol:
                case EWeaponHash.MarksmanPistol:
                    animName = "PISTOL";
                    animTime = 0.365f;
                    break;
            }
            
            fightLobby.SendAllPlayerEvent(DToClientEvent.ApplySuicideAnimation, null, player.Player.Handle.Value, animName, animTime);
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
            if (player.Player is null || target.Player is null)
                return;
            // Am I blocked?
            if (player.BlockingPlayerIds.Contains(target.Entity?.Id ?? 0))
            {
                player.SendMessage(string.Format(player.Language.YOU_GOT_BLOCKED_BY, target.DisplayName));
                return;
            }

            // Am I already in chat?
            if (player.InPrivateChatWith != null)
            {
                player.Player.SendNotification(player.Language.ALREADY_IN_PRIVATE_CHAT_WITH.Formatted(player.InPrivateChatWith.DisplayName));
                return;
            }

            // Did I already send a request?
            if (player.SentPrivateChatRequestTo == target)
                return;

            // Withdraw my old request
            if (player.SentPrivateChatRequestTo != null)
            {
                TDSPlayer oldTargett = player.SentPrivateChatRequestTo;
                oldTargett.Player!.SendNotification(oldTargett.Language.PRIVATE_CHAT_REQUEST_CLOSED_REQUESTER.Formatted(player.DisplayName));
                player.SentPrivateChatRequestTo = null;
            }

            // Is Target already in a private chat?
            if (target.InPrivateChatWith != null)
            {
                player.Player.SendNotification(player.Language.TARGET_ALREADY_IN_PRIVATE_CHAT);
                return;
            }

            // Send request
            if (target.SentPrivateChatRequestTo != player)
            {
                player.Player.SendNotification(player.Language.PRIVATE_CHAT_REQUEST_SENT_TO.Formatted(target.DisplayName));
                target.Player.SendNotification(target.Language.PRIVATE_CHAT_REQUEST_RECEIVED_FROM.Formatted(player.DisplayName));
                player.SentPrivateChatRequestTo = target;
            }
            // Accept request
            else
            {
                player.Player.SendNotification(player.Language.PRIVATE_CHAT_OPENED_WITH.Formatted(target.DisplayName));
                target.Player.SendNotification(target.Language.PRIVATE_CHAT_OPENED_WITH.Formatted(player.DisplayName));
                target.SentPrivateChatRequestTo = null;
                player.InPrivateChatWith = target;
                target.InPrivateChatWith = player;
            }
        }

        [TDSCommand(DPlayerCommand.ClosePrivateChat)]
        public static void ClosePrivateChat(TDSPlayer player)
        {
            if (player.Player is null)
                return;
            if (player.InPrivateChatWith is null && player.SentPrivateChatRequestTo is null)
            {
                player.Player.SendChatMessage(player.Language.NOT_IN_PRIVATE_CHAT);
                return;
            }
            player.ClosePrivateChat(false);
        }

        [TDSCommand(DPlayerCommand.PrivateChat)]
        public static void PrivateChat(TDSPlayer player, [TDSRemainingText] string message)
        {
            if (player.Player is null)
                return;
            if (player.InPrivateChatWith is null)
            {
                player.Player.SendChatMessage(player.Language.NOT_IN_PRIVATE_CHAT);
                return;
            }
            string colorStr = "!$155|35|133$";
            player.InPrivateChatWith.Player?.SendChatMessage($"{colorStr}[{player.DisplayName}: {message}]");
        }

        [TDSCommand(DPlayerCommand.PrivateMessage)]
        public static void PrivateMessage(TDSPlayer player, TDSPlayer target, [TDSRemainingText] string message)
        {
            if (player == target)
                return;
            if (player.BlockingPlayerIds.Contains(target.Entity?.Id ?? 0))
            {
                player.SendMessage(string.Format(player.Language.YOU_GOT_BLOCKED_BY, target.DisplayName));
                return;
            }
            ChatManager.SendPrivateMessage(player, target, message);
            player.SendMessage(player.Language.PRIVATE_MESSAGE_SENT);
        }

        [TDSCommand(DPlayerCommand.Position)]
        public static void OutputCurrentPosition(TDSPlayer player)
        {
            if (player.Player is null)
                return;
            if (player.Player.IsInVehicle)
            {
                Vector3 pos = player.Player.Vehicle.Position;
                player.SendMessage("Vehicle X: " + pos.X + " Y: " + pos.Y + " Z: " + pos.Z);
                Vector3 rot = player.Player.Vehicle.Rotation;
                player.SendMessage("Vehicle ROT RX: " + rot.X + " RY: " + rot.Y + " RZ: " + rot.Z);
                player.SendMessage($"Vehicle dimension: {player.Player.Vehicle.Dimension} | Your dimension: {player.Player.Dimension}");
            }
            else
            {
                Vector3 pos = player.Player.Position;
                player.SendMessage("Player X: " + pos.X + " Y: " + pos.Y + " Z: " + pos.Z);
                Vector3 rot = player.Player.Rotation;
                player.SendMessage("Player ROT RX: " + rot.X + " RY: " + rot.Y + " RZ: " + rot.Z);
                player.SendMessage($"Dimension: {player.Player.Dimension}");
            }
        }

        [TDSCommand(DPlayerCommand.UserId)]
        public static void OutputUserId(TDSPlayer player)
        {
            player.SendMessage("User id: " + (player.Entity?.Id.ToString() ?? "?"));
        }

        [TDSCommand(DPlayerCommand.BlockUser)]
        public static async void BlockUser(TDSPlayer player, TDSPlayer target)
        {
            if (player.Player is null || target.Player is null)
                return;
            if (player.Entity is null || target.Entity is null)
                return;

            bool continuue = await player.ExecuteForDBAsync(async (dbContext) =>
            {
                PlayerRelations relation = await dbContext.PlayerRelations.FindAsync(player.Entity.Id, target.Entity.Id);

                if (relation != null && relation.Relation == EPlayerRelation.Block)
                {
                    //player.SendMessage(string.Format(player.Language.TARGET_ALREADY_BLOCKED, target.DisplayName));
                    UnblockUser(player, target);
                    return false;
                }

                string msg;
                if (relation != null && relation.Relation == EPlayerRelation.Friend)
                {
                    msg = string.Format(player.Language.TARGET_REMOVED_FRIEND_ADDED_BLOCK, target.DisplayName);
                    var playerRelation = player.PlayerRelationsPlayer.Find(r => r.PlayerId == player.Entity?.Id && r.TargetId == target.Entity?.Id);
                    if (playerRelation != null)
                        playerRelation.Relation = EPlayerRelation.Block;
                    var targetRelation = target.PlayerRelationsTarget.Find(r => r.PlayerId == player.Entity?.Id && r.TargetId == target.Entity?.Id);
                    if (targetRelation != null)
                        targetRelation.Relation = EPlayerRelation.Block;
                }
                else
                {
                    relation = new PlayerRelations { PlayerId = player.Entity.Id, TargetId = target.Entity.Id };
                    dbContext.PlayerRelations.Add(relation);
                    msg = string.Format(player.Language.TARGET_ADDED_BLOCK, target.DisplayName);
                    player.PlayerRelationsPlayer.Add(relation);
                    target.PlayerRelationsTarget.Add(relation);
                }
                relation.Relation = EPlayerRelation.Block;
                await dbContext.SaveChangesAsync();
                player.SendMessage(msg);

                return true;
            });

            if (!continuue)
                return;
            
            if (player.InPrivateChatWith == target)
                player.ClosePrivateChat(false);
            target.Player.DisableVoiceTo(player.Player);
 
            target.SendMessage(string.Format(target.Language.YOU_GOT_BLOCKED_BY, player.DisplayName));
        }

        [TDSCommand(DPlayerCommand.UnblockUser)]
        public static async void UnblockUser(TDSPlayer player, TDSPlayer target)
        {
            if (player.Player is null || target.Player is null)
                return;
            if (player.Entity is null || target.Entity is null)
                return;

            await player.ExecuteForDBAsync(async (dbContext) =>
            {
                var relation = await dbContext.PlayerRelations.FindAsync(player.Entity.Id, target.Entity.Id);
                if (relation is null || relation.Relation != EPlayerRelation.Block)
                {
                    player.SendMessage(string.Format(player.Language.TARGET_NOT_BLOCKED, target.DisplayName));
                    return;
                }

                dbContext.PlayerRelations.Remove(relation);
                await dbContext.SaveChangesAsync();
            });

            if (target.Team == player.Team)
                target.Player.EnableVoiceTo(player.Player);

            player.PlayerRelationsPlayer.RemoveAll(r => r.PlayerId == player.Entity?.Id && r.TargetId == target.Entity?.Id);
            target.PlayerRelationsTarget.RemoveAll(r => r.PlayerId == player.Entity?.Id && r.TargetId == target.Entity?.Id);
            player.SendMessage(string.Format(player.Language.YOU_UNBLOCKED, target.DisplayName));
            target.SendMessage(string.Format(target.Language.YOU_GOT_UNBLOCKED_BY, player.DisplayName));
        }

        [TDSCommand(DPlayerCommand.GiveMoney)]
        public static void GiveMoney(TDSPlayer player, TDSPlayer target, uint money)
        {
            if (player.Player is null || target.Player is null)
                return;
            if (player.Entity is null || target.Entity is null)
                return;

            if (money < SettingsManager.GiveMoneyMinAmount)
            {
                player.Player.SendNotification(player.Language.GIVE_MONEY_TOO_LESS);
                return;
            }

            uint fee = (uint)Math.Ceiling(money * SettingsManager.GiveMoneyFee);
            money += fee;

            if (player.Money < money)
            {
                player.Player.SendNotification(string.Format(player.Language.GIVE_MONEY_NEED_FEE, money, fee));
                return;
            }

            player.GiveMoney((int)money * -1);
            target.GiveMoney(money - fee);

            player.SendMessage(string.Format(player.Language.YOU_GAVE_MONEY_TO_WITH_FEE, money - fee, fee, target.DisplayName));
            target.SendMessage(string.Format(target.Language.YOU_GOT_MONEY_BY_WITH_FEE, money - fee, fee, player.DisplayName));
        }

        [TDSCommand(DPlayerCommand.LobbyInvitePlayer)]
        public static void LobbyInvitePlayer(TDSPlayer player, TDSPlayer target)
        {
            if (player.CurrentLobby is null)
                return;
            if (!player.IsLobbyOwner)
                return;

            switch (player.CurrentLobby.Type)
            {
                case ELobbyType.MapCreateLobby:
                    _ = new Invitation(string.Format(target.Language.INVITATION_MAPCREATELOBBY, player.DisplayName), 
                        target: target, 
                        sender: player, 
                        onAccept: async (sender, target, invitation) => 
                        {
                            await sender.CurrentLobby!.AddPlayer(target!, null);
                            target?.SendNotification(string.Format(target.Language.YOU_ACCEPTED_INVITATION, sender.DisplayName), false);
                            sender.SendNotification(string.Format(sender.Language.TARGET_ACCEPTED_INVITATION, target?.DisplayName ?? "?"), false);
                        },

                        onReject: (sender, target, invitation) => 
                        {
                            target?.SendNotification(string.Format(target.Language.YOU_REJECTED_INVITATION, sender.DisplayName), false);
                            sender.SendNotification(string.Format(sender.Language.TARGET_REJECTED_INVITATION, target?.DisplayName ?? "?"), false);
                        },

                        type: EInvitationType.Lobby
                    );
                    break;
            }
        }
    }
}
