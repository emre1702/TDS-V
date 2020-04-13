using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Misc
{
    public interface IMiscAPI
    {
        bool GetGroundZFor3dCoord(float x, float y, float z, ref float groundZ);
        uint GetHashKey(string hash);
        void IgnoreNextRestart(bool toggle);
        void SetFadeOutAfterDeath(bool toggle);
        void SetWeatherTypeNowPersist(string weatherType);
        void GetModelDimensions(uint model, Position3D a, Position3D b);
        void SetWind(float speed);
        float GetDistanceBetweenCoords(Position3D pos1, Position3D pos2, bool useZ);
        int GetGameTimer();
    }
}
