using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Misc
{
    public interface IMiscAPI
    {
        bool GetGroundZFor3dCoord(float x, float y, float v1, ref float edgeZ, bool v2);
        uint GetHashKey(string hash);
        void IgnoreNextRestart(bool v);
        void SetFadeOutAfterDeath(bool v);
        void Wait(int v);
        void GetModelDimensions(object model, Position3D a, Position3D b);
    }
}
