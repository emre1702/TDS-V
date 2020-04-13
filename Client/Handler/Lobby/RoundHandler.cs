using System;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Events;
using TDS_Shared.Default;

namespace TDS_Client.Handler.Lobby
{
    public class RoundHandler
    {
        private readonly IModAPI _modAPI;
        private readonly EventsHandler _eventsHandler;
        private readonly RoundInfosHandler _roundInfosHandler;
        private readonly SettingsHandler _settingsHandler;
        private readonly BrowserHandler _browserHandler;

        public RoundHandler(IModAPI modAPI, EventsHandler eventsHandler, RoundInfosHandler roundInfosHandler, SettingsHandler settingsHandler, BrowserHandler browserHandler)
        {
            _modAPI = modAPI;
            _eventsHandler = eventsHandler;
            _roundInfosHandler = roundInfosHandler;
            _settingsHandler = settingsHandler;
            _browserHandler = browserHandler;

            modAPI.Event.Add(ToClientEvent.RoundStart, OnRoundStartMethod);
            modAPI.Event.Add(ToClientEvent.RoundEnd, OnRoundEndMethod);
        }

        private void OnRoundStartMethod(object[] args)
        {
            bool isSpectator = Convert.ToBoolean(args[0]);
            _eventsHandler.OnRoundStarted(isSpectator);

            _modAPI.Cam.DoScreenFadeIn(50);
            _roundInfosHandler.Start(args.Length >= 2 ? Convert.ToInt32(args[1]) : 0);
        }

        private void OnRoundEndMethod(object[] args)
        {
            _eventsHandler.OnRoundEnded();

            _modAPI.Cam.DoScreenFadeOut(_settingsHandler.RoundEndTime / 2);

            string reason = (string)args[0];
            int mapId = (int)args[1];
            _browserHandler.PlainMain.ShowRoundEndReason(reason, mapId);
        }
    }
}
