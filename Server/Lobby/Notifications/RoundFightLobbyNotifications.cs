using System;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.Notifications;
using TDS_Server.Data.Models.Map;
using TDS_Server.Handler.Helper;

namespace TDS_Server.LobbySystem.Notifications
{
    public class RoundFightLobbyNotifications : BaseLobbyNotifications, IRoundFightLobbyNotifications
    {
        public RoundFightLobbyNotifications(IRoundFightLobby lobby, IRoundFightLobbyEventsHandler events, LangHelper langHelper)
            : base(lobby, langHelper)
        {
            events.InitNewMap += Events_InitNewMap;
        }

        private void Events_InitNewMap(MapDto map)
        {
            if (map.Info.IsNewMap)
                Send(lang => lang.TESTING_MAP_NOTIFICATION, flashing: true);
        }
    }
}
