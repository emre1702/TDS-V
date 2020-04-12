using System;
using System.Collections.Generic;
using System.Text;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Vehicle
{
    public interface IVehicleAPI
    {
        IVehicle Create(uint hash, Position3D position, Position3D rotation, string v = "TDS-V", bool locked = false, int dimension = 0);
        void SetVehicleOnGroundProperly(int handle, int v);
    }
}
