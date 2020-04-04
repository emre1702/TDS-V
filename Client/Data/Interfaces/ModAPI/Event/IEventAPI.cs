using System;
using System.Collections.Generic;
using TDS_Client.Data.Models;

namespace TDS_Client.Data.Interfaces.ModAPI.Event
{
    public delegate void TickDelegate();

    public interface IEventAPI
    {
        ICollection<EventMethodData<Action>> Tick { get; }
    }
}
