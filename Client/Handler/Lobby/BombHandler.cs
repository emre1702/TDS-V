using RAGE;
using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using TDS.Client.Data.Enums;
using TDS.Client.Data.Models;
using TDS.Client.Handler.Browser;
using TDS.Client.Handler.Draw.Dx;
using TDS.Client.Handler.Events;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Models;
using TDS.Shared.Data.Models.GTA;
using TDS.Shared.Default;
using static RAGE.Events;
using static RAGE.NUI.UIResText;
using Alignment = RAGE.NUI.UIResText.Alignment;

namespace TDS.Client.Handler.Lobby
{
    public class BombHandler : ServiceBase
    {
        private readonly BrowserHandler _browserHandler;

        private readonly DxHandler _dxHandler;

        private readonly EventsHandler _eventsHandler;

        private readonly LobbyMapDatasHandler _lobbyMapDatasHandler;

        private readonly RemoteEventsSender _remoteEventsSender;

        private readonly RoundInfosHandler _roundInfosHandler;

        private readonly SettingsHandler _settingsHandler;

        private readonly TimerHandler _timerHandler;

        private readonly UtilsHandler _utilsHandler;

        private bool _bombPlanted;

        //private int _plantDefuseStartTick;

        private bool _gotBomb;

        private Position3D _plantedPos;

        private PlantDefuseStatus _playerStatus;

        private DxProgressRectangle _progressRect;

        public BombHandler(LoggingHandler loggingHandler, BrowserHandler browserHandler, RoundInfosHandler roundInfosHandler, SettingsHandler settingsHandler,
            UtilsHandler utilsHandler, RemoteEventsSender remoteEventsSender, DxHandler dxHandler, TimerHandler timerHandler, EventsHandler eventsHandler,
            LobbyMapDatasHandler lobbyMapDatasHandler) : base(loggingHandler)
        {
            _browserHandler = browserHandler;
            _roundInfosHandler = roundInfosHandler;
            _settingsHandler = settingsHandler;
            _utilsHandler = utilsHandler;
            _remoteEventsSender = remoteEventsSender;
            _dxHandler = dxHandler;
            _timerHandler = timerHandler;
            _lobbyMapDatasHandler = lobbyMapDatasHandler;

            _eventsHandler = eventsHandler;

            eventsHandler.LobbyLeft += EventsHandler_LobbyLeft;
            eventsHandler.RoundEnded += _ => Stop();

            Add(ToClientEvent.PlayerGotBomb, OnPlayerGotBombMethod);
            Add(ToClientEvent.PlayerPlantedBomb, OnPlayerPlantedBombMethod);
            Add(ToClientEvent.BombPlanted, OnBombPlantedMethod);
            Add(ToClientEvent.BombNotOnHand, OnBombNotOnHandMethod);
            Add(ToClientEvent.BombOnHand, OnBombOnHandMethod);
            Add(ToClientEvent.BombDetonated, OnBombDetonatedMethod);
            Add(ToClientEvent.StopBombPlantDefuse, OnStopBombPlantDefuseMethod);
        }

        private bool _checkPlantDefuseOnTick;
        private bool _dataChanged;
        public bool BombOnHand { get; set; }

        public bool CheckPlantDefuseOnTick
        {
            get => _checkPlantDefuseOnTick;
            set
            {
                _checkPlantDefuseOnTick = value;
                if (value)
                    Tick += CheckPlantDefuse;
                else
                    Tick -= CheckPlantDefuse;
            }
        }

        public void BombPlanted(Position3D pos, bool candefuse, int? startAtMs)
        {
            _dataChanged = true;
            if (candefuse)
            {
                _plantedPos = pos;
                CheckPlantDefuseOnTick = true;
            }
            _bombPlanted = true;
            if (startAtMs.HasValue)
            {
                startAtMs += 100;  // 100 because trigger etc. propably took some time
                _roundInfosHandler.SetRoundTimeLeft(_settingsHandler.BombDetonateTimeMs - startAtMs.Value);
                _browserHandler.PlainMain.StartBombTick(_settingsHandler.BombDetonateTimeMs, startAtMs.Value);
            }
            else
            {
                _roundInfosHandler.SetRoundTimeLeft(_settingsHandler.BombDetonateTimeMs);
                _browserHandler.PlainMain.StartBombTick(_settingsHandler.BombDetonateTimeMs, 0);
            }
            _utilsHandler.Notify(_settingsHandler.Language.BOMB_PLANTED);
        }

