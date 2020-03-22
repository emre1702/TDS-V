using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models;
using TDS_Server.Handler.Entities.Player;
using TDS_Shared.Manager.Utility;

namespace TDS_Server.Handler.Entities.Utility
{
    public class Invitation
    {
        public InvitationDto Dto;
        public bool RemoveOnLobbyLeave { get; set; }

        public readonly ITDSPlayer Target;
        public readonly ITDSPlayer? Sender;
        private readonly Action<ITDSPlayer, ITDSPlayer?, Invitation>? _onAccept;
        private readonly Action<ITDSPlayer, ITDSPlayer?, Invitation>? _onReject;
        public InvitationType Type;

        private static ulong _idCounter = 0;

        private readonly Serializer _serializer;
        private readonly InvitationsHandler _invitationsHandler;

        public Invitation(string message, ITDSPlayer target, ITDSPlayer? sender,
            Serializer serializer, InvitationsHandler invitationsHandler,
            Action<ITDSPlayer, ITDSPlayer?, Invitation>? onAccept = null,
            Action<ITDSPlayer, ITDSPlayer?, Invitation>? onReject = null,
            InvitationType type = InvitationType.None)
        {
            _serializer = serializer;
            _invitationsHandler = invitationsHandler;

            Dto = new InvitationDto
            {
                Id = _idCounter++,
                Sender = sender?.DisplayName,
                Message = message
            };

            Sender = sender;
            Target = target;
            _onAccept = onAccept;
            _onReject = onReject;
            Type = type;

            invitationsHandler.Add(this);

            target.SendBrowserEvent(ToBrowserEvent.AddInvitation, serializer.ToBrowser(Dto));

        }

        public void Accept()
        {
            if (!Target.LoggedIn)
                return;
            _onAccept?.Invoke(Target, Sender, this);
        }

        public void Reject()
        {
            if (!Target.LoggedIn)
                return;
            _onReject?.Invoke(Target, Sender, this);
        }

        public void Withdraw()
        {
            _invitationsHandler.Remove(this);
            if (!Target.LoggedIn)
                return;
            Target.SendBrowserEvent(ToBrowserEvent.RemoveInvitation, Dto.Id);
        }

        public void Resend()
        {
            Target.SendBrowserEvent(ToBrowserEvent.AddInvitation, _serializer.ToBrowser(Dto));
        }
    }
}
