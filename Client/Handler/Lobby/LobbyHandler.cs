using TDS_Client.Data.Defaults;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Deathmatch;
using TDS_Client.Handler.Draw;
using TDS_Client.Handler.Draw.Dx;
using TDS_Client.Handler.Events;
using TDS_Client.Handler.Map;
using TDS_Client.Handler.Sync;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;
using TDS_Shared.Default;

namespace TDS_Client.Handler.Lobby
{
    public class LobbyHandler
    {
        public bool IsLobbyOwner
        {
            get => _isLobbyOwner;
            set
            {
                if (_isLobbyOwner != value)
                {
                    _browserHandler.Angular.SyncIsLobbyOwner(value);
                }
                _isLobbyOwner = value;
            }
        }

        public bool InFightLobby
        {
            get => _inFightLobby;
            set
            {
                _inFightLobby = value;
                _browserHandler.Angular.SyncInFightLobby(value);
                _browserHandler.Angular.ToggleHUD(_inFightLobby);
                _playerFightHandler.Reset();
            }
        }

        private LobbyType? _inLobbyType;
        private bool _isLobbyOwner;
        private bool _inFightLobby;

        public BombHandler Bomb { get; }
        public LobbyCamHandler Camera { get; }
        public CountdownHandler Countdown { get; }
        public LobbyChoiceHandler Choice { get; }
        public LobbyMapDatasHandler MapDatas { get; }
        public MainMenuHandler MainMenu { get; }
        public MapManagerHandler MapManager { get; }
        public LobbyPlayersHandler Players { get; }
        public RoundHandler Round { get; }
        public RoundInfosHandler RoundInfos { get; }
        public TeamsHandler Teams { get; }

        private readonly IModAPI _modAPI;
        private readonly BrowserHandler _browserHandler;
        private readonly PlayerFightHandler _playerFightHandler;
        private readonly InstructionalButtonHandler _instructionalButtonHandler;
        private readonly EventsHandler _eventsHandler;
        private readonly SettingsHandler _settingsHandler;
        private readonly RemoteEventsSender _remoteEventsSender;

        public LobbyHandler(IModAPI modAPI, BrowserHandler browserHandler, PlayerFightHandler playerFightHandler, InstructionalButtonHandler instructionalButtonHandler,
            EventsHandler eventsHandler, SettingsHandler settingsHandler, BindsHandler bindsHandler, RemoteEventsSender remoteEventsSender, DxHandler dxHandler,
            TimerHandler timerHandler, UtilsHandler utilsHandler, CamerasHandler camerasHandler, CursorHandler cursorHandler, DataSyncHandler dataSyncHandler,
            MapLimitHandler mapLimitHandler)
        {
            _modAPI = modAPI;
            _browserHandler = browserHandler;
            _playerFightHandler = playerFightHandler;
            _instructionalButtonHandler = instructionalButtonHandler;
            _eventsHandler = eventsHandler;
            _settingsHandler = settingsHandler;
            _remoteEventsSender = remoteEventsSender;

            Camera = new LobbyCamHandler(camerasHandler, settingsHandler, eventsHandler);
            Countdown = new CountdownHandler(settingsHandler, dxHandler, modAPI, timerHandler, browserHandler, eventsHandler);
            Choice = new LobbyChoiceHandler(modAPI, remoteEventsSender, settingsHandler);
            MapDatas = new LobbyMapDatasHandler(modAPI, dxHandler, timerHandler, eventsHandler, Camera, mapLimitHandler);
            MapManager = new MapManagerHandler(eventsHandler, modAPI, browserHandler, settingsHandler, cursorHandler, remoteEventsSender, dataSyncHandler);
            MainMenu = new MainMenuHandler(eventsHandler, browserHandler);
            Players = new LobbyPlayersHandler(browserHandler);
            Round = new RoundHandler();
            Teams = new TeamsHandler(modAPI, browserHandler, bindsHandler, this, remoteEventsSender, cursorHandler);
            RoundInfos = new RoundInfosHandler(modAPI, Teams, timerHandler, dxHandler, settingsHandler, eventsHandler);
            Bomb = new BombHandler(modAPI, browserHandler, RoundInfos, settingsHandler, utilsHandler, remoteEventsSender, dxHandler, timerHandler, eventsHandler, MapDatas);

            eventsHandler.DataChanged += EventsHandler_DataChanged;

            _modAPI.Event.Add(ToServerEvent.LeaveLobby, Leave);
        }

        public void Joined(SyncedLobbySettingsDto oldSettings, SyncedLobbySettingsDto settings)
        {
            _modAPI.LocalPlayer.ResetAlpha();

            if (_inLobbyType != null)
            {
                switch (_inLobbyType)
                {
                    default:
                        Left();
                        break;
                }
            }
            _eventsHandler.OnLobbyLeft(oldSettings);

            switch (settings.Type)
            {
                case LobbyType.Arena:
                case LobbyType.FightLobby:
                    InFightLobby = true;
                    break;

                default:
                    InFightLobby = false;
                    break;
            }
            _eventsHandler.OnLobbyJoined(settings);

            _inLobbyType = settings.Type;
        }


        private void Left()
        {
            Round.Reset(true);
        }

        private void Leave(object[] args)
        {
            _browserHandler.Angular.ToggleTeamChoiceMenu(false);
            // If we were in team choice
            _remoteEventsSender.Send(ToServerEvent.LeaveLobby);
        }

        private void EventsHandler_DataChanged(IPlayer player, PlayerDataKey key, object data)
        {
            if (key != PlayerDataKey.IsLobbyOwner)
                return;
            if (player != _modAPI.LocalPlayer)
                return;
            IsLobbyOwner = (bool)data;
        }
    }
}
