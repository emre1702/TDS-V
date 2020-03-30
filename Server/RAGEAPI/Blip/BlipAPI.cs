using System;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Blip;
using TDS_Server.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.RAGEAPI.Blip
{
    class BlipAPI : IBlipAPI
    {
        public IBlip Create(Position3D position, uint dimension)
        {
            var instance = GTANetworkAPI.NAPI.Blip.CreateBlip(position.ToVector3());
            instance.Dimension = dimension;
            return new Blip(instance);
        }
    }
}
