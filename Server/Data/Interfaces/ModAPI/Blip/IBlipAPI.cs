using TDS_Server.Data.Models.Map.Creator;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Data.Interfaces.ModAPI.Blip
{
    public interface IBlipAPI
    {
        IBlip Create(Position3DDto position, ILobby lobby) => Create(new Position3D(position.X, position.Y, position.Z), lobby);
        IBlip Create(Position3D position, ILobby lobby);
    }
}
