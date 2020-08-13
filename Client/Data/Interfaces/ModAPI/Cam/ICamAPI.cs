using TDS_Client.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Cam
{
    public interface ICamAPI
    {
        #region Public Methods

        ICam Create();

        void DestroyAllCams();

        void DoScreenFadeIn(int duration);

        void DoScreenFadeOut(int duration);

        Position GetGameplayCamCoord();

        float GetGameplayCamFov();

        Position GetGameplayCamRot(/* 0 */);

        void Render(bool render, bool ease, int easeTime);

        void SetCamEffect(CamEffect camEffect);

        /**
         * <summary>Min: 1.0f Max: 130.0f</summary>
         * */

        void SetCamFov(int handle, float fov);

        void ShakeGameplayCam(string shakeName, float intensity);

        void StopGameplayCamShaking();

        #endregion Public Methods
    }
}
