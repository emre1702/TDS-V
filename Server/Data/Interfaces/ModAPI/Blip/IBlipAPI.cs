using TDS_Server.Data.Models.Map.Creator;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Data.Interfaces.ModAPI.Blip
{
    public interface IBlipAPI
    {
        IBlip Create(Position3DDto position, uint dimension) => Create(new Position3D(position.X, position.Y, position.Z), dimension);
        IBlip Create(Position3D position, uint dimension);
    }
}
