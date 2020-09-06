﻿using RAGE;
using System;
using System.Collections.Generic;
using TDS_Client.Data.Abstracts.Entities.GTA;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Deathmatch;
using TDS_Client.Handler.Draw;
using TDS_Client.Handler.Events;
using TDS_Shared.Data.Models;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Default;
using static RAGE.Events;

namespace TDS_Client.Handler
{
    public class RankingHandler : ServiceBase
    {
        #region Private Fields

        private readonly BrowserHandler _browserHandler;
        private readonly CamerasHandler _camerasHandler;
        private readonly CursorHandler _cursorHandler;
        private readonly DeathHandler _deathHandler;
        private readonly NametagsHandler _nametagsHandler;
        private readonly SettingsHandler _settingsHandler;
        private readonly UtilsHandler _utilsHandler;
        private readonly TimerHandler _timerHandler;
        private long _confettiShownLastMs = 0;
        private ITDSPlayer _second = null;
        private ITDSPlayer _third = null;
        private ITDSPlayer _winner = null;

        #endregion Private Fields

        public RankingHandler(LoggingHandler loggingHandler, CamerasHandler camerasHandler, UtilsHandler utilsHandler, SettingsHandler settingsHandler,
            CursorHandler cursorHandler, BrowserHandler browserHandler, NametagsHandler nametagsHandler, DeathHandler deathHandler, EventsHandler eventsHandler,
            TimerHandler timerHandler)
            : base(loggingHandler)
        {
            _camerasHandler = camerasHandler;
            _utilsHandler = utilsHandler;
            _settingsHandler = settingsHandler;
            _cursorHandler = cursorHandler;
            _browserHandler = browserHandler;
            _nametagsHandler = nametagsHandler;
            _deathHandler = deathHandler;
            _timerHandler = timerHandler;

            eventsHandler.LobbyLeft += EventsHandler_LobbyLeft;
            eventsHandler.CountdownStarted += _ => Stop();

            Add(ToClientEvent.StartRankingShowAfterRound, OnStartRankingShowAfterRoundMethod);
        }

        public void Start(string rankingsJson, ushort winnerHandle, ushort secondHandle, ushort thirdHandle)
        {
            _deathHandler.PlayerSpawn();

            var cam = _camerasHandler.BetweenRoundsCam;
            cam.Position = new Vector3(-425.2233f, 1126.9731f, 326.8f);
            cam.PointCamAtCoord(new Vector3(-427.03f, 1123.21f, 325.85f));
            cam.Activate(true);
            // Cam-pos:
            //X: -425,2233
            //Y: 1126.9731
            //Z: 326.8
            //Rot: 160

            RAGE.Game.Cam.DoScreenFadeIn(200);
            Tick += OnRender;

            _winner = _utilsHandler.GetPlayerByHandleValue(winnerHandle);
            _second = secondHandle != 0 ? _utilsHandler.GetPlayerByHandleValue(secondHandle) : null;
            _third = thirdHandle != 0 ? _utilsHandler.GetPlayerByHandleValue(thirdHandle) : null;

            _browserHandler.Angular.ShowRankings(rankingsJson);
            _cursorHandler.Visible = true;
        }

        public void Stop()
        {
            Tick -= OnRender;
            _browserHandler.Angular.HideRankings();
            _cursorHandler.Visible = false;
        }

        #region Private Methods

        private void EventsHandler_LobbyLeft(SyncedLobbySettings settings)
        {
            Stop();
        }

        private void OnRender(List<TickNametagData> _)
        {
            //StartParticleFx("scr_xs_money_rain", -425.48f, 1123.55f, 325.85f, 1f);
            //StartParticleFx("scr_xs_money_rain_celeb", 427.03f, 1123.21f, 325.85f, 1f);
            var currentMs = _timerHandler.ElapsedMs;
            if (_settingsHandler.PlayerSettings.ShowConfettiAtRanking && currentMs - _confettiShownLastMs > 400)
            {
                _confettiShownLastMs = currentMs;
                StartParticleFx("scr_xs_confetti_burst", "scr_xs_celebration", -428.01f, 1123.47f, 325f, 1.5f);
                StartParticleFx("scr_xs_confetti_burst", "scr_xs_celebration", -423.48f, 1122.09f, 325f, 1.5f);
                StartParticleFx("scr_xs_confetti_burst", "scr_xs_celebration", -426.17f, 1121.18f, 325f, 2f);
            }

            // didnt work
            //StartParticleFx("scr_xs_champagne_spray", -428.01f, 1123.47f, 325f, 1.5f);
            //StartParticleFx("scr_xs_champagne_spray", -423.48f, 1122.09f, 325f, 1.5f);
            //StartParticleFx("scr_xs_beer_chug", -426.17f, 1121.18f, 325f, 2f);

            if (!(_winner is null) && _winner.Exists)
                _nametagsHandler.DrawNametag(_winner.Handle, "1. " + _utilsHandler.GetDisplayName(_winner), 7f);
            if (!(_second is null) && _second.Exists)
                _nametagsHandler.DrawNametag(_second.Handle, "2. " + _utilsHandler.GetDisplayName(_second), 7f);
            if (!(_third is null) && _third.Exists)
                _nametagsHandler.DrawNametag(_third.Handle, "3. " + _utilsHandler.GetDisplayName(_third), 7f);

            //StartParticleFx("scr_xs_champagne_spray", -425.48f, 1123.55f, 325.85f, 1f);
            //StartParticleFx("scr_xs_beer_chug", 427.03f, 1123.21f, 325.85f, 1f);
        }

        private void OnStartRankingShowAfterRoundMethod(object[] args)
        {
            Start((string)args[0], Convert.ToUInt16(args[1]), Convert.ToUInt16(args[2]), Convert.ToUInt16(args[3]));
        }

        private int StartParticleFx(string effectName, string effectDict, float x, float y, float z, float scale)
        {
            RAGE.Game.Graphics.UseParticleFxAssetNextCall(effectDict);
            return RAGE.Game.Graphics.StartParticleFxNonLoopedAtCoord(effectName, x, y, z, 0, 0, 0, scale, false, false, false);
        }

        #endregion Private Methods
    }
}
