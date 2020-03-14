using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Common.Default;
using TDS_Common.Manager.Utility;
using TDS_Server.Default;
using TDS_Server.Dto;
using TDS_Server.Enums;
using TDS_Server.Instance.PlayerInstance;

namespace TDS_Server.Handler.Entities.Utility
{
    class Invitation
    {
        public InvitationDto Dto;
        public bool RemoveOnLobbyLeave { get; set; }

        private TDSPlayer _target;
        private TDSPlayer? _sender;
        private Action<TDSPlayer, TDSPlayer?, Invitation>? _onAccept;
        private Action<TDSPlayer, TDSPlayer?, Invitation>? _onReject;
        private EInvitationType _type;

        private static ulong _idCounter = 0;
        private static Dictionary<ulong, Invitation> _invitationById = new Dictionary<ulong, Invitation>();


        public Invitation(string message, TDSPlayer target, TDSPlayer? sender, 
            Action<TDSPlayer, TDSPlayer?, Invitation>? onAccept = null, 
            Action<TDSPlayer, TDSPlayer?, Invitation>? onReject = null, 
            EInvitationType type = EInvitationType.None)
        {
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

            NAPI.ClientEvent.TriggerClientEvent(target.Player, DToClientEvent.ToBrowserEvent, ToBrowserEvent.AddInvitation, Serializer.ToBrowser(Dto));

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
            NAPI.ClientEvent.TriggerClientEvent(_target.Player, DToClientEvent.ToBrowserEvent, ToBrowserEvent.RemoveInvitation, Dto.Id);
        }

        public void Resend()
        {
            NAPI.ClientEvent.TriggerClientEvent(_target.Player, DToClientEvent.ToBrowserEvent, ToBrowserEvent.AddInvitation, Serializer.ToBrowser(Dto));
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

        public static IEnumerable<Invitation> GetBySender(TDSPlayer sender, EInvitationType type)
        {
            return _invitationById.Values.Where(i => i._sender == sender && i._type == type);
        }

        public static IEnumerable<Invitation> GetByTarget(TDSPlayer target)
        {
            return _invitationById.Values.Where(i => i._target == target);
        }
    }
}
