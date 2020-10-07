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
        private readonly IRoundFightLobbyEventsHandler _events;

        public RoundFightLobbyNotifications(IRoundFightLobby lobby, IRoundFightLobbyEventsHandler events, LangHelper langHelper)
            : base(lobby, langHelper)
        {
            _events = events;

            events.InitNewMap += Events_InitNewMap;
            events.RemoveAfter += RemoveEvents;
        }

        private void RemoveEvents(IBaseLobby lobby)
        {
            _events.InitNewMap -= Events_InitNewMap;
            _events.RemoveAfter -= RemoveEvents;
        }

        private void Events_InitNewMap(MapDto map)
        {
            if (map.Info.IsNewMap)
                Send(lang => lang.TESTING_MAP_NOTIFICATION, flashing: true);
        }
    }
}
