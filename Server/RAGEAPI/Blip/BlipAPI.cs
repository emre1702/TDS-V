using TDS_Server.Data.Interfaces.ModAPI.Blip;
using TDS_Server.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.RAGEAPI.Blip
{
    internal class BlipAPI : IBlipAPI
    {
        #region Public Methods

        public IBlip Create(uint sprite, Position position, float scale = 1f, byte color = 0, string name = "", byte alpha = 255, float drawDistance = 0, bool shortRange = false, short rotation = 0, uint dimension = uint.MaxValue)
        {
            var instance = GTANetworkAPI.NAPI.Blip.CreateBlip(sprite, position.ToMod(), scale, color, name, alpha, drawDistance, shortRange, rotation, dimension);
            return new Blip(instance);
        }

        #endregion Public Methods
    }
}
