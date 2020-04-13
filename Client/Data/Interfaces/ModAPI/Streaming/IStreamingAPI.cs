using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Streaming
{
    public interface IStreamingAPI
    {
        void SetFocusEntity(IEntityBase entity);
        void SetFocusArea(Position3D pos, int offsetX, int offsetY, int offsetZ);
        bool IsModelInCdimage(uint model);
        bool IsModelValid(uint model);
        void RequestModel(uint model);
        bool HasModelLoaded(uint model);
        void RequestAnimDict(string animDict);
    }
}
