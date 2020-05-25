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
        #region Private Fields

        private readonly CamerasHandler _camerasHandler;
        private readonly SettingsHandler _settingsHandler;
        private TDSTimer _timer;

        #endregion Private Fields

        #region Public Constructors

        public LobbyCamHandler(IModAPI modAPI, LoggingHandler loggingHandler, CamerasHandler camerasHandler, SettingsHandler settingsHandler, EventsHandler eventsHandler)
            : base(modAPI, loggingHandler)
        {
            _camerasHandler = camerasHandler;
            _settingsHandler = settingsHandler;

            eventsHandler.LobbyLeft += EventsHandler_LobbyLeft;
            eventsHandler.CountdownStarted += _ => Stop();
            eventsHandler.RoundStarted += _ => StopCountdown();
            eventsHandler.RoundEnded += _ => StopCountdown();
        }

        #endregion Public Constructors

        #region Public Methods

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

        #endregion Public Methods

        #region Private Methods

        private void EventsHandler_LobbyLeft(SyncedLobbySettings settings)
        {
            Stop();
        }

        #endregion Private Methods
    }
}
