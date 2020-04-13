using System;
using System.Collections.Generic;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Deathmatch;
using TDS_Client.Handler.Draw;
using TDS_Client.Handler.Draw.Dx;
using TDS_Client.Handler.Events;
using TDS_Client.Handler.Map;
using TDS_Client.Handler.Sync;
using TDS_Shared.Core;
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
        private readonly Serializer _serializer;
        private readonly UtilsHandler _utilsHandler;

        public LobbyHandler(IModAPI modAPI, BrowserHandler browserHandler, PlayerFightHandler playerFightHandler, InstructionalButtonHandler instructionalButtonHandler,
            EventsHandler eventsHandler, SettingsHandler settingsHandler, BindsHandler bindsHandler, RemoteEventsSender remoteEventsSender, DxHandler dxHandler,
            TimerHandler timerHandler, UtilsHandler utilsHandler, CamerasHandler camerasHandler, CursorHandler cursorHandler, DataSyncHandler dataSyncHandler,
            MapLimitHandler mapLimitHandler, Serializer serializer)
        {
            _modAPI = modAPI;
            _browserHandler = browserHandler;
            _playerFightHandler = playerFightHandler;
            _instructionalButtonHandler = instructionalButtonHandler;
            _eventsHandler = eventsHandler;
            _settingsHandler = settingsHandler;
            _remoteEventsSender = remoteEventsSender;
            _serializer = serializer;
            _utilsHandler = utilsHandler;

            Camera = new LobbyCamHandler(camerasHandler, settingsHandler, eventsHandler);
            Countdown = new CountdownHandler(settingsHandler, dxHandler, modAPI, timerHandler, browserHandler, eventsHandler, Camera);
            Choice = new LobbyChoiceHandler(modAPI, remoteEventsSender, settingsHandler);
            MapDatas = new LobbyMapDatasHandler(modAPI, dxHandler, timerHandler, eventsHandler, Camera, mapLimitHandler, serializer, settingsHandler);
            MapManager = new MapManagerHandler(eventsHandler, modAPI, browserHandler, settingsHandler, cursorHandler, remoteEventsSender, dataSyncHandler, bindsHandler);
            MainMenu = new MainMenuHandler(eventsHandler, browserHandler);
            Players = new LobbyPlayersHandler(browserHandler, eventsHandler);
            Round = new RoundHandler(modAPI, eventsHandler, RoundInfos, settingsHandler, browserHandler);
            Teams = new TeamsHandler(modAPI, browserHandler, bindsHandler, this, remoteEventsSender, cursorHandler, eventsHandler, utilsHandler, serializer);
            RoundInfos = new RoundInfosHandler(modAPI, Teams, timerHandler, dxHandler, settingsHandler, eventsHandler, serializer);
            Bomb = new BombHandler(modAPI, browserHandler, RoundInfos, settingsHandler, utilsHandler, remoteEventsSender, dxHandler, timerHandler, eventsHandler, MapDatas, serializer);

            eventsHandler.DataChanged += EventsHandler_DataChanged;

            _modAPI.Event.Add(ToClientEvent.JoinLobby, Join);
            _modAPI.Event.Add(ToServerEvent.LeaveLobby, Leave);
            _modAPI.Event.Add(ToClientEvent.JoinSameLobby, OnJoinSameLobbyMethod);
            _modAPI.Event.Add(ToClientEvent.LeaveSameLobby, OnLeaveSameLobbyMethod);
            _modAPI.Event.Tick.Add(new EventMethodData<TickDelegate>(DisableAttack, () => Bomb.BombOnHand || !playerFightHandler.InFight));
        }

        public void Joined(SyncedLobbySettings oldSettings, SyncedLobbySettings settings)
        {
            _modAPI.LocalPlayer.ResetAlpha();

            if (oldSettings != null)
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

        public void Join(object[] args)
        {
            var oldSettings = _settingsHandler.GetSyncedLobbySettings();
            SyncedLobbySettings settings = _serializer.FromServer<SyncedLobbySettings>((string)args[0]);

            Players.Load(_utilsHandler.GetTriggeredPlayersList((string)args[1]));
            Teams.LobbyTeams = _serializer.FromServer<List<SyncedTeamDataDto>>((string)args[2]);
            Joined(oldSettings, settings);
        }

        private void Leave(object[] args)
        {
            _browserHandler.Angular.ToggleTeamChoiceMenu(false);
            // If we were in team choice
            _remoteEventsSender.Send(ToServerEvent.LeaveLobby);
        }

        private void OnJoinSameLobbyMethod(object[] args)
        {
            ushort handleValue = Convert.ToUInt16(args[0]);
            IPlayer player = _utilsHandler.GetPlayerByHandleValue(handleValue);
            _eventsHandler.OnPlayerJoinedSameLobby(player);
        }

        private void OnLeaveSameLobbyMethod(object[] args)
        {
            ushort handleValue = Convert.ToUInt16(args[0]);
            IPlayer player = _utilsHandler.GetPlayerByHandleValue(handleValue);
            string name = (string)args[1];
            _eventsHandler.OnPlayerLeftSameLobby(player, name);
        }

        private void EventsHandler_DataChanged(IPlayer player, PlayerDataKey key, object data)
        {
            if (key != PlayerDataKey.IsLobbyOwner)
                return;
            if (player != _modAPI.LocalPlayer)
                return;
            IsLobbyOwner = (bool)data;
        }

        private void DisableAttack(int _)
        {
            _modAPI.Control.DisableControlAction(InputGroup.LOOK, Control.Attack);
            _modAPI.Control.DisableControlAction(InputGroup.LOOK, Control.Attack2);
            _modAPI.Control.DisableControlAction(InputGroup.LOOK, Control.MeleeAttackLight);
            _modAPI.Control.DisableControlAction(InputGroup.LOOK, Control.MeleeAttackHeavy);
            _modAPI.Control.DisableControlAction(InputGroup.LOOK, Control.MeleeAttackAlternate);
            _modAPI.Control.DisableControlAction(InputGroup.LOOK, Control.MeleeAttack1);
            _modAPI.Control.DisableControlAction(InputGroup.LOOK, Control.MeleeAttack2);
        }
    }
}
