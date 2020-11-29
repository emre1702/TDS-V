using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.GangsSystem;
using TDS.Server.Handler;
using TDS.Server.Handler.Entities.Utility;
using TDS.Server.Handler.Helper;

namespace TDS.Server.GangsSystem
{
    internal class Invitations : IGangInvitations
    {
        private readonly InvitationsHandler _invitationsHandler;
        private readonly LangHelper _langHelper;
        private readonly IGang _gang;

        public Invitations(InvitationsHandler invitationsHandler, LangHelper langHelper, IGang gang)
            => (_invitationsHandler, _langHelper, _gang) = (invitationsHandler, langHelper, gang);

        public List<IInvitation> Send(
            Func<ILanguage, string> langGetter,
            ITDSPlayer? sender,
            Action<ITDSPlayer, ITDSPlayer?, IInvitation>? onAccept = null,
            Action<ITDSPlayer, ITDSPlayer?, IInvitation>? onReject = null,
            InvitationType type = InvitationType.None,
            bool removeOnLobbyLeave = true)
        {
            var langDictionary = _langHelper.GetLangDictionary(langGetter);
            var invitationsList = new List<IInvitation>();
            _gang.Players.DoForAll(player =>
            {
                var invitation = new Invitation(langDictionary[player.Language], player, sender, _invitationsHandler, onAccept, onReject, type) 
                {  
                    RemoveOnLobbyLeave = removeOnLobbyLeave 
                };
                invitationsList.Add(invitation);
            });
            return invitationsList;
        }
    }
}
