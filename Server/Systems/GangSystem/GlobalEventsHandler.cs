﻿using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Handler.Events;

namespace TDS.Server.GangsSystem
{
    public class GlobalEventsHandler
    {
        private readonly EventsHandler _eventsHandler;

        public GlobalEventsHandler(EventsHandler eventsHandler)
        {
            _eventsHandler = eventsHandler;

            eventsHandler.PlayerLoggedOut += EventsHandler_PlayerLoggedOut;
        }

        private void EventsHandler_PlayerLoggedOut(ITDSPlayer player)
        {
            player.Gang.Players.RemoveOnline(player);
        }
    }
}
