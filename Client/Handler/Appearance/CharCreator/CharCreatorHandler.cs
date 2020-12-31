using RAGE;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS.Client.Data.Abstracts.Entities.GTA;
using TDS.Client.Data.Defaults;
using TDS.Client.Data.Enums.CharCreator;
using TDS.Client.Handler.Appearance.CharCreator;
using TDS.Client.Handler.Appearance.CharCreator.Body;
using TDS.Client.Handler.Appearance.CharCreator.Clothes;
using TDS.Client.Handler.Browser;
using TDS.Client.Handler.Deathmatch;
using TDS.Client.Handler.Entities.GTA;
using TDS.Client.Handler.Events;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Enums.CharCreator;
using TDS.Shared.Data.Models;
using TDS.Shared.Data.Models.CharCreator.Body;
using TDS.Shared.Data.Models.GTA;
using TDS.Shared.Default;
using static RAGE.Events;

namespace TDS.Client.Handler.Appearance
{
    public class CharCreatorHandler : ServiceBase
    {
        private uint _dimension;

        private readonly BrowserHandler _browserHandler;
        private readonly CursorHandler _cursorHandler;
        private readonly EventsHandler _eventsHandler;
        private readonly BodyDataHandler _bodyDataHandler;
        private readonly ClothesDataHandler _clothesDataHandler;
        private readonly CharCreatorPedHandler _pedHandler;
        private readonly BodyPedChangesHandler _bodyPedChangesHandler;
        private readonly ClothesPedChangesHandler _clothesPedChangesHandler;
        private readonly CharCreatorCameraHandler _cameraHandler;

        public CharCreatorHandler(LoggingHandler loggingHandler, BrowserHandler browserHandler, DeathHandler deathHandler,
            CamerasHandler camerasHandler, EventsHandler eventsHandler, CursorHandler cursorHandler, UtilsHandler utilsHandler)
            : base(loggingHandler)
        {
            _browserHandler = browserHandler;
            _eventsHandler = eventsHandler;
            _cursorHandler = cursorHandler;

            _bodyDataHandler = new BodyDataHandler(browserHandler);
            _clothesDataHandler = new ClothesDataHandler(browserHandler);
            _pedHandler = new CharCreatorPedHandler(loggingHandler, _bodyDataHandler);
            _bodyPedChangesHandler = new BodyPedChangesHandler(_pedHandler, _bodyDataHandler);
            _clothesPedChangesHandler = new ClothesPedChangesHandler(_pedHandler, _clothesDataHandler);
            _cameraHandler = new CharCreatorCameraHandler(loggingHandler, deathHandler, _pedHandler, camerasHandler, utilsHandler);
            new BodyNavCameraHandler(_cameraHandler);
            new ClothesNavCameraHandler(_cameraHandler);

            Add(ToClientEvent.StartCharCreator, Start);
            Add(FromBrowserEvent.BodyDataChanged, BodyDataChanged);
            Add(FromBrowserEvent.ClothesDataChanged, ClothesDataChanged);
        }

        public void Start(object[] args)
        {
            try
            {
                _eventsHandler.LobbyLeft += Stop;

                string json = (string)args[0];
                var _bodyData = Serializer.FromServer<BodyData>(json);
                _dimension = Convert.ToUInt32(args[1]);

                _browserHandler.Angular.ToggleCharCreator(true);
                Chat.Show(false);
                RAGE.Game.Ui.DisplayRadar(false);

                RAGE.Elements.Player.LocalPlayer.SetAlpha(0, true);
                _cursorHandler.Visible = true;

                _bodyDataHandler.Start(_bodyData);
                _pedHandler.Start(_dimension);
                _cameraHandler.Start();
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        public void Stop(SyncedLobbySettings settings)
        {
            try
            {
                _eventsHandler.LobbyLeft -= Stop;

                RAGE.Elements.Player.LocalPlayer.SetAlpha(255, true);
                _browserHandler.Angular.ToggleCharCreator(false);
                Chat.Show(true);
                RAGE.Game.Ui.DisplayRadar(true);
                _cursorHandler.Visible = false;

                _pedHandler.Stop();
                _cameraHandler.Stop();
                _bodyDataHandler.Stop();
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void BodyDataChanged(object[] keyAndArgs)
        {
            try
            {
                if (_pedHandler.Ped is null)
                    return;

                var key = (BodyDataKey)Convert.ToInt32(keyAndArgs[0]);
                var args = new ArraySegment<object>(keyAndArgs, 1, keyAndArgs.Length - 1);
                _bodyDataHandler.DataChanged(key, ref args);
                _bodyPedChangesHandler.DataChanged(key, ref args);

                if (key == BodyDataKey.IsMale)
                    _cameraHandler.PrepareCameraDelayed(1000);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void ClothesDataChanged(object[] keyAndArgs)
        {
            try
            {
                if (_pedHandler.Ped is null)
                    return;

                var key = (ClothesDataKey)Convert.ToInt32(keyAndArgs[0]);
                var drawableId = Convert.ToInt32(keyAndArgs[1]);
                var textureId = Convert.ToInt32(keyAndArgs[2]);
                _clothesDataHandler.DataChanged(key, drawableId, textureId);
                _clothesPedChangesHandler.DataChanged(key, drawableId, textureId);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }
    }
}