using System;
using System.Drawing;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Draw.Dx;
using TDS_Client.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Default;

namespace TDS_Client.Handler.Lobby
{
    public class BombHandler : ServiceBase
    {
        #region Private Fields

        private readonly BrowserHandler _browserHandler;

        private readonly DxHandler _dxHandler;

        private readonly EventsHandler _eventsHandler;

        private readonly LobbyMapDatasHandler _lobbyMapDatasHandler;

        private readonly RemoteEventsSender _remoteEventsSender;

        private readonly RoundInfosHandler _roundInfosHandler;

        private readonly Serializer _serializer;

        private readonly SettingsHandler _settingsHandler;

        private readonly EventMethodData<TickDelegate> _tickEventMethod;

        private readonly TimerHandler _timerHandler;

        private readonly UtilsHandler _utilsHandler;

        private bool _bombPlanted;

        //private int _plantDefuseStartTick;
        private bool _dataChanged;

        private bool _gotBomb;

        private Position3D _plantedPos;

        private PlantDefuseStatus _playerStatus;

        private DxProgressRectangle _progressRect;

        #endregion Private Fields

        #region Public Constructors

        public BombHandler(IModAPI modAPI, LoggingHandler loggingHandler, BrowserHandler browserHandler, RoundInfosHandler roundInfosHandler, SettingsHandler settingsHandler,
            UtilsHandler utilsHandler, RemoteEventsSender remoteEventsSender, DxHandler dxHandler, TimerHandler timerHandler, EventsHandler eventsHandler,
            LobbyMapDatasHandler lobbyMapDatasHandler, Serializer serializer) : base(modAPI, loggingHandler)
        {
            _browserHandler = browserHandler;
            _roundInfosHandler = roundInfosHandler;
            _settingsHandler = settingsHandler;
            _utilsHandler = utilsHandler;
            _remoteEventsSender = remoteEventsSender;
            _dxHandler = dxHandler;
            _timerHandler = timerHandler;
            _lobbyMapDatasHandler = lobbyMapDatasHandler;
            _serializer = serializer;
            _eventsHandler = eventsHandler;

            _tickEventMethod = new EventMethodData<TickDelegate>(CheckPlantDefuse, () => CheckPlantDefuseOnTick);

            eventsHandler.LobbyLeft += EventsHandler_LobbyLeft;
            eventsHandler.RoundEnded += _ => Stop();

            modAPI.Event.Add(ToClientEvent.PlayerGotBomb, OnPlayerGotBombMethod);
            modAPI.Event.Add(ToClientEvent.PlayerPlantedBomb, OnPlayerPlantedBombMethod);
            modAPI.Event.Add(ToClientEvent.BombPlanted, OnBombPlantedMethod);
            modAPI.Event.Add(ToClientEvent.BombNotOnHand, OnBombNotOnHandMethod);
            modAPI.Event.Add(ToClientEvent.BombOnHand, OnBombOnHandMethod);
            modAPI.Event.Add(ToClientEvent.BombDetonated, OnBombDetonatedMethod);
            modAPI.Event.Add(ToClientEvent.StopBombPlantDefuse, OnStopBombPlantDefuseMethod);
        }

        #endregion Public Constructors

        #region Public Properties

        public bool BombOnHand { get; set; }

        public bool CheckPlantDefuseOnTick { get; private set; }

        public bool DataChanged
        {
            get => _dataChanged;
            set
            {
                if (_dataChanged == value)
                    return;
                _dataChanged = value;
                if (value)
                    ModAPI.Event.Tick.Add(_tickEventMethod);
                else
                    ModAPI.Event.Tick.Remove(_tickEventMethod);
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void BombPlanted(Position3D pos, bool candefuse, int? startAtMs)
        {
            DataChanged = true;
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

        public void CheckPlantDefuse(int currentMs)
        {
            if (_playerStatus == PlantDefuseStatus.None)
                CheckPlantDefuseStart();
            else
                CheckPlantDefuseStop();
        }

        public void Detonate()
        {
            ModAPI.Cam.ShakeGameplayCam(CamShakeName.LARGE_EXPLOSION_SHAKE, 1.0f);
            new TDSTimer(ModAPI.Cam.StopGameplayCamShaking, 4000, 1);
            _browserHandler.PlainMain.StopBombTick();
        }

        public void LocalPlayerGotBomb()
        {
            DataChanged = true;
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
                if (!DataChanged)
                    return;
                DataChanged = false;
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

        #endregion Public Methods

        #region Private Methods

        private void CheckDefuseStart()
        {
            if (!IsOnDefuseSpot())
                return;
            //_plantDefuseStartTick = TimerManager.ElapsedTicks;
            _playerStatus = PlantDefuseStatus.Defusing;
            _progressRect = new DxProgressRectangle(_dxHandler, ModAPI, _timerHandler, _settingsHandler.Language.DEFUSING, 0.5f, 0.71f, 0.12f, 0.05f, Color.White, Color.Black, Color.ForestGreen,
                textScale: 0.7f, alignmentX: AlignmentX.Center, alignmentY: AlignmentY.Center, frontPriority: 900);
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
            _progressRect = new DxProgressRectangle(_dxHandler, ModAPI, _timerHandler, _settingsHandler.Language.PLANTING, 0.5f, 0.71f, 0.2f, 0.08f, Color.White, Color.Black, Color.ForestGreen,
                textScale: 0.7f, alignmentX: AlignmentX.Center, alignmentY: AlignmentY.Center, frontPriority: 900);
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
            Position3D playerpos = ModAPI.LocalPlayer.Position;
            return ModAPI.Misc.GetDistanceBetweenCoords(playerpos, _plantedPos, true) <= _settingsHandler.DistanceToSpotToDefuse;
        }

        private bool IsOnPlantSpot()
        {
            if (_lobbyMapDatasHandler.MapDatas is null || _lobbyMapDatasHandler.MapDatas.BombPlaces is null || _lobbyMapDatasHandler.MapDatas.BombPlaces.Count == 0)
                return false;
            Position3D playerpos = ModAPI.LocalPlayer.Position;
            foreach (Position3D pos in _lobbyMapDatasHandler.MapDatas.BombPlaces)
            {
                if (ModAPI.Misc.GetDistanceBetweenCoords(playerpos, pos, pos.Z != 0) <= _settingsHandler.DistanceToSpotToPlant)
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
            BombPlanted(_serializer.FromServer<Position3D>((string)args[0]), Convert.ToBoolean(args[1]), args.Length > 2 ? (int?)args[2] : null);
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
            var weaponHash = ModAPI.LocalPlayer.GetSelectedWeapon();
            if (weaponHash != WeaponHash.Unarmed)
                return true;
            if (!ModAPI.Control.IsDisabledControlPressed(InputGroup.MOVE, Control.Attack))
                return true;
            if (ModAPI.LocalPlayer.IsDeadOrDying())
                return true;
            return false;
        }

        #endregion Private Methods
    }
}
