using System;
using System.Collections.Generic;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.LobbySystem.Notifications;
using TDS.Server.Handler.Helper;

namespace TDS.Server.LobbySystem.Notifications
{
    public class BaseLobbyNotifications : IBaseLobbyNotifications
    {
        protected IBaseLobby Lobby { get; }
        private readonly LangHelper _langHelper;

        public BaseLobbyNotifications(IBaseLobby lobby, LangHelper langHelper)
            => (Lobby, _langHelper) = (lobby, langHelper);

        public virtual void Send(Func<ILanguage, string> langGetter, bool flashing = false)
        {
            Dictionary<ILanguage, string> texts = _langHelper.GetLangDictionary(langGetter);
            Lobby.Players.DoInMain(player =>
            {
                player.SendNotification(texts[player.Language], flashing);
            });
        }
    }
}
