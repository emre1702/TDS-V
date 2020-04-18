using TDS_Server.Data.Models.Map.Creator;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Data.Interfaces.ModAPI.Blip
{
    public interface IBlipAPI
    {
        IBlip Create(uint sprite, Position3DDto position, float scale = 1f, byte color = 0, string name = "", byte alpha = 255, float drawDistance = 0, bool shortRange = false, short rotation = 0, uint dimension = uint.MaxValue)
            => Create(sprite, position, scale, color, name, alpha, drawDistance, shortRange, rotation, dimension);
        IBlip Create(uint sprite, Position3D position, float scale = 1f, byte color = 0, string name = "", byte alpha = 255, float drawDistance = 0, bool shortRange = false, short rotation = 0, uint dimension = uint.MaxValue);
    }
}
