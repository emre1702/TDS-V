using System;
using System.Collections.Generic;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Handler.Helper;
using TDS_Server.LobbySystem.Players;

namespace TDS_Server.LobbySystem.Notifications
{
    public class BaseLobbyNotifications
    {
        private readonly Action<Action<ITDSPlayer>> _doForPlayersActionProvider;
        private readonly LangHelper _langHelper;

        public BaseLobbyNotifications(Action<Action<ITDSPlayer>> doForPlayersActionProvider, LangHelper langHelper)
            => (_doForPlayersActionProvider, _langHelper) = (doForPlayersActionProvider, langHelper);

        public void Send(Func<ILanguage, string> langGetter, ITeam? targetTeam = null, bool flashing = false)
        {
            Dictionary<ILanguage, string> texts = _langHelper.GetLangDictionary(langGetter);
            if (targetTeam is null)
            {
                _doForPlayersActionProvider(player =>
                {
                    player.SendNotification(texts[player.Language], flashing);
                });
            }
            else
            {
                targetTeam.FuncIterate(player =>
                {
                    player.SendNotification(texts[player.Language], flashing);
                });
            }
        }
    }
}
