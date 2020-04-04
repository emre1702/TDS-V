using TDS_Client.Data.Interfaces.ModAPI.Cam;

namespace TDS_Client.RAGEAPI.Cam
{
    class CamAPI : ICamAPI
    {
        public ICam Create()
        {
            var cam = RAGE.Game.Cam.CreateCam("DEFAULT_SCRIPTED_CAMERA", false);
            return new Cam.Cam(cam);
        }
    }
}
