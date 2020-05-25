using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI.Cam;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Cam
{
    internal class CamAPI : ICamAPI
    {
        #region Public Methods

        public ICam Create()
        {
            var cam = RAGE.Game.Cam.CreateCam("DEFAULT_SCRIPTED_CAMERA", false);
            return new Cam(cam);
        }

        public void DestroyAllCams()
        {
            RAGE.Game.Cam.DestroyAllCams(false);
        }

        public void DoScreenFadeIn(int duration)
        {
            RAGE.Game.Cam.DoScreenFadeIn(duration);
        }

        public void DoScreenFadeOut(int duration)
        {
            RAGE.Game.Cam.DoScreenFadeOut(duration);
        }

        public Position3D GetGameplayCamCoord()
        {
            return RAGE.Game.Cam.GetGameplayCamCoord().ToPosition3D();
        }

        public float GetGameplayCamFov()
        {
            return RAGE.Game.Cam.GetGameplayCamFov();
        }

        public Position3D GetGameplayCamRot()
        {
            return RAGE.Game.Cam.GetGameplayCamRot(2).ToPosition3D();
        }

        public void Render(bool render, bool ease, int easeTime)
        {
            RAGE.Game.Cam.RenderScriptCams(render, ease, easeTime, true, false, 0);
        }

        public void SetCamEffect(CamEffect effect)
        {
            RAGE.Game.Cam.SetCamEffect((int)effect);
        }

        public void SetCamFov(int handle, float fov)
        {
            RAGE.Game.Cam.SetCamFov(handle, fov);
        }

        public void ShakeGameplayCam(string shakeName, float intensity)
        {
            RAGE.Game.Cam.ShakeGameplayCam(shakeName, intensity);
        }

        public void StopGameplayCamShaking()
        {
            RAGE.Game.Cam.StopGameplayCamShaking(true);
        }

        #endregion Public Methods
    }
}
