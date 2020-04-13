using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Blip
{
    public interface IBlipAPI
    {
        IBlip Create(uint sprite, Position3D position, string name = "", float scale = 1, int color = 0, int alpha = 255, float drawDistance = 0, bool shortRange = false, 
            int rotation = 0, float radius = 0, uint dimension = 0);
    }
}
