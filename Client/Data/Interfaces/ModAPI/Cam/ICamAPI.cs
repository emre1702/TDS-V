using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Cam
{
    public interface ICamAPI
    {
        ICam Create();
        void DestroyAllCams();
        void RenderScriptCams(bool v1, bool v2, int v3, bool v4, bool v5, int v6);
        Position3D GetGameplayCamRot(/* 0 */);
        Position3D GetGameplayCamCoord();
    }
}
