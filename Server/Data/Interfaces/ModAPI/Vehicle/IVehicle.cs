using System;
using System.Collections.Generic;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Data.Interfaces.ModAPI.Vehicle
{
    public interface IVehicle : IEntity, IEquatable<IVehicle>
    {
        ushort Id { get; }
        List<ITDSEntity> Occupants { get; }
        int MaxOccupants { get; }
        Position3D Rotation { get; set; }
        void Delete();
    }
}
