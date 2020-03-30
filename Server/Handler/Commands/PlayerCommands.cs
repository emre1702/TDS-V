﻿using System;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.CustomAttribute;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Server.Handler.Entities.Player;
using TDS_Server.Handler.Entities.Utility;
using TDS_Server.Handler.Sync;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;
using TDS_Shared.Core;

namespace TDS_Server.Handler.Commands
{
    partial class BaseCommands
    {

        [TDSCommand(PlayerCommand.LobbyLeave)]
        public async void OnLobbyLeave(TDSPlayer player)
        {
            if (player.Lobby is null)
                return;
            if (player.Lobby.Entity.Type == LobbyType.MainMenu)
            {
                if (_customLobbyMenuSyncHandler.IsPlayerInCustomLobbyMenu(player))
                {
                    _customLobbyMenuSyncHandler.RemovePlayer(player);
                    player.SendEvent(ToClientEvent.LeaveCustomLobbyMenu);
                }
                return;
            }

            player.Lobby.RemovePlayer(player);
            await _lobbiesHandler.MainMenu.AddPlayer(player, 0);
        }

        [TDSCommand(PlayerCommand.Suicide)]
        public void Suicide(TDSPlayer player)
        {
            if (player.ModPlayer is null)
                return;
            if (!(player.Lobby is FightLobby fightLobby))
                return;
            if (player.Lifes == 0)
                return;

            string animName = "PILL";
            float animTime = 0.536f;
            switch (player.ModPlayer.CurrentWeapon)
            {
                // Pistols //
                case WeaponHash.Pistol:
                case WeaponHash.Combatpistol:
                case WeaponHash.Appistol:
                case WeaponHash.Pistol50:
                case WeaponHash.Revolver:
                case WeaponHash.Snspistol:
                case WeaponHash.Heavypistol:
                case WeaponHash.Doubleaction:
                case WeaponHash.Revolver_mk2:
                case WeaponHash.Snspistol_mk2:
                case WeaponHash.Pistol_mk2:
                case WeaponHash.Vintagepistol:
                case WeaponHash.Marksmanpistol:
                    animName = "PISTOL";
                    animTime = 0.365f;
                    break;
            }

            _modAPI.Sync.SendEvent(fightLobby, ToClientEvent.ApplySuicideAnimation, player.RemoteId, animName, animTime);
        }

        [TDSCommand(PlayerCommand.GlobalChat)]
        public void GlobalChat(TDSPlayer player, [TDSRemainingText] string message)
        {
            _chatHandler.SendGlobalMessage(player, message);
        }

        [TDSCommand(PlayerCommand.TeamChat)]
        public void TeamChat(TDSPlayer player, [TDSRemainingText] string message)
        {
            _chatHandler.SendTeamChat(player, message);
        }

        [TDSCommand(PlayerCommand.OpenPrivateChat)]
        public void OpenPrivateChat(TDSPlayer player, TDSPlayer target)
        {
            if (player.ModPlayer is null || target.ModPlayer is null)
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
                player.SendNotification(string.Format(player.Language.ALREADY_IN_PRIVATE_CHAT_WITH, player.InPrivateChatWith.DisplayName));
                return;
            }

            // Did I already send a request?
            if (player.SentPrivateChatRequestTo == target)
                return;

            // Withdraw my old request
            if (player.SentPrivateChatRequestTo != null)
            {
                ITDSPlayer oldTargett = player.SentPrivateChatRequestTo;
                oldTargett.SendNotification(string.Format(oldTargett.Language.PRIVATE_CHAT_REQUEST_CLOSED_REQUESTER, player.DisplayName));
                player.SentPrivateChatRequestTo = null;
            }

            // Is Target already in a private chat?
            if (target.InPrivateChatWith != null)
            {
                player.ModPlayer.SendNotification(player.Language.TARGET_ALREADY_IN_PRIVATE_CHAT);
                return;
            }

            // Send request
            if (target.SentPrivateChatRequestTo != player)
            {
                player.ModPlayer.SendNotification(string.Format(player.Language.PRIVATE_CHAT_REQUEST_SENT_TO, target.DisplayName));
                target.SendNotification(string.Format(target.Language.PRIVATE_CHAT_REQUEST_RECEIVED_FROM, player.DisplayName));
                player.SentPrivateChatRequestTo = target;
            }
            // Accept request
            else
            {
                player.ModPlayer.SendNotification(string.Format(player.Language.PRIVATE_CHAT_OPENED_WITH, target.DisplayName));
                target.SendNotification(string.Format(target.Language.PRIVATE_CHAT_OPENED_WITH, player.DisplayName));
                target.SentPrivateChatRequestTo = null;
                player.InPrivateChatWith = target;
                target.InPrivateChatWith = player;
            }
        }

