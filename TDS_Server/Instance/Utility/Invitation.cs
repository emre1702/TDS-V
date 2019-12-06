using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Common.Default;
using TDS_Common.Manager.Utility;
using TDS_Server.Default;
using TDS_Server.Dto;
using TDS_Server.Enum;
using TDS_Server.Instance.Player;

namespace TDS_Server.Instance.Utility
{
    class Invitation
    {
        public InvitationDto Dto;

        private TDSPlayer _sender;
        private TDSPlayer _target;
        private Action<TDSPlayer, TDSPlayer>? _onAccept;
        private Action<TDSPlayer, TDSPlayer>? _onReject;
        private EInvitationType _type;

        private static ulong _idCounter = 0;
        private static Dictionary<ulong, Invitation> _invitationById = new Dictionary<ulong, Invitation>();


        public Invitation(string message, TDSPlayer sender, TDSPlayer target, 
            Action<TDSPlayer, TDSPlayer>? onAccept = null, 
            Action<TDSPlayer, TDSPlayer>? onReject = null, 
            EInvitationType type = EInvitationType.None)
        {
            Dto = new InvitationDto 
            {
                Id = _idCounter++,
                Sender = sender.DisplayName,
                Message = message
            };

            _sender = sender;
            _target = target;
            _onAccept = onAccept;
            _onReject = onReject;
            _type = type;

            _invitationById[Dto.Id] = this; 

            NAPI.ClientEvent.TriggerClientEvent(target.Client, DToClientEvent.ToBrowserEvent, DToBrowserEvent.AddInvitation, Serializer.ToBrowser(Dto));

        }

        public void Accept()
        {
            if (!_target.LoggedIn)
                return;
            _onAccept?.Invoke(_sender, _target);
        }

        public void Reject()
        {
            if (!_target.LoggedIn)
                return;
            _onReject?.Invoke(_sender, _target);
        }

        public void Withdraw()
        {
            _invitationById.Remove(Dto.Id);
            if (!_target.LoggedIn)
                return;
            NAPI.ClientEvent.TriggerClientEvent(_target.Client, DToClientEvent.ToBrowserEvent, DToBrowserEvent.RemoveInvitation, Dto.Id);
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
    }
}