        public void CheckPlantDefuse(List<TickNametagData> _)
        {
            if (_playerStatus == PlantDefuseStatus.None)
                CheckPlantDefuseStart();
            else
                CheckPlantDefuseStop();
        }

        public void Detonate()
        {
            RAGE.Game.Cam.ShakeGameplayCam(CamShakeName.LARGE_EXPLOSION_SHAKE, 1.0f);
            new TDSTimer(() => RAGE.Game.Cam.StopGameplayCamShaking(true), 4000, 1);
            _browserHandler.PlainMain.StopBombTick();
        }

        public void LocalPlayerGotBomb()
        {
            _dataChanged = true;
            _gotBomb = true;
            CheckPlantDefuseOnTick = true;
        }

        public void LocalPlayerPlantedBomb()
        {
            _gotBomb = false;
            _playerStatus = PlantDefuseStatus.None;
            CheckPlantDefuseOnTick = false;
            _progressRect?.Remove();
            _progressRect = null;
            BombOnHand = false;
        }

        public void SetBombNotOnHand()
        {
            BombOnHand = false;
            _progressRect?.Remove();
            _progressRect = null;
            _playerStatus = PlantDefuseStatus.None;
            _eventsHandler.LocalPlayerDied -= SetBombNotOnHand;
        }

