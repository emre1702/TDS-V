using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Client.Data.Abstracts.Entities.GTA;
using TDS_Client.Data.Enums;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Deathmatch;
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
        public BombHandler Bomb { get; }

        public LobbyCamHandler Camera { get; }

        public CountdownHandler Countdown { get; }

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

        public bool HasAllVsAllTeam => Teams.LobbyTeams.Count(t => !t.IsSpectator) == 1;

        public MainMenuHandler MainMenu { get; }
        public LobbyMapDatasHandler MapDatas { get; }
        public MapManagerHandler MapManager { get; }
        public LobbyPlayersHandler Players { get; }
        public RoundHandler Round { get; }
        public RoundInfosHandler RoundInfos { get; }
        public TeamsHandler Teams { get; }

        private readonly BrowserHandler _browserHandler;

        private readonly EventsHandler _eventsHandler;

        private readonly PlayerFightHandler _playerFightHandler;

        private readonly RemoteEventsSender _remoteEventsSender;

        private readonly SettingsHandler _settingsHandler;

        private readonly UtilsHandler _utilsHandler;

        private bool _inFightLobby;

        private LobbyType? _inLobbyType;

        private bool _isLobbyOwner;

        public LobbyHandler(LoggingHandler loggingHandler, BrowserHandler browserHandler, PlayerFightHandler playerFightHandler,
            EventsHandler eventsHandler, SettingsHandler settingsHandler, BindsHandler bindsHandler, RemoteEventsSender remoteEventsSender, DxHandler dxHandler,
            TimerHandler timerHandler, UtilsHandler utilsHandler, CamerasHandler camerasHandler, CursorHandler cursorHandler, DataSyncHandler dataSyncHandler,
            MapLimitHandler mapLimitHandler)
            : base(loggingHandler)
        {
            _browserHandler = browserHandler;
            _playerFightHandler = playerFightHandler;
            _eventsHandler = eventsHandler;
            _settingsHandler = settingsHandler;
            _remoteEventsSender = remoteEventsSender;

            _utilsHandler = utilsHandler;

            Camera = new LobbyCamHandler(loggingHandler, camerasHandler, settingsHandler, eventsHandler);
            Countdown = new CountdownHandler(loggingHandler, settingsHandler, dxHandler, timerHandler, browserHandler, eventsHandler, Camera);
            MapDatas = new LobbyMapDatasHandler(loggingHandler, dxHandler, timerHandler, eventsHandler, Camera, mapLimitHandler, settingsHandler);
            MapManager = new MapManagerHandler(eventsHandler, browserHandler, settingsHandler, cursorHandler, remoteEventsSender, dataSyncHandler, bindsHandler);
            MainMenu = new MainMenuHandler(eventsHandler, browserHandler);
            Players = new LobbyPlayersHandler(browserHandler, eventsHandler);
            Teams = new TeamsHandler(loggingHandler, browserHandler, bindsHandler, this, remoteEventsSender, cursorHandler, eventsHandler, utilsHandler);
            RoundInfos = new RoundInfosHandler(loggingHandler, Teams, timerHandler, dxHandler, settingsHandler, eventsHandler);
            Round = new RoundHandler(loggingHandler, eventsHandler, RoundInfos, settingsHandler, browserHandler);
            Bomb = new BombHandler(loggingHandler, browserHandler, RoundInfos, settingsHandler, utilsHandler, remoteEventsSender, dxHandler, timerHandler, eventsHandler,
                MapDatas);

            eventsHandler.DataChanged += EventsHandler_DataChanged;

            RAGE.Events.Add(ToClientEvent.JoinLobby, Join);
            RAGE.Events.Add(ToServerEvent.LeaveLobby, Leave);
            RAGE.Events.Add(ToClientEvent.JoinSameLobby, OnJoinSameLobbyMethod);
            RAGE.Events.Add(ToClientEvent.LeaveSameLobby, OnLeaveSameLobbyMethod);

            RAGE.Events.Tick += DisableAttack;
        }

        public void Join(object[] args)
        {
            try
            {
                var oldSettings = _settingsHandler.GetSyncedLobbySettings();
                SyncedLobbySettings settings = Serializer.FromServer<SyncedLobbySettings>((string)args[0]);

                Players.Load(_utilsHandler.GetTriggeredPlayersList((string)args[1]));
                Teams.LobbyTeams = Serializer.FromServer<List<SyncedTeamDataDto>>((string)args[2]);
                Joined(oldSettings, settings);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        public void Joined(SyncedLobbySettings oldSettings, SyncedLobbySettings settings)
        {
            try
            {
                RAGE.Elements.Player.LocalPlayer.ResetAlpha();

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

        private void DisableAttack(List<RAGE.Events.TickNametagData> _)
        {
            if (Bomb.BombOnHand || !_playerFightHandler.InFight)
            {
                RAGE.Game.Pad.DisableControlAction((int)InputGroup.LOOK, (int)Control.Attack, true);
                RAGE.Game.Pad.DisableControlAction((int)InputGroup.LOOK, (int)Control.Attack2, true);
                RAGE.Game.Pad.DisableControlAction((int)InputGroup.LOOK, (int)Control.MeleeAttackLight, true);
                RAGE.Game.Pad.DisableControlAction((int)InputGroup.LOOK, (int)Control.MeleeAttackHeavy, true);
                RAGE.Game.Pad.DisableControlAction((int)InputGroup.LOOK, (int)Control.MeleeAttackAlternate, true);
                RAGE.Game.Pad.DisableControlAction((int)InputGroup.LOOK, (int)Control.MeleeAttack1, true);
                RAGE.Game.Pad.DisableControlAction((int)InputGroup.LOOK, (int)Control.MeleeAttack2, true);
            }
        }

        private void EventsHandler_DataChanged(ITDSPlayer player, PlayerDataKey key, object data)
        {
            try
            {
                if (key != PlayerDataKey.IsLobbyOwner)
                    return;
                if (player != RAGE.Elements.Player.LocalPlayer)
                    return;
                IsLobbyOwner = (bool)data;
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
                ITDSPlayer player = _utilsHandler.GetPlayerByHandleValue(handleValue);
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
                ITDSPlayer player = _utilsHandler.GetPlayerByHandleValue(handleValue);
                string name = (string)args[1];
                _eventsHandler.OnPlayerLeftSameLobby(player, name);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }
    }
}
