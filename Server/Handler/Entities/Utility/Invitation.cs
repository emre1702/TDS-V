using GTANetworkAPI;
using System;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Defaults;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Models;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Core;

namespace TDS.Server.Handler.Entities.Utility
{
    public class Invitation : IInvitation
    {
        public ITDSPlayer? Sender { get; set; }
        public ITDSPlayer Target { get; set; }
        public InvitationDto Dto { get; set; }
        public InvitationType Type { get; set; }
        public bool Withdrawn { get; private set; }
        public bool Reacted { get; private set; }

        private static ulong _idCounter = 0;
        private readonly InvitationsHandler _invitationsHandler;
        private readonly Action<ITDSPlayer, ITDSPlayer?, Invitation>? _onAccept;
        private readonly Action<ITDSPlayer, ITDSPlayer?, Invitation>? _onReject;

        public Invitation(string message, ITDSPlayer target, ITDSPlayer? sender,
            InvitationsHandler invitationsHandler,
            Action<ITDSPlayer, ITDSPlayer?, Invitation>? onAccept = null,
            Action<ITDSPlayer, ITDSPlayer?, Invitation>? onReject = null,
            InvitationType type = InvitationType.None)
        {
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

            Show();
        }

        public bool RemoveOnLobbyLeave { get; set; }

        public void Accept()
        {
            if (Reacted)
                return;
            if (!Target.LoggedIn)
                return;
            Reacted = true;
            _onAccept?.Invoke(Target, Sender, this);
        }

        public void Reject()
        {
            if (Reacted)
                return;
            if (!Target.LoggedIn)
                return;
            Reacted = true;
            _onReject?.Invoke(Target, Sender, this);
        }

        public void Show()
        {
            Reacted = false;
            var dataJson = Serializer.ToBrowser(Dto);
            NAPI.Task.RunSafe(() =>
                Target.TriggerBrowserEvent(ToBrowserEvent.AddInvitation, dataJson));
        }

        public void Withdraw()
        {
            if (Withdrawn)
                return;
            Withdrawn = true;
            _invitationsHandler.Remove(this);
            if (!Target.LoggedIn)
                return;
            NAPI.Task.RunSafe(() =>
                Target.TriggerBrowserEvent(ToBrowserEvent.RemoveInvitation, Dto.Id));
        }
    }
}
