using System;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Events;
using TDS_Shared.Default;

namespace TDS_Client.Handler.Lobby
{
    public class RoundHandler : ServiceBase
    {
        private readonly EventsHandler _eventsHandler;
        private readonly RoundInfosHandler _roundInfosHandler;
        private readonly SettingsHandler _settingsHandler;
        private readonly BrowserHandler _browserHandler;

        public RoundHandler(IModAPI modAPI, LoggingHandler loggingHandler, EventsHandler eventsHandler, RoundInfosHandler roundInfosHandler, 
            SettingsHandler settingsHandler, BrowserHandler browserHandler)
            : base(modAPI, loggingHandler)
        {
            _eventsHandler = eventsHandler;
            _roundInfosHandler = roundInfosHandler;
            _settingsHandler = settingsHandler;
            _browserHandler = browserHandler;

            modAPI.Event.Add(ToClientEvent.RoundStart, OnRoundStartMethod);
            modAPI.Event.Add(ToClientEvent.RoundEnd, OnRoundEndMethod);
        }

        private void OnRoundStartMethod(object[] args)
        {
            try
            {
                bool isSpectator = Convert.ToBoolean(args[0]);
                _eventsHandler.OnRoundStarted(isSpectator);

                ModAPI.Cam.DoScreenFadeIn(50);
                _roundInfosHandler.Start(args.Length >= 2 ? Convert.ToInt32(args[1]) : 0);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void OnRoundEndMethod(object[] args)
        {
            bool isSpectator = (bool)args[0];
            _eventsHandler.OnRoundEnded(isSpectator);

            ModAPI.Cam.DoScreenFadeOut(_settingsHandler.RoundEndTime / 2);

            string reason = (string)args[1];
            int mapId = (int)args[2];
            _browserHandler.PlainMain.ShowRoundEndReason(reason, mapId);
        }
    }
}
