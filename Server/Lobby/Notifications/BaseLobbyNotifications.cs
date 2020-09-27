using System;
using System.Collections.Generic;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.Notifications;
using TDS_Server.Handler.Helper;

namespace TDS_Server.LobbySystem.Notifications
{
    public class BaseLobbyNotifications : IBaseLobbyNotifications
    {
        private readonly IBaseLobby _lobby;
        private readonly LangHelper _langHelper;

        public BaseLobbyNotifications(IBaseLobby lobby, LangHelper langHelper)
            => (_lobby, _langHelper) = (lobby, langHelper);

        public void Send(Func<ILanguage, string> langGetter, ITeam? targetTeam = null, bool flashing = false)
        {
            Dictionary<ILanguage, string> texts = _langHelper.GetLangDictionary(langGetter);
            if (targetTeam is null)
            {
                _lobby.Players.Do(player =>
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
