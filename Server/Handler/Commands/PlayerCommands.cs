using GTANetworkAPI;
using System;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.CustomAttribute;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Entities.Utility;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Commands
{
    partial class BaseCommands
    {
        [TDSCommand(PlayerCommand.BlockUser)]
        public async Task BlockUser(ITDSPlayer player, ITDSPlayer target)
        {
            if (player.Entity is null || target.Entity is null)
                return;

            bool continuue = await player.Database.ExecuteForDBAsync(async (dbContext) =>
            {
                PlayerRelations relation = await dbContext.PlayerRelations.FindAsync(player.Entity.Id, target.Entity.Id);

                if (relation != null && relation.Relation == PlayerRelation.Block)
                {
                    //player.SendChatMessage(string.Format(player.Language.TARGET_ALREADY_BLOCKED, target.DisplayName));
                    await UnblockUser(player, target);
                    return false;
                }

                string msg;
                if (relation != null && relation.Relation == PlayerRelation.Friend)
                {
                    msg = string.Format(player.Language.TARGET_REMOVED_FRIEND_ADDED_BLOCK, target.DisplayName);
                }
                else
                {
                    relation = new PlayerRelations { PlayerId = player.Entity.Id, TargetId = target.Entity.Id };
                    dbContext.PlayerRelations.Add(relation);
                    msg = string.Format(player.Language.TARGET_ADDED_BLOCK, target.DisplayName);
                }
                player.Relations.SetRelation(target, PlayerRelation.Block);
                relation.Relation = PlayerRelation.Block;
                await dbContext.SaveChangesAsync();
                NAPI.Task.Run(() => player.SendChatMessage(msg));

                return true;
            });

            if (!continuue)
                return;

            NAPI.Task.Run(() =>
            {
                if (player.InPrivateChatWith == target)
                    player.Chat.ClosePrivateChat(false);
                target.Voice.SetVoiceTo(player, false);

                target.SendChatMessage(string.Format(target.Language.YOU_GOT_BLOCKED_BY, player.DisplayName));
            });
        }

        [TDSCommand(PlayerCommand.ClosePrivateChat)]
        public void ClosePrivateChat(ITDSPlayer player)
        {
            if (player.InPrivateChatWith is null && player.SentPrivateChatRequestTo is null)
            {
                player.SendChatMessage(player.Language.NOT_IN_PRIVATE_CHAT);
                return;
            }
            player.Chat.ClosePrivateChat(false);
        }

        [TDSCommand(PlayerCommand.GiveMoney)]
        public void GiveMoney(ITDSPlayer player, ITDSPlayer target, uint money)
        {
            if (player.Entity is null || target.Entity is null)
                return;

            if (money < _settingsHandler.ServerSettings.GiveMoneyMinAmount)
            {
                player.SendNotification(player.Language.GIVE_MONEY_TOO_LESS);
                return;
            }

            uint fee = (uint)Math.Ceiling(money * _settingsHandler.ServerSettings.GiveMoneyFee);
            money += fee;

            if (player.Money < money)
            {
                player.SendNotification(string.Format(player.Language.GIVE_MONEY_NEED_FEE, money, fee));
                return;
            }

            player.MoneyHandler.GiveMoney((int)money * -1);
            target.MoneyHandler.GiveMoney(money - fee);

            player.SendChatMessage(string.Format(player.Language.YOU_GAVE_MONEY_TO_WITH_FEE, money - fee, fee, target.DisplayName));
            target.SendChatMessage(string.Format(target.Language.YOU_GOT_MONEY_BY_WITH_FEE, money - fee, fee, player.DisplayName));
        }

        [TDSCommand(PlayerCommand.GlobalChat)]
        public void GlobalChat(ITDSPlayer player, [TDSRemainingText] string message)
        {
            _chatHandler.SendGlobalMessage(player, message);
        }

        [TDSCommand(PlayerCommand.LobbyInvitePlayer)]
        public void LobbyInvitePlayer(ITDSPlayer player, ITDSPlayer target)
        {
            if (player.Lobby is null)
                return;

            switch (player.Lobby.Type)
            {
                case LobbyType.DamageTestLobby:
                case LobbyType.MapCreateLobby:
                    _ = new Invitation(string.Format(target.Language.INVITATION_LOBBY, player.DisplayName, player.Lobby.Type.ToString()),
                        target: target,
                        sender: player,
                        invitationsHandler: _invitationsHandler,
                        onAccept: async (target, sender, invitation) =>
                        {
                            if (sender is null)
                                return;
                            if (sender.Lobby is null)
                                return;
                            await sender.Lobby.Players.AddPlayer(target!, 0);
                            NAPI.Task.Run(() =>
                            {
                                target.SendNotification(string.Format(target.Language.YOU_ACCEPTED_INVITATION, sender.DisplayName), false);
                                sender.SendNotification(string.Format(sender.Language.TARGET_ACCEPTED_INVITATION, target.DisplayName), false);
                            });
                        },

                        onReject: (target, sender, invitation) =>
                        {
                            target.SendNotification(string.Format(target.Language.YOU_REJECTED_INVITATION, sender?.DisplayName ?? "?"), false);
                            sender?.SendNotification(string.Format(sender.Language.TARGET_REJECTED_INVITATION, target.DisplayName), false);
                        },

                        type: InvitationType.Lobby
                    );
                    break;

                default:
                    player.SendNotification(player.Language.NOT_POSSIBLE_IN_THIS_LOBBY);
                    break;
            }
        }

        [TDSCommand(PlayerCommand.LobbyLeave)]
        public async Task OnLobbyLeave(ITDSPlayer player)
        {
            if (player.Lobby is null)
                return;
            if (player.Lobby.Entity.Type == LobbyType.MainMenu)
            {
                NAPI.Task.Run(() =>
                {
                    if (_customLobbyMenuSyncHandler.IsPlayerInCustomLobbyMenu(player))
                    {
                        _customLobbyMenuSyncHandler.RemovePlayer(player);
                        player.TriggerEvent(ToClientEvent.ToBrowserEvent, ToBrowserEvent.LeaveCustomLobbyMenu);
                    }
                });
                return;
            }

            await _lobbiesHandler.MainMenu.Players.AddPlayer(player, 0);
        }

        [TDSCommand(PlayerCommand.OpenPrivateChat)]
        public void OpenPrivateChat(ITDSPlayer player, ITDSPlayer target)
        {
            // Am I blocked?
            if (target.Relations.HasRelationTo(player, PlayerRelation.Block))
            {
                player.SendChatMessage(string.Format(player.Language.YOU_GOT_BLOCKED_BY, target.DisplayName));
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
                player.SendNotification(player.Language.TARGET_ALREADY_IN_PRIVATE_CHAT);
                return;
            }

            // Send request
            if (target.SentPrivateChatRequestTo != player)
            {
                player.SendNotification(string.Format(player.Language.PRIVATE_CHAT_REQUEST_SENT_TO, target.DisplayName));
                target.SendNotification(string.Format(target.Language.PRIVATE_CHAT_REQUEST_RECEIVED_FROM, player.DisplayName));
                player.SentPrivateChatRequestTo = target;
            }
            // Accept request
            else
            {
                player.SendNotification(string.Format(player.Language.PRIVATE_CHAT_OPENED_WITH, target.DisplayName));
                target.SendNotification(string.Format(target.Language.PRIVATE_CHAT_OPENED_WITH, player.DisplayName));
                target.SentPrivateChatRequestTo = null;
                player.InPrivateChatWith = target;
                target.InPrivateChatWith = player;
            }
        }

        [TDSCommand(PlayerCommand.Position)]
        public void OutputCurrentPosition(ITDSPlayer player)
        {
            if (player.IsInVehicle && player.Vehicle is { })
            {
                var pos = player.Vehicle.Position;
                player.SendChatMessage("Vehicle X: " + pos.X + " Y: " + pos.Y + " Z: " + pos.Z);
                var rot = player.Vehicle.Rotation;
                player.SendChatMessage("Vehicle ROT RX: " + rot.X + " RY: " + rot.Y + " RZ: " + rot.Z);
                player.SendChatMessage($"Vehicle dimension: {player.Vehicle.Dimension} | Your dimension: {player.Dimension}");
            }
            else
            {
                var pos = player.Position;
                player.SendChatMessage("Player X: " + pos.X + " Y: " + pos.Y + " Z: " + pos.Z);
                var rot = player.Rotation;
                player.SendChatMessage("Player ROT: " + rot);
                player.SendChatMessage($"Dimension: {player.Dimension}");
            }
        }

        [TDSCommand(PlayerCommand.UserId)]
        public void OutputUserId(ITDSPlayer player)
        {
            player.SendChatMessage("User id: " + (player.Entity?.Id.ToString() ?? "?"));
        }

        [TDSCommand(PlayerCommand.PrivateChat)]
        public void PrivateChat(ITDSPlayer player, [TDSRemainingText] string message)
        {
            if (player.InPrivateChatWith is null)
            {
                player.SendChatMessage(player.Language.NOT_IN_PRIVATE_CHAT);
                return;
            }
            string colorStr = "!$155|35|133$";
            player.InPrivateChatWith.SendChatMessage($"{colorStr}[{player.DisplayName}: {message}]");
        }

        [TDSCommand(PlayerCommand.PrivateMessage)]
        public void PrivateMessage(ITDSPlayer player, ITDSPlayer target, [TDSRemainingText] string message)
        {
            if (player == target)
                return;
            if (target.Relations.HasRelationTo(player, PlayerRelation.Block))
            {
                player.SendChatMessage(string.Format(player.Language.YOU_GOT_BLOCKED_BY, target.DisplayName));
                return;
            }
            _chatHandler.SendPrivateMessage(player, target, message);
            player.SendChatMessage(player.Language.PRIVATE_MESSAGE_SENT);
        }

        [TDSCommand(PlayerCommand.Suicide)]
        public void Suicide(ITDSPlayer player)
        {
            if (!(player.Lobby is IFightLobby fightLobby))
                return;
            if (player.Lifes == 0)
                return;

            string animName = "PILL";
            float animTime = 0.536f;
            switch (player.CurrentWeapon)
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

            fightLobby.Sync.TriggerEvent(ToClientEvent.ApplySuicideAnimation, player.RemoteId, animName, animTime);
        }

        [TDSCommand(PlayerCommand.TeamChat)]
        public void TeamChat(ITDSPlayer player, [TDSRemainingText] string message)
        {
            _chatHandler.SendTeamChat(player, message);
        }

        [TDSCommand(PlayerCommand.UnblockUser)]
        public async Task UnblockUser(ITDSPlayer player, ITDSPlayer target)
        {
            if (player.Entity is null || target.Entity is null)
                return;

            await player.Database.ExecuteForDBAsync(async (dbContext) =>
            {
                var relation = await dbContext.PlayerRelations.FindAsync(player.Entity.Id, target.Entity.Id);
                if (relation is null || relation.Relation != PlayerRelation.Block)
                {
                    NAPI.Task.Run(() => player.SendChatMessage(string.Format(player.Language.TARGET_NOT_BLOCKED, target.DisplayName)));
                    return;
                }

                dbContext.PlayerRelations.Remove(relation);
                await dbContext.SaveChangesAsync();
            });

            NAPI.Task.Run(() =>
            {
                if (target.Team == player.Team)
                    target.Voice.SetVoiceTo(player, true);

                player.Relations.SetRelation(target, PlayerRelation.None);
                player.SendChatMessage(string.Format(player.Language.YOU_UNBLOCKED, target.DisplayName));
                target.SendChatMessage(string.Format(target.Language.YOU_GOT_UNBLOCKED_BY, player.DisplayName));
            });
        }
    }
}
