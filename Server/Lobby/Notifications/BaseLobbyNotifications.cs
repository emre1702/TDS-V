using System;
using System.Collections.Generic;
using TDS_Server.Data.Interfaces;
using TDS_Server.Handler.Helper;
using TDS_Server.LobbySystem.Players;

namespace TDS_Server.LobbySystem.Notifications
{
    public class BaseLobbyNotifications
    {
        private readonly BaseLobbyPlayers _players;
        private readonly LangHelper _langHelper;

        public BaseLobbyNotifications(BaseLobbyPlayers players, LangHelper langHelper)
            => (_players, _langHelper) = (players, langHelper);

        public void Send(Func<ILanguage, string> langGetter, ITeam? targetTeam = null, bool flashing = false)
        {
            Dictionary<ILanguage, string> texts = _langHelper.GetLangDictionary(langGetter);
            if (targetTeam is null)
            {
                _players.Do(player =>
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
