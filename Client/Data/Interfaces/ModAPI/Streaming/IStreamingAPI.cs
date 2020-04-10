using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Streaming
{
    public interface IStreamingAPI
    {
        void SetFocusEntity(IEntity entity);
        void SetFocusArea(Position3D pos, int v1, int v2, int v3);
        bool IsModelInCdimage(uint hash);
        bool IsModelValid(uint hash);
        void RequestModel(uint hash);
        bool HasModelLoaded(uint hash);
    }
}
