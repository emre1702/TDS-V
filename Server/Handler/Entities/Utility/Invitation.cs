using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models;
using TDS_Server.Handler.Entities.Player;
using TDS_Shared.Manager.Utility;

namespace TDS_Server.Handler.Entities.Utility
{
    class Invitation
    {
        public InvitationDto Dto;
        public bool RemoveOnLobbyLeave { get; set; }

        private readonly ITDSPlayer _target;
        private readonly ITDSPlayer? _sender;
        private readonly Action<ITDSPlayer, ITDSPlayer?, Invitation>? _onAccept;
        private readonly Action<ITDSPlayer, ITDSPlayer?, Invitation>? _onReject;
        private InvitationType _type;

        private static ulong _idCounter = 0;
        private static Dictionary<ulong, Invitation> _invitationById = new Dictionary<ulong, Invitation>();

        private readonly Serializer _serializer;

        public Invitation(string message, ITDSPlayer target, ITDSPlayer? sender,
            Serializer serializer,
            Action<ITDSPlayer, ITDSPlayer?, Invitation>? onAccept = null,
            Action<ITDSPlayer, ITDSPlayer?, Invitation>? onReject = null,
            InvitationType type = InvitationType.None)
        {
            _serializer = serializer;

            Dto = new InvitationDto
            {
                Id = _idCounter++,
                Sender = sender?.DisplayName,
                Message = message
            };

            _sender = sender;
            _target = target;
            _onAccept = onAccept;
            _onReject = onReject;
            _type = type;

            _invitationById[Dto.Id] = this;

            target.SendBrowserEvent(ToBrowserEvent.AddInvitation, serializer.ToBrowser(Dto));

        }

        public void Accept()
        {
            if (!_target.LoggedIn)
                return;
            _onAccept?.Invoke(_target, _sender, this);
        }

        public void Reject()
        {
            if (!_target.LoggedIn)
                return;
            _onReject?.Invoke(_target, _sender, this);
        }

        public void Withdraw()
        {
            _invitationById.Remove(Dto.Id);
            if (!_target.LoggedIn)
                return;
            _target.SendBrowserEvent(ToBrowserEvent.RemoveInvitation, Dto.Id);
        }

        public void Resend()
        {
            _target.SendBrowserEvent(ToBrowserEvent.AddInvitation, _serializer.ToBrowser(Dto));
        }

        public static Invitation? GetById(ulong id)
        {
            if (!_invitationById.TryGetValue(id, out Invitation? invitation))
                return null;
            return invitation;
        }

        public static IEnumerable<Invitation> GetBySender(TDSPlayer sender)
        {
            return _invitationById.Values.Where(i => i._sender == sender);
        }

        public static IEnumerable<Invitation> GetBySender(TDSPlayer sender, InvitationType type)
        {
            return _invitationById.Values.Where(i => i._sender == sender && i._type == type);
        }

        public static IEnumerable<Invitation> GetByTarget(TDSPlayer target)
        {
            return _invitationById.Values.Where(i => i._target == target);
        }
    }
}
