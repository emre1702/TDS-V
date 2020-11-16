using System;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Handler.Helper;

namespace TDS.Server.LobbySystem.Notifications
{
    public class GangLobbyNotifications : BaseLobbyNotifications
    {
        protected new IGangLobby Lobby => (IGangLobby)base.Lobby;

        public GangLobbyNotifications(IGangLobby lobby, LangHelper langHelper) : base(lobby, langHelper)
        { }

        public override void Send(Func<ILanguage, string> langGetter, bool flashing = false)
        {
            base.Send(langGetter, flashing);
            Lobby.Notifications.Send(langGetter, flashing);
        }
    }
}