        public void Stop()
        {
            try
            {
                Logging.LogInfo("", "BombHandler.Stop");
                SetBombNotOnHand();
                if (!_dataChanged)
                    return;
                _dataChanged = false;
                CheckPlantDefuseOnTick = false;
                _gotBomb = false;
                _playerStatus = PlantDefuseStatus.None;
                //_plantDefuseStartTick = 0;
                if (_bombPlanted)
                    _browserHandler.PlainMain.StopBombTick();
                _bombPlanted = false;
                _plantedPos = null;
                Logging.LogInfo("", "BombHandler.Stop", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        public void StopRequestByServer()
        {
            Logging.LogInfo("", "BombHandler.StopRequestByServer");
            _playerStatus = PlantDefuseStatus.None;
            _progressRect?.Remove();
            _progressRect = null;
            Logging.LogInfo("", "BombHandler.StopRequestByServer", true);
        }

        private void CheckDefuseStart()
        {
            if (!IsOnDefuseSpot())
                return;
            //_plantDefuseStartTick = TimerManager.ElapsedTicks;
            _playerStatus = PlantDefuseStatus.Defusing;
            _progressRect = new DxProgressRectangle(_dxHandler, _timerHandler, _settingsHandler.Language.DEFUSING, 0.5f, 0.71f, 0.12f, 0.05f, Color.White, Color.Black, Color.ForestGreen,
                textScale: 0.7f, alignmentX: Alignment.Centered, alignmentY: AlignmentY.Center, frontPriority: 900);
            int defuseTime = _settingsHandler.GetPlantOrDefuseTime(_playerStatus);
            _progressRect.SetAutomatic(defuseTime);
            _remoteEventsSender.Send(ToServerEvent.StartDefusing);
        }

        private void CheckPlantDefuseStart()
        {
            if (ShouldPlantDefuseStop())
                return;
            if (_gotBomb)
                CheckPlantStart();
            else
                CheckDefuseStart();
        }

        private void CheckPlantDefuseStop()
        {
            if (ShouldPlantDefuseStop())
            {
                if (_playerStatus == PlantDefuseStatus.Planting)
                    _remoteEventsSender.Send(ToServerEvent.StopPlanting);
                else if (_playerStatus == PlantDefuseStatus.Defusing)
                    _remoteEventsSender.Send(ToServerEvent.StopDefusing);
                _playerStatus = PlantDefuseStatus.None;
                _progressRect?.Remove();
                _progressRect = null;
            }
        }

        private void CheckPlantStart()
        {
            Logging.LogInfo("", "BombHandler.CheckPlantStart");
            if (!IsOnPlantSpot())
                return;
            //_plantDefuseStartTick = TimerManager.ElapsedTicks;
            _playerStatus = PlantDefuseStatus.Planting;
            _progressRect = new DxProgressRectangle(_dxHandler, _timerHandler, _settingsHandler.Language.PLANTING, 0.5f, 0.71f, 0.2f, 0.08f, Color.White, Color.Black, Color.ForestGreen,
                textScale: 0.7f, alignmentX: Alignment.Centered, alignmentY: AlignmentY.Center, frontPriority: 900);
            int plantTime = _settingsHandler.GetPlantOrDefuseTime(_playerStatus);
            _progressRect.SetAutomatic(plantTime);
            _remoteEventsSender.Send(ToServerEvent.StartPlanting);
            Logging.LogInfo("", "BombHandler.CheckPlantStart", true);
        }

        private void EventsHandler_LobbyLeft(SyncedLobbySettings settings)
        {
            if (settings.Type != LobbyType.Arena)
                return;
            Stop();
        }

        private bool IsOnDefuseSpot()
        {
            if (_plantedPos == null)
                return false;
            Vector3 playerpos = RAGE.Elements.Player.LocalPlayer.Position;
            return RAGE.Game.Misc.GetDistanceBetweenCoords(playerpos.X, playerpos.Y, playerpos.Z, _plantedPos.X, _plantedPos.Y, _plantedPos.Z, true) <= _settingsHandler.DistanceToSpotToDefuse;
        }

        private bool IsOnPlantSpot()
        {
            if (_lobbyMapDatasHandler.MapDatas is null || _lobbyMapDatasHandler.MapDatas.BombPlaces is null || _lobbyMapDatasHandler.MapDatas.BombPlaces.Count == 0)
                return false;
            Vector3 playerpos = RAGE.Elements.Player.LocalPlayer.Position;
            foreach (var pos in _lobbyMapDatasHandler.MapDatas.BombPlaces)
            {
                if (RAGE.Game.Misc.GetDistanceBetweenCoords(playerpos.X, playerpos.Y, playerpos.Z, pos.X, pos.Y, pos.Z, pos.Z != 0) <= _settingsHandler.DistanceToSpotToPlant)
                    return true;
            }
            return false;
        }

        private void OnBombDetonatedMethod(object[] args)
        {
            Detonate();
        }

        private void OnBombNotOnHandMethod(object[] args)
        {
            SetBombNotOnHand();
        }

        private void OnBombOnHandMethod(object[] args)
        {
            BombOnHand = true;
            _eventsHandler.LocalPlayerDied += SetBombNotOnHand;
        }

        private void OnBombPlantedMethod(object[] args)
        {
            BombPlanted(Serializer.FromServer<Position3D>((string)args[0]), Convert.ToBoolean(args[1]), args.Length > 2 ? (int?)args[2] : null);
        }

        private void OnPlayerGotBombMethod(object[] args)
        {
            LocalPlayerGotBomb();
        }

        private void OnPlayerPlantedBombMethod(object[] args)
        {
            LocalPlayerPlantedBomb();
        }

        private void OnStopBombPlantDefuseMethod(object[] args)
        {
            StopRequestByServer();
        }

        private bool ShouldPlantDefuseStop()
        {
            var weaponHash = RAGE.Elements.Player.LocalPlayer.GetSelectedWeapon();
            if (weaponHash != (uint)WeaponHash.Unarmed)
                return true;
            if (!Pad.IsDisabledControlPressed((int)InputGroup.MOVE, (int)Control.Attack)
                && !Pad.IsControlPressed((int)InputGroup.MOVE, (int)Control.Attack))
                return true;
            if (RAGE.Elements.Player.LocalPlayer.IsDeadOrDying(true))
                return true;
            return false;
        }
    }
}