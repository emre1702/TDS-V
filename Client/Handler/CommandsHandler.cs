using System.Linq;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Handler.Deathmatch;
using TDS_Client.Handler.Events;
using TDS_Client.Handler.Lobby;
using TDS_Shared.Default;

namespace TDS_Client.Handler
{
    public class CommandsHandler : ServiceBase
    {
        #region Private Fields

        private readonly CamerasHandler _camerasHandler;
        private readonly ChatHandler _chatHandler;
        private readonly LobbyHandler _lobbyHandler;
        private readonly PlayerFightHandler _playerFightHandler;
        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly SettingsHandler _settingsHandler;

        #endregion Private Fields

        #region Public Constructors

        public CommandsHandler(IModAPI modAPI, LoggingHandler loggingHandler, ChatHandler chatHandler, SettingsHandler settingsHandler,
            LobbyHandler lobbyHandler, PlayerFightHandler playerFightHandler, CamerasHandler camerasHandler, RemoteEventsSender remoteEventsSender)
            : base(modAPI, loggingHandler)
        {
            _chatHandler = chatHandler;
            _lobbyHandler = lobbyHandler;
            _playerFightHandler = playerFightHandler;
            _camerasHandler = camerasHandler;
            _remoteEventsSender = remoteEventsSender;
            _settingsHandler = settingsHandler;

            modAPI.Event.Add(FromBrowserEvent.CommandUsed, OnCommandUsedMethod);
        }

        #endregion Public Constructors

        #region Private Methods

        private void OnCommandUsedMethod(object[] args)
        {
            _chatHandler.CloseChatInput();

            string msg = (string)args[0];
            var cmd = msg.Split(' ')[0];

            if (cmd == "checkshoot")
            {
                if (_lobbyHandler.Bomb.BombOnHand || !_playerFightHandler.InFight)
                    ModAPI.Chat.Output("Shooting is blocked. Reason: " + (_playerFightHandler.InFight ? "bomb" : (!_lobbyHandler.Bomb.BombOnHand ? "round" : "both")));
                else
                    ModAPI.Chat.Output("Shooting is not blocked.");
                return;
            }
            else if (cmd == "activecam" || cmd == "activecamera")
            {
                Logging.LogWarning((_camerasHandler.ActiveCamera?.Name ?? "No camera") + " | " + (_camerasHandler.ActiveCamera?.SpectatingEntity is null ? "no spectating" : "spectating"), "ChatHandler.Command");
                Logging.LogWarning((_camerasHandler.Spectating.IsSpectator ? "Is spectator" : "Is not spectator") + " | " + (_camerasHandler.Spectating.SpectatingEntity != null ? "spectating " + ((IPlayer)_camerasHandler.Spectating.SpectatingEntity).Name : "not spectating entity"), "ChatHandler.Command");
                Logging.LogWarning(_camerasHandler.SpectateCam.Position.ToString() + " | " + (_camerasHandler.Spectating.SpectatingEntity != null ? "spectating " + _camerasHandler.Spectating.SpectatingEntity.Position.ToString() : "not spectating entity"), "ChatHandler.Command");
                return;
            }
            else if (cmd == "campos" || cmd == "camerapos")
            {
                ModAPI.Chat.Output("Position: " + _camerasHandler.ActiveCamera.Position.ToString());
                ModAPI.Chat.Output("Rotation: " + _camerasHandler.ActiveCamera.Rotation.ToString());
                return;
            }

            var origCmdData = _settingsHandler.CommandsData.AddedCommands.FirstOrDefault(c => c.CustomCommand.Equals(cmd, System.StringComparison.OrdinalIgnoreCase));

            if (!(origCmdData is null))
            {
                var origCmd = _settingsHandler.CommandsData.InitialCommands.FirstOrDefault(c => c.Id == origCmdData.CommandId);
                if (!(origCmd is null))
                {
                    msg = origCmd.Command + (msg.Length > cmd.Length ? msg.Substring(cmd.Length) : string.Empty);
                    cmd = origCmd.Command;
                }
            }
            _remoteEventsSender.Send(ToServerEvent.CommandUsed, msg);
        }

        #endregion Private Methods
    }
}
