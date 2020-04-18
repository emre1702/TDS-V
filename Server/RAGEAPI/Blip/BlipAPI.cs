using System;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Blip;
using TDS_Server.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.RAGEAPI.Blip
{
    class BlipAPI : IBlipAPI
    {
        public IBlip Create(uint sprite, Position3D position, float scale = 1f, byte color = 0, string name = "", byte alpha = 255, float drawDistance = 0, bool shortRange = false, short rotation = 0, uint dimension = uint.MaxValue)
        {
            var instance = GTANetworkAPI.NAPI.Blip.CreateBlip(sprite, position.ToVector3(), scale, color, name, alpha, drawDistance, shortRange, rotation, dimension);
            return new Blip(instance);
        }
    }
}
