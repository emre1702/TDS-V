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
    public class LobbyHandler : ServiceBase
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

        private readonly BrowserHandler _browserHandler;
        private readonly PlayerFightHandler _playerFightHandler;
        private readonly InstructionalButtonHandler _instructionalButtonHandler;
        private readonly EventsHandler _eventsHandler;
        private readonly SettingsHandler _settingsHandler;
        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly Serializer _serializer;
        private readonly UtilsHandler _utilsHandler;

        public LobbyHandler(IModAPI modAPI, LoggingHandler loggingHandler, BrowserHandler browserHandler, PlayerFightHandler playerFightHandler, 
            InstructionalButtonHandler instructionalButtonHandler,
            EventsHandler eventsHandler, SettingsHandler settingsHandler, BindsHandler bindsHandler, RemoteEventsSender remoteEventsSender, DxHandler dxHandler,
            TimerHandler timerHandler, UtilsHandler utilsHandler, CamerasHandler camerasHandler, CursorHandler cursorHandler, DataSyncHandler dataSyncHandler,
            MapLimitHandler mapLimitHandler, Serializer serializer)
            : base(modAPI, loggingHandler)
        {
            _browserHandler = browserHandler;
            _playerFightHandler = playerFightHandler;
            _instructionalButtonHandler = instructionalButtonHandler;
            _eventsHandler = eventsHandler;
            _settingsHandler = settingsHandler;
            _remoteEventsSender = remoteEventsSender;
            _serializer = serializer;
            _utilsHandler = utilsHandler;

            Camera = new LobbyCamHandler(modAPI, loggingHandler, camerasHandler, settingsHandler, eventsHandler);
            Countdown = new CountdownHandler(modAPI, loggingHandler, settingsHandler, dxHandler, timerHandler, browserHandler, eventsHandler, Camera);
            Choice = new LobbyChoiceHandler(modAPI, remoteEventsSender, settingsHandler);
            MapDatas = new LobbyMapDatasHandler(modAPI, loggingHandler, dxHandler, timerHandler, eventsHandler, Camera, mapLimitHandler, serializer, settingsHandler);
            MapManager = new MapManagerHandler(eventsHandler, modAPI, browserHandler, settingsHandler, cursorHandler, remoteEventsSender, dataSyncHandler, bindsHandler);
            MainMenu = new MainMenuHandler(eventsHandler, browserHandler);
            Players = new LobbyPlayersHandler(browserHandler, eventsHandler);
            Teams = new TeamsHandler(modAPI, loggingHandler, browserHandler, bindsHandler, this, remoteEventsSender, cursorHandler, eventsHandler, utilsHandler);
            RoundInfos = new RoundInfosHandler(modAPI, loggingHandler, Teams, timerHandler, dxHandler, settingsHandler, eventsHandler, serializer);
            Round = new RoundHandler(modAPI, loggingHandler, eventsHandler, RoundInfos, settingsHandler, browserHandler);
            Bomb = new BombHandler(modAPI, loggingHandler, browserHandler, RoundInfos, settingsHandler, utilsHandler, remoteEventsSender, dxHandler, timerHandler, eventsHandler, 
                MapDatas, serializer);

            eventsHandler.DataChanged += EventsHandler_DataChanged;

            ModAPI.Event.Add(ToClientEvent.JoinLobby, Join);
            ModAPI.Event.Add(ToServerEvent.LeaveLobby, Leave);
            ModAPI.Event.Add(ToClientEvent.JoinSameLobby, OnJoinSameLobbyMethod);
            ModAPI.Event.Add(ToClientEvent.LeaveSameLobby, OnLeaveSameLobbyMethod);
            ModAPI.Event.Tick.Add(new EventMethodData<TickDelegate>(DisableAttack, () => Bomb.BombOnHand || !playerFightHandler.InFight));
        }

        public void Joined(SyncedLobbySettings oldSettings, SyncedLobbySettings settings)
        {
            try
            {
                ModAPI.LocalPlayer.ResetAlpha();

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
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        public void Join(object[] args)
        {
            try
            { 
                var oldSettings = _settingsHandler.GetSyncedLobbySettings();
                SyncedLobbySettings settings = _serializer.FromServer<SyncedLobbySettings>((string)args[0]);

                Players.Load(_utilsHandler.GetTriggeredPlayersList((string)args[1]));
                Teams.LobbyTeams = _serializer.FromServer<List<SyncedTeamDataDto>>((string)args[2]);
                Joined(oldSettings, settings);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void Leave(object[] args)
        {
            try
            { 
                _browserHandler.Angular.ToggleTeamChoiceMenu(false);
                // If we were in team choice
                _remoteEventsSender.Send(ToServerEvent.LeaveLobby);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void OnJoinSameLobbyMethod(object[] args)
        {
            try
            {
                ushort handleValue = Convert.ToUInt16(args[0]);
                IPlayer player = _utilsHandler.GetPlayerByHandleValue(handleValue);
                _eventsHandler.OnPlayerJoinedSameLobby(player);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void OnLeaveSameLobbyMethod(object[] args)
        {
            try
            { 
                ushort handleValue = Convert.ToUInt16(args[0]);
                IPlayer player = _utilsHandler.GetPlayerByHandleValue(handleValue);
                string name = (string)args[1];
                _eventsHandler.OnPlayerLeftSameLobby(player, name);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void EventsHandler_DataChanged(IPlayer player, PlayerDataKey key, object data)
        {
            try
            {
                if (key != PlayerDataKey.IsLobbyOwner)
                    return;
                if (player != ModAPI.LocalPlayer)
                    return;
                IsLobbyOwner = (bool)data;
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void DisableAttack(int _)
        {
            ModAPI.Control.DisableControlAction(InputGroup.LOOK, Control.Attack);
            ModAPI.Control.DisableControlAction(InputGroup.LOOK, Control.Attack2);
            ModAPI.Control.DisableControlAction(InputGroup.LOOK, Control.MeleeAttackLight);
            ModAPI.Control.DisableControlAction(InputGroup.LOOK, Control.MeleeAttackHeavy);
            ModAPI.Control.DisableControlAction(InputGroup.LOOK, Control.MeleeAttackAlternate);
            ModAPI.Control.DisableControlAction(InputGroup.LOOK, Control.MeleeAttack1);
            ModAPI.Control.DisableControlAction(InputGroup.LOOK, Control.MeleeAttack2);
        }
    }
}
