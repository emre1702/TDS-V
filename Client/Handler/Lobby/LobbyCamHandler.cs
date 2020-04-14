using System;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Models;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Handler.Lobby
{
    public class LobbyCamHandler : ServiceBase
    {
        private TDSTimer _timer;

        private readonly CamerasHandler _camerasHandler;
        private readonly SettingsHandler _settingsHandler;

        public LobbyCamHandler(IModAPI modAPI, LoggingHandler loggingHandler, CamerasHandler camerasHandler, SettingsHandler settingsHandler, EventsHandler eventsHandler)
            : base(modAPI, loggingHandler)
        {
            _camerasHandler = camerasHandler;
            _settingsHandler = settingsHandler;

            eventsHandler.LobbyLeft += EventsHandler_LobbyLeft;
            eventsHandler.CountdownStarted += Stop;
            eventsHandler.RoundStarted += _ => StopCountdown();
            eventsHandler.RoundEnded += StopCountdown;
        }

        public void SetToMapCenter(Position3D mapcenter)
        {
            try
            {
                Logging.LogInfo("", "LobbyCamHandler.SetToMapCenter");
                var cam = _camerasHandler.BetweenRoundsCam;
                cam.PointCamAtCoord(mapcenter);
                cam.Activate();
                var mapCenterHighPos = mapcenter.AddToZ(110);
                cam.RenderToPosition(mapCenterHighPos);
                Logging.LogInfo("", "LobbyCamHandler.SetToMapCenter", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        public void SetGoTowardsPlayer(int? time = null)
        {
            Logging.LogInfo("", "LobbyCamHandler.SetGoTowardsPlayer");
            _camerasHandler.RenderBack(true, (int)(time ?? (_settingsHandler.CountdownTime * 0.9)));
            Logging.LogInfo("", "LobbyCamHandler.SetGoTowardsPlayer", true);
        }

        public void SetTimerTowardsPlayer(uint execafterms)
        {
            Logging.LogInfo("", "LobbyCamHandler.SetTimerTowardsPlayer");
            _timer = new TDSTimer(() => SetGoTowardsPlayer(), execafterms, 1);
            Logging.LogInfo("", "LobbyCamHandler.SetTimerTowardsPlayer", true);
        }

        public void StopCountdown()
        {
            Logging.LogInfo("", "LobbyCamHandler.StopCountdown");
            _camerasHandler.RenderBack(false, 0);
            Logging.LogInfo("", "LobbyCamHandler.StopCountdown", true);
        }

        public void Stop()
        {
            _timer?.Kill();
            _timer = null;
        }

        private void EventsHandler_LobbyLeft(SyncedLobbySettings settings)
        {
            Stop();
        }
    }
}
