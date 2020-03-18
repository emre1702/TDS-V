using TDS_Server.Data.Models.Map.Creator;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Data.Interfaces.ModAPI.MapObject
{
#nullable enable
    public interface IMapObjectAPI
    {
        IMapObject Create(int hashNumber, Position3D position, Position3D? rotation, byte alpha, ILobby lobby);
        IMapObject Create(string hashName, Position3DDto position, Position3D? rotation, byte alpha, ILobby lobby);
    }
}
