using System;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.LobbySystem.Notifications;
using TDS.Server.Data.Models.Map;
using TDS.Server.Handler.Helper;

namespace TDS.Server.LobbySystem.Notifications
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
