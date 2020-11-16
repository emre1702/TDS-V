using RAGE;
using System;
using TDS.Client.Data.Extensions;
using TDS.Client.Handler.Events;
using TDS.Shared.Core;
using TDS.Shared.Data.Models;
using TDS.Shared.Data.Models.GTA;

namespace TDS.Client.Handler.Lobby
{
    public class LobbyCamHandler : ServiceBase
    {
        private readonly CamerasHandler _camerasHandler;
        private readonly SettingsHandler _settingsHandler;
        private TDSTimer _timer;

        public LobbyCamHandler(LoggingHandler loggingHandler, CamerasHandler camerasHandler, SettingsHandler settingsHandler, EventsHandler eventsHandler)
            : base(loggingHandler)
        {
            _camerasHandler = camerasHandler;
            _settingsHandler = settingsHandler;

            eventsHandler.LobbyLeft += EventsHandler_LobbyLeft;
            eventsHandler.CountdownStarted += _ => Stop();
            eventsHandler.RoundStarted += _ => StopCountdown();
            eventsHandler.RoundEnded += _ => StopCountdown();
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

        public void SetToMapCenter(Vector3 mapCenter)
        {
            try
            {
                Logging.LogInfo("", "LobbyCamHandler.SetToMapCenter");
                var cam = _camerasHandler.BetweenRoundsCam;
                cam.PointCamAtCoord(mapCenter);
                cam.Activate();
                var mapCenterHighPos = mapCenter.AddToZ(110);
                cam.RenderToPosition(mapCenterHighPos);
                Logging.LogInfo("", "LobbyCamHandler.SetToMapCenter", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        public void Stop()
        {
            _timer?.Kill();
            _timer = null;
        }

        public void StopCountdown()
        {
            Logging.LogInfo("", "LobbyCamHandler.StopCountdown");
            _camerasHandler.RenderBack(false, 0);
            Logging.LogInfo("", "LobbyCamHandler.StopCountdown", true);
        }

        private void EventsHandler_LobbyLeft(SyncedLobbySettings settings)
        {
            Stop();
        }
    }
}
