using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Blip
{
    public interface IBlipAPI
    {
        IBlip Create(int teamSpawnBlipSprite, Position3D position, string name, int dimension);
    }
}
