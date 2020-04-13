using TDS_Client.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Cam
{
    public interface ICamAPI
    {
        ICam Create();
        void DestroyAllCams();
        Position3D GetGameplayCamRot(/* 0 */);
        Position3D GetGameplayCamCoord();
        void DoScreenFadeIn(int duration);
        void SetCamEffect(CamEffect camEffect);
        void DoScreenFadeOut(int duration);
        void Render(bool render, bool ease, int easeTime);
        float GetGameplayCamFov();
        /**
         * <summary>Min: 1.0f Max: 130.0f</summary>
         * */
        void SetCamFov(int handle, float fov);
        void ShakeGameplayCam(string shakeName, float intensity);
        void StopGameplayCamShaking();
    }
}
