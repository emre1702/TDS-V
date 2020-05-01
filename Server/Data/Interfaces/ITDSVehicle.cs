using System;
using TDS_Server.Data.Interfaces.ModAPI.Vehicle;

namespace TDS_Server.Data.Interfaces
{
    #nullable enable
    public interface ITDSVehicle : ITDSEntity, IEquatable<ITDSVehicle>
    {
        IVehicle? Vehicle { get; }

        void Delete();
    }
}
