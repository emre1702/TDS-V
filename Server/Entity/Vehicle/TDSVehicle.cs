using AltV.Net.Data;
using System;
using TDS_Server.Data.Interfaces.Entities;

namespace TDS_Server.Entity.Vehicle
{
    public class TDSVehicle : AltV.Net.Elements.Entities.Vehicle, ITDSVehicle
    {

        public TDSVehicle(IntPtr nativePointer, ushort id) : base(nativePointer, id)
        {

        }

        public TDSVehicle(uint model, Position position, Rotation rotation): base(model, position, rotation) { }
    }
}
