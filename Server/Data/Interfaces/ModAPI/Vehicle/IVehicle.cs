using System;
using System.Collections.Generic;

namespace TDS_Server.Data.Interfaces.ModAPI.Vehicle
{
    public interface IVehicle : IEntity, IEquatable<IVehicle>
    {
        ushort Id { get; }
        List<ITDSEntity> Occupants { get; }
        int MaxOccupants { get; }

        void Delete();
    }
}