        [TDSCommand(PlayerCommand.ClosePrivateChat)]
        public void ClosePrivateChat(TDSPlayer player)
        {
            if (player.ModPlayer is null)
                return;
            if (player.InPrivateChatWith is null && player.SentPrivateChatRequestTo is null)
            {
                player.SendMessage(player.Language.NOT_IN_PRIVATE_CHAT);
                return;
            }
            player.ClosePrivateChat(false);
        }

        [TDSCommand(PlayerCommand.PrivateChat)]
        public void PrivateChat(TDSPlayer player, [TDSRemainingText] string message)
        {
            if (player.ModPlayer is null)
                return;
            if (player.InPrivateChatWith is null)
            {
                player.SendMessage(player.Language.NOT_IN_PRIVATE_CHAT);
                return;
            }
            string colorStr = "!$155|35|133$";
            player.InPrivateChatWith.SendMessage($"{colorStr}[{player.DisplayName}: {message}]");
        }

        [TDSCommand(PlayerCommand.PrivateMessage)]
        public void PrivateMessage(TDSPlayer player, TDSPlayer target, [TDSRemainingText] string message)
        {
            if (player == target)
                return;
            if (player.BlockingPlayerIds.Contains(target.Entity?.Id ?? 0))
            {
                player.SendMessage(string.Format(player.Language.YOU_GOT_BLOCKED_BY, target.DisplayName));
                return;
            }
            _chatHandler.SendPrivateMessage(player, target, message);
            player.SendMessage(player.Language.PRIVATE_MESSAGE_SENT);
        }

        [TDSCommand(PlayerCommand.Position)]
        public void OutputCurrentPosition(TDSPlayer player)
        {
            if (player.ModPlayer is null)
                return;
            if (player.ModPlayer.IsInVehicle)
            {
                var pos = player.ModPlayer.Vehicle.Position;
                player.SendMessage("Vehicle X: " + pos.X + " Y: " + pos.Y + " Z: " + pos.Z);
                var rot = player.ModPlayer.Vehicle.Rotation;
                player.SendMessage("Vehicle ROT RX: " + rot.X + " RY: " + rot.Y + " RZ: " + rot.Z);
                player.SendMessage($"Vehicle dimension: {player.ModPlayer.Vehicle.Dimension} | Your dimension: {player.ModPlayer.Dimension}");
            }
            else
            {
                var pos = player.ModPlayer.Position;
                player.SendMessage("Player X: " + pos.X + " Y: " + pos.Y + " Z: " + pos.Z);
                var rot = player.ModPlayer.Rotation;
                player.SendMessage("Player ROT: " + rot);
                player.SendMessage($"Dimension: {player.ModPlayer.Dimension}");
            }
        }

        [TDSCommand(PlayerCommand.UserId)]
        public void OutputUserId(TDSPlayer player)
        {
            player.SendMessage("User id: " + (player.Entity?.Id.ToString() ?? "?"));
        }

        [TDSCommand(PlayerCommand.BlockUser)]
        public async void BlockUser(ITDSPlayer player, ITDSPlayer target)
        {
            if (player.ModPlayer is null || target.ModPlayer is null)
                return;
            if (player.Entity is null || target.Entity is null)
                return;

            bool continuue = await player.ExecuteForDBAsync(async (dbContext) =>
            {
                PlayerRelations relation = await dbContext.PlayerRelations.FindAsync(player.Entity.Id, target.Entity.Id);

                if (relation != null && relation.Relation == PlayerRelation.Block)
                {
                    //player.SendMessage(string.Format(player.Language.TARGET_ALREADY_BLOCKED, target.DisplayName));
                    UnblockUser(player, target);
                    return false;
                }

                string msg;
                if (relation != null && relation.Relation == PlayerRelation.Friend)
                {
                    msg = string.Format(player.Language.TARGET_REMOVED_FRIEND_ADDED_BLOCK, target.DisplayName);
                    var playerRelation = player.PlayerRelationsPlayer.Find(r => r.PlayerId == player.Entity?.Id && r.TargetId == target.Entity?.Id);
                    if (playerRelation != null)
                        playerRelation.Relation = PlayerRelation.Block;
                    var targetRelation = target.PlayerRelationsTarget.Find(r => r.PlayerId == player.Entity?.Id && r.TargetId == target.Entity?.Id);
                    if (targetRelation != null)
                        targetRelation.Relation = PlayerRelation.Block;
                }
                else
                {
                    relation = new PlayerRelations { PlayerId = player.Entity.Id, TargetId = target.Entity.Id };
                    dbContext.PlayerRelations.Add(relation);
                    msg = string.Format(player.Language.TARGET_ADDED_BLOCK, target.DisplayName);
                    player.PlayerRelationsPlayer.Add(relation);
                    target.PlayerRelationsTarget.Add(relation);
                }
                relation.Relation = PlayerRelation.Block;
                await dbContext.SaveChangesAsync();
                player.SendMessage(msg);

                return true;
            });

            if (!continuue)
                return;

            if (player.InPrivateChatWith == target)
                player.ClosePrivateChat(false);
            target.ModPlayer.SetVoiceTo(player, false);

            target.SendMessage(string.Format(target.Language.YOU_GOT_BLOCKED_BY, player.DisplayName));
        }

