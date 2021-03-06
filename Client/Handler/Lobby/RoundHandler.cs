﻿using System;
using TDS.Client.Handler.Browser;
using TDS.Client.Handler.Events;
using TDS.Shared.Default;

namespace TDS.Client.Handler.Lobby
{
    public class RoundHandler : ServiceBase
    {
        private readonly BrowserHandler _browserHandler;
        private readonly EventsHandler _eventsHandler;
        private readonly RoundInfosHandler _roundInfosHandler;
        private readonly SettingsHandler _settingsHandler;

        public RoundHandler(LoggingHandler loggingHandler, EventsHandler eventsHandler, RoundInfosHandler roundInfosHandler,
            SettingsHandler settingsHandler, BrowserHandler browserHandler)
            : base(loggingHandler)
        {
            _eventsHandler = eventsHandler;
            _roundInfosHandler = roundInfosHandler;
            _settingsHandler = settingsHandler;
            _browserHandler = browserHandler;

            RAGE.Events.Add(ToClientEvent.RoundStart, OnRoundStartMethod);
            RAGE.Events.Add(ToClientEvent.RoundEnd, OnRoundEndMethod);
        }

        private void OnRoundEndMethod(object[] args)
        {
            bool isSpectator = (bool)args[0];
            _eventsHandler.OnRoundEnded(isSpectator);

            RAGE.Game.Cam.DoScreenFadeOut(_settingsHandler.RoundEndTime / 2);

            string reason = (string)args[1];
            int mapId = (int)args[2];
            _browserHandler.PlainMain.ShowRoundEndReason(reason, mapId);
        }

        private void OnRoundStartMethod(object[] args)
        {
            try
            {
                bool isSpectator = Convert.ToBoolean(args[0]);
                _eventsHandler.OnRoundStarted(isSpectator);

                RAGE.Game.Cam.DoScreenFadeIn(50);
                _roundInfosHandler.Start(args.Length >= 2 ? Convert.ToInt32(args[1]) : 0);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }
    }
}
