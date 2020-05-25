using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Handler.Entities.Utility;
using TDS_Server.Handler.Events;

namespace TDS_Server.Core.Manager.Utility
{
    public class InvitationsHandler
    {
        #region Private Fields

        private readonly Dictionary<ulong, Invitation> _invitationById = new Dictionary<ulong, Invitation>();

        #endregion Private Fields

        #region Public Constructors

        public InvitationsHandler(EventsHandler eventsHandler)
        {
            eventsHandler.PlayerLeftLobby += RemoveSendersLobbyInvitations;
        }

        #endregion Public Constructors

        #region Public Methods

        public object? AcceptInvitation(ITDSPlayer player, ref ArraySegment<object> args)
        {
            if (!ulong.TryParse(Convert.ToString(args[0]), out ulong id))
                return null;

            Invitation? invitation = GetById(id);
            if (invitation is null)
            {
                player.SendNotification(player.Language.INVITATION_WAS_WITHDRAWN_OR_REMOVED);
                return null;
            }

            invitation.Accept();

            return null;
        }

        public void Add(Invitation invitation)
        {
            _invitationById[invitation.Dto.Id] = invitation;
        }

        public object? RejectInvitation(ITDSPlayer player, ref ArraySegment<object> args)
        {
            if (!ulong.TryParse(Convert.ToString(args[0]), out ulong id))
                return null;

            Invitation? invitation = GetById(id);
            if (invitation is null)
            {
                player.SendNotification(player.Language.INVITATION_WAS_WITHDRAWN_OR_REMOVED);
                return null;
            }

            invitation.Reject();

            return null;
        }

        public void Remove(Invitation invitation)
        {
            _invitationById.Remove(invitation.Dto.Id);
        }

        #endregion Public Methods

        #region Private Methods

        private Invitation? GetById(ulong id)
        {
            if (!_invitationById.TryGetValue(id, out Invitation? invitation))
                return null;
            return invitation;
        }

        private IEnumerable<Invitation> GetBySender(ITDSPlayer sender, InvitationType type)
        {
            return _invitationById.Values.Where(i => i.Sender == sender && i.Type == type);
        }

        private IEnumerable<Invitation> GetByTarget(ITDSPlayer target)
        {
            return _invitationById.Values.Where(i => i.Target == target);
        }

        private void RemoveSendersLobbyInvitations(ITDSPlayer player, ILobby lobby)
        {
            var invitations = GetBySender(player, InvitationType.Lobby);
            foreach (var invitation in invitations)
            {
                invitation.Withdraw();
            }

            invitations = GetByTarget(player).Where(i => i.RemoveOnLobbyLeave);
            foreach (var invitation in invitations)
            {
                invitation.Withdraw();
            }
        }

        #endregion Private Methods

        /*private IEnumerable<Invitation> GetBySender(ITDSPlayer sender)
        {
            return _invitationById.Values.Where(i => i.Sender == sender);
        }*/
    }
}
