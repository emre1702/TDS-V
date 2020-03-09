using GTANetworkAPI;
using System;
using System.Linq;
using TDS_Server.Enums;
using TDS_Server.Instance.LobbyInstances;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Instance.Utility;
using TDS_Server.Manager.EventManager;

namespace TDS_Server.Core.Manager.Utility
{
    static class InvitationManager
    {
        public static void Init()
        {
            CustomEventManager.OnPlayerLeftLobby += RemoveSendersLobbyInvitations;
        }

        private static void RemoveSendersLobbyInvitations(TDSPlayer player, Lobby lobby)
        {
            var invitations = Invitation.GetBySender(player, EInvitationType.Lobby);
            foreach (var invitation in invitations)
            {
                invitation.Withdraw();
            }

            invitations = Invitation.GetByTarget(player).Where(i => i.RemoveOnLobbyLeave);
            foreach (var invitation in invitations)
            {
                invitation.Withdraw();
            }
        }

        public static object? AcceptInvitation(TDSPlayer player, object[] args)
        {
            if (!ulong.TryParse(Convert.ToString(args[0]), out ulong id))
                return null;

            Invitation? invitation = Invitation.GetById(id);
            if (invitation is null)
            {
                player.SendNotification(player.Language.INVITATION_WAS_WITHDRAWN_OR_REMOVED);
                return null;
            }
                
            invitation.Accept();

            return null;
        }

        public static object? RejectInvitation(TDSPlayer player, object[] args)
        {
            if (!ulong.TryParse(Convert.ToString(args[0]), out ulong id))
                return null;

            Invitation? invitation = Invitation.GetById(id);
            if (invitation is null)
            {
                player.SendNotification(player.Language.INVITATION_WAS_WITHDRAWN_OR_REMOVED);
                return null;
            }

            invitation.Reject();

            return null;
        }
    }
}
