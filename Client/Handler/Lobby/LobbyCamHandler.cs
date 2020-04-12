using TDS_Client.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Models;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Handler.Lobby
{
    public class LobbyCamHandler
    {
        private TDSTimer _timer;

        private readonly CamerasHandler _camerasHandler;
        private readonly SettingsHandler _settingsHandler;

        public LobbyCamHandler(CamerasHandler camerasHandler, SettingsHandler settingsHandler, EventsHandler eventsHandler)
        {
            _camerasHandler = camerasHandler;
            _settingsHandler = settingsHandler;

            eventsHandler.LobbyLeft += EventsHandler_LobbyLeft;
        }

        public void SetToMapCenter(Position3D mapcenter)
        {
            var cam = _camerasHandler.BetweenRoundsCam;
            cam.PointCamAtCoord(mapcenter);
            cam.Activate();
            cam.RenderToPosition(mapcenter.AddToZ(110));
        }

        public void SetGoTowardsPlayer(int? time = null)
        {
            _camerasHandler.RenderBack(true, (int)(time ?? (_settingsHandler.CountdownTime * 0.9)));
        }

        public void SetTimerTowardsPlayer(uint execafterms)
        {
            _timer = new TDSTimer(() => SetGoTowardsPlayer(), execafterms, 1);
        }

        public void StopCountdown()
        {
            _camerasHandler.RenderBack(false, 0);
        }

        public void Stop()
        {
            _timer?.Kill();
            _timer = null;
        }

        private void EventsHandler_LobbyLeft(SyncedLobbySettingsDto settings)
        {
            Stop();
        }
    }
}
