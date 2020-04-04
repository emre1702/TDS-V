using System;
using System.Collections.Generic;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Models;

namespace TDS_Client.RAGEAPI.Event
{
    class EventAPI : IEventAPI
    {
        public ICollection<EventMethodData<Action>> Tick { get; }

        public EventAPI()
        {
            Tick = new TickEventHandler();
        }
    }
}
