using System;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Deathmatch;
using TDS_Client.Handler.Events;
using TDS_Client.Handler.Lobby;
using TDS_Client.Manager.Utility;
using TDS_Shared.Default;

namespace TDS_Client.Handler
{
    public class ChatHandler : ServiceBase
    {
        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                if (_isOpen == value)
                    return;
                _isOpen = value;
                _eventsHandler.OnChatInputToggled(value);
                if (value)
                    ModAPI.Event.Tick.Add(_tickEventMethod);
                else
                    ModAPI.Event.Tick.Remove(_tickEventMethod);
            }
        }

        private bool _isOpen;

        private readonly EventMethodData<TickDelegate> _tickEventMethod;

        private readonly BrowserHandler _browserHandler;
        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly LobbyHandler _lobbyHandler;
        private readonly PlayerFightHandler _playerFightHandler;
        private readonly EventsHandler _eventsHandler;
        private readonly CamerasHandler _camerasHandler;

        public ChatHandler(IModAPI modAPI, LoggingHandler loggingHandler,  BrowserHandler browserHandler, BindsHandler bindsHandler, RemoteEventsSender remoteEventsSender,
            LobbyHandler lobbyHandler, PlayerFightHandler playerFightHandler, EventsHandler eventsHandler, CamerasHandler camerasHandler)
            : base(modAPI, loggingHandler)
        {
            _browserHandler = browserHandler;
            _remoteEventsSender = remoteEventsSender;
            _lobbyHandler = lobbyHandler;
            _playerFightHandler = playerFightHandler;
            _eventsHandler = eventsHandler;
            _camerasHandler = camerasHandler;

            _tickEventMethod = new EventMethodData<TickDelegate>(OnUpdate);

            modAPI.Chat.Show(false);

            bindsHandler.Add(Control.MpTextChatAll, OpenLobbyChatInput);
            bindsHandler.Add(Control.MpTextChatTeam, OpenTeamChatInput);
            
            bindsHandler.Add(Key.Escape, (_) => CloseChatInput());

            modAPI.Event.Add(FromBrowserEvent.CloseChat, _ => CloseChatInput());
            modAPI.Event.Add(FromBrowserEvent.ChatUsed, OnChatUsedMethod);
            modAPI.Event.Add(FromBrowserEvent.CommandUsed, OnCommandUsedMethod);
        }

        public void CloseChatInput(bool force = false)
        {
            if (!IsOpen && !force)
                return;
            IsOpen = false;
            _browserHandler.Angular.ToggleChatInput(false);
        }

        private void OnUpdate(int _)
        {
            ModAPI.Control.DisableAllControlActions(InputGroup.LOOK);
            ModAPI.Control.DisableAllControlActions(InputGroup.MOVE);
            ModAPI.Control.DisableAllControlActions(InputGroup.SUB);
        }

        private void OpenLobbyChatInput(Control _)
        {
            if (_browserHandler.InInput)
                return;

            OpenChatInput(null);
        }

#pragma warning disable IDE0051 // Remove unused private members
        private void OpenGlobalChatInput(Control _)
#pragma warning restore IDE0051 // Remove unused private members
        {
            if (_browserHandler.InInput)
                return;

            OpenChatInput("/globalsay ");
        }

        private void OpenTeamChatInput(Control _)
        {
            if (_browserHandler.InInput)
                return;

            OpenChatInput("/teamsay ");
        }

        private void OpenChatInput(string cmd)
        {
            if (IsOpen)
                return;
            IsOpen = true;

            if (cmd == null)
                _browserHandler.Angular.ToggleChatInput(true);
            else
                _browserHandler.Angular.ToggleChatInput(true, cmd);
        }

        private void OnChatUsedMethod(object[] args)
        {
            CloseChatInput();
            string msg = (string)args[0];
            int chatTypeNumber = (int)(args[1]);
            _remoteEventsSender.Send(ToServerEvent.LobbyChatMessage, msg, chatTypeNumber);
        }

        private void OnCommandUsedMethod(object[] args)
        {
            CloseChatInput();
            string msg = (string)args[0];
            if (msg == "checkshoot")
            {
                if (_lobbyHandler.Bomb.BombOnHand || !_playerFightHandler.InFight)
                    ModAPI.Chat.Output("Shooting is blocked. Reason: " + (_playerFightHandler.InFight ? "bomb" : (!_lobbyHandler.Bomb.BombOnHand ? "round" : "both")));
                else
                    ModAPI.Chat.Output("Shooting is not blocked.");
                return;
            }
            else if (msg == "activecam" || msg == "activecamera")
            {
                Logging.LogWarning((_camerasHandler.ActiveCamera?.Name ?? "No camera") + " | " + (_camerasHandler.ActiveCamera?.SpectatingEntity is null ? "no spectating" : "spectating"), "ChatHandler.Command");
                Logging.LogWarning((_camerasHandler.Spectating.IsSpectator ? "Is spectator" : "Is not spectator") + " | " + (_camerasHandler.Spectating.SpectatingEntity != null ? "spectating " + ((IPlayer)_camerasHandler.Spectating.SpectatingEntity).Name : "not spectating entity"), "ChatHandler.Command");
                Logging.LogWarning(_camerasHandler.SpectateCam.Position.ToString() + " | " + (_camerasHandler.Spectating.SpectatingEntity != null ? "spectating " + _camerasHandler.Spectating.SpectatingEntity.Position.ToString() : "not spectating entity"), "ChatHandler.Command");
            }
            _remoteEventsSender.Send(ToServerEvent.CommandUsed, msg);
        }
    }
}
