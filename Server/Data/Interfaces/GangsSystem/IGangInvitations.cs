using System;
using System.Collections.Generic;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Enums;

namespace TDS.Server.Data.Interfaces.GangsSystem
{
#nullable enable
    public interface IGangInvitations
    {
        List<IInvitation> Send(
            Func<ILanguage, string> langGetter,
            ITDSPlayer? sender,
            Action<ITDSPlayer, ITDSPlayer?, IInvitation>? onAccept = null,
            Action<ITDSPlayer, ITDSPlayer?, IInvitation>? onReject = null,
            InvitationType type = InvitationType.None,
            bool removeOnLobbyLeave = true);
    }
}