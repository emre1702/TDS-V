using AltV.Net.Elements.Entities;
using System;
using System.Collections.Generic;

namespace TDS_Server.Data.Interfaces.Entities
{
    public interface ITDSVehicle : IVehicle
    {
        List<ITDSPlayer> Occupants { get; }
        byte Seats { get; }
        bool HasFreeSeat { get; }
    }
}