        [TDSCommand(PlayerCommand.UnblockUser)]
        public async void UnblockUser(ITDSPlayer player, ITDSPlayer target)
        {
            if (player.ModPlayer is null || target.ModPlayer is null)
                return;
            if (player.Entity is null || target.Entity is null)
                return;

            await player.ExecuteForDBAsync(async (dbContext) =>
            {
                var relation = await dbContext.PlayerRelations.FindAsync(player.Entity.Id, target.Entity.Id);
                if (relation is null || relation.Relation != PlayerRelation.Block)
                {
                    player.SendMessage(string.Format(player.Language.TARGET_NOT_BLOCKED, target.DisplayName));
                    return;
                }

                dbContext.PlayerRelations.Remove(relation);
                await dbContext.SaveChangesAsync();
            });

            if (target.Team == player.Team)
                target.SetVoiceTo(player, true);

            player.PlayerRelationsPlayer.RemoveAll(r => r.PlayerId == player.Entity?.Id && r.TargetId == target.Entity?.Id);
            target.PlayerRelationsTarget.RemoveAll(r => r.PlayerId == player.Entity?.Id && r.TargetId == target.Entity?.Id);
            player.SendMessage(string.Format(player.Language.YOU_UNBLOCKED, target.DisplayName));
            target.SendMessage(string.Format(target.Language.YOU_GOT_UNBLOCKED_BY, player.DisplayName));
        }

        [TDSCommand(PlayerCommand.GiveMoney)]
        public void GiveMoney(TDSPlayer player, TDSPlayer target, uint money)
        {
            if (player.ModPlayer is null || target.ModPlayer is null)
                return;
            if (player.Entity is null || target.Entity is null)
                return;

            if (money < _settingsHandler.ServerSettings.GiveMoneyMinAmount)
            {
                player.ModPlayer.SendNotification(player.Language.GIVE_MONEY_TOO_LESS);
                return;
            }

            uint fee = (uint)Math.Ceiling(money * _settingsHandler.ServerSettings.GiveMoneyFee);
            money += fee;

            if (player.Money < money)
            {
                player.ModPlayer.SendNotification(string.Format(player.Language.GIVE_MONEY_NEED_FEE, money, fee));
                return;
            }

            player.GiveMoney((int)money * -1);
            target.GiveMoney(money - fee);

            player.SendMessage(string.Format(player.Language.YOU_GAVE_MONEY_TO_WITH_FEE, money - fee, fee, target.DisplayName));
            target.SendMessage(string.Format(target.Language.YOU_GOT_MONEY_BY_WITH_FEE, money - fee, fee, player.DisplayName));
        }

        [TDSCommand(PlayerCommand.LobbyInvitePlayer)]
        public void LobbyInvitePlayer(TDSPlayer player, TDSPlayer target)
        {
            if (player.Lobby is null)
                return;
            if (!player.IsLobbyOwner)
                return;

            switch (player.Lobby.Type)
            {
                case LobbyType.MapCreateLobby:
                    _ = new Invitation(string.Format(target.Language.INVITATION_MAPCREATELOBBY, player.DisplayName),
                        target: target,
                        sender: player,
                        serializer: _serializer,
                        invitationsHandler: _invitationsHandler,
                        onAccept: async (sender, target, invitation) =>
                        {
                            await sender.Lobby!.AddPlayer(target!, null);
                            target?.SendNotification(string.Format(target.Language.YOU_ACCEPTED_INVITATION, sender.DisplayName), false);
                            sender.SendNotification(string.Format(sender.Language.TARGET_ACCEPTED_INVITATION, target?.DisplayName ?? "?"), false);
                        },

                        onReject: (sender, target, invitation) =>
                        {
                            target?.SendNotification(string.Format(target.Language.YOU_REJECTED_INVITATION, sender.DisplayName), false);
                            sender.SendNotification(string.Format(sender.Language.TARGET_REJECTED_INVITATION, target?.DisplayName ?? "?"), false);
                        },

                        type: InvitationType.Lobby
                    );
                    break;
            }
        }
    }
}
