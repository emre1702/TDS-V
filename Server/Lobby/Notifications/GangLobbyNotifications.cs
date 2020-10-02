using System;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Handler.Helper;

namespace TDS_Server.LobbySystem.Notifications
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
