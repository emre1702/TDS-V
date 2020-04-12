using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Misc
{
    public interface IMiscAPI
    {
        bool GetGroundZFor3dCoord(float x, float y, float v1, ref float edgeZ, bool v2);
        uint GetHashKey(string hash);
        void IgnoreNextRestart(bool v);
        void SetFadeOutAfterDeath(bool v);
        void SetWeatherTypeNowPersist(string v);
        void Wait(int v);
        void GetModelDimensions(IEntity model, Position3D a, Position3D b);
        void SetWind(int v);
        float GetDistanceBetweenCoords(Position3D playerpos, Position3D pos, bool v);
    }
}
