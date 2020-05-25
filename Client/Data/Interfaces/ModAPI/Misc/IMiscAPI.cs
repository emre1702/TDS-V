using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Misc
{
    public interface IMiscAPI
    {
        #region Public Methods

        float GetDistanceBetweenCoords(Position3D pos1, Position3D pos2, bool useZ);

        int GetGameTimer();

        bool GetGroundZFor3dCoord(float x, float y, float z, ref float groundZ);

        uint GetHashKey(string hash);

        void GetModelDimensions(uint model, Position3D a, Position3D b);

        void IgnoreNextRestart(bool toggle);

        void SetFadeOutAfterDeath(bool toggle);

        void SetWeatherTypeNowPersist(string weatherType);

        void SetWind(float speed);

        #endregion Public Methods
    }
}
