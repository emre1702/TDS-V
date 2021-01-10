using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Models;
using TDS.Server.Handler.Entities.Utility;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Default;

namespace TDS.Server.Handler
{
    public class InvitationsHandler
    {
        private readonly Dictionary<ulong, Invitation> _invitationById = new Dictionary<ulong, Invitation>();

        public InvitationsHandler(EventsHandler eventsHandler, RemoteBrowserEventsHandler remoteBrowserEventsHandler)
        {
            eventsHandler.PlayerLeftLobby += RemoveSendersLobbyInvitations;
            eventsHandler.PlayerLeftGang += (player, _) => RemoveSendersGangInvitations(player);
            eventsHandler.PlayerLeftGang += (player, _) => RemoveTargetsGangActionInvitations(player);
            eventsHandler.PlayerLoggedOut += RemoveSendersGangInvitations;

            remoteBrowserEventsHandler.Add(ToServerEvent.AcceptInvitation, AcceptInvitation);
            remoteBrowserEventsHandler.Add(ToServerEvent.RejectInvitation, RejectInvitation);
        }

        public object? AcceptInvitation(RemoteBrowserEventArgs args)
        {
            if (!ulong.TryParse(Convert.ToString(args.Args[0]), out ulong id))
                return null;

            Invitation? invitation = GetById(id);
            if (invitation is null)
            {
                NAPI.Task.RunSafe(() =>
                    args.Player.SendNotification(args.Player.Language.INVITATION_WAS_WITHDRAWN_OR_REMOVED));
                return null;
            }

            invitation.Accept();

            return null;
        }

        public void Add(Invitation invitation)
        {
            lock (_invitationById)
                _invitationById[invitation.Dto.Id] = invitation;
        }

        public object? RejectInvitation(RemoteBrowserEventArgs args)
        {
            if (!ulong.TryParse(Convert.ToString(args.Args[0]), out ulong id))
                return null;

            Invitation? invitation = GetById(id);
            if (invitation is null)
            {
                NAPI.Task.RunSafe(() =>
                    args.Player.SendNotification(args.Player.Language.INVITATION_WAS_WITHDRAWN_OR_REMOVED));
                return null;
            }

            invitation.Reject();

            return null;
        }

        public void Remove(Invitation invitation)
        {
            lock (_invitationById)
                _invitationById.Remove(invitation.Dto.Id);
        }

        public Invitation? FindInvitation(ITDSPlayer sender, ITDSPlayer target, InvitationType type)
        {
            lock (_invitationById)
            {
                return _invitationById.Values.FirstOrDefault(i => i.Sender == sender && i.Target == target && i.Type == type);
            }
        }

        private Invitation? GetById(ulong id)
        {
            lock (_invitationById)
            {
                if (!_invitationById.TryGetValue(id, out Invitation? invitation))
                    return null;
                return invitation;
            }
        }

        private IEnumerable<Invitation> GetBySender(ITDSPlayer sender, InvitationType type)
        {
            lock (_invitationById)
            {
                return _invitationById.Values.Where(i => i.Sender == sender && i.Type == type);
            }
        }

        private IEnumerable<Invitation> GetByTarget(ITDSPlayer target)
        {
            lock (_invitationById)
            {
                return _invitationById.Values.Where(i => i.Target == target);
            }
        }

        private IEnumerable<Invitation> GetByTarget(ITDSPlayer sender, InvitationType type)
        {
            lock (_invitationById)
            {
                return _invitationById.Values.Where(i => i.Target == sender && i.Type == type);
            }
        }

        private void RemoveSendersLobbyInvitations(ITDSPlayer player, IBaseLobby lobby)
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

        private void RemoveSendersGangInvitations(ITDSPlayer player)
        {
            var invitations = GetBySender(player, InvitationType.Gang);
            foreach (var invitation in invitations)
            {
                invitation.Withdraw();
            }
        }

        private void RemoveTargetsGangActionInvitations(ITDSPlayer player)
        {
            var invitations = GetByTarget(player, InvitationType.GangAction);
            foreach (var invitation in invitations)
            {
                invitation.Withdraw();
            }
        }
    }
}