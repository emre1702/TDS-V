using GTANetworkAPI;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.CustomAttribute;
using TDS.Server.Data.Defaults;
using TDS.Server.Data.Enums;
using TDS.Server.Handler.Entities.Utility;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.Sync;
using TDS.Shared.Data.Enums;
using TDS.Shared.Default;

namespace TDS.Server.Handler.Commands.User
{
    public class UserLobbyCommands
    {
        private readonly InvitationsHandler _invitationsHandler;
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly CustomLobbyMenuSyncHandler _customLobbyMenuSyncHandler;

        public UserLobbyCommands(InvitationsHandler invitationsHandler, CustomLobbyMenuSyncHandler customLobbyMenuSyncHandler, LobbiesHandler lobbiesHandler)
            => (_invitationsHandler, _customLobbyMenuSyncHandler, _lobbiesHandler) = (invitationsHandler, customLobbyMenuSyncHandler, lobbiesHandler);

        [TDSCommand(UserCommand.LobbyInvitePlayer)]
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
                            await sender.Lobby.Players.AddPlayer(target!, 0).ConfigureAwait(false);
                            NAPI.Task.RunSafe(() =>
                            {
                                target.SendNotification(string.Format(target.Language.YOU_ACCEPTED_INVITATION, sender.DisplayName), false);
                                sender.SendNotification(string.Format(sender.Language.TARGET_ACCEPTED_INVITATION, target.DisplayName), false);
                            });
                        },

                        onReject: (target, sender, invitation) =>
                        {
                            NAPI.Task.RunSafe(() =>
                            {
                                target.SendNotification(string.Format(target.Language.YOU_REJECTED_INVITATION, sender?.DisplayName ?? "?"), false);
                                sender?.SendNotification(string.Format(sender.Language.TARGET_REJECTED_INVITATION, target.DisplayName), false);
                            });
                        },

                        type: InvitationType.Lobby
                    );
                    break;

                default:
                    NAPI.Task.RunSafe(() => player.SendNotification(player.Language.NOT_POSSIBLE_IN_THIS_LOBBY));
                    break;
            }
        }

        [TDSCommand(UserCommand.LobbyLeave)]
        public async Task OnLobbyLeave(ITDSPlayer player)
        {
            if (player.Lobby is null)
                return;
            if (player.Lobby.Entity.Type == LobbyType.MainMenu)
            {
                if (_customLobbyMenuSyncHandler.IsPlayerInCustomLobbyMenu(player))
                {
                    _customLobbyMenuSyncHandler.RemovePlayer(player);
                    NAPI.Task.RunSafe(() => player.TriggerEvent(ToClientEvent.ToBrowserEvent, ToBrowserEvent.LeaveCustomLobbyMenu));
                }
                return;
            }

            await _lobbiesHandler.MainMenu.Players.AddPlayer(player, 0).ConfigureAwait(false);
        }
    }
}
