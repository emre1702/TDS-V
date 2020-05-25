using TDS_Server.Data.Models.Map.Creator;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Data.Interfaces.ModAPI.Blip
{
    public interface IBlipAPI
    {
        #region Public Methods

        IBlip Create(uint sprite, Position3DDto position, float scale = 1f, byte color = 1, string name = "", byte alpha = 255, float drawDistance = 0, bool shortRange = false, short rotation = 0, uint dimension = uint.MaxValue)
            => Create(sprite, new Position3D(position.X, position.Y, position.Z), scale, color, name, alpha, drawDistance, shortRange, rotation, dimension);

        IBlip Create(uint sprite, Position3D position, float scale = 1f, byte color = 1, string name = "", byte alpha = 255, float drawDistance = 0, bool shortRange = false, short rotation = 0, uint dimension = uint.MaxValue);

        #endregion Public Methods
    }
}
